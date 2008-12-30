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
			this.TB_POLPath.Text = (string)Settings.Global.Properties["POLPath"];
			this.TB_POLEXEPath.Text = (string)Settings.Global.Properties["POLExePath"];
			this.TB_UOCnvrtEXEPath.Text = (string)Settings.Global.Properties["UOConvertExePath"];
			this.TB_ECompileEXEPath.Text = (string)Settings.Global.Properties["ECompileExePath"];
            this.TB_POLTabShutdown.Text = (string)Settings.Global.Properties["POLTabShutdown"];
		}

		private void ConfigurationForm_Load(object sender, EventArgs e)
		{

		}

		private void BTN_BrowsePOLPath_Click(object sender, EventArgs e)
		{
			this.TB_POLPath.Text = FilePicker.SelectFolder();
		}

		private void TB_POLPath_TextChanged(object sender, EventArgs e)
		{
			if (File.Exists(this.TB_POLPath.Text + @"\pol.exe"))
				this.TB_POLEXEPath.Text = this.TB_POLPath.Text + @"\pol.exe";
			if (File.Exists(this.TB_POLPath.Text + @"\uoconvert.exe"))
				this.TB_UOCnvrtEXEPath.Text = this.TB_POLPath.Text + @"\uoconvert.exe";
			if (File.Exists(this.TB_POLPath.Text + @"\scripts\ecompile.exe"))
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
			Settings.Global.Properties["POLPath"] = this.TB_POLPath.Text;
			Settings.Global.Properties["POLExePath"] = this.TB_POLEXEPath.Text;
			Settings.Global.Properties["UOConvertExePath"] = this.TB_UOCnvrtEXEPath.Text;
            if (File.Exists(this.TB_POLPath.Text + @"\uoconvert.cfg"))
            {
                Settings.Global.Properties["UOConvertCfgPath"] = this.TB_POLPath.Text + @"\uoconvert.cfg";
            }
            Settings.Global.Properties["ECompileExePath"] = this.TB_ECompileEXEPath.Text;
            if (File.Exists(this.TB_POLPath.Text + @"\scripts\ecompile.cfg"))
            {
                Settings.Global.Properties["ECompileCfgPath"] = this.TB_POLPath.Text + @"\scripts\ecompile.cfg";
            }
            Settings.Global.Properties["POLTabShutdown"] = this.TB_POLTabShutdown.Text;
        }

		private void BTN_OKAY_Click(object sender, EventArgs e)
		{
			BTN_Apply_Click(sender, e);
			this.Close();
		}
	}
}
