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

using System.Windows.Forms;

namespace UoFiddler
{
    public partial class UnDocked : Form
    {
        private int m_index;
        private string m_name;
        public UnDocked(Control contr,string name, int index)
        {
            this.Controls.Clear();
            this.Controls.Add(contr);
            InitializeComponent();
            this.Text = name;
            m_index = index;
            m_name = name;
            if (UoFiddler.ActiveForm.TopMost)
                this.TopMost = true;
        }

        public void ChangeControl(Control contr)
        {
            this.Controls.Clear();
            this.Controls.Add(contr);
            this.PerformLayout();

        }
        private void OnClose(object sender, FormClosingEventArgs e)
        {
            UoFiddler.ReDock(this.Controls[0], m_index, m_name);
        }
    }
}
