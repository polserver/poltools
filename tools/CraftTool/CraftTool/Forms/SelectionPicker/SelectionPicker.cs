using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CraftTool.Forms.SelectionPicker
{
	public partial class SelectionPicker : Form
	{
		protected string _input_string = String.Empty;
		DialogResult _result;

		public SelectionPicker(string prompt, List<string>options)
		{
			InitializeComponent();

			label1.Text = prompt;
			comboBox1.Items.AddRange(options.ToArray());
		}

		private void BTN_ok_Click(object sender, EventArgs e)
		{
			string selected = this.comboBox1.SelectedItem.ToString();

			_input_string = selected;
			_result = DialogResult.OK;
			this.DialogResult = _result;

			this.Close();
		}

		private void BTN_cancel_Click(object sender, EventArgs e)
		{
			string selected = this.comboBox1.SelectedItem.ToString();
			_result = DialogResult.Cancel;

			this.DialogResult = _result;

			this.Close();
		}
	}
}
