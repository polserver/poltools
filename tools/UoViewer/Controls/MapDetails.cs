using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ultima;

namespace Controls
{
    public partial class MapDetails : Form
    {
        public MapDetails(Ultima.Map currmap, Point point)
        {
            InitializeComponent();
            Tile currtile = currmap.Tiles.GetLandTile(point.X, point.Y);
            richTextBox.AppendText(String.Format("X: {0} Y: {1}\n\n",point.X,point.Y));
            richTextBox.AppendText("LandTile:\n");
            richTextBox.AppendText(String.Format("{0}: 0x{1:X} Altitute: {2}\n\n", 
                                                 Ultima.TileData.LandTable[currtile.ID].Name,
                                                 currtile.ID, 
                                                 currtile.Z));
            HuedTile[] currStatics=currmap.Tiles.GetStaticTiles(point.X, point.Y);
            richTextBox.AppendText("Statics:\n");
            
            foreach (HuedTile currstatic in currStatics)
            {
                richTextBox.AppendText(String.Format("{0}: 0x{1:X} Hue: {2} Altitute: {3}\n",
                                                     Ultima.TileData.ItemTable[currstatic.ID & 0x3FFF].Name, 
                                                     currstatic.ID & 0x3FFF, 
                                                     currstatic.Hue, 
                                                     currstatic.Z));
            }
        }
    }
}