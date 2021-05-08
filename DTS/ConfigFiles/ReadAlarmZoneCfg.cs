using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Collections;
using System.Xml.Serialization;
using DTS.CreateData;

namespace DTS.ConfigFiles
{
    class ReadAlarmZoneCfg
    {
        private const string FileName = "AlarmZoneCfg.config";
        private readonly FileSystemWatcher _fsw;       
        private string ConfigPath;
        private Dictionary<string, DTSEquip> existEquips;      //key为主机编号,channelCfg配置文件信息
        internal Dictionary<string, DTSEquip> ExistEquips { get => existEquips; set => existEquips = value; }


        private static ReadAlarmZoneCfg _instance;
        public static ReadAlarmZoneCfg Create()
        {
            return _instance ?? (_instance = new ReadAlarmZoneCfg());
        }       
        public ReadAlarmZoneCfg()
        {
            ConfigPath = System.Environment.CurrentDirectory + "\\";
            ExistEquips = new Dictionary<string, DTSEquip>();
            if (_fsw == null)
            {
                _fsw = new FileSystemWatcher
                {
                    Path = ConfigPath,
                    Filter = FileName,
                    NotifyFilter = NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.Attributes
                };
                _fsw.Changed += new FileSystemEventHandler(FswChanged);
                _fsw.EnableRaisingEvents = true;
            }
        }
        public Dictionary<string, DTSEquip> ReadFile()
        {
            ExistEquips = ReadChannelCfg.Create().ReadFile();
            LoadOption();
            return ExistEquips;
        }

