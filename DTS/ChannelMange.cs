using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DTS.ConfigFiles;
using System.Collections;
using DTS.CreateData;

namespace DTS
{
    public partial class ChannelMange : Form
    {  
        public static  UInt16 DefaultMeasureTime = 5; //默认测量时间5S
        public static UInt16 DefaultFiberLen = 10000;     //默认光纤长度
        public static UInt16 DefaultZoneCount = 1;      //默认分区数
        public static float DefaultChannelTemp = 30;     //通道默认温度
        private string oldEquipNum;
        private Dictionary<string, DTSEquip> existEquips;   
        internal Dictionary<string, DTSEquip> ExistEquips { get => existEquips; set => existEquips = value; }

        public ChannelMange()
        {            
            InitializeComponent();   
        }             
        private void ChannelMange_Load(object sender, EventArgs e)
        {
            cmb_ChooseEquip.Tag = null;
            cmb_ChooseEquip.Items.Clear();
            ExistEquips = ReadChannelCfg.Create().ReadFile();            
            foreach(KeyValuePair<string, DTSEquip> kvp in ExistEquips)
            {
                cmb_ChooseEquip.Items.Add("DTS:" + kvp.Key);                              
            }
            if (cmb_ChooseEquip.Items.Count > 0)
            {
                cmb_ChooseEquip.SelectedIndex = 0;
                int index = cmb_ChooseEquip.SelectedItem.ToString().IndexOf(":");
                oldEquipNum = cmb_ChooseEquip.SelectedItem.ToString().Substring(index + 1);
            }

            foreach (KeyValuePair<string, DTSEquip> kvp in ExistEquips)
            {
                int index = cmb_ChooseEquip.SelectedItem.ToString().IndexOf(":");
                string equipnum = ((string)cmb_ChooseEquip.SelectedItem).Substring(index + 1);
                if (kvp.Key.Contains(equipnum))
                {
                    DGV_ChannelInfo.RowCount = kvp.Value.ChannelCount;
                    List<ChannelInfos> channels = kvp.Value.channelInfo;
                    channels.Sort((x, y) => x.ChannelNum.CompareTo(y.ChannelNum));
                    for (int i = 0; i < kvp.Value.ChannelCount; i++)
                    {
                        DGV_ChannelInfo.Rows[i].Cells[0].Value = channels[i].ChannelNum;
                        DGV_ChannelInfo.Rows[i].Cells[1].Value = channels[i].ZoneCount;
                        if (channels[i].BaseTemp == 0)
                            channels[i].BaseTemp = 30;
                        DGV_ChannelInfo.Rows[i].Cells[2].Value = channels[i].BaseTemp;
                        DGV_ChannelInfo.Rows[i].Cells[3].Value = channels[i].MeasureTime;                   
                        DGV_ChannelInfo.Rows[i].Cells[4].Value = channels[i].FiberLen;
                    }
                }
            }
            DGV_ChannelInfo.Columns[0].ReadOnly = true;
            cmb_ChooseEquip.Tag = 1;
        }
        
