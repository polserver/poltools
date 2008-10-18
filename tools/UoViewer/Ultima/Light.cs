using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Ultima
{
    public class Light
    {
        private static FileIndex m_FileIndex = new FileIndex("lightidx.mul", "light.mul", 0x100, -1);
        public static FileIndex FileIndex { get { return m_FileIndex; } }

        private static Bitmap[] m_Cache = new Bitmap[0x100];

        public static Bitmap[] Cache { get { return m_Cache; } }

        public static int GetCount()
        {
            string idxPath=Client.GetFilePath("lightidx.mul");
            FileStream index = new FileStream( idxPath, FileMode.Open, FileAccess.Read, FileShare.Read );

            return (int)(index.Length/12);
        }

        public static bool TestLight(int index)
        {
            if (!FileIndex.CacheData)
            {
                if (m_Cache[index] != null)
                    return true;
            }

            int length, extra;
            bool patched;

            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);

            if (stream == null)
                return false;

            return true;
        }

        public unsafe static Bitmap GetLight(int index)
        {
            if (!FileIndex.CacheData)
            {
                if (m_Cache[index] != null)
                    return m_Cache[index];
            }

            int length, extra;
            bool patched;

            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);

            if (stream == null)
                return null;

            int width = (extra & 0xFFFF);
            int height = ((extra >> 16) & 0xFFFF);

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
            BinaryReader bin = new BinaryReader(stream);

            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;

            ushort read;

            for (int y = 0; y < height; ++y, line += delta)
            {
                ushort* cur = line;
                ushort* end = cur + width;

                while (cur < end)
                {
                    read = (ushort)bin.ReadSByte();

                    if (read == 0)
                        *cur++ = 255;
                    else
                        *cur++ = (ushort)((read ^0x8000));
                }
            }

            bmp.UnlockBits(bd);

            if (!FileIndex.CacheData)
                return m_Cache[index] = bmp;
            else
                return bmp;
        }
    }
}
