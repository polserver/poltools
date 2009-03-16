using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System;

namespace Ultima
{
    public sealed class Hues
    {
        private static int[] m_Header;

        public static Hue[] List { get; private set; }

        static Hues()
        {
            Initialize();
        }

        /// <summary>
        /// Reads hues.mul and fills <see cref="List"/>
        /// </summary>
        public static void Initialize()
        {
            string path = Files.GetFilePath("hues.mul");
            int index = 0;

            List = new Hue[3000];

            if (path != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryReader bin = new BinaryReader(fs);

                    int blockCount = (int)fs.Length / 708;

                    if (blockCount > 375)
                        blockCount = 375;
                    m_Header = new int[blockCount];

                    for (int i = 0; i < blockCount; ++i)
                    {
                        m_Header[i] = bin.ReadInt32();

                        for (int j = 0; j < 8; ++j, ++index)
                            List[index] = new Hue(index, bin);
                    }
                }
            }

            for (; index < List.Length; ++index)
                List[index] = new Hue(index);
        }

        public static void Save(string path)
        {
            string mul = Path.Combine(path, "hues.mul");
            using (FileStream fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryWriter binmul = new BinaryWriter(fsmul);
                int index = 0;
                for (int i = 0; i < m_Header.Length; i++)
                {
                    binmul.Write(m_Header[i]);
                    for (int j = 0; j < 8; j++, index++)
                    {
                        for (int c = 0; c < 32; c++)
                            binmul.Write((short)(List[index].Colors[c] ^ 0x8000));

                        binmul.Write((short)(List[index].TableStart ^ 0x8000));
                        binmul.Write((short)(List[index].TableEnd ^ 0x8000));
                        byte[] b = new byte[20];
                        if (List[index].Name != null)
                        {
                            byte[] bb = Encoding.Default.GetBytes(List[index].Name);
                            if (bb.Length > 20)
                                Array.Resize(ref bb, 20);
                            bb.CopyTo(b, 0);
                        }
                        binmul.Write(b);
                    }
                }
            }
        }

        /// <summary>
        /// Returns <see cref="Hue"/>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Hue GetHue(int index)
        {
            index &= 0x3FFF;

            if (index >= 0 && index < 3000)
                return List[index];

            return List[0];
        }

        /// <summary>
        /// Converts RGB value to Huecolor
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static short ColorToHue(Color c)
        {
            return (short)((((c.R & 0xF8) << 7) | ((c.G & 0xF8) << 2) | (c.B >> 3)) ^ 0x8000);
        }

        /// <summary>
        /// Converts Huecolor to RGBColor
        /// </summary>
        /// <param name="hue"></param>
        /// <returns></returns>
        public static Color HueToColor(short hue)
        {
            int c16 = hue;

            return Color.FromArgb((c16 & 0x7C00) >> 7, (c16 & 0x3E0) >> 2, (c16 & 0x1F) << 3);
        }

        public unsafe static void ApplyTo(Bitmap bmp, short[] Colors, bool onlyHueGrayPixels)
        {
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format16bppArgb1555);

            int stride = bd.Stride >> 1;
            int width = bd.Width;
            int height = bd.Height;
            int delta = stride - width;

            ushort* pBuffer = (ushort*)bd.Scan0;
            ushort* pLineEnd = pBuffer + width;
            ushort* pImageEnd = pBuffer + (stride * height);

            if (onlyHueGrayPixels)
            {
                int c;
                int r;
                int g;
                int b;

                while (pBuffer < pImageEnd)
                {
                    while (pBuffer < pLineEnd)
                    {
                        c = *pBuffer;
                        if (c != 0)
                        {
                            r = (c >> 10) & 0x1F;
                            g = (c >> 5) & 0x1F;
                            b = c & 0x1F;
                            if (r == g && r == b)
                                *pBuffer = (ushort)Colors[(c >> 10) & 0x1F];
                        }
                        ++pBuffer;
                    }

                    pBuffer += delta;
                    pLineEnd += stride;
                }
            }
            else
            {
                while (pBuffer < pImageEnd)
                {
                    while (pBuffer < pLineEnd)
                    {
                        if (*pBuffer != 0)
                            *pBuffer = (ushort)Colors[(*pBuffer >> 10) & 0x1F];
                        ++pBuffer;
                    }

                    pBuffer += delta;
                    pLineEnd += stride;
                }
            }

            bmp.UnlockBits(bd);
        }
    }

    public sealed class Hue
    {

        public int Index { get; private set; }
        public short[] Colors { get; set; }
        public string Name { get; set; }
        public short TableStart { get; set; }
        public short TableEnd { get; set; }

        public Hue(int index)
        {
            Name = "Null";
            Index = index;
            Colors = new short[32];
            TableStart = 0;
            TableEnd = 0;
        }

        public Color GetColor(int index)
        {
            int c16 = Colors[index];

            return Color.FromArgb((c16 & 0x7C00) >> 7, (c16 & 0x3E0) >> 2, (c16 & 0x1F) << 3);
        }

        private static byte[] m_StringBuffer = new byte[20];
        public Hue(int index, BinaryReader bin)
        {
            Index = index;
            Colors = new short[32];

            for (int i = 0; i < 32; ++i)
                Colors[i] = (short)(bin.ReadUInt16() | 0x8000);
            TableStart = (short)(bin.ReadUInt16() | 0x8000);
            TableEnd = (short)(bin.ReadUInt16() | 0x8000);

            bin.Read(m_StringBuffer, 0, 20);
            int count;
            for (count = 0; count < 20 && m_StringBuffer[count] != 0; ++count) ;

            Name = Encoding.Default.GetString(m_StringBuffer, 0, count);
            Name = Name.Replace("\n", " ");
        }

        /// <summary>
        /// Applies Hue to Bitmap
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="onlyHueGrayPixels"></param>
        public unsafe void ApplyTo(Bitmap bmp, bool onlyHueGrayPixels)
        {
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format16bppArgb1555);

            int stride = bd.Stride >> 1;
            int width = bd.Width;
            int height = bd.Height;
            int delta = stride - width;

            ushort* pBuffer = (ushort*)bd.Scan0;
            ushort* pLineEnd = pBuffer + width;
            ushort* pImageEnd = pBuffer + (stride * height);

            if (onlyHueGrayPixels)
            {
                int c;
                int r;
                int g;
                int b;

                while (pBuffer < pImageEnd)
                {
                    while (pBuffer < pLineEnd)
                    {
                        c = *pBuffer;
                        if (c != 0)
                        {
                            r = (c >> 10) & 0x1F;
                            g = (c >> 5) & 0x1F;
                            b = c & 0x1F;
                            if (r == g && r == b)
                                *pBuffer = (ushort)Colors[(c >> 10) & 0x1F];
                        }
                        ++pBuffer;
                    }

                    pBuffer += delta;
                    pLineEnd += stride;
                }
            }
            else
            {
                while (pBuffer < pImageEnd)
                {
                    while (pBuffer < pLineEnd)
                    {
                        if (*pBuffer != 0)
                            *pBuffer = (ushort)Colors[(*pBuffer >> 10) & 0x1F];
                        ++pBuffer;
                    }

                    pBuffer += delta;
                    pLineEnd += stride;
                }
            }

            bmp.UnlockBits(bd);
        }
    }
}