using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections;

namespace Ultima
{
    public sealed class Textures
    {
        private static FileIndex m_FileIndex = new FileIndex("Texidx.mul", "Texmaps.mul", 0x1000, 10);
        private static Bitmap[] m_Cache = new Bitmap[0x1000];
        private static bool[] m_Removed = new bool[0x1000];
        private static Hashtable m_patched = new Hashtable();

        /// <summary>
        /// ReReads texmaps
        /// </summary>
        public static void Reload()
        {
            m_FileIndex = new FileIndex("Texidx.mul", "Texmaps.mul", 0x1000, 10);
            m_Cache = new Bitmap[0x1000];
            m_Removed = new bool[0x1000];
            m_patched.Clear();
        }

        /// <summary>
        /// Removes Texture <see cref="m_Removed"/>
        /// </summary>
        /// <param name="index"></param>
        public static void Remove(int index)
        {
            m_Removed[index] = true;
        }

        /// <summary>
        /// Replaces Texture
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bmp"></param>
        public static void Replace(int index, Bitmap bmp)
        {
            m_Cache[index] = bmp;
            m_Removed[index] = false;
            if (m_patched.Contains(index))
                m_patched.Remove(index);
        }

        /// <summary>
        /// Tests if index is valid Texture
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool TestTexture(int index)
        {
            int length, extra;
            bool patched;
            if (m_Removed[index])
                return false;
            if (m_Cache[index] != null)
                return true;
            if (m_FileIndex.Seek(index, out length, out extra, out patched) == null)
                return false;

            return true;
        }

        /// <summary>
        /// Returns Bitmap of Texture
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe static Bitmap GetTexture(int index)
        {
            bool patched;
            return GetTexture(index, out patched);
        }
        /// <summary>
        /// Returns Bitmap of Texture with verdata bool
        /// </summary>
        /// <param name="index"></param>
        /// <param name="patched"></param>
        /// <returns></returns>
        public unsafe static Bitmap GetTexture(int index, out bool patched)
        {
            if (m_patched.Contains(index))
                patched = (bool)m_patched[index];
            else
                patched = false;
            if (m_Removed[index])
                return null;
            if (m_Cache[index] != null)
                return m_Cache[index];

            int length, extra;
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);
            if (stream == null)
                return null;
            if (patched)
                m_patched[index] = true;

            int size = extra == 0 ? 64 : 128;

            Bitmap bmp = new Bitmap(size, size, PixelFormat.Format16bppArgb1555);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, size, size), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
            BinaryReader bin = new BinaryReader(stream);

            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;

            for (int y = 0; y < size; ++y, line += delta)
            {
                ushort* cur = line;
                ushort* end = cur + size;

                while (cur < end)
                    *cur++ = (ushort)(bin.ReadUInt16() ^ 0x8000);
            }

            bmp.UnlockBits(bd);

            if (!Files.CacheData)
                return m_Cache[index] = bmp;
            else
                return bmp;
        }

        public unsafe static void Save(string path)
        {
            string idx = Path.Combine(path, "Texidx.mul");
            string mul = Path.Combine(path, "Texmaps.mul");
            using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write),
                              fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (BinaryWriter binidx = new BinaryWriter(fsidx),
                                    binmul = new BinaryWriter(fsmul))
                {
                    for (int index = 0; index < m_Cache.Length; index++)
                    {
                        if (m_Cache[index] == null)
                            m_Cache[index] = GetTexture(index);

                        Bitmap bmp = m_Cache[index];
                        if ((bmp == null) || (m_Removed[index]))
                        {
                            binidx.Write((int)-1); // lookup
                            binidx.Write((int)-1); // length
                            binidx.Write((int)-1); // extra
                        }
                        else
                        {
                            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                            ushort* line = (ushort*)bd.Scan0;
                            int delta = bd.Stride >> 1;

                            binidx.Write((int)fsmul.Position); //lookup
                            int length = (int)fsmul.Position;

                            for (int Y = 0; Y < bmp.Height; ++Y, line += delta)
                            {
                                ushort* cur = line;
                                for (int X = 0; X < bmp.Width; ++X)
                                {
                                    binmul.Write((ushort)(cur[X] ^ 0x8000));
                                }
                            }
                            length = (int)fsmul.Position - length;
                            binidx.Write(length);
                            binidx.Write((int)(bmp.Width == 64 ? 0 : 1));
                            bmp.UnlockBits(bd);
                        }
                    }
                }
            }
        }
    }
}