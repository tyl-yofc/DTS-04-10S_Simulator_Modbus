using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Diagnostics;

namespace DTS.CreateData
{
    class CreateInputRegistData     //一个设备的所有通道
    {
        Dictionary<string, List<ChannelInfos>> waitSend;   //通道的分区信息、温度数据、报警数据
        private object obj;
        private object obj1;
        Dictionary<string, List<FiberBreakInfo>> fiberBreakInfos;    //通道的断纤信息


        public Dictionary<string, List<ChannelInfos>> newwaitSend;

        public CreateInputRegistData()
        {
            waitSend = new Dictionary<string, List<ChannelInfos>>();
            fiberBreakInfos = new Dictionary<string, List<FiberBreakInfo>>();
            newwaitSend = new Dictionary<string, List<ChannelInfos>>();
            obj = new object();
            obj1 = new object();
            Thread t = new Thread(DoJob);
            t.Start();           
        }
        public void PushBaseInfo(ChannelInfos channelInfo)
        {
            if(channelInfo != null)
            {
                if (waitSend.Keys.Contains(channelInfo.ChannelNum.ToString()))
                {
                    lock (obj)
                        waitSend[channelInfo.ChannelNum.ToString()].Add(CloneChanelInfo(channelInfo));
                }
                else
                {
                    List<ChannelInfos> temp = new List<ChannelInfos>();
                    temp.Add(CloneChanelInfo(channelInfo));
                    waitSend.Add(channelInfo.ChannelNum.ToString(), temp);
                }

             //   Trace.WriteLine("CreateInputRegistData:" + waitSend["1"].Count);
            }           
        }
        public void PushFiberBreakInfo(FiberBreakInfo fiberbreakinfo)
        {
            if (fiberBreakInfos.Keys.Contains(fiberbreakinfo.ChannelNum.ToString()))
            {
                lock(obj1)
                    fiberBreakInfos[fiberbreakinfo.ChannelNum.ToString()].Add(fiberbreakinfo);
            }
            else
            {
                List<FiberBreakInfo> temp = new List<FiberBreakInfo>();
                temp.Add(fiberbreakinfo);
                fiberBreakInfos.Add(fiberbreakinfo.ChannelNum.ToString(), temp);
            }
        }
        public static ChannelInfos CloneChanelInfo( ChannelInfos obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, obj);
            memoryStream.Position = 0;
            return (ChannelInfos)formatter.Deserialize(memoryStream);
        }

        private FiberBreakInfo CloneFiberBreakInfo(FiberBreakInfo obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, obj);
            memoryStream.Position = 0;
            return (FiberBreakInfo)formatter.Deserialize(memoryStream);
        }
        private void DoJob()
        {
            while(true)
            {
                if(waitSend.Count > 0)
                {
                    lock (obj)
                    {
                        string[] keys = waitSend.Keys.ToArray();
                        for(int i=0;i<keys.Length;i++)
                        {
                            string key = keys[i];
                            if (waitSend[key].Count > 0)
                            {
                                ChannelInfos channelinfos = CloneChanelInfo(waitSend[key][0]);
                                if (channelinfos.FiberBreakStatus == 1)
                                {
                                    List<FiberBreakInfo> fiberBreakInfo;
                                    if (fiberBreakInfos.TryGetValue(key, out fiberBreakInfo))
                                    {
                                        if (fiberBreakInfo.Count > 0)
                                        {
                                            channelinfos.FiberBreak = CloneFiberBreakInfo(fiberBreakInfo[0]);
                                            lock (obj1)
                                                fiberBreakInfos[key].RemoveAt(0);
                                        }
                                    }
                                }
                                //    RegistDatas.Create().PushChannelInfos(channelinfos);
                                if (newwaitSend.Keys.Contains(key))
                                    newwaitSend[key].Add(channelinfos);
                                else
                                {
                                    List<ChannelInfos> c = new List<ChannelInfos>();
                                    c.Add(channelinfos);
                                    newwaitSend.Add(key, c);
                                }

                                waitSend[key].RemoveAt(0);
                            }
                        }                        
                    }
                    Thread.Sleep(10);
                }
            }
        }      
    }
}
