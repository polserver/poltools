using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using ConfigUtil;
using POLTools.ConfigRepository;

namespace CraftTool
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Settings.Global.LoadSettings();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Forms.SettingsForm.SettingsForm settings_form = new Forms.SettingsForm.SettingsForm();
			settings_form.ShowDialog(this);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Forms.AboutForm.AboutForm about_form = new CraftTool.Forms.AboutForm.AboutForm();
			about_form.ShowDialog(this);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			TB_loadoutput.Clear();
			List<string> pkg_paths = new List<string>();
			List<POLTools.Package.POLPackage> packages;
			if (!Directory.Exists(Settings.Global.rootdir))
			{
				TB_loadoutput.AppendText("Invalid root directory. Please check settings."+Environment.NewLine+Settings.Global.rootdir);
				return;
			}
			TB_loadoutput.AppendText("Checking for packages... ");
			packages = POLTools.Package.POLPackage.GetEnabledPackages(Settings.Global.rootdir);
			TB_loadoutput.AppendText("Enabled pkg.cfg files found = " + packages.Count + Environment.NewLine);
			foreach (POLTools.Package.POLPackage package in packages)
			{
				TB_loadoutput.AppendText(package.name+Environment.NewLine);
				string itemdesc_path = package.GetPackagedConfigPath("itemdesc.cfg");
				if (itemdesc_path != null)
				{
					ConfigFile itemdesc = ConfigRepository.global.LoadConfigFile(itemdesc_path);
					TB_loadoutput.AppendText("  Loaded "+ itemdesc.filename + Environment.NewLine);
				}
			}
		}
	}
}
