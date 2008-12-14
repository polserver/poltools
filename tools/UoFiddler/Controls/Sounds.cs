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

namespace Controls
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
            MessageBox.Show(String.Format("Sound saved to {0}", FileName), "Saved", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }
    }
}
