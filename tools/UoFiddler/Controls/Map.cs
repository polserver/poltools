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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Ultima;

namespace Controls
{
    public partial class Map : UserControl
    {
        public Map()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            if (!Files.CacheData)
                PreloadMap.Visible = false;
            ProgressBar.Visible = false;
        }

        private Bitmap map;
        private Ultima.Map currmap;
        private bool ShowStatics = true;
        private bool SyncWithClient = false;
        private int ClientX = 0;
        private int ClientY = 0;
        private int ClientZ = 0;
        private int ClientMap = 0;
        private Point currPoint;
        private double Zoom = 1;
        bool moving = false;
        Point movingpoint;

        private bool Loaded = false;

        /// <summary>
        /// ReLoads if loaded
        /// </summary>
        public void Reload()
        {
            if (!Loaded)
                return;
            Zoom = 1;
            moving = false;
            OnLoad(this, EventArgs.Empty);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            Loaded = true;
            currmap = Ultima.Map.Felucca;
            feluccaToolStripMenuItem.Checked = true;
            map = currmap.GetImage(0, 0, (int)(pictureBox.Right / Zoom)+8 >> 3, (int)(pictureBox.Bottom / Zoom)+8 >> 3, ShowStatics);
            ZoomMap(ref map);
            pictureBox.Image = map;
            ZoomLabel.Text = String.Format("Zoom: {0}",Zoom);
            SetScrollBarValues();
            Refresh();
            this.Cursor = Cursors.Default;
        }

        private void HandleScroll(object sender, ScrollEventArgs e)
        {
            map = currmap.GetImage(hScrollBar.Value >> 3, vScrollBar.Value >> 3, (int)(pictureBox.Right / Zoom)+8 >> 3, (int)(pictureBox.Bottom / Zoom)+8 >> 3, ShowStatics);
            ZoomMap(ref map);
            pictureBox.Image = map;
            pictureBox.Update();
        }
        
        private void ZoomMap(ref Bitmap bmp0)
        {
            Bitmap bmp1 = new Bitmap((int)(map.Width*Zoom),(int)(map.Height*Zoom));
            Graphics graph = Graphics.FromImage(bmp1);
            graph.InterpolationMode = InterpolationMode.NearestNeighbor;
            graph.PixelOffsetMode = PixelOffsetMode.Half;
            graph.DrawImage(bmp0,new Rectangle(0,0,bmp1.Width,bmp1.Height));
            graph.Dispose();
            bmp0 = bmp1;
        }

        private void SetScrollBarValues()
        {
            vScrollBar.Minimum = 0;
            hScrollBar.Minimum = 0;
            hScrollBar.Maximum = (int)(currmap.Width);
            hScrollBar.Maximum -= (int)(pictureBox.ClientSize.Width);
            hScrollBar.Maximum += (int)(40);
            hScrollBar.Maximum = (hScrollBar.Maximum >> 3) << 3;
            hScrollBar.LargeChange = 40;
            hScrollBar.SmallChange = 8;
            vScrollBar.Maximum = (int)(currmap.Height);
            vScrollBar.Maximum -= (int)(pictureBox.ClientSize.Height);
            vScrollBar.Maximum += (int)(40);
            vScrollBar.Maximum = (vScrollBar.Maximum >> 3) << 3;
            vScrollBar.LargeChange = 40;
            vScrollBar.SmallChange = 8;
            vScrollBar.Value = 0;
            hScrollBar.Value = 0;
        }

