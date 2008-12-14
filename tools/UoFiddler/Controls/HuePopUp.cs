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
using System.Windows.Forms;

namespace Controls
{
    public partial class HuePopUp : Form
    {
        private Controls.Animationlist refMob;
        public HuePopUp(Controls.Animationlist ref_, int hue)
        {
            InitializeComponent();
            if ((hue & 0x8000)!=0)
            {
                hue ^= 0x8000;
                HueOnlyGray.Checked=true;
            }
            if (hue != 0)
                control.Selected = hue;
            refMob = ref_;
        }

        private void Click_OK(object sender, EventArgs e)
        {
            int Selected = control.Selected;
            if (HueOnlyGray.Checked)
                Selected ^= 0x8000;
            refMob.ChangeHue(Selected);
            this.Close();
        }

        private void OnClick_Clear(object sender, EventArgs e)
        {
            refMob.ChangeHue(-1);
            this.Close();
        }
    }
}