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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Ultima;

namespace Controls
{
    public partial class LandTilesAlternative : UserControl
    {
        public LandTilesAlternative()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            pictureBox.MouseWheel += new MouseEventHandler(OnMouseWheel);
            pictureBox.Image = bmp;
        }

        private ArrayList TileList = new ArrayList();
        private int col;
        private int row;
        private int selected = -1;
        private Bitmap bmp;

        public int GetIndex(int x, int y)
        {
            int value = Math.Max(0, ((col * (vScrollBar.Value - 1)) + (x + (y * col))));
            if (TileList.Count > value)
                return (int)TileList[value];
            else
                return -1;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x1000; i++)
            {
                if (Art.IsValidLand(i))
                    TileList.Add((object)i);
            }
            vScrollBar.Maximum = TileList.Count / col + 1;
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            PaintBox();
        }

        private void OnScroll(object sender, ScrollEventArgs e)
        {
            PaintBox();
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                if (vScrollBar.Value < vScrollBar.Maximum)
                {
                    vScrollBar.Value++;
                    PaintBox();
                }
            }
            else
            {
                if (vScrollBar.Value > 1)
                {
                    vScrollBar.Value--;
                    PaintBox();
                }
            }
        }

        private void PaintBox()
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                for (int x = 0; x <= col; x++)
                {
                    g.DrawLine(Pens.Gray, new Point(x * 49, 0),
                        new Point(x * 49, row * 49));
                }

                for (int y = 0; y <= row; y++)
                {
                    g.DrawLine(Pens.Gray, new Point(0, y * 49),
                        new Point(col * 49, y * 49));
                }

                for (int y = 0; y < row; y++)
                {
                    for (int x = 0; x < col; x++)
                    {
                        int index = GetIndex(x, y);
                        if (index >= 0)
                        {
                            Bitmap b = Art.GetLand(index);

                            if (b != null)
                            {
                                Point loc = new Point((x * 49) + 1, (y * 49) + 1);
                                Size size = new Size(49 - 1, 49 - 1);
                                Rectangle rect = new Rectangle(loc, size);

                                g.Clip = new Region(rect);

                                int width = b.Width;
                                int height = b.Height;
                                if (width > size.Width)
                                {
                                    width = size.Width;
                                    height = size.Height * b.Height / b.Width;
                                }
                                if (height > size.Height)
                                {
                                    height = size.Height;
                                    width = size.Width * b.Width / b.Height;
                                }
                                g.DrawImage(b, new Rectangle(loc, new Size(width, height)));
                                if (index == selected)
                                    g.DrawRectangle(Pens.LightBlue, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                            }
                        }
                    }
                }
                g.Save();
            }
            pictureBox.Image = bmp;
        }

        private void OnResize(object sender, EventArgs e)
        {
            col = pictureBox.Width / 49;
            row = pictureBox.Height / 49;
            vScrollBar.Maximum = TileList.Count / col + 1;
            vScrollBar.Minimum = 1;
            vScrollBar.SmallChange = 1;
            vScrollBar.LargeChange = row;
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            PaintBox();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point m = PointToClient(Control.MousePosition);
            int x = m.X / (49 - 1);
            int y = m.Y / (49 - 1);
            int index = GetIndex(x, y);
            if (index >= 0)
            {
                if (selected != index)
                {
                    selected = index;
                    namelabel.Text = String.Format("Name: {0}", TileData.LandTable[index].Name);
                    graphiclabel.Text = String.Format("ID: 0x{0:X4}", index);
                    FlagsLabel.Text = String.Format("Flags: {0}", TileData.LandTable[index].Flags);
                    PaintBox();
                }
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            pictureBox.Focus();
        }
    }
}
