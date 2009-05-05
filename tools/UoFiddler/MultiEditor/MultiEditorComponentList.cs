/***************************************************************************
 *
 * $Author: MuadDib & Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using Ultima;

namespace MultiEditor
{
    class MultiEditorComponentList
    {
        public ArrayList[][] Tiles { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int xMin { get; private set; }
        public int yMin { get; private set; }
        public int xMax { get; private set; }
        public int yMax { get; private set; }
        public int zMin { get; private set; }
        public int zMax { get; private set; }

        private bool Modified;
        private static MultiEditor Parent;

        private static Brush FloorBrush = new SolidBrush(Color.FromArgb(96, 32, 192, 32));

        public MultiEditorComponentList(MultiComponentList list, MultiEditor parent)
        {
            Parent = parent;
            Width = list.Width;
            Height = list.Height;
            Tiles = new ArrayList[Width][];
            for (int x = 0; x < Width; ++x)
            {
                Tiles[x] = new ArrayList[Height];
                for (int y = 0; y < Height; ++y)
                {
                    Tiles[x][y] = new ArrayList();
                    for (int i = 0; i < list.Tiles[x][y].Length; i++)
                    {
                        MultiTile tile = new MultiTile(list.Tiles[x][y][i].ID-0x4000, list.Tiles[x][y][i].Z);
                        Tiles[x][y].Add(tile);
                    }
                }
            }
            Modified = true;
            RecalcMinMax();
        }
        
        /// <summary>
        /// Adds an <see cref="MultiTile"/> to specific location
        /// </summary>
        public void AddTile(int x, int y, int z, int id)
        {
            if ((x > Width) || (y > Height))
                return;
            Tiles[x][y].Add(new MultiTile(id,z));
            Tiles[x][y].Sort();
            Modified = true;
            RecalcMinMax();
        }

        /// <summary>
        /// Removes specific <see cref="MultiTile"/>
        /// </summary>
        public void RemoveTile(MultiTile tile)
        {
            if (Width == 0 || Height == 0)
                return;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int i = 0; i < Tiles[x][y].Count; i++)
                    {
                        if (tile == Tiles[x][y][i])
                        {
                            Tiles[x][y].RemoveAt(i);
                            Modified = true;
                            RecalcMinMax();
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Recalcs Bitmap size
        /// </summary>
        private void RecalcMinMax()
        {
            xMin = yMin = zMin = 1000;
            xMax = yMax = zMax = -1000;
            

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    ArrayList tiles = Tiles[x][y];

                    for (int i = 0; i < tiles.Count; ++i)
                    {
                        MultiTile tile = (MultiTile)tiles[i];
                        Bitmap bmp = Art.GetStatic(tile.ID);

                        if (bmp == null)
                            continue;

                        int px = (x - y) * 22;
                        int py = (x + y) * 22;

                        px -= (bmp.Width / 2);
                        py -= tile.Z * 4;
                        py -= bmp.Height;

                        if (px < xMin)
                            xMin = px;

                        if (py < yMin)
                            yMin = py;

                        px += bmp.Width;
                        py += bmp.Height;

                        if (px > xMax)
                            xMax = px;
                        if (py > yMax)
                            yMax = py;

                        if (tile.Z > zMax)
                            zMax = tile.Z;
                        if (tile.Z < zMin)
                            zMin = tile.Z;
                    }
                }
            }
            Modified = false;
        }

        /// <summary>
        /// Gets <see cref="MultiTile"/> from given Pixel Location
        /// </summary>
        /// <param name="mouseLoc"></param>
        /// <param name="maxheight"></param>
        /// <returns></returns>
        public MultiTile GetSelected(Point mouseLoc,int maxheight)
        {
            if (Width == 0 || Height == 0)
                return null;
            MultiTile selected=null;
            if (mouseLoc != Point.Empty)
            {
                for (int x = 0; x < Width; ++x)
                {
                    for (int y = 0; y < Height; ++y)
                    {
                        ArrayList tiles = Tiles[x][y];

                        for (int i = 0; i < tiles.Count; ++i)
                        {
                            MultiTile tile = (MultiTile)tiles[i];
                            Bitmap bmp = Art.GetStatic(tile.ID);
                            if (bmp == null)
                                continue;
                            if ((tile.Z) > maxheight)
                                continue;
                            int px = (x - y) * 22;
                            int py = (x + y) * 22;

                            px -= (bmp.Width / 2);
                            py -= tile.Z * 4;
                            py -= bmp.Height;
                            px -= xMin;
                            py -= yMin;
                            px += 22;
                            py += 22;

                            if (((mouseLoc.X > px) && (mouseLoc.X < (px + bmp.Width))) &&
                                ((mouseLoc.Y > py) && (mouseLoc.Y < (py + bmp.Height))))
                            {
                                //Check for transparent part
                                Color p = bmp.GetPixel(mouseLoc.X - px, mouseLoc.Y - py);
                                if (!((p.R == 0) && (p.G == 0) && (p.B == 0)))
                                    selected = tile;
                            }
                        }
                    }
                }
            }
            return selected;
        }

        /// <summary>
        /// Gets Bitmap of Multi and sets HoverTile
        /// </summary>
        /// <param name="maxheight"></param>
        /// <param name="mouseLoc"></param>
        /// <param name="drawFloor"></param>
        /// <returns></returns>
        public Bitmap GetImage(int maxheight, Point mouseLoc, bool drawFloor)
        {
            if (Width == 0 || Height == 0)
                return null;

            if (Modified)
                RecalcMinMax();

            Parent.HoverTile = GetSelected(mouseLoc, maxheight);

            Bitmap canvas = new Bitmap(xMax - xMin+88, yMax - yMin+88);
            Graphics gfx = Graphics.FromImage(canvas);
            gfx.Clear(Color.White);
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    ArrayList tiles = Tiles[x][y];
                    bool floordrawed = false;
                    for (int i = 0; i < tiles.Count; ++i)
                    {
                        MultiTile tile = (MultiTile)tiles[i];
                        if ((drawFloor) && (!floordrawed))
                        {
                            if (tile.Z >= Parent.DrawFloorZ)
                            {
                                floordrawed = true;
                                int fx = (x - y) * 22;
                                int fy = (x + y) * 22;
                                fx -= (44 / 2);
                                fy -= Parent.DrawFloorZ * 4;
                                fy -= 44;
                                fx -= xMin;
                                fy -= yMin;
                                fx += 22; //Mod for a bit of gap
                                fy += 22;
                                gfx.FillPolygon(FloorBrush, new Point[]{
                                    new Point(fx+22,fy),
                                    new Point(fx+44,fy+22),
                                    new Point(fx+22,fy+44),
                                    new Point(fx,fy+22)});
                            }
                        }
                        
                        Bitmap bmp = Art.GetStatic(tile.ID);

                        if (bmp == null)
                            continue;
                        if ((tile.Z) > maxheight)
                            continue;
                        int px = (x - y) * 22;
                        int py = (x + y) * 22;

                        px -= (bmp.Width / 2);
                        py -= tile.Z * 4;
                        py -= bmp.Height;
                        px -= xMin;
                        py -= yMin;
                        py += 22; //Mod for a bit of gap
                        px += 22;

                        if ((Parent.HoverTile != null) && (Parent.HoverTile == tile))
                            gfx.DrawImage(bmp, new Rectangle(px, py, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, MultiTile.HoverColor);
                        else if ((Parent.SelectedTile != null) && (Parent.SelectedTile == tile))
                            gfx.DrawImage(bmp, new Rectangle(px, py, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, MultiTile.SelectedColor);
                        else
                            gfx.DrawImageUnscaled(bmp, px, py, bmp.Width, bmp.Height);
                    }
                    if ((drawFloor) && (!floordrawed))
                    {
                        floordrawed = true;
                        int fx = (x - y) * 22;
                        int fy = (x + y) * 22;
                        fx -= (44 / 2);
                        fy -= Parent.DrawFloorZ * 4;
                        fy -= 44;
                        fx -= xMin;
                        fy -= yMin;
                        fy += 22; //Mod for a bit of gap
                        fx += 22;
                        gfx.FillPolygon(FloorBrush, new Point[]{
                                new Point(fx+22,fy),
                                new Point(fx+44,fy+22),
                                new Point(fx+22,fy+44),
                                new Point(fx,fy+22)});
                    }
                }
            }
            gfx.Dispose();

            return canvas;
        }
    }

    public class MultiTile : IComparable
    {
        public int ID { get; private set; }
        public int Z { get; private set; }
        public int Height { get { return TileData.ItemTable[ID].Height; } }

        private static ImageAttributes m_HoverColor = null;
        private static ColorMatrix m_HoverMatrix = new ColorMatrix(new float[5][]
			{
                new float[5] {1, 0, 0, 0, 0},
                new float[5] {0, 1, 0, 0, 0},
                new float[5] {0, 0, 1, 0, 0},
                new float[5] {0, 0, 0, 1, 0},
                new float[5] {.0f, .0f, .80f, .0f, 1}
			});
        private static ImageAttributes m_SelectedColor = null;
        private static ColorMatrix m_SelectedMatrix = new ColorMatrix(new float[5][]
			{
                new float[5] {1, 0, 0, 0, 0},
                new float[5] {0, 1, 0, 0, 0},
                new float[5] {0, 0, 1, 0, 0},
                new float[5] {0, 0, 0, 1, 0},
                new float[5] {.80f, .0f, .0f, .0f, 1}
			});
        private static ImageAttributes m_DrawColor = null;
        private static ColorMatrix m_DrawMatrix = new ColorMatrix(new float[5][]
			{
                new float[5]{ 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
				new float[5]{ 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
				new float[5]{ 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
				new float[5]{ 0.0f, 0.0f, 0.0f, 0.5f, 0.0f },
				new float[5]{ 0.0f, 0.0f, 0.0f, 0.0f, 1.0f }
			});

        public static ImageAttributes DrawColor { get { return m_DrawColor; } }
        public static ImageAttributes SelectedColor { get { return m_SelectedColor; } }
        public static ImageAttributes HoverColor { get { return m_HoverColor; } }

        public MultiTile(int id, int z)
        {
            ID = id;
            Z = z;
            if (m_HoverColor == null)
            {
                m_HoverColor = new ImageAttributes();
                m_HoverColor.SetColorMatrix(m_HoverMatrix);
            }
            if (m_SelectedColor == null)
            {
                m_SelectedColor = new ImageAttributes();
                m_SelectedColor.SetColorMatrix(m_SelectedMatrix);
            }
            if (m_DrawColor == null)
            {
                m_DrawColor = new ImageAttributes();
                m_DrawColor.SetColorMatrix(m_DrawMatrix);
            }
        }

        public void Set(int id, int z)
        {
            ID = id;
            Z = z;
        }

        public int CompareTo(object x)
        {
            if (x == null)
                return 1;

            if (!(x is MultiTile))
                throw new ArgumentNullException();

            MultiTile a = (MultiTile)x;

            if (Z > a.Z)
                return 1;
            else if (a.Z > Z)
                return -1;

            ItemData ourData = TileData.ItemTable[ID];
            ItemData theirData = TileData.ItemTable[a.ID];

            if (ourData.Height > theirData.Height)
                return 1;
            else if (theirData.Height > ourData.Height)
                return -1;

            if (ourData.Background && !theirData.Background)
                return -1;
            else if (theirData.Background && !ourData.Background)
                return 1;

            return 0;
        }
    }
}
