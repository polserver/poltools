using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

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
			List<string> pkg_paths = new List<string>();
			TB_loadoutput.Clear();
			if (!Directory.Exists(Settings.Global.rootdir))
			{
				TB_loadoutput.AppendText("Invalid root directory. Please check settings."+Environment.NewLine+Settings.Global.rootdir);
				return;
			}

			TB_loadoutput.AppendText("Checking for packages... ");
			List<string> pkg_cfgs = FileLister.FileSystemUtil.GetAllFileNames(Settings.Global.rootdir, "pkg.cfg", SearchOption.AllDirectories);

			TB_loadoutput.AppendText("pkg.cfg files found = " + pkg_cfgs.Count + Environment.NewLine);
			POLTools.Package.POLPackage pkg = new POLTools.Package.POLPackage(pkg_cfgs[0]);

			TB_loadoutput.AppendText(pkg.enabled.ToString());
		}
	}
}
