using System;
using System.IO;

namespace Ultima
{
    public sealed class TileMatrix
    {
        private HuedTile[][][][][] m_StaticTiles;
        private Tile[][][] m_LandTiles;

        private Tile[] m_InvalidLandBlock;

        private FileStream m_Map;

        private FileStream m_Index;
        private BinaryReader m_IndexReader;

        private FileStream m_Statics;

        public TileMatrixPatch Patch { get; private set; }

        public int BlockWidth { get; private set; }

        public int BlockHeight { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public TileMatrix(int fileIndex, int mapID, int width, int height)
        {
            Width = width;
            Height = height;
            BlockWidth = width >> 3;
            BlockHeight = height >> 3;

            if (fileIndex != 0x7F)
            {
                string mapPath = Files.GetFilePath("map{0}.mul", fileIndex);

                if (mapPath != null)
                    m_Map = new FileStream(mapPath, FileMode.Open, FileAccess.Read, FileShare.Read);

                string indexPath = Files.GetFilePath("staidx{0}.mul", fileIndex);

                if (indexPath != null)
                {
                    m_Index = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    m_IndexReader = new BinaryReader(m_Index);
                }

                string staticsPath = Files.GetFilePath("statics{0}.mul", fileIndex);

                if (staticsPath != null)
                    m_Statics = new FileStream(staticsPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            EmptyStaticBlock = new HuedTile[8][][];

            for (int i = 0; i < 8; ++i)
            {
                EmptyStaticBlock[i] = new HuedTile[8][];

                for (int j = 0; j < 8; ++j)
                {
                    EmptyStaticBlock[i][j] = new HuedTile[0];
                }
            }

            m_InvalidLandBlock = new Tile[196];

            m_LandTiles = new Tile[BlockWidth][][];
            m_StaticTiles = new HuedTile[BlockWidth][][][][];

            if (Map.UseDiff)
                Patch = new TileMatrixPatch(this, mapID);

            /*for ( int i = 0; i < m_BlockWidth; ++i )
            {
                m_LandTiles[i] = new Tile[m_BlockHeight][];
                m_StaticTiles[i] = new Tile[m_BlockHeight][][][];
            }*/
        }

        public HuedTile[][][] EmptyStaticBlock { get; private set; }

        public void SetStaticBlock(int x, int y, HuedTile[][][] value)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight)
                return;

            if (m_StaticTiles[x] == null)
                m_StaticTiles[x] = new HuedTile[BlockHeight][][][];

            m_StaticTiles[x][y] = value;
        }

        public HuedTile[][][] GetStaticBlock(int x, int y)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight || m_Statics == null || m_Index == null)
                return EmptyStaticBlock;

            if (m_StaticTiles[x] == null)
                m_StaticTiles[x] = new HuedTile[BlockHeight][][][];

            HuedTile[][][] tiles = m_StaticTiles[x][y];

            if (tiles == null)
                tiles = m_StaticTiles[x][y] = ReadStaticBlock(x, y);

            return tiles;
        }

        public HuedTile[] GetStaticTiles(int x, int y)
        {
            return GetStaticBlock(x >> 3, y >> 3)[x & 0x7][y & 0x7];
        }

