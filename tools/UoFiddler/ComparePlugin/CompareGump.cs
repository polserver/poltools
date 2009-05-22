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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using FiddlerControls;
using Ultima;


namespace ComparePlugin
{
    public partial class CompareGump : UserControl
    {
        public CompareGump()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }
        Hashtable m_Compare = new Hashtable();
        SHA256Managed shaM = new SHA256Managed();

        private bool Loaded = false;

        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Options.LoadedUltimaClass["Gumps"] = true;

            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            for (int i = 0; i < 0x10000; i++)
            {
                listBox1.Items.Add(i);
            }

            listBox1.EndUpdate();
            listBox2.Items.Clear();
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
            if (!Loaded)
                FiddlerControls.Options.FilePathChangeEvent += new FiddlerControls.Options.FilePathChangeHandler(OnFilePathChangeEvent);
            Loaded = true;
            Cursor.Current = Cursors.Default;
        }

        private void OnFilePathChangeEvent()
        {
            Reload();
        }

        private void Reload()
        {
            if (Loaded)
                OnLoad(EventArgs.Empty);
        }

        private void listbox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (e.Index < 0)
                return;
            Brush fontBrush = Brushes.Gray;

            int i = (int)listBox.Items[e.Index];

            if (listBox.SelectedIndex == e.Index)
                e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            bool valid;
            if ((int)listBox.Tag == 1)
                valid = Gumps.IsValidIndex(i);
            else
                valid = SecondGump.IsValidIndex(i);

            if (valid)
            {
                Bitmap bmp;
                if ((int)listBox.Tag == 1)
                    bmp = Gumps.GetGump(i);
                else
                    bmp = SecondGump.GetGump(i);

                if (bmp != null)
                {
                    if (listBox2.Items.Count > 0)
                    {
                        if (!Compare(i))
                            fontBrush = Brushes.Blue;
                    }
                    int width = bmp.Width > 80 ? 80 : bmp.Width;
                    int height = bmp.Height > 54 ? 54 : bmp.Height;

                    e.Graphics.DrawImage(bmp, new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 3, width, height));
                }
                else
                    fontBrush = Brushes.Red;
            }
            else
                fontBrush = Brushes.Red;

            e.Graphics.DrawString(String.Format("0x{0:X}", i), Font, fontBrush,
                new PointF((float)85,
                e.Bounds.Y + ((e.Bounds.Height / 2) -
                (e.Graphics.MeasureString(String.Format("0x{0:X}", i), Font).Height / 2))));
        }

        private void listbox_measureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 60;
        }

        private void listbox_SelectedChange(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedIndex == -1)
                return;

            int i = (int)listBox.Items[listBox.SelectedIndex];
            bool valid;
            if ((int)listBox.Tag == 1)
            {
                valid = Gumps.IsValidIndex(i);
                if (listBox2.Items.Count > 0)
                    listBox2.SelectedIndex = listBox2.Items.IndexOf(i);
            }
            else
            {
                valid = SecondGump.IsValidIndex(i);
                listBox1.SelectedIndex = listBox1.Items.IndexOf(i);
            }
            if (valid)
            {
                Bitmap bmp;
                if ((int)listBox.Tag == 1)
                    bmp = Gumps.GetGump(i);
                else
                    bmp = SecondGump.GetGump(i);

                if (bmp != null)
                {
                    if ((int)listBox.Tag == 1)
                        pictureBox1.BackgroundImage = bmp;
                    else
                        pictureBox2.BackgroundImage = bmp;
                }
                else
                {
                    if ((int)listBox.Tag == 1)
                        pictureBox1.BackgroundImage = null;
                    else
                        pictureBox2.BackgroundImage = null;
                }
            }
            else
            {
                if ((int)listBox.Tag == 1)
                    pictureBox1.BackgroundImage = null;
                else
                    pictureBox2.BackgroundImage = null;
            }
            listBox.Refresh();
        }

        private void Browse_OnClick(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select directory containing the gump files";
                dialog.ShowNewFolderButton = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                    textBoxSecondDir.Text = dialog.SelectedPath;
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            if (textBoxSecondDir.Text == null)
                return;
            string path = textBoxSecondDir.Text;
            string file = Path.Combine(path, "gumpart.mul");
            string file2 = Path.Combine(path, "gumpidx.mul");
            if ((File.Exists(file)) && (File.Exists(file2)))
            {
                SecondGump.SetFileIndex(file2, file);
                LoadSecond();
            }
        }

        private void LoadSecond()
        {
            listBox2.Items.Clear();
            listBox2.BeginUpdate();
            for (int i = 0; i < 0x10000; i++)
            {
                listBox2.Items.Add(i);
            }
            listBox2.EndUpdate();
            listBox1.Invalidate();
        }

        private bool Compare(int index)
        {
            if (m_Compare.Contains(index))
                return (bool)m_Compare[index];
            int width1, height1;
            int width2, height2;
            byte[] org = Gumps.GetRawGump(index, out width1, out height1);
            byte[] sec = SecondGump.GetRawGump(index,out width2,out height2);
            bool res = false;

            if ((org == null) && (sec == null))
                res = true;
            else if (((org == null) || (sec == null))
                || (org.Length != sec.Length))
                res = false;
            else if ((width1 != width2) || (height1 != height2))
                res = false;
            else
            {
                string hash1string = BitConverter.ToString(shaM.ComputeHash(org));
                string hash2string = BitConverter.ToString(shaM.ComputeHash(sec));
                if (hash1string == hash2string)
                    res = true;
            }
            m_Compare[index] = res;
            return res;
        }

        private void ShowDiff_OnClick(object sender, EventArgs e)
        {
            if (m_Compare.Count < 1)
            {
                if (checkBox1.Checked)
                {
                    MessageBox.Show("Second Gump file is not loaded!");
                    checkBox1.Checked = false;
                }
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            listBox1.BeginUpdate();
            listBox2.BeginUpdate();
            if (checkBox1.Checked)
            {
                for (int i = 0; i < 0x10000; i++)
                {
                    if (Compare(i))
                    {
                        listBox1.Items.Remove(i);
                        listBox2.Items.Remove(i);
                    }
                }
            }
            else
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                for (int i = 0; i < 0x10000; i++)
                {
                    listBox1.Items.Add(i);
                    listBox2.Items.Add(i);
                }
            }
            listBox1.EndUpdate();
            listBox2.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        private void Export_Bmp(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
                return;
            int i = int.Parse(listBox2.Items[listBox2.SelectedIndex].ToString());
            if (!SecondGump.IsValidIndex(i))
                return;
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, String.Format("Gump(Sec) 0x{0:X}.bmp", i));
            SecondGump.GetGump(i).Save(FileName, ImageFormat.Bmp);
            MessageBox.Show(
                String.Format("Gump saved to {0}", FileName),
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void Export_Tiff(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
                return;
            int i = int.Parse(listBox2.Items[listBox2.SelectedIndex].ToString());
            if (!SecondGump.IsValidIndex(i))
                return;
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, String.Format("Gump(Sec) 0x{0:X}.tiff", i));
            SecondGump.GetGump(i).Save(FileName, ImageFormat.Tiff);
            MessageBox.Show(
                String.Format("Gump saved to {0}", FileName),
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }
    }
}
