namespace CraftTool.Forms.InputForm
{
	partial class InputForm
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
			this.BTN_OK = new System.Windows.Forms.Button();
			this.BTN_CANCEL = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
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
			// BTN_OK
			// 
			this.BTN_OK.Location = new System.Drawing.Point(82, 69);
			this.BTN_OK.Name = "BTN_OK";
			this.BTN_OK.Size = new System.Drawing.Size(75, 23);
			this.BTN_OK.TabIndex = 1;
			this.BTN_OK.Text = "OK";
			this.BTN_OK.UseVisualStyleBackColor = true;
			this.BTN_OK.Click += new System.EventHandler(this.BTN_OK_Click);
			// 
			// BTN_CANCEL
			// 
			this.BTN_CANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BTN_CANCEL.Location = new System.Drawing.Point(163, 69);
			this.BTN_CANCEL.Name = "BTN_CANCEL";
			this.BTN_CANCEL.Size = new System.Drawing.Size(75, 23);
			this.BTN_CANCEL.TabIndex = 2;
			this.BTN_CANCEL.Text = "Cancel";
			this.BTN_CANCEL.UseVisualStyleBackColor = true;
			this.BTN_CANCEL.Click += new System.EventHandler(this.BTN_CANCEL_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox1.Location = new System.Drawing.Point(13, 35);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(296, 20);
			this.textBox1.TabIndex = 3;
			// 
			// InputForm
			// 
			this.AcceptButton = this.BTN_OK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
			this.CancelButton = this.BTN_CANCEL;
			this.ClientSize = new System.Drawing.Size(321, 104);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.BTN_CANCEL);
			this.Controls.Add(this.BTN_OK);
			this.Controls.Add(this.label1);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Input Request";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button BTN_OK;
		private System.Windows.Forms.Button BTN_CANCEL;
		private System.Windows.Forms.TextBox textBox1;
	}
}