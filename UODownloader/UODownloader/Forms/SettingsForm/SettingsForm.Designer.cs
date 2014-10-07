namespace UODownloader.Forms
{
	partial class SettingsForm
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
			this.groupBox23 = new groupBox2();
			this.label3 = new System.Windows.Forms.Label();
			this.BTN_GameDIR = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.groupBox22 = new groupBox2();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.groupBox21 = new groupBox2();
			this.BTN_Cancel = new System.Windows.Forms.Button();
			this.BTN_OKAY = new System.Windows.Forms.Button();
			this.groupBox23.SuspendLayout();
			this.groupBox22.SuspendLayout();
			this.groupBox21.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox23
			// 
			this.groupBox23.BorderColor = System.Drawing.Color.White;
			this.groupBox23.Controls.Add(this.label3);
			this.groupBox23.Controls.Add(this.BTN_GameDIR);
			this.groupBox23.Controls.Add(this.label2);
			this.groupBox23.Controls.Add(this.textBox2);
			this.groupBox23.Location = new System.Drawing.Point(12, 102);
			this.groupBox23.Name = "groupBox23";
			this.groupBox23.Size = new System.Drawing.Size(526, 134);
			this.groupBox23.TabIndex = 1;
			this.groupBox23.TabStop = false;
			this.groupBox23.Text = "Local Settings";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(216, 42);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(95, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Save File Location";
			// 
			// BTN_GameDIR
			// 
			this.BTN_GameDIR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.BTN_GameDIR.Location = new System.Drawing.Point(17, 37);
			this.BTN_GameDIR.Name = "BTN_GameDIR";
			this.BTN_GameDIR.Size = new System.Drawing.Size(57, 23);
			this.BTN_GameDIR.TabIndex = 4;
			this.BTN_GameDIR.Text = "&Browse";
			this.BTN_GameDIR.UseVisualStyleBackColor = false;
			this.BTN_GameDIR.Click += new System.EventHandler(this.BTN_GameDIR_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 21);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Game Directory";
			// 
			// textBox2
			// 
			this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.textBox2.ForeColor = System.Drawing.Color.Black;
			this.textBox2.Location = new System.Drawing.Point(93, 19);
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox2.Size = new System.Drawing.Size(427, 20);
			this.textBox2.TabIndex = 3;
			// 
			// groupBox22
			// 
			this.groupBox22.BorderColor = System.Drawing.Color.White;
			this.groupBox22.Controls.Add(this.label1);
			this.groupBox22.Controls.Add(this.textBox1);
			this.groupBox22.Location = new System.Drawing.Point(12, 12);
			this.groupBox22.Name = "groupBox22";
			this.groupBox22.Size = new System.Drawing.Size(526, 84);
			this.groupBox22.TabIndex = 1;
			this.groupBox22.TabStop = false;
			this.groupBox22.Text = "Server Settings";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Update URL";
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.textBox1.ForeColor = System.Drawing.Color.Black;
			this.textBox1.Location = new System.Drawing.Point(80, 18);
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(440, 20);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = "http://";
			// 
			// groupBox21
			// 
			this.groupBox21.BorderColor = System.Drawing.Color.White;
			this.groupBox21.Controls.Add(this.BTN_Cancel);
			this.groupBox21.Controls.Add(this.BTN_OKAY);
			this.groupBox21.Location = new System.Drawing.Point(174, 242);
			this.groupBox21.Name = "groupBox21";
			this.groupBox21.Size = new System.Drawing.Size(203, 53);
			this.groupBox21.TabIndex = 1;
			this.groupBox21.TabStop = false;
			// 
			// BTN_Cancel
			// 
			this.BTN_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.BTN_Cancel.Location = new System.Drawing.Point(140, 19);
			this.BTN_Cancel.Name = "BTN_Cancel";
			this.BTN_Cancel.Size = new System.Drawing.Size(57, 23);
			this.BTN_Cancel.TabIndex = 6;
			this.BTN_Cancel.Text = "&Cancel";
			this.BTN_Cancel.UseVisualStyleBackColor = false;
			this.BTN_Cancel.Click += new System.EventHandler(this.BTN_Cancel_Click);
			// 
			// BTN_OKAY
			// 
			this.BTN_OKAY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.BTN_OKAY.Location = new System.Drawing.Point(6, 19);
			this.BTN_OKAY.Name = "BTN_OKAY";
			this.BTN_OKAY.Size = new System.Drawing.Size(57, 23);
			this.BTN_OKAY.TabIndex = 5;
			this.BTN_OKAY.Text = "&Okay";
			this.BTN_OKAY.UseVisualStyleBackColor = false;
			this.BTN_OKAY.Click += new System.EventHandler(this.BTN_OKAY_Click);
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
			this.ClientSize = new System.Drawing.Size(550, 307);
			this.Controls.Add(this.groupBox23);
			this.Controls.Add(this.groupBox22);
			this.Controls.Add(this.groupBox21);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "SettingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Settings";
			this.Load += new System.EventHandler(this.SettingsForm_Load);
			this.groupBox23.ResumeLayout(false);
			this.groupBox23.PerformLayout();
			this.groupBox22.ResumeLayout(false);
			this.groupBox22.PerformLayout();
			this.groupBox21.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BTN_OKAY;
		private System.Windows.Forms.Button BTN_Cancel;
		private groupBox2 groupBox21;
		private groupBox2 groupBox22;
		private groupBox2 groupBox23;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button BTN_GameDIR;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label3;
	}
}