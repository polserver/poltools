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
using System.Reflection;
using System.IO;
using PluginInterface;
using Host;

namespace UoFiddler
{
    public partial class UoFiddler : Form
    {
        public static string Version = "3.1b";
        private FiddlerControls.ItemShowAlternative controlItemShowAlt;
        private FiddlerControls.TextureAlternative controlTextureAlt;
        private FiddlerControls.LandTilesAlternative controlLandTilesAlt;
        private static UoFiddler refmarker;

        public UoFiddler()
        {
            refmarker = this;
            InitializeComponent();
            Versionlabel.Text = "Version " + Version;
            ChangeDesign();
            LoadExternToolStripMenu();
            GlobalPlugins.Plugins.FindPlugins(Application.StartupPath + @"\plugins");

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
 
            foreach (Host.Types.AvailablePlugin plug in GlobalPlugins.Plugins.AvailablePlugins)
            {
                if (plug.Loaded)
                {
                    plug.Instance.ModifyPluginToolStrip(this.toolStripDropDownButtonPlugins);
                    plug.Instance.ModifyTabPages(this.tabControl2);
                }
            }
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

        private void Restart(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            Ultima.Verdata.Initialize();
            if (FiddlerControls.Options.LoadedUltimaClass["TileData"])
                Ultima.TileData.Initialize();
            if (FiddlerControls.Options.LoadedUltimaClass["Hues"])
                Ultima.Hues.Initialize();
            if (FiddlerControls.Options.LoadedUltimaClass["ASCIIFont"])
                Ultima.ASCIIText.Initialize();
            if (FiddlerControls.Options.LoadedUltimaClass["UnicodeFont"])
                Ultima.UnicodeFonts.Initialize();
            if (FiddlerControls.Options.LoadedUltimaClass["Animdata"])
                Ultima.Animdata.Initialize();
            if (FiddlerControls.Options.LoadedUltimaClass["Light"])
                Ultima.Light.Reload();
            if (FiddlerControls.Options.LoadedUltimaClass["Skills"])
                Ultima.Skills.Reload();
            if (FiddlerControls.Options.LoadedUltimaClass["Sound"])
                Ultima.Sounds.Initialize();
            if (FiddlerControls.Options.LoadedUltimaClass["Texture"])
                Ultima.Textures.Reload();
            if (FiddlerControls.Options.LoadedUltimaClass["Gumps"])
                Ultima.Gumps.Reload();
            if (FiddlerControls.Options.LoadedUltimaClass["Animations"])
                Ultima.Animations.Reload();
            if (FiddlerControls.Options.LoadedUltimaClass["Art"])
                Ultima.Art.Reload();
            if (FiddlerControls.Options.LoadedUltimaClass["Map"])
                Ultima.Map.Reload();
            if (FiddlerControls.Options.LoadedUltimaClass["Multis"])
                Ultima.Multis.Reload();
            if (FiddlerControls.Options.LoadedUltimaClass["Speech"])
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
            this.controlAnimdata.Reload();

            foreach (Host.Types.AvailablePlugin plug in GlobalPlugins.Plugins.AvailablePlugins)
            {
                if (plug.Loaded)
                    plug.Instance.Reload();
            }
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
                refmarker.controlItemShowAlt = new FiddlerControls.ItemShowAlternative();
                refmarker.controlItemShowAlt.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlItemShowAlt.Location = new System.Drawing.Point(3, 3);
                refmarker.controlItemShowAlt.Name = "controlItemShow";
                refmarker.controlItemShowAlt.Size = new System.Drawing.Size(613, 318);
                refmarker.controlItemShowAlt.TabIndex = 0;
                Control parent = refmarker.controlItemShow.Parent;
                parent.Controls.Clear();
                parent.Controls.Add(refmarker.controlItemShowAlt);
                parent.PerformLayout();
                refmarker.controlItemShow.Dispose();
                
                refmarker.controlTextureAlt = new FiddlerControls.TextureAlternative();
                refmarker.controlTextureAlt.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlTextureAlt.Location = new System.Drawing.Point(3, 3);
                refmarker.controlTextureAlt.Name = "controlTexture";
                refmarker.controlTextureAlt.Size = new System.Drawing.Size(613, 318);
                refmarker.controlTextureAlt.TabIndex = 0;
                parent = refmarker.controlTexture.Parent;
                parent.Controls.Clear();
                parent.Controls.Add(refmarker.controlTextureAlt);
                parent.PerformLayout();
                refmarker.controlTexture.Dispose();

                refmarker.controlLandTilesAlt = new FiddlerControls.LandTilesAlternative();
                refmarker.controlLandTilesAlt.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlLandTilesAlt.Location = new System.Drawing.Point(3, 3);
                refmarker.controlLandTilesAlt.Name = "controlLandTiles";
                refmarker.controlLandTilesAlt.Size = new System.Drawing.Size(613, 318);
                refmarker.controlLandTilesAlt.TabIndex = 0;
                parent = refmarker.controlLandTiles.Parent;
                parent.Controls.Clear();
                parent.Controls.Add(refmarker.controlLandTilesAlt);
                parent.PerformLayout();
                refmarker.controlLandTiles.Dispose();
            }
            else
            {
                if (refmarker.controlItemShowAlt == null)
                    return;
                
                refmarker.controlItemShow = new FiddlerControls.ItemShow();
                refmarker.controlItemShow.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlItemShow.Location = new System.Drawing.Point(3, 3);
                refmarker.controlItemShow.Name = "controlItemShow";
                refmarker.controlItemShow.Size = new System.Drawing.Size(613, 318);
                refmarker.controlItemShow.TabIndex = 0;
                Control parent = refmarker.controlItemShowAlt.Parent;
                parent.Controls.Clear();
                parent.Controls.Add(refmarker.controlItemShow);
                parent.PerformLayout();
                refmarker.controlItemShowAlt.Dispose();
                
                refmarker.controlTexture = new FiddlerControls.Texture();
                refmarker.controlTexture.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlTexture.Location = new System.Drawing.Point(3, 3);
                refmarker.controlTexture.Name = "controlTexture";
                refmarker.controlTexture.Size = new System.Drawing.Size(613, 318);
                refmarker.controlTexture.TabIndex = 0;
                parent = refmarker.controlTextureAlt.Parent;
                parent.Controls.Clear();
                parent.Controls.Add(refmarker.controlTexture);
                parent.PerformLayout();
                refmarker.controlTextureAlt.Dispose();

                refmarker.controlLandTiles = new FiddlerControls.LandTiles();
                refmarker.controlLandTiles.Dock = System.Windows.Forms.DockStyle.Fill;
                refmarker.controlLandTiles.Location = new System.Drawing.Point(3, 3);
                refmarker.controlLandTiles.Name = "controlLandTiles";
                refmarker.controlLandTiles.Size = new System.Drawing.Size(613, 318);
                refmarker.controlLandTiles.TabIndex = 0;
                parent = refmarker.controlLandTilesAlt.Parent;
                parent.Controls.Clear();
                parent.Controls.Add(refmarker.controlLandTiles);
                parent.PerformLayout();
                refmarker.controlLandTilesAlt.Dispose();
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
            if (FiddlerControls.Options.LoadedUltimaClass["Map"])
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
            int tag = (int)tabControl2.SelectedTab.Tag;
            if (tag > 0)
            {
                    new UnDocked(tabControl2.SelectedTab.Controls[0], 
                        tabControl2.SelectedTab.Text, tag).Show();
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
            p.Tag = index;
            p.Controls.Add(contr);
            foreach (TabPage page in refmarker.tabControl2.TabPages)
            {
                if ((int)page.Tag>index)
                {
                    refmarker.tabControl2.TabPages.Insert(refmarker.tabControl2.TabPages.IndexOf(page), p);
                    done = true;
                    break;
                }
            }
            if (!done)
                refmarker.tabControl2.TabPages.Add(p);
            refmarker.tabControl2.SelectedTab = p;
        }

        private ManagePlugins pluginsform;
        private void onClickManagePlugins(object sender, EventArgs e)
        {
            if ((pluginsform == null) || (pluginsform.IsDisposed))
            {
                pluginsform = new ManagePlugins();
                pluginsform.TopMost = true;
                pluginsform.Show();
            }
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            GlobalPlugins.Plugins.ClosePlugins();
        }
    }
}