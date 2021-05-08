using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DTS.ConfigFiles;
using DTS.CreateData;

namespace DTS
{
    public partial class ChannelParaSet : Form
    {

        private Dictionary<string, List<ChannelInfos>> channelinfos;   //利用ChannelInfos的Flag判断该通道是否有效
        public Dictionary<string, List<ChannelInfos>> ChannelInfo { get => channelinfos; set => channelinfos = value; }

        private Dictionary<string, List<ChannelInfos>> zoneInfos;
        public Dictionary<string, List<ChannelInfos>> ZoneInfos { get => zoneInfos; set => zoneInfos = value; }

        public Dictionary<string, DTSEquip> ExistEquips;

        private string oldChannelNum;
        private string oldEquipNum;
        public ChannelParaSet()
        {
            InitializeComponent();
          //  ChannelInfo = new Dictionary<string, List<ChannelInfos>>();
         //   ZoneInfos = new Dictionary<string, List<ChannelInfos>>();
            ExistEquips = new Dictionary<string, DTSEquip>();
        }

        private void ChannelParaSet_Load(object sender, EventArgs e)
        {
          //  ExistChannels = ReadChannelCfg.Create().ExistChannels;

            cmb_chooseEquip.Tag = null;
            cmb_chooseEquip.Items.Clear();
            cmb_ChooseChannel.Tag = null;
            cmb_ChooseChannel.Items.Clear();
            //  EquipAllInfos.Create().ReadCfg();
            //  ChannelInfo = EquipAllInfos.Create().ChannelInfos;
            //    ZoneInfos = ReadAlarmZoneCfg.Create().ChannelZoneInfos;
            ExistEquips = ReadAlarmZoneCfg.Create().ReadFile();
            foreach (KeyValuePair<string, DTSEquip> kvp in ExistEquips)
            {
                cmb_chooseEquip.Items.Add("DTS:" + kvp.Key);
            }
            if (cmb_chooseEquip.Items.Count > 0)
            {
                cmb_chooseEquip.SelectedIndex = 0;
                int index = cmb_chooseEquip.SelectedItem.ToString().IndexOf(":");
                oldEquipNum = cmb_chooseEquip.SelectedItem.ToString().Substring(index + 1);

                tabControl2.Enabled = true;
                tabControl2.Tabs[0].Text = "DTS:" + oldEquipNum;
                List<ChannelInfos> channels = ExistEquips[oldEquipNum].channelInfo;
                for (int i = 0; i < channels.Count; i++)
                {
                    cmb_ChooseChannel.Items.Add(channels[i].ChannelNum.ToString());
                }
                if (cmb_ChooseChannel.Items.Count > 0)
                {
                    cmb_ChooseChannel.SelectedIndex = 0;
                    string channelnum = cmb_ChooseChannel.SelectedItem.ToString();
                    oldChannelNum = channelnum;
                    ChannelInfos ci = channels.Find(delegate (ChannelInfos c) { return c.ChannelNum == ushort.Parse(channelnum); });
                    if (ci != null)
                    {
                        DGV_AlarmSet.RowCount = ci.ZoneTempInfos.Count;
                        for (int i = 0; i < ci.ZoneTempInfos.Count; i++)
                        {
                            DGV_AlarmSet.Rows[i].Cells[0].Value = ci.ZoneTempInfos[i].ZoneNumber;
                            DGV_AlarmSet.Rows[i].Cells[1].Value = ci.ZoneTempInfos[i].StartPos;
                            DGV_AlarmSet.Rows[i].Cells[2].Value = ci.ZoneTempInfos[i].StopPos;
                            DGV_AlarmSet.Rows[i].Cells[3].Value = ci.ZoneTempInfos[i].TempRiseFlag;
                            DGV_AlarmSet.Rows[i].Cells[4].Value = ci.ZoneTempInfos[i].ConsTempFlag;
                            DGV_AlarmSet.Rows[i].Cells[5].Value = ci.ZoneTempInfos[i].RegionTempDifFlag;
                            DGV_AlarmSet.Rows[i].Cells[6].Value = ci.ZoneTempInfos[i].FiberBreakFlag;
                        }

                        txt_SampleInterval.Text = (ExistEquips[oldEquipNum].SampleInterval).ToString("F2");
                        txt_MeasureTime.Text = ci.MeasureTime.ToString();
                        txt_FiberLen.Text = ci.FiberLen.ToString();
                        txt_Temp.Text = ci.BaseTemp.ToString();
                    }
                }
            }
            cmb_chooseEquip.Tag = 1;
            cmb_ChooseChannel.Tag = 1;
            DGV_AlarmSet.Columns["dataGridViewTextBoxColumn1"].ReadOnly = true;
            DGV_AlarmSet.Columns["dataGridViewTextBoxColumn2"].ReadOnly = true;
            DGV_AlarmSet.Columns["Column3"].ReadOnly = true;     
        }

