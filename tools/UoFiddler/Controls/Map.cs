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

namespace FiddlerControls
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
        private int currmapint = 0;
        private bool ShowStatics = true;
        private bool SyncWithClient = false;
        private int ClientX = 0;
        private int ClientY = 0;
        private int ClientZ = 0;
        private int ClientMap = 0;
        private Point currPoint;
        private double Zoom = 1;
        private bool moving = false;
        private Point movingpoint;

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
            ChangeMapNames();
            ZoomLabel.Text = String.Format("Zoom: {0}",Zoom);
            SetScrollBarValues();
            Refresh();
            pictureBox.Refresh();
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Changes the Names of maps
        /// </summary>
        public void ChangeMapNames()
        {
            if (!Loaded)
                return;
            feluccaToolStripMenuItem.Text = Options.MapNames[0];
            trammelToolStripMenuItem.Text = Options.MapNames[1];
            ilshenarToolStripMenuItem.Text = Options.MapNames[2];
            malasToolStripMenuItem.Text = Options.MapNames[3];
            tokunoToolStripMenuItem.Text = Options.MapNames[4];
        }

        private void HandleScroll(object sender, ScrollEventArgs e)
        {
            pictureBox.Refresh();
        }

        private int Round(int x)
        {
            return (int)((x>>3)<<3);
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
            hScrollBar.Maximum = Round(hScrollBar.Maximum);
            hScrollBar.LargeChange = 40;
            hScrollBar.SmallChange = 8;
            vScrollBar.Maximum = (int)(currmap.Height);
            vScrollBar.Maximum -= (int)(pictureBox.ClientSize.Height);
            vScrollBar.Maximum += (int)(40);
            vScrollBar.Maximum = Round(vScrollBar.Maximum);
            vScrollBar.LargeChange = 40;
            vScrollBar.SmallChange = 8;
            vScrollBar.Value = 0;
            hScrollBar.Value = 0;
        }

        private void ChangeScrollBar()
        {
            hScrollBar.Maximum = (int)(currmap.Width);
            hScrollBar.Maximum -= Round((int)(pictureBox.ClientSize.Width / Zoom) - 8);
            if (Zoom >= 1)
                hScrollBar.Maximum += (int)(40 * Zoom);
            else if (Zoom < 1)
                hScrollBar.Maximum += (int)(40 / Zoom);
            hScrollBar.Maximum = Round(hScrollBar.Maximum);
            vScrollBar.Maximum = (int)(currmap.Height);
            vScrollBar.Maximum -= Round((int)(pictureBox.ClientSize.Height / Zoom) - 8);
            if (Zoom >= 1)
                vScrollBar.Maximum += (int)(40 * Zoom);
            else if (Zoom < 1)
                vScrollBar.Maximum += (int)(40 / Zoom);
            vScrollBar.Maximum = Round(vScrollBar.Maximum);
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                ChangeScrollBar();
                pictureBox.Refresh();
            }
        }

        private void ClickShowStatics(object sender, EventArgs e)
        {
            ShowStatics = !ShowStatics;
            pictureBox.Refresh();
        }

        private void ChangeMap()
        {
            SetScrollBarValues();
            pictureBox.Refresh();
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
                currmapint = 0;
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
                currmapint = 1;
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
                currmapint = 2;
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
                currmapint = 3;
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
                currmapint = 4;
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
            int xDelta = Math.Min(currmap.Width, (int)(e.X / Zoom) + Round(hScrollBar.Value));
            int yDelta = Math.Min(currmap.Height, (int)(e.Y / Zoom) + Round(vScrollBar.Value));
            CoordsLabel.Text=String.Format("Coords: {0},{1}",xDelta,yDelta);
            if (moving)
            {
                int deltax = (int)(-1 * (e.X - movingpoint.X) / Zoom);
                int deltay = (int)(-1 * (e.Y - movingpoint.Y) / Zoom);
                movingpoint.X = e.X;
                movingpoint.Y = e.Y;
                hScrollBar.Value = Math.Max(0, Math.Min(hScrollBar.Maximum, hScrollBar.Value + deltax));
                vScrollBar.Value = Math.Max(0, Math.Min(vScrollBar.Maximum, vScrollBar.Value + deltay));
                pictureBox.Refresh();
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
            if (currmapint != mapClient)
            {
                ResetCheckedMap();
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
                currmapint = mapClient;
            }
            ClientX = x;
            ClientY = y;
            ClientZ = z;
            ClientMap = mapClient;
            SetScrollBarValues();
            hScrollBar.Value = (int)Math.Max(0,x-pictureBox.Right/Zoom/2);
            vScrollBar.Value = (int)Math.Max(0,y-pictureBox.Bottom/Zoom/2);
            pictureBox.Refresh();
            ClientLocLabel.Text = String.Format("ClientLoc: {0},{1},{2},{3}", x, y, z, Options.MapNames[mapClient]);   
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
                        mapname = Options.MapNames[mapClient];
                    }
                }

                ClientLocLabel.Text=String.Format("ClientLoc: {0},{1},{2},{3}", x, y, z, mapname);
                pictureBox.Refresh();
            }
        }

        private void GetMapInfo(object sender, EventArgs e)
        {
            new MapDetails(currmap, currPoint).Show();
        }

        private void onClickShowXCross(object sender, EventArgs e)
        {
            pictureBox.Refresh();
        }

        private void OnOpenContext(object sender, CancelEventArgs e)  // Speichern für GetMapInfo
        {
            currPoint = pictureBox.PointToClient(Control.MousePosition);
            currPoint.X = (int)(currPoint.X / Zoom);
            currPoint.Y = (int)(currPoint.Y / Zoom);
            currPoint.X += hScrollBar.Value;
            currPoint.Y += vScrollBar.Value;
        }

        private void onContextClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            pictureBox.Refresh();
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
            hScrollBar.Value = Round(x);
            vScrollBar.Value = Round(y);
            pictureBox.Refresh();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            map = currmap.GetImage(hScrollBar.Value >> 3, vScrollBar.Value >> 3,
                (int)((e.ClipRectangle.Width / Zoom) + 8) >> 3, (int)((e.ClipRectangle.Height / Zoom) + 8) >> 3,
                ShowStatics);
            ZoomMap(ref map);
            e.Graphics.DrawImageUnscaledAndClipped(map, e.ClipRectangle);

            if (showCenterCrossToolStripMenuItem.Checked)
            {
                Brush brush = new SolidBrush(Color.FromArgb(180, Color.White));
                Pen pen = new Pen(brush);
                int x = Round((int)(pictureBox.Width/2));
                int y = Round((int)(pictureBox.Height/2));
                e.Graphics.DrawLine(pen, x - 4, y, x + 4, y);
                e.Graphics.DrawLine(pen, x, y - 4, x, y + 4);
                pen.Dispose();
                brush.Dispose();
            }

            if (showClientCrossToolStripMenuItem.Checked)
            {
                if (Client.Running)
                {
                    if ((ClientX > hScrollBar.Value) &&
                        (ClientX < hScrollBar.Value + e.ClipRectangle.Width * Zoom) &&
                        (ClientY > vScrollBar.Value) &&
                        (ClientX < vScrollBar.Value + e.ClipRectangle.Height * Zoom) &&
                        (ClientMap == currmapint))
                    {
                        Brush brush = new SolidBrush(Color.FromArgb(180, Color.Yellow));
                        Pen pen = new Pen(brush);
                        int x = (int)((ClientX - Round(hScrollBar.Value)) * Zoom);
                        int y = (int)((ClientY - Round(vScrollBar.Value)) * Zoom);
                        e.Graphics.DrawLine(pen, x - 4, y, x + 4, y);
                        e.Graphics.DrawLine(pen, x, y - 4, x, y + 4);
                        e.Graphics.DrawEllipse(pen, x - 2, y - 2, 2 * 2, 2 * 2);
                        pen.Dispose();
                        brush.Dispose();
                    }
                }
            }
        }

        private void ExtractMap(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string name = String.Format("{0}.tiff", Options.MapNames[currmapint]);
            string FileName = Path.Combine(path, name);
            Bitmap extract = currmap.GetImage(0, 0, (currmap.Width >> 3), (currmap.Height >> 3), ShowStatics);
            extract.Save(FileName, ImageFormat.Tiff);
            this.Cursor = Cursors.Default;
            MessageBox.Show(String.Format("Map saved to {0}", FileName), "Saved",
                MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);
        }

        private void onKeyDownGoto(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string line = TextBoxGoto.Text.Trim();
                if (line.Length > 0)
                {
                    string[] args = line.Split(' ');
                    if (args.Length != 2)
                        args = line.Split(',');
                    if (args.Length == 2)
                    {
                        int x, y;
                        if (int.TryParse(args[0], out x) && (int.TryParse(args[1], out y)))
                        {
                            if ((x >= 0) && (y >= 0))
                            {
                                if ((x <= currmap.Width) && (x <= currmap.Height))
                                {
                                    contextMenuStrip1.Close();
                                    hScrollBar.Value = (int)Math.Max(0, x - pictureBox.Right / Zoom / 2);
                                    vScrollBar.Value = (int)Math.Max(0, y - pictureBox.Bottom / Zoom / 2);
                                }
                            }
                        }
                    }
                }
                pictureBox.Refresh();
            }
        }

        private void onClickSendClient(object sender, EventArgs e)
        {
            if (Client.Running)
            {
                int x = (int)(pictureBox.Width / Zoom / 2);
                int y = (int)(pictureBox.Height / Zoom / 2);
                x += hScrollBar.Value;
                y += vScrollBar.Value;
                SendCharTo(x, y);
            }
        }

        private void onClickSendClientToPos(object sender, EventArgs e)
        {
            if (Client.Running)
                SendCharTo(currPoint.X, currPoint.Y);
        }

        private void SendCharTo(int x, int y)
        {
            string format = "{0} " + Options.MapArgs;
            int z = currmap.Tiles.GetLandTile(x, y).Z;
            Client.SendText(String.Format(format, Options.MapCmd, x, y, z, currmapint, Options.MapNames[currmapint]));
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