        /// <summary>
        /// 序号列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_ChannelInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(((DataGridViewX)sender).RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
            }
        }  

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }       

        private void DGV_ChannelInfo_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewX grid = (DataGridViewX)sender;
            TextBox tx = e.Control as TextBox;
            if (grid.CurrentCell.ColumnIndex == 0 || grid.CurrentCell.ColumnIndex == 1 || grid.CurrentCell.ColumnIndex ==3)
            {
                tx.KeyPress -= new KeyPressEventHandler(tx_KeyPress1);
                tx.KeyPress -= new KeyPressEventHandler(tx_KeyPress);
                tx.KeyPress += new KeyPressEventHandler(tx_KeyPress1);
            }
            else
            {
                tx.KeyPress -= new KeyPressEventHandler(tx_KeyPress);
                tx.KeyPress -= new KeyPressEventHandler(tx_KeyPress1);
                tx.KeyPress += new KeyPressEventHandler(tx_KeyPress);
            }                           
        }
        private void tx_KeyPress1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//允许输入退格键 
            {
                int len = ((TextBox)sender).Text.Length;
                if (len < 1 && e.KeyChar == '0')
                    e.Handled = true;
                else if ((e.KeyChar < '0') || (e.KeyChar > '9'))//允许输入0-9数字                 
                    e.Handled = true;
            }                
       }
        private void tx_KeyPress(object sender, KeyPressEventArgs e)
        {
            //允许输入数字、小数点、删除键
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != (char)('.'))
                e.Handled = true;  
            //小数点只能输入一次
            if (e.KeyChar == (char)('.') && ((TextBox)sender).Text.IndexOf('.') != -1)
                e.Handled = true;
            if(e.KeyChar == (char)('.') && ((TextBox)sender).Text == "")
                e.Handled = true;
            //第一位是0，第二位必须为小数点
            if (e.KeyChar != (char)('.') && e.KeyChar != 8 && ((TextBox)sender).Text == "0")
                e.Handled = true;
        }
        private void AddChannel_FormClosed(object sender, FormClosedEventArgs e)
        {
            //保持当前设备的通道信息   
            int index = cmb_ChooseEquip.SelectedItem.ToString().IndexOf(":");
            oldEquipNum = cmb_ChooseEquip.SelectedItem.ToString().Substring(index + 1);
            List<ChannelInfos> channels = new List<ChannelInfos>();
            for (int i = 0; i < DGV_ChannelInfo.RowCount; i++)
            {
                ChannelInfos ci = new ChannelInfos();
                ci.SampleInterval = ExistEquips[oldEquipNum].SampleInterval;
                ci.ChannelNum = ushort.Parse(DGV_ChannelInfo.Rows[i].Cells[0].Value.ToString());
                ci.ZoneCount = ushort.Parse(DGV_ChannelInfo.Rows[i].Cells[1].Value.ToString());
                ci.BaseTemp = float.Parse(DGV_ChannelInfo.Rows[i].Cells[2].Value.ToString());
                ci.MeasureTime = ushort.Parse(DGV_ChannelInfo.Rows[i].Cells[3].Value.ToString());
                ci.FiberLen = float.Parse(DGV_ChannelInfo.Rows[i].Cells[4].Value.ToString());
                ci.SlaveNum = (ushort)(ExistEquips[oldEquipNum].SlaveNum + ci.ChannelNum);
                channels.Add(ci);
            }
            if (ExistEquips.Keys.Contains(oldEquipNum))
            {
                ExistEquips[oldEquipNum].channelInfo = new List<ChannelInfos>(channels);
            }
            ReadChannelCfg.Create().SetValue(ExistEquips);
        }

        private void cmb_ChooseEquip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_ChooseEquip.Tag != null)
            {           
                //切换设备前需要保存当前设备所有通道信息
                int index = cmb_ChooseEquip.SelectedItem.ToString().IndexOf(":");
                string num = cmb_ChooseEquip.SelectedItem.ToString().Substring(index + 1);
                if (oldEquipNum != null)
                {
                    List<ChannelInfos> channels = new List<ChannelInfos>();
                    for (int i = 0; i < DGV_ChannelInfo.RowCount; i++)
                    {
                        ChannelInfos ci = new ChannelInfos();
                        ci.SampleInterval = ExistEquips[oldEquipNum].SampleInterval;
                        ci.ChannelNum = ushort.Parse(DGV_ChannelInfo.Rows[i].Cells[0].Value.ToString());
                        ci.ZoneCount = ushort.Parse(DGV_ChannelInfo.Rows[i].Cells[1].Value.ToString());
                        ci.BaseTemp = float.Parse(DGV_ChannelInfo.Rows[i].Cells[2].Value.ToString());
                        ci.MeasureTime = ushort.Parse(DGV_ChannelInfo.Rows[i].Cells[3].Value.ToString());
                        ci.FiberLen = float.Parse(DGV_ChannelInfo.Rows[i].Cells[4].Value.ToString());
                        ci.SlaveNum = (ushort)(ExistEquips[oldEquipNum].SlaveNum + ci.ChannelNum);
                        channels.Add(ci);
                    }
                    if (ExistEquips.Keys.Contains(oldEquipNum))
                    {
                        ExistEquips[oldEquipNum].channelInfo = new List<ChannelInfos>(channels);
                    }
                    ReadChannelCfg.Create().SetValue(ExistEquips);

                    oldEquipNum = num;
                    //更新Datagridview
                    DGV_ChannelInfo.Rows.Clear();
                    DGV_ChannelInfo.RowCount = ExistEquips[oldEquipNum].ChannelCount;
                    for (int i = 0; i < DGV_ChannelInfo.RowCount; i++)
                    {
                        DGV_ChannelInfo.Rows[i].Cells[0].Value = ExistEquips[oldEquipNum].channelInfo[i].ChannelNum;
                        DGV_ChannelInfo.Rows[i].Cells[1].Value = ExistEquips[oldEquipNum].channelInfo[i].ZoneCount;
                        DGV_ChannelInfo.Rows[i].Cells[2].Value = ExistEquips[oldEquipNum].channelInfo[i].BaseTemp;
                        DGV_ChannelInfo.Rows[i].Cells[3].Value = ExistEquips[oldEquipNum].channelInfo[i].MeasureTime;
                        DGV_ChannelInfo.Rows[i].Cells[4].Value = ExistEquips[oldEquipNum].channelInfo[i].FiberLen;
                    }
                }
            }
        }

       
    }
}