        private void cmb_ChooseChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //切换通道前保存当前通道参数
            if (cmb_chooseEquip.Tag != null && cmb_ChooseChannel.Tag != null)
            {
                //切换设备前需要保存当前设备所有通道信息
                int index = cmb_chooseEquip.SelectedItem.ToString().IndexOf(":");
                string equipnum = cmb_chooseEquip.SelectedItem.ToString().Substring(index + 1);
                string channelnum = cmb_ChooseChannel.SelectedItem.ToString();

                //保存通道基本信息/分区信息
                ChannelInfos channel = ExistEquips[oldEquipNum].channelInfo.Find(delegate (ChannelInfos c) { return c.ChannelNum == ushort.Parse(oldChannelNum); });
                int channelindex = ExistEquips[oldEquipNum].channelInfo.FindIndex(item => item.ChannelNum == ushort.Parse(oldChannelNum));
                if (channel != null)
                {
                    ExistEquips[oldEquipNum].SampleInterval = float.Parse(txt_SampleInterval.Text);
                    channel.MeasureTime = ushort.Parse(txt_MeasureTime.Text);
                    channel.FiberLen = float.Parse(txt_FiberLen.Text);
                    channel.BaseTemp = float.Parse(txt_Temp.Text);
                    ExistEquips[oldEquipNum].channelInfo[channelindex] = channel;
                    ReadEquipCfg.Create().SetValue(ExistEquips);
                    ReadChannelCfg.Create().SetValue(ExistEquips);

                    ZoneTempInfo zone;
                    int zoneindex = -1;
                    ushort zonenum = 0;
                    for (int i = 0; i < DGV_AlarmSet.RowCount; i++)
                    {
                        zonenum = ushort.Parse(DGV_AlarmSet.Rows[i].Cells[0].Value.ToString());
                        zone = channel.ZoneTempInfos.Find(delegate (ZoneTempInfo c) { return c.ZoneNumber == zonenum; });
                        zoneindex = channel.ZoneTempInfos.FindIndex(item => item.ZoneNumber == zonenum);
                        if (zoneindex != -1)
                        {
                            zone.TempRiseFlag = bool.Parse(DGV_AlarmSet.Rows[i].Cells[3].Value.ToString());
                            zone.ConsTempFlag = bool.Parse(DGV_AlarmSet.Rows[i].Cells[4].Value.ToString());
                            zone.RegionTempDifFlag = bool.Parse(DGV_AlarmSet.Rows[i].Cells[5].Value.ToString());
                            zone.FiberBreakFlag = bool.Parse(DGV_AlarmSet.Rows[i].Cells[6].Value.ToString());
                        }
                        ExistEquips[oldEquipNum].channelInfo[channelindex].ZoneTempInfos[zoneindex] = zone;
                    }
                    ReadAlarmZoneCfg.Create().SetValue(ExistEquips);
                }
                oldEquipNum = equipnum;
                oldChannelNum = channelnum;
                               
                //更新通道基本信息/分区信息
                if (ExistEquips[oldEquipNum].ChannelCount > 0)
                {
                    ChannelInfos channel1 = ExistEquips[oldEquipNum].channelInfo.Find(delegate (ChannelInfos c) { return c.ChannelNum == ushort.Parse(oldChannelNum); });
                    if (channel1 != null)
                    {
                        txt_SampleInterval.Text = (ExistEquips[oldEquipNum].SampleInterval).ToString("F2");
                        txt_MeasureTime.Text = channel1.MeasureTime.ToString();
                        txt_FiberLen.Text = channel1.FiberLen.ToString();
                        txt_Temp.Text = channel1.BaseTemp.ToString();

                        DGV_AlarmSet.RowCount = channel1.ZoneTempInfos.Count;
                        for (int i = 0; i < DGV_AlarmSet.RowCount; i++)
                        {
                            DGV_AlarmSet.Rows[i].Cells[0].Value = channel1.ZoneTempInfos[i].ZoneNumber;
                            DGV_AlarmSet.Rows[i].Cells[1].Value = channel1.ZoneTempInfos[i].StartPos;
                            DGV_AlarmSet.Rows[i].Cells[2].Value = channel1.ZoneTempInfos[i].StopPos;
                            DGV_AlarmSet.Rows[i].Cells[3].Value = channel1.ZoneTempInfos[i].TempRiseFlag;
                            DGV_AlarmSet.Rows[i].Cells[4].Value = channel1.ZoneTempInfos[i].ConsTempFlag;
                            DGV_AlarmSet.Rows[i].Cells[5].Value = channel1.ZoneTempInfos[i].RegionTempDifFlag;
                            DGV_AlarmSet.Rows[i].Cells[6].Value = channel1.ZoneTempInfos[i].FiberBreakFlag;
                        }
                    }
                }              
            }
        }

