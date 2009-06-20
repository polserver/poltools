using System.IO;

// FileIDs
//0 - map0.mul
//1 - staidx0.mul
//2 - statics0.mul
//3 - artidx.mul
//4 - art.mul
//5 - anim.idx
//6 - anim.mul
//7 - soundidx.mul
//8 - sound.mul
//9 - texidx.mul
//10 - texmaps.mul
//11 - gumpidx.mul
//12 - gumpart.mul
//13 - multi.idx
//14 - multi.mul
//15 - skills.idx
//16 - skills.mul
//30 - tiledata.mul
//31 - animdata.mul 

namespace Ultima
{
    public sealed class Verdata
    {
        private static Entry5D[] m_Patches;
        private static Stream m_Stream;

        public static Stream Stream { get { return m_Stream; } }
        public static Entry5D[] Patches { get { return m_Patches; } }

        private static string path;

        static Verdata()
        {
            Initialize();
        }

        public static void Initialize()
        {
            path = Files.GetFilePath("verdata.mul");

            if (path == null)
            {
                m_Patches = new Entry5D[0];
                m_Stream = Stream.Null;
            }
            else
            {
                using (m_Stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BinaryReader bin = new BinaryReader(m_Stream))
                    {
                        m_Patches = new Entry5D[bin.ReadInt32()];

                        for (int i = 0; i < m_Patches.Length; ++i)
                        {
                            m_Patches[i].file = bin.ReadInt32();
                            m_Patches[i].index = bin.ReadInt32();
                            m_Patches[i].lookup = bin.ReadInt32();
                            m_Patches[i].length = bin.ReadInt32();
                            m_Patches[i].extra = bin.ReadInt32();
                        }
                    }
                }
            }
        }

        public static void Seek(int lookup)
        {
            if (Stream == null || !Stream.CanRead || !Stream.CanSeek)
            {
                if (path != null)
                    m_Stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            Stream.Seek(lookup, SeekOrigin.Begin);
        }
    }

    public struct Entry5D
    {
        public int file;
        public int index;
        public int lookup;
        public int length;
        public int extra;
    }
}