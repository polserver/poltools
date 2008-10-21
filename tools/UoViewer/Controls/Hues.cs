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
    public partial class Hues : UserControl
    {
        public Hues()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }
        private int Selected=0;
        private void OnLoad(object sender, EventArgs e)
        {
            listBox.BeginUpdate();
            listBox.Items.Clear();
            foreach (Hue hue in Ultima.Hues.List)
            {
                listBox.Items.Add(hue);
            }
            listBox.EndUpdate();
            listBox.SelectedIndex = Selected;
        }

        public int GetSelect()
        {
            return listBox.SelectedIndex;
        }
        public void SetSelect(int sel)
        {
            if (listBox.Items.Count > 0)
                listBox.SelectedIndex = sel;
            else
                Selected = sel;
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
