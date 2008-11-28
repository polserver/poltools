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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ultima;
using System.Globalization;
using System.IO;

namespace Controls
{
    public partial class Cliloc : UserControl
    {
        public Cliloc()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            refmarker = this;
            source = new BindingSource();
        }

        private static Cliloc refmarker;
        private static StringList cliloc;
        private static BindingSource source;
        private static int lang=0;
        private SortOrder sortorder = SortOrder.Ascending;
        private int sortcolumn = 0;

        public static void SaveEntry(int number, string text)
        {
            for (int i = 0; i < cliloc.Entries.Count; i++)
            {
                if (((StringEntry)cliloc.Entries[i]).Number==number)
                {
                    ((StringEntry)cliloc.Entries[i]).Text = text;
                    ((StringEntry)cliloc.Entries[i]).Flag = StringEntry.CliLocFlag.Modified;
                    refmarker.dataGridView1.Refresh();
                    refmarker.dataGridView1.Rows[i].Selected = true;
                    refmarker.dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    return;
                }
            }
        }

        public static bool IsNumberFree(int number)
        {
            foreach (StringEntry entry in cliloc.Entries)
            {
                if (entry.Number == number)
                    return false;
            }
            return true;
        }

        public static void AddEntry(int number)
        {
            int index = 0;
            foreach (StringEntry entry in cliloc.Entries)
            {
                if (entry.Number > number)
                {
                    cliloc.Entries.Insert(index,new StringEntry(number, "", StringEntry.CliLocFlag.Custom));
                    refmarker.dataGridView1.Refresh();
                    refmarker.dataGridView1.Rows[index].Selected = true;
                    refmarker.dataGridView1.FirstDisplayedScrollingRowIndex = index;
                    return;
                }
                ++index;
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            cliloc = new StringList("enu");
            LangComboBox.SelectedIndex=0;
            cliloc.Entries.Sort(new StringList.NumberComparerAsc());
            source.DataSource = cliloc.Entries;
            dataGridView1.DataSource = source;
            dataGridView1.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.None;
            dataGridView1.Columns[2].HeaderCell.SortGlyphDirection = SortOrder.None;
            dataGridView1.Columns[2].Width = 60;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Refresh();
        }

        private void onLangChange(object sender, EventArgs e)
        {
            if (LangComboBox.SelectedIndex != lang)
            {
                switch (LangComboBox.SelectedIndex)
                {
                    case 0: 
                        cliloc = new StringList("enu");
                        lang = 0;
                        break;
                    case 1:
                        cliloc = new StringList("deu");
                        lang = 1;
                        break;
                }
                cliloc.Entries.Sort(new StringList.NumberComparerAsc());
                source.DataSource = cliloc.Entries;
                dataGridView1.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                dataGridView1.Columns[0].Width = 60;
                dataGridView1.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.None;
                dataGridView1.Columns[2].HeaderCell.SortGlyphDirection = SortOrder.None;
                dataGridView1.Columns[2].Width = 60;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Refresh();
            }
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

        private void OnClickSave(object sender, EventArgs e)
        {
            dataGridView1.CancelEdit();
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, String.Format("Cliloc.{0}",cliloc.Language));
            cliloc.SaveStringList(FileName);
            dataGridView1.Columns[sortcolumn].HeaderCell.SortGlyphDirection = SortOrder.None;
            dataGridView1.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            sortcolumn = 0;
            sortorder = SortOrder.Ascending;
            dataGridView1.Refresh();
            MessageBox.Show(String.Format("CliLoc saved to {0}",FileName),"Saved");
        }

        private void onCell_dbClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int CellNr = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                string CellText = (string)dataGridView1.Rows[e.RowIndex].Cells[1].Value;
                new ClilocDetail(CellNr, CellText).Show();
            }
        }

        private void OnClick_AddEntry(object sender, EventArgs e)
        {
            new ClilocAdd().Show();
        }

        private void OnClick_DeleteEntry(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                cliloc.Entries.RemoveAt(dataGridView1.SelectedCells[0].OwningRow.Index);
                dataGridView1.Refresh();
            }
        }

        private void OnHeaderClicked(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (sortcolumn == e.ColumnIndex)
            {
                if (sortorder == SortOrder.Ascending)
                    sortorder = SortOrder.Descending;
                else
                    sortorder = SortOrder.Ascending;
            }
            else
            {
                sortorder = SortOrder.Ascending;
                dataGridView1.Columns[sortcolumn].HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection =
                sortorder == SortOrder.Ascending ?
                SortOrder.Ascending : SortOrder.Descending;
            sortcolumn = e.ColumnIndex;
            if (e.ColumnIndex == 0)
            {
                if (sortorder == SortOrder.Ascending)
                    cliloc.Entries.Sort(new StringList.NumberComparerAsc());
                else
                    cliloc.Entries.Sort(new StringList.NumberComparerDesc());
            }
            else if (e.ColumnIndex == 1)
            {
                if (sortorder == SortOrder.Ascending)
                    cliloc.Entries.Sort(new StringList.TextComparerAsc());
                else
                    cliloc.Entries.Sort(new StringList.TextComparerDesc());
            }
            else
            {
                if (sortorder == SortOrder.Ascending)
                    cliloc.Entries.Sort(new StringList.FlagComparerAsc());
                else
                    cliloc.Entries.Sort(new StringList.FlagComparerDesc());
            }
            dataGridView1.Refresh();
        }
    }
}
