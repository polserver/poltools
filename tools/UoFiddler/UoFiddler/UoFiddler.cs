/***************************************************************************
 *
 * $Author: Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace UoFiddler
{
    public partial class UoFiddler : Form
    {
        public static string Version = "2.9b";
        private FiddlerControls.ItemShowAlternative controlItemShowAlt;
        private FiddlerControls.TextureAlternative controlTextureAlt;
        private FiddlerControls.LandTilesAlternative controlLandTilesAlt;
        private Dictionary<string, bool> LoadedUltimaClass = new Dictionary<string, bool>();
        private static UnDocked[] undockedforms = new UnDocked[18];
        private static UoFiddler refmarker;

        public UoFiddler()
        {
            refmarker = this;
            InitializeComponent();
            Versionlabel.Text = "Version " + Version;
            ChangeDesign();
            LoadExternToolStripMenu();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            LoadedUltimaClass.Add("Animations", false);
            LoadedUltimaClass.Add("Animdata", false);
            LoadedUltimaClass.Add("Art", false);
            LoadedUltimaClass.Add("ASCIIFont", false);
            LoadedUltimaClass.Add("UnicodeFont", false);
            LoadedUltimaClass.Add("Gumps", false);
            LoadedUltimaClass.Add("Hues", false);
            LoadedUltimaClass.Add("Light", false);
            LoadedUltimaClass.Add("Map", false);
            LoadedUltimaClass.Add("Multis", false);
            LoadedUltimaClass.Add("Skills", false);
            LoadedUltimaClass.Add("Sound", false);
            LoadedUltimaClass.Add("Speech", false);
            LoadedUltimaClass.Add("StringList", false);
            LoadedUltimaClass.Add("Texture", false);
            LoadedUltimaClass.Add("TileData", false);
        }

        private PathSettings m_Path = new PathSettings();
        private void click_path(object sender, EventArgs e)
        {
            if (m_Path.IsDisposed)
                m_Path = new PathSettings();
            else
                m_Path.Focus();
            m_Path.TopMost = true;
            m_Path.Show();
        }

        private void onClickAlwaysTop(object sender, EventArgs e)
        {
            this.TopMost=AlwaysOnTopMenuitem.Checked;
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse((string)tabControl2.SelectedTab.Tag) == 1) // Multis
            {
                if (!LoadedUltimaClass["TileData"])
                    LoadedUltimaClass["TileData"] = true;
                if (!LoadedUltimaClass["Art"])
                    LoadedUltimaClass["Art"] = true;
                if (!LoadedUltimaClass["Multis"])
                    LoadedUltimaClass["Multis"] = true;
                if (!LoadedUltimaClass["Hues"])
                    LoadedUltimaClass["Hues"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 2) // Animations
            {
                if (!LoadedUltimaClass["Animations"])
                    LoadedUltimaClass["Animations"] = true;
                if (!LoadedUltimaClass["Hues"])
                    LoadedUltimaClass["Hues"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 3) // Items
            {
                if (!LoadedUltimaClass["TileData"])
                    LoadedUltimaClass["TileData"] = true;
                if (!LoadedUltimaClass["Art"])
                    LoadedUltimaClass["Art"] = true;
                if (!LoadedUltimaClass["Animdata"])
                    LoadedUltimaClass["Animdata"] = true;
                if (!LoadedUltimaClass["Hues"])
                    LoadedUltimaClass["Hues"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 4) // LandTiles
            {
                if (!LoadedUltimaClass["TileData"])
                    LoadedUltimaClass["TileData"] = true;
                if (!LoadedUltimaClass["Art"])
                    LoadedUltimaClass["Art"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 5) // Texture
            {
                if (!LoadedUltimaClass["Texture"])
                    LoadedUltimaClass["Texture"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 6) // Gumps
            {
                if (!LoadedUltimaClass["Gumps"])
                    LoadedUltimaClass["Gumps"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 7) // Sounds
            {
                if (!LoadedUltimaClass["Sound"])
                    LoadedUltimaClass["Sound"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 8) // Hues
            {
                if (!LoadedUltimaClass["Hues"])
                    LoadedUltimaClass["Hues"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 9) // Fonts
            {
                if (!LoadedUltimaClass["ASCIIFont"])
                    LoadedUltimaClass["ASCIIFont"] = true;
                if (!LoadedUltimaClass["UnicodeFont"])
                    LoadedUltimaClass["UnicodeFont"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 10) // Cliloc
            {
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 11) // Map
            {
                if (!LoadedUltimaClass["Map"])
                    LoadedUltimaClass["Map"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 12) // Light
            {
                if (!LoadedUltimaClass["Light"])
                    LoadedUltimaClass["Light"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 13) // Speech
            {
                if (!LoadedUltimaClass["Speech"])
                    LoadedUltimaClass["Speech"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 14) // Skills
            {
                if (!LoadedUltimaClass["Skills"])
                    LoadedUltimaClass["Skills"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 15) // MultiMap
            {
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 16) // Dress
            {
                if (!LoadedUltimaClass["TileData"])
                    LoadedUltimaClass["TileData"] = true;
                if (!LoadedUltimaClass["Art"])
                    LoadedUltimaClass["Art"] = true;
                if (!LoadedUltimaClass["Hues"])
                    LoadedUltimaClass["Hues"] = true;
                if (!LoadedUltimaClass["Animations"])
                    LoadedUltimaClass["Animations"] = true;
                if (!LoadedUltimaClass["Gumps"])
                    LoadedUltimaClass["Gumps"] = true;
            }
            else if (int.Parse((string)tabControl2.SelectedTab.Tag) == 17) // TileData
            {
                if (!LoadedUltimaClass["TileData"])
                    LoadedUltimaClass["TileData"] = true;
                if (!LoadedUltimaClass["Art"])
                    LoadedUltimaClass["Art"] = true;
            }
            
        }

        private void Restart(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            Ultima.Verdata.Initialize();
            if (LoadedUltimaClass["TileData"])
                Ultima.TileData.Initialize();
            if (LoadedUltimaClass["Hues"])
                Ultima.Hues.Initialize();
            if (LoadedUltimaClass["ASCIIFont"])
                Ultima.ASCIIText.Initialize();
            if (LoadedUltimaClass["UnicodeFont"])
                Ultima.UnicodeFonts.Initialize();
            if (LoadedUltimaClass["Animdata"])
                Ultima.Animdata.Initialize();
            if (LoadedUltimaClass["Light"])
                Ultima.Light.Reload();
            if (LoadedUltimaClass["Skills"])
                Ultima.Skills.Reload();
            if (LoadedUltimaClass["Sound"])
                Ultima.Sounds.Initialize();
            if (LoadedUltimaClass["Texture"])
                Ultima.Textures.Reload();
            if (LoadedUltimaClass["Gumps"])
                Ultima.Gumps.Reload();
            if (LoadedUltimaClass["Animations"])
                Ultima.Animations.Reload();
            if (LoadedUltimaClass["Art"])
                Ultima.Art.Reload();
            if (LoadedUltimaClass["Map"])
                Ultima.Map.Reload();
            if (LoadedUltimaClass["Multis"])
                Ultima.Multis.Reload();
            if (LoadedUltimaClass["Speech"])
                Ultima.SpeechList.Initialize();

            this.controlMulti.Reload();
            this.controlAnimations.Reload();
            if (FiddlerControls.Options.DesignAlternative)
                this.controlItemShowAlt.Reload();
            else
                this.controlItemShow.Reload();
            if (FiddlerControls.Options.DesignAlternative)
                this.controlLandTilesAlt.Reload();
            else
                this.controlLandTiles.Reload();
            if (FiddlerControls.Options.DesignAlternative)
                this.controlTextureAlt.Reload();
            else
                this.controlTexture.Reload();
            this.controlGumps.Reload();
            this.controlSound.Reload();
            this.controlHue.Reload();
            this.controlfonts.Reload();
            this.controlCliloc.Reload();
            this.controlmap.Reload();
            this.controlLight.Reload();
            this.controldress.Reload();
            this.controlMultimap.Reload();
            this.controlSkills.Reload();
            this.controlTileData.Reload();
            this.controlspeech.Reload();

            this.Cursor = Cursors.Default;
        }

        private void OnClickAbout(object sender, EventArgs e)
        {
            new AboutBox().Show();
        }

        /// <summary>
        /// Reloads the Extern Tools DropDown <see cref="Options.ExternTools"/>
        /// </summary>
        public static void LoadExternToolStripMenu()
        {
            refmarker.ExternToolsDropDown.DropDownItems.Clear();
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Text = "Manage..";
            item.Click += new System.EventHandler(refmarker.onClickToolManage);
            refmarker.ExternToolsDropDown.DropDownItems.Add(item);
            refmarker.ExternToolsDropDown.DropDownItems.Add(new ToolStripSeparator());
            for (int i = 0; i < Options.ExternTools.Count; i++)
            {
                ExternTool tool = (ExternTool)Options.ExternTools[i];
                item = new ToolStripMenuItem();
                item.Text = tool.Name;
                item.Tag = i;
                item.DropDownItemClicked += new ToolStripItemClickedEventHandler(refmarker.ExternTool_ItemClicked);
                ToolStripMenuItem sub = new ToolStripMenuItem();
                sub.Text = "Start";
                sub.Tag = -1;
                item.DropDownItems.Add(sub);
                item.DropDownItems.Add(new ToolStripSeparator());
                for (int j = 0; j < tool.Args.Count; j++)
                {
                    ToolStripMenuItem arg = new ToolStripMenuItem();
                    arg.Text = (string)tool.ArgsName[j];
                    arg.Tag = j;
                    item.DropDownItems.Add(arg);
                }
                refmarker.ExternToolsDropDown.DropDownItems.Add(item);
            }
        }

        private void ExternTool_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int arginfo = (int)e.ClickedItem.Tag;
            int toolinfo = (int)e.ClickedItem.OwnerItem.Tag;

            if (toolinfo >= 0)
            {
                if (arginfo >= -1)
                {
                    Process P = new Process();
                    ExternTool tool = (ExternTool)Options.ExternTools[toolinfo];
                    P.StartInfo.FileName = tool.FileName;
                    if (arginfo >= 0)
                        P.StartInfo.Arguments = (string)tool.Args[arginfo];
                    try
                    {
                        P.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error starting tool", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error, 
                            MessageBoxDefaultButton.Button1);
                    }
                }
            }
        }

        private ManageTools manageform;
        private void onClickToolManage(object sender, EventArgs e)
        {
            if ((manageform == null) || (manageform.IsDisposed))
            {
                manageform = new ManageTools();
                manageform.TopMost = true;
                manageform.Show();
            }
        }

        private OptionsForm optionsform;
        private void OnClickOptions(object sender, EventArgs e)
        {
            if ((optionsform == null) || (optionsform.IsDisposed))
            {
                optionsform = new OptionsForm();
                optionsform.TopMost = true;
                optionsform.Show();
            }
        }

        /// <summary>
        /// switches Alternative Design aka Hack'n'Slay attack damn...
        /// </summary>
        public static void ChangeDesign()
        {
            if (FiddlerControls.Options.DesignAlternative)
            {
                refmarker.controlItemShow.Dispose();
                refmarker.controlItemShowAlt = new FiddlerControls.ItemShowAlternative();
                refmarker.controlItemShowAlt.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlItemShowAlt.Location = new System.Drawing.Point(3, 3);
                refmarker.controlItemShowAlt.Name = "controlItemShow";
                refmarker.controlItemShowAlt.Size = new System.Drawing.Size(613, 318);
                refmarker.controlItemShowAlt.TabIndex = 0;

                bool done = false;
                foreach (TabPage p in refmarker.tabControl2.TabPages)
                {
                    if (p.Text == "Items")
                    {
                        p.Controls.Clear();
                        p.Controls.Add(refmarker.controlItemShowAlt);
                        p.PerformLayout();
                        done = true;
                    }
                }
                if (!done)
                    undockedforms[3].ChangeControl(refmarker.controlItemShowAlt);

                refmarker.controlTexture.Dispose();
                refmarker.controlTextureAlt = new FiddlerControls.TextureAlternative();
                refmarker.controlTextureAlt.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlTextureAlt.Location = new System.Drawing.Point(3, 3);
                refmarker.controlTextureAlt.Name = "controlTexture";
                refmarker.controlTextureAlt.Size = new System.Drawing.Size(613, 318);
                refmarker.controlTextureAlt.TabIndex = 0;

                done = false;
                foreach (TabPage p in refmarker.tabControl2.TabPages)
                {
                    if (p.Text == "Texture")
                    {
                        p.Controls.Clear();
                        p.Controls.Add(refmarker.controlTextureAlt);
                        p.PerformLayout();
                        done = true;
                    }
                }
                if (!done)
                    undockedforms[5].ChangeControl(refmarker.controlTextureAlt);

                refmarker.controlLandTiles.Dispose();
                refmarker.controlLandTilesAlt = new FiddlerControls.LandTilesAlternative();
                refmarker.controlLandTilesAlt.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlLandTilesAlt.Location = new System.Drawing.Point(3, 3);
                refmarker.controlLandTilesAlt.Name = "controlLandTiles";
                refmarker.controlLandTilesAlt.Size = new System.Drawing.Size(613, 318);
                refmarker.controlLandTilesAlt.TabIndex = 0;

                done = false;
                foreach (TabPage p in refmarker.tabControl2.TabPages)
                {
                    if (p.Text == "LandTiles")
                    {
                        p.Controls.Clear();
                        p.Controls.Add(refmarker.controlLandTilesAlt);
                        p.PerformLayout();
                        done = true;
                    }
                }
                if (!done)
                    undockedforms[4].ChangeControl(refmarker.controlLandTilesAlt);
            }
            else
            {
                if (refmarker.controlItemShowAlt == null)
                    return;
                refmarker.controlItemShowAlt.Dispose();
                refmarker.controlItemShow = new FiddlerControls.ItemShow();
                refmarker.controlItemShow.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlItemShow.Location = new System.Drawing.Point(3, 3);
                refmarker.controlItemShow.Name = "controlItemShow";
                refmarker.controlItemShow.Size = new System.Drawing.Size(613, 318);
                refmarker.controlItemShow.TabIndex = 0;
                bool done = false;
                foreach (TabPage p in refmarker.tabControl2.TabPages)
                {
                    if (p.Text=="Items")
                    {
                        p.Controls.Clear();
                        p.Controls.Add(refmarker.controlItemShow);
                        p.PerformLayout();
                        done = true;
                    }
                }
                if (!done)
                    undockedforms[3].ChangeControl(refmarker.controlItemShow);

                refmarker.controlTextureAlt.Dispose();
                refmarker.controlTexture = new FiddlerControls.Texture();
                refmarker.controlTexture.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlTexture.Location = new System.Drawing.Point(3, 3);
                refmarker.controlTexture.Name = "controlTexture";
                refmarker.controlTexture.Size = new System.Drawing.Size(613, 318);
                refmarker.controlTexture.TabIndex = 0;
                done = false;
                foreach (TabPage p in refmarker.tabControl2.TabPages)
                {
                    if (p.Text == "Texture")
                    {
                        p.Controls.Clear();
                        p.Controls.Add(refmarker.controlTexture);
                        p.PerformLayout();
                        done = true;
                    }
                }
                if (!done)
                    undockedforms[5].ChangeControl(refmarker.controlTexture);

                refmarker.controlLandTilesAlt.Dispose();
                refmarker.controlLandTiles = new FiddlerControls.LandTiles();
                refmarker.controlLandTiles.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlLandTiles.Location = new System.Drawing.Point(3, 3);
                refmarker.controlLandTiles.Name = "controlLandTiles";
                refmarker.controlLandTiles.Size = new System.Drawing.Size(613, 318);
                refmarker.controlLandTiles.TabIndex = 0;
                done = false;
                foreach (TabPage p in refmarker.tabControl2.TabPages)
                {
                    if (p.Text == "LandTiles")
                    {
                        p.Controls.Clear();
                        p.Controls.Add(refmarker.controlLandTiles);
                        p.PerformLayout();
                        done = true;
                    }
                }
                if (!done)
                    undockedforms[4].ChangeControl(refmarker.controlLandTiles);
            }
        }

        /// <summary>
        /// Reloads Itemtab
        /// </summary>
        public static void ReloadItemTab()
        {
            if (FiddlerControls.Options.DesignAlternative)
                refmarker.controlItemShowAlt.ChangeTileSize();
            else
                refmarker.controlItemShow.ChangeTileSize();
        }

        /// <summary>
        /// Updates Map tab
        /// </summary>
        public static void ChangeMapSize()
        {
            if (refmarker.LoadedUltimaClass["Map"])
                Ultima.Map.Reload();
            refmarker.controlmap.Reload();
        }

        /// <summary>
        /// Updates in Map tab the mapnames
        /// </summary>
        public static void ChangeMapNames()
        {
            refmarker.controlmap.ChangeMapNames();
        }

        private void OnClickUndock(object sender, EventArgs e)
        {
            int tag = int.Parse((string)tabControl2.SelectedTab.Tag);
            if (tag > 0)
            {
                undockedforms[tag]=
                    new UnDocked(tabControl2.SelectedTab.Controls[0], 
                        tabControl2.SelectedTab.Text, tag);
                undockedforms[tag].Show();
                tabControl2.TabPages.Remove(tabControl2.SelectedTab);
            }
        }

        /// <summary>
        /// ReDocks closed Form
        /// </summary>
        /// <param name="contr"></param>
        /// <param name="index"></param>
        /// <param name="name"></param>
        public static void ReDock(Control contr, int index, string name)
        {
            bool done = false;
            TabPage p = new TabPage(name);
            p.Tag = index.ToString();
            p.Controls.Add(contr);
            foreach (TabPage page in refmarker.tabControl2.TabPages)
            {
                if (int.Parse((string)page.Tag)>index)
                {
                    refmarker.tabControl2.TabPages.Insert(refmarker.tabControl2.TabPages.IndexOf(page), p);
                    done = true;
                    break;
                }
            }
            if (!done)
                refmarker.tabControl2.TabPages.Add(p);
            refmarker.tabControl2.SelectedTab = p;
            undockedforms[index] = null;
        }
    }
}