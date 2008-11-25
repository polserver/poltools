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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ultima;

namespace Controls
{
    public partial class Skills : UserControl
    {
        public Skills()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            object[] data = new object[3];
            for (int i = 0; i < 56; i++)
            {
                Ultima.Skills.SkillInfo skill = Ultima.Skills.GetSkill(i);
                if (skill.Name == null)
                    break;
                data[0] = i;
                data[1] = skill.IsAction;
                data[2] = skill.Name;
                dataGridView1.Rows.Add(data);
            }
        }
    }
}
