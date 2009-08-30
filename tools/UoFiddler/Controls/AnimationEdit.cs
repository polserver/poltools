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
            Bitmap bmp = new Bitmap(0x100, pictureBoxPalette.Height, PixelFormat.Format16bppArgb1555);
            if (edit != null)
            {
                BitmapData bd = bmp.LockBits(new Rectangle(0, 0, 0x100, pictureBoxPalette.Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
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

                        numericUpDownCx.Value = ((FrameEdit)edit.Frames[trackBar2.Value]).Center.X;
                        numericUpDownCy.Value = ((FrameEdit)edit.Frames[trackBar2.Value]).Center.Y;
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
            if (listView1.SelectedItems.Contains(e.Item))
                e.Graphics.DrawRectangle(new Pen(Color.Red), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            else
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
                        int x = FramePoint.X - ((FrameEdit)edit.Frames[trackBar2.Value]).Center.X;
                        int y = FramePoint.Y - ((FrameEdit)edit.Frames[trackBar2.Value]).Center.Y - currbits[trackBar2.Value].Height;
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
                numericUpDownCx.Value = ((FrameEdit)edit.Frames[trackBar2.Value]).Center.X;
                numericUpDownCy.Value = ((FrameEdit)edit.Frames[trackBar2.Value]).Center.Y;
            }
            pictureBox1.Refresh();
        }

        private void OnCenterXValueChanged(object sender, EventArgs e)
        {
            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
            if (edit != null)
            {
                FrameEdit frame = (FrameEdit)edit.Frames[trackBar2.Value];
                if (numericUpDownCx.Value != frame.Center.X)
                {
                    frame.ChangeCenter((int)numericUpDownCx.Value, frame.Center.Y);
                    Options.ChangedUltimaClass["Animations"] = true;
                    pictureBox1.Refresh();
                }
            }
        }

        private void OnCenterYValueChanged(object sender, EventArgs e)
        {
            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
            if (edit != null)
            {
                FrameEdit frame = (FrameEdit)edit.Frames[trackBar2.Value];
                if (numericUpDownCy.Value != frame.Center.Y)
                {
                    frame.ChangeCenter(frame.Center.X, (int)numericUpDownCy.Value);
                    Options.ChangedUltimaClass["Animations"] = true;
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
                                    string filename = String.Format("anim{5}_{0}_{1}_{2}_{3}{4}", body, a, i, j, menu.Tag, FileType);
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
                                string filename = String.Format("anim{5}_{0}_{1}_{2}_{3}{4}", body, action, i, j, menu.Tag, FileType);
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
            MessageBox.Show(
                    String.Format("Frames saved to {0}", path),
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);

        }

        private void OnClickRemoveAction(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Parent == null)
            {
                DialogResult result =
                       MessageBox.Show(String.Format("Are you sure to remove animation {0}", CurrBody),
                       "Remove",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Question,
                       MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    treeView1.Nodes[CurrBody].ForeColor = Color.Red;
                    for (int i = 0; i < treeView1.Nodes[CurrBody].Nodes.Count; i++)
                    {
                        treeView1.Nodes[CurrBody].Nodes[i].ForeColor = Color.Red;
                        for (int d = 0; d < 5; ++d)
                        {
                            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, i, d);
                            if (edit != null)
                                edit.ClearFrames();
                        }
                    }
                    Options.ChangedUltimaClass["Animations"] = true;
                    AfterSelectTreeView(this, null);
                }
            }
            else
            {
                DialogResult result =
                       MessageBox.Show(String.Format("Are you sure to remove action {0}", CurrAction),
                       "Remove",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Question,
                       MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, i);
                        if (edit != null)
                            edit.ClearFrames();
                    }
                    treeView1.Nodes[CurrBody].Nodes[CurrAction].ForeColor = Color.Red;
                    bool valid = false;
                    foreach (TreeNode node in treeView1.Nodes[CurrBody].Nodes)
                    {
                        if (node.ForeColor != Color.Red)
                        {
                            valid = true;
                            break;
                        }
                    }
                    if (!valid)
                        treeView1.Nodes[CurrBody].ForeColor = Color.Red;
                    Options.ChangedUltimaClass["Animations"] = true;
                    AfterSelectTreeView(this, null);
                }
            }
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
            Options.ChangedUltimaClass["Animations"] = false;
        }

        private void OnClickRemoveFrame(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int frameindex = (int)listView1.SelectedItems[0].Tag;
                DialogResult result =
                       MessageBox.Show(String.Format("Are you sure to remove {0}", frameindex),
                       "Remove",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Question,
                       MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        edit.RemoveFrame(frameindex);
                        listView1.Items.RemoveAt(listView1.Items.Count - 1);
                        listView1.Refresh();
                        Options.ChangedUltimaClass["Animations"] = true;
                    }
                }
            }
        }

        private void OnClickReplace(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count>0)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    int frameindex = (int)listView1.SelectedItems[0].Tag;
                    dialog.Multiselect = false;
                    dialog.Title = String.Format("Choose image file to replace at {0}", frameindex);
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
                            Options.ChangedUltimaClass["Animations"] = true;
                        }
                    }
                }
            }
        }

        private void OnClickAdd(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Title = "Choose image file to add";
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
                        edit.AddFrame(bmp);
                        ListViewItem item;
                        int i = edit.Frames.Count - 1;
                        item = new ListViewItem(i.ToString(), 0);
                        item.Tag = i;
                        listView1.Items.Add(item);
                        int width = listView1.TileSize.Width - 5;
                        if (bmp.Width > listView1.TileSize.Width)
                            width = bmp.Width;
                        int height = listView1.TileSize.Height - 5;
                        if (bmp.Height > listView1.TileSize.Height)
                            height = bmp.Height;

                        listView1.TileSize = new Size(width + 5, height + 5);
                        trackBar2.Maximum = i;
                        listView1.Refresh();
                        Options.ChangedUltimaClass["Animations"] = true;
                    }
                }
            }
        }

        private void onClickExtractPalette(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
            if (edit != null)
            {
                string name=String.Format("palette_anim{0}_{1}_{2}_{3}",FileType,CurrBody,CurrAction,CurrDir);
                if (((string)menu.Tag) == "txt")
                {
                    string path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, name + ".txt");
                    edit.ExportPalette(path, 0);
                }
                else
                {
                    string path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, name + "." + (string)menu.Tag);
                    if (((string)menu.Tag) == "bmp")
                        edit.ExportPalette(path, 1);
                    else
                        edit.ExportPalette(path, 2);
                }
                MessageBox.Show(
                    String.Format("Palette saved to {0}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase),
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
            }
        }

        private void onClickImportPalette(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Title = "Choose palette file";
                dialog.CheckFileExists = true;
                dialog.Filter = "txt files (*.txt)|*.txt";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        using (StreamReader sr = new StreamReader(dialog.FileName))
                        {
                            string line;
                            ushort[] Palette = new ushort[0x100];
                            int i = 0;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                                    continue;
                                Palette[i] = ushort.Parse(line);
                                ++i;
                                if (i >= 0x100)
                                    break;
                            }
                            edit.ReplacePalette(Palette);
                        }
                        SetPaletteBox();
                        listView1.Refresh();
                        Options.ChangedUltimaClass["Animations"] = true;
                    }
                }
            }
        }
    }
}
