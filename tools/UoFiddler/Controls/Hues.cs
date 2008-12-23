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

namespace Controls
{
    public partial class Hues : UserControl
    {
        public Hues()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }
        private int selected = 0;
        private bool Loaded = false;

        /// <summary>
        /// Sets Selected Hue
        /// </summary>
        public int Selected
        {
            get { return listBox.SelectedIndex; }
            set
            {
                selected = value;
                if (listBox.Items.Count > 0)
                    listBox.SelectedIndex = value;
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
            this.Cursor = Cursors.AppStarting;
            Loaded = true;
            listBox.BeginUpdate();
            listBox.Items.Clear();
            listBox.DataSource = Ultima.Hues.List;
            //listBox.Items.AddRange(Ultima.Hues.List);
            listBox.EndUpdate();
            listBox.SelectedIndex = selected;
            this.Cursor = Cursors.Default;
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, 200, listBox.ItemHeight);
            if ((e.State & DrawItemState.Selected) > DrawItemState.None)
                e.Graphics.FillRectangle(SystemBrushes.Highlight, rect);
            else
                e.Graphics.FillRectangle(SystemBrushes.Window, rect);

            float num2 = ((float)(e.Bounds.Width - 200)) / 32;
            Hue hue = (Hue)listBox.Items[e.Index];

            Rectangle stringrect = new Rectangle((e.Bounds.X + 3), e.Bounds.Y, e.Bounds.Width, listBox.ItemHeight);
            e.Graphics.DrawString(String.Format("{0,-5} {1,-7} {2}",hue.Index+1, String.Format("(0x{0:X})",hue.Index+1), hue.Name), e.Font, Brushes.Black, stringrect);

            for (int i=0;i<hue.Colors.Length;i++)
            {
                Rectangle rectangle = new Rectangle((e.Bounds.X + 200) + ((int)Math.Round((double)(i * num2))), e.Bounds.Y, (int)Math.Round((double)(num2 + 1f)), e.Bounds.Height);
                e.Graphics.FillRectangle(new SolidBrush(hue.GetColor(i)), rectangle);
            }
        }
    }
}
