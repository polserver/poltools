namespace CraftTool.Forms.SettingsForm
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
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.TB_pol_path = new System.Windows.Forms.TextBox();
			this.BTN_pol_path_browse = new System.Windows.Forms.Button();
			this.BTN_apply = new System.Windows.Forms.Button();
			this.BTN_ok = new System.Windows.Forms.Button();
			this.BTN_cancel = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.TB_uol_path = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.BTN_uol_path_browse = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "POL Path";
			// 
			// TB_pol_path
			// 
			this.TB_pol_path.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TB_pol_path.Location = new System.Drawing.Point(73, 12);
			this.TB_pol_path.Name = "TB_pol_path";
			this.TB_pol_path.Size = new System.Drawing.Size(594, 20);
			this.TB_pol_path.TabIndex = 1;
			this.toolTip1.SetToolTip(this.TB_pol_path, "If blank or invalid - Program will assume root directory is where this tool is lo" +
				   "cated at.");
			// 
			// BTN_pol_path_browse
			// 
			this.BTN_pol_path_browse.Location = new System.Drawing.Point(673, 9);
			this.BTN_pol_path_browse.Name = "BTN_pol_path_browse";
			this.BTN_pol_path_browse.Size = new System.Drawing.Size(26, 23);
			this.BTN_pol_path_browse.TabIndex = 2;
			this.BTN_pol_path_browse.Text = "...";
			this.BTN_pol_path_browse.UseVisualStyleBackColor = true;
			this.BTN_pol_path_browse.Click += new System.EventHandler(this.BTN_pol_path_browse_Click);
			// 
			// BTN_apply
			// 
			this.BTN_apply.Location = new System.Drawing.Point(318, 131);
			this.BTN_apply.Name = "BTN_apply";
			this.BTN_apply.Size = new System.Drawing.Size(75, 23);
			this.BTN_apply.TabIndex = 3;
			this.BTN_apply.Text = "Apply";
			this.BTN_apply.UseVisualStyleBackColor = true;
			this.BTN_apply.Click += new System.EventHandler(this.BTN_apply_Click);
			// 
			// BTN_ok
			// 
			this.BTN_ok.Location = new System.Drawing.Point(237, 131);
			this.BTN_ok.Name = "BTN_ok";
			this.BTN_ok.Size = new System.Drawing.Size(75, 23);
			this.BTN_ok.TabIndex = 4;
			this.BTN_ok.Text = "OK";
			this.BTN_ok.UseVisualStyleBackColor = true;
			this.BTN_ok.Click += new System.EventHandler(this.BTN_ok_Click);
			// 
			// BTN_cancel
			// 
			this.BTN_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BTN_cancel.Location = new System.Drawing.Point(399, 131);
			this.BTN_cancel.Name = "BTN_cancel";
			this.BTN_cancel.Size = new System.Drawing.Size(75, 23);
			this.BTN_cancel.TabIndex = 5;
			this.BTN_cancel.Text = "Cancel";
			this.BTN_cancel.UseVisualStyleBackColor = true;
			this.BTN_cancel.Click += new System.EventHandler(this.BTN_cancel_Click);
			// 
			// TB_uol_path
			// 
			this.TB_uol_path.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TB_uol_path.Location = new System.Drawing.Point(71, 48);
			this.TB_uol_path.Name = "TB_uol_path";
			this.TB_uol_path.Size = new System.Drawing.Size(594, 20);
			this.TB_uol_path.TabIndex = 7;
			this.toolTip1.SetToolTip(this.TB_uol_path, "If blank or invalid - Program will assume root directory is where this tool is lo" +
				   "cated at.");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "UO Path";
			// 
			// BTN_uol_path_browse
			// 
			this.BTN_uol_path_browse.Location = new System.Drawing.Point(673, 45);
			this.BTN_uol_path_browse.Name = "BTN_uol_path_browse";
			this.BTN_uol_path_browse.Size = new System.Drawing.Size(26, 23);
			this.BTN_uol_path_browse.TabIndex = 8;
			this.BTN_uol_path_browse.Text = "...";
			this.BTN_uol_path_browse.UseVisualStyleBackColor = true;
			this.BTN_uol_path_browse.Click += new System.EventHandler(this.BTN_uol_path_browse_Click);
			// 
			// SettingsForm
			// 
			this.AcceptButton = this.BTN_ok;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.BTN_cancel;
			this.ClientSize = new System.Drawing.Size(711, 166);
			this.Controls.Add(this.BTN_uol_path_browse);
			this.Controls.Add(this.TB_uol_path);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.BTN_cancel);
			this.Controls.Add(this.BTN_ok);
			this.Controls.Add(this.BTN_apply);
			this.Controls.Add(this.BTN_pol_path_browse);
			this.Controls.Add(this.TB_pol_path);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "SettingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Settings";
			this.Load += new System.EventHandler(this.SettingsForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TB_pol_path;
		private System.Windows.Forms.Button BTN_pol_path_browse;
		private System.Windows.Forms.Button BTN_apply;
		private System.Windows.Forms.Button BTN_ok;
		private System.Windows.Forms.Button BTN_cancel;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox TB_uol_path;
		private System.Windows.Forms.Button BTN_uol_path_browse;
	}
}