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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ultima;
using System.IO;
using System.Drawing.Imaging;

namespace Controls
{
    public partial class Map : UserControl
    {
        public Map()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private Bitmap map;
        private Ultima.Map currmap;
        private bool ShowStatics = true;
        private bool SyncWithClient = false;
        private int ClientX = 0;
        private int ClientY = 0;
        private int ClientMap = 0;
        private static Brush brush = new SolidBrush(Color.FromArgb(180, Color.White));
        private Pen pen = new Pen(brush);
        private Point currPoint;

        private void OnLoad(object sender, EventArgs e)
        {
            currmap = Ultima.Map.Felucca;
            feluccaToolStripMenuItem.Checked = true;
            map=currmap.GetImage(0, 0,pictureBox.Right >>3,pictureBox.Bottom>>3 ,ShowStatics);

            pictureBox.Image = map;
            SetScrollBarValues();
            Refresh();
        }

        private void HandleScroll(object sender, ScrollEventArgs e)
        {
            map = currmap.GetImage(hScrollBar.Value >> 3, vScrollBar.Value >> 3, pictureBox.Right >> 3, pictureBox.Bottom >> 3, ShowStatics);
            pictureBox.Image = map;
            pictureBox.Update();
        }

        public void SetScrollBarValues()
        {

            this.vScrollBar.Minimum = 0;
            this.hScrollBar.Minimum = 0;

            this.hScrollBar.Maximum = currmap.Width;

            this.hScrollBar.LargeChange = 40;
            this.hScrollBar.SmallChange = 8;

            this.hScrollBar.Maximum += this.hScrollBar.LargeChange;

            this.vScrollBar.Maximum = currmap.Height;

            this.vScrollBar.LargeChange = 40;
            this.vScrollBar.SmallChange = 8;

            this.vScrollBar.Maximum += this.vScrollBar.LargeChange;

            this.vScrollBar.Value = 0;
            this.hScrollBar.Value = 0;
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                map = currmap.GetImage(hScrollBar.Value >> 3, vScrollBar.Value >> 3, pictureBox.ClientSize.Width >> 3, pictureBox.ClientSize.Height >> 3, ShowStatics);
                pictureBox.Image = map;
                pictureBox.Update();
            }
        }

        private void ChangeMap()
        {
            map = currmap.GetImage(0, 0, pictureBox.Right >> 3, pictureBox.Bottom >> 3, ShowStatics);
            pictureBox.Image = map;
            pictureBox.Update();
            SetScrollBarValues();
        }

        private void ChangeMapFelucca(object sender, EventArgs e)
        {
            if (!feluccaToolStripMenuItem.Checked)
            {
                feluccaToolStripMenuItem.Checked = true;
                trammelToolStripMenuItem.Checked = false;
                malasToolStripMenuItem.Checked = false;
                ilshenarToolStripMenuItem.Checked = false;
                tokunoToolStripMenuItem.Checked = false;
                currmap = Ultima.Map.Felucca;
                ChangeMap();
            }
        }

        private void ChangeMapTrammel(object sender, EventArgs e)
        {
            if (!trammelToolStripMenuItem.Checked)
            {
                trammelToolStripMenuItem.Checked = true;
                feluccaToolStripMenuItem.Checked = false;
                malasToolStripMenuItem.Checked = false;
                ilshenarToolStripMenuItem.Checked = false;
                tokunoToolStripMenuItem.Checked = false;
                currmap = Ultima.Map.Trammel;
                ChangeMap();
            }
        }

        private void ChangeMapMalas(object sender, EventArgs e)
        {
            if (!malasToolStripMenuItem.Checked)
            {
                malasToolStripMenuItem.Checked = true;
                trammelToolStripMenuItem.Checked = false;
                feluccaToolStripMenuItem.Checked = false;
                ilshenarToolStripMenuItem.Checked = false;
                tokunoToolStripMenuItem.Checked = false;
                currmap = Ultima.Map.Malas;
                ChangeMap();
            }
        }

        private void ChangeMapIlshenar(object sender, EventArgs e)
        {
            if (!ilshenarToolStripMenuItem.Checked)
            {
                ilshenarToolStripMenuItem.Checked = true;
                trammelToolStripMenuItem.Checked = false;
                malasToolStripMenuItem.Checked = false;
                feluccaToolStripMenuItem.Checked = false;
                tokunoToolStripMenuItem.Checked = false;
                currmap = Ultima.Map.Ilshenar;
                ChangeMap();
            }
        }

        private void ChangeMapTokuno(object sender, EventArgs e)
        {
            if (!tokunoToolStripMenuItem.Checked)
            {
                tokunoToolStripMenuItem.Checked = true;
                trammelToolStripMenuItem.Checked = false;
                malasToolStripMenuItem.Checked = false;
                ilshenarToolStripMenuItem.Checked = false;
                feluccaToolStripMenuItem.Checked = false;
                currmap = Ultima.Map.Tokuno;
                ChangeMap();
            }
        }

