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
using System.Text;
using System.Windows.Forms;

namespace FiddlerControls
{
    public partial class MultiMap : UserControl
    {
        public MultiMap()
        {
            InitializeComponent();
            hScrollBar.Visible = false;
            vScrollBar.Visible = false;
        }

        bool moving = false;
        Point movingpoint;

        private bool Loaded = false;

        /// <summary>
        /// ReLoads if loaded
        /// </summary>
        public void Reload()
        {
            if (!Loaded)
                return;
            buttonGenerate.Visible = true;
            buttonLoad.Visible = true;
            hScrollBar.Visible = false;
            vScrollBar.Visible = false;
            pictureBox.Image = null;
            moving = false;
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                DisplayScrollBars();
                SetScrollBarValues();
                Refresh();
            }
        }

        private void HandleScroll(object sender, ScrollEventArgs e)
        {
            Graphics g = pictureBox.CreateGraphics();

            g.DrawImage(pictureBox.Image,
              new Rectangle(0, 0, pictureBox.Right - vScrollBar.Width,
              pictureBox.Bottom - hScrollBar.Height),
              new Rectangle(hScrollBar.Value, vScrollBar.Value,
              pictureBox.Right - vScrollBar.Width,
              pictureBox.Bottom - hScrollBar.Height),
              GraphicsUnit.Pixel);

            pictureBox.Update();
        }

        private void DisplayScrollBars()
        {
            if (pictureBox.Width > pictureBox.Image.Width - vScrollBar.Width)
                hScrollBar.Visible = false;
            else
                hScrollBar.Visible = true;

            if (pictureBox.Height >
                pictureBox.Image.Height - hScrollBar.Height)
                vScrollBar.Visible = false;
            else
                vScrollBar.Visible = true;
        }

        private void SetScrollBarValues()
        {
            vScrollBar.Minimum = 0;
            hScrollBar.Minimum = 0;

            if ((pictureBox.Image.Size.Width - pictureBox.ClientSize.Width) > 0)
                hScrollBar.Maximum = pictureBox.Image.Size.Width - pictureBox.ClientSize.Width;

            if (vScrollBar.Visible)
                hScrollBar.Maximum += vScrollBar.Width;

            hScrollBar.LargeChange = hScrollBar.Maximum / 10;
            hScrollBar.SmallChange = hScrollBar.Maximum / 20;

            hScrollBar.Maximum += hScrollBar.LargeChange;

            if ((pictureBox.Image.Size.Height - pictureBox.ClientSize.Height) > 0)
                vScrollBar.Maximum = pictureBox.Image.Size.Height - pictureBox.ClientSize.Height;

            if (hScrollBar.Visible)
                vScrollBar.Maximum += hScrollBar.Height;

            vScrollBar.LargeChange = vScrollBar.Maximum / 10;
            vScrollBar.SmallChange = vScrollBar.Maximum / 20;

            vScrollBar.Maximum += vScrollBar.LargeChange;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                moving = true;
                movingpoint.X = e.X;
                movingpoint.Y = e.Y;
                this.Cursor = Cursors.Hand;
            }
            else
            {
                moving = false;
                this.Cursor = Cursors.Default;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (moving)
            {
                if (pictureBox.Image != null)
                {
                    int deltax = (int)(-1 * (e.X - movingpoint.X));
                    int deltay = (int)(-1 * (e.Y - movingpoint.Y));
                    movingpoint.X = e.X;
                    movingpoint.Y = e.Y;
                    hScrollBar.Value = Math.Max(0, Math.Min(hScrollBar.Maximum, hScrollBar.Value + deltax));
                    vScrollBar.Value = Math.Max(0, Math.Min(vScrollBar.Maximum, vScrollBar.Value + deltay));

                    Graphics g = pictureBox.CreateGraphics();

                    g.DrawImage(pictureBox.Image,
                      new Rectangle(0, 0, pictureBox.Right - vScrollBar.Width,
                      pictureBox.Bottom - hScrollBar.Height),
                      new Rectangle(hScrollBar.Value, vScrollBar.Value,
                      pictureBox.Right - vScrollBar.Width,
                      pictureBox.Bottom - hScrollBar.Height),
                      GraphicsUnit.Pixel);

                    pictureBox.Update();
                }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
            this.Cursor = Cursors.Default;
        }

        private void OnClickLoad(object sender, EventArgs e)
        {
            Loaded = true;
            this.Cursor = Cursors.WaitCursor;
            buttonGenerate.Visible = false;
            buttonLoad.Visible = false;
            pictureBox.Image = Ultima.MultiMap.GetMultiMap();
            if (pictureBox.Image != null)
            {
                DisplayScrollBars();
                SetScrollBarValues();
            }
            toolTip1.SetToolTip(pictureBox, "");
            this.Cursor = Cursors.Default;
        }

        private void OnClickRLE(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select Image to convert";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap image = new Bitmap(dialog.FileName);
                if (image != null)
                {
                    if ((image.Height != 2048) || (image.Width != 2560))
                    {
                        MessageBox.Show("Invalid image height or width", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    this.Cursor = Cursors.WaitCursor;
                    string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    string FileName = Path.Combine(path, "MultiMap.rle");
                    using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
                    {
                        BinaryWriter bin = new BinaryWriter(fs, Encoding.Unicode);
                        Ultima.MultiMap.SaveMultiMap(image, bin);
                    }
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(String.Format("MultiMap saved to {0}", FileName), "Convert",
                        MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else
                    MessageBox.Show("No image found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
            }
        }

        private void OnClickExportBmp(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, "MultiMap.bmp");
            pictureBox.Image.Save(FileName, ImageFormat.Bmp);
            MessageBox.Show(String.Format("MultiMap saved to {0}", FileName), "Export",
                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void OnClickExportTiff(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, "MultiMap.tiff");
            pictureBox.Image.Save(FileName, ImageFormat.Tiff);
            MessageBox.Show(String.Format("MultiMap saved to {0}", FileName), "Export",
                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
    }
}
