using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using DTS.CreateData;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DTS
{
    [Serializable]
    public class ChannelInfos
    {
        public UInt16 SlaveNum;      //通道从站号，设备从站号+通道号
        public UInt16 ChannelNum { get; set; }          //通道号
        public UInt16 ZoneCount { get; set; }        //通道分区数

        public float FiberLen;         //光纤长度

        public UInt16 MeasureTime;         //测量时间   

        public float BaseTemp;         //用户设置每个通道通道的参考温度

        public UInt16 FiberBreakStatus { get; set; }          //断纤报警状态。0：正常，1：断纤
        public UInt16 ConsTempAlarmCount { get; set; }      //定温报警个数
        public UInt16 TempRiseAlarmCount { get; set; }        //温升报警个数
        public UInt16 RegionTempDifAlarmCount { get; set; }      //区域温差报警个数
        public float SampleInterval { get; set; }         //采样间隔
        public static int SampleRate = 100000;

        public UInt16 TempCount { get; set; }          //温度有效点数
        public UInt16 FreshTime_YearMonth
        {
            get
            {
                int year = FreshTime.Year - 2000;
                int month = FreshTime.Month;
                byte[] b = new byte[2];
                b[0] = BitConverter.GetBytes(year)[0];
                b[1] = BitConverter.GetBytes(month)[0];
                return BitConverter.ToUInt16(b, 0);
            }
        }
        public UInt16 FreshTime_DayHour
        {
            get
            {
                int day = FreshTime.Day;
                int hour = FreshTime.Hour;
                byte[] b = new byte[2];
                b[0] = BitConverter.GetBytes(day)[0];
                b[1] = BitConverter.GetBytes(hour)[0];
                return BitConverter.ToUInt16(b, 0);
            }
        }
        public UInt16 FreshTime_MinSec
        {
            get
            {
                int minute = FreshTime.Minute;
                int second = FreshTime.Second;
                byte[] b = new byte[2];
                b[0] = BitConverter.GetBytes(minute)[0];
                b[1] = BitConverter.GetBytes(second)[0];
                return BitConverter.ToUInt16(b, 0);
            }
        }
        public UInt16 FiberBreak_Pos
        {
            get
            {
                return (UInt16)FiberBreak.FiberBreakPos;
            }
        }
        public UInt16 FiberBreak_YearMonth
        {
            get
            {
                int year = FiberBreak.FiberBreakTmie.Year - 2000;
                int month = FiberBreak.FiberBreakTmie.Month;
                byte[] b = new byte[2];
                b[0] = BitConverter.GetBytes(year)[0];
                b[1] = BitConverter.GetBytes(month)[0];
                return BitConverter.ToUInt16(b, 0);
            }
        }
        public UInt16 FiberBreak_DayHour
        {
            get
            {
                int day = FiberBreak.FiberBreakTmie.Day;
                int hour = FiberBreak.FiberBreakTmie.Hour;
                byte[] b = new byte[2];
                b[0] = BitConverter.GetBytes(day)[0];
                b[1] = BitConverter.GetBytes(hour)[0];
                return BitConverter.ToUInt16(b, 0);
            }
        }
        public UInt16 FiberBreak_MinSec
        {
            get
            {
                int minute = FiberBreak.FiberBreakTmie.Minute;
                int second = FiberBreak.FiberBreakTmie.Second;
                byte[] b = new byte[2];
                b[0] = BitConverter.GetBytes(minute)[0];
                b[1] = BitConverter.GetBytes(second)[0];
                return BitConverter.ToUInt16(b, 0);
            }
        }       
        public DTSFaultInfo DTSFault { get; set; }      //设备故障信息
        public List<ZoneTempInfo> ZoneTempInfos { get; set; }         //所有分区温度信息
        public List<ZoneAlarmInfo> ConsTempAlarms { get; set; }    //所有定温报警信息
        public List<ZoneAlarmInfo> TempRiseAlarms{ get; set; }     //所有温升报警信息
        public List<ZoneAlarmInfo> RegionTempDifAlarms { get; set; }     //所有区域温差报警信息
        public List<double> TempDatas { get; set; }       //温度数据
        public DateTime FreshTime { get; set; }            //温度刷新时间
        public FiberBreakInfo FiberBreak { get; set; }      //断纤信息       

        public bool Flag;    //标识有效通道，有效：true  无效：false       
        
        
        public ChannelInfos()
        {
            FiberBreak = new FiberBreakInfo();
            DTSFault = new DTSFaultInfo();
            ZoneTempInfos = new List<ZoneTempInfo>();
            ConsTempAlarms = new List<ZoneAlarmInfo>();
            TempRiseAlarms = new List<ZoneAlarmInfo>();
            RegionTempDifAlarms = new List<ZoneAlarmInfo>();
            TempDatas = new List<double>();
            Flag = false;        
        }
        
    }

    /// <summary>
    /// 通道的分区温度信息
    /// </summary>
    [Serializable]
    public struct ZoneTempInfo
    {
        public UInt16 ChannelNum;    //通道号
        public UInt16 ZoneNumber;   //分区号
        public string ZoneName { get; set; }    //分区名称         
        public float StartPos;         //起始位置
        public float StopPos;         //结束位置
        public UInt16 TempRiseThreshold;         //温升阈值
        public UInt16 ConsTempThreshold;         //定温阈值
        public UInt16 RegionTempDifThreshold;       //区域温差阈值
        public bool TempRiseFlag;       //是否触发温升报警，false：不触发
        public bool ConsTempFlag;         //是否触发定温报警，false:不触发
        public bool RegionTempDifFlag;       //是否触发区域温差报警，false:不触发
        public bool FiberBreakFlag;            //是否触发断纤报警，false:不触发

        public float HigestTemp;         //分区最高温度
        public float LowestTemp;         //分区最低温度
        public float AvgTemp;          //分区平均温度
        public float HigestTempPos;      //最高温度点位
        public float LowestTempPos;       //最低温度点位   
        public UInt16 TempAlarmStatus { get; set; }     //温度报警状态，0：正常，1：火警
    };

    /// <summary>
    /// 分区定温报警、温升报警、区域温差报警信息
    /// </summary>
    [Serializable]
    public struct ZoneAlarmInfo
    {
        public float AlarmStartPos;      //报警起始位置
        public float AlarmStopPos;       //报警结束位置
        public DateTime AlarmTime;       //报警时间
        public UInt16 Alarm_YearMonth
        {
            get
            {
                int year = AlarmTime.Year - 2000;
                int month = AlarmTime.Month;
                byte[] b = new byte[2];
                b[0] = BitConverter.GetBytes(year)[0];
                b[1] = BitConverter.GetBytes(month)[0];
                return BitConverter.ToUInt16(b, 0);
            }
        }
        public UInt16 Alarm_DayHour
        {
            get
            {
                int day = AlarmTime.Day;
                int hour = AlarmTime.Hour;
                byte[] b = new byte[2];
                b[0] = BitConverter.GetBytes(day)[0];
                b[1] = BitConverter.GetBytes(hour)[0];
                return BitConverter.ToUInt16(b, 0);
            }
        }
        public UInt16 Alarm_MinSec
        {
            get
            {
                int minute = AlarmTime.Minute;
                int second = AlarmTime.Second;
                byte[] b = new byte[2];
                b[0] = BitConverter.GetBytes(minute)[0];
                b[1] = BitConverter.GetBytes(second)[0];
                return BitConverter.ToUInt16(b, 0);
            }
        }
        public UInt16 AlarmZoneNum;        //报警分区号
    };

    [Serializable]
    public struct FiberBreakInfo
    {
        public UInt16 ChannelNum;    //通道号
        public float FiberBreakPos;         //断纤位置         
        public DateTime FiberBreakTmie;      //断纤报警时间
    };

    [Serializable]
    public struct DTSFaultInfo
    {
        public UInt16 CommunicateFault;    //通信故障   0：正常，1：故障
        public UInt16 MainPowerFault;      //主电故障   0：正常，1：故障
        public UInt16 StandbyPowerFault;    //备电故障   0：正常，1：故障
        public UInt16 ChargeFault;        //充电故障   0：正常，1：故障
    };


}
