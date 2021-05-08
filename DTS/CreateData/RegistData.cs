using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using DTS.Helpers;

namespace DTS.CreateData
{
    class RegistDatas
    {
        public Dictionary<string,List<ChannelInfos>> channelInfos;

        public Dictionary<string, List<RegistData>> waitSendRegistDatas;        
        private object obj;
        private object obj1;

        public RegistDatas()
        {
            channelInfos = new Dictionary<string, List<ChannelInfos>>();
            waitSendRegistDatas = new Dictionary<string, List<RegistData>>();
            obj = new object();
            obj1 = new object();
            Thread t = new Thread(DoJob);
            t.Start();
        }
        private void DoJob()
        {
            while(true)
            {
                if(channelInfos.Count > 0)
                {
                    string[] keys = channelInfos.Keys.ToArray();
                    for (int i = 0; i < keys.Length; i++)
                    {
                        lock (obj)
                        {
                            if (channelInfos[keys[i]].Count > 0)
                            {
                                SerialChanenlInfo(channelInfos[keys[i]][0]);
                                channelInfos[keys[i]].RemoveAt(0);
                            }
                        }
                    }
                    Thread.Sleep(10);
                }
            }
        }
        public void PushChannelInfos(ChannelInfos channelinfo)
        {
            if(channelInfos.Keys.Contains(channelinfo.ChannelNum.ToString()))
            {
                lock(obj)
                {
                    channelInfos[channelinfo.ChannelNum.ToString()].Add(channelinfo);
                }
            }
            else
            {
                List<ChannelInfos> listchannelinfos = new List<ChannelInfos>();
                listchannelinfos.Add(channelinfo);
                channelInfos.Add(channelinfo.ChannelNum.ToString(), listchannelinfos);
            }
        }

