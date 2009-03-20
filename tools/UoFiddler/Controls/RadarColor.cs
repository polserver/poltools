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
    public partial class RadarColor : UserControl
    {
        public RadarColor()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private bool Loaded = false;
        private int SelectedIndex = -1;
        private short CurrCol = -1;
        private bool UpDownSuppressR = false;
        private bool UpDownSuppressG = false;
        private bool UpDownSuppressB = false;
        private bool TextShortSuppress = false;


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
            Options.LoadedUltimaClass["RadarColor"] = true;
            Loaded = true;

            treeViewItem.BeginUpdate();
            treeViewItem.Nodes.Clear();
            if (TileData.ItemTable != null)
            {
                for (int i = 0; i < TileData.ItemTable.Length; ++i)
                {
                    TreeNode node = new TreeNode(String.Format("0x{0:X4} ({0}) {1}", i, TileData.ItemTable[i].Name));
                    node.Tag = i;
                    treeViewItem.Nodes.Add(node);
                }
            }
            treeViewItem.EndUpdate();
            treeViewLand.BeginUpdate();
            treeViewLand.Nodes.Clear();
            if (TileData.LandTable != null)
            {
                for (int i = 0; i < TileData.LandTable.Length; ++i)
                {
                    TreeNode node = new TreeNode(String.Format("0x{0:X4} ({0}) {1}", i, TileData.LandTable[i].Name));
                    node.Tag = i;
                    treeViewLand.Nodes.Add(node);
                }
            }
            treeViewLand.EndUpdate();
            this.Cursor = Cursors.Default;
        }

        private void AfterSelectTreeViewitem(object sender, TreeViewEventArgs e)
        {
            SelectedIndex = (int)e.Node.Tag;
            try
            {
                Bitmap bit = Art.GetStatic(SelectedIndex);
                Bitmap newbit = new Bitmap(pictureBoxArt.Size.Width, pictureBoxArt.Size.Height);
                Graphics newgraph = Graphics.FromImage(newbit);
                newgraph.Clear(Color.FromArgb(-1));
                newgraph.DrawImage(bit, (pictureBoxArt.Size.Width - bit.Width) / 2, 1);
                pictureBoxArt.Image = newbit;
            }
            catch
            {
                pictureBoxArt.Image = new Bitmap(pictureBoxArt.Width, pictureBoxArt.Height);
            }
            CurrCol = Ultima.RadarCol.GetItemColor(SelectedIndex);
            Color col = Ultima.Hues.HueToColor(CurrCol);
            pictureBoxColor.BackColor = col;
            TextShortSuppress = true;
            UpDownSuppressR = true;
            UpDownSuppressG = true;
            UpDownSuppressB = true;
            textBoxShortCol.Text = ((ushort)CurrCol).ToString();
            numericUpDownR.Value = col.R;
            numericUpDownG.Value = col.G;
            numericUpDownB.Value = col.B;
        }

        private void AfterSelectTreeViewLand(object sender, TreeViewEventArgs e)
        {
            SelectedIndex = (int)e.Node.Tag;
            try
            {
                Bitmap bit = Ultima.Art.GetLand(SelectedIndex);
                Bitmap newbit = new Bitmap(pictureBoxArt.Size.Width, pictureBoxArt.Size.Height);
                Graphics newgraph = Graphics.FromImage(newbit);
                newgraph.Clear(Color.FromArgb(-1));
                newgraph.DrawImage(bit, (pictureBoxArt.Size.Width - bit.Width) / 2, 1);
                pictureBoxArt.Image = newbit;
            }
            catch
            {
                pictureBoxArt.Image = new Bitmap(pictureBoxArt.Width, pictureBoxArt.Height);
            }
            CurrCol = Ultima.RadarCol.GetLandColor(SelectedIndex);
            Color col = Ultima.Hues.HueToColor(CurrCol);
            pictureBoxColor.BackColor = col;
            TextShortSuppress = true;
            UpDownSuppressR = true;
            UpDownSuppressG = true;
            UpDownSuppressB = true;
            textBoxShortCol.Text = ((ushort)CurrCol).ToString();
            numericUpDownR.Value = col.R;
            numericUpDownG.Value = col.G;
            numericUpDownB.Value = col.B;
        }

        private void OnClickMeanColor(object sender, EventArgs e)
        {
            Bitmap image;
            if (tabControl2.SelectedIndex==0)
                image = Art.GetStatic(SelectedIndex);
            else
                image = Art.GetLand(SelectedIndex);
            if (image == null)
                return;
            unsafe
            {
                BitmapData bd = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                ushort* line = (ushort*)bd.Scan0;
                int delta = bd.Stride >> 1;
                ushort* cur = line;
                int meanr = 0;
                int meang = 0;
                int meanb = 0;
                int count = 0;
                for (int y = 0; y < image.Height; y++, line += delta)
                {
                    cur = line;
                    for (int x = 0; x < image.Width; x++)
                    {
                        if (cur[x] != 0)
                        {
                            meanr += Ultima.Hues.HueToColorR((short)cur[x]);
                            meang += Ultima.Hues.HueToColorG((short)cur[x]);
                            meanb += Ultima.Hues.HueToColorB((short)cur[x]);
                            count++;
                        }
                    }
                }
                image.UnlockBits(bd);
                meanr /= count;
                meang /= count;
                meanb /= count;
                Color col = Color.FromArgb(meanr, meang, meanb);
                pictureBoxColor.BackColor = col;
                CurrCol = Ultima.Hues.ColorToHue(col);
                TextShortSuppress = true;
                UpDownSuppressR = true;
                UpDownSuppressG = true;
                UpDownSuppressB = true;
                textBoxShortCol.Text = ((ushort)CurrCol).ToString();
                numericUpDownR.Value=col.R;
                numericUpDownG.Value=col.G;
                numericUpDownB.Value=col.B;
            }
        }

        private void onClickSaveFile(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, "radarcol.mul");
            Ultima.RadarCol.Save(FileName);
            MessageBox.Show(
                String.Format("RadarCol saved to {0}", FileName),
                "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
            Options.ChangedUltimaClass["RadarCol"] = false;
        }

        private void onClickSaveColor(object sender, EventArgs e)
        {
            if (SelectedIndex>=0)
            {
                if (tabControl2.SelectedIndex == 0)
                    Ultima.RadarCol.SetItemColor(SelectedIndex,CurrCol);
                else
                    Ultima.RadarCol.SetLandColor(SelectedIndex,CurrCol);
                Options.ChangedUltimaClass["RadarCol"] = true;
            }

        }

        private void UpdateTextShort()
        {
            Color col = Color.FromArgb((int)numericUpDownR.Value, (int)numericUpDownG.Value, (int)numericUpDownB.Value);
            pictureBoxColor.BackColor = col;
            TextShortSuppress = true;
            CurrCol = Ultima.Hues.ColorToHue(col);
            textBoxShortCol.Text = ((ushort)CurrCol).ToString();
        }

        private void UpdateUpDown()
        {
            short txtcol;
            if (short.TryParse(textBoxShortCol.Text, out txtcol))
            {
                Color col = Ultima.Hues.HueToColor(txtcol);
                pictureBoxColor.BackColor = col;
                UpDownSuppressR = true;
                UpDownSuppressG = true;
                UpDownSuppressB = true;
                numericUpDownR.Value = col.R;
                numericUpDownG.Value = col.G;
                numericUpDownB.Value = col.B;
            }
        }

        private void onChangeR(object sender, EventArgs e)
        {
            if (!UpDownSuppressR)
                UpdateTextShort();
            UpDownSuppressR = false;
        }

        private void OnChangeG(object sender, EventArgs e)
        {
            if (!UpDownSuppressG)
                UpdateTextShort();
            UpDownSuppressG = false;
        }

        private void OnChangeB(object sender, EventArgs e)
        {
            if (!UpDownSuppressB)
                UpdateTextShort();
            UpDownSuppressB = false;
        }

        private void OnChangeShortText(object sender, EventArgs e)
        {
            if (!TextShortSuppress)
                UpdateUpDown();
            TextShortSuppress = false;
        }

        private void OnClickmeanColorFromTo(object sender, EventArgs e)
        {
            int from, to;
            if ((Utils.ConvertStringToInt(textBoxMeanFrom.Text, out from, 0, 0x4000)) &&
                (Utils.ConvertStringToInt(textBoxMeanTo.Text, out to, 0, 0x4000)))
            {
                if (to < from)
                {
                    int temp = from;
                    from = to;
                    to = temp;
                }
                int gmeanr = 0;
                int gmeang = 0;
                int gmeanb = 0;
                for (int i = from; i < to; i++)
                {
                    Bitmap image;
                    if (tabControl2.SelectedIndex == 0)
                        image = Art.GetStatic(i);
                    else
                        image = Art.GetLand(i);
                    if (image == null)
                        continue;
                    unsafe
                    {
                        BitmapData bd = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                        ushort* line = (ushort*)bd.Scan0;
                        int delta = bd.Stride >> 1;
                        ushort* cur = line;
                        int meanr = 0;
                        int meang = 0;
                        int meanb = 0;
                        int count = 0;
                        for (int y = 0; y < image.Height; y++, line += delta)
                        {
                            cur = line;
                            for (int x = 0; x < image.Width; x++)
                            {
                                if (cur[x] != 0)
                                {
                                    meanr += Ultima.Hues.HueToColorR((short)cur[x]);
                                    meang += Ultima.Hues.HueToColorG((short)cur[x]);
                                    meanb += Ultima.Hues.HueToColorB((short)cur[x]);
                                    count++;
                                }
                            }
                        }
                        image.UnlockBits(bd);
                        meanr /= count;
                        meang /= count;
                        meanb /= count;
                        gmeanr += meanr;
                        gmeang += meang;
                        gmeanb += meanb;
                    }
                    
                }
                gmeanr /= (to - from);
                gmeang /= (to - from);
                gmeanb /= (to - from);
                Color col = Color.FromArgb(gmeanr, gmeang, gmeanb);
                pictureBoxColor.BackColor = col;
                CurrCol = Ultima.Hues.ColorToHue(col);
                TextShortSuppress = true;
                UpDownSuppressR = true;
                UpDownSuppressG = true;
                UpDownSuppressB = true;
                textBoxShortCol.Text = ((ushort)CurrCol).ToString();
                numericUpDownR.Value = col.R;
                numericUpDownG.Value = col.G;
                numericUpDownB.Value = col.B;
            }
        }
    }
}
