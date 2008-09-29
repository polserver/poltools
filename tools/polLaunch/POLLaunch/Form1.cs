using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace POLLaunch
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			checkBox1.Checked = Settings.Global.ToBoolean((string)Settings.Global.Properties["ShowPOLTabOnStart"]);
			if (checkBox1.Checked)
				tabControl1.SelectedIndex = tabControl1.TabPages.IndexOfKey("tabPage6");
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutForm tmp = new AboutForm();
			tmp.ShowDialog(this);
		}

		private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Configuration.ConfigurationForm tmp = new Configuration.ConfigurationForm();
			tmp.ShowDialog(this);
		}

		private void checkBox1_CheckStateChanged(object sender, EventArgs e)
		{
			Settings.Global.Properties["ShowPOLTabOnStart"] = checkBox1.Checked;
		}

		private void BTN_RunTests_Click(object sender, EventArgs e)
		{
			textBox1.Text = "";
			Cursor = Cursors.WaitCursor;
			
			textBox1.Text += "Analyzing setup in '" + Settings.Global.Properties["POLPath"] + "'" + Environment.NewLine;
			POLChecks.RealmChecks(ref textBox1);
			POLChecks.ScriptChecks(ref textBox1);

			Cursor = Cursors.Default;
		}
	}
}
