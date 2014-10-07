namespace POLLaunch.Configuration
{
	partial class ConfigurationForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.BTN_BrowseEcompileEXEPath = new System.Windows.Forms.Button();
			this.TB_ECompileEXEPath = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.BTN_BrowseUOCnvrtEXEPath = new System.Windows.Forms.Button();
			this.TB_UOCnvrtEXEPath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.BTN_BrowsePOLEXEPath = new System.Windows.Forms.Button();
			this.TB_POLEXEPath = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.BTN_BrowsePOLPath = new System.Windows.Forms.Button();
			this.TB_POLPath = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.TB_POLTabShutdown = new System.Windows.Forms.TextBox();
			this.BTN_OKAY = new System.Windows.Forms.Button();
			this.BTN_Cancel = new System.Windows.Forms.Button();
			this.BTN_Apply = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(634, 414);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
			this.tabPage1.Controls.Add(this.panel1);
			this.tabPage1.Controls.Add(this.BTN_OKAY);
			this.tabPage1.Controls.Add(this.BTN_Cancel);
			this.tabPage1.Controls.Add(this.BTN_Apply);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(626, 388);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Paths";
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.groupBox2);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(620, 348);
			this.panel1.TabIndex = 3;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.BTN_BrowseEcompileEXEPath);
			this.groupBox2.Controls.Add(this.TB_ECompileEXEPath);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.BTN_BrowseUOCnvrtEXEPath);
			this.groupBox2.Controls.Add(this.TB_UOCnvrtEXEPath);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.BTN_BrowsePOLEXEPath);
			this.groupBox2.Controls.Add(this.TB_POLEXEPath);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.BTN_BrowsePOLPath);
			this.groupBox2.Controls.Add(this.TB_POLPath);
			this.groupBox2.Location = new System.Drawing.Point(4, 85);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(588, 260);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "POL";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(372, 202);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(209, 13);
			this.label7.TabIndex = 13;
			this.label7.Text = "(Should be in the pol root /scripts directory)";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(403, 142);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(178, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "(Should be in the POL root directory)";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(87, 202);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(233, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Path to the ECompile executable (Ecompile.exe)";
			// 
			// BTN_BrowseEcompileEXEPath
			// 
			this.BTN_BrowseEcompileEXEPath.Location = new System.Drawing.Point(6, 197);
			this.BTN_BrowseEcompileEXEPath.Name = "BTN_BrowseEcompileEXEPath";
			this.BTN_BrowseEcompileEXEPath.Size = new System.Drawing.Size(75, 23);
			this.BTN_BrowseEcompileEXEPath.TabIndex = 10;
			this.BTN_BrowseEcompileEXEPath.Text = "Browse";
			this.BTN_BrowseEcompileEXEPath.UseVisualStyleBackColor = true;
			this.BTN_BrowseEcompileEXEPath.Click += new System.EventHandler(this.BTN_BrowseEcompileEXEPath_Click);
			// 
			// TB_ECompileEXEPath
			// 
			this.TB_ECompileEXEPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
			this.TB_ECompileEXEPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TB_ECompileEXEPath.Location = new System.Drawing.Point(6, 226);
			this.TB_ECompileEXEPath.Name = "TB_ECompileEXEPath";
			this.TB_ECompileEXEPath.Size = new System.Drawing.Size(575, 20);
			this.TB_ECompileEXEPath.TabIndex = 9;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(88, 142);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(252, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Path to the UOConvert executable (UOConvert.exe)";
			// 
			// BTN_BrowseUOCnvrtEXEPath
			// 
			this.BTN_BrowseUOCnvrtEXEPath.Location = new System.Drawing.Point(7, 137);
			this.BTN_BrowseUOCnvrtEXEPath.Name = "BTN_BrowseUOCnvrtEXEPath";
			this.BTN_BrowseUOCnvrtEXEPath.Size = new System.Drawing.Size(75, 23);
			this.BTN_BrowseUOCnvrtEXEPath.TabIndex = 7;
			this.BTN_BrowseUOCnvrtEXEPath.Text = "Browse";
			this.BTN_BrowseUOCnvrtEXEPath.UseVisualStyleBackColor = true;
			this.BTN_BrowseUOCnvrtEXEPath.Click += new System.EventHandler(this.BTN_BrowseUOCnvrtEXEPath_Click);
			// 
			// TB_UOCnvrtEXEPath
			// 
			this.TB_UOCnvrtEXEPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
			this.TB_UOCnvrtEXEPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TB_UOCnvrtEXEPath.Location = new System.Drawing.Point(6, 166);
			this.TB_UOCnvrtEXEPath.Name = "TB_UOCnvrtEXEPath";
			this.TB_UOCnvrtEXEPath.Size = new System.Drawing.Size(575, 20);
			this.TB_UOCnvrtEXEPath.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(87, 82);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(188, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Path to the POL executable (POL.exe)";
			// 
			// BTN_BrowsePOLEXEPath
			// 
			this.BTN_BrowsePOLEXEPath.Location = new System.Drawing.Point(6, 77);
			this.BTN_BrowsePOLEXEPath.Name = "BTN_BrowsePOLEXEPath";
			this.BTN_BrowsePOLEXEPath.Size = new System.Drawing.Size(75, 23);
			this.BTN_BrowsePOLEXEPath.TabIndex = 4;
			this.BTN_BrowsePOLEXEPath.Text = "Browse";
			this.BTN_BrowsePOLEXEPath.UseVisualStyleBackColor = true;
			this.BTN_BrowsePOLEXEPath.Click += new System.EventHandler(this.BTN_BrowsePOLEXEPath_Click);
			// 
			// TB_POLEXEPath
			// 
			this.TB_POLEXEPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
			this.TB_POLEXEPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TB_POLEXEPath.Location = new System.Drawing.Point(6, 106);
			this.TB_POLEXEPath.Name = "TB_POLEXEPath";
			this.TB_POLEXEPath.Size = new System.Drawing.Size(575, 20);
			this.TB_POLEXEPath.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(88, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(139, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Path to POL\'s root directory.";
			// 
			// BTN_BrowsePOLPath
			// 
			this.BTN_BrowsePOLPath.Location = new System.Drawing.Point(7, 17);
			this.BTN_BrowsePOLPath.Name = "BTN_BrowsePOLPath";
			this.BTN_BrowsePOLPath.Size = new System.Drawing.Size(75, 23);
			this.BTN_BrowsePOLPath.TabIndex = 1;
			this.BTN_BrowsePOLPath.Text = "Browse";
			this.BTN_BrowsePOLPath.UseVisualStyleBackColor = true;
			this.BTN_BrowsePOLPath.Click += new System.EventHandler(this.BTN_BrowsePOLPath_Click);
			// 
			// TB_POLPath
			// 
			this.TB_POLPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
			this.TB_POLPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TB_POLPath.Location = new System.Drawing.Point(7, 46);
			this.TB_POLPath.Name = "TB_POLPath";
			this.TB_POLPath.Size = new System.Drawing.Size(575, 20);
			this.TB_POLPath.TabIndex = 0;
			this.TB_POLPath.TextChanged += new System.EventHandler(this.TB_POLPath_TextChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.TB_POLTabShutdown);
			this.groupBox1.Location = new System.Drawing.Point(4, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(588, 75);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "POL Tab";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(177, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Letter to send to POL for Shutdown:";
			// 
			// TB_POLTabShutdown
			// 
			this.TB_POLTabShutdown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
			this.TB_POLTabShutdown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TB_POLTabShutdown.Location = new System.Drawing.Point(190, 17);
			this.TB_POLTabShutdown.MaxLength = 1;
			this.TB_POLTabShutdown.Name = "TB_POLTabShutdown";
			this.TB_POLTabShutdown.Size = new System.Drawing.Size(24, 20);
			this.TB_POLTabShutdown.TabIndex = 0;
			// 
			// BTN_OKAY
			// 
			this.BTN_OKAY.Location = new System.Drawing.Point(195, 357);
			this.BTN_OKAY.Name = "BTN_OKAY";
			this.BTN_OKAY.Size = new System.Drawing.Size(75, 23);
			this.BTN_OKAY.TabIndex = 2;
			this.BTN_OKAY.Text = "Okay";
			this.BTN_OKAY.UseVisualStyleBackColor = true;
			this.BTN_OKAY.Click += new System.EventHandler(this.BTN_OKAY_Click);
			// 
			// BTN_Cancel
			// 
			this.BTN_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BTN_Cancel.Location = new System.Drawing.Point(357, 357);
			this.BTN_Cancel.Name = "BTN_Cancel";
			this.BTN_Cancel.Size = new System.Drawing.Size(75, 23);
			this.BTN_Cancel.TabIndex = 1;
			this.BTN_Cancel.Text = "Cancel";
			this.BTN_Cancel.UseVisualStyleBackColor = true;
			// 
			// BTN_Apply
			// 
			this.BTN_Apply.Location = new System.Drawing.Point(276, 357);
			this.BTN_Apply.Name = "BTN_Apply";
			this.BTN_Apply.Size = new System.Drawing.Size(75, 23);
			this.BTN_Apply.TabIndex = 0;
			this.BTN_Apply.Text = "Apply";
			this.BTN_Apply.UseVisualStyleBackColor = true;
			this.BTN_Apply.Click += new System.EventHandler(this.BTN_Apply_Click);
			// 
			// ConfigurationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
			this.CancelButton = this.BTN_Cancel;
			this.ClientSize = new System.Drawing.Size(634, 414);
			this.Controls.Add(this.tabControl1);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "ConfigurationForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Tool Configuration";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.ConfigurationForm_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Button BTN_OKAY;
		private System.Windows.Forms.Button BTN_Cancel;
		private System.Windows.Forms.Button BTN_Apply;
		private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button BTN_BrowsePOLPath;
		private System.Windows.Forms.TextBox TB_POLPath;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button BTN_BrowseUOCnvrtEXEPath;
		private System.Windows.Forms.TextBox TB_UOCnvrtEXEPath;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button BTN_BrowsePOLEXEPath;
		private System.Windows.Forms.TextBox TB_POLEXEPath;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button BTN_BrowseEcompileEXEPath;
		private System.Windows.Forms.TextBox TB_ECompileEXEPath;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_POLTabShutdown;

	}
}