        public void SetLandBlock(int x, int y, Tile[] value)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight)
                return;

            if (m_LandTiles[x] == null)
                m_LandTiles[x] = new Tile[BlockHeight][];

            m_LandTiles[x][y] = value;
        }

        public Tile[] GetLandBlock(int x, int y)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight || m_Map == null) return m_InvalidLandBlock;

            if (m_LandTiles[x] == null)
                m_LandTiles[x] = new Tile[BlockHeight][];

            Tile[] tiles = m_LandTiles[x][y];

            if (tiles == null)
                tiles = m_LandTiles[x][y] = ReadLandBlock(x, y);

            return tiles;
        }

        public Tile GetLandTile(int x, int y)
        {
            return GetLandBlock(x >> 3, y >> 3)[((y & 0x7) << 3) + (x & 0x7)];
        }

        private static HuedTileList[][] m_Lists;

        private unsafe HuedTile[][][] ReadStaticBlock(int x, int y)
        {
            m_IndexReader.BaseStream.Seek(((x * BlockHeight) + y) * 12, SeekOrigin.Begin);
            int lookup = m_IndexReader.ReadInt32();
            int length = m_IndexReader.ReadInt32();

            if (lookup < 0 || length <= 0)
            {
                return EmptyStaticBlock;
            }
            else
            {
                int count = length / 7;

                m_Statics.Seek(lookup, SeekOrigin.Begin);

                StaticTile[] staTiles = new StaticTile[count];

                fixed (StaticTile* pTiles = staTiles)
                {
                    NativeMethods._lread(m_Statics.SafeFileHandle, pTiles, length);

                    if (m_Lists == null)
                    {
                        m_Lists = new HuedTileList[8][];

                        for (int i = 0; i < 8; ++i)
                        {
                            m_Lists[i] = new HuedTileList[8];

                            for (int j = 0; j < 8; ++j)
                                m_Lists[i][j] = new HuedTileList();
                        }
                    }

                    HuedTileList[][] lists = m_Lists;

                    StaticTile* pCur = pTiles, pEnd = pTiles + count;

                    while (pCur < pEnd)
                    {
                        lists[pCur->m_X & 0x7][pCur->m_Y & 0x7].Add((short)((pCur->m_ID & 0x3FFF) + 0x4000), pCur->m_Hue, pCur->m_Z);
                        ++pCur;
                    }

                    HuedTile[][][] tiles = new HuedTile[8][][];

                    for (int i = 0; i < 8; ++i)
                    {
                        tiles[i] = new HuedTile[8][];

                        for (int j = 0; j < 8; ++j)
                            tiles[i][j] = lists[i][j].ToArray();
                    }

                    return tiles;
                }
            }
        }

        private unsafe Tile[] ReadLandBlock(int x, int y)
        {
            m_Map.Seek(((x * BlockHeight) + y) * 196 + 4, SeekOrigin.Begin);

            Tile[] tiles = new Tile[64];

            fixed (Tile* pTiles = tiles)
            {
                NativeMethods._lread(m_Map.SafeFileHandle, pTiles, 192);
            }

            return tiles;
        }

        public void Dispose()
        {
            if (m_Map != null)
                m_Map.Close();

            if (m_Statics != null)
                m_Statics.Close();

            if (m_IndexReader != null)
                m_IndexReader.Close();
        }
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    public struct StaticTile
    {
        public short m_ID;
        public byte m_X;
        public byte m_Y;
        public sbyte m_Z;
        public short m_Hue;
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    public struct HuedTile
    {
        internal sbyte m_Z;
        internal int m_ID;
        internal int m_Hue;

        public int ID { get{return m_ID;} }

        public int Hue { get { return m_Hue; } }

        public int Z
        {
            get
            {
                return m_Z;
            }
            set
            {
                m_Z = (sbyte)value;
            }
        }

        public HuedTile(short id, short hue, sbyte z)
        {
            m_ID = id;
            m_Hue = hue;
            m_Z = z;
        }

        public void Set(short id, short hue, sbyte z)
        {
            m_ID = id;
            m_Hue = hue;
            m_Z = z;
        }
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    public struct Tile : IComparable
    {
        internal short m_ID;
        internal sbyte m_Z;

        public int ID
        {
            get
            {
                return m_ID;
            }
        }

        public int Z
        {
            get
            {
                return m_Z;
            }
            set
            {
                m_Z = (sbyte)value;
            }
        }

        public Tile(short id, sbyte z)
        {
            m_ID = id;
            m_Z = z;
        }

        public void Set(short id, sbyte z)
        {
            m_ID = id;
            m_Z = z;
        }

        public int CompareTo(object x)
        {
            if (x == null)
                return 1;

            if (!(x is Tile))
                throw new ArgumentNullException();

            Tile a = (Tile)x;

            if (m_Z > a.m_Z)
                return 1;
            else if (a.m_Z > m_Z)
                return -1;

            ItemData ourData = TileData.ItemTable[m_ID & 0x3FFF];
            ItemData theirData = TileData.ItemTable[a.m_ID & 0x3FFF];

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