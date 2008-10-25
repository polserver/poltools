using System;
using System.IO;
using System.Collections;
using System.Windows.Forms.Design;

namespace Ultima
{
	public class FileIndex
	{
        public static bool CacheData = true;
        public static string[] m_Files = new string[]
		{
			"anim.idx",
			"anim.mul",
			"anim2.idx",
			"anim2.mul",
			"anim3.idx",
			"anim3.mul",
			"anim4.idx",
			"anim4.mul",
            "anim5.idx",
			"anim5.mul",
			"art.mul",
			"artidx.mul",
			"body.def",
			"bodyconv.def",
            "cliloc.deu",
            "cliloc.enu",
            "fonts.mul",
			"gumpart.mul",
			"gumpidx.mul",
            "gump.def",
            "equipconv.def",
			"hues.mul",
            "light.mul",
            "lightidx.mul",
			"map0.mul",
			"map2.mul",
			"map3.mul",
			"map4.mul",
			"mapdif0.mul",
			"mapdif1.mul",
			"mapdif2.mul",
			"mapdif3.mul",
			"mapdif4.mul",
			"mapdifl0.mul",
			"mapdifl1.mul",
			"mapdifl2.mul",
			"mapdifl3.mul",
			"mapdifl4.mul",
            "multi.idx",
            "multi.mul",
			"radarcol.mul",
            "soundidx.mul",
            "sound.mul",
            "sound.def",
			"stadif0.mul",
			"stadif1.mul",
			"stadif2.mul",
			"stadif3.mul",
			"stadif4.mul",
			"stadifi0.mul",
			"stadifi1.mul",
			"stadifi2.mul",
			"stadifi3.mul",
			"stadifi4.mul",
			"stadifl0.mul",
			"stadifl1.mul",
			"stadifl2.mul",
			"stadifl3.mul",
			"stadifl4.mul",
			"staidx0.mul",
			"staidx2.mul",
			"staidx3.mul",
			"staidx4.mul",
			"statics0.mul",
			"statics2.mul",
			"statics3.mul",
			"statics4.mul",
            "texidx.mul",
            "texmaps.mul",
            "tiledata.mul",
            "unifont.mul",
            "unifont1.mul",
            "unifont2.mul",
            "unifont3.mul",
            "unifont4.mul",
            "unifont5.mul",
            "unifont6.mul",
			"verdata.mul"
        };

		private Entry3D[] m_Index;
		private Stream m_Stream;
        public static IDictionary MulPath=new Hashtable();

		public Entry3D[] Index{ get{ return m_Index; } }
		public Stream Stream{ get{ return m_Stream; } }

        public static void LoadMulPath()
        {
            MulPath = new Hashtable();
            string path;
            try
            {
                path = (string)Client.Directories[0];
            }
            catch
            {
                path = "";
            }
            string filePath;
            foreach (string file in m_Files)
            {
                filePath = Path.Combine(path, file);
                if (File.Exists(filePath))
                    MulPath[file] = filePath;
                else
                    MulPath[file] = "";
            }
        }

		public Stream Seek( int index, out int length, out int extra, out bool patched )
		{
			if ( index < 0 || index >= m_Index.Length )
			{
				length = extra = 0;
				patched = false;
				return null;
			}

			Entry3D e = m_Index[index];

			if ( e.lookup < 0 )
			{
				length = extra = 0;
				patched = false;
				return null;
			}

			length = e.length & 0x7FFFFFFF;
			extra = e.extra;

			if ( (e.length & (1 << 31)) != 0 )
			{
				patched = true;

				Verdata.Stream.Seek( e.lookup, SeekOrigin.Begin );
				return Verdata.Stream;
			}
			else if ( m_Stream == null )
			{
				length = extra = 0;
				patched = false;
				return null;
			}

			patched = false;

			m_Stream.Seek( e.lookup, SeekOrigin.Begin );
			return m_Stream;
		}

		public FileIndex( string idxFile, string mulFile, int length, int file )
		{
			m_Index = new Entry3D[length];

            string idxPath;
            string mulPath;
            if (MulPath.Count > 0)
            {
                idxPath = MulPath[idxFile.ToLower()].ToString();
                mulPath = MulPath[mulFile.ToLower()].ToString();
                if (!File.Exists(idxPath))
                    idxPath = Client.GetFilePath(idxFile);
                if (!File.Exists(mulPath))
                    mulPath = Client.GetFilePath(mulFile);
            }
            else
            {
                idxPath = Client.GetFilePath(idxFile);
                mulPath = Client.GetFilePath(mulFile);
            }

			if ( idxPath != null && mulPath != null )
			{
				using ( FileStream index = new FileStream( idxPath, FileMode.Open, FileAccess.Read, FileShare.Read ) )
				{
					BinaryReader bin = new BinaryReader( index );
					m_Stream = new FileStream( mulPath, FileMode.Open, FileAccess.Read, FileShare.Read );

					int count = (int)(index.Length / 12);

					for ( int i = 0; i < count && i < length; ++i )
					{
						m_Index[i].lookup = bin.ReadInt32();
						m_Index[i].length = bin.ReadInt32();
						m_Index[i].extra = bin.ReadInt32();
					}

					for ( int i = count; i < length; ++i )
					{
						m_Index[i].lookup = -1;
						m_Index[i].length = -1;
						m_Index[i].extra = -1;
					}
				}
			}

			Entry5D[] patches = Verdata.Patches;

			for ( int i = 0; i < patches.Length; ++i )
			{
				Entry5D patch = patches[i];

				if ( patch.file == file && patch.index >= 0 && patch.index < length )
				{
					m_Index[patch.index].lookup = patch.lookup;
					m_Index[patch.index].length = patch.length | (1 << 31);
					m_Index[patch.index].extra = patch.extra;
				}
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