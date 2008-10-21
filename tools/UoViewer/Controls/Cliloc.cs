using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ultima;
using System.Globalization;

namespace Controls
{
    public partial class Cliloc : UserControl
    {
        public Cliloc()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private static StringList cliloc;
        private static int lang=0;

        private void OnLoad(object sender, EventArgs e)
        {
            cliloc = new StringList("enu");
            LangComboBox.SelectedIndex=0;
            RefreshData();
            loadLabel.Text = "enu loaded";
        }

        private void RefreshData()
        {
            loadLabel.Text = "Loading...";
            dataGridView1.Rows.Clear();
            object[] data = new object[2];
            foreach (StringEntry entry in cliloc.Entries)
            {
                data[0] = entry.Number;
                data[1] = entry.Text;
                dataGridView1.Rows.Add(data);
            }
        }

        private void onLangChange(object sender, EventArgs e)
        {
            if (LangComboBox.SelectedIndex != lang)
            {
                switch (LangComboBox.SelectedIndex)
                {
                    case 0: 
                        cliloc = new StringList("enu");
                        RefreshData();
                        loadLabel.Text = "enu loaded";
                        lang = 0;
                        break;
                    case 1:
                        cliloc = new StringList("deu");
                        RefreshData();
                        loadLabel.Text = "deu loaded";
                        lang = 1;
                        break;
                }
            }
        }

        private void onCell_dbClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string CellNr = (string)dataGridView1.Rows[e.RowIndex].Cells[0].FormattedValue;
            string CellText = (string)dataGridView1.Rows[e.RowIndex].Cells[1].FormattedValue;
            new ClilocDetail(CellNr, CellText).Show();
        }


        private void GotoNr(object sender, EventArgs e)
        {
            int nr;
            bool result = Int32.TryParse(GotoEntry.Text.ToString(), NumberStyles.Integer, null, out nr);

            if (result)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if ((int)dataGridView1.Rows[i].Cells[0].Value == nr)
                    {
                        dataGridView1.Rows[i].Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        return;
                    }
                }
            }
            MessageBox.Show("Number not found.", "Goto", MessageBoxButtons.OK);
        }

        private void FindEntryClick(object sender, EventArgs e)
        {
            string find = FindEntry.Text.ToString();
            for (int i = (dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected)+1); i < dataGridView1.Rows.Count; i++)
            {
                if ((dataGridView1.Rows[i].Cells[1].Value.ToString().IndexOf(find))!=-1)
                {
                    dataGridView1.Rows[i].Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    return;
                }
            }
            MessageBox.Show("Entry not found.", "Entry", MessageBoxButtons.OK);
        }
    }
}
