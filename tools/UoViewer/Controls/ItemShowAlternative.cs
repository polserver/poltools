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
    public partial class ItemShowAlternative : UserControl
    {
        public ItemShowAlternative()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            refMarker = this;
            pictureBox.MouseWheel += new MouseEventHandler(OnMouseWheel);
            pictureBox.Image = bmp;
        }

        private static ItemShowAlternative refMarker = null;
        private ArrayList ItemList = new ArrayList();
        private int col;
        private int row;
        private int selected = -1;
        private Bitmap bmp;

        private void MakeHashFile()
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, "UOViewerArt.hash");
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryWriter bin = new BinaryWriter(fs);
                byte[] md5 = FileIndex.GetMD5(Client.GetFilePath("Art.mul"));
                int length = md5.Length;
                bin.Write(length);
                bin.Write(md5);
                foreach (int item in ItemList)
                {
                    bin.Write(item);
                }
            }
        }

        public static bool SearchGraphic(int graphic)
        {
            int index = 0;
            if (refMarker.selected >= 0)
                index = refMarker.selected + 1;
            if (index >= refMarker.ItemList.Count)
                index = 0;

            for (int i = index; i < refMarker.ItemList.Count; i++)
            {
                if ((int)refMarker.ItemList[i] == graphic)
                {
                    refMarker.selected = i;
                    refMarker.vScrollBar.Value=i/refMarker.col+1;
                    refMarker.namelabel.Text = String.Format("Name: {0}", TileData.ItemTable[i].Name);
                    refMarker.graphiclabel.Text = String.Format("Graphic: 0x{0:X4} ({0})", i);
                    refMarker.UpdateDetail(i);
                    refMarker.PaintBox();
                    return true;
                }
            }
            return false;
        }

        public static bool SearchName(string name)
        {
            int index = 0;
            if (refMarker.selected >= 0)
                index = refMarker.selected + 1;
            if (index >= refMarker.ItemList.Count)
                index = 0;

            for (int i = index; i < refMarker.ItemList.Count; i++)
            {
                if (TileData.ItemTable[(int)refMarker.ItemList[i]].Name.Contains(name))
                {
                    refMarker.selected = i;
                    refMarker.vScrollBar.Value = i / refMarker.col + 1;
                    refMarker.namelabel.Text = String.Format("Name: {0}", TileData.ItemTable[i].Name);
                    refMarker.graphiclabel.Text = String.Format("Graphic: 0x{0:X4} ({0})", i);
                    refMarker.UpdateDetail(i);
                    refMarker.PaintBox();
                    return true;
                }
            }
            return false;
        }

        public int GetIndex(int x, int y)
        {
            int value = Math.Max(0,((col * (vScrollBar.Value - 1)) + (x + (y * col))));
            if (ItemList.Count > value)
                return (int)ItemList[value];
            else
                return -1;
        }

        private bool Loaded = false;
        public void Reload()
        {
            if (Loaded)
                OnLoad(this, EventArgs.Empty);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            Loaded = true;
            ItemList = new ArrayList();
            if ((FileIndex.UseHashFile) && (FileIndex.CompareHashFile("Art")))
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string FileName = Path.Combine(path, "UOViewerArt.hash");
                if (File.Exists(FileName))
                {
                    using (BinaryReader bin = new BinaryReader(new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read)))
                    {
                        int length = bin.ReadInt32();
                        bin.BaseStream.Seek(length, SeekOrigin.Current);
                        while (bin.BaseStream.Length != bin.BaseStream.Position)
                        {
                            int i = bin.ReadInt32();
                            ItemList.Add((object)i);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 0x4000; i++)
                {
                    if (Art.IsValidStatic(i))
                        ItemList.Add((object)i);
                }
                if (FileIndex.UseHashFile)
                    MakeHashFile();
            }
            vScrollBar.Maximum = ItemList.Count / col + 1;
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
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

        private void OnScroll(object sender, ScrollEventArgs e)
        {
            PaintBox();
        }

        private void PaintBox()
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                for (int x = 0; x <= col; x++)
                {
                    g.DrawLine(Pens.Gray, new Point(x * Options.ArtItemSizeWidth, 0),
                        new Point(x * Options.ArtItemSizeWidth, row * Options.ArtItemSizeHeight));
                }

                for (int y = 0; y <= row; y++)
                {
                    g.DrawLine(Pens.Gray, new Point(0, y * Options.ArtItemSizeHeight),
                        new Point(col * Options.ArtItemSizeWidth, y * Options.ArtItemSizeHeight));
                }

                for (int y = 0; y < row; y++)
                {
                    for (int x = 0; x < col; x++)
                    {
                        int index = GetIndex(x, y);
                        if (index >= 0)
                        {
                            Bitmap b = Art.GetStatic(index);

                            if (b != null)
                            {
                                Point loc = new Point((x * Options.ArtItemSizeWidth) + 1, (y * Options.ArtItemSizeHeight) + 1);
                                Size size = new Size(Options.ArtItemSizeHeight - 1, Options.ArtItemSizeWidth - 1);
                                Rectangle rect = new Rectangle(loc, size);

                                g.Clip = new Region(rect);

                                if (index == selected)
                                    g.FillRectangle(Brushes.LightBlue, rect);

                                if (Options.ArtItemClip)
                                    g.DrawImage(b, loc);
                                else
                                {
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
                                }
                            }
                        }
                    }
                }
                g.Save();
            }
            pictureBox.Image = bmp;
            pictureBox.Update();
        }

        private void OnResize(object sender, EventArgs e)
        {
            col = pictureBox.Width / Options.ArtItemSizeWidth;
            row = pictureBox.Height / Options.ArtItemSizeHeight;
            vScrollBar.Maximum = ItemList.Count / col + 1;
            vScrollBar.Minimum = 1;
            vScrollBar.SmallChange = 1;
            vScrollBar.LargeChange = row;
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            PaintBox();
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            pictureBox.Focus();
            Point m = PointToClient(Control.MousePosition);
            int x = m.X / (Options.ArtItemSizeWidth - 1);
            int y = m.Y / (Options.ArtItemSizeHeight - 1);
            int index = GetIndex(x, y);
            if (index >= 0)
            {
                selected = index;
                namelabel.Text = String.Format("Name: {0}", TileData.ItemTable[selected].Name);
                graphiclabel.Text = String.Format("Graphic: 0x{0:X4} ({0})", selected);
                UpdateDetail(selected);
                PaintBox();
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

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point m = PointToClient(Control.MousePosition);
            int x = m.X / (Options.ArtItemSizeWidth - 1);
            int y = m.Y / (Options.ArtItemSizeHeight - 1);
            int index = GetIndex(x, y);
            if (index >= 0)
            {
                ItemDetail f = new ItemDetail(index);
                f.TopMost = true;
                f.Show();
            }
        }

        private ItemSearch showform = null;
        private void OnSearchClick(object sender, EventArgs e)
        {
            if ((showform == null) || (showform.IsDisposed))
            {
                showform = new ItemSearch(true);
                showform.TopMost = true;
                showform.Show();
            }
        }

        private void OnClickPreload(object sender, EventArgs e)
        {
            if (PreLoader.IsBusy)
                return;
            ProgressBar.Minimum = 1;
            ProgressBar.Maximum = ItemList.Count;
            ProgressBar.Step = 1;
            ProgressBar.Value = 1;
            ProgressBar.Visible = true;
            PreLoader.RunWorkerAsync();
        }

        private void PreLoaderDoWork(object sender, DoWorkEventArgs e)
        {
            foreach (int item in ItemList)
            {
                Art.GetStatic(item);
                PreLoader.ReportProgress(1);
            }
        }

        private void PreLoaderProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.PerformStep();
        }

        private void PreLoaderCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBar.Visible = false;
        }
    }
}
