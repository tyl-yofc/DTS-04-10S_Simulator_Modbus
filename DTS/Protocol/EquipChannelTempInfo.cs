using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DTS.Protocol
{
    [Serializable]
    /// <summary>
    /// 0x02
    /// </summary>
    /// 
    public class EquipChannelTempInfo
    {
        public string DTSNum;     //设备编号
        public float SampleInterval;
        public List<ChannelTempInfo> EquipChannelTempInfos;

     //   public Dictionary<string, List<ChannelTempInfo>> ChannelTempInfos;      //key为通道号

        public EquipChannelTempInfo()
        {
            EquipChannelTempInfos = new List<ChannelTempInfo>();
        }
        public EquipChannelTempInfo DataConvert(string equipnum, List<ChannelInfos> channelInfo,int channelcount, float SampleInterval)
        {
            this.DTSNum = equipnum;
            this.SampleInterval = SampleInterval;
            for (int i = 0; i < channelcount; i++)
            {
                ChannelTempInfo equipchanneltempinfo = new ChannelTempInfo();
                equipchanneltempinfo.DTSNum = equipnum;
                equipchanneltempinfo.ChannelNum = channelInfo[i].ChannelNum;
                equipchanneltempinfo.SampleInterval = SampleInterval;
                equipchanneltempinfo.ChannelFiberLen = Convert.ToUInt16(channelInfo[i].FiberLen);
                equipchanneltempinfo.FiberBreakStatus = channelInfo[i].FiberBreakStatus;
                equipchanneltempinfo.ChannelTempCount = (ushort)(channelInfo[i].TempDatas.Count);
                equipchanneltempinfo.ChannelTemps = new List<double>();
                for (int j = 0; j < channelInfo[i].TempDatas.Count; j++)
                {
                    equipchanneltempinfo.ChannelTemps.Add(channelInfo[i].TempDatas[j]);
                }
                
                EquipChannelTempInfos.Add(equipchanneltempinfo);
            }
            return this;
        }

        public static byte[] ProtocolConvert(ChannelTempInfo t)
        {
            try
            {
                ChannelTempInfo channeltempinfo = ChannelTempInfo.Clone(t);
                int framecount = channeltempinfo.ChannelTemps.Count / 256;
                if (channeltempinfo.ChannelTemps.Count % 256 != 0)
                    framecount += 1;
                byte[] data = new byte[65536*2];    //数据
                for (int j = 0; j < data.Length; j++)
                    data[j] = 0xff;
                byte[] temp = BitConverter.GetBytes(channeltempinfo.ChannelFiberLen);
                data[2] = temp[1];
                data[3] = temp[0];
                temp = BitConverter.GetBytes(channeltempinfo.ChannelTempCount);
                data[4] = temp[1];
                data[5] = temp[0];
                temp = BitConverter.GetBytes((ushort)(channeltempinfo.SampleInterval*ChannelInfos.SampleRate));
                data[6] = temp[1];
                data[7] = temp[0];
                temp = BitConverter.GetBytes(channeltempinfo.FiberBreakStatus);
                data[8] = temp[1];
                data[9] = temp[0];
                for(int i=0;i< channeltempinfo.ChannelTemps.Count;i++)
                {
                    temp = BitConverter.GetBytes((ushort)(channeltempinfo.ChannelTemps[i]*10));
                    data[22 + i * 2] = temp[1];
                    data[22 + i * 2 + 1] = temp[0];
                }
                return data;
            }catch(Exception ex)
            {
                throw;
            }
        }

        public static EquipChannelTempInfo Clone(EquipChannelTempInfo ecsi)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, ecsi);
            memoryStream.Position = 0;
            return (EquipChannelTempInfo)formatter.Deserialize(memoryStream);
        }
    }

    [Serializable]
    public class ChannelTempInfo
    {
        public string DTSNum;     //设备编号
        public ushort ChannelNum;   //通道号
        public float SampleInterval;    //通道采样间隔
        public UInt16 ChannelFiberLen;    //通道光纤长度
        public UInt16 ChannelTempCount;    //通道温度点数
        public List<double> ChannelTemps;    //通道温度数据

        public ushort FiberBreakStatus;      //通道断纤告警状态,0:正常，1：断纤

        public ChannelTempInfo()
        {
            ChannelTemps = new List<double>();
            FiberBreakStatus = 0;
        }
        public static ChannelTempInfo Clone(ChannelTempInfo cti)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, cti);
            memoryStream.Position = 0;
            return (ChannelTempInfo)formatter.Deserialize(memoryStream);
        }
    }


}
