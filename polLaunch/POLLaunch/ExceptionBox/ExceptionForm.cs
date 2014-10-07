using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace POLLaunch.ExceptionBox
{
    public partial class ExceptionForm : Form
    {
        public ExceptionForm(ref Exception ex)
        {
            InitializeComponent();
            TB_ExceptionInformation.Text = ex.ToString();
        }

        private void BTN_CopyToClipBoard_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Text, (Object)TB_ExceptionInformation.Text);
        }

        private void BTN_CloseExceptionForm_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            this.Close();
        }
    }
}
