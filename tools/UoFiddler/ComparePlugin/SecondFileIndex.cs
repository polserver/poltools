using System.IO;

namespace ComparePlugin
{
    public sealed class SecondFileIndex
    {
        private Entry3D[] m_Index;
        private Stream m_Stream;
        public long IdxLength { get; private set; }

        public Entry3D[] Index { get { return m_Index; } }
        public Stream Stream { get { return m_Stream; } }

        public Stream Seek(int index, out int length, out int extra)
        {
            if (index < 0 || index >= m_Index.Length)
            {
                length = extra = 0;
                return null;
            }

            Entry3D e = m_Index[index];

            if (e.lookup < 0)
            {
                length = extra = 0;
                return null;
            }

            length = e.length & 0x7FFFFFFF;
            extra = e.extra;

            if (m_Stream == null)
            {
                length = extra = 0;
                return null;
            }

            m_Stream.Seek(e.lookup, SeekOrigin.Begin);
            return m_Stream;
        }

        public SecondFileIndex(string idxFile, string mulFile, int length)
        {
            m_Index = new Entry3D[length];

            if (!File.Exists(idxFile))
                idxFile = null;
            if (!File.Exists(mulFile))
                mulFile = null;

            if (idxFile != null && mulFile != null)
            {
                using (FileStream index = new FileStream(idxFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryReader bin = new BinaryReader(index);
                    m_Stream = new FileStream(mulFile, FileMode.Open, FileAccess.Read, FileShare.Read);

                    int count = (int)(index.Length / 12);
                    IdxLength = index.Length;

                    for (int i = 0; i < count && i < length; ++i)
                    {
                        m_Index[i].lookup = bin.ReadInt32();
                        m_Index[i].length = bin.ReadInt32();
                        m_Index[i].extra = bin.ReadInt32();
                    }

                    for (int i = count; i < length; ++i)
                    {
                        m_Index[i].lookup = -1;
                        m_Index[i].length = -1;
                        m_Index[i].extra = -1;
                    }
                }
            }
            else
            {
                m_Stream = null;
                return;
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
