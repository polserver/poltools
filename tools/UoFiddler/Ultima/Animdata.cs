using System.Collections;
using System.IO;

namespace Ultima
{
    public sealed class Animdata
    {
        public static Hashtable AnimData;
        static Animdata()
        {
            Initialize();
        }

        /// <summary>
        /// Reads animdata.mul and fills <see cref="AnimData"/>
        /// </summary>
        public static void Initialize()
        {
            AnimData = new Hashtable();
            string path = Files.GetFilePath("animdata.mul");
            if (path != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryReader bin = new BinaryReader(fs);
                    int id=0;
                    byte unk, fcount, finter, fstart;
                    sbyte[] fdata;
                    while (bin.BaseStream.Length != bin.BaseStream.Position)
                    {
                        bin.ReadInt32(); // chunk header

                        // Read 8 tiles
                        for (int i = 0; i < 8; ++i)
                        {
                            fdata = new sbyte[64];
                            for (int j = 0; j < 64; ++j)
                            {
                                fdata[j] = bin.ReadSByte();
                            }
                            unk = bin.ReadByte();
                            fcount = bin.ReadByte();
                            finter = bin.ReadByte();
                            fstart = bin.ReadByte();
                            if (fcount > 0)
                                AnimData[id] = new Data(fdata, unk, fcount, finter, fstart);
                            ++id;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Gets Animation <see cref="Data"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Data GetAnimData(int id)
        {
            if (AnimData.Contains(id))
                return ((Data)AnimData[id]);
            else
                return null;
        }

        public class Data
        {
            private sbyte[] m_FrameData;
            private byte m_unknown;
            private byte m_FrameCount;
            private byte m_FrameInterval;
            private byte m_FrameStart;

            public sbyte[] FrameData { get { return m_FrameData; } }
            public byte Unknown { get { return m_unknown; } }
            public byte FrameCount { get { return m_FrameCount; } }
            public byte FrameInterval { get { return m_FrameInterval; } }
            public byte FrameStart { get { return m_FrameStart; } }

            public Data(sbyte[] frame, byte unk, byte fcount, byte finter, byte fstart)
            {
                m_FrameData = frame;
                m_unknown = unk;
                m_FrameCount = fcount;
                m_FrameInterval = finter;
                m_FrameStart = fstart;
            }
        }
    }
}