        public static RegistData SerialChanenlInfo(ChannelInfos channelinfo)
        {            
            MemoryStream stream = new MemoryStream();
            BinaryWriter bf = new BinaryWriter(stream);

            //通道基本信息
            byte[] baseinfo = new byte[100];
            for(int i=0;i< baseinfo.Length;i++)            
                baseinfo[i] = 0xff;

            bf.Write(channelinfo.ChannelNum.ByteChange());
            bf.Write(channelinfo.ZoneCount.ByteChange());
            bf.Write(channelinfo.FiberBreakStatus.ByteChange());
            bf.Write(channelinfo.ConsTempAlarmCount.ByteChange());
            bf.Write(channelinfo.TempRiseAlarmCount.ByteChange());
            bf.Write(channelinfo.RegionTempDifAlarmCount.ByteChange());
            byte[] b1 = stream.ToArray();    
            Array.Copy(stream.ToArray(), 0, baseinfo, 10, b1.Length);
            stream.Position = 0;

            //通道采集信息
            byte[] collectinfo = new byte[100];
            for (int i = 0; i < collectinfo.Length; i++)            
                collectinfo[i] = 0xff;
            bf.Write(((ushort)(channelinfo.SampleInterval*ChannelInfos.SampleRate)).ByteChange());
            bf.Write(channelinfo.TempCount.ByteChange());
            bf.Write(channelinfo.FreshTime_YearMonth.ByteChange());
            bf.Write(channelinfo.FreshTime_DayHour.ByteChange());
            bf.Write(channelinfo.FreshTime_MinSec.ByteChange());
            b1 = stream.ToArray();
            Array.Copy(b1, 0, collectinfo, 0, 10);
            stream.Position = 0;

            //通道断纤信息
            byte[] fiberbreakinfo = new byte[200];
            for (int i = 0; i < fiberbreakinfo.Length; i++)            
                fiberbreakinfo[i] = 0xff;            
            if (channelinfo.FiberBreakStatus == 1)
            {
                bf.Write(channelinfo.FiberBreak_Pos.ByteChange());
                bf.Write(channelinfo.FiberBreak_YearMonth.ByteChange());
                bf.Write(channelinfo.FiberBreak_DayHour.ByteChange());
                bf.Write(channelinfo.FiberBreak_MinSec.ByteChange());
                b1 = stream.ToArray();
                Array.Copy(b1, 0, fiberbreakinfo, 0, 8);
                stream.Position = 0;
            }

            //设备故障信息
            byte[] faultinfo = new byte[1600];
            for (int i = 0; i < faultinfo.Length; i++)            
                faultinfo[i] = 0xff;
            bf.Write(channelinfo.DTSFault.CommunicateFault.ByteChange());
            bf.Write(channelinfo.DTSFault.MainPowerFault.ByteChange());
            bf.Write(channelinfo.DTSFault.StandbyPowerFault.ByteChange());
            bf.Write(channelinfo.DTSFault.ChargeFault.ByteChange());
            b1 = stream.ToArray();
            Array.Copy(b1, 0, faultinfo, 0, 8);
            stream.Position = 0;
            
            //分区温度信息
            byte[] zonetempinfos = new byte[18000];
            for (int i = 0; i < zonetempinfos.Length; i++)            
                zonetempinfos[i] = 0xff;
            
            List<byte> listzonetempinfo = new List<byte>();
            for(int i=0;i<channelinfo.ZoneCount;i++)
            {
                byte[] zonetempinfo = new byte[20];
                for (int j = 0; j < zonetempinfo.Length; j++)
                    zonetempinfo[j] = 0xff;
                bf.Write(channelinfo.ZoneTempInfos[i].TempAlarmStatus.ByteChange());
                bf.Write(((ushort)channelinfo.ZoneTempInfos[i].HigestTemp).ByteChange());
                bf.Write(((ushort)channelinfo.ZoneTempInfos[i].AvgTemp).ByteChange());
                bf.Write(((ushort)channelinfo.ZoneTempInfos[i].LowestTemp).ByteChange());
                bf.Write(((ushort)channelinfo.ZoneTempInfos[i].HigestTempPos).ByteChange());
                bf.Write(((ushort)channelinfo.ZoneTempInfos[i].LowestTempPos).ByteChange());
                b1 = stream.ToArray();
                Array.Copy(b1, 0, zonetempinfo, 0, 12);
                stream.Position = 0;

                listzonetempinfo.AddRange(zonetempinfo);
            }
            listzonetempinfo.ToArray().CopyTo(zonetempinfos, 0);           

            //定温报警
            byte[] consttempalarm = new byte[4000];
            for (int i = 0; i < consttempalarm.Length; i++)            
                consttempalarm[i] = 0xff;
            
            List<byte> listconsttempalarm = new List<byte>();
            for (int i=0;i<channelinfo.ConsTempAlarms.Count;i++)
            {
                byte[] consttempalarminfo = new byte[20];
                for (int j = 0; j < consttempalarminfo.Length; j++)
                    consttempalarminfo[j] = 0xff;
                bf.Write(((ushort)channelinfo.ConsTempAlarms[i].AlarmStartPos).ByteChange());
                bf.Write(((ushort)channelinfo.ConsTempAlarms[i].AlarmStopPos).ByteChange());
                bf.Write(((ushort)channelinfo.ConsTempAlarms[i].Alarm_YearMonth).ByteChange());
                bf.Write(((ushort)channelinfo.ConsTempAlarms[i].Alarm_DayHour).ByteChange());
                bf.Write(((ushort)channelinfo.ConsTempAlarms[i].Alarm_MinSec).ByteChange());
                bf.Write(((ushort)channelinfo.ConsTempAlarms[i].AlarmZoneNum).ByteChange());
                b1 = stream.ToArray();
                Array.Copy(b1, 0, consttempalarminfo, 0, 12);
                stream.Position = 0;

                listconsttempalarm.AddRange(consttempalarminfo);
            }
            listconsttempalarm.ToArray().CopyTo(consttempalarm, 0);

            //温升报警
            byte[] temprisealarm = new byte[4000];
            for (int i = 0; i < temprisealarm.Length; i++)            
                temprisealarm[i] = 0xff;
            
            List<byte> listtemprisealarm = new List<byte>();
            for (int i = 0; i < channelinfo.TempRiseAlarms.Count; i++)
            {
                byte[] temprisealarminfo = new byte[20];
                for (int j = 0; j < temprisealarminfo.Length; j++)
                    temprisealarminfo[j] = 0xff;
                bf.Write(((ushort)channelinfo.TempRiseAlarms[i].AlarmStartPos).ByteChange());
                bf.Write(((ushort)channelinfo.TempRiseAlarms[i].AlarmStopPos).ByteChange());
                bf.Write(((ushort)channelinfo.TempRiseAlarms[i].Alarm_YearMonth).ByteChange());
                bf.Write(((ushort)channelinfo.TempRiseAlarms[i].Alarm_DayHour).ByteChange());
                bf.Write(((ushort)channelinfo.TempRiseAlarms[i].Alarm_MinSec).ByteChange());
                bf.Write(((ushort)channelinfo.TempRiseAlarms[i].AlarmZoneNum).ByteChange());
                b1 = stream.ToArray();
                Array.Copy(b1, 0, temprisealarminfo, 0, 12);
                stream.Position = 0;

                listtemprisealarm.AddRange(temprisealarminfo);
            }
            listtemprisealarm.ToArray().CopyTo(temprisealarm, 0);

            //区域温差报警
            byte[] tempdifalarm = new byte[12000];
            for (int i = 0; i < tempdifalarm.Length; i++)            
                tempdifalarm[i] = 0xff;            
            List<byte> listtempdifalarm = new List<byte>();
            for (int i = 0; i < channelinfo.RegionTempDifAlarms.Count; i++)
            {
                byte[] tempdifalarminfo = new byte[20];
                for (int j = 0; j < tempdifalarm.Length; j++)
                    tempdifalarm[j] = 0xff;
                bf.Write(((ushort)channelinfo.RegionTempDifAlarms[i].AlarmStartPos).ByteChange());
                bf.Write(((ushort)channelinfo.RegionTempDifAlarms[i].AlarmStopPos).ByteChange());
                bf.Write(((ushort)channelinfo.RegionTempDifAlarms[i].Alarm_YearMonth).ByteChange());
                bf.Write(((ushort)channelinfo.RegionTempDifAlarms[i].Alarm_DayHour).ByteChange());
                bf.Write(((ushort)channelinfo.RegionTempDifAlarms[i].Alarm_MinSec).ByteChange());
                bf.Write(((ushort)channelinfo.RegionTempDifAlarms[i].AlarmZoneNum).ByteChange());
                b1 = stream.ToArray();
                Array.Copy(b1, 0, tempdifalarminfo, 0, 12);
                stream.Position = 0;

                listtempdifalarm.AddRange(tempdifalarminfo);
            }
            listtempdifalarm.ToArray().CopyTo(tempdifalarm, 0);

            //温度数据
            byte[] tempdata = new byte[channelinfo.TempCount*2];
            for (int i = 0; i < tempdata.Length; i++)            
                tempdata[i] = 0xff;
            
            for (int i=0;i<channelinfo.TempCount;i++)
            {
                bf.Write(((ushort)(channelinfo.TempDatas[i]*10)).ByteChange());
            }
            stream.ToArray().CopyTo(tempdata, 0);
            stream.Position = 0;

            List<byte> inputregistdata = new List<byte>();    //输入寄存器
            inputregistdata.AddRange(baseinfo);
            inputregistdata.AddRange(collectinfo);
            inputregistdata.AddRange(fiberbreakinfo);
            inputregistdata.AddRange(faultinfo);
            inputregistdata.AddRange(zonetempinfos);
            byte[] b = new byte[8000];
            for (int i = 0; i < b.Length; i++)
                b[i] = 0xff;
            inputregistdata.AddRange(consttempalarm);
            inputregistdata.AddRange(temprisealarm);
            inputregistdata.AddRange(tempdifalarm);
            inputregistdata.AddRange(tempdata);
            
            //保持寄存器数据
            List<byte> holdregistdata = new List<byte>();
            //设备控制
            byte[] equipcontrol = new byte[102];
            for (int i = 0; i < equipcontrol.Length; i++)
            {
                equipcontrol[i] = 0xff;
            }
            stream.Position = 0;

            //通道配置信息
            byte[] cfginfo = new byte[1898];
            for (int i = 0; i < cfginfo.Length; i++)
            {
                cfginfo[i] = 0xff;
            }
            BitConverter.GetBytes((ushort)(channelinfo.MeasureTime * 10)).CopyTo(cfginfo, 0);

            //分区配置信息
            byte[] zonecfginfos = new byte[channelinfo.ZoneCount * 10 * 2];
            for (int i = 0; i < zonecfginfos.Length; i++)
            {
                zonecfginfos[i] = 0xff;
            }
            for (int i=0;i<channelinfo.ZoneCount;i++)
            {
                byte[] zonecfginfo = new byte[20];
                for (int j = 0; j < zonecfginfo.Length; j++)
                    zonecfginfo[j] = 0xff;

                bf.Write(((ushort)channelinfo.ZoneTempInfos[i].StartPos).ByteChange());
                bf.Write(((ushort)channelinfo.ZoneTempInfos[i].StopPos).ByteChange());
                bf.Write(((ushort)channelinfo.ZoneTempInfos[i].ConsTempThreshold).ByteChange());
                bf.Write(((ushort)channelinfo.ZoneTempInfos[i].TempRiseThreshold).ByteChange());
                bf.Write(((ushort)channelinfo.ZoneTempInfos[i].RegionTempDifThreshold).ByteChange());
                b1 = stream.ToArray();
                Array.Copy(b1, 0, zonecfginfo, 0, 10);
                stream.Position = 0;

                Array.Copy(zonecfginfo, 0, zonecfginfos, i * 20, 20);
            }            
            holdregistdata.AddRange(equipcontrol);
            holdregistdata.AddRange(cfginfo);
            holdregistdata.AddRange(zonecfginfos);

            bf.Close();
            stream.Close();
            RegistData registdata = new RegistData(channelinfo.ChannelNum);
            registdata.holdRegistData = holdregistdata.ToArray();
            registdata.inputRegistData = inputregistdata.ToArray();
            return registdata;
        }

