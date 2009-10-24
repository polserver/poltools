using System.IO;

namespace Ultima
{
    public sealed class FileIndex
    {
        public Entry3D[] Index { get; private set; }
        public Stream Stream { get; private set; }
        public long IdxLength { get; private set; }
        private string MulPath;

        public Stream Seek(int index, out int length, out int extra, out bool patched)
        {
            if (index < 0 || index >= Index.Length)
            {
                length = extra = 0;
                patched = false;
                return null;
            }

            Entry3D e = Index[index];

            if (e.lookup < 0)
            {
                length = extra = 0;
                patched = false;
                return null;
            }
            if (e.length < 0)
            {
                length = extra = 0;
                patched = false;
                return null;
            }

            length = e.length & 0x7FFFFFFF;
            extra = e.extra;

            if ((e.length & (1 << 31)) != 0)
            {
                patched = true;
                Verdata.Seek(e.lookup);
                return Verdata.Stream;
            }

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
                patched = false;
                return null;
            }
            else if (Stream.Length < e.lookup)
            {
                length = extra = 0;
                patched = false;
                return null;
            }

            patched = false;

            Stream.Seek(e.lookup, SeekOrigin.Begin);
            return Stream;
        }

        public FileIndex(string idxFile, string mulFile, int length, int file)
        {
            Index = new Entry3D[length];

            string idxPath = null;
            MulPath = null;
            if (Files.MulPath == null)
                Files.LoadMulPath();
            if (Files.MulPath.Count > 0)
            {
                idxPath = Files.MulPath[idxFile.ToLower()].ToString();
                MulPath = Files.MulPath[mulFile.ToLower()].ToString();
                if (idxPath == "")
                    idxPath = null;
                else
                {
                    if (Path.GetDirectoryName(idxPath) == "")
                        idxPath = Path.Combine(Files.RootDir, idxPath);
                    if (!File.Exists(idxPath))
                        idxPath = null;
                }
                if (MulPath == "")
                    MulPath = null;
                else
                {
                    if (Path.GetDirectoryName(MulPath) == "")
                        MulPath = Path.Combine(Files.RootDir, MulPath);
                    if (!File.Exists(MulPath))
                        MulPath = null;
                }
            }

            if ((idxPath != null) && (MulPath != null))
            {
                using (FileStream index = new FileStream(idxPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BinaryReader bin = new BinaryReader(index))
                    {
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
            }
            else
            {
                Stream = null;
                return;
            }
            Entry5D[] patches = Verdata.Patches;

            if (file > -1)
            {
                for (int i = 0; i < patches.Length; ++i)
                {
                    Entry5D patch = patches[i];

                    if (patch.file == file && patch.index >= 0 && patch.index < length)
                    {
                        Index[patch.index].lookup = patch.lookup;
                        Index[patch.index].length = patch.length | (1 << 31);
                        Index[patch.index].extra = patch.extra;
                    }
                }
            }
        }

        public FileIndex(string idxFile, string mulFile, int file)
        {
            string idxPath = null;
            MulPath = null;
            if (Files.MulPath == null)
                Files.LoadMulPath();
            if (Files.MulPath.Count > 0)
            {
                idxPath = Files.MulPath[idxFile.ToLower()].ToString();
                MulPath = Files.MulPath[mulFile.ToLower()].ToString();
                if (idxPath == "")
                    idxPath = null;
                else
                {
                    if (Path.GetDirectoryName(idxPath) == "")
                        idxPath = Path.Combine(Files.RootDir, idxPath);
                    if (!File.Exists(idxPath))
                        idxPath = null;
                }
                if (MulPath == "")
                    MulPath = null;
                else
                {
                    if (Path.GetDirectoryName(MulPath) == "")
                        MulPath = Path.Combine(Files.RootDir, MulPath);
                    if (!File.Exists(MulPath))
                        MulPath = null;
                }
            }

            if ((idxPath != null) && (MulPath != null))
            {
                using (FileStream index = new FileStream(idxPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BinaryReader bin = new BinaryReader(index))
                    {
                        Stream = new FileStream(MulPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        int count = (int)(index.Length / 12);
                        IdxLength = index.Length;
                        Index = new Entry3D[count];
                        for (int i = 0; i < count; ++i)
                        {
                            Index[i].lookup = bin.ReadInt32();
                            Index[i].length = bin.ReadInt32();
                            Index[i].extra = bin.ReadInt32();
                        }
                    }
                }
            }
            else
            {
                Stream = null;
                Index = new Entry3D[1];
                return;
            }
            Entry5D[] patches = Verdata.Patches;

            if (file > -1)
            {
                for (int i = 0; i < patches.Length; ++i)
                {
                    Entry5D patch = patches[i];

                    if (patch.file == file && patch.index >= 0 && patch.index < Index.Length)
                    {
                        Index[patch.index].lookup = patch.lookup;
                        Index[patch.index].length = patch.length | (1 << 31);
                        Index[patch.index].extra = patch.extra;
                    }
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