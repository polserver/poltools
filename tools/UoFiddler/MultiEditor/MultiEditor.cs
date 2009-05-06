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

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Ultima;

namespace MultiEditor
{
    public partial class MultiEditor : UserControl
    {
		#region Fields (8) 

        private MultiEditorComponentList compList;
        private bool Loaded = false;
        private int m_DrawFloorZ;
        private MultiTile m_DrawTile;
        private MultiTile m_HoverTile;
        private MultiTile m_SelectedTile;
        /// <summary>
        /// Current MouseLoc + Scrollbar values (for hover effect)
        /// </summary>
        private Point MouseLoc;
        private static MultiEditor refMarkerMulti = null;

		#endregion Fields 

		#region Constructors (1) 

        public MultiEditor()
        {
            InitializeComponent();
            refMarkerMulti = this;
            InitializeToolBox();
            MouseLoc = new Point();
            m_DrawTile = new MultiTile();
        }

		#endregion Constructors 

		#region Properties (3) 

        /// <summary>
        /// Floor Z level
        /// </summary>
        public int DrawFloorZ { get { return m_DrawFloorZ; } }

        /// <summary>
        /// Current Hovered Tile (set inside MultiComponentList)
        /// </summary>
        public MultiTile HoverTile
        {
            get { return m_HoverTile; }
            set
            {
                m_HoverTile = value;
                if (value != null)
                    toolTip1.SetToolTip(pictureBoxMulti, String.Format("ID: 0x{0:X} Z: {1}", m_HoverTile.ID, m_HoverTile.Z));
            }
        }

        /// <summary>
        /// Current Selected Tile (set OnMouseUp)
        /// </summary>
        public MultiTile SelectedTile
        {
            get { return m_SelectedTile; }
        }

		#endregion Properties 

		#region Methods (22) 

		// Private Methods (22) 

        private void AddChildren(TreeNode node, XmlElement mainNode)
        {
            foreach (XmlElement e in mainNode)
            {

                TreeNode tempNode = new TreeNode();

                tempNode.Text = e.GetAttribute("name");
                tempNode.Tag = e.GetAttribute("index");
                if (e.Name == "subgroup")
                {
                    tempNode.ImageIndex = 0;
                }
                else
                {
                    tempNode.Text = tempNode.Tag.ToString();
                    tempNode.ImageIndex = 1;
                }

                if (e.HasChildNodes)
                {
                    AddChildren(tempNode, e);
                }

                node.Nodes.Add(tempNode);
            }
        }

