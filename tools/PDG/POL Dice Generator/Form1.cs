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

        private void BTN_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BTN_Create_Click(object sender, EventArgs e)
        {
            update_result(true, chkUniform.Checked, chkAutocopy.Checked);
        }

        private void update_result(bool should_msg, bool uniform, bool autocopy)
        {
            if (TB_MinValue.Text == "" || TB_MaxValue.Text == "")
            {
                return;
            }

            ulong min = 0, max = 0;
            try
            {
                min = Convert.ToUInt64(TB_MinValue.Text);
                max = Convert.ToUInt64(TB_MaxValue.Text);

                TB_Result.Text = Dice.dicestring(min, max);
                if (autocopy)
                    copy_result();

            }
            catch (OverflowException)
            {
                if (should_msg)
                    MessageBox.Show("Min and Max Values must be less than 2^64!!");
            }
            catch (FormatException)
            {
                if (should_msg)
                    MessageBox.Show("Min and Max Values must be positive integers!");
            }
        }

        private void BTN_Copy_Click(object sender, EventArgs e)
        {
            copy_result();
        }

        private void copy_result()
        {
            TB_Result.SelectAll();
            TB_Result.Copy();
        }

        private void TB_MaxMinValue_TextChanged(object sender, EventArgs e)
        {
            if (chkUpdate.Checked)
                update_result(false, chkUniform.Checked, chkAutocopy.Checked);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            MyForm.ActiveForm.TopMost = chkOnTop.Checked;
        }
    }
}