        private void ExtractMap(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            string name="";
            if (feluccaToolStripMenuItem.Checked)
                name = "Felucca.tiff";
            else if (trammelToolStripMenuItem.Checked)
                name = "Trammel.tiff";
            else if (malasToolStripMenuItem.Checked)
                name = "Malas.tiff";
            else if (ilshenarToolStripMenuItem.Checked)
                name = "Ilshenar.tiff";
            else if (tokunoToolStripMenuItem.Checked)
                name = "Tokuno.tiff";

            if (name == "")
                return;
            
            string FileName = Path.Combine(path, name);
            Bitmap extract = currmap.GetImage(0, 0, (currmap.Width >> 3), (currmap.Height >> 3));
            extract.Save(FileName, ImageFormat.Tiff);
            
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            int xDelta = e.X + hScrollBar.Value;
			int yDelta = e.Y + vScrollBar.Value;
            CoordsLabel.Text=String.Format("Coords: {0},{1}",xDelta,yDelta);
        }

        private void onClick_ShowClientLoc(object sender, EventArgs e)
        {
            SyncWithClient = !SyncWithClient;
        }

        private void onClick_GotoClientLoc(object sender, EventArgs e)
        {
            int x = 0;
            int y = 0;
            int z = 0;
            int mapClient = 0;
            Ultima.Client.Calibrate();
            Ultima.Client.FindLocation(ref x, ref y, ref z, ref mapClient);
            tokunoToolStripMenuItem.Checked = false;
            trammelToolStripMenuItem.Checked = false;
            malasToolStripMenuItem.Checked = false;
            ilshenarToolStripMenuItem.Checked = false;
            feluccaToolStripMenuItem.Checked = false;
            if (mapClient == 0)
            {
                feluccaToolStripMenuItem.Checked = true;
                currmap = Ultima.Map.Felucca;
            }
            else if (mapClient == 2)
            {
                ilshenarToolStripMenuItem.Checked = true;
                currmap = Ultima.Map.Ilshenar;
            }
            else if (mapClient == 3)
            {
                malasToolStripMenuItem.Checked = true;
                currmap = Ultima.Map.Malas;
            }
            else if (mapClient == 4)
            {
                tokunoToolStripMenuItem.Checked = true;
                currmap = Ultima.Map.Tokuno;
            }
            int screenX = x-pictureBox.Right/2;
            int screenY = y-pictureBox.Bottom/2;
            if (screenX < 0)
                screenX = 0;
            if (screenY < 0)
                screenY = 0;
            SetScrollBarValues();
            hScrollBar.Value = screenX;
            vScrollBar.Value = screenY;
            map = currmap.GetImage(screenX>>3, screenY>>3, pictureBox.Right >> 3, pictureBox.Bottom >> 3, ShowStatics);
            pictureBox.Image = map;
            pictureBox.Update();
            string mapname = "Britannia";
            if (mapClient == 2)
                mapname = "Ilshenar";
            else if (mapClient == 3)
                mapname = "Malas";
            else if (mapClient == 4)
                mapname = "Tokuno";

            ClientLocLabel.Text = String.Format("ClientLoc: {0},{1},{2},{3}", x, y, z, mapname);   
        }

        
        private void SyncClientTimer(object sender, EventArgs e)
        {
            if (SyncWithClient)
            {
                int x = 0;
                int y = 0;
                int z = 0;
                int mapClient = 0;
                Ultima.Client.Calibrate();
                Ultima.Client.FindLocation(ref x, ref y, ref z, ref mapClient);
                if ((ClientX == x) && (ClientY == y) && (ClientMap == mapClient))
                    return;
                ClientX = x;
                ClientY = y;
                ClientMap = mapClient;
                string mapname = "Britannia";
                if (mapClient == 2)
                    mapname = "Ilshenar";
                else if (mapClient == 3)
                    mapname = "Malas";
                else if (mapClient == 4)
                    mapname = "Tokuno";

                ClientLocLabel.Text=String.Format("ClientLoc: {0},{1},{2},{3}", x, y, z, mapname);               
            }
        }

        private void GetMapInfo(object sender, EventArgs e)
        {
            new MapDetails(currmap, currPoint).Show();
        }

        private void OnOpenContext(object sender, CancelEventArgs e)  // Speichern für GetMapInfo
        {
            currPoint = pictureBox.PointToClient(Control.MousePosition);
            currPoint.X += hScrollBar.Value;
            currPoint.Y += vScrollBar.Value;
        }
    }
}
