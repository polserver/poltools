/***************************************************************************
 *
 * $Author: MuadDib & Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

namespace MultiEditor
{
    partial class MultiEditor
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiEditor));
            this.TC_MultiEditorToolbox = new System.Windows.Forms.TabControl();
            this.tileTab = new System.Windows.Forms.TabPage();
            this.treeViewTilesXML = new System.Windows.Forms.TreeView();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.designTab = new System.Windows.Forms.TabPage();
            this.importTab = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.MaxHeightTrackBar = new System.Windows.Forms.TrackBar();
            this.pictureBoxMulti = new System.Windows.Forms.PictureBox();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.DrawFloortoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.DrawFloortoolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelCoord = new System.Windows.Forms.ToolStripLabel();
            this.DrawTileButton = new System.Windows.Forms.ToolStripButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.TC_MultiEditorToolbox.SuspendLayout();
            this.tileTab.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxHeightTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMulti)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TC_MultiEditorToolbox
            // 
            this.TC_MultiEditorToolbox.Controls.Add(this.tileTab);
            this.TC_MultiEditorToolbox.Controls.Add(this.designTab);
            this.TC_MultiEditorToolbox.Controls.Add(this.importTab);
            this.TC_MultiEditorToolbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TC_MultiEditorToolbox.Location = new System.Drawing.Point(0, 0);
            this.TC_MultiEditorToolbox.Name = "TC_MultiEditorToolbox";
            this.TC_MultiEditorToolbox.SelectedIndex = 0;
            this.TC_MultiEditorToolbox.Size = new System.Drawing.Size(200, 324);
            this.TC_MultiEditorToolbox.TabIndex = 0;
            // 
            // tileTab
            // 
            this.tileTab.BackColor = System.Drawing.SystemColors.Window;
            this.tileTab.Controls.Add(this.treeViewTilesXML);
            this.tileTab.Location = new System.Drawing.Point(4, 22);
            this.tileTab.Name = "tileTab";
            this.tileTab.Padding = new System.Windows.Forms.Padding(3);
            this.tileTab.Size = new System.Drawing.Size(192, 298);
            this.tileTab.TabIndex = 0;
            this.tileTab.Text = "Tiles";
            this.tileTab.UseVisualStyleBackColor = true;
            // 
            // treeViewTilesXML
            // 
            this.treeViewTilesXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTilesXML.ImageIndex = 0;
            this.treeViewTilesXML.ImageList = this.imageListTreeView;
            this.treeViewTilesXML.Location = new System.Drawing.Point(3, 3);
            this.treeViewTilesXML.Name = "treeViewTilesXML";
            this.treeViewTilesXML.SelectedImageIndex = 0;
            this.treeViewTilesXML.Size = new System.Drawing.Size(186, 292);
            this.treeViewTilesXML.TabIndex = 0;
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeView.Images.SetKeyName(0, "treeViewImage.bmp");
            // 
            // designTab
            // 
            this.designTab.BackColor = System.Drawing.SystemColors.Window;
            this.designTab.Location = new System.Drawing.Point(4, 22);
            this.designTab.Name = "designTab";
            this.designTab.Padding = new System.Windows.Forms.Padding(3);
            this.designTab.Size = new System.Drawing.Size(192, 298);
            this.designTab.TabIndex = 1;
            this.designTab.Text = "Design";
            this.designTab.UseVisualStyleBackColor = true;
            // 
            // importTab
            // 
            this.importTab.BackColor = System.Drawing.SystemColors.Window;
            this.importTab.Location = new System.Drawing.Point(4, 22);
            this.importTab.Name = "importTab";
            this.importTab.Size = new System.Drawing.Size(192, 298);
            this.importTab.TabIndex = 2;
            this.importTab.Text = "Import";
            this.importTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TC_MultiEditorToolbox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(619, 324);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 25);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.MaxHeightTrackBar);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pictureBoxMulti);
            this.splitContainer2.Panel2.Controls.Add(this.hScrollBar);
            this.splitContainer2.Panel2.Controls.Add(this.vScrollBar);
            this.splitContainer2.Size = new System.Drawing.Size(415, 299);
            this.splitContainer2.SplitterDistance = 30;
            this.splitContainer2.TabIndex = 2;
            // 
            // MaxHeightTrackBar
            // 
            this.MaxHeightTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaxHeightTrackBar.Location = new System.Drawing.Point(0, 0);
            this.MaxHeightTrackBar.Name = "MaxHeightTrackBar";
            this.MaxHeightTrackBar.Size = new System.Drawing.Size(415, 30);
            this.MaxHeightTrackBar.TabIndex = 0;
            this.MaxHeightTrackBar.ValueChanged += new System.EventHandler(this.OnValueChangedMaxHeight);
            // 
            // pictureBoxMulti
            // 
            this.pictureBoxMulti.BackColor = System.Drawing.Color.White;
            this.pictureBoxMulti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxMulti.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxMulti.Name = "pictureBoxMulti";
            this.pictureBoxMulti.Size = new System.Drawing.Size(398, 248);
            this.pictureBoxMulti.TabIndex = 0;
            this.pictureBoxMulti.TabStop = false;
            this.pictureBoxMulti.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMovePictureBoxMulti);
            this.pictureBoxMulti.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaintPictureBoxMulti);
            this.pictureBoxMulti.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpPictureBoxMulti);
            this.pictureBoxMulti.SizeChanged += new System.EventHandler(this.OnResizePictureBoxMulti);
            // 
            // hScrollBar
            // 
            this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar.Location = new System.Drawing.Point(0, 248);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(398, 17);
            this.hScrollBar.TabIndex = 2;
            this.hScrollBar.ValueChanged += new System.EventHandler(this.OnScrollBarValueChanged);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(398, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 265);
            this.vScrollBar.TabIndex = 1;
            this.vScrollBar.ValueChanged += new System.EventHandler(this.OnScrollBarValueChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DrawTileButton,
            this.DrawFloortoolStripButton,
            this.DrawFloortoolStripTextBox,
            this.toolStripLabelCoord});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(415, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // DrawFloortoolStripButton
            // 
            this.DrawFloortoolStripButton.Checked = true;
            this.DrawFloortoolStripButton.CheckOnClick = true;
            this.DrawFloortoolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DrawFloortoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DrawFloortoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("DrawFloortoolStripButton.Image")));
            this.DrawFloortoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DrawFloortoolStripButton.Name = "DrawFloortoolStripButton";
            this.DrawFloortoolStripButton.Size = new System.Drawing.Size(63, 22);
            this.DrawFloortoolStripButton.Text = "Draw Floor";
            this.DrawFloortoolStripButton.Click += new System.EventHandler(this.OnClickDrawFloor);
            // 
            // DrawFloortoolStripTextBox
            // 
            this.DrawFloortoolStripTextBox.MaxLength = 4;
            this.DrawFloortoolStripTextBox.Name = "DrawFloortoolStripTextBox";
            this.DrawFloortoolStripTextBox.Size = new System.Drawing.Size(50, 25);
            this.DrawFloortoolStripTextBox.Text = "0";
            this.DrawFloortoolStripTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDownDrawFloorEntry);
            // 
            // toolStripLabelCoord
            // 
            this.toolStripLabelCoord.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabelCoord.Name = "toolStripLabelCoord";
            this.toolStripLabelCoord.Size = new System.Drawing.Size(33, 22);
            this.toolStripLabelCoord.Text = "0,0,0";
            // 
            // DrawTileButton
            // 
            this.DrawTileButton.CheckOnClick = true;
            this.DrawTileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DrawTileButton.Image = ((System.Drawing.Image)(resources.GetObject("DrawTileButton.Image")));
            this.DrawTileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DrawTileButton.Name = "DrawTileButton";
            this.DrawTileButton.Size = new System.Drawing.Size(55, 22);
            this.DrawTileButton.Text = "Draw Tile";
            this.DrawTileButton.Click += new System.EventHandler(this.OnClickDrawTile);
            // 
            // MultiEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MultiEditor";
            this.Size = new System.Drawing.Size(619, 324);
            this.TC_MultiEditorToolbox.ResumeLayout(false);
            this.tileTab.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MaxHeightTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMulti)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TC_MultiEditorToolbox;
        private System.Windows.Forms.TabPage tileTab;
        private System.Windows.Forms.TabPage designTab;
        private System.Windows.Forms.TabPage importTab;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewTilesXML;
        private System.Windows.Forms.ImageList imageListTreeView;
        private System.Windows.Forms.PictureBox pictureBoxMulti;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TrackBar MaxHeightTrackBar;
        private System.Windows.Forms.HScrollBar hScrollBar;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton DrawFloortoolStripButton;
        private System.Windows.Forms.ToolStripTextBox DrawFloortoolStripTextBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelCoord;
        private System.Windows.Forms.ToolStripButton DrawTileButton;
    }
}
