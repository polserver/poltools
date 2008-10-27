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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ultima;

namespace Controls
{
    public partial class Fonts : UserControl
    {
        public Fonts()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            treeView.Nodes.Clear();
            treeView.BeginUpdate();
            TreeNode node = new TreeNode("ASCII");
            node.Tag = 0;
            treeView.Nodes.Add(node);
            for (int i = 0; i < ASCIIText.Fonts.Length; i++)
            {
                node = new TreeNode(i.ToString());
                node.Tag = i;
                treeView.Nodes[0].Nodes.Add(node);
            }
            node = new TreeNode("Unicode");
            node.Tag = 1;
            treeView.Nodes.Add(node);
            for (int i = 0; i < 7; i++)
            {
                node = new TreeNode(i.ToString());
                node.Tag = i;
                treeView.Nodes[1].Nodes.Add(node);
            }
            treeView.ExpandAll();
            treeView.EndUpdate();
            treeView.SelectedNode = treeView.Nodes[0].Nodes[0];
            
        }

        private void onSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView.SelectedNode.Parent == null)
                treeView.SelectedNode = treeView.SelectedNode.Nodes[0];

            int font = (int)treeView.SelectedNode.Tag;
            listView1.Clear();
            listView1.BeginUpdate();
            if ((int)treeView.SelectedNode.Parent.Tag == 1)
            {
                for (int i = 0; i < 230; i++)
                {
                    ListViewItem item = new ListViewItem(i.ToString(), 0);
                    item.Tag = UnicodeFonts.GetCharImage(font, i);
                    listView1.Items.Add(item);
                }
            }
            else
            {
                for (int i = 0; i < ASCIIText.Fonts[font].Characters.Length; i++)
                {
                    ListViewItem item = new ListViewItem(i.ToString(), 0);
                    item.Tag = ASCIIText.Fonts[font].Characters[i];
                    listView1.Items.Add(item);
                }
            }
            listView1.TileSize = new Size(30, 30);
            listView1.EndUpdate();
        }

        private void drawitem(object sender, DrawListViewItemEventArgs e)
        {
            int i = int.Parse(e.Item.Text.ToString());
            Bitmap bmp = (Bitmap)e.Item.Tag;
            
            if (bmp != null)
            {
                int width = bmp.Width;
                int height = bmp.Height;

                if (width > e.Bounds.Width)
                    width = e.Bounds.Width - 2;

                if (height > e.Bounds.Height)
                    height = e.Bounds.Height - 2;


                char c = (char)(i+ 32);
                e.Graphics.DrawString(c.ToString(), Fonts.DefaultFont, Brushes.Gray, e.Bounds.X + e.Bounds.Width / 2, e.Bounds.Y + e.Bounds.Height / 2);
                e.Graphics.DrawImage(bmp, new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, width, height));
                e.Graphics.DrawRectangle(new Pen(Color.Gray), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }
        }

        private void onSelectChar(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                toolStripStatusLabel1.Text = " : ()";
            else
            {
                int i = int.Parse(listView1.SelectedItems[0].Text.ToString());
                i+=32;
                toolStripStatusLabel1.Text = String.Format("'{0}' : {1} (0x{2:X})",(char)i,i,i);
            }
        }
    }
}
