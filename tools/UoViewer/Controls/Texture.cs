using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ultima;

namespace Controls
{
    public partial class Texture : UserControl
    {
        public Texture()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private static Brush borderBrush = Brushes.Gray;
        private Pen borderPen = new Pen(borderBrush);
        private Bitmap bmp;
        private int i;

        private void drawitem(object sender, DrawListViewItemEventArgs e)
        {
            i = (int)e.Item.Tag;

            bmp = Textures.GetTexture(i);

            if (bmp != null)
            {
                int width = bmp.Width;
                int height = bmp.Height;

                if (width >= e.Bounds.Width)
                    width = e.Bounds.Width - 2;

                if (height >= e.Bounds.Height)
                    height = e.Bounds.Height - 2;

                e.Graphics.DrawImage(bmp, new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, width, height));

                if (listView1.SelectedItems.Contains(e.Item))
                    e.DrawFocusRectangle();
                else
                    e.Graphics.DrawRectangle(borderPen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }
        }

        private void istView_SelectedIndexChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
                Label.Text = String.Format("Graphic: 0x{0:X4}", (int)listView1.SelectedItems[0].Tag);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            ListViewItem item;
            for (i = 0; i < 0x1000; i++)
            {
                if (Textures.TestTexture(i))
                {
                    item = new ListViewItem(i.ToString(),0);
                    item.Tag = i;
                    listView1.Items.Add(item);
                }
            }

            listView1.TileSize = new Size(64, 64);
            listView1.EndUpdate();
        }
    }
}
