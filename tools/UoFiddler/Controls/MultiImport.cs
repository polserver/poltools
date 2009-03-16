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
using System.IO;
using System.Windows.Forms;
using Ultima;

namespace FiddlerControls
{
    public partial class MultiImport : Form
    {
        public MultiImport(FiddlerControls.Multis _parent, int _id)
        {
            InitializeComponent();
            id = _id;
            parent = _parent;
            comboBox1.SelectedIndex = 0;
        }
        int id;
        FiddlerControls.Multis parent;

        private void OnClickBrowse(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            string type = "txt";
            switch (comboBox1.SelectedIndex)
            {
                case 0: type = "txt";
                    break;
                case 1: type = "uoa";
                    break;
                case 2: type = "wsc";
                    break;
            }
            dialog.Title = String.Format("Choose {0} file to import", type);
            dialog.CheckFileExists = true;
            dialog.Filter = String.Format("{0} file (*.{0})|*.{0}", type);
            if (dialog.ShowDialog() == DialogResult.OK)
                textBox1.Text = dialog.FileName;
        }

        private void OnClickImport(object sender, EventArgs e)
        {
            if (File.Exists(textBox1.Text))
            {
                bool centeritem = checkBox1.Checked;
                Ultima.Multis.ImportType type = (Ultima.Multis.ImportType)comboBox1.SelectedIndex;
                MultiComponentList multi = Ultima.Multis.ImportFromFile(id, textBox1.Text, type, centeritem);
                parent.ChangeMulti(id, multi);
                Options.ChangedUltimaClass["Multis"] = true;
                Close();
            }
        }
    }
}
