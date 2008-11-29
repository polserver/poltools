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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ultima;
using System.Drawing.Imaging;
using System.Xml;
using System.IO;
using Controls;

namespace UoViewer
{
    public partial class UoViewer : Form
    {
        private static string Version = "2.0b";
        public static bool AlternativeDesign = false;
        private Controls.ItemShowAlternative controlItemShowAlt;
        private Controls.TextureAlternative controlTextureAlt;
        private Controls.LandTilesAlternative controlLandTilesAlt;

        public UoViewer()
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
    }
}