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

namespace UoViewer
{
    partial class UoViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UoViewer));
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.Start = new System.Windows.Forms.TabPage();
            this.Versionlabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Multis = new System.Windows.Forms.TabPage();
            this.MobGraphic = new System.Windows.Forms.TabPage();
            this.Items = new System.Windows.Forms.TabPage();
            this.LandTiles = new System.Windows.Forms.TabPage();
            this.Texture = new System.Windows.Forms.TabPage();
            this.Gumps = new System.Windows.Forms.TabPage();
            this.Sounds = new System.Windows.Forms.TabPage();
            this.Hue = new System.Windows.Forms.TabPage();
            this.fonts = new System.Windows.Forms.TabPage();
            this.Cliloc = new System.Windows.Forms.TabPage();
            this.map = new System.Windows.Forms.TabPage();
            this.Light = new System.Windows.Forms.TabPage();
            this.Dress = new System.Windows.Forms.TabPage();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.alwaysOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.restartNeededToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlMulti = new Controls.Multis();
            this.controlMobList = new Controls.MobList();
            this.controlItemShow = new Controls.ItemShow();
            this.controlLandTiles = new Controls.LandTiles();
            this.controlTexture = new Controls.Texture();
            this.controlGumps = new Controls.Gump();
            this.controlSound = new Controls.Sounds();
            this.controlHue = new Controls.Hues();
            this.controlfonts = new Controls.Fonts();
            this.controlCliloc = new Controls.Cliloc();
            this.controlmap = new Controls.Map();
            this.controlLight = new Controls.Light();
            this.controldress = new Controls.Dress();
            this.tabControl2.SuspendLayout();
            this.Start.SuspendLayout();
            this.Multis.SuspendLayout();
            this.MobGraphic.SuspendLayout();
            this.Items.SuspendLayout();
            this.LandTiles.SuspendLayout();
            this.Texture.SuspendLayout();
            this.Gumps.SuspendLayout();
            this.Sounds.SuspendLayout();
            this.Hue.SuspendLayout();
            this.fonts.SuspendLayout();
            this.Cliloc.SuspendLayout();
            this.map.SuspendLayout();
            this.Light.SuspendLayout();
            this.Dress.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.Start);
            this.tabControl2.Controls.Add(this.Multis);
            this.tabControl2.Controls.Add(this.MobGraphic);
            this.tabControl2.Controls.Add(this.Items);
            this.tabControl2.Controls.Add(this.LandTiles);
            this.tabControl2.Controls.Add(this.Texture);
            this.tabControl2.Controls.Add(this.Gumps);
            this.tabControl2.Controls.Add(this.Sounds);
            this.tabControl2.Controls.Add(this.Hue);
            this.tabControl2.Controls.Add(this.fonts);
            this.tabControl2.Controls.Add(this.Cliloc);
            this.tabControl2.Controls.Add(this.map);
            this.tabControl2.Controls.Add(this.Light);
            this.tabControl2.Controls.Add(this.Dress);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 25);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(627, 350);
            this.tabControl2.TabIndex = 1;
            // 
            // Start
            // 
            this.Start.Controls.Add(this.Versionlabel);
            this.Start.Controls.Add(this.label1);
            this.Start.Location = new System.Drawing.Point(4, 22);
            this.Start.Name = "Start";
            this.Start.Padding = new System.Windows.Forms.Padding(3);
            this.Start.Size = new System.Drawing.Size(619, 324);
            this.Start.TabIndex = 10;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            // 
            // Versionlabel
            // 
            this.Versionlabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Versionlabel.AutoSize = true;
            this.Versionlabel.Location = new System.Drawing.Point(553, 311);
            this.Versionlabel.Name = "Versionlabel";
            this.Versionlabel.Size = new System.Drawing.Size(42, 13);
            this.Versionlabel.TabIndex = 1;
            this.Versionlabel.Text = "Version";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Lucida Calligraphy", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(130, 119);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "UOViewer by Turley";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Multis
            // 
            this.Multis.Controls.Add(this.controlMulti);
            this.Multis.Location = new System.Drawing.Point(4, 22);
            this.Multis.Name = "Multis";
            this.Multis.Padding = new System.Windows.Forms.Padding(3);
            this.Multis.Size = new System.Drawing.Size(619, 324);
            this.Multis.TabIndex = 1;
            this.Multis.Text = "Multis";
            this.Multis.UseVisualStyleBackColor = true;
            // 
            // MobGraphic
            // 
            this.MobGraphic.Controls.Add(this.controlMobList);
            this.MobGraphic.Location = new System.Drawing.Point(4, 22);
            this.MobGraphic.Name = "MobGraphic";
            this.MobGraphic.Padding = new System.Windows.Forms.Padding(3);
            this.MobGraphic.Size = new System.Drawing.Size(619, 324);
            this.MobGraphic.TabIndex = 0;
            this.MobGraphic.Text = "MobGraphic";
            this.MobGraphic.UseVisualStyleBackColor = true;
            // 
            // Items
            // 
            this.Items.Controls.Add(this.controlItemShow);
            this.Items.Location = new System.Drawing.Point(4, 22);
            this.Items.Name = "Items";
            this.Items.Padding = new System.Windows.Forms.Padding(3);
            this.Items.Size = new System.Drawing.Size(619, 324);
            this.Items.TabIndex = 2;
            this.Items.Text = "Items";
            this.Items.UseVisualStyleBackColor = true;
            // 
            // LandTiles
            // 
            this.LandTiles.Controls.Add(this.controlLandTiles);
            this.LandTiles.Location = new System.Drawing.Point(4, 22);
            this.LandTiles.Name = "LandTiles";
            this.LandTiles.Padding = new System.Windows.Forms.Padding(3);
            this.LandTiles.Size = new System.Drawing.Size(619, 324);
            this.LandTiles.TabIndex = 3;
            this.LandTiles.Text = "LandTiles";
            this.LandTiles.UseVisualStyleBackColor = true;
            // 
            // Texture
            // 
            this.Texture.Controls.Add(this.controlTexture);
            this.Texture.Location = new System.Drawing.Point(4, 22);
            this.Texture.Name = "Texture";
            this.Texture.Padding = new System.Windows.Forms.Padding(3);
            this.Texture.Size = new System.Drawing.Size(619, 324);
            this.Texture.TabIndex = 11;
            this.Texture.Text = "Texture";
            this.Texture.UseVisualStyleBackColor = true;
            // 
            // Gumps
            // 
            this.Gumps.Controls.Add(this.controlGumps);
            this.Gumps.Location = new System.Drawing.Point(4, 22);
            this.Gumps.Name = "Gumps";
            this.Gumps.Padding = new System.Windows.Forms.Padding(3);
            this.Gumps.Size = new System.Drawing.Size(619, 324);
            this.Gumps.TabIndex = 4;
            this.Gumps.Text = "Gumps";
            this.Gumps.UseVisualStyleBackColor = true;
            // 
            // Sounds
            // 
            this.Sounds.Controls.Add(this.controlSound);
            this.Sounds.Location = new System.Drawing.Point(4, 22);
            this.Sounds.Name = "Sounds";
            this.Sounds.Padding = new System.Windows.Forms.Padding(3);
            this.Sounds.Size = new System.Drawing.Size(619, 324);
            this.Sounds.TabIndex = 5;
            this.Sounds.Text = "Sounds";
            this.Sounds.UseVisualStyleBackColor = true;
            // 
            // Hue
            // 
            this.Hue.Controls.Add(this.controlHue);
            this.Hue.Location = new System.Drawing.Point(4, 22);
            this.Hue.Name = "Hue";
            this.Hue.Padding = new System.Windows.Forms.Padding(3);
            this.Hue.Size = new System.Drawing.Size(619, 324);
            this.Hue.TabIndex = 6;
            this.Hue.Text = "Hue";
            this.Hue.UseVisualStyleBackColor = true;
            // 
            // fonts
            // 
            this.fonts.Controls.Add(this.controlfonts);
            this.fonts.Location = new System.Drawing.Point(4, 22);
            this.fonts.Name = "fonts";
            this.fonts.Padding = new System.Windows.Forms.Padding(3);
            this.fonts.Size = new System.Drawing.Size(619, 324);
            this.fonts.TabIndex = 7;
            this.fonts.Text = "Fonts";
            this.fonts.UseVisualStyleBackColor = true;
            // 
            // Cliloc
            // 
            this.Cliloc.Controls.Add(this.controlCliloc);
            this.Cliloc.Location = new System.Drawing.Point(4, 22);
            this.Cliloc.Name = "Cliloc";
            this.Cliloc.Padding = new System.Windows.Forms.Padding(3);
            this.Cliloc.Size = new System.Drawing.Size(619, 324);
            this.Cliloc.TabIndex = 8;
            this.Cliloc.Text = "CliLoc";
            this.Cliloc.UseVisualStyleBackColor = true;
            // 
            // map
            // 
            this.map.Controls.Add(this.controlmap);
            this.map.Location = new System.Drawing.Point(4, 22);
            this.map.Name = "map";
            this.map.Padding = new System.Windows.Forms.Padding(3);
            this.map.Size = new System.Drawing.Size(619, 324);
            this.map.TabIndex = 9;
            this.map.Text = "Map";
            this.map.UseVisualStyleBackColor = true;
            // 
            // Light
            // 
            this.Light.Controls.Add(this.controlLight);
            this.Light.Location = new System.Drawing.Point(4, 22);
            this.Light.Name = "Light";
            this.Light.Size = new System.Drawing.Size(619, 324);
            this.Light.TabIndex = 12;
            this.Light.Text = "Light";
            this.Light.UseVisualStyleBackColor = true;
            // 
            // Dress
            // 
            this.Dress.Controls.Add(this.controldress);
            this.Dress.Location = new System.Drawing.Point(4, 22);
            this.Dress.Name = "Dress";
            this.Dress.Padding = new System.Windows.Forms.Padding(3);
            this.Dress.Size = new System.Drawing.Size(619, 324);
            this.Dress.TabIndex = 13;
            this.Dress.Text = "Dress";
            this.Dress.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(627, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alwaysOnTopToolStripMenuItem,
            this.pathSettingsToolStripMenuItem,
            this.toolStripSeparator1,
            this.restartNeededToolStripMenuItem});
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(62, 22);
            this.toolStripSplitButton1.Text = "Settings";
            // 
            // alwaysOnTopToolStripMenuItem
            // 
            this.alwaysOnTopToolStripMenuItem.CheckOnClick = true;
            this.alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
            this.alwaysOnTopToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.alwaysOnTopToolStripMenuItem.Text = "Always On Top";
            this.alwaysOnTopToolStripMenuItem.Click += new System.EventHandler(this.onClickAlwaysTop);
            // 
            // pathSettingsToolStripMenuItem
            // 
            this.pathSettingsToolStripMenuItem.Name = "pathSettingsToolStripMenuItem";
            this.pathSettingsToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.pathSettingsToolStripMenuItem.Text = "Path Settings";
            this.pathSettingsToolStripMenuItem.Click += new System.EventHandler(this.click_path);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(162, 6);
            // 
            // restartNeededToolStripMenuItem
            // 
            this.restartNeededToolStripMenuItem.ForeColor = System.Drawing.Color.DarkRed;
            this.restartNeededToolStripMenuItem.Name = "restartNeededToolStripMenuItem";
            this.restartNeededToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.restartNeededToolStripMenuItem.Text = "Restart Needed!";
            // 
            // controlMulti
            // 
            this.controlMulti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlMulti.Location = new System.Drawing.Point(3, 3);
            this.controlMulti.Name = "controlMulti";
            this.controlMulti.Size = new System.Drawing.Size(613, 318);
            this.controlMulti.TabIndex = 0;
            // 
            // controlMobList
            // 
            this.controlMobList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlMobList.Location = new System.Drawing.Point(3, 3);
            this.controlMobList.Name = "controlMobList";
            this.controlMobList.Size = new System.Drawing.Size(613, 318);
            this.controlMobList.TabIndex = 0;
            // 
            // controlItemShow
            // 
            this.controlItemShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlItemShow.Location = new System.Drawing.Point(3, 3);
            this.controlItemShow.Name = "controlItemShow";
            this.controlItemShow.Size = new System.Drawing.Size(613, 318);
            this.controlItemShow.TabIndex = 0;
            // 
            // controlLandTiles
            // 
            this.controlLandTiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlLandTiles.Location = new System.Drawing.Point(3, 3);
            this.controlLandTiles.Name = "controlLandTiles";
            this.controlLandTiles.Size = new System.Drawing.Size(613, 318);
            this.controlLandTiles.TabIndex = 0;
            // 
            // controlTexture
            // 
            this.controlTexture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlTexture.Location = new System.Drawing.Point(3, 3);
            this.controlTexture.Name = "controlTexture";
            this.controlTexture.Size = new System.Drawing.Size(613, 318);
            this.controlTexture.TabIndex = 0;
            // 
            // controlGumps
            // 
            this.controlGumps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlGumps.Location = new System.Drawing.Point(3, 3);
            this.controlGumps.Name = "controlGumps";
            this.controlGumps.Size = new System.Drawing.Size(613, 318);
            this.controlGumps.TabIndex = 0;
            // 
            // controlSound
            // 
            this.controlSound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlSound.Location = new System.Drawing.Point(3, 3);
            this.controlSound.Name = "controlSound";
            this.controlSound.Size = new System.Drawing.Size(613, 318);
            this.controlSound.TabIndex = 0;
            // 
            // controlHue
            // 
            this.controlHue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlHue.Location = new System.Drawing.Point(3, 3);
            this.controlHue.Name = "controlHue";
            this.controlHue.Size = new System.Drawing.Size(613, 318);
            this.controlHue.TabIndex = 0;
            // 
            // controlfonts
            // 
            this.controlfonts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlfonts.Location = new System.Drawing.Point(3, 3);
            this.controlfonts.Name = "controlfonts";
            this.controlfonts.Size = new System.Drawing.Size(613, 318);
            this.controlfonts.TabIndex = 0;
            // 
            // controlCliloc
            // 
            this.controlCliloc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlCliloc.Location = new System.Drawing.Point(3, 3);
            this.controlCliloc.Name = "controlCliloc";
            this.controlCliloc.Size = new System.Drawing.Size(613, 318);
            this.controlCliloc.TabIndex = 0;
            // 
            // controlmap
            // 
            this.controlmap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlmap.Location = new System.Drawing.Point(3, 3);
            this.controlmap.Margin = new System.Windows.Forms.Padding(0);
            this.controlmap.Name = "controlmap";
            this.controlmap.Size = new System.Drawing.Size(613, 318);
            this.controlmap.TabIndex = 0;
            // 
            // controlLight
            // 
            this.controlLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlLight.Location = new System.Drawing.Point(0, 0);
            this.controlLight.Name = "controlLight";
            this.controlLight.Size = new System.Drawing.Size(619, 324);
            this.controlLight.TabIndex = 0;
            // 
            // controldress
            // 
            this.controldress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controldress.Location = new System.Drawing.Point(3, 3);
            this.controldress.Name = "controldress";
            this.controldress.Size = new System.Drawing.Size(613, 318);
            this.controldress.TabIndex = 0;
            // 
            // UoViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 375);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UoViewer";
            this.Text = "UOViewer";
            this.tabControl2.ResumeLayout(false);
            this.Start.ResumeLayout(false);
            this.Start.PerformLayout();
            this.Multis.ResumeLayout(false);
            this.MobGraphic.ResumeLayout(false);
            this.Items.ResumeLayout(false);
            this.LandTiles.ResumeLayout(false);
            this.Texture.ResumeLayout(false);
            this.Gumps.ResumeLayout(false);
            this.Sounds.ResumeLayout(false);
            this.Hue.ResumeLayout(false);
            this.fonts.ResumeLayout(false);
            this.Cliloc.ResumeLayout(false);
            this.map.ResumeLayout(false);
            this.Light.ResumeLayout(false);
            this.Dress.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage MobGraphic;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TabPage Items;
        private Controls.ItemShow controlItemShow;
        private Controls.MobList controlMobList;
        private System.Windows.Forms.TabPage LandTiles;
        private Controls.LandTiles controlLandTiles;
        private System.Windows.Forms.TabPage Gumps;
        private Controls.Gump controlGumps;
        private System.Windows.Forms.TabPage Sounds;
        private Controls.Sounds controlSound;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem restartNeededToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabPage Multis;
        private Controls.Multis controlMulti;
        private System.Windows.Forms.TabPage Hue;
        private Controls.Hues controlHue;
        private System.Windows.Forms.TabPage fonts;
        private Controls.Fonts controlfonts;
        private System.Windows.Forms.TabPage Cliloc;
        private Controls.Cliloc controlCliloc;
        private System.Windows.Forms.TabPage map;
        private Controls.Map controlmap;
        private System.Windows.Forms.TabPage Start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem pathSettingsToolStripMenuItem;
        private System.Windows.Forms.TabPage Texture;
        private Controls.Texture controlTexture;
        private System.Windows.Forms.TabPage Light;
        private Controls.Light controlLight;
        private System.Windows.Forms.Label Versionlabel;
        private System.Windows.Forms.TabPage Dress;
        private Controls.Dress controldress;
        private System.Windows.Forms.ToolStripMenuItem alwaysOnTopToolStripMenuItem;
    }
}