        public void LoadOption()
        {
            string ConfigFilePath = ConfigPath + FileName;
            ExeConfigurationFileMap ecf = new ExeConfigurationFileMap();
            ecf.ExeConfigFilename = ConfigFilePath;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(ecf, ConfigurationUserLevel.None);   
            string[] fileKeys = config.AppSettings.Settings.AllKeys;
            Dictionary<string, List<ChannelInfos>> fileEquipChannelZoness = new Dictionary<string, List<ChannelInfos>>();    //key为主机编号，value为该设备的通道
            if (fileKeys.Length > 0)
            {
                foreach (string key in fileKeys)     //将配置文件中的信息存入fileEquipChannels
                {
                    string value = GetIndexConfigValue(key);
                    if (value.Length >= 1)
                    {
                        if (key.Contains('_')) //主机编号_通道号_分区号
                        {
                            string equipnum = key.Split('_')[0].Trim();
                            string channelnum = key.Split('_')[1].Trim();
                            string zonenum = key.Split('_')[2].Trim();
                            if (!fileEquipChannelZoness.Keys.Contains(equipnum))
                            {
                                List<ChannelInfos> channels = new List<ChannelInfos>();
                                fileEquipChannelZoness.Add(equipnum, channels);
                            }
                            string[] values = value.Split(';');
                            if (values.Length > 1)
                            {
                                ChannelInfos ci = new ChannelInfos();
                                ci.ChannelNum = UInt16.Parse(channelnum);
                                ci.ZoneTempInfos = new List<ZoneTempInfo>();
                                ZoneTempInfo zti = new ZoneTempInfo();
                                zti.ChannelNum = UInt16.Parse(channelnum);
                                zti.ZoneNumber = ushort.Parse(zonenum);
                                for (int j = 0; j < values.Length; j++)
                                {
                                    string[] temp = values[j].Split('=');
                                    if (temp.Length > 1)
                                    {                            
                                        switch (temp[0].Trim().ToLower())
                                        {
                                            case "zonename":                                                
                                                zti.ZoneName = temp[1].Trim();
                                                break;
                                             case "startpos":                                                
                                                zti.StartPos = float.Parse(temp[1].Trim());
                                                break;
                                            case "stoppos":                                                
                                                zti.StopPos = float.Parse(temp[1].Trim());
                                                break;
                                            case "temprisethre":                                                
                                                zti.TempRiseThreshold = ushort.Parse(temp[1].Trim());
                                                break;
                                            case "consttempthre":                                                
                                                zti.ConsTempThreshold = ushort.Parse(temp[1].Trim());
                                                break;
                                            case "regiontempdifthre":                                                
                                                zti.RegionTempDifThreshold = ushort.Parse(temp[1].Trim());
                                                break;
                                            case "temprise":                                                
                                                zti.TempRiseFlag = bool.Parse(temp[1].Trim());
                                                break;
                                            case "consttemp":                                                
                                                zti.ConsTempFlag = bool.Parse(temp[1].Trim());
                                                break;
                                            case "regiontempdif":                                                
                                                zti.RegionTempDifFlag = bool.Parse(temp[1].Trim());
                                                break;
                                            case "fiberbreak":                                                
                                                zti.FiberBreakFlag = bool.Parse(temp[1].Trim());
                                                break;                                            
                                        }
                                    }
                                }
                                int count = fileEquipChannelZoness[equipnum].Count;
                                ChannelInfos fileci = fileEquipChannelZoness[equipnum].Find(delegate (ChannelInfos c) { return c.ChannelNum == ci.ChannelNum; });
                                int index = -1;
                                index = fileEquipChannelZoness[equipnum].FindIndex(item => item.ChannelNum == ci.ChannelNum);
                                if (index == -1)
                                {
                                    fileci = ci;
                                    index = 0;
                                }
                                if (fileci.ZoneTempInfos == null)
                                    fileci.ZoneTempInfos = new List<ZoneTempInfo>();
                                ZoneTempInfo zt = fileci.ZoneTempInfos.Find(delegate (ZoneTempInfo z) { return z.ZoneNumber == zti.ZoneNumber; });
                                int index1 = -1;
                                index1 = fileci.ZoneTempInfos.FindIndex(item => item.ZoneNumber == zti.ZoneNumber);
                                if (index1 == -1)
                                {
                                    fileci.ZoneTempInfos.Add(zti);
                                }
                                if (index == 0)
                                    fileEquipChannelZoness[equipnum].Add(fileci);

                                /*
                                if (!ci.ZoneTempInfos.Contains(zti))
                                {
                                    ci.ZoneTempInfos.Add(zti);
                                }                                
                                if (!fileEquipChannelZoness[equipnum].Contains(ci))
                                    fileEquipChannelZoness[equipnum].Add(ci);
                                    */
                            }
                        }
                    }                   
                }
                foreach (KeyValuePair<string, DTSEquip> kvp in ExistEquips)     //根据fileEquipChannels更新ExistEquips中的ZoneTempInfo分区信息
                {
                    int channelcount = kvp.Value.ChannelCount;
                    List<ChannelInfos> channeldlginfo = kvp.Value.channelInfo;
                    channeldlginfo.Sort((x, y) => x.ChannelNum.CompareTo(y.ChannelNum));
                    for (int i = 0; i < channelcount; i++)
                    {
                        int zonecount = kvp.Value.channelInfo[i].ZoneCount;
                        float interval = (float)Math.Round(kvp.Value.channelInfo[i].FiberLen / kvp.Value.SampleInterval, 2);
                        if (fileEquipChannelZoness.Keys.Contains(kvp.Key))
                        {
                            ChannelInfos ci = fileEquipChannelZoness[kvp.Key].Find(delegate (ChannelInfos c) { return c.ChannelNum == kvp.Value.channelInfo[i].ChannelNum; });
                            bool flag = false;
                            if (ci != null && zonecount == ci.ZoneTempInfos.Count)
                            {
                                int zc = ci.ZoneTempInfos.Count;
                                if (zc == kvp.Value.channelInfo[i].ZoneCount)
                                {
                                    if (Math.Abs(ci.ZoneTempInfos[zc - 1].StopPos - kvp.Value.channelInfo[i].FiberLen) < 0.001)
                                        flag = true;
                                }
                              //  kvp.Value.channelInfo[i].ZoneTempInfos = new List<ZoneTempInfo>(ci.ZoneTempInfos);
                            }
                            if (flag)
                            {
                                kvp.Value.channelInfo[i].ZoneTempInfos = new List<ZoneTempInfo>(ci.ZoneTempInfos);
                            }
                            else
                            {
                                List<ZoneTempInfo> zti = new List<ZoneTempInfo>();
                                for (int j = 0; j < zonecount; j++)
                                {
                                    ZoneTempInfo zone = new ZoneTempInfo();
                                    zone.ChannelNum = kvp.Value.channelInfo[i].ChannelNum;
                                    zone.ZoneNumber = (ushort)(j + 1);
                                    zone.ZoneName = (j + 1).ToString();
                                    if (j == 0)
                                        zone.StartPos = 0;
                                    else
                                        zone.StartPos = j * interval;

                                    if (j == zonecount - 1)
                                        zone.StopPos = kvp.Value.channelInfo[i].FiberLen;
                                    else
                                        zone.StopPos = j * interval + interval;
                                    zone.TempRiseThreshold = AlarmZoneMange.DefaultTempRiseThres;
                                    zone.ConsTempThreshold = AlarmZoneMange.DefaultConsTempThres;
                                    zone.RegionTempDifThreshold = AlarmZoneMange.DefaultRegionTempDifThres;
                                    zone.TempRiseFlag = false;
                                    zone.ConsTempFlag = false;
                                    zone.RegionTempDifFlag = false;
                                    zone.FiberBreakFlag = false;
                                    zti.Add(zone);
                                }
                                kvp.Value.channelInfo[i].ZoneTempInfos = new List<ZoneTempInfo>(zti);
                            }
                        }
                        else
                        {
                            List<ZoneTempInfo> zti = new List<ZoneTempInfo>();
                            for (int j = 0; j < zonecount; j++)
                            {
                                ZoneTempInfo zone = new ZoneTempInfo();
                                zone.ChannelNum = kvp.Value.channelInfo[i].ChannelNum;
                                zone.ZoneNumber = (ushort)(j + 1);
                                zone.ZoneName = (j + 1).ToString();
                                if (j == 0)
                                    zone.StartPos = 0;
                                else
                                    zone.StartPos = j * interval;

                                if (j == zonecount - 1)
                                    zone.StopPos = kvp.Value.channelInfo[i].FiberLen;
                                else
                                    zone.StopPos = j * interval + interval;
                                zone.TempRiseThreshold = AlarmZoneMange.DefaultTempRiseThres;
                                zone.ConsTempThreshold = AlarmZoneMange.DefaultConsTempThres;
                                zone.RegionTempDifThreshold = AlarmZoneMange.DefaultRegionTempDifThres;
                                zone.TempRiseFlag = false;
                                zone.ConsTempFlag = false;
                                zone.RegionTempDifFlag = false;
                                zone.FiberBreakFlag = false;
                                zti.Add(zone);
                            }
                            kvp.Value.channelInfo[i].ZoneTempInfos = new List<ZoneTempInfo>(zti);
                        }
                    }
                }
            }
            else   //配置文件为空
            {
                foreach (KeyValuePair<string, DTSEquip> kvp in ExistEquips)
                {
                    int channelcount = kvp.Value.ChannelCount;
                    
                    for (int i=0;i<channelcount;i++)
                    {
                        float interval = (float)Math.Round(kvp.Value.channelInfo[i].FiberLen / kvp.Value.SampleInterval, 2);
                        int zonecount = kvp.Value.channelInfo[i].ZoneCount;
                        List<ZoneTempInfo> zti = new List<ZoneTempInfo>();
                        for (int j=0;j<zonecount;j++)
                        {
                            ZoneTempInfo zone = new ZoneTempInfo();
                            zone.ChannelNum = kvp.Value.channelInfo[i].ChannelNum;
                            zone.ZoneNumber = (ushort)(j + 1);
                            zone.ZoneName = (j + 1).ToString();
                            if (j == 0)
                                zone.StartPos = 0;
                            else
                                zone.StartPos = j * interval;

                            if (j == zonecount - 1)
                                zone.StopPos = kvp.Value.channelInfo[i].FiberLen;
                            else
                                zone.StopPos = j * interval + interval;
                            zone.TempRiseThreshold = AlarmZoneMange.DefaultTempRiseThres;
                            zone.ConsTempThreshold = AlarmZoneMange.DefaultConsTempThres;
                            zone.RegionTempDifThreshold = AlarmZoneMange.DefaultRegionTempDifThres;
                            zone.TempRiseFlag = false;
                            zone.ConsTempFlag = false;
                            zone.RegionTempDifFlag = false;
                            zone.FiberBreakFlag = false;
                            zti.Add(zone);
                        }
                        kvp.Value.channelInfo[i].ZoneTempInfos = new List<ZoneTempInfo>(zti);
                    }        
                }
            }
        }       

