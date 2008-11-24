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
    public partial class LandTiles : UserControl
    {
        public LandTiles()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            listView1.BeginUpdate();

            for (int i = 0; i < 0x4000; i++)
            {
                if (Art.IsValidLand(i))
                {
                    ListViewItem item = new ListViewItem(i.ToString(), 0);
                    item.Tag = i;
                    listView1.Items.Add(item);
                }
            }

            listView1.TileSize = new Size(49, 49);
            listView1.EndUpdate();
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int i = (int)listView1.SelectedItems[0].Tag;
                namelabel.Text = String.Format("Name: {0}", TileData.LandTable[i].Name);
                graphiclabel.Text = String.Format("ID: 0x{0:X4}", i);
                FlagsLabel.Text = String.Format("Flags: {0}", TileData.LandTable[i].Flags);
            }
        }

        private void drawitem(object sender, DrawListViewItemEventArgs e)
        {
            int i = (int)listView1.Items[e.ItemIndex].Tag;

            Bitmap bmp = Art.GetLand(i);

            //index 21696 is valid index, but no valid bitmap data?

            if (bmp != null)
            {
                int width = bmp.Width;
                int height = bmp.Height;

                if (width > e.Bounds.Width)
                    width = e.Bounds.Width - 2;

                if (height > e.Bounds.Height)
                    height = e.Bounds.Height - 2;

                e.Graphics.DrawImage(bmp, e.Bounds.X + 1, e.Bounds.Y + 1,
                                     new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1),
                                     GraphicsUnit.Pixel);

                if (listView1.SelectedItems.Contains(e.Item))
                    e.DrawFocusRectangle();
                else
                    e.Graphics.DrawRectangle(Pens.Gray, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }

        }
    }
}
