using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTS.Protocol
{
    public class DataManage
    {
        public Dictionary<string, List<EquipInfo>> EquipBaseInfos;      //0x01数据

        public Dictionary<string, List<EquipChannelTempInfo>> EquipChannelTempInfos;      //0x02数据，list是一次产生的所有通道数据

        private ChannelTempInfo channelTempInfo;


        private object objbaseinfo;
        private object objchanneltempinfo;
        private object objchannelstatusinfo;

          public DataManage()
        {
            EquipBaseInfos = new Dictionary<string, List<EquipInfo>>();
            EquipChannelTempInfos = new Dictionary<string, List<EquipChannelTempInfo>>();

            objbaseinfo = new object();
            objchanneltempinfo = new object();
            objchannelstatusinfo = new object();
        }


        public void PushEquipInfo(EquipInfo equipinfo)
        {
            if (EquipBaseInfos.Keys.Contains(equipinfo.DTSNum))
            {
                EquipBaseInfos[equipinfo.DTSNum].Add(equipinfo);
            }
            else
            {
                List<EquipInfo> t = new List<EquipInfo>();
                t.Add(equipinfo);
                EquipBaseInfos.Add(equipinfo.DTSNum, t);
            }

            int count = EquipBaseInfos[equipinfo.DTSNum].Count;
            if (count > 10 )
            {
                lock (objbaseinfo)
                    EquipBaseInfos[equipinfo.DTSNum].RemoveRange(0, count-1);
            }
        }


        public void PushEquipChannelTempInfo(EquipChannelTempInfo equipchanneltempinfo)
        {
            if (equipchanneltempinfo.EquipChannelTempInfos.Count > 0)
            {
                if (EquipChannelTempInfos.Keys.Contains(equipchanneltempinfo.DTSNum))
                {                    
                    EquipChannelTempInfos[equipchanneltempinfo.DTSNum].Add(EquipChannelTempInfo.Clone(equipchanneltempinfo));
                }
                else
                {
                    List<EquipChannelTempInfo> t = new List<EquipChannelTempInfo>();
                    EquipChannelTempInfos.Add(equipchanneltempinfo.DTSNum, t);
                    EquipChannelTempInfos[equipchanneltempinfo.DTSNum].Add(EquipChannelTempInfo.Clone(equipchanneltempinfo));
                }

                int count = EquipChannelTempInfos[equipchanneltempinfo.DTSNum].Count;
                if (count > 10)
                {
                    lock (objchanneltempinfo)
                        EquipChannelTempInfos[equipchanneltempinfo.DTSNum].RemoveRange(0, count-1);
                }
            }                
        }       

        public object PopData(string equipnum,ushort slavenum)
        {
            object data = null;
            if(EquipBaseInfos.Keys.Contains(equipnum))
            {
                int count = EquipBaseInfos[equipnum].Count;
                if (count > 0)
                {
                    int equipslavenum = EquipBaseInfos[equipnum][count - 1].SlaveNum;
                    int channelnum = Math.Abs(slavenum - equipslavenum);
                    if (channelnum == 0)
                    {
                        //取基本信息
                    }
                    else
                    {
                        //取通道温度数据
                        if (EquipChannelTempInfos.Keys.Contains(equipnum))
                        {
                            try
                            {
                                List<EquipChannelTempInfo> listecti = EquipChannelTempInfos[equipnum].ToList();
                                int channelcount = listecti.Count;
                                if (channelcount > 0)
                                {
                                    int index = listecti[channelcount - 1].EquipChannelTempInfos.FindIndex(item => item.ChannelNum == channelnum);
                                    ChannelTempInfo cti = ChannelTempInfo.Clone(listecti[channelcount - 1].EquipChannelTempInfos.Find(delegate (ChannelTempInfo c) { return c.ChannelNum == channelnum; }));
                                    if (index != -1)
                                    {
                                        channelTempInfo = ChannelTempInfo.Clone(cti);
                                    }
                                    else
                                    {
                                        if (channelTempInfo == null)
                                        {
                                            ChannelTempInfo cti1 = new ChannelTempInfo();
                                            cti1.DTSNum = equipnum;
                                            cti1.ChannelNum = (ushort)channelnum;
                                            cti1.SampleInterval = listecti[count - 1].SampleInterval;
                                            cti1.ChannelFiberLen = 0;
                                            channelTempInfo = cti1;
                                        }
                                    }
                                }
                                else
                                {
                                    if (channelTempInfo == null)
                                    {
                                        ChannelTempInfo cti1 = new ChannelTempInfo();
                                        cti1.DTSNum = equipnum;
                                        cti1.ChannelNum = (ushort)channelnum;
                                        cti1.SampleInterval = listecti[count - 1].SampleInterval;
                                        cti1.ChannelFiberLen = 0;
                                        channelTempInfo = cti1;
                                    }
                                }
                                data = channelTempInfo;
                            }
                            catch (Exception ex) { }
                        }
                    }
                }
            }
            return data;
        }





    }
}
