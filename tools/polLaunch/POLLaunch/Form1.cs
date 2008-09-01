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
	}
}
