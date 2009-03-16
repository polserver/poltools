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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Ultima;

namespace FiddlerControls
{
    public partial class LandTiles : UserControl
    {
        public LandTiles()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            refMarker = this;
        }

        private static LandTiles refMarker = null;
        private bool Loaded = false;

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

            for (int i = index; i < refMarker.listView1.Items.Count; i++)
            {
                ListViewItem item = refMarker.listView1.Items[i];
                if (TileData.LandTable[(int)item.Tag].Name.Contains(name))
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
        public void Reload()
        {
            if (Loaded)
                OnLoad(this, EventArgs.Empty);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            Options.LoadedUltimaClass["TileData"] = true;
            Options.LoadedUltimaClass["Art"] = true;
            Loaded = true;
            listView1.BeginUpdate();
            listView1.Clear();

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
            this.Cursor = Cursors.Default;
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int i = (int)listView1.SelectedItems[0].Tag;
                namelabel.Text = String.Format("Name: {0}", TileData.LandTable[i].Name);
                graphiclabel.Text = String.Format("ID: 0x{0:X4} ({0})", i);
                FlagsLabel.Text = String.Format("Flags: {0}", TileData.LandTable[i].Flags);
            }
        }

        private void drawitem(object sender, DrawListViewItemEventArgs e)
        {
            int i = (int)listView1.Items[e.ItemIndex].Tag;

            bool patched;
            Bitmap bmp = Art.GetLand(i, out patched);

            if (bmp != null)
            {
                if (listView1.SelectedItems.Contains(e.Item))
                    e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else if (patched)
                    e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else
                    e.Graphics.FillRectangle(Brushes.White, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

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

        private LandTileSearch showform = null;
        private void OnClickSearch(object sender, EventArgs e)
        {
            if ((showform == null) || (showform.IsDisposed))
            {
                showform = new LandTileSearch();
                showform.TopMost = true;
                showform.Show();
            }
        }

        private void OnClickReplace(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Title = "Choose image file to replace";
                dialog.CheckFileExists = true;
                dialog.Filter = "image files (*.tiff;*.bmp)|*.tiff;*.bmp";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(dialog.FileName);
                    if ((bmp.Height != 44) || (bmp.Width != 44))
                    {
                        MessageBox.Show("Height or Width Invalid", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    if (dialog.FileName.Contains(".bmp"))
                        bmp = Utils.ConvertBmp(bmp);
                    Art.ReplaceLand((int)listView1.SelectedItems[0].Tag, bmp);
                    listView1.Invalidate();
                    Options.ChangedUltimaClass["Art"] = true;
                }
            }
        }

        private void OnClickSave(object sender, EventArgs e)
        {
            DialogResult result =
                        MessageBox.Show("Are you sure? Will take a while", "Save",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                Art.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
                this.Cursor = Cursors.Default;
                MessageBox.Show(
                    String.Format("Saved to {0}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase),
                    "Save",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                Options.ChangedUltimaClass["Art"] = false;
            }
        }

        private void onTextChanged_Insert(object sender, EventArgs e)
        {
            int index;
            if (Utils.ConvertStringToInt(InsertText.Text, out index, 0, 0x3FFF))
            {
                if (Art.IsValidLand(index))
                    InsertText.ForeColor = Color.Red;
                else
                    InsertText.ForeColor = Color.Black;
            }
            else
                InsertText.ForeColor = Color.Red;
        }

        private void OnKeyDown_Insert(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int index;
                if (Utils.ConvertStringToInt(InsertText.Text, out index, 0, 0x3FFF))
                {
                    if (Art.IsValidLand(index))
                        return;
                    contextMenuStrip1.Close();
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Multiselect = false;
                    dialog.Title = String.Format("Choose image file to insert at 0x{0:X}", index);
                    dialog.CheckFileExists = true;
                    dialog.Filter = "image files (*.tiff;*.bmp)|*.tiff;*.bmp";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Bitmap bmp = new Bitmap(dialog.FileName);
                        if ((bmp.Height != 44) || (bmp.Width != 44))
                        {
                            MessageBox.Show("Height or Width Invalid", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return;
                        }
                        if (dialog.FileName.Contains(".bmp"))
                            bmp = Utils.ConvertBmp(bmp);
                        Art.ReplaceLand(index, bmp);
                        Options.ChangedUltimaClass["Art"] = true;
                        ListViewItem item = new ListViewItem(index.ToString(), 0);
                        item.Tag = index;
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
                        listView1.View = View.Details; // that works faszinating
                        listView1.View = View.Tile;
                        if (listView1.SelectedItems.Count == 1)
                            listView1.SelectedItems[0].Selected = false;
                        item.Selected = true;
                        item.Focused = true;
                        item.EnsureVisible();
                    }
                }
            }
        }

        private void onClickRemove(object sender, EventArgs e)
        {
            int i = (int)listView1.SelectedItems[0].Tag;
            DialogResult result =
                        MessageBox.Show(String.Format("Are you sure to remove {0}", i), "Save",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                Art.RemoveLand(i);
                i = listView1.SelectedItems[0].Index;
                listView1.SelectedItems[0].Selected = false;
                listView1.Items.RemoveAt(i);
                listView1.Invalidate();
                Options.ChangedUltimaClass["Art"] = true;
            }
        }

        private void onClickFindFree(object sender, EventArgs e)
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

        private void onClickExportBmp(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                int i = (int)listView1.SelectedItems[0].Tag;
                string FileName = Path.Combine(path, String.Format("Landtile {0}.bmp", i));
                Bitmap bmp = Art.GetLand(i);
                bmp.Save(FileName, ImageFormat.Bmp);
                MessageBox.Show(String.Format("Landtile saved to {0}", FileName), "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void onClickExportTiff(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                int i = (int)listView1.SelectedItems[0].Tag;
                string FileName = Path.Combine(path, String.Format("Landtile {0}.tiff", i));
                Bitmap bmp = Art.GetLand(i);
                bmp.Save(FileName, ImageFormat.Tiff);
                MessageBox.Show(String.Format("Landtile saved to {0}", FileName), "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void OnClickSelectTiledata(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
                FiddlerControls.TileDatas.Select((int)listView1.SelectedItems[0].Tag, true);
        }
    }
}
