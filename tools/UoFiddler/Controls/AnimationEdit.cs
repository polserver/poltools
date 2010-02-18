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
using System.Collections;

namespace FiddlerControls
{
    public partial class AnimationEdit : Form
    {
        public AnimationEdit()
        {
            InitializeComponent();
            this.Icon = FiddlerControls.Options.GetFiddlerIcon();
            FileType = 0;
            CurrDir = 0;
            toolStripComboBox1.SelectedIndex = 0;
            FramePoint = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
            ShowOnlyValid = false;
            Loaded = false;
        }

        private bool Loaded;
        private int FileType;
        int CurrAction;
        int CurrBody;
        private int CurrDir;
        private Point FramePoint;
        private bool ShowOnlyValid;

        private void onLoad(object sender, EventArgs e)
        {
            Options.LoadedUltimaClass["AnimationEdit"] = true;
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            if (FileType != 0)
            {
                int count = Animations.GetAnimCount(FileType);
                ArrayList nodes = new ArrayList();
                for (int i = 0; i < count; ++i)
                {
                    int animlength = Animations.GetAnimLength(i, FileType);
                    string type = animlength == 22 ? "H" : animlength == 13 ? "L" : "P";
                    TreeNode node = new TreeNode();
                    node.Tag = i;
                    node.Text = String.Format("{0}: {1} ({2})", type, i, BodyConverter.GetTrueBody(FileType, i));
                    bool valid = false;
                    for (int j = 0; j < animlength; ++j)
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
                    {
                        if (ShowOnlyValid)
                            continue;
                        node.ForeColor = Color.Red;
                    }
                    nodes.Add(node);
                }
                treeView1.Nodes.AddRange((TreeNode[])nodes.ToArray(typeof(TreeNode)));
            }
            treeView1.EndUpdate();
            if (treeView1.Nodes.Count > 0)
                treeView1.SelectedNode = treeView1.Nodes[0];
            if (!Loaded)
                FiddlerControls.Events.FilePathChangeEvent += new FiddlerControls.Events.FilePathChangeHandler(OnFilePathChangeEvent);
            Loaded = true;
        }

        private void OnFilePathChangeEvent()
        {
            if (!Loaded)
                return;
            FileType = 0;
            CurrDir = 0;
            CurrAction = 0;
            CurrBody = 0;
            toolStripComboBox1.SelectedIndex = 0;
            FramePoint = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
            ShowOnlyValid = false;
            showOnlyValidToolStripMenuItem.Checked = false;
            OnLoad(null);
        }

        private void onClose(object sender, FormClosedEventArgs e)
        {
            Loaded = false;
            FiddlerControls.Events.FilePathChangeEvent -= new FiddlerControls.Events.FilePathChangeHandler(OnFilePathChangeEvent);

        }

        private TreeNode GetNode(int tag)
        {
            if (ShowOnlyValid)
            {
                foreach (TreeNode node in treeView1.Nodes)
                {
                    if ((int)node.Tag == tag)
                        return node;
                }
                return null;
            }
            else
                return treeView1.Nodes[tag];
        }

