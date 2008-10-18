using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Controls
{
    public partial class HuePopUpItem : Form
    {
        private Controls.ItemDetail refItem;
        public HuePopUpItem(Controls.ItemDetail ref_, int hue)
        {
            InitializeComponent();
            if (hue>=0)
                control.SetSelect(hue);
            refItem = ref_;
        }

        private void Click_OK(object sender, EventArgs e)
        {
            int Selected=control.GetSelect();
            refItem.ChangeHue(Selected);
            //this.Close();
            this.Hide();
        }

        private void OnClick_Clear(object sender, EventArgs e)
        {
            refItem.ChangeHue(-1);
            this.Hide();
            //this.Close();
        }
        public void SetHue(int hue)
        {
            control.SetSelect(hue);
        }
    }
}