using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections;
using Ultima;

namespace ComparePlugin
{
    class SecondArt
    {
        private static SecondFileIndex m_FileIndex;
        private static Bitmap[] m_Cache;

        public static void SetFileIndex(string idxPath, string mulPath)
        {
            m_FileIndex = new SecondFileIndex(idxPath, mulPath, 0xC000);
            m_Cache = new Bitmap[0xC000];
        }
        public static bool IsValidStatic(int index)
        {
            index += 0x4000;
            index &= 0xFFFF;

            if (m_Cache[index] != null)
                return true;

            int length, extra;
            Stream stream = m_FileIndex.Seek(index, out length, out extra);

            if (stream == null)
                return false;

            BinaryReader bin = new BinaryReader(stream);

            bin.ReadInt32();
            int width = bin.ReadInt16();
            int height = bin.ReadInt16();

            if (width <= 0 || height <= 0)
                return false;

            return true;
        }

        public static Bitmap GetStatic(int index)
        {
            index += 0x4000;
            index &= 0xFFFF;
            if (m_Cache[index] != null)
                return m_Cache[index];

            int length, extra;
            Stream stream = m_FileIndex.Seek(index, out length, out extra);
            if (stream == null)
                return null;

            if (Files.CacheData)
                return m_Cache[index] = LoadStatic(stream);
            else
                return LoadStatic(stream);
        }

        public static byte[] GetRawStatic(int index)
        {
            index += 0x4000;
            index &= 0xFFFF;

            int length, extra;
            Stream stream = m_FileIndex.Seek(index, out length, out extra);
            if (stream == null)
                return null;
            byte[] buffer=new byte[length];
            stream.Read(buffer, 0, length);
            return buffer;
        }

        private static unsafe Bitmap LoadStatic(Stream stream)
        {
            BinaryReader bin = new BinaryReader(stream);

            bin.ReadInt32();
            int width = bin.ReadInt16();
            int height = bin.ReadInt16();

            if (width <= 0 || height <= 0)
                return null;

            int[] lookups = new int[height];

            int start = (int)bin.BaseStream.Position + (height * 2);

            for (int i = 0; i < height; ++i)
                lookups[i] = (int)(start + (bin.ReadUInt16() * 2));

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);

            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;

            for (int y = 0; y < height; ++y, line += delta)
            {
                bin.BaseStream.Seek(lookups[y], SeekOrigin.Begin);

                ushort* cur = line;
                ushort* end;

                int xOffset, xRun;

                while (((xOffset = bin.ReadUInt16()) + (xRun = bin.ReadUInt16())) != 0)
                {
                    cur += xOffset;
                    end = cur + xRun;

                    while (cur < end)
                        *cur++ = (ushort)(bin.ReadUInt16() ^ 0x8000);
                }
            }

            bmp.UnlockBits(bd);

            return bmp;
        }

        public static bool IsValidLand(int index)
        {
            index &= 0x3FFF;
            if (m_Cache[index] != null)
                return true;

            int length, extra;
            Stream stream = m_FileIndex.Seek(index, out length, out extra);

            if (stream == null)
                return false;
            return true;
        }

        public static Bitmap GetLand(int index)
        {
            index &= 0x3FFF;

            if (m_Cache[index] != null)
                return m_Cache[index];

            int length, extra;
            Stream stream = m_FileIndex.Seek(index, out length, out extra);
            if (stream == null)
                return null;

            if (Files.CacheData)
                return m_Cache[index] = LoadLand(stream);
            else
                return LoadLand(stream);
        }

        public static byte[] GetRawLand(int index)
        {
            index &= 0x3FFF;

            int length, extra;
            Stream stream = m_FileIndex.Seek(index, out length, out extra);
            if (stream == null)
                return null;
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            return buffer;
        }

        private static unsafe Bitmap LoadLand(Stream stream)
        {
            Bitmap bmp = new Bitmap(44, 44, PixelFormat.Format16bppArgb1555);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, 44, 44), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
            BinaryReader bin = new BinaryReader(stream);

            int xOffset = 21;
            int xRun = 2;

            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;

            for (int y = 0; y < 22; ++y, --xOffset, xRun += 2, line += delta)
            {
                ushort* cur = line + xOffset;
                ushort* end = cur + xRun;

                while (cur < end)
                    *cur++ = (ushort)(bin.ReadUInt16() | 0x8000);
            }

            xOffset = 0;
            xRun = 44;

            for (int y = 0; y < 22; ++y, ++xOffset, xRun -= 2, line += delta)
            {
                ushort* cur = line + xOffset;
                ushort* end = cur + xRun;

                while (cur < end)
                    *cur++ = (ushort)(bin.ReadUInt16() | 0x8000);
            }

            bmp.UnlockBits(bd);

            return bmp;
        }
    }

    public sealed class SecondFileIndex
    {
        private Entry3D[] m_Index;
        private Stream m_Stream;

        public Entry3D[] Index { get { return m_Index; } }
        public Stream Stream { get { return m_Stream; } }

        public Stream Seek(int index, out int length, out int extra)
        {
            if (index < 0 || index >= m_Index.Length)
            {
                length = extra = 0;
                return null;
            }

            Entry3D e = m_Index[index];

            if (e.lookup < 0)
            {
                length = extra = 0;
                return null;
            }

            length = e.length & 0x7FFFFFFF;
            extra = e.extra;

            if (m_Stream == null)
            {
                length = extra = 0;
                return null;
            }

            m_Stream.Seek(e.lookup, SeekOrigin.Begin);
            return m_Stream;
        }

        public SecondFileIndex(string idxFile, string mulFile, int length)
        {
            m_Index = new Entry3D[length];

            if (!File.Exists(idxFile))
                idxFile = null;
            if (!File.Exists(mulFile))
                mulFile = null;

            if (idxFile != null && mulFile != null)
            {
                using (FileStream index = new FileStream(idxFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryReader bin = new BinaryReader(index);
                    m_Stream = new FileStream(mulFile, FileMode.Open, FileAccess.Read, FileShare.Read);

                    int count = (int)(index.Length / 12);

                    for (int i = 0; i < count && i < length; ++i)
                    {
                        m_Index[i].lookup = bin.ReadInt32();
                        m_Index[i].length = bin.ReadInt32();
                        m_Index[i].extra = bin.ReadInt32();
                    }

                    for (int i = count; i < length; ++i)
                    {
                        m_Index[i].lookup = -1;
                        m_Index[i].length = -1;
                        m_Index[i].extra = -1;
                    }
                }
            }
            else
            {
                m_Stream = null;
                return;
            }
        }
    }

    public struct Entry3D
    {
        public int lookup;
        public int length;
        public int extra;
    }
}
