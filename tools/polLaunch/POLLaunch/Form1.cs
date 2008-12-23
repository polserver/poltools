using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POLLaunch.Console;
using System.IO;

namespace POLLaunch
{
    public partial class Form1 : Form
	{
        MyConsole POLConsole = null;
        bool Ctrl, Alt, Shift; // small kludge... will it work?

		public Form1()
		{
			InitializeComponent();
			checkBox1.Checked = Settings.Global.ToBoolean((string)Settings.Global.Properties["ShowPOLTabOnStart"]);
            if (checkBox1.Checked && verifyConfiguration())
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
            // We only run this if it's Checked, which means the state changed it to
            // checked either by the user clicking on it, or Code changing it.
            if (checkBox1.Checked)
            {
                if (!verifyConfiguration())
                {
                    MessageBox.Show("You have not set your Configuration yet. Please do so before continuing.");
                    checkBox1.Checked = false;
                }
                else
                {
                    Settings.Global.Properties["ShowPOLTabOnStart"] = checkBox1.Checked.ToString();
                }
            }
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

        private void BTN_StartPOL_Click(object sender, EventArgs e)
        {
            if (this.POLConsole != null)
            { // already running
                MessageBox.Show("POL's already running"); // How?!
                return;
            }

            string exepath = Settings.Global.Properties["POLExePath"];
            string dirpath = Settings.Global.Properties["POLPath"];

            if (!File.Exists(exepath))
            {
                MessageBox.Show("File does not exist in POLExePath!");
                return;
            }

            if (!Directory.Exists(dirpath))
            {
                dirpath = Path.GetDirectoryName(exepath);
            }
             
            // begin pol console...          

            this.POLConsole = new MyConsole();
            this.POLConsole.Start(Path.GetFullPath(exepath), Path.GetFullPath(dirpath));
            this.POLConsole.Exited += new EventHandler(POLConsole_Exited);
            this.POLConsole.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(POLConsole_OutputDataReceived);
            BTN_StartPOL.Enabled = false;
        }

        void POLConsole_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data != String.Empty)
                txtPOLConsole.Invoke((MethodInvoker)delegate() { txtPOLConsole.Text += e.Data + System.Environment.NewLine; });
        }

        void POLConsole_Exited(object sender, EventArgs e)
        {
            txtPOLConsole.Invoke((MethodInvoker)delegate() { txtPOLConsole.Text += "<Process exited>" + System.Environment.NewLine; });

            ((MyConsole)sender).Dispose();
            this.POLConsole = null;

            BTN_StartPOL.Invoke((MethodInvoker)delegate() { BTN_StartPOL.Enabled = true; });
        }

        private void txtPOLConsole_KeyUp(object sender, KeyEventArgs e)
        {
            this.Ctrl = e.Control;
            this.Alt = e.Alt;
            this.Shift = e.Shift;
        }
        private void txtPOLConsole_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.POLConsole != null)
            {
                this.POLConsole.Write(e.KeyChar);
            }
            e.Handled = true;
        }

        private void txtPOLConsole_TextChanged(object sender, EventArgs e)
        {
            // Auto scrolling
            txtPOLConsole.Select(txtPOLConsole.Text.Length, 0);
            txtPOLConsole.ScrollToCaret();
        }

        private bool verifyConfiguration()
        {
            if (Settings.Global.Properties.Count < 5)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Verifies the Configuration has been set before allowing the user to change
        /// tabs in the Control TabControl.
        /// </summary>
        /// <param name="sender">Windows Form default object sender</param>
        /// <param name="e">Windows Form default EventArgs e</param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name.ToString() != "tabPage1" && !verifyConfiguration())
            {
                MessageBox.Show("You have not set your Configuration yet. Please do so before continuing.");
                tabControl1.SelectTab(0);
            }
        }
	}
}
