using System;
using System.IO;

namespace Ultima
{
    public sealed class TileMatrix
    {
        private HuedTile[][][][][] m_StaticTiles;
        private HuedTile[][][][][] m_StaticTiles_ToAdd;
        private Tile[][][] m_LandTiles;
        private bool[][] m_RemovedStaticBlock;

        public static Tile[] InvalidLandBlock { get; private set; }
        public static HuedTile[][][] EmptyStaticBlock { get; private set; }

        private FileStream m_Map;

        private FileStream m_Index;
        private BinaryReader m_IndexReader;

        private FileStream m_Statics;

        public TileMatrixPatch Patch { get; private set; }

        public int BlockWidth { get; private set; }

        public int BlockHeight { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        private string mapPath;
        private string indexPath;
        private string staticsPath;

        public TileMatrix(int fileIndex, int mapID, int width, int height, string path)
        {
            Width = width;
            Height = height;
            BlockWidth = width >> 3;
            BlockHeight = height >> 3;

            if (path == null)
                mapPath = Files.GetFilePath("map{0}.mul", fileIndex);
            else
            {
                mapPath = Path.Combine(path, String.Format("map{0}.mul", fileIndex));
                if (!File.Exists(mapPath))
                    mapPath = null;
            }

            if (path == null)
                indexPath = Files.GetFilePath("staidx{0}.mul", fileIndex);
            else
            {
                indexPath = Path.Combine(path, String.Format("staidx{0}.mul", fileIndex));
                if (!File.Exists(indexPath))
                    indexPath = null;
            }

            if (path == null)
                staticsPath = Files.GetFilePath("statics{0}.mul", fileIndex);
            else
            {
                staticsPath = Path.Combine(path, String.Format("statics{0}.mul", fileIndex));
                if (!File.Exists(staticsPath))
                    staticsPath = null;
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

            InvalidLandBlock = new Tile[196];

            m_LandTiles = new Tile[BlockWidth][][];
            m_StaticTiles = new HuedTile[BlockWidth][][][][];

            Patch = new TileMatrixPatch(this, mapID, path);
        }


        public void SetStaticBlock(int x, int y, HuedTile[][][] value)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight)
                return;

            if (m_StaticTiles[x] == null)
                m_StaticTiles[x] = new HuedTile[BlockHeight][][][];

            m_StaticTiles[x][y] = value;
        }

        public void AddStaticTile(int blockx, int blocky, int x, int y, HuedTile value)
        {
            if (m_StaticTiles[blockx] == null)
                m_StaticTiles[blockx] = new HuedTile[BlockHeight][][][];
            HuedTile[][][] tiles = new HuedTile[8][][];
            tiles = m_StaticTiles[blockx][blocky];
            if (tiles == null)
                tiles = ReadStaticBlock(blockx, blocky);
            if (tiles == EmptyStaticBlock)
            {
                tiles = new HuedTile[8][][];
                for (int i = 0; i < 8; ++i)
                {
                    tiles[i] = new HuedTile[8][];
                    for (int j = 0; j < 8; ++j)
                    {
                        tiles[i][j] = new HuedTile[0];
                    }
                }
                tiles[x][y] = new HuedTile[1];
                tiles[x][y][0] = value;
            }
            else
            {
                HuedTile[] old = tiles[x][y];
                tiles[x][y] = new HuedTile[old.Length + 1];
                old.CopyTo(tiles[x][y], 0);
                tiles[x][y][old.Length] = value;
            }
            m_StaticTiles[blockx][blocky] = tiles;
            if (m_StaticTiles_ToAdd == null)
                m_StaticTiles_ToAdd = new HuedTile[BlockWidth][][][][];
            if (m_StaticTiles_ToAdd[blockx]==null)
                m_StaticTiles_ToAdd[blockx] = new HuedTile[BlockHeight][][][];
            if (m_StaticTiles_ToAdd[blockx][blocky] == null)
            {
                m_StaticTiles_ToAdd[blockx][blocky] = new HuedTile[8][][];
                for (int i = 0; i < 8; ++i)
                {
                    m_StaticTiles_ToAdd[blockx][blocky][i] = new HuedTile[8][];
                    for (int j = 0; j < 8; ++j)
                    {
                        m_StaticTiles_ToAdd[blockx][blocky][i][j] = new HuedTile[0];
                    }
                }
                m_StaticTiles_ToAdd[blockx][blocky][x][y] = new HuedTile[1];
                m_StaticTiles_ToAdd[blockx][blocky][x][y][0] = value;
            }
            else
            {
                HuedTile[] old = m_StaticTiles_ToAdd[blockx][blocky][x][y];
                m_StaticTiles_ToAdd[blockx][blocky][x][y] = new HuedTile[old.Length + 1];
                old.CopyTo(m_StaticTiles_ToAdd[blockx][blocky][x][y], 0);
                m_StaticTiles_ToAdd[blockx][blocky][x][y][old.Length] = value;
            }

            if (IsStaticBlockRemoved(blockx, blocky))
                m_RemovedStaticBlock[blockx][blocky] = false;
        }

