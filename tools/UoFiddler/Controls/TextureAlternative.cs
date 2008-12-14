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
using System.Windows.Forms;
using Ultima;

namespace Controls
{
    public partial class TextureAlternative : UserControl
    {
        public TextureAlternative()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            pictureBox.MouseWheel += new MouseEventHandler(OnMouseWheel);
            pictureBox.Image = bmp;
            refMarker = this;
        }
        private static TextureAlternative refMarker = null;
        private ArrayList TextureList = new ArrayList();
        private int col;
        private int row;
        private int selected = -1;
        private Bitmap bmp;

        private bool Loaded = false;
        public void Reload()
        {
            if (!Loaded)
                return;
            TextureList = new ArrayList();
            selected = -1;
            OnLoad(this, EventArgs.Empty);
        }

        public static bool SearchGraphic(int graphic)
        {
            int index = 0;

            for (int i = index; i < refMarker.TextureList.Count; i++)
            {
                if ((int)refMarker.TextureList[i] == graphic)
                {
                    refMarker.selected = graphic;
                    refMarker.vScrollBar.Value = i / refMarker.col + 1;
                    refMarker.Label.Text = String.Format("Graphic: 0x{0:X4} ({0}) [{1}x{1}]", graphic, Textures.GetTexture(graphic));
                    refMarker.PaintBox();
                    return true;
                }
            }
            return false;
        }

        public int GetIndex(int x, int y)
        {
            int value = Math.Max(0, ((col * (vScrollBar.Value - 1)) + (x + (y * col))));
            if (TextureList.Count > value)
                return (int)TextureList[value];
            else
                return -1;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            Loaded = true;
            for (int i = 0; i < 0x1000; i++)
            {
                if (Textures.TestTexture(i))
                    TextureList.Add((object)i);
            }
            vScrollBar.Maximum = TextureList.Count / col + 1;
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            PaintBox();
            this.Cursor = Cursors.Default;
        }

        private void OnScroll(object sender, ScrollEventArgs e)
        {
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

        private void PaintBox()
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                for (int x = 0; x <= col; x++)
                {
                    g.DrawLine(Pens.Gray, new Point(x * 64, 0),
                        new Point(x * 64, row * 64));
                }

                for (int y = 0; y <= row; y++)
                {
                    g.DrawLine(Pens.Gray, new Point(0, y * 64),
                        new Point(col * 64, y * 64));
                }

                for (int y = 0; y < row; y++)
                {
                    for (int x = 0; x < col; x++)
                    {
                        int index = GetIndex(x, y);
                        if (index >= 0)
                        {
                            Bitmap b = Textures.GetTexture(index);

                            if (b != null)
                            {
                                Point loc = new Point((x * 64) + 1, (y * 64) + 1);
                                Size size = new Size(64 - 1, 64 - 1);
                                Rectangle rect = new Rectangle(loc, size);

                                g.Clip = new Region(rect);

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
                                if (index == selected)
                                    g.DrawRectangle(Pens.LightBlue,rect.X,rect.Y,rect.Width-1,rect.Height-1);
                            }
                        }
                    }
                }
                g.Save();
            }
            pictureBox.Image = bmp;
        }

        private void OnResize(object sender, EventArgs e)
        {
            col = pictureBox.Width / 64;
            row = pictureBox.Height / 64;
            vScrollBar.Maximum = TextureList.Count / col + 1;
            vScrollBar.Minimum = 1;
            vScrollBar.SmallChange = 1;
            vScrollBar.LargeChange = row;
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            PaintBox();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point m = PointToClient(Control.MousePosition);
            int x = m.X / (64 - 1);
            int y = m.Y / (64 - 1);
            int index = GetIndex(x, y);
            if (index >= 0)
            {
                if (selected != index)
                {
                    selected = index;
                    Label.Text = String.Format("Graphic: 0x{0:X4} ({0}) [{1}x{1}]", selected, Textures.GetTexture(selected));
                    PaintBox();
                }
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            pictureBox.Focus();
        }

        private TextureSearch showform = null;
        private void OnClickSearch(object sender, EventArgs e)
        {
            if ((showform == null) || (showform.IsDisposed))
            {
                showform = new TextureSearch();
                showform.TopMost = true;
                showform.Show();
            }
        }

        private void onClickExport(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, String.Format("Texture {0}.tiff", selected));
            Bitmap bmp = Textures.GetTexture(selected);
            bmp.Save(FileName, ImageFormat.Tiff);
            MessageBox.Show(
                String.Format("Texture saved to {0}", FileName),
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void onClickFindNext(object sender, EventArgs e)
        {
            int id = selected;
            id++;
            for (int i = TextureList.IndexOf((object)selected) + 1; i < TextureList.Count; i++)
            {
                if (id < (int)TextureList[i])
                {
                    selected = (int)TextureList[i];
                    vScrollBar.Value = i / refMarker.col + 1;
                    Label.Text = String.Format("Graphic: 0x{0:X4} ({0}) [{1}x{1}", selected, Textures.GetTexture(selected));
                    PaintBox();
                    break;
                }
                id++;
            }
        }

        private void onClickRemove(object sender, EventArgs e)
        {
            DialogResult result =
                        MessageBox.Show(String.Format("Are you sure to remove 0x{0:X}", selected),
                        "Save",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                Textures.Remove(selected);
                TextureList.Remove((object)selected);
                selected--;
                PaintBox();
            }
        }

        private void onClickReplace(object sender, EventArgs e)
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
                    Textures.Replace(selected, bmp);
                    PaintBox();
                }
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

            if (index > 0xFFF)
                candone = false;
            if (candone)
            {
                if (Textures.TestTexture(index))
                    InsertText.ForeColor = Color.Red;
                else
                    InsertText.ForeColor = Color.Black;
            }
            else
                InsertText.ForeColor = Color.Red;
        }

        private void onKeyDownInsert(object sender, KeyEventArgs e)
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
                if (index > 0xFFF)
                    candone = false;
                if (candone)
                {
                    if (Textures.TestTexture(index))
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
                        if (((bmp.Width == 64) && (bmp.Height == 64)) || ((bmp.Width == 128) && (bmp.Height == 128)))
                        {
                            Textures.Replace(index, bmp);
                            for (int i = 0; i < TextureList.Count; i++)
                            {
                                if (index < (int)TextureList[i])
                                {
                                    selected = index;
                                    TextureList.Insert(i, (object)index);
                                    vScrollBar.Value = i / refMarker.col + 1;
                                    Label.Text = String.Format("Graphic: 0x{0:X4} ({0}) [{1}x{1}]", selected,Textures.GetTexture(selected));
                                    PaintBox();
                                    break;
                                }
                            }
                        }
                        else
                            MessageBox.Show("Height or Width Invalid", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                }
            }
        }

        private void onClickSave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Textures.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
            this.Cursor = Cursors.Default;
            MessageBox.Show(
                String.Format("Saved to {0}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase),
                "Save",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
    }
}
