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
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.treeViewTilesXML = new System.Windows.Forms.TreeView();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.pictureBoxDrawTiles = new System.Windows.Forms.PictureBox();
            this.vScrollBarDrawTiles = new System.Windows.Forms.VScrollBar();
            this.designTab = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_SaveToID = new System.Windows.Forms.TextBox();
            this.BTN_Save = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDown_Size_Width = new System.Windows.Forms.NumericUpDown();
            this.BTN_Resize = new System.Windows.Forms.Button();
            this.numericUpDown_Size_Height = new System.Windows.Forms.NumericUpDown();
            this.importTab = new System.Windows.Forms.TabPage();
            this.treeViewMultiList = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.BTN_Floor = new System.Windows.Forms.CheckBox();
            this.BTN_Z = new System.Windows.Forms.CheckBox();
            this.imageListTools = new System.Windows.Forms.ImageList(this.components);
            this.BTN_Remove = new System.Windows.Forms.CheckBox();
            this.BTN_Draw = new System.Windows.Forms.CheckBox();
            this.BTN_Select = new System.Windows.Forms.CheckBox();
            this.numericUpDown_Floor = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_Z = new System.Windows.Forms.NumericUpDown();
            this.collapsibleSplitter1 = new FiddlerControls.CollapsibleSplitter();
            this.Selectedpanel = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numericUpDown_Selected_Z = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_Selected_Y = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_Selected_X = new System.Windows.Forms.NumericUpDown();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.MaxHeightTrackBar = new System.Windows.Forms.TrackBar();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pictureBoxMulti = new System.Windows.Forms.PictureBox();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelCoord = new System.Windows.Forms.ToolStripLabel();
            this.SelectedTileLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.FloatingPreviewPanel = new System.Windows.Forms.Panel();
            this.BTN_CreateBlank = new System.Windows.Forms.Button();
            this.TC_MultiEditorToolbox.SuspendLayout();
            this.tileTab.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDrawTiles)).BeginInit();
            this.designTab.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Size_Width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Size_Height)).BeginInit();
            this.importTab.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Floor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Z)).BeginInit();
            this.Selectedpanel.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Selected_Z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Selected_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Selected_X)).BeginInit();
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
            this.TC_MultiEditorToolbox.Location = new System.Drawing.Point(0, 91);
            this.TC_MultiEditorToolbox.Name = "TC_MultiEditorToolbox";
            this.TC_MultiEditorToolbox.SelectedIndex = 0;
            this.TC_MultiEditorToolbox.Size = new System.Drawing.Size(200, 181);
            this.TC_MultiEditorToolbox.TabIndex = 0;
            // 
            // tileTab
            // 
            this.tileTab.BackColor = System.Drawing.SystemColors.Window;
            this.tileTab.Controls.Add(this.splitContainer4);
            this.tileTab.Location = new System.Drawing.Point(4, 22);
            this.tileTab.Name = "tileTab";
            this.tileTab.Padding = new System.Windows.Forms.Padding(3);
            this.tileTab.Size = new System.Drawing.Size(192, 155);
            this.tileTab.TabIndex = 0;
            this.tileTab.Text = "Tiles";
            this.tileTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.treeViewTilesXML);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.pictureBoxDrawTiles);
            this.splitContainer4.Panel2.Controls.Add(this.vScrollBarDrawTiles);
            this.splitContainer4.Size = new System.Drawing.Size(186, 149);
            this.splitContainer4.SplitterDistance = 73;
            this.splitContainer4.TabIndex = 0;
            // 
            // treeViewTilesXML
            // 
            this.treeViewTilesXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTilesXML.ImageIndex = 0;
            this.treeViewTilesXML.ImageList = this.imageListTreeView;
            this.treeViewTilesXML.Location = new System.Drawing.Point(0, 0);
            this.treeViewTilesXML.Name = "treeViewTilesXML";
            this.treeViewTilesXML.SelectedImageIndex = 0;
            this.treeViewTilesXML.Size = new System.Drawing.Size(186, 73);
            this.treeViewTilesXML.TabIndex = 0;
            this.treeViewTilesXML.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterSelectTreeViewTilesXML);
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeView.Images.SetKeyName(0, "treeViewImage.bmp");
            // 
            // pictureBoxDrawTiles
            // 
            this.pictureBoxDrawTiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxDrawTiles.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxDrawTiles.Name = "pictureBoxDrawTiles";
            this.pictureBoxDrawTiles.Size = new System.Drawing.Size(169, 72);
            this.pictureBoxDrawTiles.TabIndex = 3;
            this.pictureBoxDrawTiles.TabStop = false;
            this.pictureBoxDrawTiles.MouseLeave += new System.EventHandler(this.pictureBoxDrawTilesMouseLeave);
            this.pictureBoxDrawTiles.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDrawTilesMouseMove);
            this.pictureBoxDrawTiles.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDrawTiles_OnMouseClick);
            this.pictureBoxDrawTiles.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxDrawTiles_OnPaint);
            this.pictureBoxDrawTiles.SizeChanged += new System.EventHandler(this.pictureBoxDrawTiles_OnResize);
            // 
            // vScrollBarDrawTiles
            // 
            this.vScrollBarDrawTiles.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBarDrawTiles.Location = new System.Drawing.Point(169, 0);
            this.vScrollBarDrawTiles.Name = "vScrollBarDrawTiles";
            this.vScrollBarDrawTiles.Size = new System.Drawing.Size(17, 72);
            this.vScrollBarDrawTiles.TabIndex = 0;
            this.vScrollBarDrawTiles.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBarDrawTiles_Scroll);
            // 
            // designTab
            // 
            this.designTab.BackColor = System.Drawing.SystemColors.Window;
            this.designTab.Controls.Add(this.groupBox2);
            this.designTab.Controls.Add(this.groupBox1);
            this.designTab.Location = new System.Drawing.Point(4, 22);
            this.designTab.Name = "designTab";
            this.designTab.Padding = new System.Windows.Forms.Padding(3);
            this.designTab.Size = new System.Drawing.Size(192, 155);
            this.designTab.TabIndex = 1;
            this.designTab.Text = "Design";
            this.designTab.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.textBox_SaveToID);
            this.groupBox2.Controls.Add(this.BTN_Save);
            this.groupBox2.Location = new System.Drawing.Point(7, 90);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(179, 59);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Save";
            // 
            // textBox_SaveToID
            // 
            this.textBox_SaveToID.Location = new System.Drawing.Point(92, 22);
            this.textBox_SaveToID.Name = "textBox_SaveToID";
            this.textBox_SaveToID.Size = new System.Drawing.Size(66, 20);
            this.textBox_SaveToID.TabIndex = 1;
            // 
            // BTN_Save
            // 
            this.BTN_Save.Location = new System.Drawing.Point(7, 20);
            this.BTN_Save.Name = "BTN_Save";
            this.BTN_Save.Size = new System.Drawing.Size(75, 23);
            this.BTN_Save.TabIndex = 0;
            this.BTN_Save.Text = "Save to ID";
            this.BTN_Save.UseVisualStyleBackColor = true;
            this.BTN_Save.Click += new System.EventHandler(this.BTN_Save_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.BTN_CreateBlank);
            this.groupBox1.Controls.Add(this.numericUpDown_Size_Width);
            this.groupBox1.Controls.Add(this.BTN_Resize);
            this.groupBox1.Controls.Add(this.numericUpDown_Size_Height);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 78);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Multi Size";
            // 
            // numericUpDown_Size_Width
            // 
            this.numericUpDown_Size_Width.Location = new System.Drawing.Point(35, 19);
            this.numericUpDown_Size_Width.Name = "numericUpDown_Size_Width";
            this.numericUpDown_Size_Width.Size = new System.Drawing.Size(52, 20);
            this.numericUpDown_Size_Width.TabIndex = 0;
            // 
            // BTN_Resize
            // 
            this.BTN_Resize.Location = new System.Drawing.Point(11, 45);
            this.BTN_Resize.Name = "BTN_Resize";
            this.BTN_Resize.Size = new System.Drawing.Size(75, 23);
            this.BTN_Resize.TabIndex = 2;
            this.BTN_Resize.Text = "Resize Multi";
            this.BTN_Resize.UseVisualStyleBackColor = true;
            this.BTN_Resize.Click += new System.EventHandler(this.BTN_ResizeMulti_Click);
            // 
            // numericUpDown_Size_Height
            // 
            this.numericUpDown_Size_Height.Location = new System.Drawing.Point(93, 19);
            this.numericUpDown_Size_Height.Name = "numericUpDown_Size_Height";
            this.numericUpDown_Size_Height.Size = new System.Drawing.Size(52, 20);
            this.numericUpDown_Size_Height.TabIndex = 1;
            // 
            // importTab
            // 
            this.importTab.BackColor = System.Drawing.SystemColors.Window;
            this.importTab.Controls.Add(this.treeViewMultiList);
            this.importTab.Location = new System.Drawing.Point(4, 22);
            this.importTab.Name = "importTab";
            this.importTab.Size = new System.Drawing.Size(192, 160);
            this.importTab.TabIndex = 2;
            this.importTab.Text = "Import";
            this.importTab.UseVisualStyleBackColor = true;
            // 
            // treeViewMultiList
            // 
            this.treeViewMultiList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewMultiList.Location = new System.Drawing.Point(0, 0);
            this.treeViewMultiList.Name = "treeViewMultiList";
            this.treeViewMultiList.Size = new System.Drawing.Size(192, 160);
            this.treeViewMultiList.TabIndex = 0;
            this.treeViewMultiList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewMultiList_NodeMouseDoubleClick);
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(619, 324);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.BTN_Floor);
            this.splitContainer3.Panel1.Controls.Add(this.BTN_Z);
            this.splitContainer3.Panel1.Controls.Add(this.BTN_Remove);
            this.splitContainer3.Panel1.Controls.Add(this.BTN_Draw);
            this.splitContainer3.Panel1.Controls.Add(this.BTN_Select);
            this.splitContainer3.Panel1.Controls.Add(this.numericUpDown_Floor);
            this.splitContainer3.Panel1.Controls.Add(this.numericUpDown_Z);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.TC_MultiEditorToolbox);
            this.splitContainer3.Panel2.Controls.Add(this.collapsibleSplitter1);
            this.splitContainer3.Panel2.Controls.Add(this.Selectedpanel);
            this.splitContainer3.Size = new System.Drawing.Size(200, 324);
            this.splitContainer3.SplitterWidth = 2;
            this.splitContainer3.TabIndex = 1;
            // 
            // BTN_Floor
            // 
            this.BTN_Floor.Appearance = System.Windows.Forms.Appearance.Button;
            this.BTN_Floor.Location = new System.Drawing.Point(4, 26);
            this.BTN_Floor.Name = "BTN_Floor";
            this.BTN_Floor.Size = new System.Drawing.Size(71, 21);
            this.BTN_Floor.TabIndex = 13;
            this.BTN_Floor.Text = "Virtual Floor";
            this.BTN_Floor.UseVisualStyleBackColor = true;
            this.BTN_Floor.Click += new System.EventHandler(this.BTN_Floor_Clicked);
            // 
            // BTN_Z
            // 
            this.BTN_Z.Appearance = System.Windows.Forms.Appearance.Button;
            this.BTN_Z.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BTN_Z.ImageKey = "AltitudeButton.bmp";
            this.BTN_Z.ImageList = this.imageListTools;
            this.BTN_Z.Location = new System.Drawing.Point(84, 3);
            this.BTN_Z.Name = "BTN_Z";
            this.BTN_Z.Size = new System.Drawing.Size(21, 21);
            this.BTN_Z.TabIndex = 12;
            this.toolTip1.SetToolTip(this.BTN_Z, "Apply Z Level");
            this.BTN_Z.UseVisualStyleBackColor = true;
            this.BTN_Z.CheckStateChanged += new System.EventHandler(this.BTN_Toolbox_CheckedChanged);
            this.BTN_Z.Click += new System.EventHandler(this.BTN_Z_Click);
            // 
            // imageListTools
            // 
            this.imageListTools.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTools.ImageStream")));
            this.imageListTools.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTools.Images.SetKeyName(0, "AltitudeButton.bmp");
            this.imageListTools.Images.SetKeyName(1, "DrawButton.bmp");
            this.imageListTools.Images.SetKeyName(2, "RemoveButton.bmp");
            this.imageListTools.Images.SetKeyName(3, "SelectButton.bmp");
            this.imageListTools.Images.SetKeyName(4, "AltitudeButton_Selected.bmp");
            this.imageListTools.Images.SetKeyName(5, "DrawButton_Selected.bmp");
            this.imageListTools.Images.SetKeyName(6, "RemoveButton_Selected.bmp");
            this.imageListTools.Images.SetKeyName(7, "SelectButton_Selected.bmp");
            // 
            // BTN_Remove
            // 
            this.BTN_Remove.Appearance = System.Windows.Forms.Appearance.Button;
            this.BTN_Remove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BTN_Remove.ImageKey = "RemoveButton.bmp";
            this.BTN_Remove.ImageList = this.imageListTools;
            this.BTN_Remove.Location = new System.Drawing.Point(58, 3);
            this.BTN_Remove.Name = "BTN_Remove";
            this.BTN_Remove.Size = new System.Drawing.Size(21, 21);
            this.BTN_Remove.TabIndex = 11;
            this.toolTip1.SetToolTip(this.BTN_Remove, "Remove A Tile");
            this.BTN_Remove.UseVisualStyleBackColor = true;
            this.BTN_Remove.CheckStateChanged += new System.EventHandler(this.BTN_Toolbox_CheckedChanged);
            this.BTN_Remove.Click += new System.EventHandler(this.BTN_Remove_Click);
            // 
            // BTN_Draw
            // 
            this.BTN_Draw.Appearance = System.Windows.Forms.Appearance.Button;
            this.BTN_Draw.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BTN_Draw.ImageKey = "DrawButton.bmp";
            this.BTN_Draw.ImageList = this.imageListTools;
            this.BTN_Draw.Location = new System.Drawing.Point(31, 3);
            this.BTN_Draw.Name = "BTN_Draw";
            this.BTN_Draw.Size = new System.Drawing.Size(21, 21);
            this.BTN_Draw.TabIndex = 10;
            this.toolTip1.SetToolTip(this.BTN_Draw, "Draw A Tile");
            this.BTN_Draw.UseVisualStyleBackColor = true;
            this.BTN_Draw.CheckStateChanged += new System.EventHandler(this.BTN_Toolbox_CheckedChanged);
            this.BTN_Draw.Click += new System.EventHandler(this.BTN_Draw_Click);
            // 
            // BTN_Select
            // 
            this.BTN_Select.Appearance = System.Windows.Forms.Appearance.Button;
            this.BTN_Select.Checked = true;
            this.BTN_Select.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BTN_Select.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BTN_Select.ImageKey = "SelectButton.bmp";
            this.BTN_Select.ImageList = this.imageListTools;
            this.BTN_Select.Location = new System.Drawing.Point(4, 3);
            this.BTN_Select.Name = "BTN_Select";
            this.BTN_Select.Size = new System.Drawing.Size(21, 21);
            this.BTN_Select.TabIndex = 9;
            this.toolTip1.SetToolTip(this.BTN_Select, "Select A Tile");
            this.BTN_Select.UseVisualStyleBackColor = true;
            this.BTN_Select.CheckStateChanged += new System.EventHandler(this.BTN_Toolbox_CheckedChanged);
            this.BTN_Select.Click += new System.EventHandler(this.BTN_Select_Click);
            // 
            // numericUpDown_Floor
            // 
            this.numericUpDown_Floor.Location = new System.Drawing.Point(78, 27);
            this.numericUpDown_Floor.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numericUpDown_Floor.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.numericUpDown_Floor.Name = "numericUpDown_Floor";
            this.numericUpDown_Floor.Size = new System.Drawing.Size(50, 20);
            this.numericUpDown_Floor.TabIndex = 8;
            this.numericUpDown_Floor.ValueChanged += new System.EventHandler(this.numericUpDown_Floor_Changed);
            // 
            // numericUpDown_Z
            // 
            this.numericUpDown_Z.Location = new System.Drawing.Point(111, 5);
            this.numericUpDown_Z.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numericUpDown_Z.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.numericUpDown_Z.Name = "numericUpDown_Z";
            this.numericUpDown_Z.Size = new System.Drawing.Size(50, 20);
            this.numericUpDown_Z.TabIndex = 5;
            // 
            // collapsibleSplitter1
            // 
            this.collapsibleSplitter1.AnimationDelay = 20;
            this.collapsibleSplitter1.AnimationStep = 20;
            this.collapsibleSplitter1.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
            this.collapsibleSplitter1.ControlToHide = this.Selectedpanel;
            this.collapsibleSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.collapsibleSplitter1.ExpandParentForm = false;
            this.collapsibleSplitter1.Location = new System.Drawing.Point(0, 83);
            this.collapsibleSplitter1.Name = "collapsibleSplitter1";
            this.collapsibleSplitter1.TabIndex = 5;
            this.collapsibleSplitter1.TabStop = false;
            this.toolTip1.SetToolTip(this.collapsibleSplitter1, "Selected Tile Panel");
            this.collapsibleSplitter1.UseAnimations = true;
            this.collapsibleSplitter1.VisualStyle = FiddlerControls.VisualStyles.DoubleDots;
            // 
            // Selectedpanel
            // 
            this.Selectedpanel.Controls.Add(this.groupBox3);
            this.Selectedpanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.Selectedpanel.Location = new System.Drawing.Point(0, 0);
            this.Selectedpanel.Name = "Selectedpanel";
            this.Selectedpanel.Size = new System.Drawing.Size(200, 83);
            this.Selectedpanel.TabIndex = 6;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numericUpDown_Selected_Z);
            this.groupBox3.Controls.Add(this.numericUpDown_Selected_Y);
            this.groupBox3.Controls.Add(this.numericUpDown_Selected_X);
            this.groupBox3.Location = new System.Drawing.Point(11, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(179, 75);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Selected Tile X,Y,Z";
            // 
            // numericUpDown_Selected_Z
            // 
            this.numericUpDown_Selected_Z.Location = new System.Drawing.Point(56, 46);
            this.numericUpDown_Selected_Z.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numericUpDown_Selected_Z.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.numericUpDown_Selected_Z.Name = "numericUpDown_Selected_Z";
            this.numericUpDown_Selected_Z.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown_Selected_Z.TabIndex = 2;
            this.numericUpDown_Selected_Z.ValueChanged += new System.EventHandler(this.numericUpDown_Selected_Z_Changed);
            // 
            // numericUpDown_Selected_Y
            // 
            this.numericUpDown_Selected_Y.Location = new System.Drawing.Point(100, 20);
            this.numericUpDown_Selected_Y.Name = "numericUpDown_Selected_Y";
            this.numericUpDown_Selected_Y.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown_Selected_Y.TabIndex = 1;
            this.numericUpDown_Selected_Y.ValueChanged += new System.EventHandler(this.numericUpDown_Selected_Y_Changed);
            // 
            // numericUpDown_Selected_X
            // 
            this.numericUpDown_Selected_X.Location = new System.Drawing.Point(20, 20);
            this.numericUpDown_Selected_X.Name = "numericUpDown_Selected_X";
            this.numericUpDown_Selected_X.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown_Selected_X.TabIndex = 0;
            this.numericUpDown_Selected_X.ValueChanged += new System.EventHandler(this.numericUpDown_Selected_X_Changed);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.MaxHeightTrackBar);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitter1);
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
            this.toolTip1.SetToolTip(this.MaxHeightTrackBar, "Max Height Displayed");
            this.MaxHeightTrackBar.ValueChanged += new System.EventHandler(this.MaxHeightTrackBarOnValueChanged);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 248);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
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
            this.pictureBoxMulti.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBoxMultiOnMouseMove);
            this.pictureBoxMulti.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBoxMultiOnPaint);
            this.pictureBoxMulti.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBoxMultiOnMouseUp);
            this.pictureBoxMulti.SizeChanged += new System.EventHandler(this.PictureBoxMultiOnResize);
            // 
            // hScrollBar
            // 
            this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar.Location = new System.Drawing.Point(0, 248);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(398, 17);
            this.hScrollBar.TabIndex = 2;
            this.hScrollBar.ValueChanged += new System.EventHandler(this.ScrollBarsValueChanged);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(398, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 265);
            this.vScrollBar.TabIndex = 1;
            this.vScrollBar.ValueChanged += new System.EventHandler(this.ScrollBarsValueChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelCoord,
            this.SelectedTileLabel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 299);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(415, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabelCoord
            // 
            this.toolStripLabelCoord.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabelCoord.Name = "toolStripLabelCoord";
            this.toolStripLabelCoord.Size = new System.Drawing.Size(33, 22);
            this.toolStripLabelCoord.Text = "0,0,0";
            this.toolStripLabelCoord.ToolTipText = "Coordinates";
            // 
            // SelectedTileLabel
            // 
            this.SelectedTileLabel.Name = "SelectedTileLabel";
            this.SelectedTileLabel.Size = new System.Drawing.Size(22, 22);
            this.SelectedTileLabel.Text = "ID:";
            // 
            // FloatingPreviewPanel
            // 
            this.FloatingPreviewPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.FloatingPreviewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FloatingPreviewPanel.Location = new System.Drawing.Point(250, 67);
            this.FloatingPreviewPanel.Name = "FloatingPreviewPanel";
            this.FloatingPreviewPanel.Size = new System.Drawing.Size(200, 100);
            this.FloatingPreviewPanel.TabIndex = 4;
            // 
            // BTN_CreateBlank
            // 
            this.BTN_CreateBlank.AutoSize = true;
            this.BTN_CreateBlank.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BTN_CreateBlank.Location = new System.Drawing.Point(92, 45);
            this.BTN_CreateBlank.Name = "BTN_CreateBlank";
            this.BTN_CreateBlank.Size = new System.Drawing.Size(78, 23);
            this.BTN_CreateBlank.TabIndex = 3;
            this.BTN_CreateBlank.Text = "Create Blank";
            this.BTN_CreateBlank.UseVisualStyleBackColor = true;
            this.BTN_CreateBlank.Click += new System.EventHandler(this.BTN_CreateBlank_Click);
            // 
            // MultiEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FloatingPreviewPanel);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MultiEditor";
            this.Size = new System.Drawing.Size(619, 324);
            this.Load += new System.EventHandler(this.OnLoad);
            this.TC_MultiEditorToolbox.ResumeLayout(false);
            this.tileTab.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDrawTiles)).EndInit();
            this.designTab.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Size_Width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Size_Height)).EndInit();
            this.importTab.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Floor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Z)).EndInit();
            this.Selectedpanel.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Selected_Z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Selected_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Selected_X)).EndInit();
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
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelCoord;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TreeView treeViewMultiList;
        private System.Windows.Forms.NumericUpDown numericUpDown_Size_Height;
        private System.Windows.Forms.NumericUpDown numericUpDown_Size_Width;
        private System.Windows.Forms.Button BTN_Resize;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.NumericUpDown numericUpDown_Z;
        private System.Windows.Forms.NumericUpDown numericUpDown_Floor;
        private System.Windows.Forms.CheckBox BTN_Floor;
        private System.Windows.Forms.CheckBox BTN_Z;
        private System.Windows.Forms.CheckBox BTN_Remove;
        private System.Windows.Forms.CheckBox BTN_Draw;
        private System.Windows.Forms.CheckBox BTN_Select;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_SaveToID;
        private System.Windows.Forms.Button BTN_Save;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.ToolStripLabel SelectedTileLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown numericUpDown_Selected_Z;
        private System.Windows.Forms.NumericUpDown numericUpDown_Selected_Y;
        private System.Windows.Forms.NumericUpDown numericUpDown_Selected_X;
        private FiddlerControls.CollapsibleSplitter collapsibleSplitter1;
        private System.Windows.Forms.Panel Selectedpanel;
        private System.Windows.Forms.ImageList imageListTools;
        private System.Windows.Forms.PictureBox pictureBoxDrawTiles;
        private System.Windows.Forms.VScrollBar vScrollBarDrawTiles;
        private System.Windows.Forms.Panel FloatingPreviewPanel;
        private System.Windows.Forms.Button BTN_CreateBlank;
    }
}
