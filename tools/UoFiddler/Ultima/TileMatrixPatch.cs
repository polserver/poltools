using System;
using System.IO;

namespace Ultima
{
    public sealed class TileMatrixPatch
    {
        private int m_LandBlocks, m_StaticBlocks;

        public int LandBlocks { get { return m_LandBlocks; } }
        public int StaticBlocks { get { return m_StaticBlocks; } }

        public TileMatrixPatch(TileMatrix matrix, int index, string path)
        {
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
                m_LandBlocks = PatchLand(matrix, mapDataPath, mapIndexPath);

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
                m_StaticBlocks = PatchStatics(matrix, staDataPath, staIndexPath, staLookupPath);
        }

        private unsafe int PatchLand(TileMatrix matrix, string dataPath, string indexPath)
        {
            using (FileStream fsData = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read),
                              fsIndex = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryReader indexReader = new BinaryReader(fsIndex);

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

                    matrix.SetLandBlock(x, y, tiles);
                }

                indexReader.Close();
                return count;
            }
        }

        private unsafe int PatchStatics(TileMatrix matrix, string dataPath, string indexPath, string lookupPath)
        {
            using (FileStream fsData = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read),
                              fsIndex = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.Read),
                              fsLookup = new FileStream(lookupPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryReader indexReader = new BinaryReader(fsIndex);
                BinaryReader lookupReader = new BinaryReader(fsLookup);

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
                        matrix.SetStaticBlock(blockX, blockY, matrix.EmptyStaticBlock);
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

                        matrix.SetStaticBlock(blockX, blockY, tiles);
                    }
                }

                indexReader.Close();
                lookupReader.Close();

                return count;
            }
        }
    }
}