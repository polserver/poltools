using System;
using System.IO;

namespace Ultima
{
    public sealed class TileMatrixPatch
    {
        public int LandBlocksCount { get; private set; }
        public int StaticBlocksCount { get; private set; }

        public Tile[][][] LandBlocks { get; private set; }
        public HuedTile[][][][][] StaticBlocks { get; private set; }

        private int BlockWidth;
        private int BlockHeight;

        public bool IsLandBlockPatched(int x, int y)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight)
                return false;
            if (LandBlocks[x] == null)
                return false;
            if (LandBlocks[x][y] == null)
                return false;
            return true;
        }
        public Tile[] GetLandBlock(int x, int y)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight)
                return TileMatrix.InvalidLandBlock;
            if (LandBlocks[x]==null)
                return TileMatrix.InvalidLandBlock;
            return LandBlocks[x][y];
        }

        public Tile GetLandTile(int x, int y)
        {
            return GetLandBlock(x >> 3, y >> 3)[((y & 0x7) << 3) + (x & 0x7)];
        }

        public bool IsStaticBlockPatched(int x, int y)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight)
                return false;
            if (StaticBlocks[x] == null)
                return false;
            if (StaticBlocks[x][y] == null)
                return false;
            return true;
        }

        public HuedTile[][][] GetStaticBlock(int x, int y)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight)
                return TileMatrix.EmptyStaticBlock;
            if (StaticBlocks[x] == null)
                return TileMatrix.EmptyStaticBlock;
            return StaticBlocks[x][y];
        }

        public HuedTile[] GetStaticTiles(int x, int y)
        {
            return GetStaticBlock(x >> 3, y >> 3)[x & 0x7][y & 0x7];
        }

        public TileMatrixPatch(TileMatrix matrix, int index, string path)
        {
            BlockWidth = matrix.BlockWidth;
            BlockHeight = matrix.BlockWidth;

            LandBlocksCount = StaticBlocksCount = 0;
            string mapDataPath, mapIndexPath;
            if (path == null)
            {
                mapDataPath = Files.GetFilePath("mapdif{0}.mul", index);
                mapIndexPath = Files.GetFilePath("mapdifl{0}.mul", index);
            }
            else
            {
                mapDataPath = Path.Combine(path, String.Format("mapdif{0}.mul", index));
                if (!File.Exists(mapDataPath))
                    mapDataPath = null;
                mapIndexPath = Path.Combine(path, String.Format("mapdifl{0}.mul", index));
                if (!File.Exists(mapIndexPath))
                    mapIndexPath = null;
            }

            if (mapDataPath != null && mapIndexPath != null)
            {
                LandBlocks = new Tile[matrix.BlockWidth][][];
                LandBlocksCount = PatchLand(matrix, mapDataPath, mapIndexPath);
            }

            string staDataPath, staIndexPath, staLookupPath;
            if (path == null)
            {
                staDataPath = Files.GetFilePath("stadif{0}.mul", index);
                staIndexPath = Files.GetFilePath("stadifl{0}.mul", index);
                staLookupPath = Files.GetFilePath("stadifi{0}.mul", index);
            }
            else
            {
                staDataPath = Path.Combine(path, String.Format("stadif{0}.mul", index));
                if (!File.Exists(staDataPath))
                    staDataPath = null;
                staIndexPath = Path.Combine(path, String.Format("stadifl{0}.mul", index));
                if (!File.Exists(staIndexPath))
                    staIndexPath = null;
                staLookupPath = Path.Combine(path, String.Format("stadifi{0}.mul", index));
                if (!File.Exists(staLookupPath))
                    staLookupPath = null;
            }

            if (staDataPath != null && staIndexPath != null && staLookupPath != null)
            {
                StaticBlocks = new HuedTile[matrix.BlockWidth][][][][];
                StaticBlocksCount = PatchStatics(matrix, staDataPath, staIndexPath, staLookupPath);
            }
        }

        private unsafe int PatchLand(TileMatrix matrix, string dataPath, string indexPath)
        {
            using (FileStream fsData = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read),
                              fsIndex = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (BinaryReader indexReader = new BinaryReader(fsIndex))
                {
                    int count = (int)(indexReader.BaseStream.Length / 4);

                    for (int i = 0; i < count; ++i)
                    {
                        int blockID = indexReader.ReadInt32();
                        int x = blockID / matrix.BlockHeight;
                        int y = blockID % matrix.BlockHeight;

                        fsData.Seek(4, SeekOrigin.Current);

                        Tile[] tiles = new Tile[64];

                        fixed (Tile* pTiles = tiles)
                        {
                            NativeMethods._lread(fsData.SafeFileHandle, pTiles, 192);
                        }

                        //matrix.SetLandBlock(x, y, tiles);
                        if (LandBlocks[x] == null)
                            LandBlocks[x] = new Tile[matrix.BlockHeight][];
                        LandBlocks[x][y] = tiles;
                    }
                    return count;
                }
            }
        }

        private unsafe int PatchStatics(TileMatrix matrix, string dataPath, string indexPath, string lookupPath)
        {
            using (FileStream fsData = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read),
                              fsIndex = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.Read),
                              fsLookup = new FileStream(lookupPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (BinaryReader indexReader = new BinaryReader(fsIndex),
                                    lookupReader = new BinaryReader(fsLookup))
                {
                    int count = (int)(indexReader.BaseStream.Length / 4);

                    HuedTileList[][] lists = new HuedTileList[8][];

                    for (int x = 0; x < 8; ++x)
                    {
                        lists[x] = new HuedTileList[8];

                        for (int y = 0; y < 8; ++y)
                            lists[x][y] = new HuedTileList();
                    }

                    for (int i = 0; i < count; ++i)
                    {
                        int blockID = indexReader.ReadInt32();
                        int blockX = blockID / matrix.BlockHeight;
                        int blockY = blockID % matrix.BlockHeight;

                        int offset = lookupReader.ReadInt32();
                        int length = lookupReader.ReadInt32();
                        lookupReader.ReadInt32(); // Extra

                        if (offset < 0 || length <= 0)
                        {
                            if (StaticBlocks[blockX] == null)
                                StaticBlocks[blockX] = new HuedTile[matrix.BlockHeight][][][];

                            StaticBlocks[blockX][blockY] = TileMatrix.EmptyStaticBlock;
                            //matrix.SetStaticBlock(blockX, blockY, matrix.EmptyStaticBlock);
                            continue;
                        }

                        fsData.Seek(offset, SeekOrigin.Begin);

                        int tileCount = length / 7;

                        StaticTile[] staTiles = new StaticTile[tileCount];

                        fixed (StaticTile* pTiles = staTiles)
                        {
                            NativeMethods._lread(fsData.SafeFileHandle, pTiles, length);

                            StaticTile* pCur = pTiles, pEnd = pTiles + tileCount;

                            while (pCur < pEnd)
                            {
                                lists[pCur->m_X & 0x7][pCur->m_Y & 0x7].Add((short)((pCur->m_ID & 0x3FFF) + 0x4000), pCur->m_Hue, pCur->m_Z);
                                ++pCur;
                            }

                            HuedTile[][][] tiles = new HuedTile[8][][];

                            for (int x = 0; x < 8; ++x)
                            {
                                tiles[x] = new HuedTile[8][];

                                for (int y = 0; y < 8; ++y)
                                    tiles[x][y] = lists[x][y].ToArray();
                            }

                            //matrix.SetStaticBlock(blockX, blockY, tiles);
                            if (StaticBlocks[blockX] == null)
                                StaticBlocks[blockX] = new HuedTile[matrix.BlockHeight][][][];

                            StaticBlocks[blockX][blockY] = tiles;
                        }
                    }

                    return count;
                }
            }
        }

        //private unsafe int GetStaticsList(FileStream fsData, BinaryReader indexReader, BinaryReader lookupReader, int BlockHeight)
        //{

        //    int count = (int)(indexReader.BaseStream.Length / 4);

        //    HuedTileList[][] lists = new HuedTileList[8][];

        //    for (int x = 0; x < 8; ++x)
        //    {
        //        lists[x] = new HuedTileList[8];

        //        for (int y = 0; y < 8; ++y)
        //            lists[x][y] = new HuedTileList();
        //    }

        //    for (int i = 0; i < count; ++i)
        //    {
        //        int blockID = indexReader.ReadInt32();
        //        int blockX = blockID / BlockHeight;
        //        int blockY = blockID % BlockHeight;

        //        int offset = lookupReader.ReadInt32();
        //        int length = lookupReader.ReadInt32();
        //        lookupReader.ReadInt32(); // Extra

        //        if (offset < 0 || length <= 0)
        //        {
        //            if (StaticBlocks[blockX] == null)
        //                StaticBlocks[blockX] = new HuedTile[matrix.BlockHeight][][][];

        //            StaticBlocks[blockX][blockY] = TileMatrix.EmptyStaticBlock;
        //            //matrix.SetStaticBlock(blockX, blockY, matrix.EmptyStaticBlock);
        //            continue;
        //        }

        //        fsData.Seek(offset, SeekOrigin.Begin);

        //        int tileCount = length / 7;

        //        StaticTile[] staTiles = new StaticTile[tileCount];

        //        fixed (StaticTile* pTiles = staTiles)
        //        {
        //            NativeMethods._lread(fsData.SafeFileHandle, pTiles, length);

        //            StaticTile* pCur = pTiles, pEnd = pTiles + tileCount;

        //            while (pCur < pEnd)
        //            {
        //                lists[pCur->m_X & 0x7][pCur->m_Y & 0x7].Add((short)((pCur->m_ID & 0x3FFF) + 0x4000), pCur->m_Hue, pCur->m_Z);
        //                ++pCur;
        //            }

        //            HuedTile[][][] tiles = new HuedTile[8][][];

        //            for (int x = 0; x < 8; ++x)
        //            {
        //                tiles[x] = new HuedTile[8][];

        //                for (int y = 0; y < 8; ++y)
        //                    tiles[x][y] = lists[x][y].ToArray();
        //            }

        //            //matrix.SetStaticBlock(blockX, blockY, tiles);
        //            if (StaticBlocks[blockX] == null)
        //                StaticBlocks[blockX] = new HuedTile[matrix.BlockHeight][][][];

        //            StaticBlocks[blockX][blockY] = tiles;
        //        }
        //    }
        //}
    }
}