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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Controls
{
    public partial class LandTileSearch : Form
    {
        private static bool alt;
        public LandTileSearch(bool alternative)
        {
            InitializeComponent();
            alt = alternative;
        }

        private void SearchGraphic(object sender, EventArgs e)
        {
            int graphic;
            string convert;
            bool candone;
            if (textBoxGraphic.Text.Contains("0x"))
            {
                convert = textBoxGraphic.Text.Replace("0x", "");
                candone = int.TryParse(convert, System.Globalization.NumberStyles.HexNumber, null, out graphic);
            }
            else
                candone = int.TryParse(textBoxGraphic.Text, System.Globalization.NumberStyles.Integer, null, out graphic);

            if (candone)
            {
                bool res;
                if (alt)
                    res = LandTilesAlternative.SearchGraphic(graphic);
                else
                    res = LandTiles.SearchGraphic(graphic);
                if (!res)
                {
                    DialogResult result = MessageBox.Show("No landtile found", "Result", MessageBoxButtons.OKCancel);
                    if (result == DialogResult.Cancel)
                        Close();
                }
            }
        }

        private void SearchName(object sender, EventArgs e)
        {
            bool res;
            if (alt)
                res = LandTilesAlternative.SearchName(textBoxItemName.Text,false);
            else
                res = LandTiles.SearchName(textBoxItemName.Text,false);
            if (!res)
            {
                DialogResult result = MessageBox.Show("No landtile found", "Result", MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel)
                    Close();
            }
        }

        private void SearchNextName(object sender, EventArgs e)
        {
            bool res;
            if (alt)
                res = LandTilesAlternative.SearchName(textBoxItemName.Text, true);
            else
                res = LandTiles.SearchName(textBoxItemName.Text, true);
            if (!res)
            {
                DialogResult result = MessageBox.Show("No landtile found", "Result", MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel)
                    Close();
            }
        }
    }
}
