using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

/*

Reason why this works:

    Suppose a dice-string: XdY + Z

    max = if all dice come up Y = x*y + z              (1)
    min = if all dice come up 1 = x*1 + z = x + z      (2)

    Solving for x in (2), we get:
 
        x = min - z               (3)

    Solving for y in (1):

        xy + z = max
            xy = max - z
             y = (max - z) / x    (4)

    Subst. (3) in (4):

      y = (max-z)/(min-z)         (5)


    With (3) and (5), we have two equations for
 three variables. But, we need y to be integer (otherwise,
 there'd be no dice-string)

    Then, we vary z from 0 to a-1 and see which is the first z
 to allow y to be an integer. Having this z, we're done. Just
 need to substitute everything.

	Z = the one from the iterations
	X = min - Z
        Y = (max-z)/X

	And we have the dice-string: XdY + Z  :) 

--
Fernando Rozenblit (rozenblit@gmail.com) - 2009-03-10
	
*/

namespace POL_Dice_Generator
{
    public partial class MyForm : Form
    {
        public MyForm()
        {
            InitializeComponent();
        }

        private ulong x = 0, y = 0, z = 0; //dice string part

        private void BTN_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BTN_Create_Click(object sender, EventArgs e)
        {
            if ( TB_MinValue.Text == "" || TB_MaxValue.Text == "" )
            {
                return;
            }
            if ( (Convert.ToInt64(TB_MinValue.Text) < 0) || (Convert.ToInt64(TB_MaxValue.Text) < 0) )
            {
                MessageBox.Show("Min and Max Values must be positive integers!");
                return;
            }

            ulong min = 0;
            ulong max = 0;

            min = Convert.ToUInt64(TB_MinValue.Text);
            max = Convert.ToUInt64(TB_MaxValue.Text);

            if (min == max)
            {
                TB_Result.Text = min + "d1";
                return;
            }

            if (min > max)
            {
                find_dice(max, min); // inverted min/max
            }
            else
            {
                find_dice(min, max); // normal min/max
            }

            if (z == 0)
            {
                TB_Result.Text = x + "d" + y;
            }
            else
            {
                TB_Result.Text = x + "d" + y + "+" + z;
            }
            return;
        }

        /* Fernando Rozenblit (rozenblit@gmail.com) - 2009-03-10 */
        void find_dice(ulong min, ulong max)
        {

            for (z = 0; z < min; ++z)
            {
                x = (min - z);
                if ((max - z) % (x) == 0)
                {
                    y = (max - z) / (x);
                    return;
                }
            }
        }

        private void BTN_Copy_Click(object sender, EventArgs e)
        {
            TB_Result.SelectAll();
            TB_Result.Copy();
        }
    }
}
