using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ExceptionBox
{
    public partial class ExceptionForm : Form
    {
		public ExceptionForm(ref Exception ex)
		{
			Initialize("Exception Occured", "An exception has occured", ref ex);
		}
		public ExceptionForm(string title, ref Exception ex)
		{
			Initialize(title, title, ref ex);
		}
		public ExceptionForm(string title, string description, ref Exception ex)
		{
			Initialize(title, description, ref ex);
		}

		private void Initialize(string title, string description, ref Exception ex)
		{
			InitializeComponent();

			this.Text = title;
			this.LBL_ExceptionInfo1.Text = description;

			TB_ExceptionInformation.Text = string.Empty;
			TB_ExceptionInformation.AppendText(ex.Message);
			TB_ExceptionInformation.AppendText(Environment.NewLine);
			TB_ExceptionInformation.AppendText("=============================================");
			TB_ExceptionInformation.AppendText(Environment.NewLine);
			TB_ExceptionInformation.AppendText(ex.ToString());
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

		private void ExceptionForm_Load(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
    }
}
