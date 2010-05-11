using System;
using System.Windows.Forms;

namespace UODownloader.Forms
{
	public partial class SettingsForm : Form
	{
		public SettingsForm()
		{
			InitializeComponent();
			this.Icon = global::UODownloader.Properties.Resources.c25;
			textBox1.Text = (string)Settings.Global.settings["UpdateURL"];
			if (!textBox1.Text.StartsWith("http://", true, System.Globalization.CultureInfo.CurrentCulture))
				textBox1.Text.Insert(0, @"http://");
			textBox2.Text = (string)Settings.Global.settings["GameDirectory"];
		}

		private void BTN_OKAY_Click(object sender, EventArgs e)
		{
			Settings.Global.settings["UpdateURL"] = textBox1.Text;
			Settings.Global.settings["GameDirectory"] = textBox2.Text;
			Settings.Global.WriteSettingsFile();
			this.Close();
		}

		private void BTN_Cancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void BTN_GameDIR_Click(object sender, EventArgs e)
		{
			string dir = FilePicker.SelectFolder(Program.GetPath());
			if (dir.Length > 0)
			{
				textBox2.Text = dir;
			}
		}

	}
}
