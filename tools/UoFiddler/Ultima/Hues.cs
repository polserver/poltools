using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Ultima
{
	public sealed class Hues
	{
		private static Hue[] m_List;

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

					for ( int i = 0; i < blockCount; ++i )
					{
						bin.ReadInt32();

						for ( int j = 0; j < 8; ++j, ++index )
							m_List[index] = new Hue( index, bin );
					}
				}
			}
            
			for ( ; index < m_List.Length; ++index )
				m_List[index] = new Hue( index );
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
	}

	public sealed class Hue
	{
		private int m_Index;
		private short[] m_Colors;
		private string m_Name;

		public int Index{ get{ return m_Index; } }
		public short[] Colors{ get{ return m_Colors; } }
		public string Name{ get{ return m_Name; } }

		public Hue( int index )
		{
			m_Name = "Null";
			m_Index = index;
			m_Colors = new short[34];
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
			m_Colors = new short[34];

			for ( int i = 0; i < 34; ++i )
				m_Colors[i] = (short)(bin.ReadUInt16() | 0x8000);

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