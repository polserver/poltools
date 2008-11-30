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
using System.Xml;
using System.IO;
using System.Drawing.Imaging;

namespace Controls
{
    public partial class Gump : UserControl
    {
        public Gump()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            if (!FileIndex.CacheData)
                Preload.Visible = false;
            ProgressBar.Visible = false;
        }

        private bool Loaded = false;
        public void Reload()
        {
            if (Loaded)
                OnLoad(EventArgs.Empty);
        }
        protected override void OnLoad(EventArgs e)
        {
            Loaded = true;
            listBox.BeginUpdate();
            listBox.Items.Clear();
            for (int i = 0; i < 0xFFFF; i++)
            {
                if (Gumps.IsValidIndex(i))
                    listBox.Items.Add(i);
            }
            listBox.EndUpdate();
            pictureBox.BackgroundImage = Gumps.GetGump(0);
            listBox.SelectedIndex = 0;
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            Brush fontBrush = Brushes.Gray;

            int i = int.Parse(listBox.Items[e.Index].ToString());
            if (Gumps.IsValidIndex(i))
            {
                Bitmap bmp = Gumps.GetGump(i);
                
                //index 21696 is valid index, but no valid bitmap data?
                if (bmp != null)
                {
                    int width = bmp.Width;
                    int height = bmp.Height;

                    if (width > 100)
                        width = 100;

                    if (height > 54)
                        height = 54;

                    if (listBox.SelectedIndex == e.Index)
                        e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds.X, e.Bounds.Y, 105, 60);

                    e.Graphics.DrawImage(bmp,new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 3, width, height));
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

        private void extract_Image_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            int i = int.Parse(listBox.Items[listBox.SelectedIndex].ToString());
            string FileName = Path.Combine(path, String.Format("Gump {0}.jpg", i));
            Bitmap bmp = Gumps.GetGump(i);
            bmp.Save(FileName, ImageFormat.Jpeg);
        }

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
    }
}
