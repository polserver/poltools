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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Host;
using Ultima;

namespace FiddlerControls
{
    public partial class ItemShow : UserControl
    {
        public ItemShow()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            refMarker = this;
            if (!Files.CacheData)
                PreloadItems.Visible = false;
            ProgressBar.Visible = false;
        }

        private static ItemShow refMarker = null;
        private bool Loaded = false;
        private bool ShowFreeSlots = false;

        public static ItemShow RefMarker { get { return refMarker; } }
        public static ListView ItemListView { get { return refMarker.listView1; } }

        /// <summary>
        /// Updates if TileSize is changed
        /// </summary>
        public void ChangeTileSize()
        {
            listView1.TileSize = new Size(Options.ArtItemSizeWidth, Options.ArtItemSizeHeight);
            listView1.View = View.Details; // that works faszinating
            listView1.View = View.Tile;
        }

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
                foreach (ListViewItem item in listView1.Items)
                {
                    bin.Write((int)item.Tag);
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

        /// <summary>
        /// Searches for name and selects
        /// </summary>
        /// <param name="name"></param>
        /// <param name="next">starting from current selected</param>
        /// <returns></returns>
        public static bool SearchName(string name, bool next)
        {
            int index = 0;
            if (next)
            {
                if (refMarker.listView1.SelectedIndices.Count == 1)
                    index = refMarker.listView1.SelectedIndices[0] + 1;
                if (index >= refMarker.listView1.Items.Count)
                    index = 0;
            }

            Regex regex = new Regex(@name, RegexOptions.IgnoreCase);
            for (int i = index; i < refMarker.listView1.Items.Count; i++)
            {
                ListViewItem item = refMarker.listView1.Items[i];
                if (regex.IsMatch(TileData.ItemTable[(int)item.Tag].Name))
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

        /// <summary>
        /// ReLoads if loaded
        /// </summary>
        private void Reload()
        {
            if (Loaded)
                OnLoad(this, EventArgs.Empty);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            Options.LoadedUltimaClass["TileData"] = true;
            Options.LoadedUltimaClass["Art"] = true;
            Options.LoadedUltimaClass["Animdata"] = true;
            Options.LoadedUltimaClass["Hues"] = true;
            if (!Loaded) // only once
            {
                PluginInterface.Events.FireModifyItemShowContextMenuEvent(this.contextMenuStrip1);
            }
            
            ShowFreeSlots = false;
            showFreeSlotsToolStripMenuItem.Checked = false;
            listView1.BeginUpdate();
            listView1.Clear();

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
                            ListViewItem item = new ListViewItem(i.ToString(), 0);
                            item.Tag = i;
                            listView1.Items.Add(item);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 0x4000; i++)
                {
                    if (Art.IsValidStatic(i))
                    {
                        ListViewItem item = new ListViewItem(i.ToString(), 0);
                        item.Tag = i;
                        listView1.Items.Add(item);
                    }
                }
                if (Files.UseHashFile)
                    MakeHashFile();
            }

            listView1.TileSize = new Size(Options.ArtItemSizeWidth, Options.ArtItemSizeHeight);
            listView1.EndUpdate();

            if (!Loaded)
                FiddlerControls.Options.FilePathChangeEvent += new FiddlerControls.Options.FilePathChangeHandler(OnFilePathChangeEvent);
            Loaded = true;

            this.Cursor = Cursors.Default;
        }

        private void OnFilePathChangeEvent()
        {
            if (!FiddlerControls.Options.DesignAlternative)
                Reload();
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int i = (int)listView1.SelectedItems[0].Tag;
                if (i == -1)
                {
                    namelabel.Text = "Name: FREE";
                    graphiclabel.Text = String.Format("Graphic: 0x{0:X4} ({0})", listView1.SelectedIndices[0]);
                    UpdateDetail(listView1.SelectedIndices[0]);
                }
                else
                {
                    namelabel.Text = String.Format("Name: {0}", TileData.ItemTable[i].Name);
                    graphiclabel.Text = String.Format("Graphic: 0x{0:X4} ({0})", i);
                    UpdateDetail(i);
                }
            }
        }

