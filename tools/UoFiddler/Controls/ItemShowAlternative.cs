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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
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
        private bool Loaded = false;
        private bool ShowFreeSlots = false;

        private void MakeHashFile()
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, "UOFiddlerArt.hash");
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryWriter bin = new BinaryWriter(fs);
                byte[] md5 = Files.GetMD5(Files.GetFilePath("Art.mul"));
                if (md5 == null)
                    return;
                int length = md5.Length;
                bin.Write(length);
                bin.Write(md5);
                foreach (int item in ItemList)
                {
                    bin.Write(item);
                }
            }
        }

        /// <summary>
        /// Searches Objtype and Select
        /// </summary>
        /// <param name="graphic"></param>
        /// <returns></returns>
        public static bool SearchGraphic(int graphic)
        {
            int index = 0;

            for (int i = index; i < refMarker.ItemList.Count; i++)
            {
                if ((int)refMarker.ItemList[i] == graphic)
                {
                    refMarker.selected = graphic;
                    refMarker.vScrollBar.Value=i/refMarker.col+1;
                    refMarker.namelabel.Text = String.Format("Name: {0}", TileData.ItemTable[graphic].Name);
                    refMarker.graphiclabel.Text = String.Format("Graphic: 0x{0:X4} ({0})", graphic);
                    refMarker.UpdateDetail(graphic);
                    refMarker.PaintBox();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Searches for name and selects
        /// </summary>
        /// <param name="name"></param>
        /// <param name="next">starting from current selected</param>
        /// <returns></returns>
        public static bool SearchName(string name,bool next)
        {
            int index = 0;
            if (next)
            {
                if (refMarker.selected >= 0)
                    index = refMarker.ItemList.IndexOf((object)refMarker.selected) + 1;
                if (index >= refMarker.ItemList.Count)
                    index = 0;
            }

            for (int i = index; i < refMarker.ItemList.Count; i++)
            {
                if (TileData.ItemTable[(int)refMarker.ItemList[i]].Name.Contains(name))
                {
                    refMarker.selected = (int)refMarker.ItemList[i];
                    refMarker.vScrollBar.Value = i / refMarker.col + 1;
                    refMarker.namelabel.Text = String.Format("Name: {0}", TileData.ItemTable[refMarker.selected].Name);
                    refMarker.graphiclabel.Text = String.Format("Graphic: 0x{0:X4} ({0})", refMarker.selected);
                    refMarker.UpdateDetail(refMarker.selected);
                    refMarker.PaintBox();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ReLoads if loaded
        /// </summary>
        public void Reload()
        {
            if (Loaded)
                OnLoad(this, EventArgs.Empty);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            Loaded = true;
            ShowFreeSlots = false;
            showFreeSlotsToolStripMenuItem.Checked = false;
            ItemList = new ArrayList();
            if ((Files.UseHashFile) && (Files.CompareHashFile("Art")))
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string FileName = Path.Combine(path, "UOFiddlerArt.hash");
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
                if (Files.UseHashFile)
                    MakeHashFile();
            }
            vScrollBar.Maximum = ItemList.Count / col + 1;
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            PaintBox();
            this.Cursor = Cursors.Default;
        }

        private int GetIndex(int x, int y)
        {
            int value = Math.Max(0, ((col * (vScrollBar.Value - 1)) + (x + (y * col))));
            if (ItemList.Count > value)
                return (int)ItemList[value];
            else
                return -1;
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
                            else
                            {
                                Point loc = new Point((x * Options.ArtItemSizeWidth) + 1, (y * Options.ArtItemSizeHeight) + 1);
                                Size size = new Size(Options.ArtItemSizeHeight - 1, Options.ArtItemSizeWidth - 1);
                                Rectangle rect = new Rectangle(loc, size);

                                g.Clip = new Region(rect);
                                g.FillRectangle(Brushes.Red, rect);
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
            if (selected!=-1)
                UpdateDetail(selected);
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
                if (!Art.IsValidStatic(index))
                {
                    namelabel.Text = "Name: FREE";
                    graphiclabel.Text = String.Format("Graphic: 0x{0:X4} ({0})", selected);
                }
                else
                {
                    namelabel.Text = String.Format("Name: {0}", TileData.ItemTable[selected].Name);
                    graphiclabel.Text = String.Format("Graphic: 0x{0:X4} ({0})", selected);
                }
                UpdateDetail(selected);
                PaintBox();
            }
        }

        private void UpdateDetail(int id)
        {
            Ultima.ItemData item = Ultima.TileData.ItemTable[id];
            Bitmap bit = Ultima.Art.GetStatic(id);
            if (bit == null)
            {
                splitContainer2.SplitterDistance = 10;
                Bitmap newbit = new Bitmap(DetailPictureBox.Size.Width, DetailPictureBox.Size.Height);
                Graphics newgraph = Graphics.FromImage(newbit);
                newgraph.Clear(Color.FromArgb(-1));
                DetailPictureBox.Image = newbit;
            }
            else
            {
                splitContainer2.SplitterDistance = bit.Size.Height + 10;
                Bitmap newbit = new Bitmap(DetailPictureBox.Size.Width, DetailPictureBox.Size.Height);
                Graphics newgraph = Graphics.FromImage(newbit);
                newgraph.Clear(Color.FromArgb(-1));
                newgraph.DrawImage(bit, (DetailPictureBox.Size.Width - bit.Width) / 2, 5);
                DetailPictureBox.Image = newbit;
            }

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
                showform = new ItemSearch();
                showform.TopMost = true;
                showform.Show();
            }
        }

        private void onClickFindFree(object sender, EventArgs e)
        {
            if (ShowFreeSlots)
            {
                for (int i = ItemList.IndexOf((object)selected) + 1; i < ItemList.Count; i++)
                {
                    if (!Art.IsValidStatic((int)ItemList[i]))
                    {
                        selected = (int)ItemList[i];
                        vScrollBar.Value = i / refMarker.col + 1;
                        namelabel.Text = "Name: FREE";
                        graphiclabel.Text = String.Format("Graphic: 0x{0:X4} ({0})", selected);
                        UpdateDetail(selected);
                        PaintBox();
                        break;
                    }
                }
            }
            else
            {
                int id = selected;
                id++;
                for (int i = ItemList.IndexOf((object)selected) + 1; i < ItemList.Count; i++)
                {
                    if (id < (int)ItemList[i])
                    {
                        selected = (int)ItemList[i];
                        vScrollBar.Value = i / refMarker.col + 1;
                        namelabel.Text = String.Format("Name: {0}", TileData.ItemTable[selected].Name);
                        graphiclabel.Text = String.Format("Graphic: 0x{0:X4} ({0})", selected);
                        UpdateDetail(selected);
                        PaintBox();
                        break;
                    }
                    id++;
                }
            }
        }

        private void OnClickReplace(object sender, EventArgs e)
        {
            if (selected >= 0)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Title = "Choose image file to replace";
                dialog.CheckFileExists = true;
                dialog.Filter = "image file (*.tiff)|*.tiff";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(dialog.FileName);
                    Art.ReplaceStatic(selected, bmp);
                    PaintBox();
                }
            }
        }

        private void OnClickRemove(object sender, EventArgs e)
        {
            if (!Art.IsValidStatic(selected))
                return;
            DialogResult result =
                        MessageBox.Show(String.Format("Are you sure to remove 0x{0:X}", selected), 
                        "Save", 
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                Art.RemoveStatic(selected);
                if (!ShowFreeSlots)
                    ItemList.Remove((object)selected);
                selected--;
                PaintBox();
            }
        }

        private void onTextChangedInsert(object sender, EventArgs e)
        {
            int index;
            bool candone;
            if (InsertText.Text.Contains("0x"))
            {
                string convert = InsertText.Text.Replace("0x", "");
                candone = int.TryParse(convert, System.Globalization.NumberStyles.HexNumber, null, out index);
            }
            else
                candone = int.TryParse(InsertText.Text, System.Globalization.NumberStyles.Integer, null, out index);

            if (index > 0xBFFF)
                candone = false;
            if (candone)
            {
                if (Art.IsValidStatic(index))
                    InsertText.ForeColor = Color.Red;
                else
                    InsertText.ForeColor = Color.Black;
            }
            else
                InsertText.ForeColor = Color.Red;
        }

        private void onKeyDownInsertText(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int index;
                bool candone;
                if (InsertText.Text.Contains("0x"))
                {
                    string convert = InsertText.Text.Replace("0x", "");
                    candone = int.TryParse(convert, System.Globalization.NumberStyles.HexNumber, null, out index);
                }
                else
                    candone = int.TryParse(InsertText.Text, System.Globalization.NumberStyles.Integer, null, out index);
                if (index > 0x3FFF)
                    candone = false;
                if (candone)
                {
                    if (Art.IsValidStatic(index))
                        return;
                    contextMenuStrip1.Close();
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Multiselect = false;
                    dialog.Title = String.Format("Choose image file to insert at 0x{0:X}", index);
                    dialog.CheckFileExists = true;
                    dialog.Filter = "image file (*.tiff)|*.tiff";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Bitmap bmp = new Bitmap(dialog.FileName);
                        Art.ReplaceStatic(index, bmp);
                        if (ShowFreeSlots)
                        {
                            selected = index;
                            vScrollBar.Value = index / refMarker.col + 1;
                            namelabel.Text = String.Format("Name: {0}", TileData.ItemTable[selected].Name);
                            graphiclabel.Text = String.Format("Graphic: 0x{0:X4} ({0})", selected);
                            UpdateDetail(selected);
                            PaintBox();
                        }
                        else
                        {
                            for (int i = 0; i < ItemList.Count; i++)
                            {
                                if (index < (int)ItemList[i])
                                {
                                    selected = index;
                                    ItemList.Insert(i, (object)index);
                                    vScrollBar.Value = i / refMarker.col + 1;
                                    namelabel.Text = String.Format("Name: {0}", TileData.ItemTable[selected].Name);
                                    graphiclabel.Text = String.Format("Graphic: 0x{0:X4} ({0})", selected);
                                    UpdateDetail(selected);
                                    PaintBox();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void onClickSave(object sender, EventArgs e)
        {
            DialogResult result =
                        MessageBox.Show("Are you sure? Will take a while", "Save",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                Art.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
                this.Cursor = Cursors.Default;
                MessageBox.Show(
                    String.Format("Saved to {0}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), 
                    "Save", 
                    MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);
            }
        }

        private void onClickShowFreeSlots(object sender, EventArgs e)
        {
            ShowFreeSlots = !ShowFreeSlots;
            if (ShowFreeSlots)
            {
                for (int j = 0; j < 0x4000; j++)
                {
                    if (ItemList.Count > j)
                    {
                        if ((int)ItemList[j] != j)
                            ItemList.Insert(j, (object)j);
                    }
                    else
                        ItemList.Insert(j, (object)j);
                }
                vScrollBar.Maximum = ItemList.Count / col + 1;
                PaintBox();
            }
            else
            {
                Reload();
            }
        }

        #region Preloader
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
        #endregion
    }
}
