using System.IO;

namespace ComparePlugin
{
    public sealed class SecondFileIndex
    {
        public Entry3D[] Index { get; private set; }
        public Stream Stream { get; private set; }
        public long IdxLength { get; private set; }
        private string MulPath;

        public Stream Seek(int index, out int length, out int extra)
        {
            if (index < 0 || index >= Index.Length)
            {
                length = extra = 0;
                return null;
            }

            Entry3D e = Index[index];

            if (e.lookup < 0)
            {
                length = extra = 0;
                return null;
            }

            length = e.length & 0x7FFFFFFF;
            extra = e.extra;

            if ((Stream == null) || (!Stream.CanRead) || (!Stream.CanSeek))
            {
                if (MulPath == null)
                    Stream = null;
                else
                    Stream = new FileStream(MulPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            if (Stream == null)
            {
                length = extra = 0;
                return null;
            }

            Stream.Seek(e.lookup, SeekOrigin.Begin);
            return Stream;
        }

        public SecondFileIndex(string idxFile, string mulFile, int length)
        {
            Index = new Entry3D[length];

            MulPath = mulFile;
            if (!File.Exists(idxFile))
                idxFile = null;
            if (!File.Exists(MulPath))
                MulPath = null;

            if (idxFile != null && MulPath != null)
            {
                using (FileStream index = new FileStream(idxFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryReader bin = new BinaryReader(index);
                    Stream = new FileStream(MulPath, FileMode.Open, FileAccess.Read, FileShare.Read);

                    int count = (int)(index.Length / 12);
                    IdxLength = index.Length;

                    for (int i = 0; i < count && i < length; ++i)
                    {
                        Index[i].lookup = bin.ReadInt32();
                        Index[i].length = bin.ReadInt32();
                        Index[i].extra = bin.ReadInt32();
                    }

                    for (int i = count; i < length; ++i)
                    {
                        Index[i].lookup = -1;
                        Index[i].length = -1;
                        Index[i].extra = -1;
                    }
                }
            }
            else
            {
                Stream = null;
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
