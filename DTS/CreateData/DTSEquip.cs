using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DTS.Protocol;

namespace DTS.CreateData
{
    public class DTSEquip
    {
        //通过equipcfg配置文件获取的信息
        public ModBusTcpServer tcpServer;     //每个同通道的tcp
        public int ChannelCount;        //一台DTS包含的通道数
        public string DTSNum;       //设备编号
        public float SampleInterval;      //设备采样间隔
        public UInt16 SlaveNum;     //设备从站号

        //通过channelcfg和alarmzonecfg配置文件获取的信息
        public List<ChannelInfos> channelInfo;    //配置页面的所有通道

        //通过CreateDatas的createTempData函数创建的数据
        public Dictionary<string, List<ChannelInfos>> Channels;    //按照通道号创建的通道数据
        private Dictionary<string, List<FiberBreakInfo>> ChannelFiberBreakInfo;    //按照通道号创建的断纤数据

        //通过RegistData的SerialChanenlInfo函数获取的数据
        public Dictionary<string, List<RegistData>> ChannelRegistData;     //按照通道号产生的寄存器数据       

        private bool _isRunning;
        private Thread CreateChannelInfos;
        private Thread CreateRegistDatas;
        private object obj;
        private object obj1;
        public DataManage dataMange;

        public DTSEquip()
        {
            channelInfo = new List<ChannelInfos>();
            Channels = new Dictionary<string, List<ChannelInfos>>();
            ChannelFiberBreakInfo = new Dictionary<string, List<FiberBreakInfo>>();
            ChannelRegistData = new Dictionary<string, List<RegistData>>();
            tcpServer = new ModBusTcpServer();
            dataMange = new DataManage();
           
            _isRunning = false;
            tcpServer.equipnum = DTSNum;
            obj = new object();
            obj1 = new object();
            
        }
        public void Start()
        {
            _isRunning = true;
            CreateChannelInfos = new Thread(CreateData);
        //    CreateRegistDatas = new Thread(DoJob);
        //    CreateRegistDatas.Start();
            CreateChannelInfos.Start();
        }

        public void Stop()
        {
            _isRunning = false;
        //    CreateRegistDatas.Abort();
            CreateChannelInfos.Abort();
        }
        private void CreateData()
        {
            while (_isRunning)
            {
                int count = channelInfo.Count;
                List<ChannelInfos> channels = new List<ChannelInfos>();
                for (int i = 0; i < count; i++)
                {
                    ChannelInfos ci = CloneChanelInfo(channelInfo[i]);
                    FiberBreakInfo fbi = CreateDatas.CreateTempData(ref ci);
                    if (fbi.FiberBreakPos != -1)
                    {
                        ci.FiberBreak = CloneFiberBreakInfo(fbi);
                      //  PushCreateDatasFBI(fbi);
                    }
                 //   PushCreateDatasCI(ci);
                    channels.Add(ci);
                }
                EquipInfo ei = new EquipInfo();
                ei = ei.DataConvert(DTSNum, channels, count, SampleInterval,this.SlaveNum);
                dataMange.PushEquipInfo(ei);

                EquipChannelTempInfo ecti = new EquipChannelTempInfo();
                ecti = ecti.DataConvert(DTSNum, channels, count, SampleInterval);
                dataMange.PushEquipChannelTempInfo(ecti);
                Thread.Sleep(1000);
            }
        }

        private void DoJob()
        {
            while (_isRunning)
            {
                if (Channels.Count > 0)
                {
                    lock (obj)
                    {
                        string[] keys = Channels.Keys.ToArray();
                        for (int i = 0; i < keys.Length; i++)
                        {
                            string key = keys[i];
                            if (Channels[key].Count > 0)
                            {
                                ChannelInfos channelinfos = CloneChanelInfo(Channels[key][0]);
                                if (channelinfos.FiberBreakStatus == 1)
                                {
                                    List<FiberBreakInfo> fiberBreakInfo;
                                    if (ChannelFiberBreakInfo.TryGetValue(key, out fiberBreakInfo))
                                    {
                                        if (fiberBreakInfo.Count > 0)
                                        {
                                            channelinfos.FiberBreak = CloneFiberBreakInfo(fiberBreakInfo[0]);
                                            lock (obj1)
                                                ChannelFiberBreakInfo[key].RemoveAt(0);
                                        }
                                    }
                                }
                                RegistData rd = RegistDatas.SerialChanenlInfo(channelinfos);
                                PushRegistDatas(rd);

                                if (this.DTSNum == Main.oldEquipNum)
                                {
                                    Main.channelCurvers[channelinfos.ChannelNum.ToString()].DrawCurver(channelinfos);
                                    Main.ObjRefreshZed(channelinfos.ChannelNum.ToString());
                                }
                                Channels[key].RemoveAt(0);
                            }
                        }
                    }
                    Thread.Sleep(10);
                }
            }
        }


        public void PushCreateDatasCI(ChannelInfos ci)
        {
            if (ci != null)
            {
                if (Channels.Keys.Contains(ci.ChannelNum.ToString()))
                {
                    lock (obj)
                        Channels[ci.ChannelNum.ToString()].Add(ci);
                }
                else
                {
                    List<ChannelInfos> temp = new List<ChannelInfos>();
                    temp.Add(ci);
                    Channels.Add(ci.ChannelNum.ToString(), temp);
                }
            }
        }

        public void PushCreateDatasFBI(FiberBreakInfo fiberbreakinfo)
        {
            if (ChannelFiberBreakInfo.Keys.Contains(fiberbreakinfo.ChannelNum.ToString()))
            {
                lock (obj1)
                    ChannelFiberBreakInfo[fiberbreakinfo.ChannelNum.ToString()].Add(fiberbreakinfo);
            }
            else
            {
                List<FiberBreakInfo> temp = new List<FiberBreakInfo>();
                temp.Add(fiberbreakinfo);
                ChannelFiberBreakInfo.Add(fiberbreakinfo.ChannelNum.ToString(), temp);
            }
        }

        public void PushRegistDatas(RegistData rd)
        {
            if (ChannelRegistData.Keys.Contains(rd.channelNum.ToString()))
            {
                // lock (obj2)
                {
                    ChannelRegistData[rd.channelNum.ToString()].Add(rd);
                }
            }
            else
            {
                List<RegistData> listregistdatas = new List<RegistData>();
                listregistdatas.Add(rd);
                ChannelRegistData.Add(rd.channelNum.ToString(), listregistdatas);
            }
        }

        public ChannelInfos CloneChanelInfo(ChannelInfos obj)
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

    }
}
