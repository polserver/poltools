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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ultima;
using System.IO;
using System.Drawing.Imaging;

namespace Controls
{
    public partial class ItemDetail : Form
    {
        public int index;
        private int defHue = -1;
        private bool partialHue = false;
        private Bitmap origbit;
        public ItemDetail(int i)
        {
            InitializeComponent();
            index = i;
        }
        private int DefHue
        {
            get { return defHue; }
            set
            {
                defHue = value;
                Bitmap huebit = new Bitmap(origbit);
                if (defHue >= 0)
                {
                    Hue hue = Ultima.Hues.List[defHue];
                    hue.ApplyTo(huebit, partialHue);
                }
                Bitmap newbit = new Bitmap(this.Graphic.Size.Width, this.Graphic.Size.Height);
                Graphics newgraph = Graphics.FromImage(newbit);
                newgraph.Clear(Color.FromArgb(-1));
                newgraph.DrawImage(huebit, (this.Graphic.Size.Width - huebit.Width) / 2, 0);

                this.Graphic.Image = newbit;
                this.Graphic.Refresh();
            }
        }

        
        private void onLoad(object sender, EventArgs e)
        {
            Ultima.ItemData item = Ultima.TileData.ItemTable[index];

            this.Text = String.Format("Item Detail 0x{0:X} '{1}'", index, item.Name);
            origbit = Ultima.Art.GetStatic(index);
            this.Size = new System.Drawing.Size(300, origbit.Size.Height + this.Data.Size.Height + 5);
            this.splitContainer1.SplitterDistance = origbit.Size.Height + 5;
            this.Graphic.Size = new System.Drawing.Size(300, origbit.Size.Height + 5);
            
            Bitmap newbit=new Bitmap(this.Graphic.Size.Width,this.Graphic.Size.Height);
            Graphics newgraph = Graphics.FromImage(newbit);
            newgraph.Clear(Color.FromArgb(-1));
            newgraph.DrawImage(origbit, (this.Graphic.Size.Width - origbit.Width) / 2, 0);
            
            this.Graphic.Image=newbit;
            
            this.Data.AppendText(String.Format("Name: {0}\n",item.Name));
            this.Data.AppendText(String.Format("Graphic: 0x{0:X4}\n", index));
            this.Data.AppendText(String.Format("Height/Capacity: {0}\n", item.Height));
            this.Data.AppendText(String.Format("Weight: {0}\n", item.Weight));
            this.Data.AppendText(String.Format("Animation: {0}\n",item.Animation));
            this.Data.AppendText(String.Format("Quality/Layer/Light: {0}\n",item.Quality));
            this.Data.AppendText(String.Format("Quantity: {0}\n",item.Quantity));
            this.Data.AppendText(String.Format("Hue: {0}\n", item.Hue));
            this.Data.AppendText(String.Format("StackingOffset/Unk4: {0}\n", item.StackingOffset));
            this.Data.AppendText(String.Format("Flags: {0}\n", item.Flags));
            if ((item.Flags & TileFlag.PartialHue) != 0)
                partialHue = true;
        }

        private void extract_Image_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, String.Format("Item 0x{0:X}.jpg", index));
            Bitmap bit = new Bitmap(origbit.Width, origbit.Height);
            Graphics newgraph = Graphics.FromImage(bit);
            newgraph.Clear(Color.FromArgb(-1));
            Bitmap huebit = new Bitmap(origbit);
            if (defHue > 0)
            {
                Hue hue = Ultima.Hues.List[defHue];
                hue.ApplyTo(huebit, partialHue);
            }
            newgraph.DrawImage(huebit,0,0);
            bit.Save(FileName, ImageFormat.Jpeg);
        }

        private HuePopUpItem showform = null;
        private void OnClick_Hue(object sender, EventArgs e)
        {
            if ((showform == null) || (showform.IsDisposed))
                showform = new HuePopUpItem(this, DefHue - 1);
            else
                showform.SetHue(DefHue - 1);
            showform.TopMost = true;
            showform.Show();
        }

        public void ChangeHue(int select)
        {
            DefHue = select;
        }
    }
}