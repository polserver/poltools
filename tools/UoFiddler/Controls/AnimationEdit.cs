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
            FileType = 1;
            CurrDir = 0;
            toolStripComboBox1.SelectedIndex = 0;
            FramePoint = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
        }

        private Frame[] CurrFrames;
        private int FileType;
        private int CurrDir;
        private Point FramePoint;

        private void onLoad(object sender, EventArgs e)
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();

            int count = Animations.GetAnimCount(FileType); ;

            for (int i = 0; i < count; i++)
            {
                if (Animations.IsAnimDefinied(i,0,0,FileType))
                {
                    int animlength = Animations.GetAnimLength(i, FileType);
                    string type = animlength == 22 ? "H" : animlength == 13 ? "L" : "P";
                    TreeNode node = new TreeNode();
                    node.Tag = i;
                    node.Text = String.Format("{0}: {1} ({2})",type,i,BodyConverter.GetTrueBody(FileType,i));
                    treeView1.Nodes.Add(node);
                    for (int j = 0; j < animlength; j++)
                    {
                        if (Animations.IsAnimDefinied(i, j, 0, FileType))
                        {
                            TreeNode subnode = new TreeNode();
                            subnode.Tag = j;
                            subnode.Text = j.ToString();
                            node.Nodes.Add(subnode);
                        }
                    }
                }
            }
            treeView1.EndUpdate();
            if (treeView1.Nodes.Count > 0)
                treeView1.SelectedNode = treeView1.Nodes[0];
        }

        private void AfterSelectTreeView(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
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
                CurrFrames = Animations.GetAnimation(body, action, CurrDir, FileType);
                listView1.BeginUpdate();
                listView1.Clear();
                if (CurrFrames != null)
                {
                    int width = 80;
                    int height = 110;
                    for (int i = 0; i < CurrFrames.Length; i++)
                    {
                        if (CurrFrames[i].Bitmap == null)
                            continue;
                        ListViewItem item;
                        item = new ListViewItem(i.ToString(), 0);
                        item.Tag = i;
                        listView1.Items.Add(item);
                        if (CurrFrames[i].Bitmap.Width > width)
                            width = CurrFrames[i].Bitmap.Width;
                        if (CurrFrames[i].Bitmap.Height > height)
                            height = CurrFrames[i].Bitmap.Height;
                    }
                    listView1.TileSize = new Size(width + 5, height + 5);
                    trackBar2.Maximum = CurrFrames.Length - 1;
                    trackBar2.Value = 0;

                    numericUpDownCx.Value = CurrFrames[trackBar2.Value].Center.X;
                    numericUpDownCy.Value = CurrFrames[trackBar2.Value].Center.Y;
                }
                listView1.EndUpdate();
                pictureBox1.Refresh();

            }
        }

        private void DrawFrameItem(object sender, DrawListViewItemEventArgs e)
        {
            Bitmap bmp = CurrFrames[(int)e.Item.Tag].Bitmap;
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
            for (int i = 0; i < CurrFrames.Length; i++)
            {
                string FileName = Path.Combine(path, String.Format("Animation {0} {1}-{2}.tiff", body, action,i));
                CurrFrames[i].Bitmap.Save(FileName, ImageFormat.Tiff);
            }
            MessageBox.Show(
                    String.Format("Frames saved to {0}",path),
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
            e.Graphics.Clear(Color.LightGray);
            e.Graphics.DrawLine(Pens.Black, new Point(FramePoint.X, 0), new Point(FramePoint.X, pictureBox1.Height));
            e.Graphics.DrawLine(Pens.Black, new Point(0, FramePoint.Y), new Point(pictureBox1.Width, FramePoint.Y));
            if (CurrFrames.Length > 0)
            {
                if (CurrFrames[trackBar2.Value].Bitmap != null)
                {
                    int x = FramePoint.X - CurrFrames[trackBar2.Value].Center.X;
                    int y = FramePoint.Y - CurrFrames[trackBar2.Value].Center.Y - CurrFrames[trackBar2.Value].Bitmap.Height;
                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(x, y, CurrFrames[trackBar2.Value].Bitmap.Width, CurrFrames[trackBar2.Value].Bitmap.Height));
                    e.Graphics.DrawImage(CurrFrames[trackBar2.Value].Bitmap, x, y);
                }
            }
        }

        private void onFrameCountBarChanged(object sender, EventArgs e)
        {
            numericUpDownCx.Value = CurrFrames[trackBar2.Value].Center.X;
            numericUpDownCy.Value = CurrFrames[trackBar2.Value].Center.Y;
            pictureBox1.Refresh();
        }

        private void OnCenterXValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownCx.Value != CurrFrames[trackBar2.Value].Center.X)
            {
                CurrFrames[trackBar2.Value].Center = new Point((int)numericUpDownCx.Value, CurrFrames[trackBar2.Value].Center.Y);
                pictureBox1.Refresh();
            }
        }

        private void OnCenterYValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownCy.Value != CurrFrames[trackBar2.Value].Center.Y)
            {
                CurrFrames[trackBar2.Value].Center = new Point(CurrFrames[trackBar2.Value].Center.X, (int)numericUpDownCy.Value);
                pictureBox1.Refresh();
            }
        }
    }
}
