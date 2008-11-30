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
using System.IO;

namespace Controls
{
    public partial class TileDatas : UserControl
    {
        public TileDatas()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            InitializeComponent();
            checkedListBox1.BeginUpdate();
            checkedListBox1.Items.Clear();
            string[] EnumNames=System.Enum.GetNames(typeof(TileFlag));
            for (int i = 1; i < EnumNames.Length; ++i)
            {
                checkedListBox1.Items.Add(EnumNames[i], false);
            }
            checkedListBox1.EndUpdate();
            checkedListBox2.BeginUpdate();
            checkedListBox2.Items.Clear();
            checkedListBox2.Items.Add(System.Enum.GetName(typeof(TileFlag), TileFlag.Damaging), false);
            checkedListBox2.Items.Add(System.Enum.GetName(typeof(TileFlag), TileFlag.Wet), false);
            checkedListBox2.Items.Add(System.Enum.GetName(typeof(TileFlag), TileFlag.Impassable), false);
            checkedListBox2.Items.Add(System.Enum.GetName(typeof(TileFlag), TileFlag.Wall), false);
            checkedListBox2.Items.Add(System.Enum.GetName(typeof(TileFlag), TileFlag.Unknown3), false);
            checkedListBox2.EndUpdate();
        }

        private bool Loaded = false;
        public void Reload()
        {
            if (Loaded)
                OnLoad(this, EventArgs.Empty);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            Loaded = true;
            treeViewItem.BeginUpdate();
            treeViewItem.Nodes.Clear();
            for (int i = 0; i < TileData.ItemTable.Length; ++i)
            {
                TreeNode node = new TreeNode(String.Format("0x{0:X4} {1}",i,TileData.ItemTable[i].Name));
                node.Tag=i;
                treeViewItem.Nodes.Add(node);
            }
            treeViewItem.EndUpdate();
            treeViewLand.BeginUpdate();
            treeViewLand.Nodes.Clear();
            for (int i = 0; i < TileData.LandTable.Length; ++i)
            {
                TreeNode node = new TreeNode(String.Format("0x{0:X4} {1}", i, TileData.LandTable[i].Name));
                node.Tag = i;
                treeViewLand.Nodes.Add(node);
            }
            treeViewLand.EndUpdate();
        }

        private void AfterSelectTreeViewItem(object sender, TreeViewEventArgs e)
        {
            int index=(int)e.Node.Tag;
            try
            {
                Bitmap bit = Ultima.Art.GetStatic(index);
                Bitmap newbit = new Bitmap(pictureBoxItem.Size.Width, pictureBoxItem.Size.Height);
                Graphics newgraph = Graphics.FromImage(newbit);
                newgraph.Clear(Color.FromArgb(-1));
                newgraph.DrawImage(bit, (pictureBoxItem.Size.Width - bit.Width) / 2, 1);
                pictureBoxItem.Image = newbit;
            }
            catch
            {
                pictureBoxItem.Image = new Bitmap(pictureBoxItem.Width, pictureBoxItem.Height);
            }
            ItemData data=TileData.ItemTable[index];
            textBoxName.Text = data.Name;
            textBoxAnim.Text = data.Animation.ToString();
            textBoxWeight.Text = data.Weight.ToString();
            textBoxQuality.Text = data.Quality.ToString();
            textBoxQuantity.Text = data.Quantity.ToString();
            textBoxHue.Text = data.Hue.ToString();
            textBoxStackOff.Text = data.StackingOffset.ToString();
            textBoxValue.Text = data.Value.ToString();
            textBoxHeigth.Text = data.Height.ToString();
            textBoxUnk1.Text = data.Unk1.ToString();
            textBoxUnk2.Text = data.Unk2.ToString();
            textBoxUnk3.Text = data.Unk3.ToString();
            Array EnumValues=System.Enum.GetValues(typeof(TileFlag));
            for (int i = 1; i < EnumValues.Length; i++)
            {
                if ((data.Flags & (TileFlag)EnumValues.GetValue(i))!=0)
                    checkedListBox1.SetItemChecked(i-1,true);
                else
                    checkedListBox1.SetItemChecked(i-1,false);
            }
        }

