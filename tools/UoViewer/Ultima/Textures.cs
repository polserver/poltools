using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Ultima
{
	public sealed class Textures
	{
		private static FileIndex m_FileIndex = new FileIndex( "Texidx.mul", "Texmaps.mul", 0x1000, 10 );
		//public static FileIndex FileIndex{ get{ return m_FileIndex; } }

        private static Bitmap[] m_Cache = new Bitmap[0x1000];
        //public static Bitmap[] Cache { get { return m_Cache; } }

        public static bool TestTexture(int index)
        {
            int length, extra;
            bool patched;

            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);

            if (stream == null)
                return false;

            return true;
        }
		public unsafe static Bitmap GetTexture( int index )
		{
            if (!FileIndex.CacheData)
            {
                if (m_Cache[index] != null)
                    return m_Cache[index];
            }

			int length, extra;
			bool patched;

			Stream stream = m_FileIndex.Seek( index, out length, out extra, out patched );

			if ( stream == null )
				return null;

			int size = extra == 0 ? 64 : 128;

			Bitmap bmp = new Bitmap( size, size, PixelFormat.Format16bppArgb1555 );
			BitmapData bd = bmp.LockBits( new Rectangle( 0, 0, size, size ), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555 );
			BinaryReader bin = new BinaryReader( stream );

			ushort *line = (ushort *)bd.Scan0;
			int delta = bd.Stride >> 1;

			for ( int y = 0; y < size; ++y, line += delta )
			{
				ushort *cur = line;
				ushort *end = cur + size;

				while ( cur < end )
					*cur++ = (ushort)(bin.ReadUInt16() ^ 0x8000);
			}

			bmp.UnlockBits( bd );

            if (!FileIndex.CacheData)
                return m_Cache[index] = bmp;
            else
                return bmp;
		}
	}
}