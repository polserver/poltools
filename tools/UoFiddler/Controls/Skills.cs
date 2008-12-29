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

namespace FiddlerControls
{
    public partial class Skills : UserControl
    {
        public Skills()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private bool Loaded = false;

        /// <summary>
        /// ReLoads if loaded
        /// </summary>
        public void Reload()
        {
            if (Loaded)
                OnLoad(this, EventArgs.Empty);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            Loaded = true;
            object[] data = new object[3];
            dataGridView1.Rows.Clear();
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
            this.Cursor = Cursors.Default;
        }
    }
}
