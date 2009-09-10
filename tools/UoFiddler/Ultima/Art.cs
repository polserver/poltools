using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ultima
{
    public sealed class Art
    {
        private static FileIndex m_FileIndex = new FileIndex("Artidx.mul", "Art.mul", 0x10000, 4);
        private static Bitmap[] m_Cache = new Bitmap[0x10000];
        private static bool[] m_Removed = new bool[0x10000];
        private static Hashtable m_patched = new Hashtable();
        public static bool Modified=false;

        private Art()
        {
        }

        public static bool IsUOSA()
        {
            return (GetIdxLength() == 0xC000);
        }

        public static int GetIdxLength()
        {
            return (int)(m_FileIndex.IdxLength/12);
        }
        /// <summary>
        /// ReReads Art.mul
        /// </summary>
        public static void Reload()
        {
            m_Cache = new Bitmap[0x10000];
            m_Removed = new bool[0x10000];
            m_FileIndex = new FileIndex("Artidx.mul", "Art.mul", 0x10000, 4);
            m_patched.Clear();
            Modified = false;
        }

        /// <summary>
        /// Sets bmp of index in <see cref="m_Cache"/> of Static
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bmp"></param>
        public static void ReplaceStatic(int index, Bitmap bmp)
        {
            index += 0x4000;
            index &= 0xFFFF;
            m_Cache[index] = bmp;
            m_Removed[index] = false;
            if (m_patched.Contains(index))
                m_patched.Remove(index);
            Modified = true;
        }

        /// <summary>
        /// Sets bmp of index in <see cref="m_Cache"/> of Land
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bmp"></param>
        public static void ReplaceLand(int index, Bitmap bmp)
        {
            index &= 0x3FFF;
            m_Cache[index] = bmp;
            m_Removed[index] = false;
            if (m_patched.Contains(index))
                m_patched.Remove(index);
            Modified = true;
        }

        /// <summary>
        /// Removes Static index <see cref="m_Removed"/>
        /// </summary>
        /// <param name="index"></param>
        public static void RemoveStatic(int index)
        {
            index += 0x4000;
            index &= 0xFFFF;
            m_Removed[index] = true;
            Modified = true;
        }

        /// <summary>
        /// Removes Land index <see cref="m_Removed"/>
        /// </summary>
        /// <param name="index"></param>
        public static void RemoveLand(int index)
        {
            index &= 0x3FFF;
            m_Removed[index] = true;
            Modified = true;
        }

        /// <summary>
        /// Tests if Static is definied (width and hight check)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsValidStatic(int index)
        {
            index += 0x4000;
            index &= 0xFFFF;

            if (m_Removed[index])
                return false;
            if (m_Cache[index] != null)
                return true;

            int length, extra;
            bool patched;
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);

            if (stream == null)
                return false;

            using (BinaryReader bin = new BinaryReader(stream))
            {
                bin.ReadInt32();
                int width = bin.ReadInt16();
                int height = bin.ReadInt16();
                if (width <= 0 || height <= 0)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Tests if LandTile is definied
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsValidLand(int index)
        {
            index &= 0x3FFF;
            if (m_Removed[index])
                return false;
            if (m_Cache[index] != null)
                return true;

            int length, extra;
            bool patched;

            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);
            bool def = true;
            if (stream == null)
                def = false;
            else
                stream.Close();
            return def;
        }

        /// <summary>
        /// Returns Bitmap of LandTile (with Cache)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Bitmap GetLand(int index)
        {
            bool patched;
            return GetLand(index, out patched);
        }
        /// <summary>
        /// Returns Bitmap of LandTile (with Cache) and verdata bool
        /// </summary>
        /// <param name="index"></param>
        /// <param name="patched"></param>
        /// <returns></returns>
        public static Bitmap GetLand(int index, out bool patched)
        {
            index &= 0x3FFF;
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

            if (Files.CacheData)
                return m_Cache[index] = LoadLand(stream);
            else
                return LoadLand(stream);
        }

        public static byte[] GetRawLand(int index)
        {
            index &= 0x3FFF;

            int length, extra;
            bool patched;
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);
            if (stream == null)
                return null;
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            stream.Close();
            return buffer;
        }

        /// <summary>
        /// Returns Bitmap of Static (with Cache)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Bitmap GetStatic(int index)
        {
            bool patched;
            return GetStatic(index, out patched);
        }
        /// <summary>
        /// Returns Bitmap of Static (with Cache) and verdata bool
        /// </summary>
        /// <param name="index"></param>
        /// <param name="patched"></param>
        /// <returns></returns>
        public static Bitmap GetStatic(int index, out bool patched)
        {
            index += 0x4000;
            index &= 0xFFFF;
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
            bool patched;
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);
            if (stream == null)
                return null;
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            stream.Close();
            return buffer;
        }

        public unsafe static void Measure(Bitmap bmp, out int xMin, out int yMin, out int xMax, out int yMax)
        {
            xMin = yMin = 0;
            xMax = yMax = -1;

            if (bmp == null || bmp.Width <= 0 || bmp.Height <= 0)
                return;

            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);

            int delta = (bd.Stride >> 1) - bd.Width;
            int lineDelta = bd.Stride >> 1;

            ushort* pBuffer = (ushort*)bd.Scan0;
            ushort* pLineEnd = pBuffer + bd.Width;
            ushort* pEnd = pBuffer + (bd.Height * lineDelta);

            bool foundPixel = false;

            int x = 0, y = 0;

            while (pBuffer < pEnd)
            {
                while (pBuffer < pLineEnd)
                {
                    ushort c = *pBuffer++;

                    if ((c & 0x8000) != 0)
                    {
                        if (!foundPixel)
                        {
                            foundPixel = true;
                            xMin = xMax = x;
                            yMin = yMax = y;
                        }
                        else
                        {
                            if (x < xMin)
                                xMin = x;

                            if (y < yMin)
                                yMin = y;

                            if (x > xMax)
                                xMax = x;

                            if (y > yMax)
                                yMax = y;
                        }
                    }
                    ++x;
                }

                pBuffer += delta;
                pLineEnd += lineDelta;
                ++y;
                x = 0;
            }

            bmp.UnlockBits(bd);
        }

        private static unsafe Bitmap LoadStatic(Stream stream)
        {
            using (BinaryReader bin = new BinaryReader(stream))
            {
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
                        if (xOffset > delta)
                            break;
                        cur += xOffset;
                        if (xOffset + xRun > delta)
                            break;
                        end = cur + xRun;

                        while (cur < end)
                            *cur++ = (ushort)(bin.ReadUInt16() ^ 0x8000);
                    }
                }

                bmp.UnlockBits(bd);
                return bmp;
            }
        }

        private static unsafe Bitmap LoadLand(Stream stream)
        {
            Bitmap bmp = new Bitmap(44, 44, PixelFormat.Format16bppArgb1555);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, 44, 44), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
            using (BinaryReader bin = new BinaryReader(stream))
            {
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

        /// <summary>
        /// Saves mul
        /// </summary>
        /// <param name="path"></param>
        public static unsafe void Save(string path)
        {
            string idx = Path.Combine(path, "artidx.mul");
            string mul = Path.Combine(path, "art.mul");
            using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write),
                              fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (BinaryWriter binidx = new BinaryWriter(fsidx),
                                    binmul = new BinaryWriter(fsmul))
                {
                    for (int index = 0; index < GetIdxLength(); index++)
                    {
                        if (m_Cache[index] == null)
                        {
                            if (index < 0x4000)
                                m_Cache[index] = GetLand(index);
                            else
                                m_Cache[index] = GetStatic(index - 0x4000);
                        }
                        Bitmap bmp = m_Cache[index];
                        if ((bmp == null) || (m_Removed[index]))
                        {
                            binidx.Write((int)-1); // lookup
                            binidx.Write((int)-1); // length
                            binidx.Write((int)-1); // extra
                        }
                        else if (index < 0x4000)
                        {
                            //land
                            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                            ushort* line = (ushort*)bd.Scan0;
                            int delta = bd.Stride >> 1;
                            binidx.Write((int)fsmul.Position); //lookup
                            int length = (int)fsmul.Position;
                            int x = 22;
                            int y = 0;
                            int linewidth = 2;
                            for (int m = 0; m < 22; ++m, ++y, line += delta, linewidth += 2)
                            {
                                --x;
                                ushort* cur = line;
                                for (int n = 0; n < linewidth; ++n)
                                    binmul.Write((ushort)(cur[x + n] ^ 0x8000));
                            }
                            x = 0;
                            linewidth = 44;
                            y = 22;
                            line = (ushort*)bd.Scan0;
                            line += delta * 22;
                            for (int m = 0; m < 22; m++, y++, line += delta, ++x, linewidth -= 2)
                            {
                                ushort* cur = line;
                                for (int n = 0; n < linewidth; n++)
                                    binmul.Write((ushort)(cur[x + n] ^ 0x8000));
                            }
                            length = (int)fsmul.Position - length;
                            binidx.Write(length);
                            binidx.Write((int)0);
                            bmp.UnlockBits(bd);
                        }
                        else
                        {
                            // art
                            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                            ushort* line = (ushort*)bd.Scan0;
                            int delta = bd.Stride >> 1;
                            binidx.Write((int)fsmul.Position); //lookup
                            int length = (int)fsmul.Position;
                            binmul.Write((int)1234); // header
                            binmul.Write((short)bmp.Width);
                            binmul.Write((short)bmp.Height);
                            int lookup = (int)fsmul.Position;
                            int streamloc = lookup + bmp.Height * 2;
                            int width = 0;
                            for (int i = 0; i < bmp.Height; ++i)// fill lookup
                                binmul.Write(width);
                            int X = 0;
                            for (int Y = 0; Y < bmp.Height; ++Y, line += delta)
                            {
                                ushort* cur = line;
                                width = (int)(fsmul.Position - streamloc) / 2;
                                fsmul.Seek(lookup + Y * 2, SeekOrigin.Begin);
                                binmul.Write(width);
                                fsmul.Seek(streamloc + width * 2, SeekOrigin.Begin);
                                int i = 0;
                                int j = 0;
                                X = 0;
                                while (i < bmp.Width)
                                {
                                    i = X;
                                    for (i = X; i <= bmp.Width; ++i)
                                    {
                                        //first pixel set
                                        if (i < bmp.Width)
                                        {
                                            if (cur[i] != 0)
                                                break;
                                        }
                                    }
                                    if (i < bmp.Width)
                                    {
                                        for (j = (i + 1); j < bmp.Width; ++j)
                                        {
                                            //next non set pixel
                                            if (cur[j] == 0)
                                                break;
                                        }
                                        binmul.Write((short)(i - X)); //xoffset
                                        binmul.Write((short)(j - i)); //run
                                        for (int p = i; p < j; ++p)
                                            binmul.Write((ushort)(cur[p] ^ 0x8000));
                                        X = j;
                                    }
                                }
                                binmul.Write((short)0); //xOffset
                                binmul.Write((short)0); //Run
                            }
                            length = (int)fsmul.Position - length;
                            binidx.Write(length);
                            binidx.Write((int)0);
                            bmp.UnlockBits(bd);
                        }
                    }
                }
            }
        }
    }
}