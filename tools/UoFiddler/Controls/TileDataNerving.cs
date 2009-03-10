using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FiddlerControls
{
    public partial class TileDataNerving : Form
    {
        private FiddlerControls.TileDatas refTileData;
        public TileDataNerving(FiddlerControls.TileDatas ref_, int id)
        {
            InitializeComponent();
            refTileData = ref_;
            checkBox1.Checked = refTileData.ShowNervingMsg;
            label1.Text=String.Format("Edits of 0x{0:X4} ({0}) saved to memory. Click 'Save Tiledata' to write to file.", id);
        }

        private void onExit(object sender, EventArgs e)
        {
            refTileData.ShowNervingMsg = checkBox1.Checked;
            Close();
        }
    }
}
