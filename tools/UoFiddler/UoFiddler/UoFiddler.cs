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
        public static string Version = "2.8b";
        private FiddlerControls.ItemShowAlternative controlItemShowAlt;
        private FiddlerControls.TextureAlternative controlTextureAlt;
        private FiddlerControls.LandTilesAlternative controlLandTilesAlt;
        private Dictionary<string, bool> LoadedUltimaClass = new Dictionary<string, bool>();
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
            if ((string)tabControl2.SelectedTab.Tag == "Multis") // Multis
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
            else if ((string)tabControl2.SelectedTab.Tag == "Animations") // Animations
            {
                if (!LoadedUltimaClass["Animations"])
                    LoadedUltimaClass["Animations"] = true;
                if (!LoadedUltimaClass["Hues"])
                    LoadedUltimaClass["Hues"] = true;
            }
            else if ((string)tabControl2.SelectedTab.Tag == "Items") // Items
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
            else if ((string)tabControl2.SelectedTab.Tag == "LandTiles") // LandTiles
            {
                if (!LoadedUltimaClass["TileData"])
                    LoadedUltimaClass["TileData"] = true;
                if (!LoadedUltimaClass["Art"])
                    LoadedUltimaClass["Art"] = true;
            }
            else if ((string)tabControl2.SelectedTab.Tag == "Texture") // Texture
            {
                if (!LoadedUltimaClass["Texture"])
                    LoadedUltimaClass["Texture"] = true;
            }
            else if ((string)tabControl2.SelectedTab.Tag == "Gumps") // Gumps
            {
                if (!LoadedUltimaClass["Gumps"])
                    LoadedUltimaClass["Gumps"] = true;
            }
            else if ((string)tabControl2.SelectedTab.Tag == "Sounds") // Sounds
            {
                if (!LoadedUltimaClass["Sound"])
                    LoadedUltimaClass["Sound"] = true;
            }
            else if ((string)tabControl2.SelectedTab.Tag == "Hue") // Hues
            {
                if (!LoadedUltimaClass["Hues"])
                    LoadedUltimaClass["Hues"] = true;
            }
            else if ((string)tabControl2.SelectedTab.Tag == "Fonts") // Fonts
            {
                if (!LoadedUltimaClass["ASCIIFont"])
                    LoadedUltimaClass["ASCIIFont"] = true;
                if (!LoadedUltimaClass["UnicodeFont"])
                    LoadedUltimaClass["UnicodeFont"] = true;
            }
            else if ((string)tabControl2.SelectedTab.Tag == "Cliloc") // Cliloc
            {
            }
            else if ((string)tabControl2.SelectedTab.Tag == "map") // Map
            {
                if (!LoadedUltimaClass["Map"])
                    LoadedUltimaClass["Map"] = true;
            }
            else if ((string)tabControl2.SelectedTab.Tag == "Light") // Light
            {
                if (!LoadedUltimaClass["Light"])
                    LoadedUltimaClass["Light"] = true;
            }
            else if ((string)tabControl2.SelectedTab.Tag == "Dress") // Dress
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
            else if ((string)tabControl2.SelectedTab.Tag == "Multimap") // MultiMap
            {
            }
            else if ((string)tabControl2.SelectedTab.Tag == "Skills") // Skills
            {
                if (!LoadedUltimaClass["Skills"])
                    LoadedUltimaClass["Skills"] = true;
            }
            else if ((string)tabControl2.SelectedTab.Tag == "TileData") // TileData
            {
                if (!LoadedUltimaClass["TileData"])
                    LoadedUltimaClass["TileData"] = true;
                if (!LoadedUltimaClass["Art"])
                    LoadedUltimaClass["Art"] = true;
            }
            else if ((string)tabControl2.SelectedTab.Tag == "Speech") // Speech
            {
                if (!LoadedUltimaClass["Speech"])
                    LoadedUltimaClass["Speech"] = true;
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
        /// switches Alternative Design
        /// </summary>
        public static void ChangeDesign()
        {
            if (FiddlerControls.Options.DesignAlternative)
            {
                refmarker.Items.Controls.Clear();
                refmarker.controlItemShow.Dispose();
                refmarker.controlItemShowAlt = new FiddlerControls.ItemShowAlternative();
                refmarker.controlItemShowAlt.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlItemShowAlt.Location = new System.Drawing.Point(3, 3);
                refmarker.controlItemShowAlt.Name = "controlItemShow";
                refmarker.controlItemShowAlt.Size = new System.Drawing.Size(613, 318);
                refmarker.controlItemShowAlt.TabIndex = 0;
                refmarker.Items.Controls.Add(refmarker.controlItemShowAlt);
                refmarker.Items.PerformLayout();

                refmarker.Texture.Controls.Clear();
                refmarker.controlTexture.Dispose();
                refmarker.controlTextureAlt = new FiddlerControls.TextureAlternative();
                refmarker.controlTextureAlt.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlTextureAlt.Location = new System.Drawing.Point(3, 3);
                refmarker.controlTextureAlt.Name = "controlTexture";
                refmarker.controlTextureAlt.Size = new System.Drawing.Size(613, 318);
                refmarker.controlTextureAlt.TabIndex = 0;
                refmarker.Texture.Controls.Add(refmarker.controlTextureAlt);
                refmarker.Texture.PerformLayout();

                refmarker.LandTiles.Controls.Clear();
                refmarker.controlLandTiles.Dispose();
                refmarker.controlLandTilesAlt = new FiddlerControls.LandTilesAlternative();
                refmarker.controlLandTilesAlt.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlLandTilesAlt.Location = new System.Drawing.Point(3, 3);
                refmarker.controlLandTilesAlt.Name = "controlLandTiles";
                refmarker.controlLandTilesAlt.Size = new System.Drawing.Size(613, 318);
                refmarker.controlLandTilesAlt.TabIndex = 0;
                refmarker.LandTiles.Controls.Add(refmarker.controlLandTilesAlt);
                refmarker.LandTiles.PerformLayout();
            }
            else
            {
                if (refmarker.controlItemShowAlt == null)
                    return;
                refmarker.Items.Controls.Clear();
                refmarker.controlItemShowAlt.Dispose();
                refmarker.controlItemShow = new FiddlerControls.ItemShow();
                refmarker.controlItemShow.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlItemShow.Location = new System.Drawing.Point(3, 3);
                refmarker.controlItemShow.Name = "controlItemShow";
                refmarker.controlItemShow.Size = new System.Drawing.Size(613, 318);
                refmarker.controlItemShow.TabIndex = 0;
                refmarker.Items.Controls.Add(refmarker.controlItemShow);
                refmarker.Items.PerformLayout();

                refmarker.Texture.Controls.Clear();
                refmarker.controlTextureAlt.Dispose();
                refmarker.controlTexture = new FiddlerControls.Texture();
                refmarker.controlTexture.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlTexture.Location = new System.Drawing.Point(3, 3);
                refmarker.controlTexture.Name = "controlTexture";
                refmarker.controlTexture.Size = new System.Drawing.Size(613, 318);
                refmarker.controlTexture.TabIndex = 0;
                refmarker.Texture.Controls.Add(refmarker.controlTexture);
                refmarker.Texture.PerformLayout();

                refmarker.LandTiles.Controls.Clear();
                refmarker.controlLandTilesAlt.Dispose();
                refmarker.controlLandTiles = new FiddlerControls.LandTiles();
                refmarker.controlLandTiles.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlLandTiles.Location = new System.Drawing.Point(3, 3);
                refmarker.controlLandTiles.Name = "controlLandTiles";
                refmarker.controlLandTiles.Size = new System.Drawing.Size(613, 318);
                refmarker.controlLandTiles.TabIndex = 0;
                refmarker.LandTiles.Controls.Add(refmarker.controlLandTiles);
                refmarker.LandTiles.PerformLayout();
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
    }
}