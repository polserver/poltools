using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ultima;
using System.Drawing.Imaging;
using System.Xml;
using System.IO;
using Controls;

namespace UoViewer
{
    public partial class UoViewer : Form
    {
        public UoViewer()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private PathSettings m_Path = new PathSettings();
        private void click_path(object sender, EventArgs e)
        {
            if (m_Path.IsDisposed)
                m_Path = new PathSettings();
            else
                m_Path.Focus();
            m_Path.Show();
        }
    }
}