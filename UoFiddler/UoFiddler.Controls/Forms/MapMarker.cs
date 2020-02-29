﻿/***************************************************************************
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
using System.Drawing;
using System.Windows.Forms;
using UoFiddler.Controls.Classes;
using UoFiddler.Controls.UserControls;

namespace UoFiddler.Controls.Forms
{
    public partial class MapMarker : Form
    {
        private static Color _lastcolor = Color.FromArgb(180, Color.Yellow);

        public MapMarker(int x, int y, int map)
        {
            InitializeComponent();
            Icon = Options.GetFiddlerIcon();
            numericUpDown1.Value = x;
            numericUpDown2.Value = y;
            comboBox1.Items.AddRange(Options.MapNames);
            comboBox1.SelectedIndex = map;
            pictureBox1.BackColor = _lastcolor;
        }

        private void OnClickColor(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
            {
                _lastcolor = pictureBox1.BackColor = colorDialog1.Color;
            }
        }

        private void OnClickSave(object sender, EventArgs e)
        {
            Map.AddOverlay(
                (int)numericUpDown1.Value,
                (int)numericUpDown2.Value,
                comboBox1.SelectedIndex,
                pictureBox1.BackColor,
                textBox1.Text);
            Close();
        }
    }
}
