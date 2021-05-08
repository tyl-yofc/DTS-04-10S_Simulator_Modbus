namespace DTS
{
    partial class AlarmZoneMange
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bt_Delete = new System.Windows.Forms.Button();
            this.bt_Add = new System.Windows.Forms.Button();
            this.bt_Revise = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label_FiberLen = new System.Windows.Forms.Label();
            this.tabControl = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.DGV_ZoneInfos = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmb_ChooseChannel = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.Txt_TempRiseThres = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.Txt_StartPos = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.Txt_StopPos = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.Txt_RegionTempDifThres = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.Txt_ConstTempThres = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.Txt_ZoneName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.cmb_chooseEquip = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_ZoneInfos)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_Delete
            // 
            this.bt_Delete.Location = new System.Drawing.Point(463, 338);
            this.bt_Delete.Name = "bt_Delete";
            this.bt_Delete.Size = new System.Drawing.Size(110, 28);
            this.bt_Delete.TabIndex = 12;
            this.bt_Delete.Text = "删 除";
            this.bt_Delete.UseVisualStyleBackColor = true;
            // 
            // bt_Add
            // 
            this.bt_Add.Location = new System.Drawing.Point(463, 300);
            this.bt_Add.Name = "bt_Add";
            this.bt_Add.Size = new System.Drawing.Size(110, 28);
            this.bt_Add.TabIndex = 13;
            this.bt_Add.Text = "添 加";
            this.bt_Add.UseVisualStyleBackColor = true;
            // 
            // bt_Revise
            // 
            this.bt_Revise.Location = new System.Drawing.Point(463, 376);
            this.bt_Revise.Name = "bt_Revise";
            this.bt_Revise.Size = new System.Drawing.Size(110, 28);
            this.bt_Revise.TabIndex = 14;
            this.bt_Revise.Text = "修 改";
            this.bt_Revise.UseVisualStyleBackColor = true;
            this.bt_Revise.Click += new System.EventHandler(this.bt_Revise_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(417, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "光纤长度(m)：";
            // 
            // label_FiberLen
            // 
            this.label_FiberLen.AutoSize = true;
            this.label_FiberLen.ForeColor = System.Drawing.Color.Red;
            this.label_FiberLen.Location = new System.Drawing.Point(516, 35);
            this.label_FiberLen.Name = "label_FiberLen";
            this.label_FiberLen.Size = new System.Drawing.Size(0, 12);
            this.label_FiberLen.TabIndex = 16;
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
            this.tabControl.Location = new System.Drawing.Point(0, 33);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabControl.SelectedTabIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(586, 442);
            this.tabControl.TabIndex = 19;
            this.tabControl.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl.Tabs.Add(this.tabItem1);
            this.tabControl.Text = "1";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.DGV_ZoneInfos);
            this.tabControlPanel1.Controls.Add(this.cmb_ChooseChannel);
            this.tabControlPanel1.Controls.Add(this.label1);
            this.tabControlPanel1.Controls.Add(this.bt_Revise);
            this.tabControlPanel1.Controls.Add(this.label_FiberLen);
            this.tabControlPanel1.Controls.Add(this.bt_Add);
            this.tabControlPanel1.Controls.Add(this.groupPanel1);
            this.tabControlPanel1.Controls.Add(this.bt_Delete);
            this.tabControlPanel1.Controls.Add(this.label2);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.tabControlPanel1.Size = new System.Drawing.Size(586, 416);
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
            // DGV_ZoneInfos
            // 
            this.DGV_ZoneInfos.AllowUserToAddRows = false;
            this.DGV_ZoneInfos.AllowUserToResizeColumns = false;
            this.DGV_ZoneInfos.AllowUserToResizeRows = false;
            this.DGV_ZoneInfos.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DGV_ZoneInfos.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_ZoneInfos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV_ZoneInfos.ColumnHeadersHeight = 25;
            this.DGV_ZoneInfos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DGV_ZoneInfos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_ZoneInfos.DefaultCellStyle = dataGridViewCellStyle8;
            this.DGV_ZoneInfos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.DGV_ZoneInfos.Location = new System.Drawing.Point(3, 37);
            this.DGV_ZoneInfos.Name = "DGV_ZoneInfos";
            this.DGV_ZoneInfos.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_ZoneInfos.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.DGV_ZoneInfos.RowHeadersVisible = false;
            this.DGV_ZoneInfos.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DGV_ZoneInfos.RowTemplate.Height = 25;
            this.DGV_ZoneInfos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_ZoneInfos.Size = new System.Drawing.Size(580, 261);
            this.DGV_ZoneInfos.TabIndex = 6;
            this.DGV_ZoneInfos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_ZoneInfos_CellClick);
            this.DGV_ZoneInfos.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DGV_ZoneInfos_RowPostPaint);
            // 
            // Column7
            // 
            this.Column7.HeaderText = "分区号";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 50;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.HeaderText = "分区名称";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.Width = 85;
            // 
            // Column2
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.HeaderText = "起始位置(m)";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column3.HeaderText = "结束位置(m)";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 80;
            // 
            // Column4
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column4.HeaderText = "温升阈值(℃)";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 85;
            // 
            // Column5
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column5.HeaderText = "定温阈值(℃)";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 85;
            // 
            // Column6
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column6.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column6.HeaderText = "区域温差阈值(℃)";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 110;
            // 
            // cmb_ChooseChannel
            // 
            this.cmb_ChooseChannel.DisplayMember = "Text";
            this.cmb_ChooseChannel.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmb_ChooseChannel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmb_ChooseChannel.FormattingEnabled = true;
            this.cmb_ChooseChannel.ItemHeight = 17;
            this.cmb_ChooseChannel.Location = new System.Drawing.Point(87, 6);
            this.cmb_ChooseChannel.Name = "cmb_ChooseChannel";
            this.cmb_ChooseChannel.Size = new System.Drawing.Size(307, 23);
            this.cmb_ChooseChannel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmb_ChooseChannel.TabIndex = 5;
            this.cmb_ChooseChannel.SelectedIndexChanged += new System.EventHandler(this.cmb_ChooseChannel_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "选择通道:";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.Txt_TempRiseThres);
            this.groupPanel1.Controls.Add(this.Txt_StartPos);
            this.groupPanel1.Controls.Add(this.Txt_StopPos);
            this.groupPanel1.Controls.Add(this.Txt_RegionTempDifThres);
            this.groupPanel1.Controls.Add(this.Txt_ConstTempThres);
            this.groupPanel1.Controls.Add(this.Txt_ZoneName);
            this.groupPanel1.Controls.Add(this.labelX6);
            this.groupPanel1.Controls.Add(this.labelX5);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Location = new System.Drawing.Point(4, 301);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(427, 107);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.Style.BackColor2 = System.Drawing.SystemColors.Control;
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
            this.groupPanel1.TabIndex = 7;
            // 
            // Txt_TempRiseThres
            // 
            // 
            // 
            // 
            this.Txt_TempRiseThres.Border.Class = "TextBoxBorder";
            this.Txt_TempRiseThres.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Txt_TempRiseThres.Location = new System.Drawing.Point(307, 12);
            this.Txt_TempRiseThres.Name = "Txt_TempRiseThres";
            this.Txt_TempRiseThres.Size = new System.Drawing.Size(100, 21);
            this.Txt_TempRiseThres.TabIndex = 11;
            // 
            // Txt_StartPos
            // 
            // 
            // 
            // 
            this.Txt_StartPos.Border.Class = "TextBoxBorder";
            this.Txt_StartPos.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Txt_StartPos.Location = new System.Drawing.Point(84, 41);
            this.Txt_StartPos.Name = "Txt_StartPos";
            this.Txt_StartPos.Size = new System.Drawing.Size(100, 21);
            this.Txt_StartPos.TabIndex = 10;
            // 
            // Txt_StopPos
            // 
            // 
            // 
            // 
            this.Txt_StopPos.Border.Class = "TextBoxBorder";
            this.Txt_StopPos.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Txt_StopPos.Location = new System.Drawing.Point(84, 70);
            this.Txt_StopPos.Name = "Txt_StopPos";
            this.Txt_StopPos.Size = new System.Drawing.Size(100, 21);
            this.Txt_StopPos.TabIndex = 9;
            // 
            // Txt_RegionTempDifThres
            // 
            // 
            // 
            // 
            this.Txt_RegionTempDifThres.Border.Class = "TextBoxBorder";
            this.Txt_RegionTempDifThres.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Txt_RegionTempDifThres.Location = new System.Drawing.Point(307, 72);
            this.Txt_RegionTempDifThres.Name = "Txt_RegionTempDifThres";
            this.Txt_RegionTempDifThres.Size = new System.Drawing.Size(100, 21);
            this.Txt_RegionTempDifThres.TabIndex = 8;
            // 
            // Txt_ConstTempThres
            // 
            // 
            // 
            // 
            this.Txt_ConstTempThres.Border.Class = "TextBoxBorder";
            this.Txt_ConstTempThres.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Txt_ConstTempThres.Location = new System.Drawing.Point(307, 41);
            this.Txt_ConstTempThres.Name = "Txt_ConstTempThres";
            this.Txt_ConstTempThres.Size = new System.Drawing.Size(100, 21);
            this.Txt_ConstTempThres.TabIndex = 7;
            // 
            // Txt_ZoneName
            // 
            // 
            // 
            // 
            this.Txt_ZoneName.Border.Class = "TextBoxBorder";
            this.Txt_ZoneName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Txt_ZoneName.Location = new System.Drawing.Point(84, 12);
            this.Txt_ZoneName.Name = "Txt_ZoneName";
            this.Txt_ZoneName.Size = new System.Drawing.Size(100, 21);
            this.Txt_ZoneName.TabIndex = 6;
            // 
            // labelX6
            // 
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Location = new System.Drawing.Point(226, 12);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(75, 23);
            this.labelX6.TabIndex = 5;
            this.labelX6.Text = "温升阈值：";
            this.labelX6.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Location = new System.Drawing.Point(204, 70);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(111, 23);
            this.labelX5.TabIndex = 4;
            this.labelX5.Text = "区域温差阈值：";
            this.labelX5.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(226, 41);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 23);
            this.labelX4.TabIndex = 3;
            this.labelX4.Text = "定温阈值：";
            this.labelX4.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(13, 41);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 2;
            this.labelX3.Text = "起始位置：";
            this.labelX3.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(13, 70);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "结束位置：";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(13, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "分区名称：";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // tabItem1
            // 
            this.tabItem1.AttachedControl = this.tabControlPanel1;
            this.tabItem1.Name = "tabItem1";
            this.tabItem1.Text = "通道1";
            // 
            // cmb_chooseEquip
            // 
            this.cmb_chooseEquip.DisplayMember = "Text";
            this.cmb_chooseEquip.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmb_chooseEquip.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmb_chooseEquip.FormattingEnabled = true;
            this.cmb_chooseEquip.ItemHeight = 17;
            this.cmb_chooseEquip.Location = new System.Drawing.Point(91, 9);
            this.cmb_chooseEquip.Name = "cmb_chooseEquip";
            this.cmb_chooseEquip.Size = new System.Drawing.Size(307, 23);
            this.cmb_chooseEquip.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmb_chooseEquip.TabIndex = 21;
            this.cmb_chooseEquip.SelectedIndexChanged += new System.EventHandler(this.cmb_chooseEquip_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(14, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 14);
            this.label3.TabIndex = 20;
            this.label3.Text = "选择设备:";
            // 
            // AlarmZones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 469);
            this.Controls.Add(this.cmb_chooseEquip);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tabControl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlarmZones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "报警分区";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AlarmZones_FormClosed);
            this.Load += new System.EventHandler(this.AlarmZones_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            this.tabControlPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_ZoneInfos)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bt_Delete;
        private System.Windows.Forms.Button bt_Add;
        private System.Windows.Forms.Button bt_Revise;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_FiberLen;
        private DevComponents.DotNetBar.TabControl tabControl;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.Controls.DataGridViewX DGV_ZoneInfos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmb_ChooseChannel;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.TextBoxX Txt_TempRiseThres;
        private DevComponents.DotNetBar.Controls.TextBoxX Txt_StartPos;
        private DevComponents.DotNetBar.Controls.TextBoxX Txt_StopPos;
        private DevComponents.DotNetBar.Controls.TextBoxX Txt_RegionTempDifThres;
        private DevComponents.DotNetBar.Controls.TextBoxX Txt_ConstTempThres;
        private DevComponents.DotNetBar.Controls.TextBoxX Txt_ZoneName;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.TabItem tabItem1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmb_chooseEquip;
        private System.Windows.Forms.Label label3;
    }
}