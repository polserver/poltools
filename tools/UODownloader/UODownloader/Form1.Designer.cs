namespace UODownloader
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
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.notifyIconContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.BTN_UpdateNews = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.groupBox22 = new groupBox2();
			this.label16 = new System.Windows.Forms.Label();
			this.progressBar2 = new System.Windows.Forms.ProgressBar();
			this.groupBox21 = new groupBox2();
			this.label7 = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.BTN_Stop = new System.Windows.Forms.Button();
			this.BTN_Resume = new System.Windows.Forms.Button();
			this.BTN_Pause = new System.Windows.Forms.Button();
			this.BTN_Start = new System.Windows.Forms.Button();
			this.notifyIconContextMenuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox22.SuspendLayout();
			this.groupBox21.SuspendLayout();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.ContextMenuStrip = this.notifyIconContextMenuStrip1;
			this.notifyIcon1.Text = "UO Downloader";
			this.notifyIcon1.Visible = true;
			// 
			// notifyIconContextMenuStrip1
			// 
			this.notifyIconContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem1});
			this.notifyIconContextMenuStrip1.Name = "notifyIconContextMenuStrip1";
			this.notifyIconContextMenuStrip1.Size = new System.Drawing.Size(93, 26);
			// 
			// exitToolStripMenuItem1
			// 
			this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
			this.exitToolStripMenuItem1.Size = new System.Drawing.Size(92, 22);
			this.exitToolStripMenuItem1.Text = "&Exit";
			this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 449);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(656, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Transparent;
			this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
			this.toolStripStatusLabel1.Text = "Ready";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(656, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.settingsToolStripMenuItem.Text = "&Settings";
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.exitToolStripMenuItem.Text = "&Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.aboutToolStripMenuItem.Text = "&About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 24);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(656, 425);
			this.tabControl1.TabIndex = 2;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
			this.tabPage1.Controls.Add(this.BTN_UpdateNews);
			this.tabPage1.Controls.Add(this.textBox1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(648, 399);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Update News";
			// 
			// BTN_UpdateNews
			// 
			this.BTN_UpdateNews.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.BTN_UpdateNews.Location = new System.Drawing.Point(270, 370);
			this.BTN_UpdateNews.Name = "BTN_UpdateNews";
			this.BTN_UpdateNews.Size = new System.Drawing.Size(108, 23);
			this.BTN_UpdateNews.TabIndex = 10;
			this.BTN_UpdateNews.Text = "Get Update news";
			this.BTN_UpdateNews.UseVisualStyleBackColor = false;
			this.BTN_UpdateNews.Click += new System.EventHandler(this.BTN_UpdateNews_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.textBox1.ForeColor = System.Drawing.Color.Black;
			this.textBox1.Location = new System.Drawing.Point(8, 6);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(632, 358);
			this.textBox1.TabIndex = 9;
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
			this.tabPage2.Controls.Add(this.groupBox22);
			this.tabPage2.Controls.Add(this.groupBox21);
			this.tabPage2.Controls.Add(this.label6);
			this.tabPage2.Controls.Add(this.label5);
			this.tabPage2.Controls.Add(this.label4);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Controls.Add(this.label2);
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Controls.Add(this.textBox2);
			this.tabPage2.Controls.Add(this.BTN_Stop);
			this.tabPage2.Controls.Add(this.BTN_Resume);
			this.tabPage2.Controls.Add(this.BTN_Pause);
			this.tabPage2.Controls.Add(this.BTN_Start);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(648, 399);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Download Updates";
			// 
			// groupBox22
			// 
			this.groupBox22.BorderColor = System.Drawing.Color.White;
			this.groupBox22.Controls.Add(this.label16);
			this.groupBox22.Controls.Add(this.progressBar2);
			this.groupBox22.Location = new System.Drawing.Point(7, 278);
			this.groupBox22.Name = "groupBox22";
			this.groupBox22.Size = new System.Drawing.Size(635, 81);
			this.groupBox22.TabIndex = 25;
			this.groupBox22.TabStop = false;
			this.groupBox22.Text = "Overall Progress";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(7, 50);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(67, 13);
			this.label16.TabIndex = 1;
			this.label16.Text = "Downloaded";
			// 
			// progressBar2
			// 
			this.progressBar2.Location = new System.Drawing.Point(6, 20);
			this.progressBar2.Name = "progressBar2";
			this.progressBar2.Size = new System.Drawing.Size(623, 23);
			this.progressBar2.TabIndex = 0;
			// 
			// groupBox21
			// 
			this.groupBox21.BorderColor = System.Drawing.Color.White;
			this.groupBox21.Controls.Add(this.label7);
			this.groupBox21.Controls.Add(this.progressBar1);
			this.groupBox21.Location = new System.Drawing.Point(7, 191);
			this.groupBox21.Name = "groupBox21";
			this.groupBox21.Size = new System.Drawing.Size(635, 81);
			this.groupBox21.TabIndex = 24;
			this.groupBox21.TabStop = false;
			this.groupBox21.Text = "File Progress";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(7, 50);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(67, 13);
			this.label7.TabIndex = 1;
			this.label7.Text = "Downloaded";
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(6, 20);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(623, 23);
			this.progressBar1.TabIndex = 0;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(206, 130);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(33, 13);
			this.label6.TabIndex = 23;
			this.label6.Text = "[Size]";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(170, 130);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(30, 13);
			this.label5.TabIndex = 22;
			this.label5.Text = "Size:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(22, 161);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(97, 13);
			this.label4.TabIndex = 21;
			this.label4.Text = "[Destination Name]";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(22, 146);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(78, 13);
			this.label3.TabIndex = 20;
			this.label3.Text = "[Source Name]";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 130);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 13);
			this.label2.TabIndex = 19;
			this.label2.Text = "Current Download";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 13);
			this.label1.TabIndex = 17;
			this.label1.Text = "Download List:";
			// 
			// textBox2
			// 
			this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.textBox2.ForeColor = System.Drawing.Color.Black;
			this.textBox2.Location = new System.Drawing.Point(6, 23);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox2.Size = new System.Drawing.Size(632, 101);
			this.textBox2.TabIndex = 16;
			// 
			// BTN_Stop
			// 
			this.BTN_Stop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.BTN_Stop.Enabled = false;
			this.BTN_Stop.Location = new System.Drawing.Point(410, 365);
			this.BTN_Stop.Name = "BTN_Stop";
			this.BTN_Stop.Size = new System.Drawing.Size(77, 23);
			this.BTN_Stop.TabIndex = 15;
			this.BTN_Stop.Text = "Stop";
			this.BTN_Stop.UseVisualStyleBackColor = false;
			this.BTN_Stop.Click += new System.EventHandler(this.BTN_Stop_Click);
			// 
			// BTN_Resume
			// 
			this.BTN_Resume.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.BTN_Resume.Enabled = false;
			this.BTN_Resume.Location = new System.Drawing.Point(327, 365);
			this.BTN_Resume.Name = "BTN_Resume";
			this.BTN_Resume.Size = new System.Drawing.Size(77, 23);
			this.BTN_Resume.TabIndex = 14;
			this.BTN_Resume.Text = "Resume";
			this.BTN_Resume.UseVisualStyleBackColor = false;
			this.BTN_Resume.Click += new System.EventHandler(this.BTN_Resume_Click);
			// 
			// BTN_Pause
			// 
			this.BTN_Pause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.BTN_Pause.Enabled = false;
			this.BTN_Pause.Location = new System.Drawing.Point(244, 365);
			this.BTN_Pause.Name = "BTN_Pause";
			this.BTN_Pause.Size = new System.Drawing.Size(77, 23);
			this.BTN_Pause.TabIndex = 13;
			this.BTN_Pause.Text = "Pause";
			this.BTN_Pause.UseVisualStyleBackColor = false;
			this.BTN_Pause.Click += new System.EventHandler(this.BTN_Pause_Click);
			// 
			// BTN_Start
			// 
			this.BTN_Start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.BTN_Start.Location = new System.Drawing.Point(161, 365);
			this.BTN_Start.Name = "BTN_Start";
			this.BTN_Start.Size = new System.Drawing.Size(77, 23);
			this.BTN_Start.TabIndex = 12;
			this.BTN_Start.Text = "Start";
			this.BTN_Start.UseVisualStyleBackColor = false;
			this.BTN_Start.Click += new System.EventHandler(this.BTN_Start_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
			this.ClientSize = new System.Drawing.Size(656, 471);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.ForeColor = System.Drawing.Color.Black;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "UO Downloader";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.notifyIconContextMenuStrip1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.groupBox22.ResumeLayout(false);
			this.groupBox22.PerformLayout();
			this.groupBox21.ResumeLayout(false);
			this.groupBox21.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip notifyIconContextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button BTN_UpdateNews;
		private System.Windows.Forms.Button BTN_Start;
		private System.Windows.Forms.Button BTN_Stop;
		private System.Windows.Forms.Button BTN_Resume;
		private System.Windows.Forms.Button BTN_Pause;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private groupBox2 groupBox21;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label label7;
		private groupBox2 groupBox22;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.ProgressBar progressBar2;
	}
}

