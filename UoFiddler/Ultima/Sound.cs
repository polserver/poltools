using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Ultima
{
    public sealed class UOSound
    {
        public string Name;
        public int ID;
        public byte[] buffer;

        public UOSound(string name, int id, byte[] buff)
        {
            Name = name;
            ID = id;
            buffer = buff;
        }
    };

    public static class Sounds
    {
        private static Dictionary<int, int> m_Translations;
        private static FileIndex m_FileIndex;
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
			m_FileIndex = new FileIndex("soundidx.mul", "sound.mul", "soundLegacyMUL.uop", 0xFFF, 8, ".dat", -1, false);
            var reg = new Regex(@"(\d{1,4}) \x7B(\d{1,4})\x7D (\d{1,3})", RegexOptions.Compiled);

            m_Translations = new Dictionary<int, int>();

            string line;
            string path = Files.GetFilePath("Sound.def");
            if (path == null)
            {
                return;
            }

            using (var reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (((line = line.Trim()).Length != 0) && !line.StartsWith("#"))
                    {
                        Match match = reg.Match(line);

                        if (match.Success)
                        {
                            m_Translations.Add(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                        }
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
        public static UOSound GetSound(int soundID, out bool translated)
        {
            translated = false;
            if (soundID < 0)
            {
                return null;
            }

            if (m_Removed[soundID])
            {
                return null;
            }

            if (m_Cache[soundID] != null)
            {
                return m_Cache[soundID];
            }

            int length, extra;
            bool patched;
            Stream stream = m_FileIndex.Seek(soundID, out length, out extra, out patched);

            if ((m_FileIndex.Index[soundID].lookup < 0) || (length <= 0))
            {
                if (!m_Translations.TryGetValue(soundID, out soundID))
                {
                    return null;
                }

                translated = true;
                stream = m_FileIndex.Seek(soundID, out length, out extra, out patched);
            }

            if (stream == null)
            {
                return null;
            }

            length -= 32;
            int[] waveHeader = WaveHeader(length);

            var stringBuffer = new byte[32];
            var buffer = new byte[length];

            stream.Read(stringBuffer, 0, 32);
            stream.Read(buffer, 0, length);
            stream.Close();

            var resultBuffer = new byte[buffer.Length + (waveHeader.Length << 2)];

            Buffer.BlockCopy(waveHeader, 0, resultBuffer, 0, (waveHeader.Length << 2));
            Buffer.BlockCopy(buffer, 0, resultBuffer, (waveHeader.Length << 2), buffer.Length);

            string str = Encoding.ASCII.GetString(stringBuffer); 
                // seems that the null terminator's not being properly recognized :/
            if (str.IndexOf('\0') > 0)
            {
                str = str.Substring(0, str.IndexOf('\0'));
            }
            var sound = new UOSound(str, soundID, resultBuffer);

            if (Files.CacheData)
            {
                if (!translated) // no .def definition
                {
                    m_Cache[soundID] = sound;
                }
            }

            return sound;
        }

        private static int[] WaveHeader(int length)
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
            return new[] 
            { 0x46464952, (length + 36), 0x45564157, 0x20746D66, 0x10, 0x010001, 0x5622, 0xAC44, 0x100002, 0x61746164, length };
        }

        /// <summary>
        /// Returns Soundname and tests if valid
        /// </summary>
        /// <param name="soundID"></param>
        /// <returns></returns>
        public static bool IsValidSound(int soundID, out string name, out bool translated)
        {
            translated = false;
            name = "";
            if (soundID < 0)
            {
                return false;
            }

            if (m_Removed[soundID])
            {
                return false;
            }

            if (m_Cache[soundID] != null)
            {
                name = m_Cache[soundID].Name;
                return true;
            }

            int length, extra;
            bool patched;
            Stream stream = m_FileIndex.Seek(soundID, out length, out extra, out patched);

            if ((m_FileIndex.Index[soundID].lookup < 0) || (length <= 0))
            {
                if (!m_Translations.TryGetValue(soundID, out soundID))
                {
                    return false;
                }
                else
                {
                    translated = true;
                }

                stream = m_FileIndex.Seek(soundID, out length, out extra, out patched);
            }
            if (stream == null)
            {
                return false;
            }

            var stringBuffer = new byte[32];
            stream.Read(stringBuffer, 0, 32);
            stream.Close();
            name = Encoding.ASCII.GetString(stringBuffer); // seems that the null terminator's not being properly recognized :/
            if (name.IndexOf('\0') > 0)
            {
                name = name.Substring(0, name.IndexOf('\0'));
            }

            return true;
        }

        /// <summary>
        /// Returns length of SoundID
        /// </summary>
        /// <param name="soundID"></param>
        /// <returns></returns>
        public static double GetSoundLength(int soundID)
        {
            if (soundID < 0)
            {
                return 0;
            }

            if (m_Removed[soundID])
            {
                return 0;
            }

            double len;
            if (m_Cache[soundID] != null)
            {
                len = m_Cache[soundID].buffer.Length;
                len -= 44; //wavheaderlength
            }
            else
            {
                int length, extra;
                bool patched;
                Stream stream = m_FileIndex.Seek(soundID, out length, out extra, out patched);
                if ((m_FileIndex.Index[soundID].lookup < 0) || (length <= 0))
                {
                    if (!m_Translations.TryGetValue(soundID, out soundID))
                    {
                        return 0;
                    }

                    stream = m_FileIndex.Seek(soundID, out length, out extra, out patched);
                }

                if (stream == null)
                {
                    return 0;
                }

                stream.Close();
                length -= 32; //mulheaderlength
                len = length;
            }
            len /= 0x5622; // Sample Rate
            len /= 2;
            return len;
        }

        public static void Add(int id, string name, string file)
        {
            using (FileStream wav = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] resultBuffer = new byte[wav.Length];
                wav.Seek(0, SeekOrigin.Begin);
                wav.Read(resultBuffer, 0, (int)wav.Length);

                resultBuffer = CheckAndFixWave(resultBuffer);

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
            using (
                FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write),
                              fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (BinaryWriter binidx = new BinaryWriter(fsidx), binmul = new BinaryWriter(fsmul))
                {
                    for (int i = 0; i < m_Cache.Length; ++i)
                    {
                        UOSound sound = m_Cache[i];
                        if ((sound == null) && (!m_Removed[i]))
                        {
                            bool trans;
                            sound = GetSound(i, out trans);
                            if (!trans)
                            {
                                m_Cache[i] = sound;
                            }
                            else
                            {
                                sound = null;
                            }
                        }
                        if ((sound == null) || (m_Removed[i]))
                        {
                            binidx.Write(-1); // lookup
                            binidx.Write(-1); // length
                            binidx.Write(-1); // extra
                        }
                        else
                        {
                            binidx.Write((int)fsmul.Position); //lookup
                            var length = (int)fsmul.Position;

                            var b = new byte[32];
                            if (sound.Name != null)
                            {
                                byte[] bb = Encoding.Default.GetBytes(sound.Name);
                                if (bb.Length > 32)
                                {
                                    Array.Resize(ref bb, 32);
                                }

                                bb.CopyTo(b, 0);
                            }
                            binmul.Write(b);
                            using (var m = new MemoryStream(sound.buffer))
                            {
                                m.Seek(Headerlength, SeekOrigin.Begin);
                                var resultBuffer = new byte[m.Length - Headerlength];
                                m.Read(resultBuffer, 0, (int)m.Length - Headerlength);
                                binmul.Write(resultBuffer);
                            }

                            length = (int)fsmul.Position - length;
                            binidx.Write(length);
                            binidx.Write(i + 1);
                        }
                    }
                }
            }
        }

        public static void SaveSoundListToCSV(string FileName)
        {
            using (
                var Tex = new StreamWriter(
                    new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite), System.Text.Encoding.GetEncoding(1252)))
            {
                Tex.WriteLine("ID;Name;Length");
                string name = "";
                bool translated = false;
                for (int i = 1; i <= 0xFFF; ++i)
                {
                    if (IsValidSound(i - 1, out name, out translated))
                    {
                        Tex.Write(String.Format("0x{0:X3}", i));
                        Tex.Write(String.Format(";{0}", name));
                        Tex.WriteLine(String.Format(";{0:f}", GetSoundLength(i - 1)));
                    }
                }
            }
        }

        public static bool IsRemovedSound(int soundID)
        {
            if (soundID < 0)
            {
                return true;
            }

            return m_Removed[soundID];
        }

        private static byte[] CheckAndFixWave(byte[] inputBuffer)
        {
            if (inputBuffer.Length < 44)
            {
                throw new WaveFormatException("Invalid File.");
            }

            try
            {
                var croppedByteCount = 0;
                using (var outputMemoryStream = new MemoryStream())
                {
                    using (var output = new BinaryWriter(outputMemoryStream))
                    {
                        using (var inputMemoryStream = new MemoryStream(inputBuffer))
                        {
                            using (var input = new BinaryReader(inputMemoryStream))
                            {
                                // RIFF HEADER
                                if (input.ReadString(4) != "RIFF")
                                {
                                    throw new WaveFormatException("RIFF Header not found.");
                                }
                                output.WriteString("RIFF");

                                var riffLength = input.ReadUInt32();
                                output.Write(riffLength);
                                Debug.WriteLine("RIFF length: " + riffLength + " bytes");

                                if (input.ReadString(4) != "WAVE")
                                {
                                    throw new WaveFormatException("WAVE Format not found.");
                                }
                                output.WriteString("WAVE");

                                var fmtWritten = false;
                                while (inputMemoryStream.Position < inputMemoryStream.Length)
                                {
                                    var currentHeader = input.ReadString(4);
                                    var currentLength = input.ReadInt32();

                                    /////////////////////////////////////////////////////////////////////////
                                    if (currentHeader == "fmt ")
                                    {
                                        if (currentLength != 16)
                                        {
                                            throw new WaveFormatException("FMT size not supported.");
                                        }

                                        output.WriteString(currentHeader);
                                        output.Write(currentLength);

                                        var waveFormat = (WaveFormat)input.ReadInt16();
                                        if (waveFormat != WaveFormat.PCM)
                                        {
                                            throw new WaveFormatException("Format not supported: PCM expected but " + waveFormat + " given!");
                                        }
                                        output.Write((short)waveFormat);

                                        var channels = input.ReadUInt16();
                                        if (channels != 1)
                                        {
                                            throw new WaveFormatException("Only Mono supported, but " + channels + " channels given!");
                                        }
                                        output.Write(channels);

                                        var frequency = input.ReadUInt32();
                                        if (frequency != 22050)
                                        {
                                            throw new WaveFormatException("Only 22050Hz supported, but " + frequency + "Hz given!");
                                        }
                                        output.Write(frequency);

                                        var rate = input.ReadUInt32();
                                        output.Write(rate);
                                        Debug.WriteLine("Rate: " + rate + " byte/s");
                                        var align = input.ReadUInt16();
                                        output.Write(align);
                                        Debug.WriteLine("Align: " + align);

                                        var depth = input.ReadUInt16();
                                        if (depth != 16)
                                        {
                                            throw new WaveFormatException("Only 16 bits supported, but " + depth + " bits given!");
                                        }
                                        output.Write(depth);
                                        Debug.WriteLine("Depth: " + depth + " bits");

                                        fmtWritten = true;
                                    }
                                    /////////////////////////////////////////////////////////////////////////
                                    else if (currentHeader == "data")
                                    {
                                        if (!fmtWritten)
                                        {
                                            throw new WaveFormatException("Unexpected wave file order.");
                                        }

                                        output.WriteString(currentHeader);
                                        output.Write(currentLength);

                                        Debug.WriteLine("Data length: " + currentLength + " bytes");
                                        var data = input.ReadBytes(currentLength);

                                        output.Write(data);
                                    }
                                    /////////////////////////////////////////////////////////////////////////
                                    else
                                    {
                                        croppedByteCount += currentLength + 8;
                                        input.ReadBytes(currentLength); // throw away
                                    }
                                }

                                // Adjust riff length
                                outputMemoryStream.Position = 4;
                                output.Write((uint)(riffLength - croppedByteCount));

                                return outputMemoryStream.ToArray();
                            }
                        }
                    }
                }
            }
            catch (EndOfStreamException ex)
            {
                throw new WaveFormatException("Unexpected Data end.", ex);
            }
        }
    }
}
