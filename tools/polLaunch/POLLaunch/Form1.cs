using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip.Compression;
using POLLaunch.Console;
using POLLaunch.DataBackup;
using POLUtils.AuxSvc;
using POLUtils.ECompile;
using POLUtils.PackUnpack;
using POLUtils.UOConvert;
using POLUtils.UOConvert.UOCConfigFiles;
using POLUtils.UOConvert.UOCRealms;

namespace POLLaunch
{
    public partial class Form1 : Form
    {
        #region Main Variables
        MyConsole POLConsole = null;
        MyConsole UOCConsole = null;
        MyConsole ECConsole = null;
        EConfig ECompileCFG = null;
        bool Ctrl, Alt, Shift; // small kludge... will it work?
        #endregion

        #region Main Form Initializer Code
        public Form1()
		{
			InitializeComponent();
			CB_StraightToPOL.Checked = Settings.Global.ToBoolean((string)Settings.Global.Properties["ShowPOLTabOnStart"]);
            if (CB_StraightToPOL.Checked && verifyConfiguration())
				tabControl1.SelectedIndex = tabControl1.TabPages.IndexOfKey("tabPage6");
            LBX_CreateCmdlevel.SelectedIndex = 0;
            LBX_CreateExpansion.SelectedIndex = 0;
            TB_MULFilePath.Text = (string)Settings.Global.Properties["UOPath"];
            InitializeDataBackup();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

        }
        #endregion

        #region Configuration Verification Code
        /// <summary>
        ///     Counts the Properties set. If not enough set, they never loaded
        ///     the Configuration Form and set them! BAD DOG!
        /// </summary>
        /// <returns>False if Configurations never been set</returns>
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
        #endregion

        #region Menu Strip Stuff
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
        #endregion

        #region Initial Tab Code
        private void CB_StraightToPOL_CheckStateChanged(object sender, EventArgs e)
		{
            // We only run this if it's Checked, which means the state changed it to
            // checked either by the user clicking on it, or Code changing it.
            Settings.Global.Properties["ShowPOLTabOnStart"] = CB_StraightToPOL.Checked.ToString();
            if (CB_StraightToPOL.Checked)
            {
                if (!verifyConfiguration())
                {
                    MessageBox.Show("You have not set your Configuration yet. Please do so before continuing.");
                    CB_StraightToPOL.Checked = false;
                }
                else
                {
                    Settings.Global.Properties["ShowPOLTabOnStart"] = CB_StraightToPOL.Checked.ToString();
                }
            }
        }
        #endregion

        #region Run Tests Tab Code
        private void BTN_RunTests_Click(object sender, EventArgs e)
		{
			TB_RunTests.Text = "";
			Cursor = Cursors.WaitCursor;
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = 5;
            ProgressBar.Visible = true;
            ProgressBar.Step = 1;
			
			TB_RunTests.Text += "Analyzing setup in '" + Settings.Global.Properties["POLPath"] + "'" + Environment.NewLine;
			POLChecks.RealmChecks(ref TB_RunTests, ref ProgressBar);
			POLChecks.ScriptChecks(ref TB_RunTests, ref ProgressBar);

			Cursor = Cursors.Default;
            ProgressBar.Visible = false;
            ProgressBar.Value = 0;
        }
        #endregion

