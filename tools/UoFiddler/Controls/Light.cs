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

namespace FiddlerControls
{
    public partial class Light : UserControl
    {
        public Light()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private bool Loaded = false;

        /// <summary>
        /// ReLoads if loaded
        /// </summary>
        public void Reload()
        {
            if (Loaded)
                OnLoad(this, EventArgs.Empty);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            Loaded = true;
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            for (int i = 0; i < Ultima.Light.GetCount(); i++)
            {
                if (Ultima.Light.TestLight(i))
                {
                    TreeNode treeNode = new TreeNode(i.ToString());
                    treeNode.Tag = i;
                    treeView1.Nodes.Add(treeNode);
                }
            }
            treeView1.EndUpdate();
            if (treeView1.Nodes.Count > 0)
                treeView1.SelectedNode = treeView1.Nodes[0];
            this.Cursor = Cursors.Default;
        }

        private void AfterSelect(object sender, TreeViewEventArgs e)
        {
            pictureBox1.Image = Ultima.Light.GetLight((int)treeView1.SelectedNode.Tag);
        }

        private void OnClickExport(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            int i = (int)treeView1.SelectedNode.Tag;
            string FileName = Path.Combine(path, String.Format("Light {0}.tiff", i));
            Bitmap bmp = Ultima.Light.GetLight(i);
            bmp.Save(FileName, ImageFormat.Tiff);
            MessageBox.Show(
                String.Format("Light saved to {0}", FileName),
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void OnClickRemove(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;
            int i = (int)treeView1.SelectedNode.Tag;
            DialogResult result =
                        MessageBox.Show(String.Format("Are you sure to remove {0} (0x{0:X})", i),
                        "Save",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                Ultima.Light.Remove(i);
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                treeView1.Invalidate();
            }
        }

        private void OnClickReplace(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Title = "Choose image file to replace";
                dialog.CheckFileExists = true;
                dialog.Filter = "image file (*.tiff)|*.tiff";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(dialog.FileName);
                    int i = (int)treeView1.SelectedNode.Tag;
                    Ultima.Light.Replace(i, bmp);
                    treeView1.Invalidate();
                    AfterSelect(this, (TreeViewEventArgs)null);
                }
            }
        }

        private void OnTextChangedInsert(object sender, EventArgs e)
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

            if (index > 99)
                candone = false;
            if (candone)
            {
                if (Ultima.Light.TestLight(index))
                    InsertText.ForeColor = Color.Red;
                else
                    InsertText.ForeColor = Color.Black;
            }
            else
                InsertText.ForeColor = Color.Red;
        }

        private void OnKeyDownInsert(object sender, KeyEventArgs e)
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
                if (index > 99)
                    candone = false;
                if (candone)
                {
                    if (Ultima.Light.TestLight(index))
                        return;
                    contextMenuStrip1.Close();
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Multiselect = false;
                    dialog.Title = String.Format("Choose image file to insert at {0} (0x{0:X})", index);
                    dialog.CheckFileExists = true;
                    dialog.Filter = "image file (*.tiff)|*.tiff";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Bitmap bmp = new Bitmap(dialog.FileName);
                        Ultima.Light.Replace(index,bmp);
                        TreeNode treeNode = new TreeNode(index.ToString());
                        treeNode.Tag = index;
                        bool done=false;
                        foreach (TreeNode node in treeView1.Nodes)
                        {
                            if ((int)node.Tag>index)
                            {
                                treeView1.Nodes.Insert(node.Index,treeNode);
                                done=true;
                                break;
                            }
                        }
                        if (!done)
                            treeView1.Nodes.Add(treeNode);
                        treeView1.Invalidate();
                        treeView1.SelectedNode = treeNode;
                    }
                }
            }
        }

        private void OnClickSave(object sender, EventArgs e)
        {
            Ultima.Light.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
            MessageBox.Show(
                    String.Format("Saved to {0}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase),
                    "Save",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
        }
    }
}