        private void ChangeScrollBar()
        {
            hScrollBar.Maximum = (int)(currmap.Width);
            hScrollBar.Maximum -= ((int)(pictureBox.ClientSize.Width / Zoom) - 8 >> 3) << 3;
            if (Zoom >= 1)
                hScrollBar.Maximum += (int)(40 * Zoom);
            else if (Zoom < 1)
                hScrollBar.Maximum += (int)(40 / Zoom);
            hScrollBar.Maximum = (hScrollBar.Maximum >> 3) << 3;
            vScrollBar.Maximum = (int)(currmap.Height);
            vScrollBar.Maximum -= ((int)(pictureBox.ClientSize.Height / Zoom) - 8 >> 3) << 3;
            if (Zoom >= 1)
                vScrollBar.Maximum += (int)(40 * Zoom);
            else if (Zoom < 1)
                vScrollBar.Maximum += (int)(40 / Zoom);
            vScrollBar.Maximum = (vScrollBar.Maximum >> 3) << 3;
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                ChangeScrollBar();
                map = currmap.GetImage(hScrollBar.Value >> 3, vScrollBar.Value >> 3, (int)(pictureBox.ClientSize.Width / Zoom)+8 >> 3, (int)(pictureBox.ClientSize.Height / Zoom)+8 >> 3, ShowStatics);
                ZoomMap(ref map);
                pictureBox.Image = map;
                pictureBox.Update();
            }
        }

        private void ClickShowStatics(object sender, EventArgs e)
        {
            ShowStatics = !ShowStatics;
            map = currmap.GetImage(hScrollBar.Value >> 3, vScrollBar.Value >> 3, (int)(pictureBox.ClientSize.Width / Zoom) + 8 >> 3, (int)(pictureBox.ClientSize.Height / Zoom) + 8 >> 3, ShowStatics);
            ZoomMap(ref map);
            pictureBox.Image = map;
            pictureBox.Update();
        }

        private void ChangeMap()
        {
            map = currmap.GetImage(0, 0, (int)(pictureBox.Right / Zoom)+8 >> 3, (int)(pictureBox.Bottom / Zoom)+8 >> 3, ShowStatics);
            ZoomMap(ref map);
            pictureBox.Image = map;
            pictureBox.Update();
            SetScrollBarValues();
        }

        private void ResetCheckedMap()
        {
            feluccaToolStripMenuItem.Checked = false;
            trammelToolStripMenuItem.Checked = false;
            malasToolStripMenuItem.Checked = false;
            ilshenarToolStripMenuItem.Checked = false;
            tokunoToolStripMenuItem.Checked = false;
        }

        private void ChangeMapFelucca(object sender, EventArgs e)
        {
            if (!feluccaToolStripMenuItem.Checked)
            {
                ResetCheckedMap();
                feluccaToolStripMenuItem.Checked = true;
                currmap = Ultima.Map.Felucca;
                ChangeMap();
            }
        }

        private void ChangeMapTrammel(object sender, EventArgs e)
        {
            if (!trammelToolStripMenuItem.Checked)
            {
                ResetCheckedMap();
                trammelToolStripMenuItem.Checked = true;
                currmap = Ultima.Map.Trammel;
                ChangeMap();
            }
        }

        private void ChangeMapMalas(object sender, EventArgs e)
        {
            if (!malasToolStripMenuItem.Checked)
            {
                ResetCheckedMap();
                malasToolStripMenuItem.Checked = true;
                currmap = Ultima.Map.Malas;
                ChangeMap();
            }
        }

        private void ChangeMapIlshenar(object sender, EventArgs e)
        {
            if (!ilshenarToolStripMenuItem.Checked)
            {
                ResetCheckedMap();
                ilshenarToolStripMenuItem.Checked = true;
                currmap = Ultima.Map.Ilshenar;
                ChangeMap();
            }
        }

        private void ChangeMapTokuno(object sender, EventArgs e)
        {
            if (!tokunoToolStripMenuItem.Checked)
            {
                ResetCheckedMap();
                tokunoToolStripMenuItem.Checked = true;
                currmap = Ultima.Map.Tokuno;
                ChangeMap();
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                moving = true;
                movingpoint.X = e.X;
                movingpoint.Y = e.Y;
                this.Cursor = Cursors.Hand;
            }
            else
            {
                moving = false;
                this.Cursor = Cursors.Default;
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
            this.Cursor = Cursors.Default;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            int xDelta = Math.Min(currmap.Width,(int)(e.X / Zoom) + hScrollBar.Value);
            int yDelta = Math.Min(currmap.Height,(int)(e.Y / Zoom) + vScrollBar.Value);
            CoordsLabel.Text=String.Format("Coords: {0},{1}",xDelta,yDelta);
            if (moving)
            {
                int deltax = (int)(-1 * (e.X - movingpoint.X) / Zoom);
                int deltay = (int)(-1 * (e.Y - movingpoint.Y) / Zoom);
                movingpoint.X = e.X;
                movingpoint.Y = e.Y;
                hScrollBar.Value = Math.Max(0, Math.Min(hScrollBar.Maximum, hScrollBar.Value + deltax));
                vScrollBar.Value = Math.Max(0, Math.Min(vScrollBar.Maximum, vScrollBar.Value + deltay));
                map = currmap.GetImage(hScrollBar.Value >> 3, vScrollBar.Value >> 3, (int)(pictureBox.Right / Zoom) + 8 >> 3, (int)(pictureBox.Bottom / Zoom) + 8 >> 3, ShowStatics);
                ZoomMap(ref map);
                pictureBox.Image = map;
                pictureBox.Update();
            }
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
            if (!Ultima.Client.Running)
                return;
            Ultima.Client.Calibrate();
            if (!Ultima.Client.FindLocation(ref x, ref y, ref z, ref mapClient))
                return;
            tokunoToolStripMenuItem.Checked = false;
            trammelToolStripMenuItem.Checked = false;
            malasToolStripMenuItem.Checked = false;
            ilshenarToolStripMenuItem.Checked = false;
            feluccaToolStripMenuItem.Checked = false;
            switch (mapClient)
            {
                case 0:
                    feluccaToolStripMenuItem.Checked = true;
                    currmap = Ultima.Map.Felucca;
                    break;
                case 1:
                    trammelToolStripMenuItem.Checked = true;
                    currmap = Ultima.Map.Trammel;
                    break;
                case 2:
                    ilshenarToolStripMenuItem.Checked = true;
                    currmap = Ultima.Map.Ilshenar;
                    break;
                case 3:
                    malasToolStripMenuItem.Checked = true;
                    currmap = Ultima.Map.Malas;
                    break;
                case 4:
                    tokunoToolStripMenuItem.Checked = true;
                    currmap = Ultima.Map.Tokuno;
                    break;
            }
            int screenX = (int)Math.Max(0,x-pictureBox.Right/Zoom/2);
            int screenY = (int)Math.Max(0,y-pictureBox.Bottom/Zoom/2);
            SetScrollBarValues();
            hScrollBar.Value = screenX;
            vScrollBar.Value = screenY;
            map = currmap.GetImage(screenX >> 3, screenY >> 3, (int)(pictureBox.Right / Zoom)+8 >> 3, (int)(pictureBox.Bottom / Zoom)+8 >> 3, ShowStatics);
            ZoomMap(ref map);
            pictureBox.Image = map;
            pictureBox.Update();
            string mapname = "";
            switch (mapClient)
            {
                case 0:
                    mapname = "Felucca";
                    break;
                case 1:
                    mapname = "Trammel";
                    break;
                case 2:
                    mapname = "Ilshenar";
                    break;
                case 3:
                    mapname = "Malas";
                    break;
                case 4:
                    mapname = "Tokuno";
                    break;
            }

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
                string mapname = "";
                if (Ultima.Client.Running)
                {
                    Ultima.Client.Calibrate();
                    if (Ultima.Client.FindLocation(ref x, ref y, ref z, ref mapClient))
                    {
                        if ((ClientX == x) && (ClientY == y) && (ClientZ == z) && (ClientMap == mapClient))
                            return;
                        ClientX = x;
                        ClientY = y;
                        ClientZ = z;
                        ClientMap = mapClient;
                        switch (mapClient)
                        {
                            case 0:
                                mapname = "Felucca";
                                break;
                            case 1:
                                mapname = "Trammel";
                                break;
                            case 2:
                                mapname = "Ilshenar";
                                break;
                            case 3:
                                mapname = "Malas";
                                break;
                            case 4:
                                mapname = "Tokuno";
                                break;
                        }
                    }
                }

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
            currPoint.X = (int)(currPoint.X / Zoom);
            currPoint.Y = (int)(currPoint.Y / Zoom);
            currPoint.X += hScrollBar.Value;
            currPoint.Y += vScrollBar.Value;
        }

        private void OnZoomMinus(object sender, EventArgs e)
        {
            Zoom /= 2;
            DoZoom(true);
        }

        private void OnZoomPlus(object sender, EventArgs e)
        {
            Zoom *= 2;
            DoZoom(false);
        }

        private void DoZoom(bool minus)
        {
            ChangeScrollBar();
            ZoomLabel.Text = String.Format("Zoom: {0}", Zoom);
            int x, y;
            x = Math.Max(0,currPoint.X - (int)(pictureBox.ClientSize.Width / Zoom) / 2);
            y = Math.Max(0,currPoint.Y - (int)(pictureBox.ClientSize.Height / Zoom) / 2);
            x = Math.Min(x, hScrollBar.Maximum);
            y = Math.Min(y, vScrollBar.Maximum);
            x >>= 3;
            y >>= 3;
            hScrollBar.Value = x << 3;
            vScrollBar.Value = y << 3;
            map = currmap.GetImage(x, y, (int)(pictureBox.ClientSize.Width / Zoom) + 8 >> 3, (int)(pictureBox.ClientSize.Height / Zoom) + 8 >> 3, ShowStatics);
            ZoomMap(ref map);
            pictureBox.Image = map;
            pictureBox.Update();
        }

        private void ExtractMap(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string name = "";
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
            Bitmap extract = currmap.GetImage(0, 0, (currmap.Width >> 3), (currmap.Height >> 3), ShowStatics);
            extract.Save(FileName, ImageFormat.Tiff);
            this.Cursor = Cursors.Default;
            MessageBox.Show(String.Format("Map saved to {0}", FileName), "Saved",
                MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);
        }

        #region PreLoader
        private void OnClickPreloadMap(object sender, EventArgs e)
        {
            if (PreloadWorker.IsBusy)
                return;
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = 16;
            ProgressBar.Step = 1;
            ProgressBar.Value = 0;
            ProgressBar.Visible = true;
            PreloadWorker.RunWorkerAsync();
        }

        private void PreLoadDoWork(object sender, DoWorkEventArgs e)
        {
            int width = currmap.Width >> 3;
            int height = currmap.Height >> 3;
            width /= 4;
            height /= 4;
            for (int x = 0; x <= (currmap.Width >> 3) - width; x += width)
            {
                for (int y = 0; y <= (currmap.Height >> 3) - height; y += height)
                {
                    currmap.GetImage(x, y, width, height, true);
                    PreloadWorker.ReportProgress(1);
                }
            }
        }

        private void PreLoadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.PerformStep();
        }

        private void PreLoadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBar.Visible = false;
        }
        #endregion
    }
}
