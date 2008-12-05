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
using System.Windows.Forms;

namespace Controls
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
            if (treeView1.Nodes.Count > 0)
                treeView1.SelectedNode = treeView1.Nodes[0];
            this.Cursor = Cursors.Default;
        }

        private void AfterSelect(object sender, TreeViewEventArgs e)
        {
            pictureBox1.BackgroundImage = (Bitmap)treeView1.SelectedNode.Tag;
        }
    }
}
