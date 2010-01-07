using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace Ultima
{
    public sealed class TileMatrix
    {
        private HuedTile[][][][][] m_StaticTiles;
        private Tile[][][] m_LandTiles;
        private bool[][] m_RemovedStaticBlock;
        private Object[][] m_StaticTiles_ToAdd;

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

        public void CloseStreams()
        {
            if (m_Map != null)
                m_Map.Close();
            if (m_Index != null)
                m_Index.Close();
            if (m_Statics != null)
                m_Statics.Close();
        }

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
        private static byte[] m_Buffer;
        private unsafe HuedTile[][][] ReadStaticBlock(int x, int y)
        {
            try
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
                    return EmptyStaticBlock;

                m_Index.Seek(((x * BlockHeight) + y) * 12, SeekOrigin.Begin);
                int lookup = m_IndexReader.ReadInt32();
                int length = m_IndexReader.ReadInt32();

                if (lookup < 0 || length <= 0)
                    return EmptyStaticBlock;
                else
                {
                    int count = length / 7;

                    m_Statics.Seek(lookup, SeekOrigin.Begin);

                    if (m_Buffer == null || m_Buffer.Length < length)
                        m_Buffer = new byte[length];

                    GCHandle gc = GCHandle.Alloc(m_Buffer, GCHandleType.Pinned);
                    try
                    {
                        m_Statics.Read(m_Buffer, 0, length);

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

                        for (int i = 0; i < count; ++i)
                        {
                            IntPtr ptr = new IntPtr((long)gc.AddrOfPinnedObject() + i * sizeof(StaticTile));
                            StaticTile cur = (StaticTile)Marshal.PtrToStructure(ptr, typeof(StaticTile));
                            if (Art.IsUOSA())
                                lists[cur.m_X & 0x7][cur.m_Y & 0x7].Add((ushort)(cur.m_ID & 0x7FFF), cur.m_Hue, cur.m_Z);
                            else
                                lists[cur.m_X & 0x7][cur.m_Y & 0x7].Add((ushort)(cur.m_ID & 0x3FFF), cur.m_Hue, cur.m_Z);
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
                    finally
                    {
                        gc.Free();
                    }
                }
            }
            finally
            {
                //if (m_Index != null)
                //    m_Index.Close();
                //if (m_Statics != null)
                //    m_Statics.Close();
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

                GCHandle gc = GCHandle.Alloc(tiles, GCHandleType.Pinned);
                try
                {
                    if (m_Buffer == null || m_Buffer.Length < 192 )
                        m_Buffer = new byte[192];

                    m_Map.Read(m_Buffer, 0, 192);

                    Marshal.Copy(m_Buffer, 0, gc.AddrOfPinnedObject(), 192);
                }
                finally
                {
                    gc.Free();
                }
                //m_Map.Close();
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
            if (m_StaticTiles_ToAdd[blocky] == null)
                return false;
            if (m_StaticTiles_ToAdd[blocky][blockx] == null)
                return false;
            return true;
        }

        public void AddPendingStatic(int blockx, int blocky, StaticTile toadd)
        {
            if (m_StaticTiles_ToAdd == null)
                m_StaticTiles_ToAdd = new Object[BlockHeight][];
            if (m_StaticTiles_ToAdd[blocky] == null)
                m_StaticTiles_ToAdd[blocky] = new Object[BlockWidth];
            if (m_StaticTiles_ToAdd[blocky][blockx] == null)
                m_StaticTiles_ToAdd[blocky][blockx] = new ArrayList();
            ((ArrayList)m_StaticTiles_ToAdd[blocky][blockx]).Add(toadd);
        }

        public StaticTile[] GetPendingStatics(int blockx, int blocky)
        {
            if (m_StaticTiles_ToAdd == null)
                return null;
            if (m_StaticTiles_ToAdd[blocky] == null)
                return null;
            if (m_StaticTiles_ToAdd[blocky][blockx] == null)
                return null;

            return (StaticTile[])((ArrayList)m_StaticTiles_ToAdd[blocky][blockx]).ToArray(typeof(StaticTile));
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
        public ushort m_ID;
        public byte m_X;
        public byte m_Y;
        public sbyte m_Z;
        public short m_Hue;
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    public struct HuedTile
    {
        internal sbyte m_Z;
        internal ushort m_ID;
        internal int m_Hue;

        public ushort ID { get { return m_ID; } set { m_ID = value; } }
        public int Hue { get { return m_Hue; } set { m_Hue = value; } }
        public int Z { get { return m_Z; } set { m_Z = (sbyte)value; } }

        public HuedTile(ushort id, short hue, sbyte z)
        {
            m_ID = id;
            m_Hue = hue;
            m_Z = z;
        }

        public void Set(ushort id, short hue, sbyte z)
        {
            m_ID = id;
            m_Hue = hue;
            m_Z = z;
        }
    }

    public struct MTile : IComparable
    {
        internal ushort m_ID;
        internal sbyte m_Z;
        internal sbyte m_Flag;
        internal int m_Solver;

        public ushort ID { get { return m_ID; } }
        public int Z { get { return m_Z; } set { m_Z = (sbyte)value; } }

        public int Flag { get { return m_Flag; } set { m_Flag = (sbyte)value; } }
        public int Solver { get { return m_Solver; } set { m_Solver = value; } }

        public MTile(ushort id, sbyte z)
        {
            m_ID = id;
            m_Z = z;
            m_Flag = 1;
            m_Solver = 0;
        }

        public MTile(ushort id, sbyte z, sbyte flag)
        {
            m_ID = id;
            m_Z = z;
            m_Flag = flag;
            m_Solver = 0;
        }

        public void Set(ushort id, sbyte z)
        {
            m_ID = id;
            m_Z = z;
        }

        public void Set(ushort id, sbyte z, sbyte flag)
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

            ItemData ourData = TileData.ItemTable[ID];
            ItemData theirData = TileData.ItemTable[a.ID];

            int ourTreshold=0;
            if (ourData.Height > 0)
                ++ourTreshold;
            if (!ourData.Background)
                ++ourTreshold;
            int ourZ = Z;
            int theirTreshold = 0;
            if (theirData.Height > 0)
                ++theirTreshold;
            if (!theirData.Background)
                ++theirTreshold;
            int theirZ = a.Z;

            ourZ += ourTreshold;
            theirZ += theirTreshold;
            int res = ourZ - theirZ;
            if (res == 0)
                res = ourTreshold - theirTreshold;
            if (res == 0)
                res = Solver - a.Solver;
            return res;
        }

    }
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    public struct Tile : IComparable
    {
        internal ushort m_ID;
        internal sbyte m_Z;

        public ushort ID { get { return m_ID; } }
        public int Z { get { return m_Z; } set { m_Z = (sbyte)value; } }

        public Tile(ushort id, sbyte z)
        {
            m_ID = id;
            m_Z = z;
        }

        public void Set(ushort id, sbyte z)
        {
            m_ID = id;
            m_Z = z;
        }

        public void Set(ushort id, sbyte z, sbyte flag)
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

            ItemData ourData = TileData.ItemTable[m_ID];
            ItemData theirData = TileData.ItemTable[a.m_ID];

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