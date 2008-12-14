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

namespace UoFiddler
{
    public partial class UoFiddler : Form
    {
        public static string Version = "2.5";
        public static bool AlternativeDesign = false;
        private Controls.ItemShowAlternative controlItemShowAlt;
        private Controls.TextureAlternative controlTextureAlt;
        private Controls.LandTilesAlternative controlLandTilesAlt;
        private Dictionary<string, bool> LoadedUltimaClass = new Dictionary<string, bool>();

        public UoFiddler()
        {
            InitializeComponent();
            Versionlabel.Text = "Version " + Version;
            if (AlternativeDesign)
            {
                this.Items.Controls.Clear();
                this.controlItemShow.Dispose();
                this.controlItemShowAlt = new Controls.ItemShowAlternative();
                this.controlItemShowAlt.Dock = System.Windows.Forms.DockStyle.Fill;
                this.controlItemShowAlt.Location = new System.Drawing.Point(3, 3);
                this.controlItemShowAlt.Name = "controlItemShow";
                this.controlItemShowAlt.Size = new System.Drawing.Size(613, 318);
                this.controlItemShowAlt.TabIndex = 0;
                this.Items.Controls.Add(this.controlItemShowAlt);
                this.Items.PerformLayout();

                this.Texture.Controls.Clear();
                this.controlTexture.Dispose();
                this.controlTextureAlt = new Controls.TextureAlternative();
                this.controlTextureAlt.Dock = System.Windows.Forms.DockStyle.Fill;
                this.controlTextureAlt.Location = new System.Drawing.Point(3, 3);
                this.controlTextureAlt.Name = "controlTexture";
                this.controlTextureAlt.Size = new System.Drawing.Size(613, 318);
                this.controlTextureAlt.TabIndex = 0;
                this.Texture.Controls.Add(this.controlTextureAlt);
                this.Texture.PerformLayout();

                this.LandTiles.Controls.Clear();
                this.controlLandTiles.Dispose();
                this.controlLandTilesAlt = new Controls.LandTilesAlternative();
                this.controlLandTilesAlt.Dock = System.Windows.Forms.DockStyle.Fill;
                this.controlLandTilesAlt.Location = new System.Drawing.Point(3, 3);
                this.controlLandTilesAlt.Name = "controlLandTiles";
                this.controlLandTilesAlt.Size = new System.Drawing.Size(613, 318);
                this.controlLandTilesAlt.TabIndex = 0;
                this.LandTiles.Controls.Add(this.controlLandTilesAlt);
                this.LandTiles.PerformLayout();
            }

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
            if (AlwaysOnTopMenuitem.Checked)
                this.TopMost = true;
            else
                this.TopMost = false;
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
            if (AlternativeDesign)
                this.controlItemShowAlt.Reload();
            else
                this.controlItemShow.Reload();
            if (AlternativeDesign)
                this.controlLandTilesAlt.Reload();
            else
                this.controlLandTiles.Reload();
            if (AlternativeDesign)
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

    }
}