        private void ChannelParaSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            //保存当前通道的参数数据
            //切换设备前需要保存当前设备所有通道信息
            int index = cmb_chooseEquip.SelectedItem.ToString().IndexOf(":");
            string equipnum = cmb_chooseEquip.SelectedItem.ToString().Substring(index + 1);
            string channelnum = cmb_ChooseChannel.SelectedItem.ToString();

            //保存通道基本信息
            ChannelInfos channel = ExistEquips[equipnum].channelInfo.Find(delegate (ChannelInfos c) { return c.ChannelNum == ushort.Parse(channelnum); });
            int channelindex = ExistEquips[equipnum].channelInfo.FindIndex(item => item.ChannelNum == ushort.Parse(channelnum));
            if (channel != null)
            {
                ExistEquips[equipnum].SampleInterval = float.Parse(txt_SampleInterval.Text);
                channel.MeasureTime = ushort.Parse(txt_MeasureTime.Text);
                channel.FiberLen = float.Parse(txt_FiberLen.Text);
                channel.BaseTemp = float.Parse(txt_Temp.Text);
                ExistEquips[equipnum].channelInfo[channelindex] = channel;
                ReadEquipCfg.Create().SetValue(ExistEquips);
                ReadChannelCfg.Create().SetValue(ExistEquips);

                ZoneTempInfo zone;
                int zoneindex = -1;
                ushort zonenum = 0;
                for (int i = 0; i < DGV_AlarmSet.RowCount; i++)
                {
                    zonenum = ushort.Parse(DGV_AlarmSet.Rows[i].Cells[0].Value.ToString());
                    zone = channel.ZoneTempInfos.Find(delegate (ZoneTempInfo c) { return c.ZoneNumber == zonenum; });
                    zoneindex = channel.ZoneTempInfos.FindIndex(item => item.ZoneNumber == zonenum);
                    if (zoneindex != -1)
                    {
                        zone.TempRiseFlag = bool.Parse(DGV_AlarmSet.Rows[i].Cells[3].Value.ToString());
                        zone.ConsTempFlag = bool.Parse(DGV_AlarmSet.Rows[i].Cells[4].Value.ToString());
                        zone.RegionTempDifFlag = bool.Parse(DGV_AlarmSet.Rows[i].Cells[5].Value.ToString());
                        zone.FiberBreakFlag = bool.Parse(DGV_AlarmSet.Rows[i].Cells[6].Value.ToString());
                    }
                    ExistEquips[equipnum].channelInfo[channelindex].ZoneTempInfos[zoneindex] = zone;
                }
                ReadAlarmZoneCfg.Create().SetValue(ExistEquips);
            }           

