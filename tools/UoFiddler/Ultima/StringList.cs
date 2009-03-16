using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Ultima
{
    public sealed class StringList
    {
        private int m_Header1;
        private short m_Header2;

        public ArrayList Entries { get; set; }
        public string Language { get; private set; }

        private static byte[] m_Buffer = new byte[1024];

        /// <summary>
        /// Initialize <see cref="StringList"/> of Language
        /// </summary>
        /// <param name="language"></param>
        public StringList(string language)
        {
            Language = language;

            string path = Files.GetFilePath(String.Format("cliloc.{0}", language));

            if (path == null)
            {
                Entries = new ArrayList(0);
                return;
            }
            Entries = new ArrayList();

            using (BinaryReader bin = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                m_Header1 = bin.ReadInt32();
                m_Header2 = bin.ReadInt16();

                while (bin.BaseStream.Length != bin.BaseStream.Position)
                {
                    int number = bin.ReadInt32();
                    byte flag = bin.ReadByte();
                    int length = bin.ReadInt16();

                    if (length > m_Buffer.Length)
                        m_Buffer = new byte[(length + 1023) & ~1023];

                    bin.Read(m_Buffer, 0, length);
                    string text = Encoding.UTF8.GetString(m_Buffer, 0, length);

                    Entries.Add(new StringEntry(number, text, flag));
                }
            }
        }

        /// <summary>
        /// Saves <see cref="SaveStringList"/> to FileName
        /// </summary>
        /// <param name="FileName"></param>
        public void SaveStringList(string FileName)
        {
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryWriter bin = new BinaryWriter(fs);
                bin.Write(m_Header1);
                bin.Write(m_Header2);
                Entries.Sort(new StringList.NumberComparer(false));
                foreach (StringEntry entry in Entries)
                {
                    bin.Write(entry.Number);
                    bin.Write((byte)entry.Flag);
                    byte[] utf8String = Encoding.UTF8.GetBytes(entry.Text);
                    ushort length = (ushort)utf8String.Length;
                    bin.Write(length);
                    bin.Write(utf8String);
                }
            }
        }

        #region SortComparer

        public class NumberComparer : IComparer
        {
            private bool m_desc;

            public NumberComparer(bool desc)
            {
                m_desc = desc;
            }

            public int Compare(object objA, object objB)
            {
                StringEntry entryA = (StringEntry)objA;
                StringEntry entryB = (StringEntry)objB;
                if (entryA.Number == entryB.Number)
                    return 0;
                else if (m_desc)
                    return (entryA.Number < entryB.Number) ? 1 : -1;
                else
                    return (entryA.Number < entryB.Number) ? -1 : 1;
            }
        }

        public class FlagComparer : IComparer
        {
            private bool m_desc;

            public FlagComparer(bool desc)
            {
                m_desc = desc;
            }

            public int Compare(object objA, object objB)
            {
                StringEntry entryA = (StringEntry)objA;
                StringEntry entryB = (StringEntry)objB;
                if ((byte)entryA.Flag == (byte)entryB.Flag)
                {
                    if (entryA.Number == entryB.Number)
                        return 0;
                    else if (m_desc)
                        return (entryA.Number < entryB.Number) ? 1 : -1;
                    else
                        return (entryA.Number < entryB.Number) ? -1 : 1;
                }
                else if (m_desc)
                    return ((byte)entryA.Flag < (byte)entryB.Flag) ? 1 : -1;
                else
                    return ((byte)entryA.Flag < (byte)entryB.Flag) ? -1 : 1;
            }
        }

        public class TextComparer : IComparer
        {
            private bool m_desc;

            public TextComparer(bool desc)
            {
                m_desc = desc;
            }

            public int Compare(object objA, object objB)
            {
                StringEntry entryA = (StringEntry)objA;
                StringEntry entryB = (StringEntry)objB;
                if (m_desc)
                    return String.Compare(entryB.Text, entryA.Text);
                else
                    return String.Compare(entryA.Text, entryB.Text);
            }
        }
        #endregion
    }
}