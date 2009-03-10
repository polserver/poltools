using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;

namespace Ultima
{
	public sealed class UOSound
	{
		public string Name;
        public int ID;
        public byte[] buffer;

		public UOSound( string name, int id, byte[] buff )
		{
			Name = name;
            ID = id;
            buffer = buff;
		}
	};

	public static class Sounds
	{
		private static BinaryReader m_Index;
		private static Stream m_Stream;
		private static Dictionary<int, int> m_Translations;
        private static Stream m_Indexstream;
        private static UOSound[] m_Cache;
        private static bool[] m_Removed;

		static Sounds()
		{
            Initialize();
		}

        /// <summary>
        /// Reads Sounds and def
        /// </summary>
        public static void Initialize()
        {
            m_Cache = new UOSound[0xFFF];
            m_Removed = new bool[0xFFF];
            string path = Files.GetFilePath("soundidx.mul");
            if (path == null)
                return;
            m_Indexstream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            m_Index = new BinaryReader(m_Indexstream);
            m_Stream = new FileStream(Files.GetFilePath("sound.mul"), FileMode.Open, FileAccess.Read, FileShare.Read);
            Regex reg = new Regex(@"(\d{1,3}) \x7B(\d{1,3})\x7D (\d{1,3})", RegexOptions.Compiled);

            m_Translations = new Dictionary<int, int>();

            string line;
            path = Files.GetFilePath("Sound.def");
            if (path == null)
                return;
            using (StreamReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (((line = line.Trim()).Length != 0) && !line.StartsWith("#"))
                    {
                        Match match = reg.Match(line);

                        if (match.Success)
                            m_Translations.Add(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                    }
                }
            }
        }

        /// <summary>
        /// Returns <see cref="UOSound"/> of ID
        /// </summary>
        /// <param name="soundID"></param>
        /// <returns></returns>
        public static UOSound GetSound(int soundID)
        {
            bool translated;
            return GetSound(soundID, out translated);
        }

        /// <summary>
        /// Returns <see cref="UOSound"/> of ID with bool translated in .def
        /// </summary>
        /// <param name="soundID"></param>
        /// <param name="translated"></param>
        /// <returns></returns>
		public static UOSound GetSound( int soundID, out bool translated )
		{
            translated = false;
            if (soundID < 0)
                return null;
            if (m_Cache[soundID] != null)
                return m_Cache[soundID];

			m_Index.BaseStream.Seek( (long)( soundID * 12 ), SeekOrigin.Begin );

			int offset = m_Index.ReadInt32();
			int length = m_Index.ReadInt32();
			int extra = m_Index.ReadInt32();

            if ((offset < 0) || (length <= 0))
            {
                if (!m_Translations.TryGetValue(soundID, out soundID))
                    return null;

                translated = true;
                m_Index.BaseStream.Seek((long)(soundID * 12), SeekOrigin.Begin);

                offset = m_Index.ReadInt32();
                length = m_Index.ReadInt32();
                extra = m_Index.ReadInt32();
            }

            if ((offset < 0) || (length <= 0))
                return null;

			int[] waveHeader = WaveHeader( length );
            length -= 40;

			byte[] stringBuffer = new byte[ 40 ];
			byte[] buffer = new byte[ length ];

			m_Stream.Seek( (long)( offset ), SeekOrigin.Begin );
			m_Stream.Read( stringBuffer, 0, 40 );
			m_Stream.Read( buffer, 0, length );

			byte[] resultBuffer = new byte[ buffer.Length + ( waveHeader.Length << 2 ) ];

			Buffer.BlockCopy( waveHeader, 0, resultBuffer, 0, ( waveHeader.Length << 2 ) );
			Buffer.BlockCopy( buffer, 0, resultBuffer, ( waveHeader.Length << 2 ), buffer.Length );

			string str = Encoding.ASCII.GetString( stringBuffer ); // seems that the null terminator's not being properly recognized :/
            if (str.IndexOf('\0') > 0)
                str = str.Substring(0, str.IndexOf('\0'));
            UOSound sound = new UOSound( str, soundID, resultBuffer );

            if (Files.CacheData)
            {
                if (!translated) // no .def definition
                    m_Cache[soundID] = sound;
            }

            return sound;
		}

		private static int[] WaveHeader( int length )
		{
			/* ====================
			 * = WAVE File layout =
			 * ====================
			 * char[4] = 'RIFF' \
			 * int - chunk size |- Riff Header
			 * char[4] = 'WAVE' /
			 * char[4] = 'fmt ' \
			 * int - chunk size |
			 * short - format	|
			 * short - channels	|
			 * int - samples p/s|- Format header
			 * int - avg bytes	|
			 * short - align	|
			 * short - bits p/s /
			 * char[4] - data	\
			 * int - chunk size | - Data header
			 * short[..] - data /
			 * ====================
			 * */
            return new int[] { 0x46464952, (length + 12), 0x45564157, 0x20746D66, 0x10, 0x010001, 0x5622, 0xAC44, 0x100002, 0x61746164, (length - 24) };
		}

