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

        private void CB_BritT2AMap_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_BritMLMap.Checked)
            {
                if (CB_BritT2AMap.Checked)
                {
                    CB_BritMLMap.Checked = false;
                    MessageBox.Show("You cannot have both Britannia (The Second Age) and Britannia (Mondain's Legacy) checked.", "Different Map Sizes");
                }
            }
        }

        private void CB_BritMLMap_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_BritT2AMap.Checked)
            {
                if (CB_BritMLMap.Checked)
                {
                    CB_BritT2AMap.Checked = false;
                    MessageBox.Show("You cannot have both Britannia (The Second Age) and Britannia (Mondain's Legacy) checked.", "Different Map Sizes");
                }
            }

        }

        private void BTN_UOConvert_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implemented Yet! Check back Later.");
/*            BuildCommandLines();
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = UOConvert.BuildList.Count;
            ProgressBar.Visible = true;
            ProgressBar.Step = 1;
 */
        }

        private void BuildCommandLines()
        {
/*            if (CB_Multis.Checked)
            {
                Multis MultiCfg = new Multis();
                UOConvert.BuildList.Add(MultiCfg.GetUOCCommand());
            }
            if (CB_LandTiles.Checked)
            {
                Landtiles LandCfg = new Landtiles();
                UOConvert.BuildList.Add(LandCfg.GetUOCCommand());
            }
            if (CB_TileData.Checked)
            {
                Tiledata TileCfg = new Tiledata();
                UOConvert.BuildList.Add(TileCfg.GetUOCCommand());
            }
            if (CB_BritT2AMap.Checked)
            {
                BritanniaT2A Brit = new BritanniaT2A();
                Brit.UseDif = CB_BritT2ADif.Checked;
                UOConvert.BuildList.Add(Brit.GetUOCMapCommand());
                UOConvert.BuildList.Add(Brit.GetUOCStaticCommand());
                UOConvert.BuildList.Add(Brit.GetUOCMapTileCommand());
            }
            if (CB_BritMLMap.Checked)
            {
                BritanniaML BritML = new BritanniaML();
                BritML.UseDif = CB_BritMLDif.Checked;
                UOConvert.BuildList.Add(BritML.GetUOCMapCommand());
                UOConvert.BuildList.Add(BritML.GetUOCStaticCommand());
                UOConvert.BuildList.Add(BritML.GetUOCMapTileCommand());
            }
            if (CB_TramMap.Checked)
            {
                Britannia_Alt Tram = new Britannia_Alt();
                Tram.UseDif = CB_TramDif.Checked;
                UOConvert.BuildList.Add(Tram.GetUOCMapCommand());
                UOConvert.BuildList.Add(Tram.GetUOCStaticCommand());
                UOConvert.BuildList.Add(Tram.GetUOCMapTileCommand());
            }
            if (CB_IlshMap.Checked)
            {
                Ilshenar Ilsh = new Ilshenar();
                Ilsh.UseDif = CB_IlshDif.Checked;
                UOConvert.BuildList.Add(Ilsh.GetUOCMapCommand());
                UOConvert.BuildList.Add(Ilsh.GetUOCStaticCommand());
                UOConvert.BuildList.Add(Ilsh.GetUOCMapTileCommand());
            }
            if (CB_MalMap.Checked)
            {
                Malas Mal = new Malas();
                Mal.UseDif = CB_MalDif.Checked;
                UOConvert.BuildList.Add(Mal.GetUOCMapCommand());
                UOConvert.BuildList.Add(Mal.GetUOCStaticCommand());
                UOConvert.BuildList.Add(Mal.GetUOCMapTileCommand());
            }
            if (CB_TokMap.Checked)
            {
                Tokuno Tok = new Tokuno();
                Tok.UseDif = CB_TokDif.Checked;
                UOConvert.BuildList.Add(Tok.GetUOCMapCommand());
                UOConvert.BuildList.Add(Tok.GetUOCStaticCommand());
                UOConvert.BuildList.Add(Tok.GetUOCMapTileCommand());
            }*/
        }

	}
}