        private void AfterSelectTreeViewLand(object sender, TreeViewEventArgs e)
        {
            int index = (int)e.Node.Tag;
            try
            {
                Bitmap bit = Ultima.Art.GetLand(index);
                Bitmap newbit = new Bitmap(pictureBoxLand.Size.Width, pictureBoxLand.Size.Height);
                Graphics newgraph = Graphics.FromImage(newbit);
                newgraph.Clear(Color.FromArgb(-1));
                newgraph.DrawImage(bit, (pictureBoxLand.Size.Width - bit.Width) / 2, 1);
                pictureBoxLand.Image = newbit;
            }
            catch
            {
                pictureBoxLand.Image = new Bitmap(pictureBoxLand.Width, pictureBoxLand.Height);
            }
            LandData data = TileData.LandTable[index];
            textBoxNameLand.Text = data.Name;
            textBoxTexID.Text = data.TextureID.ToString();
            if ((data.Flags & TileFlag.Damaging) != 0)
                checkedListBox2.SetItemChecked(0, true);
            else
                checkedListBox2.SetItemChecked(0, false);
            if ((data.Flags & TileFlag.Wet) != 0)
                checkedListBox2.SetItemChecked(1, true);
            else
                checkedListBox2.SetItemChecked(1, false);
            if ((data.Flags & TileFlag.Impassable) != 0)
                checkedListBox2.SetItemChecked(2, true);
            else
                checkedListBox2.SetItemChecked(2, false);
            if ((data.Flags & TileFlag.Wall) != 0)
                checkedListBox2.SetItemChecked(3, true);
            else
                checkedListBox2.SetItemChecked(3, false);
            if ((data.Flags & TileFlag.Unknown3) != 0)
                checkedListBox2.SetItemChecked(4, true);
            else
                checkedListBox2.SetItemChecked(4, false);
        }

        private void OnClickSaveChangesItem(object sender, EventArgs e)
        {
            if (treeViewItem.SelectedNode == null)
                return;
            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            string name = textBoxName.Text;
            if (name.Length > 20)
                name = name.Substring(0, 20);
            item.Name = name;
            treeViewItem.SelectedNode.Text = String.Format("0x{0:X4} {1}", index, name);
            byte byteres;
            short shortres;
            if (short.TryParse(textBoxAnim.Text, out shortres))
                item.Animation = shortres;
            if (byte.TryParse(textBoxWeight.Text, out byteres))
                item.Weight = byteres;
            if (byte.TryParse(textBoxQuality.Text, out byteres))
                item.Quality = byteres;
            if (byte.TryParse(textBoxQuantity.Text, out byteres))
                item.Quantity = byteres;
            if (byte.TryParse(textBoxHue.Text, out byteres))
                item.Hue = byteres;
            if (byte.TryParse(textBoxStackOff.Text, out byteres))
                item.StackingOffset = byteres;
            if (byte.TryParse(textBoxValue.Text, out byteres))
                item.Value = byteres;
            if (byte.TryParse(textBoxHeigth.Text, out byteres))
                item.Height = byteres;
            if (short.TryParse(textBoxUnk1.Text, out shortres))
                item.Unk1 = shortres;
            if (byte.TryParse(textBoxUnk2.Text, out byteres))
                item.Unk2 = byteres;
            if (byte.TryParse(textBoxUnk3.Text, out byteres))
                item.Unk3 = byteres;
            item.Flags = TileFlag.None;
            Array EnumValues=System.Enum.GetValues(typeof(TileFlag));
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                    item.Flags |= (TileFlag)EnumValues.GetValue(i + 1);
            }
            TileData.ItemTable[index] = item;
        }

        private void OnClickSaveTiledata(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, "tiledata.mul");
            Ultima.TileData.SaveTileData(FileName);
            MessageBox.Show(String.Format("TileData saved to {0}", FileName), "Saved");
        }

        private void OnClickExportItems(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, "ItemData.csv");
            Ultima.TileData.ExportItemDataToCSV(FileName);
            MessageBox.Show(String.Format("ItemData saved to {0}", FileName), "Saved");
        }

        private void OnClickSaveChangesLand(object sender, EventArgs e)
        {
            if (treeViewLand.SelectedNode == null)
                return;
            int index = (int)treeViewLand.SelectedNode.Tag;
            LandData land = TileData.LandTable[index];
            string name = textBoxNameLand.Text;
            if (name.Length > 20)
                name = name.Substring(0, 20);
            land.Name = name;
            treeViewLand.SelectedNode.Text = String.Format("0x{0:X4} {1}", index, name);
            short shortres;
            if (short.TryParse(textBoxTexID.Text, out shortres))
                land.TextureID = shortres;
            land.Flags = TileFlag.None;
            if (checkedListBox2.GetItemChecked(0))
                land.Flags |= TileFlag.Damaging;
            if (checkedListBox2.GetItemChecked(1))
                land.Flags |= TileFlag.Wet;
            if (checkedListBox2.GetItemChecked(2))
                land.Flags |= TileFlag.Impassable;
            if (checkedListBox2.GetItemChecked(3))
                land.Flags |= TileFlag.Wall;
            if (checkedListBox2.GetItemChecked(4))
                land.Flags |= TileFlag.Unknown3;

            TileData.LandTable[index] = land;
        }

        private void OnClickExportLand(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, "LandData.csv");
            Ultima.TileData.ExportLandDataToCSV(FileName);
            MessageBox.Show(String.Format("LandData saved to {0}", FileName), "Saved");
        }
    }
}
