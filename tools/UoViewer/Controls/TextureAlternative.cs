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
using Ultima;

namespace Controls
{
    public partial class TextureAlternative : UserControl
    {
        public TextureAlternative()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            pictureBox.MouseWheel += new MouseEventHandler(OnMouseWheel);
            pictureBox.Image = bmp;
            refMarker = this;
        }
        private static TextureAlternative refMarker = null;
        private ArrayList TextureList = new ArrayList();
        private int col;
        private int row;
        private int selected = -1;
        private Bitmap bmp;

        private bool Loaded = false;
        public void Reload()
        {
            if (!Loaded)
                return;
            TextureList = new ArrayList();
            selected = -1;
            OnLoad(this, EventArgs.Empty);
        }

        public static bool SearchGraphic(int graphic)
        {
            int index = 0;

            for (int i = index; i < refMarker.TextureList.Count; i++)
            {
                if ((int)refMarker.TextureList[i] == graphic)
                {
                    refMarker.selected = graphic;
                    refMarker.vScrollBar.Value = i / refMarker.col + 1;
                    refMarker.Label.Text = String.Format("Graphic: 0x{0:X4} ({0})", graphic);
                    refMarker.PaintBox();
                    return true;
                }
            }
            return false;
        }

        public int GetIndex(int x, int y)
        {
            int value = Math.Max(0, ((col * (vScrollBar.Value - 1)) + (x + (y * col))));
            if (TextureList.Count > value)
                return (int)TextureList[value];
            else
                return -1;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            Loaded = true;
            for (int i = 0; i < 0x1000; i++)
            {
                if (Textures.TestTexture(i))
                    TextureList.Add((object)i);
            }
            vScrollBar.Maximum = TextureList.Count / col + 1;
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            PaintBox();
            this.Cursor = Cursors.Default;
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
                    g.DrawLine(Pens.Gray, new Point(x * 64, 0),
                        new Point(x * 64, row * 64));
                }

                for (int y = 0; y <= row; y++)
                {
                    g.DrawLine(Pens.Gray, new Point(0, y * 64),
                        new Point(col * 64, y * 64));
                }

                for (int y = 0; y < row; y++)
                {
                    for (int x = 0; x < col; x++)
                    {
                        int index = GetIndex(x, y);
                        if (index >= 0)
                        {
                            Bitmap b = Textures.GetTexture(index);

                            if (b != null)
                            {
                                Point loc = new Point((x * 64) + 1, (y * 64) + 1);
                                Size size = new Size(64 - 1, 64 - 1);
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
                                    g.DrawRectangle(Pens.LightBlue,rect.X,rect.Y,rect.Width-1,rect.Height-1);
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
            col = pictureBox.Width / 64;
            row = pictureBox.Height / 64;
            vScrollBar.Maximum = TextureList.Count / col + 1;
            vScrollBar.Minimum = 1;
            vScrollBar.SmallChange = 1;
            vScrollBar.LargeChange = row;
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            PaintBox();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point m = PointToClient(Control.MousePosition);
            int x = m.X / (64 - 1);
            int y = m.Y / (64 - 1);
            int index = GetIndex(x, y);
            if (index >= 0)
            {
                if (selected != index)
                {
                    selected = index;
                    Label.Text = String.Format("Graphic: 0x{0:X4} ({0})", selected);
                    PaintBox();
                }
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            pictureBox.Focus();
        }

        private TextureSearch showform = null;
        private void OnClickSearch(object sender, EventArgs e)
        {
            if ((showform == null) || (showform.IsDisposed))
            {
                showform = new TextureSearch(true);
                showform.TopMost = true;
                showform.Show();
            }
        }
    }
}
