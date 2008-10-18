using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ultima;

namespace Controls
{
    public partial class Light : UserControl
    {
        public Light()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            treeView1.BeginUpdate();
            for (int i = 0; i <= Ultima.Light.GetCount(); i++)
            {
                if (Ultima.Light.TestLight(i))
                {
                    TreeNode treeNode = new TreeNode(i.ToString());
                    treeNode.Tag = Ultima.Light.GetLight(i);
                    treeView1.Nodes.Add(treeNode);
                }
            }
            treeView1.EndUpdate();
            treeView1.SelectedNode = treeView1.Nodes[0];
            pictureBox1.BackgroundImage = (Bitmap)treeView1.Nodes[0].Tag;
        }

        private void AfterSelect(object sender, TreeViewEventArgs e)
        {
            pictureBox1.BackgroundImage = (Bitmap)treeView1.SelectedNode.Tag;
        }
    }
}
