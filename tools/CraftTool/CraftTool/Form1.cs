using System;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ConfigUtil;
using POLTools.ConfigRepository;
using POLTools.Itemdesc;
using POLTools.Package;

namespace CraftTool
{
	public partial class Form1 : Form
	{
		private bool _data_loaded = false;

		#region Generic Form Stuff & Reusable Functions
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

		
		private List<Control> GetAllChildControls(Control control)
		{
			List<Control> controls = new List<Control>();
			foreach (Control child in control.Controls)
			{
				controls.Add(child);
				List<Control> sub_list = GetAllChildControls(child);
				controls.AddRange(sub_list);
			}
			return controls;
		}

		private void ResetTabControls(Control container)
		{
			List<Control> controls = GetAllChildControls(container);
			foreach (Control control in controls)
			{
				if (control is TextBox)
					((TextBox)control).Clear();
				else if (control is NumericTextBox)
					((NumericTextBox)control).Clear();
				else if (control is CheckBox)
					((CheckBox)control).Checked = false;
				else if (control is DataGridView)
					((DataGridView)control).Rows.Clear();
				else if (control is ListView)
				{
					((ListView)control).Items.Clear();
					((ListView)control).ResetText();
				}
				else if (control is ComboBox)
				{
					((ComboBox)control).Items.Clear();
					((ComboBox)control).ResetText();
				}
			}
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
					if (selected_tab == tabPage5)
						PopulateCraftMenus();
					if (selected_tab == tabPage6)
						PopulateCraftItems();
				}