        private void drawitem(object sender, DrawListViewItemEventArgs e)
        {
            int i = (int)e.Item.Tag;
            if (i == -1)
            {
                if (listView1.SelectedItems.Contains(e.Item))
                    e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else
                    e.Graphics.DrawRectangle(Pens.Gray, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds.X + 5, e.Bounds.Y + 5, e.Bounds.Width - 10, e.Bounds.Height - 10);
                return;
            }
            bool patched;
            Bitmap bmp = Art.GetStatic(i, out patched);

            if (bmp != null)
            {
                if (listView1.SelectedItems.Contains(e.Item))
                    e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else if (patched)
                    e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else
                    e.Graphics.FillRectangle(Brushes.White, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

                if (Options.ArtItemClip)
                {
                    e.Graphics.DrawImage(bmp, e.Bounds.X + 1, e.Bounds.Y + 1,
                                         new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1),
                                         GraphicsUnit.Pixel);
                }
                else
                {
                    int width = bmp.Width;
                    int height = bmp.Height;
                    if (width > e.Bounds.Width)
                    {
                        width = e.Bounds.Width;
                        height = e.Bounds.Height * bmp.Height / bmp.Width;
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
                    e.Graphics.DrawRectangle(Pens.Gray, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }
        }

        public void listView_DoubleClicked(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ItemDetail f;
                if ((int)listView1.SelectedItems[0].Tag == -1)
                    f = new ItemDetail(listView1.SelectedIndices[0]);
                else
                    f = new ItemDetail((int)listView1.SelectedItems[0].Tag);
                f.TopMost = true;
                f.Show();
            }
        }

        private ItemSearch showform = null;
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

        private void OnResize(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                listView1.SelectedItems[0].EnsureVisible();
                int i = (int)listView1.SelectedItems[0].Tag;
                if (i == -1)
                    UpdateDetail(listView1.SelectedItems[0].Index);
                else
                    UpdateDetail(i);
            }
        }

        private void OnClickSave(object sender, EventArgs e)
        {
            DialogResult result =
                        MessageBox.Show("Are you sure? Will take a while",
                        "Save",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                Art.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
                this.Cursor = Cursors.Default;
                Options.ChangedUltimaClass["Art"] = false;
                MessageBox.Show(String.Format("Saved to {0}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase),
                    "Save",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
            }
        }

        private void OnClickReplace(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = false;
                    dialog.Title = "Choose image file to replace";
                    dialog.CheckFileExists = true;
                    dialog.Filter = "image files (*.tiff;*.bmp)|*.tiff;*.bmp";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Bitmap bmp = new Bitmap(dialog.FileName);
                        if (dialog.FileName.Contains(".bmp"))
                            bmp = Utils.ConvertBmp(bmp);
                        int id = (int)listView1.SelectedItems[0].Tag;
                        if (id == -1)
                            listView1.SelectedItems[0].Tag = id = listView1.SelectedItems[0].Index;
                        Art.ReplaceStatic(id, bmp);
                        listView1.Invalidate();
                        UpdateDetail(id);
                        Options.ChangedUltimaClass["Art"] = true;
                    }
                }
            }
        }

        private void onTextChanged_Insert(object sender, EventArgs e)
        {
            int index;
            if (Utils.ConvertStringToInt(InsertText.Text, out index, 0, 0x3FFF))
            {
                if (Art.IsValidStatic(index))
                    InsertText.ForeColor = Color.Red;
                else
                    InsertText.ForeColor = Color.Black;
            }
            else
                InsertText.ForeColor = Color.Red;
        }