        #region UOConvert Tab Code
            #region Realm Checkbox Code
        private void CB_BritT2AMap_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_BritMLMap.Checked)
            {
                CB_BritMLDif.Enabled = true;
                if (CB_BritT2AMap.Checked)
                {
                    CB_BritMLMap.Checked = false;
                    CB_BritMLDif.Checked = false;
                    MessageBox.Show("You cannot have both Britannia (The Second Age) and Britannia (Mondain's Legacy) checked.", "Different Map Sizes");
                }
            }
            if (CB_BritT2AMap.Checked)
                CB_BritT2ADif.Enabled = true;
            else
            {
                CB_BritT2ADif.Enabled = false;
                CB_BritT2ADif.Checked = false;
            }
        }

        private void CB_BritMLMap_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_BritT2AMap.Checked)
            {
                if (CB_BritMLMap.Checked)
                {
                    CB_BritT2AMap.Checked = false;
                    CB_BritT2ADif.Checked = false;
                    MessageBox.Show("You cannot have both Britannia (The Second Age) and Britannia (Mondain's Legacy) checked.", "Different Map Sizes");
                }
            }
            if (CB_BritMLMap.Checked)
                CB_BritMLDif.Enabled = true;
            else
            {
                CB_BritMLDif.Enabled = false;
                CB_BritMLDif.Checked = false;
            }
        }

        private void CB_TramMap_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_TramMap.Checked)
                CB_TramDif.Enabled = true;
            else
            {
                CB_TramDif.Enabled = false;
                CB_TramDif.Checked = false;
            }

        }

        private void CB_IlshMap_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_IlshMap.Checked)
                CB_IlshDif.Enabled = true;
            else
            {
                CB_IlshDif.Enabled = false;
                CB_IlshDif.Checked = false;
            }
        }

        private void CB_MalMap_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_MalMap.Checked)
                CB_MalDif.Enabled = true;
            else
            {
                CB_MalDif.Enabled = false;
                CB_MalDif.Checked = false;
            }
        }

        private void CB_TokMap_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_TokMap.Checked)
                CB_TokDif.Enabled = true;
            else
            {
                CB_TokDif.Enabled = false;
                CB_TokDif.Checked = false;
            }
        }
            #endregion

            #region UOConvert Console Code
        private void BTN_UOConvert_Click(object sender, EventArgs e)
        {
            BuildCommandLines();
            if (PL_UOConvert.BuildList.Count < 1)
            {
                MessageBox.Show("You didn't select anything to convert!");
                return;
            }

            TB_UOCOutput.Text = " ";
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = PL_UOConvert.BuildList.Count;
            ProgressBar.Visible = true;
            ProgressBar.Step = 1;

            RunUOConvert();
        }

        private void BuildCommandLines()
        {
            if (CB_Multis.Checked)
            {
                Multis MultiCfg = new Multis();
                PL_UOConvert.BuildList.Add(MultiCfg.GetUOCCommand());
            }
            if (CB_LandTiles.Checked)
            {
                Landtiles LandCfg = new Landtiles();
                PL_UOConvert.BuildList.Add(LandCfg.GetUOCCommand());
            }
            if (CB_TileData.Checked)
            {
                Tiledata TileCfg = new Tiledata();
                PL_UOConvert.BuildList.Add(TileCfg.GetUOCCommand());
            }
            if (CB_BritT2AMap.Checked)
            {
                BritanniaT2A Brit = new BritanniaT2A();
                Brit.UseDif = CB_BritT2ADif.Checked;
                PL_UOConvert.BuildList.Add(Brit.GetUOCMapCommand());
                PL_UOConvert.BuildList.Add(Brit.GetUOCStaticCommand());
                PL_UOConvert.BuildList.Add(Brit.GetUOCMapTileCommand());
            }
            if (CB_BritMLMap.Checked)
            {
                BritanniaML BritML = new BritanniaML();
                BritML.UseDif = CB_BritMLDif.Checked;
                PL_UOConvert.BuildList.Add(BritML.GetUOCMapCommand());
                PL_UOConvert.BuildList.Add(BritML.GetUOCStaticCommand());
                PL_UOConvert.BuildList.Add(BritML.GetUOCMapTileCommand());
            }
            if (CB_TramMap.Checked)
            {
                Britannia_Alt Tram = new Britannia_Alt();
                Tram.UseDif = CB_TramDif.Checked;
                PL_UOConvert.BuildList.Add(Tram.GetUOCMapCommand());
                PL_UOConvert.BuildList.Add(Tram.GetUOCStaticCommand());
                PL_UOConvert.BuildList.Add(Tram.GetUOCMapTileCommand());
            }
            if (CB_IlshMap.Checked)
            {
                Ilshenar Ilsh = new Ilshenar();
                Ilsh.UseDif = CB_IlshDif.Checked;
                PL_UOConvert.BuildList.Add(Ilsh.GetUOCMapCommand());
                PL_UOConvert.BuildList.Add(Ilsh.GetUOCStaticCommand());
                PL_UOConvert.BuildList.Add(Ilsh.GetUOCMapTileCommand());
            }
            if (CB_MalMap.Checked)
            {
                Malas Mal = new Malas();
                Mal.UseDif = CB_MalDif.Checked;
                PL_UOConvert.BuildList.Add(Mal.GetUOCMapCommand());
                PL_UOConvert.BuildList.Add(Mal.GetUOCStaticCommand());
                PL_UOConvert.BuildList.Add(Mal.GetUOCMapTileCommand());
            }
            if (CB_TokMap.Checked)
            {
                Tokuno Tok = new Tokuno();
                Tok.UseDif = CB_TokDif.Checked;
                PL_UOConvert.BuildList.Add(Tok.GetUOCMapCommand());
                PL_UOConvert.BuildList.Add(Tok.GetUOCStaticCommand());
                PL_UOConvert.BuildList.Add(Tok.GetUOCMapTileCommand());
            }
        }
        
        void RunUOConvert()
        {
                string exepath = Settings.Global.Properties["UOConvertExePath"];
                string dirpath = Settings.Global.Properties["POLPath"];

                if (!File.Exists(exepath))
                {
                    MessageBox.Show("File does not exist in UOConvertExePath!");
                    return;
                }
                if (!Directory.Exists(dirpath))
                {
                    dirpath = Path.GetDirectoryName(exepath);
                }

                string Cmd = "";
                if (PL_UOConvert.BuildList.Count != 0)
                    Cmd += PL_UOConvert.BuildList[0].ToString();
                if (TB_MULFilePath.Text.Length > 0)
                    Cmd += " uodata=\"" + TB_MULFilePath.Text + "\"";

                this.UOCConsole = new MyConsole();
                this.UOCConsole.Start(Path.GetFullPath(exepath), Path.GetFullPath(dirpath), Cmd);
                this.UOCConsole.Exited += new EventHandler(UOCConsole_Exited);
                this.UOCConsole.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(UOCConsole_OutputDataReceived);
                BTN_UOConvert.Enabled = false;
        }

        void UOCConsole_Exited(object sender, EventArgs e)
        {
            TB_UOCOutput.Invoke((MethodInvoker)delegate() { TB_UOCOutput.Text += "<Conversion Completed>" + System.Environment.NewLine + System.Environment.NewLine; });

            ((MyConsole)sender).Dispose();
            this.UOCConsole = null;

            if (File.Exists(Settings.Global.Properties["POLPath"] + "\\multis.cfg"))
            {
                PL_UOConvert.MoveConfigFile(Settings.Global.Properties["POLPath"] + "\\multis.cfg", Settings.Global.Properties["POLPath"] + "\\config\\multis.cfg");
            }
            if (File.Exists(Settings.Global.Properties["POLPath"] + "\\landtiles.cfg"))
            {
                PL_UOConvert.MoveConfigFile(Settings.Global.Properties["POLPath"] + "\\landtiles.cfg", Settings.Global.Properties["POLPath"] + "\\config\\landtiles.cfg");
            }
            if (File.Exists(Settings.Global.Properties["POLPath"] + "\\tiles.cfg"))
            {
                PL_UOConvert.MoveConfigFile(Settings.Global.Properties["POLPath"] + "\\tiles.cfg", Settings.Global.Properties["POLPath"] + "\\config\\tiles.cfg");
            }

            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate() { ProgressBar.PerformStep(); });
            else
                ProgressBar.PerformStep();

            if (PL_UOConvert.BuildList.Count != 0)
            {
                PL_UOConvert.BuildList.RemoveAt(0);
                if (PL_UOConvert.BuildList.Count != 0)
                    RunUOConvert();
                else
                {
                    BTN_UOConvert.Invoke((MethodInvoker)delegate() { BTN_UOConvert.Enabled = true; });
                    if (this.InvokeRequired)
                        this.Invoke((MethodInvoker)delegate() { ProgressBar.Value = 0; });
                    else
                        ProgressBar.Value = 0;
                    MessageBox.Show("Conversion has completed! See text box for details about the conversion process.");
                    if (this.InvokeRequired)
                        this.Invoke((MethodInvoker)delegate() { ProgressBar.Visible = false; });
                    else
                        ProgressBar.Visible = false;
                }
            }
            else
            {
                BTN_UOConvert.Invoke((MethodInvoker)delegate() { BTN_UOConvert.Enabled = true; });
                if (this.InvokeRequired)
                    this.Invoke((MethodInvoker)delegate() { ProgressBar.Value = 0; });
                else
                    ProgressBar.Value = 0;
                MessageBox.Show("Conversion has completed! See text box for details about the conversion process.");
                if (this.InvokeRequired)
                    this.Invoke((MethodInvoker)delegate() { ProgressBar.Visible = false; });
                else
                    ProgressBar.Visible = false;
            }
        }

        private void TB_UOCOutput_TextChanged(object sender, EventArgs e)
        {
            // Auto scrolling
            TB_UOCOutput.Select(TB_UOCOutput.Text.Length, 0);
            TB_UOCOutput.ScrollToCaret();
        }

        void UOCConsole_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data != String.Empty && e.Data != null)
            {
                // Let's Parse out all the Converting: xx% crap here.
                if (!e.Data.Contains("%"))
                    TB_UOCOutput.Invoke((MethodInvoker)delegate() { TB_UOCOutput.Text += e.Data + System.Environment.NewLine; });
            }
        }
            #endregion

            #region UO Client Data Path Code
        private void BTN_MULBrowse_Click(object sender, EventArgs e)
        {
            TB_MULFilePath.Text = FilePicker.SelectFolder();
        }

        private void TB_MULFilePath_TextChanged(object sender, EventArgs e)
        {
            if (TB_MULFilePath.Text.Length > 0)
            {
                foreach (string MulFile in UOConvert.RequiredMul)
                {
                    if (!File.Exists(TB_MULFilePath.Text + @"\" + MulFile))
                    {
                        MessageBox.Show(TB_MULFilePath.Text + " does not contain any of the required MUL files for UOConvert!");
                        TB_MULFilePath.Text = "";
                        return;
                    }
                }
                Settings.Global.Properties["UOPath"] = TB_MULFilePath.Text;
            }
        }
            #endregion
        #endregion

        #region ECompile Tab Code
            #region ECompile Configurations Panel
        private void BTN_EcompileLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ECompileCFG = new EConfig();
                ECompileCFG.LoadConfig(Settings.Global.Properties["ECompileCfgPath"]);

                foreach (CheckBox ThisBox in PNL_ECompileFlags.Controls)
                {
                    ThisBox.Checked = Settings.Global.ToBoolean(ECompileCFG.Option(ThisBox.Name.Substring(15)));
                    Settings.Global.Ecompile[ThisBox.Name.Substring(15)] = ECompileCFG.Option(ThisBox.Name.Substring(15));
                }
                foreach (TextBox ThisBox in PNL_ECompilePaths.Controls)
                {
                    ThisBox.Text = ECompileCFG.Option(ThisBox.Name.Substring(11));
                    Settings.Global.Ecompile[ThisBox.Name.Substring(11)] = ECompileCFG.Option(ThisBox.Name.Substring(11));
                }
                foreach (TextBox ThisBox in PNL_ECompilePathsEditTBS.Controls)
                {
                    ThisBox.Text = ECompileCFG.Option(ThisBox.Name.Substring(20));
                    Settings.Global.Ecompile[ThisBox.Name.Substring(20)] = ECompileCFG.Option(ThisBox.Name.Substring(20));
                }
            }
            catch (Exception ex)
            {
                ExceptionBox.ExceptionForm tmp = new ExceptionBox.ExceptionForm(ref ex);
                tmp.ShowDialog(this);
            }
        }

        private void BTN_ECompileSave_Click(object sender, EventArgs e)
        {
            if (ECompileCFG == null)
            {
                try
                {
                    ECompileCFG = new EConfig();
                    ECompileCFG.LoadConfig(Settings.Global.Properties["ECompileCfgPath"]);
                }
                catch (Exception ex)
                {
                    ExceptionBox.ExceptionForm tmp = new ExceptionBox.ExceptionForm(ref ex);
                    tmp.ShowDialog(this);
                }
            }
            foreach (CheckBox ThisBox in PNL_ECompileFlags.Controls)
            {
                ECompileCFG.Option(ThisBox.Name.Substring(15), ThisBox.Checked);
            }

            foreach (TextBox ThisBox in PNL_ECompilePaths.Controls)
            {
                ECompileCFG.Option(ThisBox.Name.Substring(11), ThisBox.Text);
            }

            ECompileCFG.SaveConfig();
        }

        private void BTN_ECompilePathsEdit_Click(object sender, EventArgs e)
        {
            GB_ECompilePathsEdit.Visible = true;
            GB_ECompilePathsEdit.BringToFront();
            BTN_ECompile.Enabled = false;
            BTN_ECompile.Visible = false;
        }

        private void BTN_ECompilePackageRoots_Click(object sender, EventArgs e)
        {
            if (ECompileCFG == null)
            {
                DialogResult result = MessageBox.Show(this, "We need to load ECompile.Cfg for this option. Proceed?", "Load ECompile.Cfg", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                    return;
                ECompileCFG.LoadConfig(Settings.Global.Properties["ECompileCfgPath"]);
            }
            GB_PackageRootEditor.Visible = true;
            GB_PackageRootEditor.BringToFront();
            BTN_ECompile.Enabled = false;
            BTN_ECompile.Visible = false;

            List<string> MyPackageRoots = ECompileCFG.GetPackageRoots();
            foreach (string PathName in MyPackageRoots)
            {
                object[] PathObject = new object[] { PathName.ToString() };
                int RowIndex = DGV_PackageRoot.Rows.Add(PathObject);
            }
        }
            #endregion

            #region ECompile Paths Editing Panel Code
        private void BTN_ECompilePathsEditDone_Click(object sender, EventArgs e)
        {
            if (TB_ECompilePathsEditModuleDirectory.Text.Length > 0)
                TB_ECompileModuleDirectory.Text = TB_ECompilePathsEditModuleDirectory.Text;
            else
                TB_ECompilePathsEditModuleDirectory.Text = TB_ECompileModuleDirectory.Text;
            if (TB_ECompilePathsEditIncludeDirectory.Text.Length > 0)
                TB_ECompileIncludeDirectory.Text = TB_ECompilePathsEditIncludeDirectory.Text;
            else
                TB_ECompilePathsEditIncludeDirectory.Text = TB_ECompileIncludeDirectory.Text;
            if (TB_ECompilePathsEditPolScriptRoot.Text.Length > 0)
                TB_ECompilePolScriptRoot.Text = TB_ECompilePathsEditPolScriptRoot.Text;
            else
                TB_ECompilePathsEditPolScriptRoot.Text = TB_ECompilePolScriptRoot.Text;
            GB_ECompilePathsEdit.Visible = false;
            GB_ECompilePathsEdit.SendToBack();
            BTN_ECompile.Enabled = true;
            BTN_ECompile.Visible = true;
        }

        private void BTN_ECompileEditPathsModules_Click(object sender, EventArgs e)
        {
            // We don't set the main TB's Text until Finished button is clicked.
            TB_ECompilePathsEditModuleDirectory.Text = FilePicker.SelectFolder();
        }

        private void BTN_ECompileEditPathsIncludes_Click(object sender, EventArgs e)
        {
            // We don't set the main TB's Text until Finished button is clicked.
            TB_ECompilePathsEditIncludeDirectory.Text = FilePicker.SelectFolder();
        }

        private void BTN_ECompileEditPathsScripts_Click(object sender, EventArgs e)
        {
            // We don't set the main TB's Text until Finished button is clicked.
            TB_ECompilePathsEditPolScriptRoot.Text = FilePicker.SelectFolder();
        }
            #endregion

            #region Package Root Editor Code
        private void BTN_ECompilePackageRootEditorFinished_Click(object sender, EventArgs e)
        {
            GB_PackageRootEditor.Visible = false;
            GB_PackageRootEditor.SendToBack();
            BTN_ECompile.Enabled = true;
            BTN_ECompile.Visible = true;

            // Now let's store all this useless, I mean useful, information in the
            // PackageRoot Storage.
            ECompileCFG.RemoveAllPackageRootItems(); 
            foreach (DataGridViewRow CurrentRow in DGV_PackageRoot.Rows)
            {
                if (CurrentRow.Cells["PackageRootPath"].Value != null)
                    ECompileCFG.AddPackageRootItem(CurrentRow.Cells["PackageRootPath"].Value.ToString());
            }

            // After all is said and done, let's erase the Data, since no longer needed.
            // This will help ensure variables are loaded correctly after loading a new cfg
            // without a lot of kludging around.
            DGV_PackageRoot.Rows.Clear();
        }

        /// <summary>
        ///     Handles Button Clicks in the Cells for Browse/Delete/Add
        /// </summary>
        /// <param name="sender">Grid Form</param>
        /// <param name="e">DataGridViewCell Event Arguments</param>
        private void DGV_PackageRoot_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            if (DGV_PackageRoot.Columns[e.ColumnIndex].Name == "PackageRootBrowseButton")
            {
                string PathFolder = FilePicker.SelectFolder();
                if (PathFolder.Length > 1)
                    DGV_PackageRoot.Rows[e.RowIndex].Cells["PackageRootPath"].Value = PathFolder;
                return;
            }
            if (DGV_PackageRoot.Columns[e.ColumnIndex].Name == "PackageRootDeleteButton")
            {
                if (DGV_PackageRoot.Rows.Count == 1)
                {
                    MessageBox.Show("You cannot remove all entries from the Package Roots. You must have at least 1");
                    return;
                }
                DialogResult result = MessageBox.Show(this, "Are you sure you want to delete this entry?", "Delete Package Root Entry", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    DGV_PackageRoot.Rows.RemoveAt(e.RowIndex);
            }
            if (DGV_PackageRoot.Columns[e.ColumnIndex].Name == "PackageRootAddButton")
            {
                DGV_PackageRoot.Rows.Add();
            }
        }

        /// <summary>
        ///     Cancels the Editor Changes and goes back to main portion of the form
        /// </summary>
        /// <param name="sender">Object classed Sender</param>
        /// <param name="e">Button's Event Arguments</param>
        private void BTN_ECompilePackageRootEditorCancel_Click(object sender, EventArgs e)
        {
            GB_PackageRootEditor.Visible = false;
            GB_PackageRootEditor.SendToBack();
            BTN_ECompile.Enabled = true;
            BTN_ECompile.Visible = true;
            DGV_PackageRoot.Rows.Clear();
        }
            #endregion

            #region ECompile Console Code
        private void BTN_ECompile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implemented Yet!");
            return;
        }
            #endregion
        #endregion

        #region POL Tab Code/POLConsole Handling
        #region Buttons/Checkbox Code
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

        private void BTN_StopPOL_Click(object sender, EventArgs e)
        {
            if (this.POLConsole != null)
            {
                try
                {
                    POLConsole.Write(Settings.Global.Properties["POLTabShutdown"]);
                }
                catch (Exception ex)
                {
                    ExceptionBox.ExceptionForm tmp = new ExceptionBox.ExceptionForm(ref ex);
                    tmp.ShowDialog(this);
                }
                return;
            }
        }

        private void CB_POLScrollBar_CheckedChanged(object sender, EventArgs e)
        {
            if (txtPOLConsole.ScrollBars.Equals(ScrollBars.Vertical))
                txtPOLConsole.ScrollBars = ScrollBars.None;
            else
                txtPOLConsole.ScrollBars = ScrollBars.Vertical;
            txtPOLConsole_TextChanged(sender, e);
        }
            #endregion

            #region POL Console Code
        void POLConsole_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (txtPOLConsole.Text.Length > 32000)
                txtPOLConsole.Invoke((MethodInvoker)delegate() { txtPOLConsole.Text = "<clearing POL Output Buffer To Avoid Crash>" + System.Environment.NewLine; });
            if (e.Data != String.Empty && e.Data != null)
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
            #endregion
        #endregion

        #region Account and Data Tab Code
            #region Create Account Code
        private void BTN_CreateAccount_Click(object sender, EventArgs e)
        {
            if (POLConsole == null)
            {
                MessageBox.Show("POL does not appear to be running via POL Launch.");
                return;
            }

            // Checks to make sure everything was filled out, or if can use defaults.
            if (TB_CreateUsername.Text.Length < 3)
            {
                MessageBox.Show("Usernames must be at least 3 characters long.");
                return;
            }
            if (TB_CreatePassword.Text.Length < 3)
            {
                MessageBox.Show("Passwords must be at least 3 characters long.");
                return;
            }
            if (TB_CreateEmail.Text.IndexOf("@") == -1)
                TB_CreateEmail.Text = "default.email@yourdomain.com";
            
            ArrayList NewAccountInfo = new ArrayList {
                                      TB_CreateUsername.Text,
                                      TB_CreatePassword.Text,
                                      LBX_CreateCmdlevel.SelectedIndex,
                                      LBX_CreateExpansion.SelectedIndex,
                                      TB_CreateEmail.Text,
                                      };
            if (TB_CreateAuxPassword.Text.Length > 0)
                NewAccountInfo.Add(TB_CreateAuxPassword.Text);
            AuxSvcConnection CreateAuxSvc = new AuxSvcConnection("localhost", Convert.ToInt32(TB_CreateAccountPort.Text));
            if (!CreateAuxSvc.Active)
            {
                MessageBox.Show("Server appears to be offline. Make sure Port is correct.");
                return;
            }
            CreateAuxSvc.Write(PackUnpack.Pack(NewAccountInfo));
            string RcvString = CreateAuxSvc.Read();
            object ResultObject = PackUnpack.Unpack(RcvString);
            
            // This is where we convert the Object back to what it needs to be.
            // Arrays are converted to ArrayList for storage of both Int and String
            // In the Account Creation only Arrays are being returned. The first
            // Element says "Error" or "Success", and the second is the string reason.
            string ResultMsg = "";
            string CaptionMsg = "";
            if (ResultObject.GetType() == typeof(ArrayList))
            {
                Convert.ChangeType(ResultObject, typeof(ArrayList));
                foreach (object Elem in (ArrayList)ResultObject)
                {
                    string NewElem = Convert.ToString(Elem);
                    if (NewElem.IndexOf("Error") != -1 || NewElem.IndexOf("Success") != -1)
                    {
                        CaptionMsg = Convert.ToString(Elem);
                    }
                    else
                        ResultMsg += NewElem;
                }
            }

            MessageBox.Show(ResultMsg, CaptionMsg);
        }
            #endregion

            #region Data Backup Code
        private void InitializeDataBackup()
        {
            try
            {
                CB_DataBackupData.Checked = bool.Parse(Settings.Global.DataBackup["BackupData"]);
                CB_DataBackupScripts.Checked = bool.Parse(Settings.Global.DataBackup["BackupScripts"]);
                CB_DataBackupRealms.Checked = bool.Parse(Settings.Global.DataBackup["BackupRealms"]);
                CB_DatabackupLogs.Checked = bool.Parse(Settings.Global.DataBackup["BackupLogs"]);

                if (Settings.Global.DataBackup["ArchiveType"].Contains("GZip"))
                    RD_DataBackupGZip.Checked = true;
                else
                    RD_DataBackupZip.Checked = true;
            }
            catch (Exception)
            {
                // Defaults need to be set!
                CB_DataBackupData.Checked = true;
                CB_DataBackupScripts.Checked = true;
                CB_DataBackupRealms.Checked = true;
                CB_DatabackupLogs.Checked = true;
                RD_DataBackupZip.Checked = true;
            }

            if (Settings.Global.DataBackup["DataBackupPath"] != null)
                TB_DataBackupPath.Text = Settings.Global.DataBackup["DataBackupPath"];
            else
            {
                if (Settings.Global.Properties["POLPath"] != null)
                    TB_DataBackupPath.Text = Settings.Global.Properties["POLPath"];
                else
                    TB_DataBackupPath.Text = @"C:\";
            }
        }

        private void BTN_DataBackupSavePath_Click(object sender, EventArgs e)
        {
            TB_DataBackupPath.Text = FilePicker.SelectFolder();
            if (TB_DataBackupPath.Text.Length > 0)
                Settings.Global.DataBackup["DataBackupPath"] = TB_DataBackupPath.Text;
        }

        private void RD_DataBackupGZip_CheckedChanged(object sender, EventArgs e)
        {
            if (RD_DataBackupGZip.Checked)
            {
                Settings.Global.DataBackup["ArchiveType"] = "GZip";
            }
        }
        
        private void RD_DataBackupZip_CheckedChanged(object sender, EventArgs e)
        {
            if (RD_DataBackupZip.Checked)
            {
                Settings.Global.DataBackup["ArchiveType"] = "Zip";
            }
        }

        private void CB_DataBackupData_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Global.DataBackup["BackupData"] = CB_DataBackupData.Checked.ToString();
        }

        private void CB_DataBackupScripts_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Global.DataBackup["BackupScripts"] = CB_DataBackupScripts.Checked.ToString();
        }

        private void CB_DataBackupRealms_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Global.DataBackup["BackupRealms"] = CB_DataBackupRealms.Checked.ToString();
        }

        private void CB_DatabackupLogs_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Global.DataBackup["BackupLogs"] = CB_DatabackupLogs.Checked.ToString();
        }

        private void TB_DataBackupPath_TextChanged(object sender, EventArgs e)
        {
            if (TB_DataBackupPath.Text.Length > 0)
                Settings.Global.DataBackup["DataBackupPath"] = TB_DataBackupPath.Text;
        }

        private void BTN_DataBackupBackupNow_Click(object sender, EventArgs e)
        {
            // Oh God Help Us! Dib is coding some crazy crap :(
            Cursor = Cursors.WaitCursor;

            ZipFileArchiver zf = new ZipFileArchiver();

            // We want to append the DateTimeTicks string to the end to make
            // each unique based on Exact time of execution.

            zf.Execute(Settings.Global.DataBackup["DataBackupPath"] + @"\PLBackup_" + DateTime.Now.Ticks.ToString() + ".zip", ref ProgressBar);

            Cursor = Cursors.Default;

        }
            #endregion
        #endregion

    }
}