        public static T DeepCopy<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        public string GetIndexConfigValue(string key)
        {
            string flag = "";
            string indexConfigPath = ConfigPath + FileName;
            if (string.IsNullOrEmpty(indexConfigPath))
                return flag = "-1";//配置文件为空
            if (!File.Exists(indexConfigPath))
                return flag = "-1";//配置文件不存在

            ExeConfigurationFileMap ecf = new ExeConfigurationFileMap();
            ecf.ExeConfigFilename = indexConfigPath;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(ecf, ConfigurationUserLevel.None);
            try
            {
                flag = config.AppSettings.Settings[key].Value;
            }
            catch (Exception)
            {
                flag = "-2";
            }
            return flag;
        }

        private void FswChanged(object sender, FileSystemEventArgs e)
        {
            if (String.Compare(e.Name, FileName, StringComparison.OrdinalIgnoreCase) != 0) return;
            try
            {
                FileSystemWatcher watcher = (FileSystemWatcher)sender;
                if (watcher != null)
                {
                    watcher.EnableRaisingEvents = false;
                    Thread th = new Thread(new ThreadStart(delegate ()
                    {
                        Thread.Sleep(1000);
                        watcher.EnableRaisingEvents = true;
                    }));
                    th.Start();
                   // LoadOption();
                }
            }
            catch (Exception ex)
            { }
        }       

