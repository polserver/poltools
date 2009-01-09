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
using System.Windows.Forms;
using System.IO;

namespace FiddlerControls
{
    public partial class Sounds : UserControl
    {
        public Sounds()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private System.Media.SoundPlayer sp;

        private bool Loaded = false;

        /// <summary>
        /// ReLoads if loaded
        /// </summary>
        public void Reload()
        {
            if (!Loaded)
                return;
            checkBox.Checked = false;
            OnLoad(this, EventArgs.Empty);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            Loaded = true;
            string name = "";
            treeView.BeginUpdate();
            treeView.Nodes.Clear();
            for (int i = 1; i <= 0xFFF; i++)
            {
                if (Ultima.Sounds.IsValidSound(i - 1,out name))
                {
                    TreeNode node = new TreeNode(String.Format("0x{0:X3} {1}", i, name));
                    node.Tag = i;
                    treeView.Nodes.Add(node);
                }
            }
            treeView.EndUpdate();
            if (treeView.Nodes.Count > 0)
                treeView.SelectedNode = treeView.Nodes[0];
            sp = new System.Media.SoundPlayer();
            this.Cursor = Cursors.Default;
        }

        private void PlaySound(object sender, EventArgs e)
        {
            sp.Stop();
            if (treeView.SelectedNode == null)
                return;
            sp.Stream = Ultima.Sounds.GetSound((int)treeView.SelectedNode.Tag - 1).WAVEStream;
            sp.Play();
        }

        private void OnDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            sp.Stop();
            if (treeView.SelectedNode == null)
                return;
            sp.Stream = Ultima.Sounds.GetSound((int)e.Node.Tag - 1).WAVEStream;
            sp.Play();
        }

        private void afterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;
            seconds.Text = String.Format("{0:f}s", Ultima.Sounds.GetSoundLength((int)treeView.SelectedNode.Tag-1));
        }

        private void OnChangeSort(object sender, EventArgs e)
        {
            string delimiter= " ";
            char[] delim = delimiter.ToCharArray();
            string [] name;
            treeView.BeginUpdate();
            for (int i = 0; i < treeView.Nodes.Count; i++)
            {
                name = treeView.Nodes[i].Text.Split(delim);
                treeView.Nodes[i].Text = String.Format("{0} {1} ", name[1], name[0]);
            }
            treeView.Sort();
            treeView.EndUpdate();
        }

        private void SearchName(object sender, EventArgs e)
        {
            bool res = DoSearchName(textBox1.Text, false);
            if (!res)
                MessageBox.Show("No sound found", "Result", MessageBoxButtons.OK,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1);
        }

        private void SearchNext(object sender, EventArgs e)
        {
            bool res = DoSearchName(textBox1.Text, true);
            if (!res)
                MessageBox.Show("No sound found", "Result", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        private bool DoSearchName(string name, bool next)
        {
            int index = 0;
            if (next)
            {
                if (treeView.SelectedNode.Index>=0)
                    index = treeView.SelectedNode.Index + 1;
                if (index >= treeView.Nodes.Count)
                    index = 0;
            }

            for (int i = index; i < treeView.Nodes.Count; i++)
            {
                TreeNode node = treeView.Nodes[i];
                if (node.Text.Contains(name))
                {
                    treeView.SelectedNode = node;
                    node.EnsureVisible();
                    return true;
                }
            }
            return false;
        }

        private void OnClickExtract(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;
            int id = (int)treeView.SelectedNode.Tag - 1;
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string name ="";
            Ultima.Sounds.IsValidSound(id,out name);
            string FileName = Path.Combine(path, String.Format("{0}",name));
            MemoryStream stream= Ultima.Sounds.GetSound(id).WAVEStream;
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write);
            stream.WriteTo(fs);
            fs.Close();
            stream.Dispose();
            MessageBox.Show(String.Format("Sound saved to {0}", FileName), "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void OnClickSave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            Ultima.Sounds.Save(path);
            this.Cursor = Cursors.Default;
            MessageBox.Show(
                    String.Format("Saved to {0}", path),
                    "Save",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
        }

        private void OnClickRemove(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;
            int id = (int)treeView.SelectedNode.Tag - 1;
            DialogResult result =
                        MessageBox.Show(String.Format("Are you sure to remove {0}", treeView.SelectedNode.Text), "Remove",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                Ultima.Sounds.Remove(id);
                treeView.SelectedNode.Remove();
            }
        }

        private void OnClickAddReplace(object sender, EventArgs e)
        {
            int id;
            string convert;
            bool candone;
            if (textBoxID.Text.Contains("0x"))
            {
                convert = textBoxID.Text.Replace("0x", "");
                candone = int.TryParse(convert, System.Globalization.NumberStyles.HexNumber, null, out id);
            }
            else
                candone = int.TryParse(textBoxID.Text, System.Globalization.NumberStyles.Integer, null, out id);
            if ((id > 0xFFF) || (id<1))
                candone = false;

            if (candone)
            {
                string name = textBoxName.Text;
                if (name != null)
                {
                    if (name.Length > 40)
                        name = name.Substring(0, 40);
                    string filename = textBoxWav.Text;
                    if (File.Exists(filename))
                    {
                        Ultima.Sounds.Add(id-1, name, filename);

                        TreeNode node = new TreeNode(String.Format("0x{0:X3} {1}", id, name));
                        if (checkBox.Checked)
                            node.Text = String.Format("{1} 0x{0:X3}", id, name);
                        node.Tag = id;
                        bool done = false;
                        for (int i = 0; i < treeView.Nodes.Count; i++)
                        {
                            if ((int)treeView.Nodes[i].Tag == id)
                            {
                                done = true;
                                treeView.Nodes.RemoveAt(i);
                                treeView.Nodes.Insert(i,node);
                                break;
                            }
                        }
                        if (!done)
                        {
                            treeView.Nodes.Add(node);
                            treeView.Sort();
                        }

                        node.EnsureVisible();
                        treeView.SelectedNode = node;
                        treeView.Refresh();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Invalid Filename",
                            "Add/Replace",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Invalid Name",
                        "Add/Replace",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
                }

            }
            else
            {
                MessageBox.Show(
                    "Invalid ID",
                    "Add/Replace",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
        }

        private void OnClickSelectWav(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "Choose wave file to add";
            dialog.CheckFileExists = true;
            dialog.Filter = "wav file (*.wav)|*.wav";
            if (dialog.ShowDialog() == DialogResult.OK)
                textBoxWav.Text = dialog.FileName;
        }
    }
}
