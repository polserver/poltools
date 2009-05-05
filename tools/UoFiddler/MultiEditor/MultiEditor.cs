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

        private static MultiEditor refMarkerMulti = null;
        private MultiEditorComponentList compList;

        /// <summary>
        /// Current MouseLoc + Scrollbar values (for hover effect)
        /// </summary>
        private Point MouseLoc;
       
        private MultiTile m_SelectedTile;
        private MultiTile m_HoverTile;
        private MultiTile m_DrawTile;
        private int m_DrawFloorZ;

        /// <summary>
        /// Current Selected Tile (set OnMouseUp)
        /// </summary>
        public MultiTile SelectedTile
        {
            get { return m_SelectedTile; }
        }

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
        /// Floor Z level
        /// </summary>
        public int DrawFloorZ { get { return m_DrawFloorZ; } }

        public MultiEditor()
        {
            InitializeComponent();
            refMarkerMulti = this;
            InitializeToolBox();
            MouseLoc = new Point();

            //Todo: Make it selectable
            compList = new MultiEditorComponentList(Ultima.Multis.GetComponents(100),this);
            MaxHeightTrackBar.Maximum = compList.zMax;
            MaxHeightTrackBar.Value = compList.zMax;
            SetScrollbars();
            m_DrawTile = new MultiTile(550, 0);
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

        private void AddChildren(TreeNode node, XmlElement mainNode)
        {
            foreach (XmlElement e in mainNode)
            {
                if (e.Name == "subgroup")
                {
                    TreeNode tempNode = new TreeNode();

                    tempNode.Text = e.GetAttribute("name");
                    tempNode.Tag = e.GetAttribute("index");
                    tempNode.ImageIndex = 0;

                    if (e.HasChildNodes)
                    {
                        AddChildren(tempNode, e);
                    }

                    node.Nodes.Add(tempNode);
                }
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
            x = MouseLoc.X;
            y = MouseLoc.Y;
            z = -128;
            int xOffset = (compList.xMax - compList.xMin + 88) / 2;
            int yOffset = ((compList.yMax - compList.yMin + 88) - ((compList.Height + compList.Width) * 22)) /*/ 2*/ + 22;

            x -= xOffset;
            y -= yOffset;

            if (DrawFloortoolStripButton.Checked)
            {
                y += DrawFloorZ * 4;
                z = DrawFloorZ;
            }
            y += 44;

            int vx = (x + y);
            int vy = (y - x);
            if (vx < 0)
                vx -= 44;
            if (vy < 0)
                vy -= 44;
            vx /= 44;
            vy /= 44;
            x = vx;
            y = vy;
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
                e.Graphics.DrawImageUnscaled(bit, -hScrollBar.Value, -vScrollBar.Value);
            if (DrawTileButton.Checked)
            {
                if (m_DrawTile != null)
                {
                    int x, y, z;
                    ConvertCoords(MouseLoc, out x, out y, out z);
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
                        px += 22;
                        e.Graphics.DrawImage(bmp, new Rectangle(px, py, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, MultiTile.DrawColor);
                    }
                }
            }
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
                }
            }
            else
                m_SelectedTile = compList.GetSelected(MouseLoc, MaxHeightTrackBar.Value);
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
        /// PictureBox size changed
        /// </summary>
        private void OnResizePictureBoxMulti(object sender, EventArgs e)
        {
            SetScrollbars();
            pictureBoxMulti.Refresh();
        }

        /// <summary>
        /// Does the Multi fit inside the PictureBox
        /// </summary>
        private void SetScrollbars()
        {
            if (compList == null)
                return;
            Bitmap bit = compList.GetImage(MaxHeightTrackBar.Value, Point.Empty, false);
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
        /// Scrollbars changed
        /// </summary>
        private void OnScrollBarValueChanged(object sender, EventArgs e)
        {
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
            pictureBoxMulti.Refresh();
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
                        pictureBoxMulti.Refresh();
                }
            }
        }

        private void OnClickDrawTile(object sender, EventArgs e)
        {
            pictureBoxMulti.Refresh();
        }
    }
}