        private unsafe void SetPaletteBox()
        {
            if (FileType != 0)
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
                        for (int i = 0; i < 0x100; ++i)
                        {
                            *cur++ = edit.Palette[i];
                        }
                    }
                    bmp.UnlockBits(bd);
                }
                pictureBoxPalette.Image = bmp;
            }
        }

        private void AfterSelectTreeView(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Parent == null)
                {
                    if (treeView1.SelectedNode.Tag != null)
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
                if (edit != null)
                {
                    int width = 80;
                    int height = 110;
                    Bitmap[] currbits = edit.GetFrames();
                    if (currbits != null)
                    {
                        for (int i = 0; i < currbits.Length; ++i)
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

            if (listView1.SelectedItems.Contains(e.Item))
                e.Graphics.DrawRectangle(new Pen(Color.Red), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            else
                e.Graphics.DrawRectangle(new Pen(Color.Gray), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            e.Graphics.DrawImage(bmp, e.Bounds.X, e.Bounds.Y, width, height);
            e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter);
        }

        private void onAnimChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedIndex != FileType)
            {
                FileType = toolStripComboBox1.SelectedIndex;
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
                if ((currbits != null) && (currbits.Length > 0))
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
            if (FileType != 0)
            {
                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                if (edit != null)
                {
                    if (edit.Frames.Count >= trackBar2.Value)
                    {
                        numericUpDownCx.Value = ((FrameEdit)edit.Frames[trackBar2.Value]).Center.X;
                        numericUpDownCy.Value = ((FrameEdit)edit.Frames[trackBar2.Value]).Center.Y;
                    }
                }
                pictureBox1.Refresh();
            }
        }

        private void OnCenterXValueChanged(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                if (edit != null)
                {
                    if (edit.Frames.Count >= trackBar2.Value)
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
            }
        }

        private void OnCenterYValueChanged(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                if (edit != null)
                {
                    if (edit.Frames.Count >= trackBar2.Value)
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
            }
        }

        private void onClickExtractImages(object sender, EventArgs e)
        {
            if (FileType != 0)
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
                    for (int a = 0; a < Animations.GetAnimLength(body, FileType); ++a)
                    {
                        for (int i = 0; i < 5; ++i)
                        {
                            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, body, a, i);
                            if (edit != null)
                            {
                                Bitmap[] bits = edit.GetFrames();
                                if (bits != null)
                                {
                                    for (int j = 0; j < bits.Length; ++j)
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
                    for (int i = 0; i < 5; ++i)
                    {
                        AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, body, action, i);
                        if (edit != null)
                        {
                            Bitmap[] bits = edit.GetFrames();
                            if (bits != null)
                            {
                                for (int j = 0; j < bits.Length; ++j)
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
        }

        private void OnClickRemoveAction(object sender, EventArgs e)
        {
            if (FileType != 0)
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
                        treeView1.SelectedNode.ForeColor = Color.Red;
                        for (int i = 0; i < treeView1.SelectedNode.Nodes.Count; ++i)
                        {
                            treeView1.SelectedNode.Nodes[i].ForeColor = Color.Red;
                            for (int d = 0; d < 5; ++d)
                            {
                                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, i, d);
                                if (edit != null)
                                    edit.ClearFrames();
                            }
                        }
                        if (ShowOnlyValid)
                            treeView1.SelectedNode.Remove();
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
                        for (int i = 0; i < 5; ++i)
                        {
                            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, i);
                            if (edit != null)
                                edit.ClearFrames();
                        }
                        treeView1.SelectedNode.Parent.Nodes[CurrAction].ForeColor = Color.Red;
                        bool valid = false;
                        foreach (TreeNode node in treeView1.SelectedNode.Parent.Nodes)
                        {
                            if (node.ForeColor != Color.Red)
                            {
                                valid = true;
                                break;
                            }
                        }
                        if (!valid)
                        {
                            if (ShowOnlyValid)
                                treeView1.SelectedNode.Parent.Remove();
                            else
                                treeView1.SelectedNode.Parent.ForeColor = Color.Red;
                        }
                        Options.ChangedUltimaClass["Animations"] = true;
                        AfterSelectTreeView(this, null);
                    }
                }
            }
        }

        private void OnClickSave(object sender, EventArgs e)
        {
            if (FileType != 0)
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
            if (listView1.SelectedItems.Count > 0)
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
            if (FileType != 0)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = true;
                    dialog.Title = "Choose image files to add";
                    dialog.CheckFileExists = true;
                    dialog.Filter = "image files (*.tiff;*.bmp)|*.tiff;*.bmp";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        listView1.BeginUpdate();
                        foreach (string filename in dialog.FileNames)
                        {
                            Bitmap bmp = new Bitmap(filename);
                            if (filename.Contains(".bmp"))
                                bmp = Utils.ConvertBmp(bmp);
                            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                            if (edit != null)
                            {
                                edit.AddFrame(bmp);
                                TreeNode node = GetNode(CurrBody);
                                if (node != null)
                                {
                                    node.ForeColor = Color.Black;
                                    node.Nodes[CurrAction].ForeColor = Color.Black;
                                }
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
                                Options.ChangedUltimaClass["Animations"] = true;
                            }
                        }
                        listView1.EndUpdate();
                        listView1.Refresh();
                    }
                }
            }
        }

        private void onClickExtractPalette(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                ToolStripMenuItem menu = (ToolStripMenuItem)sender;
                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                if (edit != null)
                {
                    string name = String.Format("palette_anim{0}_{1}_{2}_{3}", FileType, CurrBody, CurrAction, CurrDir);
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
        }

        private void onClickImportPalette(object sender, EventArgs e)
        {
            if (FileType != 0)
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
                                    Palette[i++] = ushort.Parse(line);
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

        private void OnClickImportFromVD(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = false;
                    dialog.Title = "Choose palette file";
                    dialog.CheckFileExists = true;
                    dialog.Filter = "vd files (*.vd)|*.vd";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        int animlength = Animations.GetAnimLength(CurrBody, FileType);
                        int currtype = animlength == 22 ? 0 : animlength == 13 ? 1 : 2;
                        using (FileStream fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using (BinaryReader bin = new BinaryReader(fs))
                            {
                                int filetype = bin.ReadInt16();
                                int animtype = bin.ReadInt16();
                                if (filetype != 6)
                                {
                                    MessageBox.Show(
                                        "Not an Anim File.",
                                        "Import",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1);
                                    return;
                                }

                                if (animtype != currtype)
                                {
                                    MessageBox.Show(
                                        "Wrong Anim Id ( Type )",
                                        "Import",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1);
                                    return;
                                }
                                Ultima.AnimationEdit.LoadFromVD(FileType, CurrBody, bin);
                            }
                        }

                        bool valid = false;
                        TreeNode node = GetNode(CurrBody);
                        if (node != null)
                        {
                            for (int j = 0; j < animlength; ++j)
                            {
                                if (Ultima.AnimationEdit.IsActionDefinied(FileType, CurrBody, j))
                                {
                                    node.Nodes[j].ForeColor = Color.Black;
                                    valid = true;
                                }
                                else
                                    node.Nodes[j].ForeColor = Color.Red;
                            }
                            if (valid)
                                node.ForeColor = Color.Black;
                            else
                                node.ForeColor = Color.Red;
                        }

                        Options.ChangedUltimaClass["Animations"] = true;
                        AfterSelectTreeView(this, null);
                        MessageBox.Show(
                                        "Finished",
                                        "Import",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1);
                    }
                }
            }
        }

        private void OnClickExportToVD(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string FileName = Path.Combine(path, String.Format("anim{0}_0x{1:X}.vd", FileType, CurrBody));
                Ultima.AnimationEdit.ExportToVD(FileType, CurrBody, FileName);
                MessageBox.Show(
                        String.Format("Animation saved to {0}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase),
                        "Export",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
            }
        }

        private void OnClickShowOnlyValid(object sender, EventArgs e)
        {
            ShowOnlyValid = !ShowOnlyValid;
            if (ShowOnlyValid)
            {
                treeView1.BeginUpdate();
                for (int i = treeView1.Nodes.Count - 1; i >= 0; --i)
                {
                    if (treeView1.Nodes[i].ForeColor == Color.Red)
                        treeView1.Nodes[i].Remove();
                }
                treeView1.EndUpdate();
            }
            else
                OnLoad(null);
        }

        private unsafe void OnClickGeneratePalette(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.Title = "Choose images to generate from";
                dialog.CheckFileExists = true;
                dialog.Filter = "image files (*.tiff;*.bmp)|*.tiff;*.bmp";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ushort[] Palette = new ushort[0x100];
                    int count = 0;
                    foreach (string filename in dialog.FileNames)
                    {
                        Bitmap bmp = new Bitmap(filename);
                        if (dialog.FileName.Contains(".bmp"))
                            bmp = Utils.ConvertBmp(bmp);

                        BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                        ushort* line = (ushort*)bd.Scan0;
                        int delta = bd.Stride >> 1;
                        ushort* cur = line;
                        for (int y = 0; y < bmp.Height; ++y, line += delta)
                        {
                            cur = line;
                            for (int x = 0; x < bmp.Width; ++x)
                            {
                                ushort c = cur[x];
                                if (c != 0)
                                {
                                    bool found = false;
                                    for (int i = 0; i < Palette.Length; ++i)
                                    {
                                        if (Palette[i] == c)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (!found)
                                        Palette[count++] = c;
                                    if (count >= 0x100)
                                    {
                                        MessageBox.Show(
                                            "More then 0x100 colors found!",
                                            "Generate",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button1);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    string FileName = Path.Combine(path, "generated palette.txt");
                    using (StreamWriter Tex = new StreamWriter(new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite)))
                    {
                        for (int i = 0; i < 0x100; ++i)
                        {
                            Tex.WriteLine(Palette[i]);
                        }
                    }
                    MessageBox.Show(
                        String.Format("Palette saved to {0}", FileName),
                        "Generate Palette",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void OnClickExportAllToVD(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "Select directory";
                    dialog.ShowNewFolderButton = true;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        for (int i = 0; i < treeView1.Nodes.Count; ++i)
                        {
                            int index = (int)treeView1.Nodes[i].Tag;
                            if (index >= 0 && treeView1.Nodes[i].Parent == null && treeView1.Nodes[i].ForeColor != Color.Red)
                            {
                                string FileName = Path.Combine(dialog.SelectedPath, String.Format("anim{0}_0x{1:X}.vd", FileType, index));
                                Ultima.AnimationEdit.ExportToVD(FileType, index, FileName);
                            }
                        }
                        MessageBox.Show(String.Format("All Animations saved to {0}", dialog.SelectedPath.ToString()),
                                "Export", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
            }

        }
    }
}
