using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ATCId.InputForm
{
	public partial class InputForm : Form
	{
		protected string _input_string = String.Empty;
		DialogResult _result;

		public InputForm(string prompt)
		{
			InitializeComponent();
			this.label1.Text = prompt;
		}

		public string text
		{
			get
			{
				return _input_string;
			}
		}

		public DialogResult result
		{
			get
			{
				return _result;
			}
		}

		private void BTN_OK_Click(object sender, EventArgs e)
		{
			_input_string = this.textBox1.Text;
			_result = DialogResult.OK;

			this.DialogResult = _result;

			this.Close();
		}

		private void BTN_CANCEL_Click(object sender, EventArgs e)
		{
			_input_string = this.textBox1.Text;
			_result = DialogResult.Cancel;

			this.DialogResult = _result;

			this.Close();
		}


	}
}
