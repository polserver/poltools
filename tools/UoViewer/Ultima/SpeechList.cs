using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Ultima
{
    public sealed class SpeechList
    {
        private static ArrayList m_Entries;
        public static ArrayList Entries { get { return m_Entries; } set { m_Entries = value; } }

        private static byte[] m_Buffer = new byte[128];

        static SpeechList()
        {
            Initialize();
        }

        /// <summary>
        /// Loads speech.mul in <see cref="SpeechList.Entries"/>
        /// </summary>
        public static void Initialize()
        {
            string path = Files.GetFilePath("speech.mul");
            if (path == null)
            {
                m_Entries = new ArrayList(0);
                return;
            }
            m_Entries = new ArrayList();
            using (BinaryReader bin = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                int order=0;
                while (bin.BaseStream.Length != bin.BaseStream.Position)
                {
                    short id = NativeMethods.SwapEndian(bin.ReadInt16());
                    short length = NativeMethods.SwapEndian(bin.ReadInt16());
                    if (length > 128)
                        length = 128;
                    bin.Read(m_Buffer, 0, length);
                    string keyword = Encoding.UTF8.GetString(m_Buffer, 0, length);
                    m_Entries.Add(new SpeechEntry(id, keyword,order));
                    ++order;
                }
            }
        }

        /// <summary>
        /// Saves speech.mul to <see cref="FileName"/>
        /// </summary>
        /// <param name="FileName"></param>
        public static void SaveSpeechList(string FileName)
        {
            Entries.Sort(new OrderComparer());
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryWriter bin = new BinaryWriter(fs);
                foreach (SpeechEntry entry in Entries)
                {
                    bin.Write(NativeMethods.SwapEndian(entry.ID));
                    byte[] utf8String = Encoding.UTF8.GetBytes(entry.KeyWord);
                    short length=(short)utf8String.Length;
                    bin.Write(NativeMethods.SwapEndian(length));
                    bin.Write(utf8String);
                }
            }
        }

        #region SortComparer
        public class IDComparer : IComparer
        {
            private bool m_desc;

            public IDComparer(bool desc)
            {
                m_desc = desc;
            }

            public int Compare(object objA, object objB)
            {
                SpeechEntry entryA = (SpeechEntry)objA;
                SpeechEntry entryB = (SpeechEntry)objB;
                if (entryA.ID == entryB.ID)
                    return 0;
                else if (m_desc)
                    return (entryA.ID < entryB.ID) ? 1 : -1;
                else
                    return (entryA.ID < entryB.ID) ? -1 : 1;
            }
        }

        public class KeyWordComparer : IComparer
        {
            private bool m_desc;

            public KeyWordComparer(bool desc)
            {
                m_desc = desc;
            }

            public int Compare(object objA, object objB)
            {
                SpeechEntry entryA = (SpeechEntry)objA;
                SpeechEntry entryB = (SpeechEntry)objB;
                if (m_desc)
                    return String.Compare(entryB.KeyWord, entryA.KeyWord);
                else
                    return String.Compare(entryA.KeyWord, entryB.KeyWord);
            }
        }

        public class OrderComparer : IComparer
        {
            public int Compare(object objA, object objB)
            {
                SpeechEntry entryA = (SpeechEntry)objA;
                SpeechEntry entryB = (SpeechEntry)objB;
                if (entryA.Order == entryB.Order)
                    return 0;
                else
                    return (entryA.Order < entryB.Order) ? -1 : 1;
            }
        }

        #endregion

    }

    public sealed class SpeechEntry
    {
        private short m_ID;
        private string m_KeyWord;
        private int m_Order;

        public short ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public string KeyWord
        {
            get { return m_KeyWord; }
            set { m_KeyWord = value; }
        }

        [Browsable(false)]
        public int Order
        {
            get { return m_Order; }
        }

        public SpeechEntry(short id, string keyword,int order)
        {
            m_ID = id;
            m_KeyWord = keyword;
            m_Order = order;
        }
    }
}
