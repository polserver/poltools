using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ultima
{
	public sealed class Gumps
	{
		private static FileIndex m_FileIndex=new FileIndex("Gumpidx.mul", "Gumpart.mul", 0x10000, 12);

        private static Bitmap[] m_Cache = new Bitmap[0x10000];
        private static bool[] m_Removed = new bool[0x10000];

		private static byte[] m_PixelBuffer;
		private static byte[] m_StreamBuffer;
		private static byte[] m_ColorTable;
        static Gumps()
        {
        }
        /// <summary>
        /// ReReads gumpart
        /// </summary>
        public static void Reload()
        {
            try
            {
                m_FileIndex = new FileIndex("Gumpidx.mul", "Gumpart.mul", 0x10000, 12);
            }
            catch
            {
                m_FileIndex = null;
            }
            m_Cache = new Bitmap[0x10000];
            m_Removed = new bool[0x10000];
            m_PixelBuffer = null;
            m_StreamBuffer = null;
            m_ColorTable = null;
        }

        /// <summary>
        /// Replaces Gump <see cref="m_Cache"/>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bmp"></param>
        public static void ReplaceGump(int index, Bitmap bmp)
        {
            m_Cache[index] = bmp;
            m_Removed[index] = false;
        }

        /// <summary>
        /// Removes Gumpindex <see cref="m_Removed"/>
        /// </summary>
        /// <param name="index"></param>
        public static void RemoveGump(int index)
        {
            m_Removed[index] = true;
        }

        /// <summary>
        /// Tests if index is definied
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsValidIndex(int index)
        {
            if (m_FileIndex == null)
                return false;
            if (m_Removed[index])
                return false;
            if (m_Cache[index] != null)
                return true;
            int length, extra;
            bool patched;
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);

            if (stream == null)
                return false;
            int width = (extra >> 16) & 0xFFFF;
            int height = extra & 0xFFFF;

            if (width <= 0 || height <= 0)
                return false;

            return true;
        }

        /// <summary>
        /// Returns Bitmap of index and applies Hue
        /// </summary>
        /// <param name="index"></param>
        /// <param name="hue"></param>
        /// <param name="onlyHueGrayPixels"></param>
        /// <returns></returns>
		public unsafe static Bitmap GetGump( int index, Hue hue, bool onlyHueGrayPixels, out bool patched )
		{
			int length, extra;
			Stream stream = m_FileIndex.Seek( index, out length, out extra, out patched );

			if ( stream == null )
				return null;

			int width = (extra >> 16) & 0xFFFF;
			int height = extra & 0xFFFF;

			if ( width <= 0 || height <= 0 )
				return null;

			int bytesPerLine = width << 1;
			int bytesPerStride = (bytesPerLine + 3) & ~3;
			int bytesForImage = height * bytesPerStride;

			int pixelsPerStride = (width + 1) & ~1;
			int pixelsPerStrideDelta = pixelsPerStride - width;

			byte[] pixelBuffer = m_PixelBuffer;

			if ( pixelBuffer == null || pixelBuffer.Length < bytesForImage )
				m_PixelBuffer = pixelBuffer = new byte[(bytesForImage + 2047) & ~2047];

			byte[] streamBuffer = m_StreamBuffer;

			if ( streamBuffer == null || streamBuffer.Length < length )
				m_StreamBuffer = streamBuffer = new byte[(length + 2047) & ~2047];

			byte[] colorTable = m_ColorTable;

			if ( colorTable == null )
				m_ColorTable = colorTable = new byte[128];

			stream.Read( streamBuffer, 0, length );

			fixed ( short *psHueColors = hue.Colors )
			{
				fixed ( byte *pbStream = streamBuffer )
				{
					fixed ( byte *pbPixels = pixelBuffer )
					{
						fixed ( byte *pbColorTable = colorTable )
						{
							ushort *pHueColors = (ushort *)psHueColors;
							ushort *pHueColorsEnd = pHueColors + 32;

							ushort *pColorTable = (ushort *)pbColorTable;

							ushort *pColorTableOpaque = pColorTable;

							while ( pHueColors < pHueColorsEnd )
								*pColorTableOpaque++ = *pHueColors++;

							ushort *pPixelDataStart = (ushort *)pbPixels;

							int *pLookup = (int *)pbStream;
							int *pLookupEnd = pLookup + height;
							int *pPixelRleStart = pLookup;
							int *pPixelRle;

							ushort *pPixel = pPixelDataStart;
							ushort *pRleEnd = pPixel;
							ushort *pPixelEnd = pPixel + width;

							ushort color, count;

							if ( onlyHueGrayPixels )
							{
								while ( pLookup < pLookupEnd )
								{
									pPixelRle = pPixelRleStart + *pLookup++;
									pRleEnd = pPixel;

									while ( pPixel < pPixelEnd )
									{
										color = *(ushort *)pPixelRle;
										count = *(1 + (ushort *)pPixelRle);
										++pPixelRle;

										pRleEnd += count;

										if ( color != 0 && (color & 0x1F) == ((color >> 5) & 0x1F) && (color & 0x1F) == ((color >> 10) & 0x1F) )
											color = pColorTable[color >> 10];
										else if ( color != 0 )
											color ^= 0x8000;

										while ( pPixel < pRleEnd )
											*pPixel++ = color;
									}

									pPixel += pixelsPerStrideDelta;
									pPixelEnd += pixelsPerStride;
								}
							}
							else
							{
								while ( pLookup < pLookupEnd )
								{
									pPixelRle = pPixelRleStart + *pLookup++;
									pRleEnd = pPixel;

									while ( pPixel < pPixelEnd )
									{
										color = *(ushort *)pPixelRle;
										count = *(1 + (ushort *)pPixelRle);
										++pPixelRle;

										pRleEnd += count;

										if ( color != 0 )
											color = pColorTable[color >> 10];

										while ( pPixel < pRleEnd )
											*pPixel++ = color;
									}

									pPixel += pixelsPerStrideDelta;
									pPixelEnd += pixelsPerStride;
								}
							}

							return new Bitmap( width, height, bytesPerStride, PixelFormat.Format16bppArgb1555, (IntPtr) pPixelDataStart );
						}
					}
				}
			}
		}

        /// <summary>
        /// Returns Bitmap of index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe static Bitmap GetGump(int index)
        {
            bool patched;
            return GetGump(index, out patched);
        }

        /// <summary>
        /// Returns Bitmap of index and if verdata patched
        /// </summary>
        /// <param name="index"></param>
        /// <param name="patched"></param>
        /// <returns></returns>
        public unsafe static Bitmap GetGump(int index, out bool patched)
        {
            patched = false;
            if (m_Removed[index])
                return null;
            if (m_Cache[index] != null)
                return m_Cache[index];
            int length, extra;
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);

            if (stream == null)
                return null;

            int width = (extra >> 16) & 0xFFFF;
            int height = extra & 0xFFFF;

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
            BinaryReader bin = new BinaryReader(stream);

            int[] lookups = new int[height];
            int start = (int)bin.BaseStream.Position;

            for (int i = 0; i < height; ++i)
                lookups[i] = start + (bin.ReadInt32() * 4);

            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;

            for (int y = 0; y < height; ++y, line += delta)
            {
                bin.BaseStream.Seek(lookups[y], SeekOrigin.Begin);

                ushort* cur = line;
                ushort* end = line + bd.Width;

                while (cur < end)
                {
                    ushort color = bin.ReadUInt16();
                    ushort* next = cur + bin.ReadUInt16();

                    if (color == 0)
                        cur = next;
                    else
                    {
                        color ^= 0x8000;

                        while (cur < next)
                            *cur++ = color;
                    }
                }
            }

            bmp.UnlockBits(bd);

            if (Files.CacheData)
                return m_Cache[index] = bmp;
            else
                return bmp;
        }

        public static unsafe void Save(string path)
        {
            string idx = Path.Combine(path, "Gumpidx.mul");
            string mul = Path.Combine(path, "Gumpart.mul");
            using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryWriter binidx = new BinaryWriter(fsidx);
                using (FileStream fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    BinaryWriter binmul = new BinaryWriter(fsmul);
                    for (int index = 0; index < m_Cache.Length; index++)
                    {
                        if (m_Cache[index] == null)
                            m_Cache[index] = GetGump(index);

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
                            int fill = 0;
                            for (int i = 0; i < bmp.Height; ++i)
                            {
                                binmul.Write(fill);
                            }
                            for (int Y = 0; Y < bmp.Height; ++Y, line += delta)
                            {
                                ushort* cur = line;

                                int X = 0;
                                int current = (int)fsmul.Position;
                                fsmul.Seek(length + Y * 4, SeekOrigin.Begin);
                                int offset = (current - length) / 4;
                                binmul.Write(offset);
                                fsmul.Seek(length + offset*4, SeekOrigin.Begin);
                                
                                while (X<bd.Width)
                                {
                                    int Run = 1;
                                    ushort c = cur[X];
                                    while ((X+Run) < bd.Width)
                                    {
                                        if (c != cur[X+Run])
                                            break;
                                        Run++;
                                    }
                                    if (c == 0)
                                        binmul.Write(c);
                                    else
                                        binmul.Write((ushort)(c^0x8000));
                                    binmul.Write((short)Run);
                                    X += Run;
                                }
                            }
                            length = (int)fsmul.Position - length;
                            binidx.Write(length);
                            binidx.Write((int)( bmp.Width << 16 ) + bmp.Height);
                            bmp.UnlockBits(bd);
                        }
                    }
                }
            }
        }
	}
}