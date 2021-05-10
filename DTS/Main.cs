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
using ZedGraph;
using DTS.DrawCurver;
using DTS.CreateData;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace DTS
{
    public partial class Main : Form
    {
        public EquipMange addEquipDlg;
        public ChannelMange addChannelDlg;
        public AlarmZoneMange alarmZonesDlg;
        public ChannelParaSet channelParaSetdlg;

        public static Dictionary<string, DTSEquip> Equips; 
        public static Dictionary<string, CurverIni> channelCurvers;
        public static string oldEquipNum;

        public static string ServerIP;
        public Main()
        {
            InitializeComponent();

            ReadChannelCfg.Create();
            ReadAlarmZoneCfg.Create();
            addEquipDlg = new EquipMange();
            addChannelDlg = new ChannelMange();
            alarmZonesDlg = new AlarmZoneMange(addChannelDlg);
            channelParaSetdlg = new ChannelParaSet();
            channelCurvers = new Dictionary<string, DrawCurver.CurverIni>();
            Equips = new Dictionary<string, DTSEquip>();
            ObjRefreshZed += SetZedGraphInvoke;
            timer.Start();
        }
        private void Main_Load(object sender, EventArgs e)
        {
            cmb_ServerIP.Items.Clear();
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach(IPAddress ip in ipadrlist)
            {
                // if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                //  ipAddressInput.Value = ip.ToString();
                if(ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    cmb_ServerIP.Items.Add(ip.ToString());
            }
            cmb_chooseEquip.Items.Clear();
            cmb_chooseEquip.Tag = null;
            RefreshCmbChooseEquip();    
            cmb_chooseEquip.Tag = 1;
            if(cmb_ServerIP.Items.Count > 0)
                cmb_ServerIP.SelectedIndex = 0;
        }
        private void RefreshCmbChooseEquip()
        {
            Equips = ReadAlarmZoneCfg.Create().ReadFile();
            cmb_chooseEquip.Items.Clear();
            if (Equips.Keys.Count > 0)
            {
                foreach (KeyValuePair<string, DTSEquip> kvp in Equips)
                {
                    if (kvp.Value.tcpServer._port > 0)
                        cmb_chooseEquip.Items.Add("DTS:" + kvp.Key);
                }
                if (cmb_chooseEquip.Items.Count > 0)
                {
                    cmb_chooseEquip.SelectedIndex = 0;
                    int index = cmb_chooseEquip.SelectedItem.ToString().IndexOf(":");
                    oldEquipNum = cmb_chooseEquip.SelectedItem.ToString().Substring(index + 1);

                    RefreshTableControl(oldEquipNum, Equips[oldEquipNum].channelInfo);
                }
            }
        }

        /// <summary>
        /// 更新TableControl的tableitem的个数和名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshTableControl(string equipnum,List<ChannelInfos> Channels)
        {
            Size size = tabControl.Tabs[0].AttachedControl.Size;
            tabControl.SuspendLayout();
            this.SuspendLayout();

            tabControl.Tabs.Clear();
            tabControl.TabAlignment = eTabStripAlignment.Top;
            List<int> channelNums = new List<int>();            
            for(int i=0;i<Channels.Count;i++)           
                channelNums.Add(Channels[i].ChannelNum);           
            
            if(channelNums.Count == 0)
            {
                TabItem ti = tabControl.CreateTab("通道1");
                ti.Name = "通道1";
                ti.AttachedControl.Size = size;
                ti.AttachedControl.Dock = DockStyle.Fill;
                ZedGraphControl zed = new ZedGraphControl();
                CurverIni curver = new CurverIni(ti,zed);

                if (channelCurvers.Keys.Contains("1"))
                    channelCurvers["1"] = curver;
                else
                    channelCurvers.Add("1", curver);
            }
            else
            {
                int[] num = channelNums.ToArray();
                Array.Sort(num);
                for(int i=0;i< channelNums.Count; i++)
                {
                    TabItem ti = tabControl.CreateTab("通道" + num[i]);
                    ti.Name = "通道" + num[i];
                    ti.AttachedControl.Size = size;                    
                    ti.AttachedControl.Dock = DockStyle.Fill;
                    ZedGraphControl zed = new ZedGraphControl();
                    CurverIni curver = new CurverIni(ti,zed);

                    if (channelCurvers.Keys.Contains(num[i].ToString()))
                        channelCurvers[num[i].ToString()] = curver;
                    else
                        channelCurvers.Add(num[i].ToString(), curver);
                }
            }           
            tabControl.ResumeLayout(true);
            this.ResumeLayout(true);
        }

        private void Menu_EquipMange_Click(object sender, EventArgs e)
        {
            addEquipDlg.ShowDialog();
        }

        private void Menu_ChannelInfo_Click(object sender, EventArgs e)
        {
            addChannelDlg.ShowDialog();
        }

        private void Menu_AlarmZone_Click(object sender, EventArgs e)
        {
            alarmZonesDlg.ShowDialog();
        }

        private void Menu_ParaSet_Click(object sender, EventArgs e)
        {
            channelParaSetdlg.ShowDialog();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            labelX_Time.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public delegate void RefreshZed(string channelNum);
        public static RefreshZed ObjRefreshZed;
        private void SetZedGraphInvoke(string channelNum)
        {
            if (tabControl.Tabs["通道" + channelNum].AttachedControl.Controls.Count > 0)
            {
                if (tabControl.Tabs["通道" + channelNum].AttachedControl.Controls[0].InvokeRequired)
                    tabControl.Tabs["通道" + channelNum].AttachedControl.Controls[0].Invoke(ObjRefreshZed, channelNum);
                else
                {
                    ((ZedGraphControl)tabControl.Tabs["通道" + channelNum].AttachedControl.Controls[0]).MasterPane.AxisChange();
                    ((ZedGraphControl)tabControl.Tabs["通道" + channelNum].AttachedControl.Controls[0]).Refresh();
                }
            }
        }

        private void Bt_Start_Click(object sender, EventArgs e)
        {                   
            RefreshCmbChooseEquip();
            /*
            foreach(KeyValuePair<string,DTSEquip> kvp in Equips)
            {
                for(int i=0;i<kvp.Value.channelInfo.Count;i++)
                {
                    kvp.Value.channelInfo[i].SlaveNum = (ushort)(kvp.Value.SlaveNum + kvp.Value.channelInfo[i].ChannelNum);
                }
            }
            */
            ServerIP = cmb_ServerIP.SelectedItem.ToString();
            if (Bt_Start.Text == "启 动")
            {   
                ServerStart.Create().Start();               
                Bt_Start.Text = "停 止";                
            }
            else
            {      
                ServerStart.Create().ServerStop();
                Bt_Start.Text = "启 动";                                   
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(Bt_Start.Text == "停 止")
                ServerStart.Create().ServerStop();
        }

        private void cmb_chooseEquip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmb_chooseEquip.Tag != null)
            {
                //刷新tabitem的个数
                int index = cmb_chooseEquip.SelectedItem.ToString().IndexOf(":");
                string equipnum = cmb_chooseEquip.SelectedItem.ToString().Substring(index + 1);
                int channelcount = Equips[equipnum].ChannelCount;
                RefreshTableControl(equipnum, Equips[equipnum].channelInfo);

                oldEquipNum = equipnum;
            }
        }

    }
}