        public void SetValue(Dictionary<string,DTSEquip> existequips)
        {
            //更新ChannelCfg配置文件 
            ExeConfigurationFileMap ecf = new ExeConfigurationFileMap();
            ecf.ExeConfigFilename = ConfigPath + FileName;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(ecf, ConfigurationUserLevel.None);
            string[] fileKeys = config.AppSettings.Settings.AllKeys; 
            List<string> newkeys = new List<string>();
            List<string> equipnums = new List<string>();
            foreach (KeyValuePair<string, DTSEquip> kvp in existequips)
            {
                equipnums.Add(kvp.Key);
            }
            //更新文件中已有的键值对
            for (int i = 0; i < equipnums.Count; i++)    //设备数
            {
                for (int j = 0; j < existequips[equipnums[i]].ChannelCount; j++)     //通道数
                {                     
                    for(int k=0;k< existequips[equipnums[i]].channelInfo[j].ZoneCount;k++)        //分区数
                    {
                        string key = equipnums[i] + "_" + existequips[equipnums[i]].channelInfo[j].ChannelNum;
                        ZoneTempInfo zone = existequips[equipnums[i]].channelInfo[j].ZoneTempInfos[k];
                        key += "_" + zone.ZoneNumber;
                        string value = "ZoneName = " + zone.ZoneName + "; StartPos =" + (zone.StartPos).ToString("F2") + "; StopPos = " + zone.StopPos + 
                            "; TempRiseThre = " + zone.TempRiseThreshold.ToString() + "; ConstTempThre = " + zone.ConsTempThreshold.ToString() + 
                            ";RegionTempDifThre = " + zone.RegionTempDifThreshold.ToString() + ";" +"TempRise = " + zone.TempRiseFlag + "; ConstTemp = " + zone.ConsTempFlag + 
                            "; RegionTempDif = " + zone.RegionTempDifFlag + "; FiberBreak = " + zone.FiberBreakFlag + ";";

                        if (((IList)fileKeys).Contains(key))
                            config.AppSettings.Settings[key].Value = value;
                        else
                            config.AppSettings.Settings.Add(key, value);
                        if (!newkeys.Contains(key))
                            newkeys.Add(key);
                    }                    
                }
            }
            //删除文件中多余的键值对
            string[] delectkey = fileKeys.Except(newkeys).ToArray();
            if (delectkey != null)
            {
                for (int i = 0; i < (delectkey.Length); i++)
                    config.AppSettings.Settings.Remove(delectkey[i]);
            }                      
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件   
        }
    }
}
