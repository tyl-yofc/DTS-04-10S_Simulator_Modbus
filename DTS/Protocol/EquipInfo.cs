using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTS.Protocol
{
    public class EquipInfo
    {
        public UInt16 GlobalAlarm;
        public UInt16 CollectStatus;
        public UInt16 ChannelCount;
        public UInt16 PointDistance;
        public UInt16 FiberBreak16;   //1-16
        public UInt16 FiberBreak32;   //17-32
        public UInt16 SlaveNum;      //设备从站号
        public List<Zone> EquipZones;

        public string DTSNum;
        public EquipInfo()
        {
            EquipZones = new List<Zone>();
        }

        public EquipInfo DataConvert(string equipnum, List<ChannelInfos> channelInfo, int channelcount, float SampleInterval,ushort equipslavenum)
        {
            this.SlaveNum = equipslavenum;
            this.DTSNum = equipnum;
            this.ChannelCount = (UInt16)channelcount;
            this.PointDistance = (ushort)(SampleInterval * ChannelInfos.SampleRate);
            channelInfo.Sort((ChannelInfos ci1, ChannelInfos ci2) => ci1.ChannelNum.CompareTo(ci2.ChannelNum));
            for (int i = 0; i < channelcount; i++)
            {
                int zonecount = channelInfo[i].ZoneCount;
                channelInfo[i].ZoneTempInfos.Sort((ZoneTempInfo zti1, ZoneTempInfo zti2) => zti1.ZoneNumber.CompareTo(zti2.ZoneNumber));
                for(int j=0;j<zonecount;j++)
                {
                    Zone zone = new Zone();
                    zone.ZoneID = channelInfo[i].ZoneTempInfos[j].ZoneNumber;
                    zone.ZoneChannelID = channelInfo[i].ChannelNum;
                    zone.StartDistance = (ushort)(channelInfo[i].ZoneTempInfos[j].StartPos/channelInfo[i].SampleInterval);
                    zone.EndDistance = (ushort)(channelInfo[i].ZoneTempInfos[j].StopPos / channelInfo[i].SampleInterval);
                    zone.AvgTemp = (ushort)(channelInfo[i].ZoneTempInfos[j].AvgTemp);
                    zone.MinTemp = (ushort)(channelInfo[i].ZoneTempInfos[j].LowestTemp);
                    zone.MaxTemp = (ushort)(channelInfo[i].ZoneTempInfos[j].HigestTemp);
                    zone.AlarmStatus = 0;
                    if (channelInfo[i].ZoneTempInfos[j].TempRiseFlag)
                        zone.AlarmStatus = (ushort)(zone.AlarmStatus | 1);
                    if (channelInfo[i].ZoneTempInfos[j].ConsTempFlag)
                        zone.AlarmStatus = (ushort)(zone.AlarmStatus | 2);
                    if (channelInfo[i].ZoneTempInfos[j].FiberBreakFlag)
                        zone.AlarmStatus = (ushort)(zone.AlarmStatus | 8);
                    EquipZones.Add(zone);
                }
            }
            return this;
        }

    }

    public class Zone
    {
        public UInt16 ZoneID;
        public UInt16 ZoneChannelID;
        public UInt16 StartDistance;
        public UInt16 EndDistance;
        public UInt16 AvgTemp;
        public UInt16 MinTemp;
        public UInt16 MaxTemp;
        public UInt16 AlarmStatus;
        public UInt16 AlarmPos;
    }
}
