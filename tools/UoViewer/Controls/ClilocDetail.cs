using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Controls
{
    public partial class ClilocDetail : Form
    {
        public ClilocDetail(string Number, string Text)
        {
            InitializeComponent();
            TextBox.AppendText(String.Format("Number: {0}\n\n", Number));
            TextBox.AppendText(String.Format("Text:\n{0}",Text));
        }
    }
}