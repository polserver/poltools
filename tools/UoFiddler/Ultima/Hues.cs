using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System;

namespace Ultima
{
	public sealed class Hues
	{
		private static Hue[] m_List;
        private static int[] m_Header;

		public static Hue[] List{ get{ return m_List; } }

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

			m_List = new Hue[3000];

			if ( path != null )
			{
				using ( FileStream fs = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read ) )
				{
					BinaryReader bin = new BinaryReader( fs );

					int blockCount = (int)fs.Length / 708;

					if ( blockCount > 375 )
						blockCount = 375;
                    m_Header = new int[blockCount];

					for ( int i = 0; i < blockCount; ++i )
					{
                        m_Header[i] = bin.ReadInt32();

						for ( int j = 0; j < 8; ++j, ++index )
							m_List[index] = new Hue( index, bin );
					}
				}
			}
            
			for ( ; index < m_List.Length; ++index )
				m_List[index] = new Hue( index );
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
                            binmul.Write((short)(m_List[index].Colors[c]^0x8000));

                        binmul.Write((short)(m_List[index].TableStart^0x8000));
                        binmul.Write((short)(m_List[index].TableEnd^0x8000));
                        byte[] b = new byte[20];
                        if (m_List[index].Name != null)
                        {
                            byte[] bb = Encoding.Default.GetBytes(m_List[index].Name);
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
		public static Hue GetHue( int index )
		{
			index &= 0x3FFF;

			if ( index >= 0 && index < 3000 )
				return m_List[index];
			
			return m_List[0];
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

			return Color.FromArgb( (c16 & 0x7C00) >> 7, (c16 & 0x3E0) >> 2, (c16 & 0x1F) << 3 );
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

            ushort* pColors = stackalloc ushort[0x40];

            fixed (short* pOriginal = Colors)
            {
                ushort* pSource = (ushort*)pOriginal;
                ushort* pDest = pColors;
                ushort* pEnd = pDest + 32;

                while (pDest < pEnd)
                    *pDest++ = 0;

                pEnd += 32;

                while (pDest < pEnd)
                    *pDest++ = *pSource++;
            }

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
                        r = (c >> 10) & 0x1F;
                        g = (c >> 5) & 0x1F;
                        b = c & 0x1F;

                        if (r == g && r == b)
                            *pBuffer++ = pColors[c >> 10];
                        else
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
                        *pBuffer = pColors[(*pBuffer) >> 10];
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
		private int m_Index;
		private short[] m_Colors;
		private string m_Name;
        private short m_TableStart;
        private short m_TableEnd;

		public int Index{ get{ return m_Index; } }
        public short[] Colors { get { return m_Colors; } set { m_Colors = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public short TableStart { get { return m_TableStart; } set { m_TableStart = value; } }
        public short TableEnd { get { return m_TableEnd; } set { m_TableEnd = value; } }

		public Hue( int index )
		{
			m_Name = "Null";
			m_Index = index;
			m_Colors = new short[32];
            m_TableStart = 0;
            m_TableEnd = 0;
		}

		public Color GetColor( int index )
		{
			int c16 = m_Colors[index];

			return Color.FromArgb( (c16 & 0x7C00) >> 7, (c16 & 0x3E0) >> 2, (c16 & 0x1F) << 3 );
		}

        private static byte[] m_StringBuffer = new byte[20];
		public Hue( int index, BinaryReader bin )
		{
			m_Index = index;
			m_Colors = new short[32];

			for ( int i = 0; i < 32; ++i )
				m_Colors[i] = (short)(bin.ReadUInt16() | 0x8000);
            m_TableStart = (short)(bin.ReadUInt16() | 0x8000);
            m_TableEnd = (short)(bin.ReadUInt16() | 0x8000);

            bin.Read(m_StringBuffer, 0, 20);
            int count;
            for (count = 0; count < 20 && m_StringBuffer[count] != 0; ++count) ;

            m_Name= Encoding.Default.GetString(m_StringBuffer, 0, count);
            m_Name = m_Name.Replace("\n", " ");
		}

        /// <summary>
        /// Applies Hue to Bitmap
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="onlyHueGrayPixels"></param>
		public unsafe void ApplyTo( Bitmap bmp, bool onlyHueGrayPixels )
		{
			BitmapData bd = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.ReadWrite, PixelFormat.Format16bppArgb1555 );

			int stride = bd.Stride >> 1;
			int width = bd.Width;
			int height = bd.Height;
			int delta = stride - width;

			ushort *pBuffer = (ushort *)bd.Scan0;
			ushort *pLineEnd = pBuffer + width;
			ushort *pImageEnd = pBuffer + (stride * height);

			ushort *pColors = stackalloc ushort[0x40];

			fixed ( short *pOriginal = m_Colors )
			{
				ushort *pSource = (ushort *)pOriginal;
				ushort *pDest = pColors;
				ushort *pEnd = pDest + 32;

				while ( pDest < pEnd )
					*pDest++ = 0;

				pEnd += 32;

				while ( pDest < pEnd )
					*pDest++ = *pSource++;
			}

			if ( onlyHueGrayPixels )
			{
				int c;
				int r;
				int g;
				int b;

				while ( pBuffer < pImageEnd )
				{
					while ( pBuffer < pLineEnd )
					{
						c = *pBuffer;
						r = (c >> 10) & 0x1F;
						g = (c >>  5) & 0x1F;
						b =  c        & 0x1F;

						if ( r == g && r == b )
							*pBuffer++ = pColors[c >> 10];
						else
							++pBuffer;
					}

					pBuffer += delta;
					pLineEnd += stride;
				}
			}
			else
			{
				while ( pBuffer < pImageEnd )
				{
					while ( pBuffer < pLineEnd )
					{
						*pBuffer = pColors[(*pBuffer) >> 10];
						++pBuffer;
					}

					pBuffer += delta;
					pLineEnd += stride;
				}
			}

			bmp.UnlockBits( bd );
		}
	}
}