				selected_tab.Enabled = true;
			}
		}
			
		private void RemoveConfigTreeNode(TreeView treeview, string cfgname)
		{
			TreeNode selected = treeview.SelectedNode;
			if (selected == null)
			{
				MessageBox.Show("You need to select a tree node.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			TreeNode parent = selected;
			while (parent.Parent != null)
			{
				parent = parent.Parent;
			}

			POLPackage package = PackageCache.GetPackage(parent.Name);
			ConfigFile config_file;
			string config_path = package.GetPackagedConfigPath(cfgname);
			if (config_path == null) // Its a pseudo config & elem at this point then. (Not on disk)
				config_path = package.path + @"\config\"+cfgname;
			config_file = ConfigRepository.global.LoadConfigFile(config_path);

			if (parent == selected)
			{
				foreach (TreeNode child in selected.Nodes)
				{
					if (child == null)
						continue;
					config_file.RemoveConfigElement(child.Name);
				}
			}
			else
			{
				config_file.RemoveConfigElement(selected.Name);
			}

			TreeView tvparent = selected.TreeView;

			toolonmaterial_treeview.Nodes.Remove(selected);

			if (parent == selected || tvparent.VisibleCount < 1)
				ConfigRepository.global.UnloadConfigFile(config_path);
		}

		private void CreateConfigFileForTreeView(TreeView treeview, string cfgname)
		{
			Forms.SelectionPicker.SelectionPicker picker = new Forms.SelectionPicker.SelectionPicker("Select a package", PackageCache.Global.packagenames);
			picker.ShowDialog(this);
			if (picker.DialogResult != DialogResult.OK)
				return;
			else if (PackageCache.GetPackage(picker.text) == null)
			{
				MessageBox.Show("Invalid package name '" + picker.text + "'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			treeview.Nodes.Add(picker.text, ":" + picker.text + ":"+cfgname);

			POLPackage pkg = PackageCache.GetPackage(picker.text);
			string filepath = pkg.path + @"\config\"+cfgname;
			ConfigFile config_file = new ConfigFile(filepath);
			ConfigRepository.global.AddConfigFile(config_file);
		}

		private void AddConfigElemForTreeNode(TreeNode selected, string cfgname, string elemprefix, string elemname, bool objtype)
		{
			string package_name = POLPackage.ParsePackageName(selected.Text);
			POLPackage package = PackageCache.GetPackage(package_name);
			ConfigFile config_file;
			string config_path = package.GetPackagedConfigPath(cfgname);
			if (config_path == null) // Its a pseudo config & elem at this point then. (Not on disk)
				config_path = package.path + @"\config\"+cfgname;

			config_file = ConfigRepository.global.LoadConfigFile(config_path);
			ConfigElem elem = new ConfigElem(elemprefix, elemname);
			config_file.AddConfigElement(elem);

			if (objtype)
				AddObjTypeToTreeView(selected, elemname);
			else
				selected.Nodes.Add(elemname, elemname);
		}

		private TreeNode AddObjTypeToTreeView(TreeNode parent_node, string objtype)
		{
			string nodename = objtype;
			ConfigElem itemdesc_elem = ItemdescCache.Global.GetElemForObjType(objtype);
			if (itemdesc_elem != null)
			{
				if (itemdesc_elem.PropertyExists("Name"))
					nodename += "   [" + itemdesc_elem.GetConfigString("Name") + "]";
			}

			TreeNode added = parent_node.Nodes.Add(objtype, nodename);
			return added;
		}

		private TreeNode GetParentTreeNode(TreeNode thenode)
		{
			TreeNode nodeparent = thenode;
			while (nodeparent.Parent != null)
			{
				nodeparent = nodeparent.Parent;
			}
			return nodeparent;
		}

		private TreeNode CheckForSelectedNode(TreeView treeview)
		{
			TreeNode selected = treeview.SelectedNode;
			if (selected == null)
			{
				MessageBox.Show("You need to select a tree node.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}
			return selected;
		}

		private void WriteTreeViewConfigFiles(TreeView treeview, string filename)
		{
			// First figure out if any existing configs need to be deleted.
			foreach (POLPackage package in PackageCache.Global.packagelist)
			{
				string cfg_path = package.GetPackagedConfigPath(filename);
				if (cfg_path == null)
					continue;
				bool loaded = ConfigRepository.global.IsPathCached(cfg_path);
				if (!loaded) // Delete
					File.Delete(cfg_path);
			}

			// Write the tree information to the config files.
			foreach (TreeNode node in treeview.Nodes)
			{
				if (node.Parent != null)
					continue;
				POLPackage package = PackageCache.GetPackage(node.Name);
				string cfg_path = package.GetPackagedConfigPath(filename);
				if (cfg_path == null) // File doesnt already exist.
					cfg_path = package.path + @"\config\"+filename;
				if (!Directory.Exists(package.path + @"\config\"))
					Directory.CreateDirectory(package.path + @"\config\");
				ConfigFile config_file = ConfigRepository.global.LoadConfigFile(cfg_path);
				ConfigRepository.WriteConfigFile(config_file);
			}

			MessageBox.Show("Done", filename, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public void PopulateTreeViewWithConfigElems(TreeView treeview, string filename, bool itemdesc)
		{
			foreach (POLTools.Package.POLPackage package in PackageCache.Global.packagelist)
			{
				string cfg_path = package.GetPackagedConfigPath(filename);
				if (cfg_path == null)
					continue;

				string nodename = ":" + package.name + ":"+filename;
				TreeNode pkg_node = treeview.Nodes.Add(package.name, nodename);
				ConfigFile config_file = ConfigRepository.global.LoadConfigFile(cfg_path);
				foreach (ConfigElem cfg_elem in config_file.GetConfigElemRefs())
				{
					if (itemdesc)
						AddObjTypeToTreeView(pkg_node, cfg_elem.name);
					else
						pkg_node.Nodes.Add(cfg_elem.name, cfg_elem.name);
				}
			}
		}

		private void craftitems_datagrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			ComboBox c = e.Control as ComboBox;
			if (c != null)
				c.DropDownStyle = ComboBoxStyle.DropDown;
		}
		
		#endregion

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
			PackageCache.LoadPackages(Settings.Global.rootdir);

			List<POLTools.Package.POLPackage> packages = PackageCache.Global.packagelist;
			TB_loadoutput.AppendText("Enabled pkg.cfg files found = " + packages.Count + Environment.NewLine);

			string[] file_names = { "attributes.cfg", "itemdesc.cfg", "materials.cfg", "toolonmaterial.cfg", "craftmenus.cfg", "craftitems.cfg" };
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

			POLTools.Itemdesc.ItemdescCache.Global.LoadItemdescFiles();

			_data_loaded = true;
		}
		
		#region Itemdesc Tab Stuff
		private void PopulateItemDesc()
		{
			List<ConfigElem> config_elems = ItemdescCache.Global.GetAllObjTypeElems();
			foreach (ConfigElem config_elem in config_elems)
			{
				string item_name = string.Empty;
				if (config_elem.PropertyExists("name"))
				{
					int count = 0;
					List<string> name_entries = config_elem.GetConfigStringList("name");
					foreach (string name in name_entries)
					{
						if (count > 0)
							item_name += ", ";
						item_name += name;
						count++;
					}
				}
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
			itemdesc_elemcontents.Clear();

			DataGridViewRow row = itemdesc_datagrid.Rows[e.RowIndex];
			//ConfigElem itemdesc_elem = ConfigRepository.global.GetElemFromConfigFiles("itemdesc.cfg", row.Cells[1].Value.ToString());
			ConfigElem itemdesc_elem = ItemdescCache.Global.GetElemForObjType(row.Cells[1].Value.ToString());
			foreach (string propname in itemdesc_elem.ListConfigElemProperties())
			{
				foreach (string value in itemdesc_elem.GetConfigStringList(propname))
				{
					itemdesc_elemcontents.AppendText(propname + "	" + value + Environment.NewLine);
				}
			}

			label4.Text = itemdesc_elem.configfile.fullpath;
			itemdesc_picturebox_tilepic.Image = (Bitmap)row.Cells[0].Value;
		}

		private void copyObjTypeToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DataGridViewRow row = itemdesc_datagrid.CurrentRow;
			DataGridViewCell cell = row.Cells[1];
			Clipboard.SetText(cell.Value.ToString());
		}
		#endregion

		#region Materials Tab Stuff
		public void PopulateMaterials()
		{
			PopulateTreeViewWithConfigElems(materials_treeview, "materials.cfg", true);
		}
		
		private void materials_tree_view_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeNode selected = materials_treeview.SelectedNode;

			ResetTabControls(groupBox3);

			if (selected.Parent == null)
				return;
						
			ConfigElem materials_elem = ConfigRepository.global.FindElemInConfigFiles("materials.cfg", selected.Name);
			label5.Text = materials_elem.configfile.fullpath;
			materials_combobox_changeto.Items.Clear();
			foreach (string propname in materials_elem.ListConfigElemProperties())
			{
				foreach (string value in materials_elem.GetConfigStringList(propname))
				{
					materials_elemcontents.AppendText(propname + "	" + value + Environment.NewLine);
				}
			}
			materials_picturebox_material.Image = global::CraftTool.Properties.Resources.unused;
			if (materials_elem.PropertyExists("Category"))
				materials_textbox_category.Text = materials_elem.GetConfigString("Category");
			if (materials_elem.PropertyExists("Color"))
				materials_textbox_color.Text = materials_elem.GetConfigString("Color");
			if (materials_elem.PropertyExists("Difficulty"))
				materials_textbox_difficulty.Text = materials_elem.GetConfigString("Difficulty");
			if (materials_elem.PropertyExists("Quality"))
				materials_textbox_quality.Text = materials_elem.GetConfigString("Quality");
			if (materials_elem.PropertyExists("CreatedScript"))
				materials_textbox_createdscript.Text = materials_elem.GetConfigString("CreatedScript");

			List<string> itemdesc_names = ItemdescCache.Global.GetAllObjTypeNames();
			materials_combobox_changeto.Items.AddRange(itemdesc_names.ToArray());
			if (materials_elem.PropertyExists("ChangeTo"))
			{
				string changeto = materials_elem.GetConfigString("ChangeTo").ToLower();
				//int pos = itemdesc_names.IndexOf(materials_elem.GetConfigString("ChangeTo"));
				int pos = materials_combobox_changeto.FindString(changeto);
				materials_combobox_changeto.SelectedIndex = pos; // Account for 'None'
			}
		}

		private void BTN_materials_update_Click(object sender, EventArgs e)
		{
			TreeNode selected = CheckForSelectedNode(materials_treeview);
			if (selected == null)
				return;
			TreeNode nodeparent = GetParentTreeNode(selected);
					
			POLPackage package = PackageCache.GetPackage(nodeparent.Name);
			ConfigFile materials_cfg;
			string config_path = package.GetPackagedConfigPath("materials.cfg");
			materials_cfg = ConfigRepository.global.LoadConfigFile(config_path);
			ConfigElem original = materials_cfg.GetConfigElem(selected.Name);

			ConfigElem newelem = new ConfigElem(original.type, original.name);
			if (materials_textbox_category.Text.Length > 0)
				newelem.AddConfigLine("Category", materials_textbox_category.Text);
			else
			{
				MessageBox.Show("No category for material was entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if ( materials_textbox_color.Text.Length > 0 )
				newelem.AddConfigLine("Color", materials_textbox_color.Text);
			if ( materials_textbox_difficulty.Text.Length > 0 )
				newelem.AddConfigLine("Difficulty", materials_textbox_difficulty.Text);
			if ( materials_textbox_quality.Text.Length > 0 )
				newelem.AddConfigLine("Quality", materials_textbox_quality.Text);
			if ( materials_combobox_changeto.Text.Length > 0 )
				newelem.AddConfigLine("ChangeTo", materials_combobox_changeto.Text);
			if ( materials_textbox_createdscript.Text.Length > 0 )
				newelem.AddConfigLine("CreatedScript", materials_textbox_createdscript.Text);
			
			materials_cfg.RemoveConfigElement(selected.Name);
			materials_cfg.AddConfigElement(newelem);

			materials_tree_view_AfterSelect(sender, null);
		}

		private void materials_context_strip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			return;
		}

		private void createNewConfigToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CreateConfigFileForTreeView(materials_treeview, "materials.cfg");
		}

		private void addNewElementToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TreeNode selected = CheckForSelectedNode(materials_treeview);
			if (selected == null)
				return;
			TreeNode nodeparent = GetParentTreeNode(selected);
			
			Forms.SelectionPicker.SelectionPicker picker = new Forms.SelectionPicker.SelectionPicker("Select a material", ItemdescCache.Global.GetAllObjTypes());
			picker.ShowDialog(this);
			if (picker.result != DialogResult.OK)
				return;
			else
			{
				if (ItemdescCache.Global.GetElemForObjType(picker.text) == null)
				{
					MessageBox.Show("Invalid object type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			AddConfigElemForTreeNode(nodeparent, "materials.cfg", "Material", picker.text, true);
		}
		
		private void removeElementToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RemoveConfigTreeNode(materials_treeview, "materials.cfg");
		}

		private void BTN_materials_write_Click(object sender, EventArgs e)
		{
			WriteTreeViewConfigFiles(materials_treeview, "materials.cfg");
		}

		#endregion

		#region Tool On Material Tab Stuff
		public void PopulateToolOnMaterial()
		{
			PopulateTreeViewWithConfigElems(toolonmaterial_treeview, "toolOnMaterial.cfg", false);
		}
		
		private void toolonmaterial_treeview_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeNode selected = toolonmaterial_treeview.SelectedNode;
			ResetTabControls(groupBox4);

			if (selected.Parent == null)
				return;

			ConfigElem tom_elem = ConfigRepository.global.FindElemInConfigFiles("toolOnMaterial.cfg", selected.Name);
						
			label17.Text = tom_elem.configfile.fullpath;
			foreach (string propname in tom_elem.ListConfigElemProperties())
			{
				foreach (string value in tom_elem.GetConfigStringList(propname))
				{
					TB_toolonmaterial.AppendText(propname + "	" + value + Environment.NewLine);
				}
			}

			toolonmaterial_picturebox_tool.Image = global::CraftTool.Properties.Resources.unused;
			if (tom_elem.PropertyExists("MenuScript"))
				tom_textbox_menuscript.Text = tom_elem.GetConfigString("MenuScript");

			if (tom_elem.PropertyExists("ShowMenu"))
			{
				string value = tom_elem.GetConfigString("ShowMenu");
				int pos = combobox_tom_showmenus.Items.Add(value);
				combobox_tom_showmenus.SelectedIndex = pos;
			}
			combobox_tom_showmenus.Items.AddRange(ConfigRepository.global.GetElemNamesFromConfigFiles("CraftMenus.cfg").ToArray());
		}
		
		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			CreateConfigFileForTreeView(toolonmaterial_treeview, "toolOnMaterial.cfg");
		}

		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
			TreeNode selected = CheckForSelectedNode(toolonmaterial_treeview);
			if (selected == null)
				return;
			TreeNode nodeparent = GetParentTreeNode(selected);

			Forms.SelectionPicker.SelectionPicker toolpicker = new Forms.SelectionPicker.SelectionPicker("Select a tool", ItemdescCache.Global.GetAllObjTypes());
			toolpicker.ShowDialog(this);
			if (toolpicker.result != DialogResult.OK)
				return;
			else
			{
				if (ItemdescCache.Global.GetElemForObjType(toolpicker.text) == null)
				{
					MessageBox.Show("Invalid object type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}
			List<string> categories = ConfigRepository.global.GetConfigPropertyValuesFromLoadedConfigFiles("materials.cfg", "Category", true, false);
			Forms.SelectionPicker.SelectionPicker catpicker = new Forms.SelectionPicker.SelectionPicker("Select a category", categories);
			catpicker.ShowDialog(this);
			if (catpicker.result != DialogResult.OK)
				return;
			else
			{
				if (!categories.Exists(delegate(string n) { return n.Equals(catpicker.text, StringComparison.CurrentCultureIgnoreCase); }))
				{
					MessageBox.Show("Invalid category name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}
			string elem_name = "Tool=" + toolpicker.text + "&Material=" + catpicker.text;

			AddConfigElemForTreeNode(nodeparent, "toolOnMaterial.cfg", "MenuPointer", elem_name, false);
		}

		private void toolStripMenuItem3_Click(object sender, EventArgs e)
		{
			RemoveConfigTreeNode(toolonmaterial_treeview, "toolOnMaterial.cfg");
		}
		
		private void BTN_tom_writefiles_Click(object sender, EventArgs e)
		{
			WriteTreeViewConfigFiles(toolonmaterial_treeview, "toolOnMaterial.cfg");
		}

		private void BTN_tom_update_Click(object sender, EventArgs e)
		{
			TreeNode selected = CheckForSelectedNode(toolonmaterial_treeview);
			if (selected == null)
				return;
			TreeNode nodeparent = GetParentTreeNode(selected);

			POLPackage package = PackageCache.GetPackage(nodeparent.Name);
			ConfigFile config_file;
			string config_path = package.GetPackagedConfigPath("toolOnMaterial.cfg");
			config_file = ConfigRepository.global.LoadConfigFile(config_path);
			ConfigElem original = config_file.GetConfigElem(selected.Name);

			ConfigElem newelem = new ConfigElem(original.type, original.name);
			if (combobox_tom_showmenus.Text.Length > 0)
				newelem.AddConfigLine("ShowMenu", combobox_tom_showmenus.Text);
			else
			{
				MessageBox.Show("A menu elem name needs to be entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if ( tom_textbox_menuscript.Text.Length > 0 )
				newelem.AddConfigLine("MenuScript", tom_textbox_menuscript.Text);
			
			config_file.RemoveConfigElement(selected.Name);
			config_file.AddConfigElement(newelem);

			toolonmaterial_treeview_AfterSelect(sender, null);
		}
		#endregion

		#region Craft Menus Tab Stuff
		public void PopulateCraftMenus()
		{
			PopulateTreeViewWithConfigElems(craftmenus_treeview, "craftMenus.cfg", true);
		}


		private void craftmenus_treeview_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeNode selected = craftmenus_treeview.SelectedNode;
			ResetTabControls(groupBox17);

			if (selected.Parent == null)
				return;

			ConfigElem config_elem = ConfigRepository.global.FindElemInConfigFiles("craftMenus.cfg", selected.Name);

			label34.Text = config_elem.configfile.fullpath;
			foreach (string propname in config_elem.ListConfigElemProperties())
			{
				foreach (string value in config_elem.GetConfigStringList(propname))
				{
					craftmenus_textbox_eleminfo.AppendText(propname + "	" + value + Environment.NewLine);
				}
			}

			craftmenus_Column1.Items.Clear();
			craftmenus_Column1.Items.AddRange(ConfigRepository.global.GetElemNamesFromConfigFiles("craftMenus.cfg").ToArray());
			craftmenus_Column4.Items.Clear();
			craftmenus_Column4.Items.AddRange(ConfigRepository.global.GetElemNamesFromConfigFiles("craftItems.cfg").ToArray());
			craftmenus_Column4.Items.Add("--------");
			craftmenus_Column4.Items.AddRange(ItemdescCache.Global.GetAllObjTypeNames().ToArray());
			
			if (config_elem.PropertyExists("SubMenu"))
			{
				foreach (string line in config_elem.GetConfigStringList("SubMenu"))
				{
					string[] split = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					if (split.Length < 1)
						continue;
					else if (!craftmenus_Column1.Items.Contains(split[0]))
						craftmenus_Column1.Items.Add(split[0]);
					
					craftmenus_datagrid_submenus.Rows.Add(split);
				}
			}

			if (config_elem.PropertyExists("Entry"))
			{
				foreach (string line in config_elem.GetConfigStringList("Entry"))
				{
					string[] split = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					if (split.Length < 1)
						continue;
					else if (!craftmenus_Column4.Items.Contains(split[0]))
						craftmenus_Column4.Items.Add(split[0]);
					
					craftmenus_datagrid_itementries.Rows.Add(split);
				}
			}
		}


		private void toolStripMenuItem4_Click(object sender, EventArgs e)
		{
			CreateConfigFileForTreeView(craftmenus_treeview, "craftMenus.cfg");
		}

		private void toolStripMenuItem5_Click(object sender, EventArgs e)
		{
			TreeNode selected = CheckForSelectedNode(craftmenus_treeview);
			if (selected == null)
				return;
			TreeNode nodeparent = GetParentTreeNode(selected);

			Forms.InputForm.InputForm input = new Forms.InputForm.InputForm("Config elem name:");
			input.ShowDialog(this);
			if (input.result != DialogResult.OK)
				return;

			AddConfigElemForTreeNode(nodeparent, "craftMenus.cfg", "MenuElem", input.text, false);
		}

		private void toolStripMenuItem6_Click(object sender, EventArgs e)
		{
			RemoveConfigTreeNode(craftmenus_treeview, "craftMenus.cfg");
		}

		private void BTN_craftmenus_update_Click(object sender, EventArgs e)
		{
			TreeNode selected = CheckForSelectedNode(craftmenus_treeview);
			if (selected == null)
				return;
			TreeNode nodeparent = GetParentTreeNode(selected);

			POLPackage package = PackageCache.GetPackage(nodeparent.Name);
			string config_path = package.GetPackagedConfigPath("craftMenus.cfg");
			ConfigFile config_file = ConfigRepository.global.LoadConfigFile(config_path);
			ConfigElem original = config_file.GetConfigElem(selected.Name);

			ConfigElem newelem = new ConfigElem(original.type, original.name);
			
			int row_count = 0;
			foreach (DataGridViewRow row in craftmenus_datagrid_submenus.Rows)
			{
				row_count++;
				string value = string.Empty;
				int empty = row.Cells.Count;
				foreach ( DataGridViewCell cell in row.Cells )
				{
					if (cell.Value == null)
						continue; // Handle empty cells
					value += cell.Value.ToString() + "\t";
					empty--;
				}
				value = value.Trim();
				if (value.Length > 0)
				{
					if (empty > 0)
					{
						MessageBox.Show("Not all submenu cells were filled out on row "+row_count+".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
					newelem.AddConfigLine("SubMenu", value);
				}
			}

			foreach (DataGridViewRow row in craftmenus_datagrid_itementries.Rows)
			{
				string value = string.Empty;
				foreach (DataGridViewCell cell in row.Cells)
				{
					if (cell.Value == null)
						continue; // Handle empty cells
					value += cell.Value.ToString() + "\t";
				}
				value = value.Trim();
				if (value.Length > 0)
					newelem.AddConfigLine("Entry", value);
			}

			config_file.RemoveConfigElement(selected.Name);
			config_file.AddConfigElement(newelem);
			
			craftmenus_treeview_AfterSelect(sender, null);
		}

		private void BTN_craftmenus_write_Click(object sender, EventArgs e)
		{
			WriteTreeViewConfigFiles(craftmenus_treeview, "craftMenus.cfg");
		}

		#endregion

		#region Craft Items Tab Stuff
		public void PopulateCraftItems()
		{
			PopulateTreeViewWithConfigElems(craftitems_treeview, "craftItems.cfg", true);
		}
		
		private void treeview_craftitems_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeNode selected = craftitems_treeview.SelectedNode;
			ResetTabControls(groupBox9);		
			if (selected.Parent == null)
				return;

			ConfigElem cfg_elem = ConfigRepository.global.FindElemInConfigFiles("craftItems.cfg", selected.Name);
						
			label21.Text = cfg_elem.configfile.fullpath;
			foreach (string propname in cfg_elem.ListConfigElemProperties())
			{
				foreach (string value in cfg_elem.GetConfigStringList(propname))
				{
					craftitems_textbox_eleminfo.AppendText(propname + "	" + value + Environment.NewLine);
				}
			}

			if ( cfg_elem.PropertyExists("NoRecycle") )
				craftitems_checkbox_norecycle.Checked = (cfg_elem.GetConfigInt("NoRecycle") > 0);
			if (cfg_elem.PropertyExists("Exceptional"))
				craftitems_checkbox_exceptional.Checked = (cfg_elem.GetConfigInt("Exceptional") > 0);
			if (cfg_elem.PropertyExists("MakeMaximum"))
				craftitems_checkbox_makemaximum.Checked = (cfg_elem.GetConfigInt("MakeMaximum") > 0);

			List<string> categories = ConfigRepository.global.GetConfigPropertyValuesFromLoadedConfigFiles("materials.cfg", "Category", true, false);
			craftitems_combobox_clickedcategory.Items.AddRange(categories.ToArray());
			if (cfg_elem.PropertyExists("ClickedCategory"))
			{
				string category = cfg_elem.GetConfigString("ClickedCategory");
				int pos = craftitems_combobox_clickedcategory.FindString(category);
				craftitems_combobox_clickedcategory.SelectedIndex = pos;
			}

			List<string> attributes = ConfigRepository.global.GetElemNamesFromConfigFiles("attributes.cfg");
			craftitems_combobox_attributes.Items.AddRange(attributes.ToArray());
			if (cfg_elem.PropertyExists("Attribute"))
			{
				string attribute = cfg_elem.GetConfigString("Attribute");
				int pos = craftitems_combobox_attributes.FindString(attribute);
				craftitems_combobox_attributes.SelectedIndex = pos;
			}

			if (cfg_elem.PropertyExists("Difficulty"))
				craftitems_textbox_difficulty.Text = cfg_elem.GetConfigString("Difficulty");
			if (cfg_elem.PropertyExists("MakeAmount"))
				craftitems_textbox_makeamount.Text = cfg_elem.GetConfigString("MakeAmount");
			if (cfg_elem.PropertyExists("CraftLoops"))
				craftitems_textbox_craftloops.Text = cfg_elem.GetConfigString("CraftLoops");
			if (cfg_elem.PropertyExists("LoopWait"))
				craftitems_textbox_loopwait.Text = cfg_elem.GetConfigString("LoopWait");
			if (cfg_elem.PropertyExists("Animation"))
				craftitems_textbox_animation.Text = cfg_elem.GetConfigString("Animation");
			if (cfg_elem.PropertyExists("SkillCheckScript"))
				craftitems_textbox_skillcheckscript.Text = cfg_elem.GetConfigString("SkillCheckScript");
			if (cfg_elem.PropertyExists("PreCreateScript"))
				craftitems_textbox_precreatescript.Text = cfg_elem.GetConfigString("PreCreateScript");
			if (cfg_elem.PropertyExists("CreateScript"))
				craftitems_textbox_createscript.Text = cfg_elem.GetConfigString("CreateScript");
			if (cfg_elem.PropertyExists("PostCreateScript"))
				craftitems_textbox_postcreatescript.Text = cfg_elem.GetConfigString("PostCreateScript");
			if (cfg_elem.PropertyExists("FindMaterialScript"))
				craftitems_textbox_findmaterialscript.Text = cfg_elem.GetConfigString("FindMaterialScript");
			if (cfg_elem.PropertyExists("ConsumeScript"))
				craftitems_textbox_consumescript.Text = cfg_elem.GetConfigString("ConsumeScript");

			if (cfg_elem.PropertyExists("Sound"))
			{
				foreach (string sound in cfg_elem.GetConfigStringList("Sound"))
				{
					craftitems_datagrid_sounds.Rows.Add(sound);
				}
			}

			if (cfg_elem.PropertyExists("Tool"))
			{
				foreach (string toolgroup in cfg_elem.GetConfigStringList("Tool"))
				{
					craftitems_datagrid_tools.Rows.Add(toolgroup);
				}
			}

			Column4.Items.Clear();
			Column4.Items.AddRange(ItemdescCache.Global.GetAllObjTypeNames().ToArray());
			Column6.Items.Clear();
			Column6.Items.AddRange(ItemdescCache.Global.GetAllObjTypeNames().ToArray());

			if (cfg_elem.PropertyExists("Material"))
			{
				foreach (string material in cfg_elem.GetConfigStringList("Material"))
				{
					string[] split = material.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					if (split[0].Equals("[clicked]", StringComparison.CurrentCultureIgnoreCase))
					{
						if ( split.Length > 1 )
							craftitems_textbox_clickedamount.Text = split[1];
						else
							craftitems_textbox_clickedamount.Text = "0";
						continue;
					}
					else if (!Column4.Items.Contains(split[0]))
					{
						Column4.Items.Add(split[0]);
						craftitems_datagrid_materials.Rows.Add(split);
					}
					craftitems_datagrid_materials.Rows.Add(split);
				}				
			}

			if (cfg_elem.PropertyExists("ClickMaterial"))
			{
				foreach (string material in cfg_elem.GetConfigStringList("ClickMaterial"))
				{
					string[] split = material.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					if (!Column6.Items.Contains(split[0]))
					{
						Column6.Items.Add(split[0]);
						craftitems_datagrid_clickedmaterials.Rows.Add(split);
					}
					craftitems_datagrid_clickedmaterials.Rows.Add(split);
				}
			}
			
			craftitems_picturebox_itempic.Image = global::CraftTool.Properties.Resources.unused;			
		}

		private void toolStripMenuItem7_Click(object sender, EventArgs e)
		{
			CreateConfigFileForTreeView(craftitems_treeview, "craftItems.cfg");
		}

		private void toolStripMenuItem8_Click(object sender, EventArgs e)
		{
			TreeNode selected = CheckForSelectedNode(craftitems_treeview);
			if (selected == null)
				return;
			TreeNode nodeparent = GetParentTreeNode(selected);

			Forms.SelectionPicker.SelectionPicker picker = new Forms.SelectionPicker.SelectionPicker("Select an item", ItemdescCache.Global.GetAllObjTypes());
			picker.ShowDialog(this);
			if (picker.result != DialogResult.OK)
				return;
			else
			{
				if (ItemdescCache.Global.GetElemForObjType(picker.text) == null)
				{
					MessageBox.Show("Invalid object type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			AddConfigElemForTreeNode(nodeparent, "craftItems.cfg", "Item", picker.text, true);
		}

		private void toolStripMenuItem9_Click(object sender, EventArgs e)
		{
			RemoveConfigTreeNode(craftitems_treeview, "craftItems.cfg");
		}
		
		private void BTN_update_craftitem_Click(object sender, EventArgs e)
		{
			TreeNode selected = CheckForSelectedNode(craftitems_treeview);
			if (selected == null)
				return;
			TreeNode nodeparent = GetParentTreeNode(selected);

			POLPackage package = PackageCache.GetPackage(nodeparent.Name);
			string config_path = package.GetPackagedConfigPath("craftItems.cfg");
			ConfigFile config_file = ConfigRepository.global.LoadConfigFile(config_path);
			ConfigElem original = config_file.GetConfigElem(selected.Name);

			ConfigElem newelem = new ConfigElem(original.type, original.name);

			if (craftitems_checkbox_norecycle.Checked)
				newelem.AddConfigLine("NoRecycle", "1");
			newelem.AddConfigLine("ClickedCategory", craftitems_combobox_clickedcategory.Text);

			newelem.AddConfigLine("Attribute", craftitems_combobox_attributes.Text);
			newelem.AddConfigLine("Difficulty", craftitems_textbox_difficulty.Text);

			if ( craftitems_textbox_craftloops.Text.Length > 0 )
				newelem.AddConfigLine("CraftLoops", craftitems_textbox_craftloops.Text);
			if ( craftitems_textbox_loopwait.Text.Length > 0 )
				newelem.AddConfigLine("LoopWait", craftitems_textbox_loopwait.Text);				
			foreach (DataGridViewRow row in craftitems_datagrid_sounds.Rows)
			{
				string value = string.Empty;
				foreach (DataGridViewCell cell in row.Cells)
				{
					if (cell.Value == null)
						continue; // Handle empty cells
					value += cell.Value.ToString() + "\t";
				}
				value = value.Trim();
				if (value.Length > 0)
					newelem.AddConfigLine("Sound", value);
			}

			foreach (DataGridViewRow row in craftitems_datagrid_tools.Rows)
			{
				string value = string.Empty;
				foreach (DataGridViewCell cell in row.Cells)
				{
					if (cell.Value == null)
						continue; // Handle empty cells
					value += cell.Value.ToString() + "\t";
				}
				value = value.Trim();
				if (value.Length > 0)
					newelem.AddConfigLine("Tool", value);
			}

			newelem.AddConfigLine("Animation", craftitems_textbox_animation.Text);

			if (craftitems_checkbox_exceptional.Checked)
				newelem.AddConfigLine("Exceptional", "1"); 
			if (craftitems_checkbox_makemaximum.Checked)
				newelem.AddConfigLine("MakeMaximum", "1");

			if ( craftitems_textbox_makeamount.Text.Length > 0 )
				newelem.AddConfigLine("MakeAmount", craftitems_textbox_makeamount.Text);

			newelem.AddConfigLine("Material", "[clicked]\t" + craftitems_textbox_clickedamount.Text);

			foreach (DataGridViewRow row in craftitems_datagrid_materials.Rows)
			{
				string value = string.Empty;
				foreach (DataGridViewCell cell in row.Cells)
				{
					if (cell.Value == null)
						continue; // Handle empty cells
					value += cell.Value.ToString() + "\t";
				}
				value = value.Trim();
				if (value.Length > 0)
					newelem.AddConfigLine("Material", value);
			}
			foreach (DataGridViewRow row in craftitems_datagrid_clickedmaterials.Rows)
			{
				string value = string.Empty;
				foreach (DataGridViewCell cell in row.Cells)
				{
					if (cell.Value == null)
						continue; // Handle empty cells
					value += cell.Value.ToString() + "\t";
				}
				value = value.Trim();
				if (value.Length > 0)
					newelem.AddConfigLine("ClickMaterial", value);
			}

			if (craftitems_textbox_skillcheckscript.Text.Length > 0)
				newelem.AddConfigLine("SkillCheckScript", craftitems_textbox_skillcheckscript.Text);
			if (craftitems_textbox_precreatescript.Text.Length > 0)
				newelem.AddConfigLine("PreCreateScript", craftitems_textbox_precreatescript.Text);
			if (craftitems_textbox_createscript.Text.Length > 0)
				newelem.AddConfigLine("CreateScript", craftitems_textbox_createscript.Text);
			if (craftitems_textbox_postcreatescript.Text.Length > 0)
				newelem.AddConfigLine("PostCreateScript", craftitems_textbox_postcreatescript.Text);
			if (craftitems_textbox_findmaterialscript.Text.Length > 0)
				newelem.AddConfigLine("FindMaterialScript", craftitems_textbox_findmaterialscript.Text);
			if (craftitems_textbox_consumescript.Text.Length > 0)
				newelem.AddConfigLine("ConsumeScript", craftitems_textbox_consumescript.Text);
			
			config_file.RemoveConfigElement(selected.Name);
			config_file.AddConfigElement(newelem);

			treeview_craftitems_AfterSelect(sender, null);
		}

		private void BTN_write_craftitems_Click(object sender, EventArgs e)
		{
			WriteTreeViewConfigFiles(craftitems_treeview, "craftItems.cfg");
		}

		#endregion
	}
}