        public  void PushRegistData(RegistData registdata)
        {
            if(waitSendRegistDatas!= null)
            {
                if(waitSendRegistDatas.Keys.Contains(registdata.channelNum.ToString()))
                {
                    lock(obj1)
                        waitSendRegistDatas[registdata.channelNum.ToString()].Add(registdata);
                }
                else
                {
                    List<RegistData> listregistdata = new List<RegistData>();
                    listregistdata.Add(registdata);
                    waitSendRegistDatas.Add(registdata.channelNum.ToString(), listregistdata);
                }
            }
        }

        public RegistData PosRegistdata(string channelnum)
        {
            RegistData registdata = null;
            if(waitSendRegistDatas!= null)
            {
                if(waitSendRegistDatas.Keys.Contains(channelnum))
                {
                    lock (obj1)
                    {
                        int count = waitSendRegistDatas[channelnum].Count;
                        if (count > 0)
                        {
                            registdata = waitSendRegistDatas[channelnum][count - 1];

                           // waitSendRegistDatas[channelnum].RemoveAt(0);
                        }

                        if(count > 50)
                        {
                            waitSendRegistDatas[channelnum].RemoveRange(0, waitSendRegistDatas[channelnum].Count - 1);
                        }
                    }                    
                }
            }
            return registdata;
        }
    }


    public class RegistData
    {
        public int channelNum;    //通道号

        public byte[] holdRegistData;   //保持寄存器数据

        public byte[] inputRegistData;   //输入寄存器数据

        public RegistData(int channelnum)
        {
            this.channelNum = channelnum;

            holdRegistData = new byte[12000];

            inputRegistData = new byte[131072];
        }
    }


}
