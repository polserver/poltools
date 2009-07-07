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
//GNA i hate osi...

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

        //private Frame[] CurrFrames;
        private int FileType;
        private int CurrDir;
        private Point FramePoint;

        private void onLoad(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();

            int count = Animations.GetAnimCount(FileType);
            TreeNode[] nodes = new TreeNode[count];
            for (int i = 0; i < count; i++)
            {
                AnimEdit anim=Ultima.AnimationEdit.GetAnimation(FileType, i);
                if (anim != null)
                {
                    int animlength = anim.Action.Length;
                    string type = animlength == 22 ? "H" : animlength == 13 ? "L" : "P";
                    TreeNode node = new TreeNode();
                    node.Tag = anim;
                    node.Text = String.Format("{0}: {1} ({2})", type, i, BodyConverter.GetTrueBody(FileType, i));
                    nodes[i]=node;
                    for (int j = 0; j < animlength; j++)
                    {
                        TreeNode subnode = new TreeNode();
                        subnode.Tag = j;
                        subnode.Text = j.ToString();
                        if (!Animations.IsAnimDefinied(i, j, 0, FileType))
                            subnode.ForeColor = Color.Red;
                        node.Nodes.Add(subnode);
                    }
                }
                else
                {
                    int animlength = Animations.GetAnimLength(i, FileType);
                    string type = animlength == 22 ? "H" : animlength == 13 ? "L" : "P";
                    TreeNode node = new TreeNode();
                    node.Tag = null;
                    node.Text = String.Format("{0}: {1} ({2})", type, i, BodyConverter.GetTrueBody(FileType, i));
                    node.ForeColor = Color.Red;
                    nodes[i] = node;
                }
            }
            treeView1.Nodes.AddRange(nodes);
            treeView1.EndUpdate();
            watch.Stop();//77816
            Console.WriteLine(watch.ElapsedMilliseconds);
            if (treeView1.Nodes.Count > 0)
                treeView1.SelectedNode = treeView1.Nodes[0];
        }

        Ultima.AnimEdit edit;
        //Bitmap[] currbits;
        int curraction;
        private void AfterSelectTreeView(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Parent == null)
                {
                    edit = (Ultima.AnimEdit)treeView1.SelectedNode.Tag;
                    curraction = 0;
                }
                else
                {
                    edit = (Ultima.AnimEdit)treeView1.SelectedNode.Parent.Tag;
                    curraction = (int)treeView1.SelectedNode.Tag;
                }
                //CurrFrames = Animations.GetAnimation(body, action, CurrDir, FileType);
                //edit = Ultima.AnimationEdit.GetAnimation(FileType, body);
                listView1.BeginUpdate();
                listView1.Clear();
                if (edit!=null)//(CurrFrames != null)
                {
                    int width = 80;
                    int height = 110;
                    Bitmap[] currbits = edit.GetFrames(curraction, CurrDir);
                    if (currbits != null)
                    {
                        for (int i = 0; i < currbits.Length/*CurrFrames.Length*/; i++)
                        {
                            if (currbits[i] == null)//(CurrFrames[i].Bitmap == null)
                                continue;
                            ListViewItem item;
                            item = new ListViewItem(i.ToString(), 0);
                            item.Tag = i;
                            listView1.Items.Add(item);
                            if (currbits[i].Width > width)
                                width = currbits[i].Width;
                            if (currbits[i].Height > height)
                                height = currbits[i].Height;
                            //if (CurrFrames[i].Bitmap.Width > width)
                            //    width = CurrFrames[i].Bitmap.Width;
                            //if (CurrFrames[i].Bitmap.Height > height)
                            //    height = CurrFrames[i].Bitmap.Height;
                        }
                        listView1.TileSize = new Size(width + 5, height + 5);
                        trackBar2.Maximum = currbits.Length - 1;
                        trackBar2.Value = 0;

                        numericUpDownCx.Value = edit.Action[curraction].Directions[CurrDir].Frames[trackBar2.Value].Center.X;
                        numericUpDownCy.Value = edit.Action[curraction].Directions[CurrDir].Frames[trackBar2.Value].Center.Y;
                    }
                }
                listView1.EndUpdate();
                pictureBox1.Refresh();
            }
        }

        private void DrawFrameItem(object sender, DrawListViewItemEventArgs e)
        {
            Bitmap[] currbits = edit.GetFrames(curraction, CurrDir);
            Bitmap bmp = currbits[(int)e.Item.Tag];//CurrFrames[(int)e.Item.Tag].Bitmap;
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

        private void onClickExtractImages(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            int body, action;
            if (treeView1.SelectedNode.Parent == null)
            {
                body = (int)treeView1.SelectedNode.Tag;
                action = 0;
            }
            else
            {
                body = (int)treeView1.SelectedNode.Parent.Tag;
                action = (int)treeView1.SelectedNode.Tag;
            }

            //for (int i = 0; i < CurrFrames.Length; i++)
            //{
            //    string FileName = Path.Combine(path, String.Format("Animation {0} {1}-{2}.tiff", body, action, i));
            //    CurrFrames[i].Bitmap.Save(FileName, ImageFormat.Tiff);
            //}
            MessageBox.Show(
                    String.Format("Frames saved to {0}", path),
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);

        }

        private void OnClickSave(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            int body, action;
            if (treeView1.SelectedNode.Parent == null)
            {
                body = (int)treeView1.SelectedNode.Tag;
                action = 0;
            }
            else
            {
                body = (int)treeView1.SelectedNode.Parent.Tag;
                action = (int)treeView1.SelectedNode.Tag;
            }
            //Animations.GetAnimation_(body, action, 0, 0, path);
            //Animations.SaveFrame(body, CurrFrames, 1, path);
            MessageBox.Show(
                    String.Format("Frames saved to {0}", path),
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
        }

        private void OnClickReplace(object sender, EventArgs e)
        {

        }

        private void onTextChangedAdd(object sender, EventArgs e)
        {
            int index;
            if (Utils.ConvertStringToInt(toolStripTextBoxAdd.Text, out index, 0, Animations.GetAnimCount(FileType)))
            {
                if (Animations.IsAnimDefinied(index, 0, 0, FileType))
                    toolStripTextBoxAdd.ForeColor = Color.Red;
                else
                    toolStripTextBoxAdd.ForeColor = Color.Black;
            }
            else
                toolStripTextBoxAdd.ForeColor = Color.Red;
        }

        private void onKeyDownAdd(object sender, KeyEventArgs e)
        {

        }

        private void OnSizeChangedPictureBox(object sender, EventArgs e)
        {
            FramePoint = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
            pictureBox1.Refresh();
        }

        private void onPaintFrame(object sender, PaintEventArgs e)
        {
            Bitmap[] currbits = edit.GetFrames(curraction, CurrDir);
            e.Graphics.Clear(Color.LightGray);
            e.Graphics.DrawLine(Pens.Black, new Point(FramePoint.X, 0), new Point(FramePoint.X, pictureBox1.Height));
            e.Graphics.DrawLine(Pens.Black, new Point(0, FramePoint.Y), new Point(pictureBox1.Width, FramePoint.Y));
            if (currbits.Length > 0)
            {
                if (currbits[trackBar2.Value] != null)
                {
                    int x = FramePoint.X - edit.Action[curraction].Directions[CurrDir].Frames[trackBar2.Value].Center.X;
                    int y = FramePoint.Y - edit.Action[curraction].Directions[CurrDir].Frames[trackBar2.Value].Center.Y - currbits[trackBar2.Value].Height;
                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(x, y, currbits[trackBar2.Value].Width, currbits[trackBar2.Value].Height));
                    e.Graphics.DrawImage(currbits[trackBar2.Value], x, y);
                }
            }
        }

        private void onFrameCountBarChanged(object sender, EventArgs e)
        {
            numericUpDownCx.Value = edit.Action[curraction].Directions[CurrDir].Frames[trackBar2.Value].Center.X;
            numericUpDownCy.Value = edit.Action[curraction].Directions[CurrDir].Frames[trackBar2.Value].Center.Y;
            pictureBox1.Refresh();
        }

        private void OnCenterXValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownCx.Value != edit.Action[curraction].Directions[CurrDir].Frames[trackBar2.Value].Center.X)
            {
                edit.Action[curraction].Directions[CurrDir].Frames[trackBar2.Value].Center = new Point((int)numericUpDownCx.Value, edit.Action[curraction].Directions[CurrDir].Frames[trackBar2.Value].Center.Y);
                pictureBox1.Refresh();
            }
        }

        private void OnCenterYValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownCy.Value != edit.Action[curraction].Directions[CurrDir].Frames[trackBar2.Value].Center.Y)
            {
                edit.Action[curraction].Directions[CurrDir].Frames[trackBar2.Value].Center = new Point(edit.Action[curraction].Directions[CurrDir].Frames[trackBar2.Value].Center.X, (int)numericUpDownCy.Value);
                pictureBox1.Refresh();
            }
        }

        private void OnClickDefrag(object sender, EventArgs e)
        {
            ToolStripMenuItem item=(ToolStripMenuItem)sender;
            Cursor.Current = Cursors.WaitCursor;
            Ultima.Animations.DefragAnim(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, (int)item.Tag);
            Cursor.Current = Cursors.Default;
            MessageBox.Show(String.Format("Saved to {0}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase),
                    "Save",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
        }
    }
}