        private void ConvertCoords(Point point, out int x, out int y, out int z)
        {
            //first check if current Tile matches
            if (HoverTile != null)
            {
                //visible?
                if ((!DrawFloortoolStripButton.Checked) || (HoverTile.Z >= DrawFloorZ))
                {
                    for (int x_ = 0; x_ < compList.Width; x_++)
                    {
                        for (int y_ = 0; y_ < compList.Height; y_++)
                        {
                            for (int i = 0; i < compList.Tiles[x_][y_].Count; i++)
                            {
                                if (HoverTile == compList.Tiles[x_][y_][i])
                                {
                                    x = x_;
                                    y = y_;
                                    z = HoverTile.Z + HoverTile.Height;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            //damn the hard way
            z = 0;

            int cx = 0; //Get MouseCoords for (0/0)
            int cy = 0;
            if (DrawFloortoolStripButton.Checked)
            {
                cy -= DrawFloorZ * 4;
                z = DrawFloorZ;
            }
            cy -= 44;
            cx -= compList.xMin;
            cy -= compList.yMin;
            cy += 22; //Mod for a bit of gap
            cx += 44;

            double mx = MouseLoc.X - cx;
            double my = MouseLoc.Y - cy;
            double xx = mx;
            double yy = my;
            my = xx * Math.Cos(Math.PI / 4) - yy * Math.Sin(Math.PI / 4); //Rotate 45° Coordinate system
            mx = yy * Math.Cos(Math.PI / 4) + xx * Math.Sin(Math.PI / 4);
            mx /= Math.Sqrt(2) * 22;
            my /= Math.Sqrt(2) * 22;
            my *= -1;
            x = (int)mx;
            y = (int)my;
        }

        private void InitializeToolBox()
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, @"plugins/multieditor.xml");
            if (!File.Exists(FileName))
                return;

            XmlDocument dom = new XmlDocument();
            dom.Load(FileName);
            XmlElement xTiles = dom["TileGroups"];

            foreach (XmlElement xRootGroup in xTiles)
            {
                TreeNode mainNode = new TreeNode();
                mainNode.Text = xRootGroup.GetAttribute("name");
                mainNode.Tag = null;

                mainNode.ImageIndex = 0;

                AddChildren(mainNode, xRootGroup);

                treeViewTilesXML.Nodes.Add(mainNode);
            }
        }

        private void OnAfterSelectTreeViewTilesXML(object sender, TreeViewEventArgs e)
        {
            if (e.Node.ImageIndex == 0)
                return;
            int id = Int32.Parse((string)e.Node.Tag);
            Bitmap bmp = Ultima.Art.GetStatic(id);
            if (bmp != null)
            {
                panelTilesView.BackgroundImage = bmp;
                m_DrawTile.Set(id, 0);
            }
        }

        /// <summary>
        /// Set Z level -1 for selected tile
        /// </summary>
        private void OnClickDownZ(object sender, EventArgs e)
        {
            if (SelectedTile == null)
                return;
            compList.TileModZ(SelectedTile, -1);
            MaxHeightTrackBar.Maximum = compList.zMax;
            pictureBoxMulti.Refresh();
        }

        /// <summary>
        /// Change Floordraw
        /// </summary>
        private void OnClickDrawFloor(object sender, EventArgs e)
        {
            int z;
            if (Int32.TryParse(DrawFloortoolStripTextBox.Text, out z))
                m_DrawFloorZ = z;

            SetScrollbars();
            pictureBoxMulti.Refresh();
        }

        /// <summary>
        /// Switch to draw mode
        /// </summary>
        private void OnClickDrawTile(object sender, EventArgs e)
        {
            if (m_DrawTile.ID < 0)
                DrawTileButton.Checked = false;
            pictureBoxMulti.Refresh();
        }

        /// <summary>
        /// Remove selected Tile
        /// </summary>
        private void OnClickRemoveTile(object sender, EventArgs e)
        {
            if (SelectedTile == null)
                return;
            compList.RemoveTile(SelectedTile);
            MaxHeightTrackBar.Maximum = compList.zMax;
            pictureBoxMulti.Refresh();
        }

        private void OnClickResizeMulti(object sender, EventArgs e)
        {
            int width = (int)numericUpDown1.Value;
            int height = (int)numericUpDown2.Value;
            if (compList != null)
            {
                compList.Resize(width, height);
                MaxHeightTrackBar.Maximum = compList.zMax;
                MaxHeightTrackBar.Value = compList.zMax;
                SetScrollbars();
                pictureBoxMulti.Refresh();
            }
        }

        /// <summary>
        /// Save to given multiid
        /// </summary>
        private void OnClickSave(object sender, EventArgs e)
        {
            int id;
            if (Int32.TryParse(toolStripTextBoxSaveID.Text, out id))
                compList.AddToSDKComponentList(id);
        }

        /// <summary>
        /// Set Z level +1 for selected tile
        /// </summary>
        private void OnClickUpZ(object sender, EventArgs e)
        {
            if (SelectedTile == null)
                return;
            compList.TileModZ(SelectedTile, +1);
            MaxHeightTrackBar.Maximum = compList.zMax;
            if (MaxHeightTrackBar.Value<SelectedTile.Z)
                MaxHeightTrackBar.Value = SelectedTile.Z;
            pictureBoxMulti.Refresh();
        }

        private void OnFilePathChangeEvent()
        {
            if (Loaded)
                OnLoad(null,null);
        }

        /// <summary>
        /// Keys.Enter refreshes Floor Z
        /// </summary>
        private void OnKeyDownDrawFloorEntry(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int z;
                if (Int32.TryParse(DrawFloortoolStripTextBox.Text, out z))
                {
                    m_DrawFloorZ = z;
                    if (DrawFloortoolStripButton.Checked)
                    {
                        SetScrollbars();
                        pictureBoxMulti.Refresh();
                    }
                }
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            FiddlerControls.Options.LoadedUltimaClass["TileData"] = true;
            FiddlerControls.Options.LoadedUltimaClass["Art"] = true;
            FiddlerControls.Options.LoadedUltimaClass["Multis"] = true;
            FiddlerControls.Options.LoadedUltimaClass["Hues"] = true;
            treeViewMultiList.BeginUpdate();
            treeViewMultiList.Nodes.Clear();
            for (int i = 0; i < 0x2000; i++)
            {
                if (Ultima.Multis.GetComponents(i) != MultiComponentList.Empty)
                {
                    TreeNode node = new TreeNode(String.Format("{0,5} (0x{0:X})", i));
                    node.Tag = i;
                    node.Name = i.ToString();
                    treeViewMultiList.Nodes.Add(node);
                }
            }
            treeViewMultiList.EndUpdate();
            if (!Loaded)
                FiddlerControls.Options.FilePathChangeEvent += new FiddlerControls.Options.FilePathChangeHandler(OnFilePathChangeEvent);
            Loaded = true;
        }

        /// <summary>
        /// Hover effect
        /// </summary>
        private void OnMouseMovePictureBoxMulti(object sender, MouseEventArgs e)
        {
            MouseLoc = e.Location;
            MouseLoc.X += hScrollBar.Value;
            MouseLoc.Y += vScrollBar.Value;
            pictureBoxMulti.Refresh();
        }

        /// <summary>
        /// Draw/Select a Tile
        /// </summary>
        private void OnMouseUpPictureBoxMulti(object sender, MouseEventArgs e)
        {
            if (compList == null)
                return;
            if (DrawTileButton.Checked)
            {
                int x, y, z;
                ConvertCoords(MouseLoc, out x, out y, out z);
                if ((x >= 0) && (x < compList.Width) && (y >= 0) && (y < compList.Height))
                {
                    compList.AddTile(x, y, z, m_DrawTile.ID);
                    MaxHeightTrackBar.Maximum = compList.zMax;
                    if (MaxHeightTrackBar.Value < z)
                        MaxHeightTrackBar.Value = z;
                }
            }
            else
                m_SelectedTile = compList.GetSelected(MouseLoc, MaxHeightTrackBar.Value);
            pictureBoxMulti.Refresh();
        }

        /// <summary>
        /// Draw Image
        /// </summary>
        private void OnPaintPictureBoxMulti(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            Bitmap bit = null;
            if (compList != null)
            {
                bit = compList.GetImage(MaxHeightTrackBar.Value, MouseLoc, DrawFloortoolStripButton.Checked);
            }
            if (bit != null)
            {
                e.Graphics.DrawImageUnscaled(bit, -hScrollBar.Value, -vScrollBar.Value);
                int x, y, z;
                ConvertCoords(MouseLoc, out x, out y, out z);
                if ((x >= 0) && (x < compList.Width) && (y >= 0) && (y < compList.Height))
                    toolStripLabelCoord.Text = String.Format("{0},{1},{2}", x, y, z);

                if (DrawTileButton.Checked)
                {
                    if (m_DrawTile.ID >= 0)
                    {
                        if ((x >= 0) && (x < compList.Width) && (y >= 0) && (y < compList.Height))
                        {
                            toolStripLabelCoord.Text = String.Format("{0},{1},{2}", x, y, z);
                            Bitmap bmp = Art.GetStatic(m_DrawTile.ID);

                            if (bmp == null)
                                return;
                            int px = (x - y) * 22;
                            int py = (x + y) * 22;

                            px -= (bmp.Width / 2);
                            py -= z * 4;
                            py -= bmp.Height;
                            px -= compList.xMin;
                            py -= compList.yMin;
                            py += 22; //Mod for a bit of gap
                            px += 44;
                            e.Graphics.DrawImage(bmp, new Rectangle(px, py, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, MultiTile.DrawColor);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// PictureBox size changed
        /// </summary>
        private void OnResizePictureBoxMulti(object sender, EventArgs e)
        {
            SetScrollbars();
            pictureBoxMulti.Refresh();
        }

        /// <summary>
        /// Scrollbars changed
        /// </summary>
        private void OnScrollBarValueChanged(object sender, EventArgs e)
        {
            pictureBoxMulti.Refresh();
        }

        /// <summary>
        /// Value of TrackBar changed (for displayed MaxHeight)
        /// </summary>
        private void OnValueChangedMaxHeight(object sender, EventArgs e)
        {
            SetScrollbars();
            toolTip1.SetToolTip(MaxHeightTrackBar, MaxHeightTrackBar.Value.ToString());
            pictureBoxMulti.Refresh();
        }

        /// <summary>
        /// Does the Multi fit inside the PictureBox
        /// </summary>
        private void SetScrollbars()
        {
            if (compList == null)
                return;
            Bitmap bit = compList.GetImage(MaxHeightTrackBar.Value, Point.Empty, DrawFloortoolStripButton.Checked);
            if (bit == null)
                return;
            if (bit.Height <= pictureBoxMulti.Height + hScrollBar.Height)
            {
                vScrollBar.Enabled = false;
                vScrollBar.Value = 0;
            }
            else
            {
                vScrollBar.Enabled = true;
                vScrollBar.Maximum = bit.Height - pictureBoxMulti.Height + 10;
            }
            if (bit.Width <= pictureBoxMulti.Width + vScrollBar.Width)
            {
                hScrollBar.Enabled = false;
                vScrollBar.Value = 0;
            }
            else
            {
                hScrollBar.Enabled = true;
                hScrollBar.Maximum = bit.Width - pictureBoxMulti.Width + 10;
            }
        }

        /// <summary>
        /// Doubleclick Node of Import treeview
        /// </summary>
        private void treeViewMultiList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to open selected Multi?", "Open", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                compList = new MultiEditorComponentList(Ultima.Multis.GetComponents((int)e.Node.Tag), this);
                MaxHeightTrackBar.Maximum = compList.zMax;
                MaxHeightTrackBar.Value = compList.zMax;
                toolStripTextBoxSaveID.Text = e.Node.Tag.ToString();
                SetScrollbars();
                pictureBoxMulti.Refresh();
            }
        }

		#endregion Methods 
    }
}
