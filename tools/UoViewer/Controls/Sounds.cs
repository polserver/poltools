using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ultima;
using System.Media;

namespace Controls
{
    public partial class Sounds : UserControl
    {
        public Sounds()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private System.Media.SoundPlayer sp = new System.Media.SoundPlayer();

        private void OnLoad(object sender, EventArgs e)
        {
            string name = "";
            treeView.BeginUpdate();
            for (int i = 1; i <= 0xFFF; i++)
            {
                name = Ultima.Sounds.IsValidSound(i-1);
                if (name != "")
                {
                    TreeNode node = new TreeNode(String.Format("0x{0:X3} {1}", i, name));
                    node.Tag = i;
                    treeView.Nodes.Add(node);
                }
            }
            treeView.EndUpdate();
            treeView.SelectedNode = treeView.Nodes[0];
        }

        private void PlaySound(object sender, EventArgs e)
        {
            sp.Stop();
            if (treeView.SelectedNode == null)
                return;
            sp.Stream = Ultima.Sounds.GetSound((int)treeView.SelectedNode.Tag - 1).WAVEStream;
            sp.Play();
        }

        private void OnChangeSort(object sender, EventArgs e)
        {
            int i;
            string delimiter= " ";
            char[] delim = delimiter.ToCharArray();
            string [] name;
            treeView.BeginUpdate();
            for (i = 0; i < treeView.Nodes.Count; i++)
            {
                name = treeView.Nodes[i].Text.Split(delim);
                treeView.Nodes[i].Text = String.Format("{0} {1} ", name[1], name[0]);
            }
            treeView.Sort();
            treeView.EndUpdate();
        }

        private void afterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;
            seconds.Text = String.Format("{0}s", Ultima.Sounds.GetSoundLength((int)treeView.SelectedNode.Tag) - 1);
        }
    }
}
