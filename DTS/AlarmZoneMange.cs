using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DTS.ConfigFiles;
using DevComponents.DotNetBar;
using System.Reflection;
using DevComponents.DotNetBar.Controls;
using DTS.CreateData;

namespace DTS
{
    public partial class AlarmZoneMange : Form
    {
      //  public Dictionary<string, List<ChannelInfos>> zoneInfos;   //key为主机编号,value为所有通道下的报警分区信息
        public const UInt16 DefaultTempRiseThres = 5;
        public const UInt16 DefaultConsTempThres = 60;
        public const UInt16 DefaultRegionTempDifThres = 5;
     //   private TabControlPanel tcp;
     //   private bool ResiveFlag = false;        //用于判断客户是否点击了添加、删除或修改按钮，及ChannelZoneInfos是否发生改变

        private string oldChannelNum;
        private string oldEquipNum;

     //   private Dictionary<string, List<ChannelInfos>> existChannels;   //key为主机编号,
    //    internal Dictionary<string, List<ChannelInfos>> ExistChannels { get => existChannels; set => existChannels = value; }

        private Dictionary<string, DTSEquip> ExistEquips;

        public AlarmZoneMange(ChannelMange addchannel)
        {
            InitializeComponent();
        }
        private void AlarmZones_Load(object sender, EventArgs e)
        {
            cmb_chooseEquip.Tag = null;
            cmb_chooseEquip.Items.Clear();
            cmb_ChooseChannel.Tag = null;
            cmb_ChooseChannel.Items.Clear();

            tabControl.Enabled = false;
          //  EquipAllInfos.Create().ReadCfg();
          //  zoneInfos = EquipAllInfos.Create().ZoneInfos;
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
                
                tabControl.Enabled = true;
                tabControl.Tabs[0].Text = "DTS:" + oldEquipNum;
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
                    ChannelInfos channel = channels.Find(delegate (ChannelInfos c) { return c.ChannelNum == ushort.Parse(channelnum); });
                    if (channel != null)
                    {
                        DGV_ZoneInfos.RowCount = channel.ZoneTempInfos.Count;
                        for (int i = 0; i < channel.ZoneTempInfos.Count; i++)
                        {
                            DGV_ZoneInfos.Rows[i].Cells[0].Value = channel.ZoneTempInfos[i].ZoneNumber;
                            DGV_ZoneInfos.Rows[i].Cells[1].Value = channel.ZoneTempInfos[i].ZoneName;
                            DGV_ZoneInfos.Rows[i].Cells[2].Value = channel.ZoneTempInfos[i].StartPos;
                            DGV_ZoneInfos.Rows[i].Cells[3].Value = channel.ZoneTempInfos[i].StopPos;
                            DGV_ZoneInfos.Rows[i].Cells[4].Value = channel.ZoneTempInfos[i].TempRiseThreshold;
                            DGV_ZoneInfos.Rows[i].Cells[5].Value = channel.ZoneTempInfos[i].ConsTempThreshold;
                            DGV_ZoneInfos.Rows[i].Cells[6].Value = channel.ZoneTempInfos[i].RegionTempDifThreshold;
                        }
                    }
                }
            }
            cmb_chooseEquip.Tag = 1;
            cmb_ChooseChannel.Tag = 1;
        }
        private void DGV_ZoneInfos_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(((DataGridViewX)sender).RowHeadersDefaultCellStyle.ForeColor))
            {
               // e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
            }
        }
        private void AlarmZones_FormClosed(object sender, FormClosedEventArgs e)
        {
            //保持当前设备的通道信息   
            int index = cmb_chooseEquip.SelectedItem.ToString().IndexOf(":");
            oldEquipNum = cmb_chooseEquip.SelectedItem.ToString().Substring(index + 1);
            oldChannelNum = cmb_ChooseChannel.SelectedItem.ToString();
            ChannelInfos ci = ExistEquips[oldEquipNum].channelInfo.Find(delegate (ChannelInfos c) { return c.ChannelNum == ushort.Parse(oldChannelNum); });
            int channelindex = ExistEquips[oldEquipNum].channelInfo.FindIndex(item => item.ChannelNum == ushort.Parse(oldChannelNum));
            if (ci != null)
            {
                ZoneTempInfo zone;
                int zoneindex = -1;
                ushort zonenum = 0;
                for (int i = 0; i < DGV_ZoneInfos.RowCount; i++)
                {
                    zonenum = ushort.Parse(DGV_ZoneInfos.Rows[i].Cells[0].Value.ToString());
                    zone = ci.ZoneTempInfos.Find(delegate (ZoneTempInfo c) { return c.ZoneNumber == zonenum; });
                    zoneindex = ci.ZoneTempInfos.FindIndex(item => item.ZoneNumber == zonenum);
                    if (zoneindex != -1)
                    {
                        zone.ZoneNumber = zonenum;
                        zone.ZoneName = DGV_ZoneInfos.Rows[i].Cells[1].Value.ToString();
                        zone.StartPos = float.Parse(DGV_ZoneInfos.Rows[i].Cells[2].Value.ToString());
                        zone.StopPos = float.Parse(DGV_ZoneInfos.Rows[i].Cells[3].Value.ToString());
                        zone.TempRiseThreshold = ushort.Parse(DGV_ZoneInfos.Rows[i].Cells[4].Value.ToString());
                        zone.ConsTempThreshold = ushort.Parse(DGV_ZoneInfos.Rows[i].Cells[5].Value.ToString());
                        zone.RegionTempDifThreshold = ushort.Parse(DGV_ZoneInfos.Rows[i].Cells[6].Value.ToString());
                        ExistEquips[oldEquipNum].channelInfo[channelindex].ZoneTempInfos[zoneindex] = zone;
                    }                    
                }
            }

            //更新配置文件     
         //   EquipAllInfos.Create().RefreshZoneInfo(ReadChannelCfg.Create().ExistChannels, zoneInfos);
            ReadAlarmZoneCfg.Create().SetValue(ExistEquips);
        }

        private void cmb_ChooseChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_ChooseChannel.Tag != null && cmb_chooseEquip.Tag != null)
            {
                //切换设备前需要保存当前设备所有通道信息
                int index = cmb_chooseEquip.SelectedItem.ToString().IndexOf(":");
                string equipnum = cmb_chooseEquip.SelectedItem.ToString().Substring(index + 1);
                string channelnum = cmb_ChooseChannel.SelectedItem.ToString();
                ChannelInfos ci = ExistEquips[oldEquipNum].channelInfo.Find(delegate (ChannelInfos c) { return c.ChannelNum == ushort.Parse(oldChannelNum); });
                int channelindex = ExistEquips[oldEquipNum].channelInfo.FindIndex(item => item.ChannelNum == ushort.Parse(oldChannelNum));
                if (ci != null)
                {
                    ZoneTempInfo zone;
                    int zoneindex = -1;
                    ushort zonenum = 0;
                    for (int i = 0; i < DGV_ZoneInfos.RowCount; i++)
                    {
                        zonenum = ushort.Parse(DGV_ZoneInfos.Rows[i].Cells[0].Value.ToString());
                        zone = ci.ZoneTempInfos.Find(delegate (ZoneTempInfo c) { return c.ZoneNumber == zonenum; });
                        zoneindex = ci.ZoneTempInfos.FindIndex(item => item.ZoneNumber == zonenum);
                        if (zoneindex != -1)
                        {
                            zone.ZoneName = DGV_ZoneInfos.Rows[i].Cells[1].Value.ToString();
                            zone.StartPos = float.Parse(DGV_ZoneInfos.Rows[i].Cells[2].Value.ToString());
                            zone.StopPos = float.Parse(DGV_ZoneInfos.Rows[i].Cells[3].Value.ToString());
                            zone.TempRiseThreshold = ushort.Parse(DGV_ZoneInfos.Rows[i].Cells[4].Value.ToString());
                            zone.ConsTempThreshold = ushort.Parse(DGV_ZoneInfos.Rows[i].Cells[5].Value.ToString());
                            zone.RegionTempDifThreshold = ushort.Parse(DGV_ZoneInfos.Rows[i].Cells[6].Value.ToString());
                            ExistEquips[oldEquipNum].channelInfo[channelindex].ZoneTempInfos[zoneindex] = zone;
                        }
                    }                                
                }
                //更新配置文件     
                //  EquipAllInfos.Create().RefreshZoneInfo(ReadChannelCfg.Create().ExistChannels, zoneInfos);
                ReadAlarmZoneCfg.Create().SetValue(ExistEquips);

                oldEquipNum = equipnum;
                oldChannelNum = channelnum;

                //更新Datagridview
                DGV_ZoneInfos.Rows.Clear();
                List<ChannelInfos> channels1 = ExistEquips[oldEquipNum].channelInfo;
                ChannelInfos channel1 = channels1.Find(delegate (ChannelInfos c) { return c.ChannelNum == ushort.Parse(oldChannelNum); });
                int channelindex1 = channels1.FindIndex(item => item.ChannelNum == ushort.Parse(oldChannelNum));
                if (channel1 != null)
                {
                    DGV_ZoneInfos.RowCount = channel1.ZoneCount;
                    for (int i = 0; i < DGV_ZoneInfos.RowCount; i++)
                    {
                        DGV_ZoneInfos.Rows[i].Cells[0].Value = channel1.ZoneTempInfos[i].ZoneNumber;
                        DGV_ZoneInfos.Rows[i].Cells[1].Value = channel1.ZoneTempInfos[i].ZoneName;
                        DGV_ZoneInfos.Rows[i].Cells[2].Value = channel1.ZoneTempInfos[i].StartPos;
                        DGV_ZoneInfos.Rows[i].Cells[3].Value = channel1.ZoneTempInfos[i].StopPos;
                        DGV_ZoneInfos.Rows[i].Cells[4].Value = channel1.ZoneTempInfos[i].ConsTempThreshold;
                        DGV_ZoneInfos.Rows[i].Cells[5].Value = channel1.ZoneTempInfos[i].ConsTempThreshold;
                        DGV_ZoneInfos.Rows[i].Cells[6].Value = channel1.ZoneTempInfos[i].RegionTempDifThreshold;
                    }
                }               
            }
        }

        private void cmb_chooseEquip_SelectedIndexChanged(object sender, EventArgs e)
        {
            //切换设备前需要保存当前设备所有通道信息
            if (cmb_chooseEquip.Tag != null)
            {
                int index = cmb_chooseEquip.SelectedItem.ToString().IndexOf(":");
                string equipnum = cmb_chooseEquip.SelectedItem.ToString().Substring(index + 1);
                string channelnum = cmb_ChooseChannel.SelectedItem.ToString();
                List<ChannelInfos> channels = ExistEquips[oldEquipNum].channelInfo;
                ChannelInfos ci = channels.Find(delegate (ChannelInfos c) { return c.ChannelNum == ushort.Parse(oldChannelNum); });
                int channelindex = channels.FindIndex(item => item.ChannelNum == ushort.Parse(oldChannelNum));
                if (ci != null)
                {
                    ZoneTempInfo zone;
                    int zoneindex = -1;
                    ushort zonenum = 0;
                    for (int i = 0; i < DGV_ZoneInfos.RowCount; i++)
                    {
                        zonenum = ushort.Parse(DGV_ZoneInfos.Rows[i].Cells[0].Value.ToString());
                        zone = ci.ZoneTempInfos.Find(delegate (ZoneTempInfo c) { return c.ZoneNumber == zonenum; });
                        zoneindex = ci.ZoneTempInfos.FindIndex(item => item.ZoneNumber == zonenum);
                        if (zoneindex != -1)
                        {
                            zone.ZoneName = DGV_ZoneInfos.Rows[i].Cells[1].Value.ToString();
                            zone.StartPos = float.Parse(DGV_ZoneInfos.Rows[i].Cells[2].Value.ToString());
                            zone.StopPos = float.Parse(DGV_ZoneInfos.Rows[i].Cells[3].Value.ToString());
                            zone.TempRiseThreshold = ushort.Parse(DGV_ZoneInfos.Rows[i].Cells[4].Value.ToString());
                            zone.ConsTempThreshold = ushort.Parse(DGV_ZoneInfos.Rows[i].Cells[5].Value.ToString());
                            zone.RegionTempDifThreshold = ushort.Parse(DGV_ZoneInfos.Rows[i].Cells[6].Value.ToString());
                            ExistEquips[oldEquipNum].channelInfo[channelindex].ZoneTempInfos[zoneindex] = zone;
                        }
                    }                   
                }

                oldEquipNum = equipnum;
                oldChannelNum = channelnum;
                tabControl.Tabs[0].Text = "DTS:" + oldEquipNum;
                //更新Datagridview
                DGV_ZoneInfos.Rows.Clear();
                cmb_ChooseChannel.Items.Clear();
                cmb_ChooseChannel.Tag = null;
                List<ChannelInfos> channels1 = ExistEquips[oldEquipNum].channelInfo;
                if (channels1.Count > 0)
                {
                    for (int i = 0; i < channels1.Count; i++)
                    {
                        cmb_ChooseChannel.Items.Add(channels1[i].ChannelNum);
                    }
                    if (cmb_ChooseChannel.Items.Count > 0)
                    {
                        cmb_ChooseChannel.SelectedIndex = 0;
                        channelnum = cmb_ChooseChannel.SelectedItem.ToString();
                        oldChannelNum = channelnum;
                        ChannelInfos channel1 = channels1.Find(delegate (ChannelInfos c) { return c.ChannelNum == ushort.Parse(oldChannelNum); });
                        if (channel1 != null)
                        {
                            DGV_ZoneInfos.RowCount = channel1.ZoneCount;
                            for (int i = 0; i < DGV_ZoneInfos.RowCount; i++)
                            {
                                DGV_ZoneInfos.Rows[i].Cells[0].Value = channel1.ZoneTempInfos[i].ZoneNumber;
                                DGV_ZoneInfos.Rows[i].Cells[1].Value = channel1.ZoneTempInfos[i].ZoneName;
                                DGV_ZoneInfos.Rows[i].Cells[2].Value = channel1.ZoneTempInfos[i].StartPos;
                                DGV_ZoneInfos.Rows[i].Cells[3].Value = channel1.ZoneTempInfos[i].StopPos;
                                DGV_ZoneInfos.Rows[i].Cells[4].Value = channel1.ZoneTempInfos[i].ConsTempThreshold;
                                DGV_ZoneInfos.Rows[i].Cells[5].Value = channel1.ZoneTempInfos[i].ConsTempThreshold;
                                DGV_ZoneInfos.Rows[i].Cells[6].Value = channel1.ZoneTempInfos[i].RegionTempDifThreshold;
                            }
                            oldChannelNum = channel1.ChannelNum.ToString();
                        }
                    }
                }

                //更新配置文件     
              //  EquipAllInfos.Create().RefreshZoneInfo(ReadChannelCfg.Create().ExistChannels, zoneInfos);
                ReadAlarmZoneCfg.Create().SetValue(ExistEquips);
            }
        }

        private void bt_Revise_Click(object sender, EventArgs e)
        {
            int row = DGV_ZoneInfos.CurrentRow.Index;
            if (Txt_ZoneName.Text == "")
                Txt_ZoneName.Text = DGV_ZoneInfos.Rows[row].Cells[0].Value.ToString();

            int forwardstoppos = -1;
            int backwardstartpos = -1;
            if (row > 0)
                forwardstoppos = int.Parse(DGV_ZoneInfos.Rows[row - 1].Cells[3].Value.ToString());
            if (row < DGV_ZoneInfos.RowCount - 1)
                backwardstartpos = int.Parse(DGV_ZoneInfos.Rows[row + 1].Cells[2].Value.ToString());
            int startpos = -1;
            int stoppos = -1;
            UInt16 risethres = 0;
            UInt16 consthres = 0;
            UInt16 tempdifthres = 0;
            bool flag = false;
            if (int.TryParse(Txt_StartPos.Text, out startpos))
            {
                if (int.TryParse(Txt_StopPos.Text, out stoppos))
                {
                    if (stoppos > startpos)
                    {
                        bool forward = false;
                        bool backward = false;
                        if (forwardstoppos != -1)
                        {
                            if (startpos - forwardstoppos >= 0)
                                forward = true;
                        }
                        else
                            forward = true;

                        if (backwardstartpos != -1)
                        {
                            if (backwardstartpos - stoppos >= 0)
                                backward = true;
                        }
                        else
                            backward = true;

                        if (forward && backward)
                            flag = true;
                    }

                    if (flag)
                    {
                        //更新DataGridView
                        DGV_ZoneInfos.Rows[row].Cells[1].Value = Txt_ZoneName.Text;
                        DGV_ZoneInfos.Rows[row].Cells[2].Value = Txt_StartPos.Text;
                        DGV_ZoneInfos.Rows[row].Cells[3].Value = Txt_StopPos.Text;
                        //温升阈值
                        if (Txt_TempRiseThres.Text != "")
                        {
                            if (!UInt16.TryParse(Txt_TempRiseThres.Text, out risethres))
                                Txt_TempRiseThres.Text = DefaultTempRiseThres.ToString();
                        }
                        else
                            Txt_TempRiseThres.Text = DefaultTempRiseThres.ToString();

                        DGV_ZoneInfos.Rows[row].Cells[4].Value = Txt_TempRiseThres.Text;

                        //定温阈值
                        if (Txt_ConstTempThres.Text != "")
                        {
                            if (!UInt16.TryParse(Txt_ConstTempThres.Text, out consthres))
                                Txt_ConstTempThres.Text = DefaultConsTempThres.ToString();
                        }
                        else
                            Txt_ConstTempThres.Text = DefaultConsTempThres.ToString();

                        DGV_ZoneInfos.Rows[row].Cells[5].Value = Txt_ConstTempThres.Text;

                        //区域温差阈值
                        if (Txt_RegionTempDifThres.Text != "")
                        {
                            if (!UInt16.TryParse(Txt_RegionTempDifThres.Text, out tempdifthres))
                                Txt_RegionTempDifThres.Text = DefaultRegionTempDifThres.ToString();
                        }
                        else
                            Txt_RegionTempDifThres.Text = DefaultRegionTempDifThres.ToString();

                        DGV_ZoneInfos.Rows[row].Cells[6].Value = Txt_RegionTempDifThres.Text;


                        //更新字典
                        string channelnum = cmb_ChooseChannel.SelectedItem.ToString();
                        float startpos1 = float.Parse(DGV_ZoneInfos.Rows[row].Cells[2].Value.ToString());
                        float stoppos1 = float.Parse(DGV_ZoneInfos.Rows[row].Cells[3].Value.ToString());
                        int equipnumindex = cmb_chooseEquip.SelectedItem.ToString().IndexOf(":");
                        string equipnum = cmb_chooseEquip.SelectedItem.ToString().Substring(equipnumindex + 1);

                        ChannelInfos channel = ExistEquips[equipnum].channelInfo.Find(delegate (ChannelInfos c) { return c.ChannelNum == ushort.Parse(channelnum); });
                        int channelindex = ExistEquips[equipnum].channelInfo.FindIndex(item => item.ChannelNum == ushort.Parse(channelnum));

                        string zonenum = DGV_ZoneInfos.Rows[row].Cells[0].Value.ToString();
                        ZoneTempInfo zone = channel.ZoneTempInfos.Find(delegate (ZoneTempInfo z) { return z.ZoneNumber == ushort.Parse(zonenum); });
                        int zoneindex = channel.ZoneTempInfos.FindIndex(item => item.ZoneNumber == ushort.Parse(zonenum));
                        if (zoneindex != -1)
                        {
                            zone.ZoneName = Txt_ZoneName.Text; ;
                            zone.StartPos = float.Parse(Txt_StartPos.Text);
                            zone.StopPos = float.Parse(Txt_StopPos.Text);
                            zone.TempRiseThreshold = UInt16.Parse(Txt_TempRiseThres.Text);
                            zone.ConsTempThreshold = UInt16.Parse(Txt_ConstTempThres.Text);
                            zone.RegionTempDifThreshold = UInt16.Parse(Txt_RegionTempDifThres.Text);
                            ExistEquips[equipnum].channelInfo[channelindex].ZoneTempInfos[zoneindex] = zone;
                        }

                        //更新配置文件     
                        // EquipAllInfos.Create().RefreshZoneInfo(ReadChannelCfg.Create().ExistChannels, zoneInfos);
                        ReadAlarmZoneCfg.Create().SetValue(ExistEquips);
                    }
                }
            }
        }

        private void DGV_ZoneInfos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Txt_ZoneName.Text = DGV_ZoneInfos.CurrentRow.Cells[1].Value.ToString();
            Txt_StartPos.Text = DGV_ZoneInfos.CurrentRow.Cells[2].Value.ToString();
            Txt_StopPos.Text = DGV_ZoneInfos.CurrentRow.Cells[3].Value.ToString();
            Txt_TempRiseThres.Text = DGV_ZoneInfos.CurrentRow.Cells[4].Value.ToString();
            Txt_ConstTempThres.Text = DGV_ZoneInfos.CurrentRow.Cells[5].Value.ToString();
            Txt_RegionTempDifThres.Text = DGV_ZoneInfos.CurrentRow.Cells[6].Value.ToString();
        }
    }


    public static class ControlExtensions
    {
        public static T Clone<T>(this T controlToClone)
            where T : Control
        {
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            T instance = Activator.CreateInstance<T>();

            foreach (PropertyInfo propInfo in controlProperties)
            {
                if (propInfo.CanWrite)
                {
                    if (propInfo.Name != "WindowTarget")
                        propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
                }
            }
            return instance;
        }
    }
}
