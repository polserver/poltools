namespace CraftTool.Forms.SelectionPicker
{
	partial class SelectionPicker
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
			this.label1 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.BTN_ok = new System.Windows.Forms.Button();
			this.BTN_cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(13, 30);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(216, 21);
			this.comboBox1.Sorted = true;
			this.comboBox1.TabIndex = 1;
			// 
			// BTN_ok
			// 
			this.BTN_ok.Location = new System.Drawing.Point(16, 70);
			this.BTN_ok.Name = "BTN_ok";
			this.BTN_ok.Size = new System.Drawing.Size(75, 23);
			this.BTN_ok.TabIndex = 2;
			this.BTN_ok.Text = "Ok";
			this.BTN_ok.UseVisualStyleBackColor = true;
			this.BTN_ok.Click += new System.EventHandler(this.BTN_ok_Click);
			// 
			// BTN_cancel
			// 
			this.BTN_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BTN_cancel.Location = new System.Drawing.Point(301, 70);
			this.BTN_cancel.Name = "BTN_cancel";
			this.BTN_cancel.Size = new System.Drawing.Size(75, 23);
			this.BTN_cancel.TabIndex = 3;
			this.BTN_cancel.Text = "Cancel";
			this.BTN_cancel.UseVisualStyleBackColor = true;
			this.BTN_cancel.Click += new System.EventHandler(this.BTN_cancel_Click);
			// 
			// SelectionPicker
			// 
			this.AcceptButton = this.BTN_ok;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
			this.CancelButton = this.BTN_cancel;
			this.ClientSize = new System.Drawing.Size(388, 105);
			this.Controls.Add(this.BTN_cancel);
			this.Controls.Add(this.BTN_ok);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SelectionPicker";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SelectionPicker";
			this.Load += new System.EventHandler(this.SelectionPicker_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button BTN_ok;
		private System.Windows.Forms.Button BTN_cancel;
	}
}