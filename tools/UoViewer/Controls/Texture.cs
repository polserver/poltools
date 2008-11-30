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

        private bool Loaded = false;
        public void Reload()
        {
            if (Loaded)
                OnLoad(this, EventArgs.Empty);
        }

        private void drawitem(object sender, DrawListViewItemEventArgs e)
        {
            int i = (int)e.Item.Tag;

            Bitmap bmp = Textures.GetTexture(i);

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
                    e.Graphics.DrawRectangle(Pens.Gray, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }
        }

        private void istView_SelectedIndexChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
                Label.Text = String.Format("Graphic: 0x{0:X4} ({0})", (int)listView1.SelectedItems[0].Tag);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Loaded = true;
            listView1.BeginUpdate();
            listView1.Clear();
            ListViewItem item;
            for (int i = 0; i < 0x1000; i++)
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
