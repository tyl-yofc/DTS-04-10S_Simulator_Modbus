namespace DTS
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.通道管理ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.通道管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.报警分区ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.参数设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cmb_chooseEquip = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX_Time = new DevComponents.DotNetBar.LabelX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.Bt_Start = new DevComponents.DotNetBar.ButtonX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.tabItem2 = new DevComponents.DotNetBar.TabItem(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.cmb_ServerIP = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.通道管理ToolStripMenuItem1,
            this.通道管理ToolStripMenuItem,
            this.报警分区ToolStripMenuItem,
            this.参数设置ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(843, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 通道管理ToolStripMenuItem1
            // 
            this.通道管理ToolStripMenuItem1.Name = "通道管理ToolStripMenuItem1";
            this.通道管理ToolStripMenuItem1.Size = new System.Drawing.Size(68, 21);
            this.通道管理ToolStripMenuItem1.Text = "设备管理";
            this.通道管理ToolStripMenuItem1.Click += new System.EventHandler(this.Menu_EquipMange_Click);
            // 
            // 通道管理ToolStripMenuItem
            // 
            this.通道管理ToolStripMenuItem.Name = "通道管理ToolStripMenuItem";
            this.通道管理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.通道管理ToolStripMenuItem.Text = "通道管理";
            this.通道管理ToolStripMenuItem.Click += new System.EventHandler(this.Menu_ChannelInfo_Click);
            // 
            // 报警分区ToolStripMenuItem
            // 
            this.报警分区ToolStripMenuItem.Name = "报警分区ToolStripMenuItem";
            this.报警分区ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.报警分区ToolStripMenuItem.Text = "报警分区";
            this.报警分区ToolStripMenuItem.Click += new System.EventHandler(this.Menu_AlarmZone_Click);
            // 
            // 参数设置ToolStripMenuItem
            // 
            this.参数设置ToolStripMenuItem.Name = "参数设置ToolStripMenuItem";
            this.参数设置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.参数设置ToolStripMenuItem.Text = "参数设置";
            this.参数设置ToolStripMenuItem.Click += new System.EventHandler(this.Menu_ParaSet_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl);
            this.panel1.Location = new System.Drawing.Point(0, 67);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(841, 517);
            this.panel1.TabIndex = 1;
            // 
            // tabControl
            // 
            this.tabControl.CanReorderTabs = true;
            this.tabControl.ColorScheme.TabBackground = System.Drawing.Color.Transparent;
            this.tabControl.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent;
            this.tabControl.ColorScheme.TabItemBackground = System.Drawing.Color.WhiteSmoke;
            this.tabControl.ColorScheme.TabItemBackground2 = System.Drawing.SystemColors.Control;
            this.tabControl.ColorScheme.TabItemBackgroundColorBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(230)))), ((int)(((byte)(249))))), 0F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(220)))), ((int)(((byte)(248))))), 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(208)))), ((int)(((byte)(245))))), 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(229)))), ((int)(((byte)(247))))), 1F)});
            this.tabControl.ColorScheme.TabItemBorder = System.Drawing.Color.LightCyan;
            this.tabControl.ColorScheme.TabItemBorderDark = System.Drawing.Color.Transparent;
            this.tabControl.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(235))))), 0F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(236)))), ((int)(((byte)(168))))), 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(218)))), ((int)(((byte)(89))))), 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(230)))), ((int)(((byte)(141))))), 1F)});
            this.tabControl.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254))))), 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254))))), 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254))))), 1F)});
            this.tabControl.ColorScheme.TabPanelBackground = System.Drawing.Color.Transparent;
            this.tabControl.ColorScheme.TabPanelBackground2 = System.Drawing.SystemColors.InactiveBorder;
            this.tabControl.ColorScheme.TabPanelBorder = System.Drawing.Color.Transparent;
            this.tabControl.Controls.Add(this.tabControlPanel1);
            this.tabControl.Location = new System.Drawing.Point(3, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabControl.SelectedTabIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(841, 510);
            this.tabControl.TabIndex = 0;
            this.tabControl.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl.Tabs.Add(this.tabItem1);
            this.tabControl.Text = "1";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(841, 484);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.Transparent;
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.SystemColors.InactiveBorder;
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.Transparent;
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right) 
            | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabItem1;
            // 
            // tabItem1
            // 
            this.tabItem1.AttachedControl = this.tabControlPanel1;
            this.tabItem1.Name = "tabItem1";
            this.tabItem1.Text = "通道1";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.cmb_ServerIP);
            this.groupPanel1.Controls.Add(this.cmb_chooseEquip);
            this.groupPanel1.Controls.Add(this.labelX_Time);
            this.groupPanel1.Controls.Add(this.labelX6);
            this.groupPanel1.Controls.Add(this.Bt_Start);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel1.Location = new System.Drawing.Point(0, 25);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(843, 38);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor = System.Drawing.Color.AliceBlue;
            this.groupPanel1.Style.BackColor2 = System.Drawing.Color.Snow;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 2;
            // 
            // cmb_chooseEquip
            // 
            this.cmb_chooseEquip.DisplayMember = "Text";
            this.cmb_chooseEquip.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmb_chooseEquip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_chooseEquip.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmb_chooseEquip.FormattingEnabled = true;
            this.cmb_chooseEquip.ItemHeight = 17;
            this.cmb_chooseEquip.Location = new System.Drawing.Point(440, 5);
            this.cmb_chooseEquip.Name = "cmb_chooseEquip";
            this.cmb_chooseEquip.Size = new System.Drawing.Size(160, 23);
            this.cmb_chooseEquip.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmb_chooseEquip.TabIndex = 25;
            this.cmb_chooseEquip.SelectedIndexChanged += new System.EventHandler(this.cmb_chooseEquip_SelectedIndexChanged);
            // 
            // labelX_Time
            // 
            this.labelX_Time.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX_Time.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_Time.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX_Time.ForeColor = System.Drawing.Color.Black;
            this.labelX_Time.Location = new System.Drawing.Point(680, 5);
            this.labelX_Time.Name = "labelX_Time";
            this.labelX_Time.Size = new System.Drawing.Size(153, 23);
            this.labelX_Time.TabIndex = 9;
            this.labelX_Time.Text = "0";
            this.labelX_Time.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX6
            // 
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.ForeColor = System.Drawing.Color.Black;
            this.labelX6.Location = new System.Drawing.Point(640, 6);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(44, 23);
            this.labelX6.TabIndex = 8;
            this.labelX6.Text = "时间：";
            this.labelX6.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // Bt_Start
            // 
            this.Bt_Start.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Bt_Start.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.Bt_Start.Location = new System.Drawing.Point(244, 0);
            this.Bt_Start.Name = "Bt_Start";
            this.Bt_Start.Size = new System.Drawing.Size(93, 32);
            this.Bt_Start.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.Bt_Start.TabIndex = 7;
            this.Bt_Start.Text = "启 动";
            this.Bt_Start.Click += new System.EventHandler(this.Bt_Start_Click);
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.ForeColor = System.Drawing.Color.Black;
            this.labelX3.Location = new System.Drawing.Point(377, 7);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(66, 23);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "选择设备：";
            this.labelX3.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(11, 5);
            this.labelX1.Name = "labelX1";
            this.labelX1.SingleLineColor = System.Drawing.Color.Transparent;
            this.labelX1.Size = new System.Drawing.Size(54, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "IP 地址";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // tabItem2
            // 
            this.tabItem2.AttachedControl = this.tabControlPanel1;
            this.tabItem2.Name = "tabItem2";
            this.tabItem2.Text = "通道1";
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // cmb_ServerIP
            // 
            this.cmb_ServerIP.DisplayMember = "Text";
            this.cmb_ServerIP.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmb_ServerIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_ServerIP.FormattingEnabled = true;
            this.cmb_ServerIP.ItemHeight = 15;
            this.cmb_ServerIP.Location = new System.Drawing.Point(71, 6);
            this.cmb_ServerIP.Name = "cmb_ServerIP";
            this.cmb_ServerIP.Size = new System.Drawing.Size(146, 21);
            this.cmb_ServerIP.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmb_ServerIP.TabIndex = 27;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 586);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DTS模拟器";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 通道管理ToolStripMenuItem1;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.TabControl tabControl;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabItem1;
        private DevComponents.DotNetBar.TabItem tabItem2;
        private DevComponents.DotNetBar.ButtonX Bt_Start;
        private DevComponents.DotNetBar.LabelX labelX_Time;
        private DevComponents.DotNetBar.LabelX labelX6;
        private System.Windows.Forms.Timer timer;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmb_chooseEquip;
        private System.Windows.Forms.ToolStripMenuItem 通道管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 报警分区ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 参数设置ToolStripMenuItem;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmb_ServerIP;
    }
}