            oldEquipNum = equipnum;
            oldChannelNum = channelnum;        
        }

        private void cmb_chooseEquip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmb_chooseEquip.Tag != null && cmb_ChooseChannel.Tag != null)
            {
                //切换设备前需要保存当前设备所有通道信息               
                int index = cmb_chooseEquip.SelectedItem.ToString().IndexOf(":");
                string equipnum = cmb_chooseEquip.SelectedItem.ToString().Substring(index + 1);
                string channelnum = cmb_ChooseChannel.SelectedItem.ToString();

                //保存通道的基本信息/分区信息
                ChannelInfos channel = ExistEquips[oldEquipNum].channelInfo.Find(delegate (ChannelInfos c) { return c.ChannelNum == ushort.Parse(oldChannelNum); });
                int channelindex = ExistEquips[oldEquipNum].channelInfo.FindIndex(item => item.ChannelNum == ushort.Parse(oldChannelNum));
                if (channel != null)
                {
                    ExistEquips[oldEquipNum].SampleInterval = float.Parse(txt_SampleInterval.Text);
                    channel.MeasureTime = ushort.Parse(txt_MeasureTime.Text);
                    channel.FiberLen = float.Parse(txt_FiberLen.Text);
                    channel.BaseTemp = float.Parse(txt_Temp.Text);
                    ExistEquips[oldEquipNum].channelInfo[channelindex] = channel;
                    ReadEquipCfg.Create().SetValue(ExistEquips);
                    ReadChannelCfg.Create().SetValue(ExistEquips);

                    int zoneindex = -1;
                    ushort zonenum = 0;
                    for (int i = 0; i < DGV_AlarmSet.RowCount; i++)
                    {
                        zonenum = ushort.Parse(DGV_AlarmSet.Rows[i].Cells[0].Value.ToString());
                        ZoneTempInfo zone = channel.ZoneTempInfos.Find(item => item.ZoneNumber == zonenum);
                        zoneindex = channel.ZoneTempInfos.FindIndex(item => item.ZoneNumber == zonenum);
                        if (zoneindex != -1)
                        {
                            zone.TempRiseFlag = bool.Parse(DGV_AlarmSet.Rows[i].Cells[3].Value.ToString());
                            zone.ConsTempFlag = bool.Parse(DGV_AlarmSet.Rows[i].Cells[4].Value.ToString());
                            zone.RegionTempDifFlag = bool.Parse(DGV_AlarmSet.Rows[i].Cells[5].Value.ToString());
                            zone.FiberBreakFlag = bool.Parse(DGV_AlarmSet.Rows[i].Cells[6].Value.ToString());
                        }
                        ExistEquips[oldEquipNum].channelInfo[channelindex].ZoneTempInfos[zoneindex] = zone;
                    }
                    ReadAlarmZoneCfg.Create().SetValue(ExistEquips);
                }               

                oldEquipNum = equipnum;
                DGV_AlarmSet.Rows.Clear();
                cmb_ChooseChannel.Items.Clear();
                tabControl2.Tabs[0].Text = "DTS:" + oldEquipNum;

                //更新通道基本信息
                if (ExistEquips[oldEquipNum].ChannelCount > 0)
                {
                    int cicount = ExistEquips[oldEquipNum].ChannelCount;
                    for (int i = 0; i < cicount; i++)
                    {
                        cmb_ChooseChannel.Items.Add(ExistEquips[oldEquipNum].channelInfo[i].ChannelNum);
                    }
                    if (cmb_ChooseChannel.Items.Count > 0)
                    {
                        cmb_ChooseChannel.SelectedIndex = 0;
                        channelnum = cmb_ChooseChannel.SelectedItem.ToString();
                        oldChannelNum = channelnum;
                    }

                    ChannelInfos channel1 = ExistEquips[oldEquipNum].channelInfo.Find(delegate (ChannelInfos c) { return c.ChannelNum == ushort.Parse(oldChannelNum); });
                    if(channel1 != null)
                    {
                        txt_SampleInterval.Text = (ExistEquips[oldEquipNum].SampleInterval).ToString("F2");
                        txt_MeasureTime.Text = channel1.MeasureTime.ToString();
                        txt_FiberLen.Text = channel1.FiberLen.ToString();
                        txt_Temp.Text = channel1.BaseTemp.ToString();

                        DGV_AlarmSet.RowCount = channel1.ZoneTempInfos.Count;
                        for (int i = 0; i < DGV_AlarmSet.RowCount; i++)
                        {
                            DGV_AlarmSet.Rows[i].Cells[0].Value = channel1.ZoneTempInfos[i].ZoneNumber;
                            DGV_AlarmSet.Rows[i].Cells[1].Value = channel1.ZoneTempInfos[i].StartPos;
                            DGV_AlarmSet.Rows[i].Cells[2].Value = channel1.ZoneTempInfos[i].StopPos;
                            DGV_AlarmSet.Rows[i].Cells[3].Value = channel1.ZoneTempInfos[i].TempRiseFlag;
                            DGV_AlarmSet.Rows[i].Cells[4].Value = channel1.ZoneTempInfos[i].ConsTempFlag;
                            DGV_AlarmSet.Rows[i].Cells[5].Value = channel1.ZoneTempInfos[i].RegionTempDifFlag;
                            DGV_AlarmSet.Rows[i].Cells[6].Value = channel1.ZoneTempInfos[i].FiberBreakFlag;
                        }
                    }                   
                }               
            }
        }
    }
}
