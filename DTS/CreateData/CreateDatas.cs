using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTS.ConfigFiles;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Timers;
using DTS.DrawCurver;

namespace DTS.CreateData
{
    class CreateDatas     //一个通道信息
    {
        private CurverIni drawCurver;        
        private const int ConstTempThreRange = 1;               //高出定温阈值的范围

        //根据分区信息和报警信息为通道创建温度数据
        public static FiberBreakInfo CreateTempData(ref ChannelInfos channelinfo)
        {
            ChannelInfos channelInfo = CloneChanelInfo(channelinfo);
            List<ZoneTempInfo> channelZoneInfos = CloneChanelZoneInfo(channelInfo.ZoneTempInfos);
            channelInfo.ZoneCount = (ushort)channelZoneInfos.Count;      //通道分区数 

            int constTempAlarmCount = 0;
            int tempRiseAlarmCount =0;
            int regionTempDifAlarmCount = 0;
            List<double> TempDatas = new List<double>();     //通道温度数据
            Random rd = new Random();
            DateTime dt = DateTime.Now;
            bool flag = false;
            FiberBreakInfo fbi = new FiberBreakInfo();
            fbi.FiberBreakPos = -1;
            for (double cp = 0; cp <= channelInfo.FiberLen; cp = cp + channelInfo.SampleInterval)
            {
                int zone_ind = Get_Zone_Index(cp, channelZoneInfos);
                if (zone_ind != -1)
                {
                    double zone_length = channelZoneInfos[zone_ind].StopPos - channelZoneInfos[zone_ind].StartPos;
                    double hightemp = 0;

                    int r = rd.Next(-10000, 10000);
                    double noise = r * ConstTempThreRange / 10000.0;
                    double T_trend;
                    flag = false;
                    //存在定温报警                
                    if (channelZoneInfos[zone_ind].ConsTempFlag)
                    {
                        double consttemp = channelZoneInfos[zone_ind].ConsTempThreshold - channelInfo.BaseTemp;
                        //存在温升报警
                        if (channelZoneInfos[zone_ind].TempRiseFlag)
                        {
                            double temprise = channelZoneInfos[zone_ind].TempRiseThreshold;
                            //存在区域温差
                            if (channelZoneInfos[zone_ind].RegionTempDifFlag)     //定温报警、温升报警、区域温差报警同时存在
                            {
                                //判断定温阈值与基本温度和差异与温升阈值、区域温差的大小                            
                                double regiontempdif = channelZoneInfos[zone_ind].RegionTempDifThreshold;
                                if (consttemp >= temprise)
                                {
                                    if (consttemp >= regiontempdif)
                                        hightemp = channelZoneInfos[zone_ind].ConsTempThreshold;                                    
                                    else
                                    {
                                        flag = true;
                                        hightemp = regiontempdif + channelInfo.BaseTemp;
                                    }
                                }
                                else
                                {
                                    if (temprise >= regiontempdif)
                                        hightemp = temprise + channelInfo.BaseTemp;
                                    else
                                    {
                                        flag = true;
                                        hightemp = regiontempdif + channelInfo.BaseTemp;
                                    }
                                }
                            }
                            else     //不存在区域温差，存在定温报警、温升报警
                            {
                                if (consttemp > temprise)
                                    hightemp = channelInfo.BaseTemp + consttemp;
                                else
                                    hightemp = channelInfo.BaseTemp + temprise;
                            }
                        }
                        else   //不存在温升报警;  
                        {
                            if (channelZoneInfos[zone_ind].RegionTempDifFlag)   //存在定温报警和区域温差报警
                            {
                                double regiontempdif = channelZoneInfos[zone_ind].RegionTempDifThreshold;
                                if (consttemp > regiontempdif)                                
                                    hightemp = channelZoneInfos[zone_ind].ConsTempThreshold;                                
                                else
                                {
                                    flag = true;
                                    hightemp = regiontempdif + channelInfo.BaseTemp;
                                }
                            }
                            else    //不存在温升和区域温差，只有定温报警
                            {
                                hightemp = channelZoneInfos[zone_ind].ConsTempThreshold;
                            }
                        }
                    }
                    else    //不存在定温报警
                    {
                        //存在温升报警
                        if (channelZoneInfos[zone_ind].TempRiseFlag)
                        {
                            double temprise = channelZoneInfos[zone_ind].TempRiseThreshold;
                            if (channelZoneInfos[zone_ind].RegionTempDifFlag)    //存在温升报警和区域温差报警
                            {
                                double regiontempdif = channelZoneInfos[zone_ind].RegionTempDifThreshold;

                                if (temprise > regiontempdif)
                                    hightemp = temprise + channelInfo.BaseTemp;
                                else
                                {
                                    flag = true;
                                    hightemp = regiontempdif + channelInfo.BaseTemp;
                                }
                            }
                            else   //只存在温升报警
                            {
                                hightemp = channelInfo.BaseTemp + channelZoneInfos[zone_ind].TempRiseThreshold;
                            }
                        }
                        else  //只存在区域温差
                        {
                            if (channelZoneInfos[zone_ind].RegionTempDifFlag)
                            {
                                flag = true;
                                hightemp = channelZoneInfos[zone_ind].RegionTempDifThreshold + channelInfo.BaseTemp;
                            }
                        }
                    }
                    if (channelZoneInfos[zone_ind].ConsTempFlag || channelZoneInfos[zone_ind].TempRiseFlag || channelZoneInfos[zone_ind].RegionTempDifFlag)
                    {
                        if (flag)
                        {
                            if (cp <= channelZoneInfos[zone_ind].StartPos + zone_length / 4)
                            {
                                double zone_phase = (cp - channelZoneInfos[zone_ind].StartPos) / zone_length * Math.PI * 2;
                                T_trend = channelInfo.BaseTemp + (hightemp - channelInfo.BaseTemp) * Math.Sin(zone_phase);
                            }
                            else if (cp > channelZoneInfos[zone_ind].StartPos + zone_length / 4 && cp <= channelZoneInfos[zone_ind].StartPos + zone_length * 3 / 4)
                            {
                                T_trend = hightemp;
                            }
                            else
                            {
                                double zone_phase = (cp - channelZoneInfos[zone_ind].StartPos - zone_length * 3 / 4) / zone_length * Math.PI * 2 + Math.PI / 2;
                                T_trend = channelInfo.BaseTemp + (hightemp - channelInfo.BaseTemp) * Math.Sin(zone_phase);
                            }
                        }
                        else
                        {
                            if (cp <= channelZoneInfos[zone_ind].StartPos + zone_length / 3)
                            {
                                double zone_phase = (cp - channelZoneInfos[zone_ind].StartPos) / zone_length * Math.PI * 1.5;
                                T_trend = channelInfo.BaseTemp + (hightemp - channelInfo.BaseTemp) * Math.Sin(zone_phase);
                            }
                            else if (cp > channelZoneInfos[zone_ind].StartPos + zone_length / 3 && cp <= channelZoneInfos[zone_ind].StartPos + zone_length * 2 / 3)
                            {
                                T_trend = hightemp;
                            }
                            else
                            {
                                double zone_phase = (cp - channelZoneInfos[zone_ind].StartPos - zone_length * 2 / 3) / zone_length * Math.PI * 1.5 + Math.PI / 2;
                                T_trend = channelInfo.BaseTemp + (hightemp - channelInfo.BaseTemp) * Math.Sin(zone_phase);
                            }
                        }
                        double T = T_trend + noise/3;
                      
                        TempDatas.Add(T);
                    }
                    else
                    {
                        double d = rd.NextDouble();
                        d = d - 0.5;
                        double T = d + channelInfo.BaseTemp;
                        TempDatas.Add(T);
                    }
                }
                else
                    TempDatas.Add(0);
            }           

            //温度数据
            channelInfo.TempDatas = TempDatas;
            for (int i=0;i< channelZoneInfos.Count;i++)
            {
                if(channelZoneInfos[i].ConsTempFlag)   //定温flag=true
                {
                    constTempAlarmCount++;
                    if(channelZoneInfos[i].TempRiseFlag)    //温升flag=true
                    {
                        tempRiseAlarmCount++;                        
                        if (channelZoneInfos[i].RegionTempDifFlag)    //区域温差flag=true
                            regionTempDifAlarmCount++;
                        else
                        {
                            //区域flag=false,利用定温阈值和温升阈值判断是否存在区域温差报警
                            if (channelZoneInfos[i].TempRiseThreshold >= channelZoneInfos[i].RegionTempDifThreshold)
                                regionTempDifAlarmCount++;
                            else
                            {
                                if((channelZoneInfos[i].ConsTempThreshold - channelInfo.BaseTemp) >= channelZoneInfos[i].RegionTempDifThreshold)
                                    regionTempDifAlarmCount++;
                            }
                        }
                    }
                    else     //温升flag=false
                    {
                        if (channelZoneInfos[i].RegionTempDifFlag)      //区域温差flag=true
                        {
                            //利用定温阈值和区域温差阈值判断是否存在温升报警
                            regionTempDifAlarmCount++;
                            if(channelZoneInfos[i].RegionTempDifThreshold >= channelZoneInfos[i].TempRiseThreshold)
                                tempRiseAlarmCount++;
                            else
                            {
                                if((channelZoneInfos[i].ConsTempThreshold - channelInfo.BaseTemp) >= channelZoneInfos[i].TempRiseThreshold)
                                    tempRiseAlarmCount++;
                            }
                        }
                        else     //区域温差flag=false
                        {
                            //利用定温阈值判断是否存在温升和区域温差报警                            
                            if ((channelZoneInfos[i].ConsTempThreshold - channelInfo.BaseTemp) >= channelZoneInfos[i].TempRiseThreshold)
                                tempRiseAlarmCount++;
                            if((channelZoneInfos[i].ConsTempThreshold - channelInfo.BaseTemp) >= channelZoneInfos[i].RegionTempDifThreshold)
                                regionTempDifAlarmCount++;
                        }
                    }  
                }
                else
                {
                    //定温报警flag=false
                    if(channelZoneInfos[i].TempRiseFlag)    //温升报警flag=true
                    {
                        tempRiseAlarmCount++;
                        if (channelZoneInfos[i].RegionTempDifFlag)    //区域温差flag=true
                        {
                            //利用温升阈值和区域温差阈值判断是否存在定温报警
                            regionTempDifAlarmCount++;
                            if (channelZoneInfos[i].TempRiseThreshold >= (channelZoneInfos[i].ConsTempThreshold - channelInfo.BaseTemp))
                                constTempAlarmCount++;
                            else
                            {
                                if(channelZoneInfos[i].RegionTempDifThreshold >= (channelZoneInfos[i].ConsTempThreshold - channelInfo.BaseTemp))
                                    constTempAlarmCount++;
                            }
                        }
                        else    //区域温差flag=false
                        {
                            //利用温升阈值判断是否存在定温报警和区域温差报警
                            if(channelZoneInfos[i].TempRiseThreshold >= (channelZoneInfos[i].ConsTempThreshold - channelInfo.BaseTemp))
                                constTempAlarmCount++;
                            if(channelZoneInfos[i].TempRiseThreshold >= channelZoneInfos[i].RegionTempDifThreshold)
                                regionTempDifAlarmCount++;
                        }
                    }
                    else     //温升报警flag=false
                    {
                        if (channelZoneInfos[i].RegionTempDifFlag)   //区域温差flag=true
                        {
                            //利用区域温差阈值判断是否存在定温报警和温升报警
                            regionTempDifAlarmCount++;
                            if (channelZoneInfos[i].RegionTempDifThreshold >= (channelZoneInfos[i].ConsTempThreshold - channelInfo.BaseTemp))
                                constTempAlarmCount++;
                            if (channelZoneInfos[i].RegionTempDifThreshold >= channelZoneInfos[i].TempRiseThreshold)
                                tempRiseAlarmCount++; 
                        }                        
                    }
                }

                if (constTempAlarmCount > channelInfo.ConsTempAlarmCount)
                {
                    ZoneAlarmInfo constTempAlarm = new ZoneAlarmInfo();
                    constTempAlarm.AlarmStartPos = channelZoneInfos[i].StartPos + (channelZoneInfos[i].StopPos - channelZoneInfos[i].StartPos) / 3;
                    constTempAlarm.AlarmStopPos = channelZoneInfos[i].StartPos + (channelZoneInfos[i].StopPos - channelZoneInfos[i].StartPos) * 2 / 3;
                    constTempAlarm.AlarmTime = DateTime.Now;
                    constTempAlarm.AlarmZoneNum = channelZoneInfos[i].ZoneNumber;

                    channelInfo.ConsTempAlarms.Add(constTempAlarm);
                }

                if(tempRiseAlarmCount > channelInfo.TempRiseAlarmCount)
                {
                    ZoneAlarmInfo constTempAlarm = new ZoneAlarmInfo();
                    constTempAlarm.AlarmStartPos = channelZoneInfos[i].StartPos ;
                    constTempAlarm.AlarmStopPos = channelZoneInfos[i].StartPos + (channelZoneInfos[i].StopPos - channelZoneInfos[i].StartPos) / 3;
                    constTempAlarm.AlarmTime = DateTime.Now;
                    constTempAlarm.AlarmZoneNum = channelZoneInfos[i].ZoneNumber;

                    channelInfo.TempRiseAlarms.Add(constTempAlarm);
                }

                if(regionTempDifAlarmCount > channelInfo.RegionTempDifAlarmCount)
                {
                    ZoneAlarmInfo constTempAlarm = new ZoneAlarmInfo();
                    constTempAlarm.AlarmStartPos = channelZoneInfos[i].StartPos;
                    constTempAlarm.AlarmStopPos = channelZoneInfos[i].StopPos;
                    constTempAlarm.AlarmTime = DateTime.Now;
                    constTempAlarm.AlarmZoneNum = channelZoneInfos[i].ZoneNumber;

                    channelInfo.RegionTempDifAlarms.Add(constTempAlarm);
                }
                //存在断纤报警
                if (channelZoneInfos[i].FiberBreakFlag)
                {
                    channelInfo.FiberBreakStatus = 1;
                    
                    fbi.ChannelNum = channelInfo.ChannelNum;
                    fbi.FiberBreakTmie = dt;
                    int len = (int)((channelZoneInfos[i].StopPos - channelZoneInfos[i].StartPos) * 100);
                    fbi.FiberBreakPos = rd.Next(len) / 100.0f;                   
                }
                //分区温度信息
                ZoneTempInfo temp = channelZoneInfos[i];
                if (channelZoneInfos[i].ConsTempFlag || channelZoneInfos[i].TempRiseFlag || channelZoneInfos[i].RegionTempDifFlag)                                  
                    temp.TempAlarmStatus = 1;

                int startind = (int)(temp.StartPos / channelInfo.SampleInterval);
                int stopind = (int)(temp.StopPos / channelInfo.SampleInterval) ;
                double higestTemp = channelInfo.TempDatas[startind];
                double lowestTemp = channelInfo.TempDatas[startind];
                int higestTempInd = startind;
                int lowestTempInd = startind;
                double avgTemp = 0;
                for(int j= startind; j < stopind;j++)
                {
                    if (channelInfo.TempDatas[j] > higestTemp)
                    {
                        higestTemp = channelInfo.TempDatas[j];
                        higestTempInd = j;
                    }
                    if (channelInfo.TempDatas[j] < lowestTemp)
                    {
                        lowestTemp = channelInfo.TempDatas[j];
                        lowestTempInd = j;
                    }
                    avgTemp += channelInfo.TempDatas[j];
                }
                avgTemp = avgTemp / (stopind - startind + 1);
                temp.HigestTemp = (float)higestTemp*10;
                temp.LowestTemp = (float)lowestTemp*10;
                temp.AvgTemp = (float)avgTemp*10;
                temp.HigestTempPos = temp.StartPos + higestTempInd * channelInfo.SampleInterval;
                temp.LowestTempPos = temp.StartPos + lowestTempInd * channelInfo.SampleInterval;

                channelInfo.ZoneTempInfos.Add(temp);
                channelInfo.ConsTempAlarmCount = (ushort)constTempAlarmCount;
                channelInfo.TempRiseAlarmCount = (ushort)tempRiseAlarmCount;
                channelInfo.RegionTempDifAlarmCount = (ushort)regionTempDifAlarmCount;
            }
            channelInfo.TempCount = (ushort)TempDatas.Count;
            channelInfo.FreshTime = dt;
            channelinfo = CloneChanelInfo(channelInfo);

            return fbi;
        }

        public static int Get_Zone_Index(double cp, List<ZoneTempInfo> channelZoneInfos)
        {
            int ind = -1;
            for(int i=0;i<channelZoneInfos.Count;i++)
            if(cp>=channelZoneInfos[i].StartPos&&cp<=channelZoneInfos[i].StopPos)
            {
                ind = i;
                break;
            }
            return ind;
        }

        public static ChannelInfos CloneChanelInfo( ChannelInfos obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, obj);
            memoryStream.Position = 0;
            return (ChannelInfos)formatter.Deserialize(memoryStream);
        }

        public static List<ZoneTempInfo> CloneChanelZoneInfo( List<ZoneTempInfo> obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, obj);
            memoryStream.Position = 0;
            return ( List<ZoneTempInfo>)formatter.Deserialize(memoryStream);
        }
    }
}
