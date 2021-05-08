using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using DTS.CreateData;

namespace DTS.ConfigFiles
{
    public class ReadChannelCfg
    {
        private readonly FileSystemWatcher _fsw;
        private const string FileName = "ChannelCfg.config";
        private string ConfigPath;
        private Dictionary<string, DTSEquip> existEquips;       //配置的设备，key为主机编号
        internal Dictionary<string, DTSEquip> ExistEquips { get => existEquips; set => existEquips = value; }



        private static ReadChannelCfg _instance;
        public static ReadChannelCfg Create()
        {
            return _instance ?? (_instance = new ReadChannelCfg());
        }
        public ReadChannelCfg()
        {
            ConfigPath = System.Environment.CurrentDirectory + "\\";
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
            ExistEquips = ReadEquipCfg.Create().ReadFile();
            LoadOption();
            return ExistEquips;
        }

        private void LoadOption()
        {
            string ConfigFilePath = ConfigPath + FileName;
            ExeConfigurationFileMap ecf = new ExeConfigurationFileMap();
            ecf.ExeConfigFilename = ConfigFilePath;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(ecf, ConfigurationUserLevel.None);         
            string[] fileKeys = config.AppSettings.Settings.AllKeys;
            Dictionary<string, List<ChannelInfos>> fileEquipChannels = new Dictionary<string, List<ChannelInfos>>();    //key为主机编号，value为该设备的通道
            if (fileKeys.Length > 0)
            {
                foreach (string key in fileKeys)     //将配置文件中的信息存入fileEquipChannels
                {
                    string value = GetIndexConfigValue(key);
                    if (value.Length >= 1)
                    {
                        if (key.Contains('_')) //主机编号_通道号
                        {
                            string equipnum = key.Split('_')[0].Trim();
                            string channelnum = key.Split('_')[1].Trim();
                            if (!fileEquipChannels.Keys.Contains(equipnum))
                            {
                                List<ChannelInfos> channels = new List<ChannelInfos>();
                                fileEquipChannels.Add(equipnum, channels);
                            }
                            string[] values = value.Split(';');
                            if (values.Length > 1)
                            {
                                ChannelInfos ci = new ChannelInfos();
                                ci.ChannelNum = UInt16.Parse(channelnum);
                                for (int j = 0; j < values.Length; j++)
                                {
                                    string[] temp = values[j].Split('=');
                                    if (temp.Length > 1)
                                    {
                                        switch (temp[0].Trim().ToLower())
                                        {                                           
                                            case "measuretime":
                                                if (temp[1] == null || temp[1] == "")
                                                    ci.MeasureTime = ChannelMange.DefaultMeasureTime;
                                                else
                                                    ci.MeasureTime = ushort.Parse(temp[1].Trim());
                                                break;
                                            case "zonecount":
                                                if (temp[1] == null || temp[1] == "")
                                                    ci.ZoneCount = ChannelMange.DefaultZoneCount;
                                                else
                                                    ci.ZoneCount = ushort.Parse(temp[1].Trim());
                                                break;
                                            case "channeltemp":
                                                if (temp[1] == null || temp[1] == "")
                                                    ci.BaseTemp = ChannelMange.DefaultChannelTemp;
                                                else
                                                    ci.BaseTemp = float.Parse(temp[1].Trim());
                                                break;
                                            case "fiberlen":
                                                if (temp[1] == null || temp[1] == "")
                                                    ci.FiberLen = ChannelMange.DefaultFiberLen;
                                                else
                                                    ci.FiberLen = float.Parse(temp[1].Trim());
                                                break;
                                        }
                                    }
                                }
                                if (!fileEquipChannels[equipnum].Contains(ci))
                                    fileEquipChannels[equipnum].Add(ci);
                            }
                        }
                    }                    
                }
                foreach (KeyValuePair<string, DTSEquip> kvp in ExistEquips)     //根据fileEquipChannels更新ExistEquips中的ChannelInfos通道信息
                {
                    List<ChannelInfos> channels = new List<ChannelInfos>();
                    if (fileEquipChannels.Keys.Contains(kvp.Key))
                    {
                        channels = fileEquipChannels[kvp.Key];
                        channels.Sort((x, y) => x.ChannelNum.CompareTo(y.ChannelNum));
                        if (kvp.Value.ChannelCount > fileEquipChannels[kvp.Key].Count)
                        {
                            int count = kvp.Value.ChannelCount - fileEquipChannels[kvp.Key].Count;
                            ushort maxchannelnum = channels[channels.Count - 1].ChannelNum;
                            for (int i = 0; i < count; i++)
                            {
                                ChannelInfos ci = new ChannelInfos();                                
                                ci.ChannelNum = (ushort)(maxchannelnum + i + 1);
                                ci.SlaveNum = (ushort)(kvp.Value.SlaveNum + ci.ChannelNum);
                                ci.MeasureTime = ChannelMange.DefaultMeasureTime;
                                ci.ZoneCount = ChannelMange.DefaultZoneCount;
                                ci.BaseTemp = ChannelMange.DefaultChannelTemp;
                                ci.FiberLen = ChannelMange.DefaultFiberLen;
                                channels.Add(ci);
                            }
                        }
                        else
                        {
                            int count = fileEquipChannels[kvp.Key].Count - kvp.Value.ChannelCount;
                            int existcount = kvp.Value.ChannelCount;
                            for (int i = 0; i < count; i++)
                            {
                                int filecount = channels.Count;
                                channels.RemoveAt(filecount - 1);
                            }
                        }
                        for (int i = 0; i < channels.Count; i++)
                            channels[i].SampleInterval = kvp.Value.SampleInterval;                      
                    }
                    else
                    {
                        int count = kvp.Value.ChannelCount;
                        for (int i=0;i< count;i++)
                        {
                            ChannelInfos ci = new ChannelInfos();
                            ci.ChannelNum = (ushort)( i + 1);
                            ci.SlaveNum = (ushort)(kvp.Value.SlaveNum + ci.ChannelNum);
                            ci.SampleInterval = kvp.Value.SampleInterval;
                            ci.MeasureTime = ChannelMange.DefaultMeasureTime;
                            ci.ZoneCount = ChannelMange.DefaultZoneCount;
                            ci.BaseTemp = ChannelMange.DefaultChannelTemp;
                            ci.FiberLen = ChannelMange.DefaultFiberLen;
                            channels.Add(ci);
                        }
                    }
                    kvp.Value.channelInfo = new List<ChannelInfos>(channels);
                }
            }
            else   //配置文件为空
            {
                foreach (KeyValuePair<string, DTSEquip> kvp in ExistEquips)
                {
                    List<ChannelInfos> channels = new List<ChannelInfos>();
                    for (int i = 0; i < kvp.Value.ChannelCount; i++)
                    {
                        ChannelInfos ci = new ChannelInfos();
                        ci.SampleInterval = kvp.Value.SampleInterval;
                        ci.ChannelNum = (ushort)(i + 1);
                        ci.SlaveNum = (ushort)(kvp.Value.SlaveNum + ci.ChannelNum);
                        ci.MeasureTime = ChannelMange.DefaultMeasureTime;
                        ci.ZoneCount = ChannelMange.DefaultZoneCount;
                        ci.BaseTemp = ChannelMange.DefaultChannelTemp;
                        ci.FiberLen = ChannelMange.DefaultFiberLen;
                        channels.Add(ci);
                    }
                    kvp.Value.channelInfo = new List<ChannelInfos>(channels);
                }
            }
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
                    LoadOption();                    
                }
            }
            catch (Exception ex)
            {                
            }
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
            foreach(KeyValuePair<string, DTSEquip> kvp in existequips)
            {
                equipnums.Add(kvp.Key);
            }
            //更新文件中已有的键值对
            for(int i=0; i< equipnums.Count;i++)
            {
                for(int j=0;j<existequips[equipnums[i]].channelInfo.Count;j++)
                {
                    string key = equipnums[i] + "_" + existequips[equipnums[i]].channelInfo[j].ChannelNum;

                    string value = "ZoneCount = " + existequips[equipnums[i]].channelInfo[j].ZoneCount + ";ChannelTemp = " + existequips[equipnums[i]].channelInfo[j].BaseTemp + 
                        ";MeasureTime = " + existequips[equipnums[i]].channelInfo[j].MeasureTime + "; FiberLen = " + existequips[equipnums[i]].channelInfo[j].FiberLen + ";";
                    if (((IList)fileKeys).Contains(key))
                        config.AppSettings.Settings[key].Value = value;
                    else
                        config.AppSettings.Settings.Add(key, value);

                    if(!newkeys.Contains(key))
                        newkeys.Add(key);
                }                            
            }
            //删除文件中多余的键值对
            string[] delectkey = fileKeys.Except(newkeys).ToArray();
            if(delectkey != null)
            {
                for(int i = 0;i<(delectkey.Length);i++)               
                    config.AppSettings.Settings.Remove(delectkey[i]);                
            }            
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件   
        }


    }
}
