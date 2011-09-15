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
		private List<POLTools.Package.POLPackage> _packages;

		public Form1()
		{
			InitializeComponent();

			foreach (TabPage tab in TabControl1.TabPages)
			{
				if (tab != tabPage1)
					tab.Enabled = false;
			}
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
			BTN_load_info.Enabled = false;
			if (!Directory.Exists(Settings.Global.rootdir))
			{
				TB_loadoutput.AppendText("Invalid root directory. Please check settings."+Environment.NewLine+Settings.Global.rootdir);
				return;
			}
			TB_loadoutput.AppendText("Checking for packages... ");
			_packages = POLTools.Package.PackageCache.GetEnabledPackages(Settings.Global.rootdir);
			TB_loadoutput.AppendText("Enabled pkg.cfg files found = " + _packages.Count + Environment.NewLine);

			string[] file_names = { "itemdesc.cfg", "materials.cfg", "toolonmaterial.cfg", "craftmenus.cfg", "craftitems.cfg" };
			foreach (POLTools.Package.POLPackage package in _packages)
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
			TabPage selected_tab = TabControl1.SelectedTab;
			if (!_data_loaded && selected_tab != tabPage1)
			{
				TabControl1.SelectedTab.Enabled = false;
				foreach (Control cntrl in selected_tab.Controls)
				{
					cntrl.Enabled = false;
				}
			}
			else
			{
				foreach (Control cntrl in selected_tab.Controls)
				{
					cntrl.Enabled = true;
				}

				if (!selected_tab.Enabled)
				{
					if (selected_tab == tabPage2)
						PopulateItemDesc();
					if (selected_tab == tabPage3)
						PopulateMaterials();
				}

				selected_tab.Enabled = true;
			}
		}

		private void PopulateItemDesc()
		{
			List<ConfigElem> config_elems = ConfigRepository.global.GetElemsForConfigFile("itemdesc.cfg");
			foreach (ConfigElem config_elem in config_elems)
			{
				string elem_name = config_elem.name;
				if (config_elem.PropertyExists("name"))
					elem_name += "     [" + config_elem.GetConfigString("name") + "]";

				listBox1.Items.Add(elem_name);
			}
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selected_name = listBox1.SelectedItem.ToString();
			string[] split = selected_name.Split(new char[] { ' ', '\t' });
			MessageBox.Show(split[0]);
			ConfigElem config_elem = ConfigRepository.global.GetElemFromConfigFiles("itemdesc.cfg", split[0]);
			TB_itemdescinfo.Clear();
			foreach (string propname in config_elem.ListConfigElemProperties())
			{
				foreach (string value in config_elem.GetConfigStringList(propname))
				{
					TB_itemdescinfo.AppendText(propname +"	"+value+Environment.NewLine);
				}
			}
		}

		public void PopulateMaterials()
		{
			foreach (POLTools.Package.POLPackage package in _packages)
			{
				string materials_cfg_path = package.GetPackagedConfigPath("materials.cfg");
				if (materials_cfg_path == null)
					continue;

				TreeNode pkg_node = materials_tree_view.Nodes.Add(package.name);
				ConfigFile materials_config = ConfigRepository.global.LoadConfigFile(materials_cfg_path);
				foreach (ConfigElem cfg_elem in materials_config.GetConfigElemRefs())
				{
					ConfigElem itemdesc_elem = ConfigRepository.global.GetElemFromConfigFiles("itemdesc.cfg", cfg_elem.name);
					string nodename = cfg_elem.name;
					if (itemdesc_elem.PropertyExists("Name"))
						nodename += "   [" + itemdesc_elem.GetConfigString("Name") + "]";

					pkg_node.Nodes.Add(nodename);
				}
			}
		}		
	}
}
