using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
            update_result(true);
        }

        // Updates dice-string
        private void update_result(bool should_msg)
        {
            update_result(should_msg, chkUniform.Checked, chkAutocopy.Checked);
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

                TB_Result.Text = Dice.dicestring(min, max, uniform);
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
                update_result(false);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            MyForm.ActiveForm.TopMost = chkOnTop.Checked;
        }

        private void chkUniform_CheckedChanged(object sender, EventArgs e)
        {
            update_result(false);
        }

        bool options_open = true;

        private void button1_Click(object sender, EventArgs e)
        {
            if (options_open)
                MyForm.ActiveForm.Height -= grpOptions.Size.Height + 3;
            else
                MyForm.ActiveForm.Height += grpOptions.Size.Height + 3;

            options_open = !options_open;

            BTN_Options.Text = (options_open) ? "Options <<" : "Options >>";
        }
    }
}
