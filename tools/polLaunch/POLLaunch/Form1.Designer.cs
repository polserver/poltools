namespace POLLaunch
{
	partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.BTN_RunTests = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.BTN_StartPOL = new System.Windows.Forms.Button();
            this.txtPOLConsole = new System.Windows.Forms.TextBox();
            this.TB_UOCOutput = new System.Windows.Forms.TextBox();
            this.UOConvertGroupBox = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CB_TileData = new System.Windows.Forms.CheckBox();
            this.CB_LandTiles = new System.Windows.Forms.CheckBox();
            this.CB_Multis = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.CB_MalMap = new System.Windows.Forms.CheckBox();
            this.CB_TokMap = new System.Windows.Forms.CheckBox();
            this.CB_IlshMap = new System.Windows.Forms.CheckBox();
            this.CB_TramMap = new System.Windows.Forms.CheckBox();
            this.CB_BritT2AMap = new System.Windows.Forms.CheckBox();
            this.CB_BritMLMap = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CB_BritT2ADif = new System.Windows.Forms.CheckBox();
            this.CB_BritMLDif = new System.Windows.Forms.CheckBox();
            this.CB_TramDif = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CB_IlshDif = new System.Windows.Forms.CheckBox();
            this.CB_MalDif = new System.Windows.Forms.CheckBox();
            this.CB_TokDif = new System.Windows.Forms.CheckBox();
            this.BTN_UOConvert = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.UOConvertGroupBox.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "POL Launcher";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(104, 26);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem1.Text = "E&xit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(684, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.configurationToolStripMenuItem.Text = "&Configuration";
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.configurationToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 442);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(684, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(684, 418);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
            this.tabPage1.Controls.Add(this.textBox3);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(676, 392);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Introduction";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.Location = new System.Drawing.Point(9, 7);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(659, 341);
            this.textBox3.TabIndex = 2;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 362);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(243, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "In future instances, go straight to the POL tab.";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckStateChanged += new System.EventHandler(this.checkBox1_CheckStateChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
            this.tabPage2.Controls.Add(this.BTN_RunTests);
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(676, 392);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Initial Checks";
            // 
            // BTN_RunTests
            // 
            this.BTN_RunTests.Location = new System.Drawing.Point(301, 363);
            this.BTN_RunTests.Name = "BTN_RunTests";
            this.BTN_RunTests.Size = new System.Drawing.Size(75, 23);
            this.BTN_RunTests.TabIndex = 1;
            this.BTN_RunTests.Text = "Run Tests";
            this.BTN_RunTests.UseVisualStyleBackColor = true;
            this.BTN_RunTests.Click += new System.EventHandler(this.BTN_RunTests_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.textBox1.ForeColor = System.Drawing.Color.Black;
            this.textBox1.Location = new System.Drawing.Point(7, 6);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(661, 351);
            this.textBox1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
            this.tabPage3.Controls.Add(this.BTN_UOConvert);
            this.tabPage3.Controls.Add(this.UOConvertGroupBox);
            this.tabPage3.Controls.Add(this.TB_UOCOutput);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(676, 392);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "UOConvert";
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(676, 392);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "ECompile";
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(676, 392);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "POL Setup";
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
            this.tabPage6.Controls.Add(this.BTN_StartPOL);
            this.tabPage6.Controls.Add(this.txtPOLConsole);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(676, 392);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "POL";
            // 
            // BTN_StartPOL
            // 
            this.BTN_StartPOL.Location = new System.Drawing.Point(301, 358);
            this.BTN_StartPOL.Name = "BTN_StartPOL";
            this.BTN_StartPOL.Size = new System.Drawing.Size(75, 23);
            this.BTN_StartPOL.TabIndex = 2;
            this.BTN_StartPOL.Text = "Start POL";
            this.BTN_StartPOL.UseVisualStyleBackColor = true;
            this.BTN_StartPOL.Click += new System.EventHandler(this.BTN_StartPOL_Click);
            // 
            // txtPOLConsole
            // 
            this.txtPOLConsole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.txtPOLConsole.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPOLConsole.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.txtPOLConsole.ForeColor = System.Drawing.Color.Black;
            this.txtPOLConsole.Location = new System.Drawing.Point(8, 3);
            this.txtPOLConsole.Multiline = true;
            this.txtPOLConsole.Name = "txtPOLConsole";
            this.txtPOLConsole.Size = new System.Drawing.Size(660, 342);
            this.txtPOLConsole.TabIndex = 1;
            this.txtPOLConsole.TextChanged += new System.EventHandler(this.txtPOLConsole_TextChanged);
            this.txtPOLConsole.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPOLConsole_KeyUp);
            this.txtPOLConsole.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPOLConsole_KeyPress);
            // 
            // TB_UOCOutput
            // 
            this.TB_UOCOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.TB_UOCOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_UOCOutput.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.TB_UOCOutput.ForeColor = System.Drawing.Color.Black;
            this.TB_UOCOutput.Location = new System.Drawing.Point(7, 6);
            this.TB_UOCOutput.Multiline = true;
            this.TB_UOCOutput.Name = "TB_UOCOutput";
            this.TB_UOCOutput.ReadOnly = true;
            this.TB_UOCOutput.Size = new System.Drawing.Size(661, 213);
            this.TB_UOCOutput.TabIndex = 2;
            // 
            // UOConvertGroupBox
            // 
            this.UOConvertGroupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
            this.UOConvertGroupBox.Controls.Add(this.panel2);
            this.UOConvertGroupBox.Controls.Add(this.panel1);
            this.UOConvertGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UOConvertGroupBox.Location = new System.Drawing.Point(7, 225);
            this.UOConvertGroupBox.Name = "UOConvertGroupBox";
            this.UOConvertGroupBox.Size = new System.Drawing.Size(661, 132);
            this.UOConvertGroupBox.TabIndex = 3;
            this.UOConvertGroupBox.TabStop = false;
            this.UOConvertGroupBox.Text = "UOConvert Controls";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.CB_TileData);
            this.panel2.Controls.Add(this.CB_LandTiles);
            this.panel2.Controls.Add(this.CB_Multis);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(414, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(239, 107);
            this.panel2.TabIndex = 7;
            // 
            // CB_TileData
            // 
            this.CB_TileData.AutoSize = true;
            this.CB_TileData.Location = new System.Drawing.Point(15, 31);
            this.CB_TileData.Name = "CB_TileData";
            this.CB_TileData.Size = new System.Drawing.Size(69, 17);
            this.CB_TileData.TabIndex = 3;
            this.CB_TileData.Text = "Tile Data";
            this.CB_TileData.UseVisualStyleBackColor = true;
            // 
            // CB_LandTiles
            // 
            this.CB_LandTiles.AutoSize = true;
            this.CB_LandTiles.Location = new System.Drawing.Point(149, 31);
            this.CB_LandTiles.Name = "CB_LandTiles";
            this.CB_LandTiles.Size = new System.Drawing.Size(75, 17);
            this.CB_LandTiles.TabIndex = 2;
            this.CB_LandTiles.Text = "Land Tiles";
            this.CB_LandTiles.UseVisualStyleBackColor = true;
            // 
            // CB_Multis
            // 
            this.CB_Multis.AutoSize = true;
            this.CB_Multis.Location = new System.Drawing.Point(90, 31);
            this.CB_Multis.Name = "CB_Multis";
            this.CB_Multis.Size = new System.Drawing.Size(53, 17);
            this.CB_Multis.TabIndex = 1;
            this.CB_Multis.Text = "Multis";
            this.CB_Multis.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(47, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Config Files to Generate";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CB_TokDif);
            this.panel1.Controls.Add(this.CB_MalDif);
            this.panel1.Controls.Add(this.CB_IlshDif);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.CB_TramDif);
            this.panel1.Controls.Add(this.CB_BritMLDif);
            this.panel1.Controls.Add(this.CB_BritT2ADif);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.CB_MalMap);
            this.panel1.Controls.Add(this.CB_TokMap);
            this.panel1.Controls.Add(this.CB_IlshMap);
            this.panel1.Controls.Add(this.CB_TramMap);
            this.panel1.Controls.Add(this.CB_BritT2AMap);
            this.panel1.Controls.Add(this.CB_BritMLMap);
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(402, 107);
            this.panel1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(149, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Maps To Convert";
            // 
            // CB_MalMap
            // 
            this.CB_MalMap.AutoSize = true;
            this.CB_MalMap.Location = new System.Drawing.Point(213, 53);
            this.CB_MalMap.Name = "CB_MalMap";
            this.CB_MalMap.Size = new System.Drawing.Size(15, 14);
            this.CB_MalMap.TabIndex = 4;
            this.CB_MalMap.UseVisualStyleBackColor = true;
            // 
            // CB_TokMap
            // 
            this.CB_TokMap.AutoSize = true;
            this.CB_TokMap.Location = new System.Drawing.Point(213, 73);
            this.CB_TokMap.Name = "CB_TokMap";
            this.CB_TokMap.Size = new System.Drawing.Size(15, 14);
            this.CB_TokMap.TabIndex = 5;
            this.CB_TokMap.UseVisualStyleBackColor = true;
            // 
            // CB_IlshMap
            // 
            this.CB_IlshMap.AutoSize = true;
            this.CB_IlshMap.Location = new System.Drawing.Point(213, 34);
            this.CB_IlshMap.Name = "CB_IlshMap";
            this.CB_IlshMap.Size = new System.Drawing.Size(15, 14);
            this.CB_IlshMap.TabIndex = 3;
            this.CB_IlshMap.UseVisualStyleBackColor = true;
            // 
            // CB_TramMap
            // 
            this.CB_TramMap.AutoSize = true;
            this.CB_TramMap.Location = new System.Drawing.Point(7, 74);
            this.CB_TramMap.Name = "CB_TramMap";
            this.CB_TramMap.Size = new System.Drawing.Size(15, 14);
            this.CB_TramMap.TabIndex = 2;
            this.CB_TramMap.UseVisualStyleBackColor = true;
            // 
            // CB_BritT2AMap
            // 
            this.CB_BritT2AMap.AutoSize = true;
            this.CB_BritT2AMap.Location = new System.Drawing.Point(7, 34);
            this.CB_BritT2AMap.Name = "CB_BritT2AMap";
            this.CB_BritT2AMap.Size = new System.Drawing.Size(15, 14);
            this.CB_BritT2AMap.TabIndex = 0;
            this.CB_BritT2AMap.UseVisualStyleBackColor = true;
            this.CB_BritT2AMap.CheckedChanged += new System.EventHandler(this.CB_BritT2AMap_CheckedChanged);
            // 
            // CB_BritMLMap
            // 
            this.CB_BritMLMap.AutoSize = true;
            this.CB_BritMLMap.Location = new System.Drawing.Point(7, 54);
            this.CB_BritMLMap.Name = "CB_BritMLMap";
            this.CB_BritMLMap.Size = new System.Drawing.Size(15, 14);
            this.CB_BritMLMap.TabIndex = 1;
            this.CB_BritMLMap.UseVisualStyleBackColor = true;
            this.CB_BritMLMap.CheckedChanged += new System.EventHandler(this.CB_BritMLMap_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Map";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "UseDif";
            // 
            // CB_BritT2ADif
            // 
            this.CB_BritT2ADif.AutoSize = true;
            this.CB_BritT2ADif.Location = new System.Drawing.Point(41, 34);
            this.CB_BritT2ADif.Name = "CB_BritT2ADif";
            this.CB_BritT2ADif.Size = new System.Drawing.Size(143, 17);
            this.CB_BritT2ADif.TabIndex = 9;
            this.CB_BritT2ADif.Text = "Britannia (T2A Size Map)";
            this.CB_BritT2ADif.UseVisualStyleBackColor = true;
            // 
            // CB_BritMLDif
            // 
            this.CB_BritMLDif.AutoSize = true;
            this.CB_BritMLDif.Location = new System.Drawing.Point(41, 53);
            this.CB_BritMLDif.Name = "CB_BritMLDif";
            this.CB_BritMLDif.Size = new System.Drawing.Size(138, 17);
            this.CB_BritMLDif.TabIndex = 10;
            this.CB_BritMLDif.Text = "Britannia (ML Size Map)";
            this.CB_BritMLDif.UseVisualStyleBackColor = true;
            // 
            // CB_TramDif
            // 
            this.CB_TramDif.AutoSize = true;
            this.CB_TramDif.Location = new System.Drawing.Point(41, 74);
            this.CB_TramDif.Name = "CB_TramDif";
            this.CB_TramDif.Size = new System.Drawing.Size(66, 17);
            this.CB_TramDif.TabIndex = 11;
            this.CB_TramDif.Text = "Trammel";
            this.CB_TramDif.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(210, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Map";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(236, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "UseDif";
            // 
            // CB_IlshDif
            // 
            this.CB_IlshDif.AutoSize = true;
            this.CB_IlshDif.Location = new System.Drawing.Point(247, 34);
            this.CB_IlshDif.Name = "CB_IlshDif";
            this.CB_IlshDif.Size = new System.Drawing.Size(63, 17);
            this.CB_IlshDif.TabIndex = 14;
            this.CB_IlshDif.Text = "Ilshenar";
            this.CB_IlshDif.UseVisualStyleBackColor = true;
            // 
            // CB_MalDif
            // 
            this.CB_MalDif.AutoSize = true;
            this.CB_MalDif.Location = new System.Drawing.Point(247, 54);
            this.CB_MalDif.Name = "CB_MalDif";
            this.CB_MalDif.Size = new System.Drawing.Size(54, 17);
            this.CB_MalDif.TabIndex = 15;
            this.CB_MalDif.Text = "Malas";
            this.CB_MalDif.UseVisualStyleBackColor = true;
            // 
            // CB_TokDif
            // 
            this.CB_TokDif.AutoSize = true;
            this.CB_TokDif.Location = new System.Drawing.Point(247, 73);
            this.CB_TokDif.Name = "CB_TokDif";
            this.CB_TokDif.Size = new System.Drawing.Size(63, 17);
            this.CB_TokDif.TabIndex = 16;
            this.CB_TokDif.Text = "Tokuno";
            this.CB_TokDif.UseVisualStyleBackColor = true;
            // 
            // BTN_UOConvert
            // 
            this.BTN_UOConvert.Location = new System.Drawing.Point(301, 363);
            this.BTN_UOConvert.Name = "BTN_UOConvert";
            this.BTN_UOConvert.Size = new System.Drawing.Size(75, 23);
            this.BTN_UOConvert.TabIndex = 4;
            this.BTN_UOConvert.Text = "Convert";
            this.BTN_UOConvert.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.ClientSize = new System.Drawing.Size(684, 464);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "POL Launch";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.UOConvertGroupBox.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button BTN_RunTests;
        private System.Windows.Forms.TextBox txtPOLConsole;
        private System.Windows.Forms.Button BTN_StartPOL;
		private System.Windows.Forms.TextBox textBox3;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox UOConvertGroupBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox CB_TileData;
        private System.Windows.Forms.CheckBox CB_LandTiles;
        private System.Windows.Forms.CheckBox CB_Multis;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox CB_MalMap;
        private System.Windows.Forms.CheckBox CB_TokMap;
        private System.Windows.Forms.CheckBox CB_IlshMap;
        private System.Windows.Forms.CheckBox CB_TramMap;
        private System.Windows.Forms.CheckBox CB_BritT2AMap;
        private System.Windows.Forms.CheckBox CB_BritMLMap;
        public System.Windows.Forms.TextBox TB_UOCOutput;
        private System.Windows.Forms.CheckBox CB_BritT2ADif;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox CB_BritMLDif;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox CB_TramDif;
        private System.Windows.Forms.CheckBox CB_IlshDif;
        private System.Windows.Forms.CheckBox CB_TokDif;
        private System.Windows.Forms.CheckBox CB_MalDif;
        private System.Windows.Forms.Button BTN_UOConvert;
	}
}

