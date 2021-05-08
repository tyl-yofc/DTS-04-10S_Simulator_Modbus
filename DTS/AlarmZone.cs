using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DTS.CreateData;

namespace DTS
{
    public partial class AlarmZone : Form
    {
        private TabControlPanel tcp;
        private Dictionary<string, DTSEquip> DTSEquips;
        public AlarmZone()
        {
            InitializeComponent();
        }

        private void AlarmZone_Load(object sender, EventArgs e)
        {
           
        }

        /*

        /// <summary>
        /// 添加分区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_Add_Click(object sender, EventArgs e)
        {
            string channelnum = ((string)cmb_ChooseChannel.SelectedItem).Substring(2);
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell txt_zonenum = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell txt_zonename = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell txt_startpos = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell txt_stoppos = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell txt_temprisethres = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell txt_constempthres = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell txt_tempdifthres = new DataGridViewTextBoxCell();
            if (Txt_ZoneName.Text == "")
                Txt_ZoneName.Text = (DGV_ZoneInfos.RowCount + 1).ToString();

            txt_zonename.Value = Txt_ZoneName.Text;
            txt_zonenum.Value = (DGV_ZoneInfos.RowCount + 1).ToString();
            int startpos = -1;
            int stoppos = -1;
            UInt16 risethres = 0;
            UInt16 consthres = 0;
            UInt16 tempdifthres = 0;
            bool flag = false;
            if (int.TryParse(Txt_StartPos.Text, out startpos))
            {
                txt_startpos.Value = startpos;
                if (int.TryParse(Txt_StopPos.Text, out stoppos) && stoppos > startpos)
                {
                    txt_stoppos.Value = stoppos;
                    //温升阈值
                    if (Txt_TempRiseThres.Text != "")
                    {
                        if (UInt16.TryParse(Txt_TempRiseThres.Text, out risethres))
                            txt_temprisethres.Value = risethres;
                        else
                        {
                            Txt_TempRiseThres.Text = DefaultTempRiseThres.ToString();
                            txt_temprisethres.Value = DefaultTempRiseThres.ToString();
                        }
                    }
                    else
                    {
                        Txt_TempRiseThres.Text = DefaultTempRiseThres.ToString();
                        txt_temprisethres.Value = DefaultTempRiseThres.ToString();
                    }
                    //定温阈值
                    if (Txt_ConstTempThres.Text != "")
                    {
                        if (UInt16.TryParse(Txt_ConstTempThres.Text, out consthres))
                            txt_constempthres.Value = consthres;
                        else
                        {
                            Txt_ConstTempThres.Text = DefaultConsTempThres.ToString();
                            txt_constempthres.Value = DefaultConsTempThres.ToString();
                        }
                    }
                    else
                    {
                        Txt_ConstTempThres.Text = DefaultConsTempThres.ToString();
                        txt_constempthres.Value = DefaultConsTempThres.ToString();
                    }
                    //区域温差阈值
                    if (Txt_RegionTempDifThres.Text != "")
                    {
                        if (UInt16.TryParse(Txt_RegionTempDifThres.Text, out tempdifthres))
                            txt_tempdifthres.Value = tempdifthres;
                        else
                        {
                            Txt_RegionTempDifThres.Text = DefaultRegionTempDifThres.ToString();
                            txt_tempdifthres.Value = DefaultRegionTempDifThres.ToString();
                        }
                    }
                    else
                    {
                        Txt_RegionTempDifThres.Text = DefaultRegionTempDifThres.ToString();
                        txt_tempdifthres.Value = DefaultRegionTempDifThres.ToString();
                    }
                    flag = true;
                }
            }

            if (flag)
            {
                //添加的报警分区有效，更新Datagridview和字典ChannelZoneInfos
                row.Cells.Add(txt_zonenum);
                row.Cells.Add(txt_zonename);
                row.Cells.Add(txt_startpos);
                row.Cells.Add(txt_stoppos);
                row.Cells.Add(txt_temprisethres);
                row.Cells.Add(txt_constempthres);
                row.Cells.Add(txt_tempdifthres);

                ZoneTempInfo zonetempinfo = new ZoneTempInfo();
                zonetempinfo.ZoneNumber = (UInt16)(DGV_ZoneInfos.RowCount + 1);
                zonetempinfo.ZoneName = Txt_ZoneName.Text;
                zonetempinfo.StartPos = startpos;
                zonetempinfo.StopPos = stoppos;
                zonetempinfo.TempRiseThreshold = risethres;
                zonetempinfo.ConsTempThreshold = consthres;
                zonetempinfo.RegionTempDifThreshold = tempdifthres;

                if (ChannelZoneInfos.Keys.Contains(channelnum))
                {
                    int count = ChannelZoneInfos[channelnum].Count;
                    if (startpos >= ChannelZoneInfos[channelnum][count - 1].StopPos)
                    {
                        DGV_ZoneInfos.Rows.Add(row);
                        ChannelZoneInfos[channelnum].Add(zonetempinfo);
                    }
                }
                else
                {
                    DGV_ZoneInfos.Rows.Add(row);
                    List<ZoneTempInfo> temp = new List<ZoneTempInfo>();
                    temp.Add(zonetempinfo);
                    ChannelZoneInfos.Add(channelnum, temp);
                }
                Txt_ZoneName.Text = "";
                ResiveFlag = true;
            }
        }


        /// <summary>
        /// 删除分区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_Delete_Click(object sender, EventArgs e)
        {
            int row = DGV_ZoneInfos.CurrentRow.Index;
            string msg = "确定删除" + cmb_ChooseChannel.SelectedItem.ToString() + "的分区：" + DGV_ZoneInfos.Rows[row].Cells[0].Value.ToString();
            if ((int)MessageBox.Show(msg, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == 1)
            {
                DGV_ZoneInfos.Rows.RemoveAt(row);
                DGV_ZoneInfos.Refresh();

                //更新字典
                string channelnum = cmb_ChooseChannel.SelectedItem.ToString().Substring(2);


                for (int i = row; i < DGV_ZoneInfos.RowCount; i++)
                {
                    int value = int.Parse(DGV_ZoneInfos.Rows[i].Cells[0].Value.ToString()) - 1;
                    DGV_ZoneInfos.Rows[i].Cells[0].Value = value;
                }


                string[] keys = ChannelZoneInfos.Keys.ToArray();
                for (int i = 0; i < keys.Length; i++)
                {
                    if (keys[i].Contains(channelnum))
                    {
                        List<ZoneTempInfo> temp = new List<ZoneTempInfo>();
                        if (DGV_ZoneInfos.Rows.Count > 0)
                        {
                            for (int j = 0; j < DGV_ZoneInfos.Rows.Count; j++)
                            {
                                ZoneTempInfo zonetempinfo = new ZoneTempInfo();
                                zonetempinfo.ZoneNumber = ushort.Parse(DGV_ZoneInfos.Rows[j].Cells[0].Value.ToString());
                                zonetempinfo.ZoneName = DGV_ZoneInfos.Rows[j].Cells[1].Value.ToString();
                                zonetempinfo.StartPos = float.Parse(DGV_ZoneInfos.Rows[j].Cells[2].Value.ToString());
                                zonetempinfo.StopPos = float.Parse(DGV_ZoneInfos.Rows[j].Cells[3].Value.ToString());
                                zonetempinfo.TempRiseThreshold = UInt16.Parse(DGV_ZoneInfos.Rows[j].Cells[4].Value.ToString());
                                zonetempinfo.ConsTempThreshold = UInt16.Parse(DGV_ZoneInfos.Rows[j].Cells[5].Value.ToString());
                                zonetempinfo.RegionTempDifThreshold = UInt16.Parse(DGV_ZoneInfos.Rows[j].Cells[6].Value.ToString());

                                temp.Add(zonetempinfo);
                            }
                            ChannelZoneInfos[keys[i]] = temp;
                        }
                        else
                        {
                            ChannelZoneInfos[keys[i]] = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 修改分区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        //更新字典
                        string channelnum = cmb_ChooseChannel.SelectedItem.ToString().Substring(2);
                        float startpos1 = float.Parse(DGV_ZoneInfos.Rows[row].Cells[2].Value.ToString());
                        float stoppos1 = float.Parse(DGV_ZoneInfos.Rows[row].Cells[3].Value.ToString());
                        int index = -1;
                        foreach (KeyValuePair<string, List<ZoneTempInfo>> kvp in ChannelZoneInfos)
                        {
                            if (kvp.Key.Contains(channelnum))
                            {
                                for (int i = 0; i < kvp.Value.Count; i++)
                                {
                                    if (kvp.Value[i].ZoneNumber == ushort.Parse(DGV_ZoneInfos.Rows[row].Cells[0].Value.ToString()))
                                    {
                                        index = i;
                                        ResiveFlag = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (ResiveFlag)
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
                            {
                                ZoneTempInfo temp = ChannelZoneInfos[channelnum][index];
                                temp.ZoneNumber = (ushort)(row + 1);
                                temp.ZoneName = Txt_ZoneName.Text;
                                temp.StartPos = float.Parse(Txt_StartPos.Text);
                                temp.StopPos = float.Parse(Txt_StopPos.Text);
                                temp.TempRiseThreshold = UInt16.Parse(Txt_TempRiseThres.Text);
                                temp.ConsTempThreshold = UInt16.Parse(Txt_ConstTempThres.Text);
                                temp.RegionTempDifThreshold = UInt16.Parse(Txt_RegionTempDifThres.Text);
                                ChannelZoneInfos[channelnum][index] = temp;
                            }
                        }
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
        private void cmb_ChooseChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //切换通道前需要保存当前通道报警分区信息
            if (ResiveFlag)
            {
                if (ChannelZoneInfos.Keys.Contains(oldChannelNum))
                {
                    List<ZoneTempInfo> channelZones = new List<ZoneTempInfo>(); ;
                    for (int i = 0; i < DGV_ZoneInfos.RowCount; i++)
                    {
                        ZoneTempInfo zonetempinfo = new ZoneTempInfo();
                        zonetempinfo.ZoneNumber = UInt16.Parse(DGV_ZoneInfos.Rows[i].Cells[0].Value.ToString());
                        zonetempinfo.ZoneName = DGV_ZoneInfos.Rows[i].Cells[1].Value.ToString();
                        zonetempinfo.StartPos = float.Parse(DGV_ZoneInfos.Rows[i].Cells[2].Value.ToString());
                        zonetempinfo.StopPos = float.Parse(DGV_ZoneInfos.Rows[i].Cells[3].Value.ToString());
                        zonetempinfo.TempRiseThreshold = UInt16.Parse(DGV_ZoneInfos.Rows[i].Cells[4].Value.ToString());
                        zonetempinfo.ConsTempThreshold = UInt16.Parse(DGV_ZoneInfos.Rows[i].Cells[5].Value.ToString());
                        zonetempinfo.RegionTempDifThreshold = UInt16.Parse(DGV_ZoneInfos.Rows[i].Cells[6].Value.ToString());
                        channelZones.Add(zonetempinfo);
                    }
                    ChannelZoneInfos[oldChannelNum] = channelZones;
                }
            }

            ResiveFlag = false;
            DGV_ZoneInfos.Rows.Clear();
            foreach (KeyValuePair<string, List<ZoneTempInfo>> kvp in ChannelZoneInfos)
            {
                string newchannelnum = ((string)cmb_ChooseChannel.SelectedItem).Substring(2);
                if (kvp.Key.Contains(newchannelnum) && kvp.Value != null)
                {
                    for (int i = 0; i < kvp.Value.Count; i++)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        DataGridViewTextBoxCell txt_zonenum = new DataGridViewTextBoxCell();
                        DataGridViewTextBoxCell txt_zonename = new DataGridViewTextBoxCell();
                        DataGridViewTextBoxCell txt_startpos = new DataGridViewTextBoxCell();
                        DataGridViewTextBoxCell txt_stoppos = new DataGridViewTextBoxCell();
                        DataGridViewTextBoxCell txt_temprisethres = new DataGridViewTextBoxCell();
                        DataGridViewTextBoxCell txt_constempthres = new DataGridViewTextBoxCell();
                        DataGridViewTextBoxCell txt_tempdifthres = new DataGridViewTextBoxCell();
                        txt_zonenum.Value = kvp.Value[i].ZoneNumber;
                        txt_zonename.Value = kvp.Value[i].ZoneName;
                        txt_startpos.Value = kvp.Value[i].StartPos;
                        txt_stoppos.Value = kvp.Value[i].StopPos;
                        txt_temprisethres.Value = kvp.Value[i].TempRiseThreshold;
                        txt_constempthres.Value = kvp.Value[i].ConsTempThreshold;
                        txt_tempdifthres.Value = kvp.Value[i].RegionTempDifThreshold;
                        row.Cells.Add(txt_zonenum);
                        row.Cells.Add(txt_zonename);
                        row.Cells.Add(txt_startpos);
                        row.Cells.Add(txt_stoppos);
                        row.Cells.Add(txt_temprisethres);
                        row.Cells.Add(txt_constempthres);
                        row.Cells.Add(txt_tempdifthres);
                        DGV_ZoneInfos.Rows.Add(row);
                    }
                }
            }
            oldChannelNum = cmb_ChooseChannel.SelectedItem.ToString().Substring(2);
            label_FiberLen.Text = existChannels[oldChannelNum].FiberLen.ToString();
        }

        private void DGV_ZoneInfos_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(((DataGridViewX)sender).RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
            }
        }

        private void AlarmZones_FormClosed(object sender, FormClosedEventArgs e)
        {
            //更新配置文件            
            ReadAlarmZoneCfg.Create().SetValue(ChannelZoneInfos);
        }
        */
    }
}
