/* 	Copyright (c) 2004-2006 Francesco Furiani & Mark Chandler
 *	
 *	Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation 
 *	files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, 
 *	modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
 *	is furnished to do so, subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 *	OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
 *	LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR 
 *	IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

/*
 * Modified by Fernando Rozenblit in 2008 to create POL Scripts.
 * 
 * Rewrote some parts of Class1.cs for a better reading / updating
 * 
 * TODO: Add distro pkg support
 */

using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace POLGumpExport
{
	public class POLExportForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lbl_Help;
		private System.Windows.Forms.GroupBox grp_Gumptype;
		private System.Windows.Forms.RadioButton rbt_Bare;
		private System.Windows.Forms.RadioButton rbt_Distro;
		private System.Windows.Forms.GroupBox grp_Properties;
		private System.Windows.Forms.TextBox txt_Gumpname;
		private System.Windows.Forms.Label lbl_Gumpname;
		private System.Windows.Forms.TextBox txt_Savefile;
		private System.Windows.Forms.GroupBox grp_Saveas;
		private System.Windows.Forms.Button btn_Browse;
		private System.Windows.Forms.Button btn_Cancel;
		private System.Windows.Forms.Button btn_Export;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public string GumpName
		{
			get
			{
				return txt_Gumpname.Text;
			}
		}

		private POLExporter seWorker;

		public POLExportForm(POLExporter seJob)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			seWorker = seJob;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lbl_Help = new System.Windows.Forms.Label();
            this.grp_Gumptype = new System.Windows.Forms.GroupBox();
            this.rbt_Bare = new System.Windows.Forms.RadioButton();
            this.rbt_Distro = new System.Windows.Forms.RadioButton();
            this.grp_Properties = new System.Windows.Forms.GroupBox();
            this.lbl_Gumpname = new System.Windows.Forms.Label();
            this.txt_Gumpname = new System.Windows.Forms.TextBox();
            this.txt_Savefile = new System.Windows.Forms.TextBox();
            this.grp_Saveas = new System.Windows.Forms.GroupBox();
            this.btn_Browse = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_Export = new System.Windows.Forms.Button();
            this.grp_Gumptype.SuspendLayout();
            this.grp_Properties.SuspendLayout();
            this.grp_Saveas.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_Help
            // 
            this.lbl_Help.BackColor = System.Drawing.SystemColors.Info;
            this.lbl_Help.Location = new System.Drawing.Point(0, 0);
            this.lbl_Help.Name = "lbl_Help";
            this.lbl_Help.Size = new System.Drawing.Size(336, 32);
            this.lbl_Help.TabIndex = 0;
            this.lbl_Help.Text = "Select your gump name and where to save the gump.";
            this.lbl_Help.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grp_Gumptype
            // 
            this.grp_Gumptype.Controls.Add(this.rbt_Bare);
            this.grp_Gumptype.Controls.Add(this.rbt_Distro);
            this.grp_Gumptype.Location = new System.Drawing.Point(0, 40);
            this.grp_Gumptype.Name = "grp_Gumptype";
            this.grp_Gumptype.Size = new System.Drawing.Size(160, 80);
            this.grp_Gumptype.TabIndex = 1;
            this.grp_Gumptype.TabStop = false;
            this.grp_Gumptype.Text = "Gump type";
            // 
            // rbt_Bare
            // 
            this.rbt_Bare.Checked = true;
            this.rbt_Bare.Location = new System.Drawing.Point(16, 24);
            this.rbt_Bare.Name = "rbt_Bare";
            this.rbt_Bare.Size = new System.Drawing.Size(128, 16);
            this.rbt_Bare.TabIndex = 3;
            this.rbt_Bare.TabStop = true;
            this.rbt_Bare.Text = "Bare gump";
            // 
            // rbt_Distro
            // 
            this.rbt_Distro.Enabled = false;
            this.rbt_Distro.Location = new System.Drawing.Point(16, 48);
            this.rbt_Distro.Name = "rbt_Distro";
            this.rbt_Distro.Size = new System.Drawing.Size(128, 24);
            this.rbt_Distro.TabIndex = 3;
            this.rbt_Distro.Text = "Distro Gump pkg";
            // 
            // grp_Properties
            // 
            this.grp_Properties.Controls.Add(this.lbl_Gumpname);
            this.grp_Properties.Controls.Add(this.txt_Gumpname);
            this.grp_Properties.Location = new System.Drawing.Point(168, 40);
            this.grp_Properties.Name = "grp_Properties";
            this.grp_Properties.Size = new System.Drawing.Size(160, 80);
            this.grp_Properties.TabIndex = 2;
            this.grp_Properties.TabStop = false;
            this.grp_Properties.Text = "Properties";
            // 
            // lbl_Gumpname
            // 
            this.lbl_Gumpname.Location = new System.Drawing.Point(16, 24);
            this.lbl_Gumpname.Name = "lbl_Gumpname";
            this.lbl_Gumpname.Size = new System.Drawing.Size(136, 16);
            this.lbl_Gumpname.TabIndex = 1;
            this.lbl_Gumpname.Text = "Gump name:";
            // 
            // txt_Gumpname
            // 
            this.txt_Gumpname.Location = new System.Drawing.Point(16, 48);
            this.txt_Gumpname.Name = "txt_Gumpname";
            this.txt_Gumpname.Size = new System.Drawing.Size(136, 20);
            this.txt_Gumpname.TabIndex = 0;
            this.txt_Gumpname.Text = "gump";
            // 
            // txt_Savefile
            // 
            this.txt_Savefile.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txt_Savefile.Enabled = false;
            this.txt_Savefile.Location = new System.Drawing.Point(8, 24);
            this.txt_Savefile.Name = "txt_Savefile";
            this.txt_Savefile.Size = new System.Drawing.Size(224, 20);
            this.txt_Savefile.TabIndex = 3;
            // 
            // grp_Saveas
            // 
            this.grp_Saveas.Controls.Add(this.btn_Browse);
            this.grp_Saveas.Controls.Add(this.txt_Savefile);
            this.grp_Saveas.Location = new System.Drawing.Point(0, 128);
            this.grp_Saveas.Name = "grp_Saveas";
            this.grp_Saveas.Size = new System.Drawing.Size(328, 56);
            this.grp_Saveas.TabIndex = 4;
            this.grp_Saveas.TabStop = false;
            this.grp_Saveas.Text = "Save As...";
            // 
            // btn_Browse
            // 
            this.btn_Browse.Location = new System.Drawing.Point(240, 24);
            this.btn_Browse.Name = "btn_Browse";
            this.btn_Browse.Size = new System.Drawing.Size(80, 24);
            this.btn_Browse.TabIndex = 4;
            this.btn_Browse.Text = "Browse";
            this.btn_Browse.Click += new System.EventHandler(this.btn_Browse_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(136, 200);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(88, 24);
            this.btn_Cancel.TabIndex = 5;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Export
            // 
            this.btn_Export.Location = new System.Drawing.Point(232, 200);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(88, 23);
            this.btn_Export.TabIndex = 6;
            this.btn_Export.Text = "Export";
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // POLExportForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(330, 234);
            this.ControlBox = false;
            this.Controls.Add(this.btn_Export);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.grp_Properties);
            this.Controls.Add(this.grp_Gumptype);
            this.Controls.Add(this.lbl_Help);
            this.Controls.Add(this.grp_Saveas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Location = new System.Drawing.Point(256, 184);
            this.Name = "POLExportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "POL Export Options";
            this.grp_Gumptype.ResumeLayout(false);
            this.grp_Properties.ResumeLayout(false);
            this.grp_Properties.PerformLayout();
            this.grp_Saveas.ResumeLayout(false);
            this.grp_Saveas.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void btn_Cancel_Click(object sender, System.EventArgs e)
		{
			base.Close();
		}

		private void btn_Browse_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog sfd_Savefile = new SaveFileDialog();
			sfd_Savefile.Filter = "POL Script (*.src)|*.src";
			if (sfd_Savefile.ShowDialog() == DialogResult.OK)
			{
				txt_Savefile.Text = sfd_Savefile.FileName;
			}
		}

		private void btn_Export_Click(object sender, System.EventArgs e)
		{
			if (txt_Savefile.Text.Length == 0)
			{
				MessageBox.Show("Please Specify the File Name.");
				return;
			}
			if (txt_Gumpname.Text.Length == 0)
			{
				MessageBox.Show("Please Specify the Gump Name.");
				return;
			}

			using (StreamWriter streamWriter = File.CreateText(txt_Savefile.Text))
			{
				using(StringWriter stringWriter = seWorker.GetPOLScript(rbt_Bare.Checked))
				{
					streamWriter.Write(stringWriter.ToString());
				}
			}

			base.Close();
		}
	}
}
