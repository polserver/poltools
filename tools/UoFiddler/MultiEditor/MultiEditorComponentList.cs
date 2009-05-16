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
		#region Fields (5) 

        private static Brush FloorBrush = new SolidBrush(Color.FromArgb(96, 32, 192, 32));
        public const int GapXMod = 44;
        public const int GapYMod = 22;
        private bool Modified;
        private static MultiEditor Parent;

		#endregion Fields 

		#region Constructors (2) 

        /// <summary>
        /// Create a blank ComponentList
        /// </summary>
        public MultiEditorComponentList(int width, int height, MultiEditor parent)
        {
            Parent = parent;
            Width = width;
            Height = height;
            Tiles = new ArrayList[Width][];
            for (int x = 0; x < Width; ++x)
            {
                Tiles[x] = new ArrayList[Height];
                for (int y = 0; y < Height; ++y)
                {
                    Tiles[x][y] = new ArrayList();
                }
            }
            Modified = true;
            RecalcMinMax();
        }

        /// <summary>
        /// Create a ComponentList from UltimaSDK
        /// </summary>
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
                        MultiTile tile = new MultiTile(list.Tiles[x][y][i].ID - 0x4000, list.Tiles[x][y][i].Z);
                        Tiles[x][y].Add(tile);
                    }
                }
            }
            Modified = true;
            RecalcMinMax();
        }

		#endregion Constructors 

		#region Properties (13) 

        public int Height { get; private set; }

        public ArrayList[][] Tiles { get; private set; }

        public int Width { get; private set; }

        public int xMax { get; private set; }

        public int xMaxOrg { get; private set; }

        public int xMin { get; private set; }

        public int xMinOrg { get; private set; }

        public int yMax { get; private set; }

        public int yMaxOrg { get; private set; }

        public int yMin { get; private set; }

        public int yMinOrg { get; private set; }

        public int zMax { get; private set; }

        public int zMin { get; private set; }

		#endregion Properties 

		#region Methods (12) 

		// Public Methods (11) 

        /// <summary>
        /// Export to given multi id
        /// </summary>
        public void AddToSDKComponentList(int id)
        {
            Ultima.Multis.Add(id, ConvertToSDK());
            FiddlerControls.Options.ChangedUltimaClass["Multis"] = true;
            FiddlerControls.Options.FireMultiChangeEvent(this, id);
        }

        public MultiComponentList ConvertToSDK()
        {
            int count = 0;
            TileList[][] tiles = new TileList[Width][];
            for (int x = 0; x < Width; ++x)
            {
                tiles[x] = new TileList[Height];
                for (int y = 0; y < Height; ++y)
                {
                    tiles[x][y] = new TileList();
                    for (int i = 0; i < Tiles[x][y].Count; i++)
                    {
                        MultiTile tile = (MultiTile)Tiles[x][y][i];
                        tiles[x][y].Add((short)(tile.ID + 0x4000), (sbyte)tile.Z);
                        count++;
                    }
                }
            }
            return new MultiComponentList(tiles, count, Width, Height);
        }

        /// <summary>
        /// Gets Bitmap of Multi and sets HoverTile
        /// </summary>
        public Bitmap GetImage(int maxheight, Point mouseLoc, bool drawFloor)
        {
            if (Width == 0 || Height == 0)
                return null;

            if (Modified)
                RecalcMinMax();
            xMin = xMinOrg;
            xMax = xMaxOrg;
            yMin = yMinOrg;
            yMax = yMaxOrg;
            
            if (drawFloor)
            {
                int floorzmod = -Parent.DrawFloorZ * 4 - 44;
                if (yMin > floorzmod)
                    yMin = floorzmod;
                floorzmod = (Width + Height) * 22 - Parent.DrawFloorZ * 4;
                if (yMaxOrg < floorzmod)
                    yMax = floorzmod;
            }
            Parent.HoverTile = GetSelected(mouseLoc, maxheight, drawFloor);

            Bitmap canvas = new Bitmap(xMax - xMin + 88, yMax - yMin + 66);
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
                                fx += GapXMod; //Mod for a bit of gap
                                fy += GapYMod;
                                gfx.FillPolygon(FloorBrush, new Point[]{
                                    new Point(fx+22,fy),
                                    new Point(fx+44,fy+22),
                                    new Point(fx+22,fy+44),
                                    new Point(fx,fy+22)});
                                gfx.DrawPolygon(Pens.White, new Point[]{
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
                        py += GapYMod; //Mod for a bit of gap
                        px += GapXMod;

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
                        fy += GapYMod; //Mod for a bit of gap
                        fx += GapXMod;
                        gfx.FillPolygon(FloorBrush, new Point[]{
                                new Point(fx+22,fy),
                                new Point(fx+44,fy+22),
                                new Point(fx+22,fy+44),
                                new Point(fx,fy+22)});
                        gfx.DrawPolygon(Pens.White, new Point[]{
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

        /// <summary>
        /// Gets <see cref="MultiTile"/> from given Pixel Location
        /// </summary>
        public MultiTile GetSelected(Point mouseLoc, int maxheight, bool drawFloor)
        {
            if (Width == 0 || Height == 0)
                return null;
            MultiTile selected = null;
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
                            if ((drawFloor) && (Parent.DrawFloorZ > tile.Z))
                                continue;
                            int px = (x - y) * 22;
                            int py = (x + y) * 22;

                            px -= (bmp.Width / 2);
                            py -= tile.Z * 4;
                            py -= bmp.Height;
                            px -= xMin;
                            py -= yMin;
                            px += GapXMod;
                            py += GapYMod;

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
        /// Resizes Multi size
        /// </summary>
        public void Resize(int width, int height)
        {
            ArrayList[][] copytiles = new ArrayList[width][];
            for (int x = 0; x < width; x++)
            {
                copytiles[x] = new ArrayList[height];
                for (int y = 0; y < height; y++)
                {
                    copytiles[x][y] = new ArrayList();
                    if ((x < Width) && (y < Height))
                    {
                        for (int i = 0; i < Tiles[x][y].Count; i++)
                        {
                            MultiTile tile = (MultiTile)Tiles[x][y][i];
                            copytiles[x][y].Add(new MultiTile(tile.ID, tile.Z));
                        }
                        Tiles[x][y].Clear();
                    }
                }
            }
            Width = width;
            Height = height;
            Tiles = copytiles;
            Modified = true;
            RecalcMinMax();
        }

        /// <summary>
        /// Adds an <see cref="MultiTile"/> to specific location
        /// </summary>
        public void TileAdd(int x, int y, int z, int id)
        {
            if ((x > Width) || (y > Height))
                return;
            Tiles[x][y].Add(new MultiTile(id, z));
            Tiles[x][y].Sort();
            Modified = true;
            RecalcMinMax();
        }

        /// <summary>
        /// Gets x,y Coords of given <see cref="MultiTile"/>
        /// </summary>
        public Point TileGetCoords(MultiTile tile)
        {
            Point point = new Point();
            point = Point.Empty;
            if (Width == 0 || Height == 0)
                return point;
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    for (int i = 0; i < Tiles[x][y].Count; i++)
                    {
                        if (tile == Tiles[x][y][i])
                        {
                            point.X = x;
                            point.Y = y;
                            return point;
                        }
                    }
                }
            }
            return point;
        }

        /// <summary>
        /// Moves given <see cref="MultiTile"/>
        /// </summary>
        public void TileMove(MultiTile tile, int newx, int newy)
        {
            if (Width == 0 || Height == 0)
                return;
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    for (int i = 0; i < Tiles[x][y].Count; i++)
                    {
                        if (tile == Tiles[x][y][i])
                        {
                            Tiles[newx][newy].Add(tile);
                            Tiles[newx][newy].Sort();
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
        /// Removes specific <see cref="MultiTile"/>
        /// </summary>
        public void TileRemove(MultiTile tile)
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
        /// Alters Z level for given <see cref="MultiTile"/>
        /// </summary>
        public void TileZMod(MultiTile tile, int modz)
        {
            tile.Z += modz;
            if (tile.Z > 127)
                tile.Z = 127;
            if (tile.Z < -128)
                tile.Z = -128;
            Modified = true;
            Point point = TileGetCoords(tile);
            if (point != Point.Empty)
                Tiles[point.X][point.Y].Sort();
            RecalcMinMax();
        }

        /// <summary>
        /// Sets Z value of given <see cref="MultiTile"/>
        /// </summary>
        public void TileZSet(MultiTile tile, int setz)
        {
            tile.Z = setz;
            if (tile.Z > 127)
                tile.Z = 127;
            if (tile.Z < -128)
                tile.Z = -128;
            Modified = true;
            Point point = TileGetCoords(tile);
            if (point != Point.Empty)
                Tiles[point.X][point.Y].Sort();
            RecalcMinMax();
        }
		// Private Methods (1) 

        /// <summary>
        /// Recalcs Bitmap size
        /// </summary>
        private void RecalcMinMax()
        {
            //CalcEdgeTiles
            yMin = -44; // 0,0
            yMax = (Width + Height) * 22; // width,height
            xMin = -Height * 22 - 22; // 0,height
            xMax = Width * 22 + 22; // width,0
            zMin = 127;
            zMax = -128;

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
            xMinOrg = xMin;
            xMaxOrg = xMax;
            yMinOrg = yMin;
            yMaxOrg = yMax;
        }

		#endregion Methods 
    }

    public class MultiTile : IComparable
    {
		#region Fields (6) 

        private static ImageAttributes m_DrawColor = null;
        private static ColorMatrix m_DrawMatrix = new ColorMatrix(new float[5][]
			{
                new float[5]{ 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
				new float[5]{ 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
				new float[5]{ 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
				new float[5]{ 0.0f, 0.0f, 0.0f, 0.5f, 0.0f },
				new float[5]{ 0.0f, 0.0f, 0.0f, 0.0f, 1.0f }
			});
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

		#endregion Fields 

		#region Constructors (2) 

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

        public MultiTile()
        {
            ID = -1;
        }

		#endregion Constructors 

		#region Properties (6) 

        public static ImageAttributes DrawColor { get { return m_DrawColor; } }

        public int Height { get { return TileData.ItemTable[ID].Height; } }

        public static ImageAttributes HoverColor { get { return m_HoverColor; } }

        public int ID { get; private set; }

        public static ImageAttributes SelectedColor { get { return m_SelectedColor; } }

        public int Z { get; set; }

		#endregion Properties 

		#region Methods (2) 

		// Public Methods (2) 

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

        public void Set(int id, int z)
        {
            ID = id;
            Z = z;
        }

		#endregion Methods 
    }
}
