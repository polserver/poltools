using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CraftTool.Forms.SettingsForm
{
	public partial class SettingsForm : Form
	{
		public SettingsForm()
		{
			InitializeComponent();
		}

		private void BTN_apply_Click(object sender, EventArgs e)
		{

		}

		private void BTN_ok_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void BTN_cancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