        private void onKeyDown_Insert(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int index;
                if (Utils.ConvertStringToInt(InsertText.Text, out index, 0, 0x3FFF))
                {
                    if (Art.IsValidStatic(index))
                        return;
                    contextMenuStrip1.Close();
                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        dialog.Multiselect = false;
                        dialog.Title = String.Format("Choose image file to insert at 0x{0:X}", index);
                        dialog.CheckFileExists = true;
                        dialog.Filter = "image files (*.tiff;*.bmp)|*.tiff;*.bmp";
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            Bitmap bmp = new Bitmap(dialog.FileName);
                            if (dialog.FileName.Contains(".bmp"))
                                bmp = Utils.ConvertBmp(bmp);
                            Art.ReplaceStatic(index, bmp);
                            ListViewItem item = new ListViewItem(index.ToString(), 0);
                            item.Tag = index;
                            if (ShowFreeSlots)
                            {
                                listView1.Items[index] = item;
                                listView1.Invalidate();
                            }
                            else
                            {
                                bool done = false;
                                foreach (ListViewItem i in listView1.Items)
                                {
                                    if ((int)i.Tag > index)
                                    {
                                        listView1.Items.Insert(i.Index, item);
                                        done = true;
                                        break;
                                    }
                                }
                                if (!done)
                                    listView1.Items.Add(item);
                            }
                            listView1.View = View.Details; // that works faszinating
                            listView1.View = View.Tile;
                            if (listView1.SelectedItems.Count == 1)
                                listView1.SelectedItems[0].Selected = false;
                            item.Selected = true;
                            item.Focused = true;
                            item.EnsureVisible();
                            Options.ChangedUltimaClass["Art"] = true;
                        }
                    }
                }
            }
        }

        private void onClickRemove(object sender, EventArgs e)
        {
            int i = (int)listView1.SelectedItems[0].Tag;
            if (i == -1)
                return;
            DialogResult result =
                        MessageBox.Show(String.Format("Are you sure to remove 0x{0:X}", i),
                        "Save",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                Art.RemoveStatic(i);
                i = listView1.SelectedItems[0].Index;
                if (!ShowFreeSlots)
                {
                    listView1.SelectedItems[0].Selected = false;
                    listView1.Items.RemoveAt(i);
                }
                else
                    listView1.Items[i].Tag = -1;
                listView1.Invalidate();
                Options.ChangedUltimaClass["Art"] = true;
            }
        }

        private void onClickFindFree(object sender, EventArgs e)
        {
            if (ShowFreeSlots)
            {
                int i;
                if (listView1.SelectedItems.Count > 0)
                    i = listView1.SelectedItems[0].Index + 1;
                else
                    i = 0;
                for (; i < listView1.Items.Count; i++)
                {
                    if ((int)listView1.Items[i].Tag == -1)
                    {
                        ListViewItem item = listView1.Items[i];
                        if (listView1.SelectedItems.Count == 1)
                            listView1.SelectedItems[0].Selected = false;
                        item.Selected = true;
                        item.Focused = true;
                        item.EnsureVisible();
                        break;
                    }
                }
            }
            else
            {
                int id, i;
                if (listView1.SelectedItems.Count > 0)
                {
                    id = (int)listView1.SelectedItems[0].Tag + 1;
                    i = listView1.SelectedItems[0].Index + 1;
                }
                else
                {
                    id = 0;
                    i = 0;
                }
                for (; i < listView1.Items.Count; i++)
                {
                    if (id < (int)listView1.Items[i].Tag)
                    {
                        ListViewItem item = listView1.Items[i];
                        if (listView1.SelectedItems.Count == 1)
                            listView1.SelectedItems[0].Selected = false;
                        item.Selected = true;
                        item.Focused = true;
                        item.EnsureVisible();
                        break;
                    }
                    id++;
                }
            }
        }

        private void onClickShowFreeSlots(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            ShowFreeSlots = !ShowFreeSlots;
            if (ShowFreeSlots)
            {
                ListViewItem item;
                for (int j = 0; j < 0x4000; j++)
                {
                    if (listView1.Items.Count > j)
                    {
                        if ((int)listView1.Items[j].Tag != j)
                        {
                            item = new ListViewItem(j.ToString(), 0);
                            item.Tag = -1;
                            listView1.Items.Insert(j, item);
                        }
                    }
                    else
                    {
                        item = new ListViewItem(j.ToString(), 0);
                        item.Tag = -1;
                        listView1.Items.Insert(j, item);
                    }
                }
            }
            else
            {
                Reload();
            }
            listView1.EndUpdate();
            listView1.View = View.Details; // that works faszinating
            listView1.View = View.Tile;
        }

        private void extract_Image_ClickBmp(object sender, EventArgs e)
        {
            int i = (int)listView1.SelectedItems[0].Tag;
            if (i == -1)
                return;
            if (!Art.IsValidStatic(i))
                return;
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, String.Format("Item 0x{0:X}.bmp", i));
            Bitmap bit = new Bitmap(Ultima.Art.GetStatic(i));
            bit.Save(FileName, ImageFormat.Bmp);
            bit.Dispose();
            MessageBox.Show(
                String.Format("Item saved to {0}", FileName),
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void extract_Image_ClickTiff(object sender, EventArgs e)
        {
            int i = (int)listView1.SelectedItems[0].Tag;
            if (i == -1)
                return;
            if (!Art.IsValidStatic(i))
                return;
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, String.Format("Item 0x{0:X}.tiff", i));
            Bitmap bit = new Bitmap(Ultima.Art.GetStatic(i));
            bit.Save(FileName, ImageFormat.Tiff);
            bit.Dispose();
            MessageBox.Show(
                String.Format("Item saved to {0}", FileName),
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void OnClickSelectTiledata(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int id = (int)listView1.SelectedItems[0].Tag;
                if (id == -1)
                    id = listView1.SelectedItems[0].Index;
                FiddlerControls.TileDatas.Select(id, false);
            }
        }

        private void OnClickSelectRadarCol(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int id = (int)listView1.SelectedItems[0].Tag;
                if (id == -1)
                    id = listView1.SelectedItems[0].Index;
                FiddlerControls.RadarColor.Select(id, false);
            }
        }

        private void OnClick_SaveAllBmp(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select directory";
                dialog.ShowNewFolderButton = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        int index = (int)listView1.Items[i].Tag;
                        if (index >= 0)
                        {
                            string FileName = Path.Combine(dialog.SelectedPath, String.Format("Item 0x{0:X}.bmp", index));
                            Bitmap bit = new Bitmap(Ultima.Art.GetStatic(index));
                            if (bit != null)
                                bit.Save(FileName, ImageFormat.Bmp);
                            bit.Dispose();
                        }
                    }
                    MessageBox.Show(String.Format("All Item saved to {0}", dialog.SelectedPath), "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void OnClick_SaveAllTiff(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select directory";
                dialog.ShowNewFolderButton = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        int index = (int)listView1.Items[i].Tag;
                        if (index >= 0)
                        {
                            string FileName = Path.Combine(dialog.SelectedPath, String.Format("Item 0x{0:X}.tiff", index));
                            Bitmap bit = new Bitmap(Ultima.Art.GetStatic(index));
                            if (bit != null)
                                bit.Save(FileName, ImageFormat.Tiff);
                            bit.Dispose();
                        }
                    }
                    MessageBox.Show(String.Format("All Item saved to {0}", dialog.SelectedPath), "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
        }

        #region Preloader
        private void OnClickPreload(object sender, EventArgs e)
        {
            if (PreLoader.IsBusy)
                return;
            ProgressBar.Minimum = 1;
            ProgressBar.Maximum = listView1.Items.Count;
            ProgressBar.Step = 1;
            ProgressBar.Value = 1;
            ProgressBar.Visible = true;
            PreLoader.RunWorkerAsync(ProgressBar);
        }

        private void PreLoaderDoWork(object sender, DoWorkEventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                Art.GetStatic((int)item.Tag);
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
