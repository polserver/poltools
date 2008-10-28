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
    
    public partial class ItemShow : UserControl
    {
        public ItemShow()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            refMarker = this;
        }
        public static int ItemSizeWidth = 48;  // ListViewSize
        public static int ItemSizeHeight = 48;
        public static bool ItemClip = false;

        private static ItemShow refMarker = null;

        public static bool SearchGraphic(int graphic)
        {
            int index = 0;
            if (refMarker.listView1.SelectedIndices.Count == 1)
                index = refMarker.listView1.SelectedIndices[0]+1;
            if (index >= refMarker.listView1.Items.Count)
                index = 0;

            for (int i = index; i < refMarker.listView1.Items.Count; i++)
            {
                ListViewItem item = refMarker.listView1.Items[i];
                if ((int)item.Tag == graphic)
                {
                    if (refMarker.listView1.SelectedItems.Count == 1)
                        refMarker.listView1.SelectedItems[0].Selected = false;
                    item.Selected = true;
                    item.Focused = true;
                    item.EnsureVisible();
                    return true;
                }
            }
            return false;
        }

        public static bool SearchName(string name)
        {
            int index=0;
            if (refMarker.listView1.SelectedIndices.Count == 1)
                index = refMarker.listView1.SelectedIndices[0]+1;
            if (index >= refMarker.listView1.Items.Count)
                index = 0;

            for (int i = index; i < refMarker.listView1.Items.Count; i++)
            {
                ListViewItem item = refMarker.listView1.Items[i];
                if (TileData.ItemTable[(int)item.Tag].Name.Contains(name))
                {
                    if (refMarker.listView1.SelectedItems.Count == 1)
                        refMarker.listView1.SelectedItems[0].Selected = false;
                    item.Selected = true;
                    item.Focused = true;
                    item.EnsureVisible();
                    return true;
                }
            }
            return false;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            listView1.BeginUpdate();

            if (!OSFeature.Feature.IsPresent(OSFeature.Themes))
            {
                listView1.View = View.SmallIcon;
                listView1.SmallImageList = new ImageList();
                listView1.SmallImageList.ImageSize = new Size(ItemSizeWidth, ItemSizeHeight);
                listView1.SmallImageList.ColorDepth = ColorDepth.Depth32Bit;
                listView1.LabelWrap = false;
                Bitmap bmptest = new Bitmap(ItemSizeWidth, ItemSizeHeight);
                Graphics test = Graphics.FromImage(bmptest);
                test.Clear(Color.White);
                listView1.SmallImageList.Images.Add(bmptest);
            }

            for (int i = 0; i < 0x4000; i++)
            {
                if (Art.IsValidStatic(i))
                {
                    ListViewItem item = new ListViewItem(i.ToString(), 0);
                    item.Tag = i;
                    listView1.Items.Add(item);
                }
            }
            if (OSFeature.Feature.IsPresent(OSFeature.Themes))
                listView1.TileSize = new Size(ItemSizeWidth, ItemSizeHeight);

            listView1.EndUpdate();
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int i = (int)listView1.SelectedItems[0].Tag;
                namelabel.Text = String.Format("Name: {0}", TileData.ItemTable[i].Name);
                graphiclabel.Text = String.Format("Graphic: 0x{0:X4}", i);
                UpdateDetail(i);
            }
        }

        private void drawitem(object sender, DrawListViewItemEventArgs e)
        {
            int i = (int)e.Item.Tag;

            Bitmap bmp = Art.GetStatic(i);

            if (bmp != null)
            {
                if (listView1.SelectedItems.Contains(e.Item))
                    e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds.X, e.Bounds.Y, e.Bounds.Width,e.Bounds.Height);
                else
                    e.Graphics.FillRectangle(Brushes.White, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

                if (ItemClip)
                {
                    e.Graphics.DrawImage(bmp, e.Bounds.X + 1, e.Bounds.Y + 1,
                                         new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1),
                                         GraphicsUnit.Pixel);
                }
                else
                {
                    int width=bmp.Width;
                    int height=bmp.Height;
                    if (width > e.Bounds.Width)
                    {
                        width = e.Bounds.Width;
                        height = e.Bounds.Height * bmp.Height/ bmp.Width;
                    }
                    if (height > e.Bounds.Height)
                    {
                        height = e.Bounds.Height;
                        width = e.Bounds.Width * bmp.Width / bmp.Height;
                    }
                    e.Graphics.DrawImage(bmp,
                                         new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, width, height));
                }
                if (!listView1.SelectedItems.Contains(e.Item))
                    e.Graphics.DrawRectangle(new Pen(Color.Gray), e.Bounds.X, e.Bounds.Y, e.Bounds.Width,e.Bounds.Height);
            }
        }

        private void listView_DoubleClicked(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ItemDetail f = new ItemDetail((int)listView1.SelectedItems[0].Tag);
                f.TopMost = true;
                f.Show();
            }
        }

        private ItemSearch showform=null;
        private void Search_Click(object sender, EventArgs e)
        {
            if ((showform == null) || (showform.IsDisposed))
            {
                showform = new ItemSearch();
                showform.TopMost = true;
                showform.Show();
            }
        }

        private void UpdateDetail(int id)
        {
            Ultima.ItemData item = Ultima.TileData.ItemTable[id];
            Bitmap bit = Ultima.Art.GetStatic(id);
            splitContainer2.SplitterDistance = bit.Size.Height + 10;
            Bitmap newbit = new Bitmap(DetailPictureBox.Size.Width, DetailPictureBox.Size.Height);
            Graphics newgraph = Graphics.FromImage(newbit);
            newgraph.Clear(Color.FromArgb(-1));
            newgraph.DrawImage(bit, (DetailPictureBox.Size.Width - bit.Width) / 2, 5);

            DetailPictureBox.Image = newbit;

            DetailTextBox.Clear();
            DetailTextBox.AppendText(String.Format("Name: {0}\n", item.Name));
            DetailTextBox.AppendText(String.Format("Graphic: 0x{0:X4}\n", id));
            DetailTextBox.AppendText(String.Format("Height/Capacity: {0}\n", item.Height));
            DetailTextBox.AppendText(String.Format("Weight: {0}\n", item.Weight));
            DetailTextBox.AppendText(String.Format("Animation: {0}\n", item.Animation));
            DetailTextBox.AppendText(String.Format("Quality/Layer/Light: {0}\n", item.Quality));
            DetailTextBox.AppendText(String.Format("Quantity: {0}\n", item.Quantity));
            DetailTextBox.AppendText(String.Format("Hue: {0}\n", item.Hue));
            DetailTextBox.AppendText(String.Format("StackingOffset/Unk4: {0}\n", item.StackingOffset));
            DetailTextBox.AppendText(String.Format("Flags: {0}\n", item.Flags));
            if ((item.Flags & TileFlag.Animation) != 0)
            {
                Animdata.Data info = Animdata.GetAnimData(id);
                if (info != null)
                    DetailTextBox.AppendText(String.Format("Animation FrameCount: {0} Interval: {1}\n", info.FrameCount, info.FrameInterval));
            }
        }
    }
}
