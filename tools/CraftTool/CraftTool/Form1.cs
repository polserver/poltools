using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using ConfigUtil;
using POLTools.ConfigRepository;
using System.Drawing;

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
					if (selected_tab == tabPage4)
						PopulateToolOnMaterial();
				}

				selected_tab.Enabled = true;
			}
		}

		private void PopulateItemDesc()
		{
			List<ConfigElem> config_elems = ConfigRepository.global.GetElemsForConfigFile("itemdesc.cfg");
			foreach (ConfigElem config_elem in config_elems)
			{
				string item_name = string.Empty;
				if (config_elem.PropertyExists("name"))
					item_name = config_elem.GetConfigString("name");

				Object[] row = new Object[3];
				row[0] = global::CraftTool.Properties.Resources.unused;
				row[1] = config_elem.name;
				row[2] = item_name;
				itemdesc_datagrid.Rows.Add(row);
			}
			itemdesc_datagrid.ScrollBars = ScrollBars.None;
			itemdesc_datagrid.Refresh();
			itemdesc_datagrid.ScrollBars = ScrollBars.Vertical;
			itemdesc_datagrid.ClearSelection();
			itemdesc_datagrid.Refresh();
		}


		private void itemdesc_datagrid_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			TB_itemdescinfo.Clear();

			DataGridViewRow row = itemdesc_datagrid.Rows[e.RowIndex];
			ConfigElem itemdesc_elem = ConfigRepository.global.GetElemFromConfigFiles("itemdesc.cfg", row.Cells[1].Value.ToString());
			foreach (string propname in itemdesc_elem.ListConfigElemProperties())
			{
				foreach (string value in itemdesc_elem.GetConfigStringList(propname))
				{
					TB_itemdescinfo.AppendText(propname + "	" + value + Environment.NewLine);
				}
			}

			label4.Text = itemdesc_elem.configfile.fullpath;
			itemdesc_picture.Image = (Bitmap)row.Cells[0].Value;
		}

		public void PopulateMaterials()
		{
			foreach (POLTools.Package.POLPackage package in _packages)
			{
				string materials_cfg_path = package.GetPackagedConfigPath("materials.cfg");
				if (materials_cfg_path == null)
					continue;

				TreeNode pkg_node = materials_tree_view.Nodes.Add(":"+package.name+":materials.cfg");
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

		private void materials_tree_view_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeNode selected = materials_tree_view.SelectedNode;
			if (selected.Parent == null)
				return;

			materials_textbox.Clear();
			string name = selected.Text;
			string[] split = name.Split(new char[] { ' ', '\t' });
			name = split[0];
			
			ConfigElem materials_elem = ConfigRepository.global.GetElemFromConfigFiles("materials.cfg", split[0]);
			label5.Text = materials_elem.configfile.fullpath;
			foreach (string propname in materials_elem.ListConfigElemProperties())
			{
				foreach (string value in materials_elem.GetConfigStringList(propname))
				{
					materials_textbox.AppendText(propname + "	" + value + Environment.NewLine);
				}
			}

			materials_picture.Image = global::CraftTool.Properties.Resources.unused;
		}

		public void PopulateToolOnMaterial()
		{
			foreach (POLTools.Package.POLPackage package in _packages)
			{
				string tom_cfg_path = package.GetPackagedConfigPath("toolOnMaterial.cfg");
				if (tom_cfg_path == null)
					continue;

				TreeNode pkg_node = toolonmaterial_treeview.Nodes.Add(":" + package.name + ":toolOnMaterial.cfg");
				ConfigFile tom_config = ConfigRepository.global.LoadConfigFile(tom_cfg_path);
				foreach (ConfigElem cfg_elem in tom_config.GetConfigElemRefs())
				{
					//ConfigElem itemdesc_elem = ConfigRepository.global.GetElemFromConfigFiles("itemdesc.cfg", cfg_elem.name);
					string nodename = cfg_elem.name;
					//if (itemdesc_elem.PropertyExists("Name"))
					//	nodename += "   [" + itemdesc_elem.GetConfigString("Name") + "]";

					pkg_node.Nodes.Add(nodename);
				}
			}
		}

		private void toolonmaterial_treeview_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeNode selected = toolonmaterial_treeview.SelectedNode;
			if (selected.Parent == null)
				return;

			TB_toolonmaterial.Clear();
			string name = selected.Text;
			
			ConfigElem tom_elem = ConfigRepository.global.GetElemFromConfigFiles("toolonmaterial.cfg", name);
			label6.Text = tom_elem.configfile.fullpath;
			foreach (string propname in tom_elem.ListConfigElemProperties())
			{
				foreach (string value in tom_elem.GetConfigStringList(propname))
				{
					this.TB_toolonmaterial.AppendText(propname + "	" + value + Environment.NewLine);
				}
			}

			toolonmaterial_picture.Image = global::CraftTool.Properties.Resources.unused;
		}
	}
}