        public HuedTile[][][] GetStaticBlock(int x, int y)
        {
            return GetStaticBlock(x, y, true);
        }
        public HuedTile[][][] GetStaticBlock(int x, int y, bool patch)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight)
                return EmptyStaticBlock;

            if (m_StaticTiles[x] == null)
                m_StaticTiles[x] = new HuedTile[BlockHeight][][][];

            HuedTile[][][] tiles = m_StaticTiles[x][y];

            if (tiles == null)
                tiles = m_StaticTiles[x][y] = ReadStaticBlock(x, y);

            if ((Map.UseDiff) && (patch))
            {
                if (Patch.StaticBlocksCount > 0)
                {
                    if (Patch.StaticBlocks[x] != null)
                    {
                        if (Patch.StaticBlocks[x][y] != null)
                            tiles = Patch.StaticBlocks[x][y];
                    }
                }
            }
            return tiles;
        }
        public HuedTile[] GetStaticTiles(int x, int y, bool patch)
        {
            return GetStaticBlock(x >> 3, y >> 3,patch)[x & 0x7][y & 0x7];
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
            return GetLandBlock(x, y, true);
        }
        public Tile[] GetLandBlock(int x, int y, bool patch)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight) 
                return InvalidLandBlock;

            if (m_LandTiles[x] == null)
                m_LandTiles[x] = new Tile[BlockHeight][];

            Tile[] tiles = m_LandTiles[x][y];

            if (tiles == null)
                tiles = m_LandTiles[x][y] = ReadLandBlock(x, y);

            if ((Map.UseDiff) && (patch))
            {
                if (Patch.LandBlocksCount > 0)
                {
                    if (Patch.LandBlocks[x] != null)
                    {
                        if (Patch.LandBlocks[x][y] != null)
                            tiles = Patch.LandBlocks[x][y];
                    }
                }
            }
            return tiles;
        }
        public Tile GetLandTile(int x, int y, bool patch)
        {
            return GetLandBlock(x >> 3, y >> 3,patch)[((y & 0x7) << 3) + (x & 0x7)];
        }

        public Tile GetLandTile(int x, int y)
        {
            return GetLandBlock(x >> 3, y >> 3)[((y & 0x7) << 3) + (x & 0x7)];
        }

        private static HuedTileList[][] m_Lists;

        private unsafe HuedTile[][][] ReadStaticBlock(int x, int y)
        {
            if (m_Index == null || !m_Index.CanRead || !m_Index.CanSeek)
            {
                if (indexPath == null)
                    m_Index = null;
                else
                {
                    m_Index = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    m_IndexReader = new BinaryReader(m_Index);
                }
            }
            if (m_Statics == null || !m_Statics.CanRead || !m_Statics.CanSeek)
            {
                if (staticsPath == null)
                    m_Statics = null;
                else
                    m_Statics = new FileStream(staticsPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            if (m_Index == null || m_Statics == null)
            {
                if (m_Index != null)
                    m_Index.Close();
                if (m_Statics != null)
                    m_Statics.Close();
                return EmptyStaticBlock;
            }

            m_Index.Seek(((x * BlockHeight) + y) * 12, SeekOrigin.Begin);
            int lookup = m_IndexReader.ReadInt32();
            int length = m_IndexReader.ReadInt32();

            if (lookup < 0 || length <= 0)
            {
                m_IndexReader.Close();
                m_Statics.Close();
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
                    m_IndexReader.Close();
                    m_Statics.Close();
                    return tiles;
                }
            }
        }

        private unsafe Tile[] ReadLandBlock(int x, int y)
        {
            if (m_Map == null || !m_Map.CanRead || !m_Map.CanSeek)
            {
                if (mapPath == null)
                    m_Map = null;
                else
                    m_Map = new FileStream(mapPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            Tile[] tiles = new Tile[64];
            if (m_Map != null)
            {
                m_Map.Seek(((x * BlockHeight) + y) * 196 + 4, SeekOrigin.Begin);

                fixed (Tile* pTiles = tiles)
                {
                    NativeMethods._lread(m_Map.SafeFileHandle, pTiles, 192);
                }
                m_Map.Close();
            }

            return tiles;
        }

        public void RemoveStaticBlock(int blockx, int blocky)
        {
            if (m_RemovedStaticBlock == null)
                m_RemovedStaticBlock = new bool[BlockWidth][];
            if (m_RemovedStaticBlock[blockx]==null)
                m_RemovedStaticBlock[blockx] = new bool[BlockHeight];
            m_RemovedStaticBlock[blockx][blocky] = true;
            if (m_StaticTiles[blockx] == null)
                m_StaticTiles[blockx] = new HuedTile[BlockHeight][][][];
            m_StaticTiles[blockx][blocky]=EmptyStaticBlock;
        }

        public bool IsStaticBlockRemoved(int blockx, int blocky)
        {
            if (m_RemovedStaticBlock == null)
                return false;
            if (m_RemovedStaticBlock[blockx] == null)
                return false;
            return m_RemovedStaticBlock[blockx][blocky];
        }

        public bool PendingStatic(int blockx, int blocky)
        {
            if (m_StaticTiles_ToAdd == null)
                return false;
            if (m_StaticTiles_ToAdd[blockx] == null)
                return false;
            if (m_StaticTiles_ToAdd[blockx][blocky] == null)
                return false;
            return true;
        }

        public StaticTile[] GetPendingStatics(int blockx, int blocky)
        {
            if (m_StaticTiles_ToAdd == null)
                return null;
            if (m_StaticTiles_ToAdd[blockx] == null)
                return null;
            if (m_StaticTiles_ToAdd[blockx][blocky] == null)
                return null;
            HuedTile[][][] tile = m_StaticTiles_ToAdd[blockx][blocky];
            int count = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    count += tile[x][y].Length;
                }
            }
            if (count > 0)
            {
                StaticTile[] tiles = new StaticTile[count];
                int j=0;
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        for (int i = 0; i < tile[x][y].Length; i++)
                        {
                            tiles[j] = new StaticTile();
                            tiles[j].m_ID = (short)tile[x][y][i].ID;
                            tiles[j].m_X = (byte)x;
                            tiles[j].m_Y = (byte)y;
                            tiles[j].m_Z = (sbyte)tile[x][y][i].Z;
                            tiles[j].m_Hue = (short)tile[x][y][i].Hue;
                            j++;
                        }
                    }
                }
                return tiles;
            }
            else
                return null;
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

        public int ID { get { return m_ID; } set { m_ID = value; } }
        public int Hue { get { return m_Hue; } set { m_Hue = value; } }
        public int Z { get { return m_Z; } set { m_Z = (sbyte)value; } }

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

    public struct MTile : IComparable
    {
        internal short m_ID;
        internal sbyte m_Z;
        internal sbyte m_Flag;

        public int ID { get { return m_ID; } }
        public int Z { get { return m_Z; } set { m_Z = (sbyte)value; } }

        public int Flag { get { return m_Flag; } set { m_Flag = (sbyte)value; } }

        public MTile(short id, sbyte z)
        {
            m_ID = id;
            m_Z = z;
            m_Flag = 1;
        }

        public MTile(short id, sbyte z, sbyte flag)
        {
            m_ID = id;
            m_Z = z;
            m_Flag = flag;
        }

        public void Set(short id, sbyte z)
        {
            m_ID = id;
            m_Z = z;
        }

        public void Set(short id, sbyte z, sbyte flag)
        {
            m_ID = id;
            m_Z = z;
            m_Flag = flag;
        }

        public int CompareTo(object x)
        {
            if (x == null)
                return 1;

            if (!(x is MTile))
                throw new ArgumentNullException();

            MTile a = (MTile)x;

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
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    public struct Tile : IComparable
    {
        internal short m_ID;
        internal sbyte m_Z;

        public int ID { get { return m_ID; } }
        public int Z { get { return m_Z; } set { m_Z = (sbyte)value; } }

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

        public void Set(short id, sbyte z, sbyte flag)
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