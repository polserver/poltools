using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FiddlerControls
{
    public partial class ProgressBar : Form
    {
        public ProgressBar()
        {
            InitializeComponent();
        }
    
        public ProgressBar(int max, string desc)
        {
            InitializeComponent();
            this.Text = desc;
            progressBar1.Maximum = max;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            FiddlerControls.Events.ProgressChangeEvent+=new FiddlerControls.Events.ProgressChangeHandler(onProgressChangeEvent);
            this.Show();
        }
        void onProgressChangeEvent()
        {
            progressBar1.PerformStep();
        }
    }
}
