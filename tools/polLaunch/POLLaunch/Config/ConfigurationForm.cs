using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;

namespace POLLaunch.Configuration
{
	public partial class ConfigurationForm : Form
	{
		public ConfigurationForm()
		{
			InitializeComponent();

			/*
			 *this.TB_UOPath.Text = Properties.Settings.Default.UOPath;
			this.TB_POLPath.Text = Properties.Settings.Default.POLPath;
			this.TB_POLEXEPath.Text = Properties.Settings.Default.POLExePath;
			this.TB_UOCnvrtEXEPath.Text = Properties.Settings.Default.UOConvertExePath;
			this.TB_ECompileEXEPath.Text = Properties.Settings.Default.EcompileExePath;
			*/
		}

		private void ConfigurationForm_Load(object sender, EventArgs e)
		{

		}

		private void BTN_BrowseUOPath_Click(object sender, EventArgs e)
		{
			this.TB_UOPath.Text = FilePicker.SelectFolder();
		}

		private void BTN_BrowsePOLPath_Click(object sender, EventArgs e)
		{
			this.TB_POLPath.Text = FilePicker.SelectFolder();
			
			if ( File.Exists(this.TB_POLPath.Text + @"\pol.exe") )
				this.TB_POLEXEPath.Text = this.TB_POLPath.Text + @"\pol.exe";
			if ( File.Exists( this.TB_POLPath.Text + @"\uoconvert.exe") )
				this.TB_UOCnvrtEXEPath.Text = this.TB_POLPath.Text + @"\uoconvert.exe";
			if ( File.Exists(this.TB_POLPath.Text + @"\scripts\ecompile.exe") )
				this.TB_ECompileEXEPath.Text = this.TB_POLPath.Text + @"\scripts\ecompile.exe";
		}

		private void BTN_BrowsePOLEXEPath_Click(object sender, EventArgs e)
		{
			this.TB_POLEXEPath.Text = FilePicker.SelectFile("Exe files (*.exe)|*.exe");
		}

		private void BTN_BrowseUOCnvrtEXEPath_Click(object sender, EventArgs e)
		{
			this.TB_UOCnvrtEXEPath.Text = FilePicker.SelectFile("Exe files (*.exe)|*.exe");
		}

		private void BTN_BrowseEcompileEXEPath_Click(object sender, EventArgs e)
		{
			this.TB_ECompileEXEPath.Text = FilePicker.SelectFile("Exe files (*.exe)|*.exe");
		}
		
		private void BTN_Apply_Click(object sender, EventArgs e)
		{
			// Save settings
			/*
			Properties.Settings.Default.UOPath = this.TB_UOPath.Text;
			Properties.Settings.Default.POLPath = this.TB_POLPath.Text;
			Properties.Settings.Default.POLExePath = this.TB_POLEXEPath.Text;
			Properties.Settings.Default.UOConvertExePath = this.TB_UOCnvrtEXEPath.Text;
			Properties.Settings.Default.EcompileExePath = this.TB_ECompileEXEPath.Text;
			Properties.Settings.Default.Save();
			 */
		}

		private void BTN_OKAY_Click(object sender, EventArgs e)
		{
			BTN_Apply_Click(sender, e);
			this.Close();
		}
	}
}
