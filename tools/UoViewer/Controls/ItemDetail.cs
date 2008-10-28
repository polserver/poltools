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
        private bool animate = false;
        private Timer m_Timer;
        int frame;
        Animdata.Data info;
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
                if (!animate)
                {
                    Bitmap huebit = new Bitmap(Ultima.Art.GetStatic(index));
                    if (defHue >= 0)
                    {
                        Hue hue = Ultima.Hues.List[defHue];
                        hue.ApplyTo(huebit, partialHue);
                    }
                    SetPicture(huebit);
                }
            }
        }

        private void SetPicture(Bitmap bit)
        {
            Bitmap newbit = new Bitmap(this.Graphic.Size.Width, this.Graphic.Size.Height);
            Graphics newgraph = Graphics.FromImage(newbit);
            newgraph.Clear(Color.FromArgb(-1));
            newgraph.DrawImage(bit, (this.Graphic.Size.Width - bit.Width) / 2, 5);

            this.Graphic.Image = newbit;
        }
        
        private void onLoad(object sender, EventArgs e)
        {
            this.animateToolStripMenuItem.Visible = false;
            Ultima.ItemData item = Ultima.TileData.ItemTable[index];

            this.Text = String.Format("Item Detail 0x{0:X} '{1}'", index, item.Name);
            this.Size = new System.Drawing.Size(300, Ultima.Art.GetStatic(index).Size.Height + this.Data.Size.Height + 10);
            this.splitContainer1.SplitterDistance = Ultima.Art.GetStatic(index).Size.Height + 10;
            this.Graphic.Size = new System.Drawing.Size(300, Ultima.Art.GetStatic(index).Size.Height + 10);
            SetPicture(Ultima.Art.GetStatic(index));
            
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
            if ((item.Flags & TileFlag.Animation) != 0 )
            {
                info = Animdata.GetAnimData(index);
                if (info != null)
                {
                    this.animateToolStripMenuItem.Visible = true;
                    this.Data.AppendText(String.Format("Animation FrameCount: {0} Interval: {1}\n", info.FrameCount,info.FrameInterval));
                }
            }
        }

        private void AnimTick(object sender, EventArgs e)
        {
            frame++;
            if (frame >= info.FrameCount)
                frame = 0;

            Bitmap animbit = Ultima.Art.GetStatic(index+info.FrameData[frame]);
            if (defHue >= 0)
            {
                Hue hue = Ultima.Hues.List[defHue];
                hue.ApplyTo(animbit, partialHue);
            }
            SetPicture(animbit);
        }

        private void extract_Image_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, String.Format("Item 0x{0:X}.jpg", index));
            Bitmap bit = new Bitmap(Ultima.Art.GetStatic(index).Width, Ultima.Art.GetStatic(index).Height);
            Graphics newgraph = Graphics.FromImage(bit);
            newgraph.Clear(Color.FromArgb(-1));
            Bitmap huebit = new Bitmap(Ultima.Art.GetStatic(index));
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

        private void OnClickAnimate(object sender, EventArgs e)
        {
            animate = !animate;
            if (animate)
            {
                m_Timer = new Timer();
                frame = -1;
                m_Timer.Interval = 100 * info.FrameInterval;
                m_Timer.Tick += new EventHandler(AnimTick);
                m_Timer.Start();
            }
            else
            {
                if (m_Timer.Enabled)
                    m_Timer.Stop();

                m_Timer.Dispose();
                m_Timer = null;
                SetPicture(Ultima.Art.GetStatic(index));
            }
        }

        private void onClose(object sender, FormClosingEventArgs e)
        {
            if (m_Timer != null)
            {
                if (m_Timer.Enabled)
                    m_Timer.Stop();

                m_Timer.Dispose();
                m_Timer = null;
            }
        }
    }
}