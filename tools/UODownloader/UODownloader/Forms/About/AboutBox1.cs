using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace UODownloader.Forms
{
    partial class AboutBox1 : Form
    {
        public AboutBox1()
        {
            InitializeComponent();
            this.Text = String.Format("{0} - About", Program.AssemblyTitle);
            this.labelVersion.Text = "("+String.Format("Version {0}", Program.AssemblyVersion)+")";
			this.textBox1.Clear();
			textBox1.Text = global::UODownloader.Properties.Resources.AboutLicense;
        }

        private void AboutBox1_Load(object sender, EventArgs e)
        {
			textBox1.Select(0, 0);
			if (File.Exists("banner.jpg"))
			{
				try
				{
					this.pictureBox1.Image = new Bitmap("banner.jpg");
				}
				catch (Exception ex)
				{
					ExceptionBox.ExceptionForm tmp = new ExceptionBox.ExceptionForm(ref ex);
					tmp.ShowDialog(this);
				}
			}
			else
			{
				this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
			}
        }
		private void BTN_Close_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void LinkLabelClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			LinkLabel clicked = (LinkLabel)sender;
			if ( clicked.Text.Contains("@") && !clicked.Text.StartsWith(@"http://", true, System.Globalization.CultureInfo.CurrentCulture) )
				System.Diagnostics.Process.Start("mailto:" + clicked.Text.ToString());
			else
				System.Diagnostics.Process.Start(clicked.Text.ToString());
		}

		private void button1_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("msinfo32.exe");
		}
    }
}
