using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UoViewer
{
    public partial class ExceptionForm : Form
    {
        public ExceptionForm(Exception err)
        {
            InitializeComponent();

            richTextBox.Text = err.Message+"\n\n"+err.StackTrace;
        }

        private void onclick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}