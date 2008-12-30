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
using Ultima;
using System.Windows.Forms.VisualStyles;

namespace FiddlerControls
{
    public partial class Hues : UserControl
    {
        public Hues()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            pictureBox.Image = bmp;
            pictureBox.MouseWheel += new MouseEventHandler(OnMouseWheel);
        }
        private const int ITEMHEIGHT = 20;
        private int selected=0;
        private bool Loaded = false;
        private Bitmap bmp;
        private int row;

        /// <summary>
        /// Sets Selected Hue
        /// </summary>
        public int Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                if (Loaded)
                {
                    if (Ultima.Hues.List.Length > 0)
                        PaintBox();
                }
            }
        }

        /// <summary>
        /// Reload when loaded (file changed)
        /// </summary>
        public void Reload()
        {
            if (!Loaded)
                return;
            selected = 0;
            OnLoad(this, EventArgs.Empty);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Loaded = true;
            vScrollBar.Maximum = Ultima.Hues.List.Length;
            vScrollBar.Minimum = 0;
            vScrollBar.Value = 0;
            vScrollBar.SmallChange = 1;
            vScrollBar.LargeChange = 10;
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            
            PaintBox();
        }

        private int GetIndex(int y)
        {
            int value = vScrollBar.Value +y;
            if (Ultima.Hues.List.Length > value)
                return value;
            else
                return -1;
        }

        private void PaintBox()
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                for (int y = 0; y <= row; y++)
                {
                    int index = GetIndex(y);
                    if (index >= 0)
                    {
                        Rectangle rect = new Rectangle(0, y * ITEMHEIGHT, 200, ITEMHEIGHT);
                        if (index == selected)
                            g.FillRectangle(SystemBrushes.Highlight, rect);
                        else
                            g.FillRectangle(SystemBrushes.Window, rect);

                        float num2 = ((float)(pictureBox.Width - 200)) / 32;
                        Hue hue = Ultima.Hues.List[index];
                        Rectangle stringrect = new Rectangle(3, y * ITEMHEIGHT, pictureBox.Width, ITEMHEIGHT);
                        g.DrawString(String.Format("{0,-5} {1,-7} {2}", hue.Index + 1, String.Format("(0x{0:X})", hue.Index + 1), hue.Name), Font, Brushes.Black, stringrect);

                        for (int i = 0; i < hue.Colors.Length; i++)
                        {
                            Rectangle rectangle = new Rectangle(200 + ((int)Math.Round((double)(i * num2))), y * ITEMHEIGHT, (int)Math.Round((double)(num2 + 1f)), ITEMHEIGHT);
                            g.FillRectangle(new SolidBrush(hue.GetColor(i)), rectangle);
                        }
                    }
                }
            }
            pictureBox.Image = bmp;
            pictureBox.Update();
        }

        private void onScroll(object sender, ScrollEventArgs e)
        {
            PaintBox();
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                if (vScrollBar.Value < vScrollBar.Maximum)
                {
                    vScrollBar.Value++;
                    PaintBox();
                }
            }
            else
            {
                if (vScrollBar.Value > 1)
                {
                    vScrollBar.Value--;
                    PaintBox();
                }
            }
        }

        private void OnResize(object sender, EventArgs e)
        {
            row = pictureBox.Height / ITEMHEIGHT;
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            PaintBox();
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            pictureBox.Focus();
            Point m = PointToClient(Control.MousePosition);
            int index = GetIndex(m.Y / ITEMHEIGHT);
            if (index >= 0)
                Selected = index;
        }

        /// <summary>
        /// Print a nice border
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int borderWidth = 1;

            Color borderColor = VisualStyleInformation.TextControlBorder;
            if (borderColor == null)
                borderColor = Color.LightBlue;
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, borderColor,
                      borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth,
                      ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid,
                      borderColor, borderWidth, ButtonBorderStyle.Solid);
        } 
    }
}
