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

namespace FiddlerControls
{
    partial class LandTilesAlternative
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LandTilesAlternative));
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asBmpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asTiffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.selectInTileDataTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectInRadarColorTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.findNextFreeSlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertAtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InsertText = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.namelabel = new System.Windows.Forms.ToolStripLabel();
            this.graphiclabel = new System.Windows.Forms.ToolStripLabel();
            this.FlagsLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.SearchButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(602, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 324);
            this.vScrollBar.TabIndex = 7;
            this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnScroll);
            // 
            // pictureBox
            // 
            this.pictureBox.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 25);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pictureBox.Size = new System.Drawing.Size(602, 299);
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            this.pictureBox.SizeChanged += new System.EventHandler(this.OnResize);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportImageToolStripMenuItem,
            this.toolStripSeparator3,
            this.selectInTileDataTabToolStripMenuItem,
            this.selectInRadarColorTabToolStripMenuItem,
            this.toolStripSeparator2,
            this.findNextFreeSlotToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.insertAtToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(202, 198);
            // 
            // exportImageToolStripMenuItem
            // 
            this.exportImageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asBmpToolStripMenuItem,
            this.asTiffToolStripMenuItem});
            this.exportImageToolStripMenuItem.Name = "exportImageToolStripMenuItem";
            this.exportImageToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.exportImageToolStripMenuItem.Text = "Export Image..";
            // 
            // asBmpToolStripMenuItem
            // 
            this.asBmpToolStripMenuItem.Name = "asBmpToolStripMenuItem";
            this.asBmpToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.asBmpToolStripMenuItem.Text = "As Bmp";
            this.asBmpToolStripMenuItem.Click += new System.EventHandler(this.onClickExportBmp);
            // 
            // asTiffToolStripMenuItem
            // 
            this.asTiffToolStripMenuItem.Name = "asTiffToolStripMenuItem";
            this.asTiffToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.asTiffToolStripMenuItem.Text = "As Tiff";
            this.asTiffToolStripMenuItem.Click += new System.EventHandler(this.onClickExportTiff);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(198, 6);
            // 
            // selectInTileDataTabToolStripMenuItem
            // 
            this.selectInTileDataTabToolStripMenuItem.Name = "selectInTileDataTabToolStripMenuItem";
            this.selectInTileDataTabToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.selectInTileDataTabToolStripMenuItem.Text = "Select in TileData tab";
            this.selectInTileDataTabToolStripMenuItem.Click += new System.EventHandler(this.OnClickSelectTiledata);
            // 
            // selectInRadarColorTabToolStripMenuItem
            // 
            this.selectInRadarColorTabToolStripMenuItem.Name = "selectInRadarColorTabToolStripMenuItem";
            this.selectInRadarColorTabToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.selectInRadarColorTabToolStripMenuItem.Text = "Select in RadarColor tab";
            this.selectInRadarColorTabToolStripMenuItem.Click += new System.EventHandler(this.OnClickSelectRadarCol);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(198, 6);
            // 
            // findNextFreeSlotToolStripMenuItem
            // 
            this.findNextFreeSlotToolStripMenuItem.Name = "findNextFreeSlotToolStripMenuItem";
            this.findNextFreeSlotToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.findNextFreeSlotToolStripMenuItem.Text = "Find Next Free Slot";
            this.findNextFreeSlotToolStripMenuItem.Click += new System.EventHandler(this.onClickFindFree);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.onClickRemove);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.replaceToolStripMenuItem.Text = "Replace";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.onClickReplace);
            // 
            // insertAtToolStripMenuItem
            // 
            this.insertAtToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InsertText});
            this.insertAtToolStripMenuItem.Name = "insertAtToolStripMenuItem";
            this.insertAtToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.insertAtToolStripMenuItem.Text = "Insert At..";
            // 
            // InsertText
            // 
            this.InsertText.Name = "InsertText";
            this.InsertText.Size = new System.Drawing.Size(100, 21);
            this.InsertText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.onKeyDownInsert);
            this.InsertText.TextChanged += new System.EventHandler(this.onTextChangedInsert);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.onClickSave);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(602, 324);
            this.panel1.TabIndex = 3;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.namelabel,
            this.graphiclabel,
            this.FlagsLabel,
            this.toolStripSeparator4,
            this.SearchButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(602, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // namelabel
            // 
            this.namelabel.AutoSize = false;
            this.namelabel.Name = "namelabel";
            this.namelabel.Size = new System.Drawing.Size(150, 22);
            this.namelabel.Text = "Name:";
            this.namelabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // graphiclabel
            // 
            this.graphiclabel.AutoSize = false;
            this.graphiclabel.Name = "graphiclabel";
            this.graphiclabel.Size = new System.Drawing.Size(120, 22);
            this.graphiclabel.Text = "Graphic:";
            this.graphiclabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FlagsLabel
            // 
            this.FlagsLabel.AutoSize = false;
            this.FlagsLabel.Name = "FlagsLabel";
            this.FlagsLabel.Size = new System.Drawing.Size(250, 22);
            this.FlagsLabel.Text = "Flags:";
            this.FlagsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // SearchButton
            // 
            this.SearchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SearchButton.Image = ((System.Drawing.Image)(resources.GetObject("SearchButton.Image")));
            this.SearchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(44, 22);
            this.SearchButton.Text = "Search";
            this.SearchButton.Click += new System.EventHandler(this.OnClickSearch);
            // 
            // LandTilesAlternative
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.vScrollBar);
            this.Name = "LandTilesAlternative";
            this.Size = new System.Drawing.Size(619, 324);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem findNextFreeSlotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertAtToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox InsertText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asBmpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asTiffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectInTileDataTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem selectInRadarColorTabToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel namelabel;
        private System.Windows.Forms.ToolStripLabel graphiclabel;
        private System.Windows.Forms.ToolStripLabel FlagsLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton SearchButton;

    }
}
