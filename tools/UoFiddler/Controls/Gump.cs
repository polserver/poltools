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
using System.Windows.Forms;
using Ultima;

namespace FiddlerControls
{
    public partial class Gump : UserControl
    {
        public Gump()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            if (!Files.CacheData)
                Preload.Visible = false;
            ProgressBar.Visible = false;
        }

        private bool Loaded = false;

        /// <summary>
        /// Reload when loaded (file changed)
        /// </summary>
        private void Reload()
        {
            if (Loaded)
                OnLoad(EventArgs.Empty);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            Options.LoadedUltimaClass["Gumps"] = true;
            
            listBox.BeginUpdate();
            listBox.Items.Clear();
            for (int i = 0; i < 0xFFFF; i++)
            {
                if (Gumps.IsValidIndex(i))
                    listBox.Items.Add(i);
            }

            listBox.EndUpdate();
            if (listBox.Items.Count > 0)
                listBox.SelectedIndex = 0;
            if (!Loaded)
                FiddlerControls.Options.FilePathChangeEvent += new FiddlerControls.Options.FilePathChangeHandler(OnFilePathChangeEvent);
            Loaded = true;
            this.Cursor = Cursors.Default;
        }

        private void OnFilePathChangeEvent()
        {
            Reload();
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            Brush fontBrush = Brushes.Gray;

            int i = int.Parse(listBox.Items[e.Index].ToString());
            if (Gumps.IsValidIndex(i))
            {
                bool patched;
                Bitmap bmp = Gumps.GetGump(i, out patched);

                if (bmp != null)
                {
                    int width = bmp.Width > 100 ? 100 : bmp.Width;
                    int height = bmp.Height > 54 ? 54 : bmp.Height;

                    if (listBox.SelectedIndex == e.Index)
                        e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds.X, e.Bounds.Y, 105, 60);
                    else if (patched)
                        e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds.X, e.Bounds.Y, 105, 60);

                    e.Graphics.DrawImage(bmp, new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 3, width, height));
                }
                else
                    fontBrush = Brushes.Red;
            }
            else
                fontBrush = Brushes.Red;

            e.Graphics.DrawString(String.Format("0x{0:X}", i), Font, fontBrush,
                new PointF((float)105,
                e.Bounds.Y + ((e.Bounds.Height / 2) -
                (e.Graphics.MeasureString(String.Format("0x{0:X}", i), Font).Height / 2))));
        }

        private void listBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 60;
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1)
                return;

            int i = int.Parse(listBox.Items[listBox.SelectedIndex].ToString());
            if (Gumps.IsValidIndex(i))
            {
                Bitmap bmp = Gumps.GetGump(i);
                if (bmp != null)
                {
                    pictureBox.BackgroundImage = bmp;
                    IDLabel.Text = String.Format("ID: 0x{0:X} ({1})", i, i);
                    SizeLabel.Text = String.Format("Size: {0},{1}", bmp.Width, bmp.Height);
                }
                else
                    pictureBox.BackgroundImage = null;
            }
            else
                pictureBox.BackgroundImage = null;
            listBox.Refresh();
        }

        private void onClickReplace(object sender, EventArgs e)
        {
            if (listBox.SelectedItems.Count == 1)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Title = "Choose image file to replace";
                dialog.CheckFileExists = true;
                dialog.Filter = "image files (*.tiff;*.bmp)|*.tiff;*.bmp";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(dialog.FileName);
                    if (dialog.FileName.Contains(".bmp"))
                        bmp = Utils.ConvertBmp(bmp);
                    int i = int.Parse(listBox.Items[listBox.SelectedIndex].ToString());
                    Gumps.ReplaceGump(i, bmp);
                    listBox.Invalidate();
                    listBox_SelectedIndexChanged(this, EventArgs.Empty);
                    Options.ChangedUltimaClass["Gumps"] = true;
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
                Gumps.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
                this.Cursor = Cursors.Default;
                MessageBox.Show(
                    String.Format("Saved to {0}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase),
                    "Save",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                Options.ChangedUltimaClass["Gumps"] = false;
            }
        }

        private void onClickRemove(object sender, EventArgs e)
        {
            int i = int.Parse(listBox.Items[listBox.SelectedIndex].ToString());
            DialogResult result =
                        MessageBox.Show(String.Format("Are you sure to remove {0}", i), "Remove",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                Gumps.RemoveGump(i);
                listBox.Items.RemoveAt(listBox.SelectedIndex);
                pictureBox.BackgroundImage = null;
                listBox.Invalidate();
                Options.ChangedUltimaClass["Gumps"] = true;
            }
        }

        private void onClickFindFree(object sender, EventArgs e)
        {
            int id = int.Parse(listBox.Items[listBox.SelectedIndex].ToString());
            id++;
            for (int i = listBox.SelectedIndex + 1; i < listBox.Items.Count; i++)
            {
                if (id < int.Parse(listBox.Items[i].ToString()))
                {
                    listBox.SelectedIndex = i;
                    break;
                }
                id++;
            }
        }

        private void onTextChanged_InsertAt(object sender, EventArgs e)
        {
            int index;
            if (Utils.ConvertStringToInt(InsertText.Text, out index, 0, 0xFFFE))
            {
                if (Gumps.IsValidIndex(index))
                    InsertText.ForeColor = Color.Red;
                else
                    InsertText.ForeColor = Color.Black;
            }
            else
                InsertText.ForeColor = Color.Red;
        }

        private void onKeydown_InsertText(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            int index;
            if (Utils.ConvertStringToInt(InsertText.Text, out index, 0, 0xFFFE))
            {
                if (Gumps.IsValidIndex(index))
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
                    if (dialog.FileName.Contains(".bmp"))
                        bmp = Utils.ConvertBmp(bmp);
                    Gumps.ReplaceGump(index, bmp);
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        int j = int.Parse(listBox.Items[i].ToString());
                        if (j > index)
                        {
                            listBox.Items.Insert(i, index);
                            listBox.SelectedIndex = i;
                            break;
                        }
                    }
                    Options.ChangedUltimaClass["Gumps"] = true;
                }
            }
        }

        private void extract_Image_ClickBmp(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            int i = int.Parse(listBox.Items[listBox.SelectedIndex].ToString());
            string FileName = Path.Combine(path, String.Format("Gump {0}.bmp", i));
            Bitmap bit = new Bitmap(Gumps.GetGump(i));
            bit.Save(FileName, ImageFormat.Bmp);
            MessageBox.Show(
                String.Format("Gump saved to {0}", FileName),
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void extract_Image_ClickTiff(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            int i = int.Parse(listBox.Items[listBox.SelectedIndex].ToString());
            string FileName = Path.Combine(path, String.Format("Gump {0}.tiff", i));
            Gumps.GetGump(i).Save(FileName, ImageFormat.Tiff);
            MessageBox.Show(
                String.Format("Gump saved to {0}", FileName),
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        #region Preloader
        private void OnClickPreload(object sender, EventArgs e)
        {
            if (PreLoader.IsBusy)
                return;
            ProgressBar.Minimum = 1;
            ProgressBar.Maximum = listBox.Items.Count;
            ProgressBar.Step = 1;
            ProgressBar.Value = 1;
            ProgressBar.Visible = true;
            PreLoader.RunWorkerAsync();
        }

        private void PreLoaderDoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                Gumps.GetGump(int.Parse(listBox.Items[i].ToString()));
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
