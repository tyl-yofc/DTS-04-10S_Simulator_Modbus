using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DTS.CreateData
{
    class CreateEquipDatas
    {
        private Dictionary<string, DTSEquip> Equip;

        private static Dictionary<DTSEquip, List<ChannelInfos>> EquipCreateDatasCI;
        private static Dictionary<DTSEquip, List<FiberBreakInfo>> EquipCreateDatasFBI;

        public static Dictionary<DTSEquip, List<RegistData>> RegistDatas;
        public bool _isRunning;
        private static object obj;
        private static object obj1;
        private static object obj2;
        Thread t;
        Thread t1;
       
        private static CreateEquipDatas _instance;

        public static CreateEquipDatas Create(Dictionary<string, DTSEquip> equip)
        {
            return _instance ?? (_instance = new CreateEquipDatas(equip));
        }

        public CreateEquipDatas(Dictionary<string, DTSEquip> equip)
        {
            Equip = equip;

            EquipCreateDatasCI = new Dictionary<DTSEquip, List<ChannelInfos>>();
            EquipCreateDatasFBI = new Dictionary<DTSEquip, List<FiberBreakInfo>>();           
            RegistDatas = new Dictionary<DTSEquip, List<RegistData>>();
            _isRunning = false;
            t = new Thread(DoJob);
        }
        /*
        public void CreateData()
        {
            EquipCreateDatasCI.Clear();
            EquipCreateDatasFBI.Clear();
            RegistDatas.Clear();
            foreach (KeyValuePair<string, DTSEquip> kvp in Equip)
            {
                kvp.Value.tcpServer.Start();
                List<ChannelInfos> channels = kvp.Value.channelInfo;
                if (channels.Count > 0)
                {
                    for (int i = 0; i < channels.Count; i++)
                    {
                        CreateDatas cd = new CreateDatas(kvp.Value,channels[i]);
                        int index = -1;
                        if (EquipCreateDatas.Keys.Contains(kvp.Key))
                        {
                            EquipCreateDatas[kvp.Key].Add(cd);
                            index = EquipCreateDatas[kvp.Key].Count - 1;
                        }
                        else
                        {
                            List<CreateDatas> list = new List<CreateDatas>();
                            list.Add(cd);
                            EquipCreateDatas.Add(kvp.Key, list);
                            index = 0;
                        }
                        EquipCreateDatas[kvp.Key][index].timer.Start();
                        _isRunning = true;
                    }
                }
            }

            //    t.Start();
            //     t1.Start();
        }
        */

        public static void PushCreateDatasCI(DTSEquip equip,ChannelInfos ci)
        {
            if (ci != null)
            {
                if (EquipCreateDatasCI.Keys.Contains(equip))
                {
                    lock (obj)
                        EquipCreateDatasCI[equip].Add(ci);
                }
                else
                {
                    List<ChannelInfos> temp = new List<ChannelInfos>();
                    temp.Add(ci);
                    EquipCreateDatasCI.Add(equip, temp);
                }

                //   Trace.WriteLine("CreateInputRegistData:" + waitSend["1"].Count);
            }
        }

        public static void PushCreateDatasFBI(DTSEquip equip,FiberBreakInfo fiberbreakinfo)
        {
            if (EquipCreateDatasFBI.Keys.Contains(equip))
            {
                lock (obj1)
                    EquipCreateDatasFBI[equip].Add(fiberbreakinfo);
            }
            else
            {
                List<FiberBreakInfo> temp = new List<FiberBreakInfo>();
                temp.Add(fiberbreakinfo);
                EquipCreateDatasFBI.Add(equip, temp);
            }
        }

        public static void PushRegistDatas(DTSEquip equip, RegistData rd)
        {
            if (RegistDatas.Keys.Contains(equip))
            {
                lock (obj2)
                {
                    RegistDatas[equip].Add(rd);
                }
            }
            else
            {
                List<RegistData> listregistdatas = new List<RegistData>();
                listregistdatas.Add(rd);
                RegistDatas.Add(equip, listregistdatas);
            }
        }
        
        public static ChannelInfos CloneChanelInfo(ChannelInfos obj)
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
            while(_isRunning)
            {
                if (EquipCreateDatasCI.Count > 0)
                {
                    lock (obj)
                    {
                        DTSEquip[] keys = EquipCreateDatasCI.Keys.ToArray();
                        for (int i = 0; i < keys.Length; i++)
                        {
                            DTSEquip key = keys[i];
                            if (EquipCreateDatasCI[key].Count > 0)
                            {
                                ChannelInfos channelinfos = CloneChanelInfo(EquipCreateDatasCI[key][0]);
                                if (channelinfos.FiberBreakStatus == 1)
                                {
                                    List<FiberBreakInfo> fiberBreakInfo;
                                    if (EquipCreateDatasFBI.TryGetValue(key, out fiberBreakInfo))
                                    {
                                        if (fiberBreakInfo.Count > 0)
                                        {
                                            channelinfos.FiberBreak = CloneFiberBreakInfo(fiberBreakInfo[0]);
                                            lock (obj1)
                                                EquipCreateDatasFBI[key].RemoveAt(0);
                                        }
                                    }
                                }
                                //     InputRigisDatas.Create().PushChannelInfos(channelinfos);
                                RegistData rd = DTS.CreateData.RegistDatas.SerialChanenlInfo( channelinfos);
                                PushRegistDatas(key, rd);

                                EquipCreateDatasCI[key].RemoveAt(0);
                            }
                        }
                    }
                    Thread.Sleep(10);
                }
            }
        }

      


        
    }
}
