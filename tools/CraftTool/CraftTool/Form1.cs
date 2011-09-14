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
		private bool _data_loaded = false;

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

			string[] file_names = { "itemdesc.cfg", "materials.cfg", "toolonmaterial.cfg", "craftmenus.cfg", "craftitems.cfg" };
			foreach (POLTools.Package.POLPackage package in packages)
			{
				TB_loadoutput.AppendText(package.name+Environment.NewLine);

				foreach ( string filename in file_names )
				{
					string config_path = package.GetPackagedConfigPath(filename);
					if (config_path != null)
					{
						ConfigFile config_file = ConfigRepository.global.LoadConfigFile(config_path);
						TB_loadoutput.AppendText("  Loaded " + config_file.filename + Environment.NewLine);
					}
				}
			}
			_data_loaded = true;
		}

		private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_data_loaded && TabControl1.SelectedTab != tabPage1)
			{
				foreach (Control cntrl in TabControl1.SelectedTab.Controls)
				{
					cntrl.Enabled = false;
				}
			}
			else
			{
				foreach (Control cntrl in TabControl1.SelectedTab.Controls)
				{
					cntrl.Enabled = true;
				}

				if (TabControl1.SelectedTab == tabPage2)
					PopulateItemDesc();
			}
		}

		private void PopulateItemDesc()
		{
			List<ConfigElem> config_elems = ConfigRepository.global.GetElemsForConfigFile("itemdesc.cfg");
			foreach (ConfigElem config_elem in config_elems)
			{
				listBox1.Items.Add(config_elem.name);
			}
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			ConfigElem config_elem = ConfigRepository.global.GetElemFromConfigFiles("itemdesc.cfg", listBox1.SelectedItem.ToString());
			TB_itemdescinfo.Clear();
			foreach (string propname in config_elem.ListConfigElemProperties())
			{
				foreach (string value in config_elem.GetConfigStringList(propname))
				{
					TB_itemdescinfo.AppendText(propname +"	"+value+Environment.NewLine);
				}
			}
		}
	}
}
