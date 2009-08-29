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
    public partial class AnimationEdit : Form
    {
        public AnimationEdit()
        {
            InitializeComponent();
            this.Icon = FiddlerControls.Options.GetFiddlerIcon();
            FileType = 1;
            CurrDir = 0;
            toolStripComboBox1.SelectedIndex = 0;
            FramePoint = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
        }

        private int FileType;
        int CurrAction;
        int CurrBody;
        private int CurrDir;
        private Point FramePoint;

        private void onLoad(object sender, EventArgs e)
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();

            int count = Animations.GetAnimCount(FileType);
            TreeNode[] nodes = new TreeNode[count];
            for (int i = 0; i < count; i++)
            {
                int animlength = Animations.GetAnimLength(i, FileType);
                string type = animlength == 22 ? "H" : animlength == 13 ? "L" : "P";
                TreeNode node = new TreeNode();
                node.Tag = i;
                node.Text = String.Format("{0}: {1} ({2})", type, i, BodyConverter.GetTrueBody(FileType, i));
                nodes[i] = node;
                bool valid = false;
                for (int j = 0; j < animlength; j++)
                {
                    TreeNode subnode = new TreeNode();
                    subnode.Tag = j;
                    subnode.Text = j.ToString();
                    if (Ultima.AnimationEdit.IsActionDefinied(FileType, i, j))
                        valid = true;
                    else
                        subnode.ForeColor = Color.Red;
                    node.Nodes.Add(subnode);
                }
                if (!valid)
                    node.ForeColor = Color.Red;
            }
            treeView1.Nodes.AddRange(nodes);
            treeView1.EndUpdate();
            if (treeView1.Nodes.Count > 0)
                treeView1.SelectedNode = treeView1.Nodes[0];
        }

        private unsafe void SetPaletteBox()
        {
            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
            Bitmap bmp = new Bitmap(pictureBoxPalette.Width, pictureBoxPalette.Height, PixelFormat.Format16bppArgb1555);
            if (edit != null)
            {
                BitmapData bd = bmp.LockBits(new Rectangle(0, 0, pictureBoxPalette.Width, pictureBoxPalette.Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
                ushort* line = (ushort*)bd.Scan0;
                int delta = bd.Stride >> 1;
                for (int y = 0; y < bd.Height; ++y, line += delta)
                {
                    ushort* cur = line;
                    for (int i = 0; i < 0x100; i++)
                    {
                        *cur++ = edit.Palette[i];
                    }
                }
                bmp.UnlockBits(bd);
            }
            pictureBoxPalette.Image = bmp;

        }
        private void AfterSelectTreeView(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Parent == null)
                {
                    if (treeView1.SelectedNode.Tag!=null)
                        CurrBody = (int)treeView1.SelectedNode.Tag;
                    CurrAction = 0;
                }
                else
                {
                    if (treeView1.SelectedNode.Parent.Tag != null)
                        CurrBody = (int)treeView1.SelectedNode.Parent.Tag;
                    CurrAction = (int)treeView1.SelectedNode.Tag;
                }
                listView1.BeginUpdate();
                listView1.Clear();
                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                if (edit!=null)
                {
                    int width = 80;
                    int height = 110;
                    Bitmap[] currbits = edit.GetFrames();
                    if (currbits != null)
                    {
                        for (int i = 0; i < currbits.Length; i++)
                        {
                            if (currbits[i] == null)
                                continue;
                            ListViewItem item;
                            item = new ListViewItem(i.ToString(), 0);
                            item.Tag = i;
                            listView1.Items.Add(item);
                            if (currbits[i].Width > width)
                                width = currbits[i].Width;
                            if (currbits[i].Height > height)
                                height = currbits[i].Height;
                        }
                        listView1.TileSize = new Size(width + 5, height + 5);
                        trackBar2.Maximum = currbits.Length - 1;
                        trackBar2.Value = 0;

                        numericUpDownCx.Value = edit.Frames[trackBar2.Value].Center.X;
                        numericUpDownCy.Value = edit.Frames[trackBar2.Value].Center.Y;
                    }
                }
                listView1.EndUpdate();
                pictureBox1.Refresh();
                SetPaletteBox();
            }
        }

        private void DrawFrameItem(object sender, DrawListViewItemEventArgs e)
        {
            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
            Bitmap[] currbits = edit.GetFrames();
            Bitmap bmp = currbits[(int)e.Item.Tag];
            int width = bmp.Width;
            int height = bmp.Height;

            e.Graphics.DrawImage(bmp, e.Bounds.X, e.Bounds.Y, width, height);
            e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter);
            e.Graphics.DrawRectangle(new Pen(Color.Gray), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
        }

        private void onAnimChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedIndex + 1 != FileType)
            {
                FileType = toolStripComboBox1.SelectedIndex + 1;
                onLoad(this, EventArgs.Empty);
            }
        }

        private void OnDirectionChanged(object sender, EventArgs e)
        {
            CurrDir = trackBar1.Value;
            AfterSelectTreeView(null, null);
        }

        private void OnSizeChangedPictureBox(object sender, EventArgs e)
        {
            FramePoint = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
            pictureBox1.Refresh();
        }

        private void onPaintFrame(object sender, PaintEventArgs e)
        {
            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
            if (edit != null)
            {
                Bitmap[] currbits = edit.GetFrames();
                e.Graphics.Clear(Color.LightGray);
                e.Graphics.DrawLine(Pens.Black, new Point(FramePoint.X, 0), new Point(FramePoint.X, pictureBox1.Height));
                e.Graphics.DrawLine(Pens.Black, new Point(0, FramePoint.Y), new Point(pictureBox1.Width, FramePoint.Y));
                if (currbits.Length > 0)
                {
                    if (currbits[trackBar2.Value] != null)
                    {
                        int x = FramePoint.X - edit.Frames[trackBar2.Value].Center.X;
                        int y = FramePoint.Y - edit.Frames[trackBar2.Value].Center.Y - currbits[trackBar2.Value].Height;
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(x, y, currbits[trackBar2.Value].Width, currbits[trackBar2.Value].Height));
                        e.Graphics.DrawImage(currbits[trackBar2.Value], x, y);
                    }
                }
            }
        }

        private void onFrameCountBarChanged(object sender, EventArgs e)
        {
            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
            if (edit != null)
            {
                numericUpDownCx.Value = edit.Frames[trackBar2.Value].Center.X;
                numericUpDownCy.Value = edit.Frames[trackBar2.Value].Center.Y;
            }
            pictureBox1.Refresh();
        }

        private void OnCenterXValueChanged(object sender, EventArgs e)
        {
            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
            if (edit != null)
            {
                if (numericUpDownCx.Value != edit.Frames[trackBar2.Value].Center.X)
                {
                    edit.Frames[trackBar2.Value].Center = new Point((int)numericUpDownCx.Value, edit.Frames[trackBar2.Value].Center.Y);
                    pictureBox1.Refresh();
                }
            }
        }

        private void OnCenterYValueChanged(object sender, EventArgs e)
        {
            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
            if (edit != null)
            {
                if (numericUpDownCy.Value != edit.Frames[trackBar2.Value].Center.Y)
                {
                    edit.Frames[trackBar2.Value].Center = new Point(edit.Frames[trackBar2.Value].Center.X, (int)numericUpDownCy.Value);
                    pictureBox1.Refresh();
                }
            }
        }

        private void onClickExtractImages(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            ImageFormat format = ImageFormat.Bmp;
            if (((string)menu.Tag) == ".tiff")
                format = ImageFormat.Tiff;
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            int body, action;
            if (treeView1.SelectedNode.Parent == null)
            {
                body = (int)treeView1.SelectedNode.Tag;
                action = -1;
            }
            else
            {
                body = (int)treeView1.SelectedNode.Parent.Tag;
                action = (int)treeView1.SelectedNode.Tag;
            }

            if (action == -1)
            {
                for (int a = 0; a < Animations.GetAnimLength(body, FileType); a++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, body, a, i);
                        if (edit != null)
                        {
                            Bitmap[] bits = edit.GetFrames();
                            if (bits != null)
                            {
                                for (int j = 0; j < bits.Length; j++)
                                {
                                    string filename = String.Format("anim_{0}_{1}_{2}_{3}{4}", body, a, i, j, menu.Tag);
                                    string file = Path.Combine(path, filename);
                                    Bitmap bit = new Bitmap(bits[j]);
                                    if (bit != null)
                                        bit.Save(file, format);
                                    bit.Dispose();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, body, action, i);
                    if (edit != null)
                    {
                        Bitmap[] bits = edit.GetFrames();
                        if (bits != null)
                        {
                            for (int j = 0; j < bits.Length; j++)
                            {
                                string filename = String.Format("anim_{0}_{1}_{2}_{3}{4}", body, action, i, j, menu.Tag);
                                string file = Path.Combine(path, filename);
                                Bitmap bit = new Bitmap(bits[j]);
                                if (bit != null)
                                    bit.Save(file, ImageFormat.Tiff);
                                bit.Dispose();
                            }
                        }
                    }
                }

            }
            MessageBox.Show(
                    String.Format("Frames saved to {0}", path),
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);

        }

        private void OnClickSave(object sender, EventArgs e)
        {
            Ultima.AnimationEdit.Save(FileType, AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
            MessageBox.Show(
                    String.Format("AnimationFile saved to {0}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase),
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
        }

        private void OnClickReplace(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count>0)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    int frameindex = (int)listView1.SelectedItems[0].Tag;
                    dialog.Multiselect = false;
                    dialog.Title = String.Format("Choose image file to replace at 0x{0:X}", frameindex);
                    dialog.CheckFileExists = true;
                    dialog.Filter = "image files (*.tiff;*.bmp)|*.tiff;*.bmp";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Bitmap bmp = new Bitmap(dialog.FileName);
                        if (dialog.FileName.Contains(".bmp"))
                            bmp = Utils.ConvertBmp(bmp);
                        AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                        if (edit != null)
                        {
                            edit.ReplaceFrame(bmp, frameindex);
                            listView1.Refresh();
                        }
                    }
                }
            }
        }
    }
}
