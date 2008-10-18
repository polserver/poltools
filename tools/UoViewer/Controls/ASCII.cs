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
    public partial class ASCII : UserControl
    {
        public ASCII()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private static Brush borderBrush = Brushes.Gray;
        private Pen borderPen = new Pen(borderBrush);
        private int i;
        private Bitmap bmp;
        private int font = 0;

        private void OnLoad(object sender, EventArgs e)
        {
            treeView.Nodes.Clear();
            TreeNode node = new TreeNode("ASCII");
            node.Tag = 0;
            treeView.Nodes.Add(node);
            for (i = 0; i < ASCIIText.Fonts.Length; i++)
            {
                node = new TreeNode(i.ToString());
                node.Tag = i;
                treeView.Nodes[0].Nodes.Add(node);
            }
            treeView.SelectedNode = treeView.Nodes[0].Nodes[0];
        }

        private void onSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView.SelectedNode.Parent == null)
                treeView.SelectedNode = treeView.SelectedNode.Nodes[0];

            font = (int)treeView.SelectedNode.Tag;

            listView1.Clear();
            listView1.BeginUpdate();
            for (i = 0; i < ASCIIText.Fonts[font].Characters.Length; i++)
            {
                ListViewItem item = new ListViewItem(i.ToString(), 0);
                item.Tag = ASCIIText.Fonts[font].Characters[i];
                listView1.Items.Add(item);
            }
            listView1.TileSize = new Size(30, 30);
            listView1.EndUpdate();
        }

        private void drawitem(object sender, DrawListViewItemEventArgs e)
        {
            i = int.Parse(e.Item.Text.ToString());
            bmp = (Bitmap)e.Item.Tag;
            
            if (bmp != null)
            {
                int width = bmp.Width;
                int height = bmp.Height;

                if (width > e.Bounds.Width)
                    width = e.Bounds.Width - 2;

                if (height > e.Bounds.Height)
                    height = e.Bounds.Height - 2;


                char c = (char)(i+ 32);
                e.Graphics.DrawString(c.ToString(), ASCII.DefaultFont, Brushes.LightGray, e.Bounds.X + e.Bounds.Width / 2, e.Bounds.Y + e.Bounds.Height / 2);

                e.Graphics.DrawImage(bmp, new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, width, height));

                e.Graphics.DrawRectangle(borderPen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }
        }
    }
}