        /// <summary>
        /// Returns Soundname and tests if valid
        /// </summary>
        /// <param name="soundID"></param>
        /// <returns></returns>
        public static bool IsValidSound(int soundID, out string name)
        {
            name = "";
            if (soundID < 0)
                return false;
            if (m_Index == null)
                return false;
			m_Index.BaseStream.Seek( (long)( soundID * 12 ), SeekOrigin.Begin );

			int offset = m_Index.ReadInt32();
			int length = m_Index.ReadInt32();

			if( ( offset < 0 ) || ( length <= 0 ) )
			{
                if (!m_Translations.TryGetValue(soundID, out soundID))
                    return false;

				m_Index.BaseStream.Seek( (long)( soundID * 12 ), SeekOrigin.Begin );

				offset = m_Index.ReadInt32();
				length = m_Index.ReadInt32();
			}

            if ((offset < 0) || (length <= 0))
                return false;

            byte[] stringBuffer = new byte[40];
            m_Stream.Seek((long)(offset), SeekOrigin.Begin);
            m_Stream.Read(stringBuffer, 0, 40);
            name = Encoding.ASCII.GetString(stringBuffer); // seems that the null terminator's not being properly recognized :/
            if (name.IndexOf('\0')>0)
                name = name.Substring(0, name.IndexOf('\0'));
            return true;
        }

        /// <summary>
        /// Returns length of SoundID
        /// ToDo: not always correct
        /// </summary>
        /// <param name="soundID"></param>
        /// <returns></returns>
        public static double GetSoundLength(int soundID)
        {
            if (soundID < 0) 
                return 0;
            double len;
            if (m_Cache[soundID] != null)
            {
                len = (double)m_Cache[soundID].buffer.Length;
                len -= 44; //wavheaderlength
            }
            else
            {
                m_Index.BaseStream.Seek((long)(soundID * 12), SeekOrigin.Begin);

                int offset = m_Index.ReadInt32();
                int length = m_Index.ReadInt32();

                if ((offset < 0) || (length <= 0))
                {
                    if (!m_Translations.TryGetValue(soundID, out soundID))
                        return 0;

                    m_Index.BaseStream.Seek((long)(soundID * 12), SeekOrigin.Begin);

                    offset = m_Index.ReadInt32();
                    length = m_Index.ReadInt32();
                }

                if ((offset < 0) || (length <= 0))
                    return 0;
                length -= 40; //mulheaderlength
                len = (double)length;
            }
            len /= 0x5622; // Sample Rate
            len /= 2;
            return len;
        }

        public static void Add(int id,string name,string file)
        {
            using (FileStream wav = new FileStream(file,FileMode.Open,FileAccess.Read,FileShare.Read))
            {
                byte[] resultBuffer = new byte[wav.Length];
                wav.Seek(0, SeekOrigin.Begin);
                wav.Read(resultBuffer,0,(int)wav.Length);

                m_Cache[id] = new UOSound(name, id, resultBuffer);
                m_Removed[id] = false;
            }
        }

        public static void Remove(int id)
        {
            m_Removed[id] = true;
            m_Cache[id] = null;
        }

        public static void Save(string path)
        {
            string idx = Path.Combine(path, "soundidx.mul");
            string mul = Path.Combine(path, "sound.mul");
            int Headerlength = 44;
            using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryWriter binidx = new BinaryWriter(fsidx);
                using (FileStream fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    BinaryWriter binmul = new BinaryWriter(fsmul);
                    for (int i = 0; i < m_Cache.Length; i++)
                    {
                        UOSound sound = m_Cache[i];
                        if ((sound == null) && (!m_Removed[i]))
                        {
                            bool trans;
                            sound = GetSound(i, out trans);
                            if (!trans)
                                m_Cache[i] = sound;
                            else
                                sound = null;
                        }
                        if ((sound == null) || (m_Removed[i]))
                        {
                            binidx.Write((int)-1); // lookup
                            binidx.Write((int)-1); // length
                            binidx.Write((int)-1); // extra
                        }
                        else
                        {
                            binidx.Write((int)fsmul.Position); //lookup
                            int length = (int)fsmul.Position;
                            
                            byte[] b = new byte[40];
                            if (sound.Name != null)
                            {
                                byte[] bb = Encoding.Default.GetBytes(sound.Name);
                                if (bb.Length > 40)
                                    Array.Resize(ref bb, 40);
                                bb.CopyTo(b, 0);
                            }
                            binmul.Write(b);
                            MemoryStream m = new MemoryStream(sound.buffer);
                            m.Seek(Headerlength, SeekOrigin.Begin);
                            byte[] resultBuffer = new byte[m.Length - Headerlength];
                            m.Read(resultBuffer, 0, (int)m.Length - Headerlength);
                            binmul.Write(resultBuffer);

                            length = (int)fsmul.Position - length;
                            binidx.Write(length);
                            binidx.Write(i+1);
                        }
                    }
                }
            }
        }

        public static void SaveSoundListToCSV(string FileName)
        {
            using (StreamWriter Tex = new StreamWriter(new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite), System.Text.Encoding.GetEncoding(1252)))
            {
                Tex.WriteLine("ID;Name;Length");
                string name = "";
                for (int i = 1; i <= 0xFFF; i++)
                {
                    if (IsValidSound(i-1, out name))
                    {
                        Tex.Write(String.Format("0x{0:X3}", i));
                        Tex.Write(";" + name);
                        Tex.WriteLine(String.Format(";{0:f}",GetSoundLength(i - 1)));
                    }
                }

            }
        }
	}
}
