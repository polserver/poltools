using System.IO;
using System.Runtime.InteropServices;

namespace Ultima
{
    public sealed class RadarCol
    {
        static RadarCol()
        {
            Initialize();
        }

        private static short[] m_Colors;
        public static short[] Colors { get { return m_Colors; } }

        public static short GetItemColor(int index)
        {
            if (index + 0x4000 < m_Colors.Length)
                return m_Colors[index + 0x4000];
            return 0;
        }
        public static short GetLandColor(int index)
        {
            if (index < m_Colors.Length)
                return m_Colors[index];
            return 0;
        }

        public static void SetItemColor(int index, short value)
        {
            m_Colors[index + 0x4000] = value;
        }
        public static void SetLandColor(int index, short value)
        {
            m_Colors[index] = value;
        }

        public static void Initialize()
        {
            string path = Files.GetFilePath("radarcol.mul");
            if (path != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    m_Colors = new short[fs.Length / 2];
                    GCHandle gc = GCHandle.Alloc(m_Colors, GCHandleType.Pinned);
                    byte[] buffer = new byte[(int)fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);
                    Marshal.Copy(buffer, 0, gc.AddrOfPinnedObject(), (int)fs.Length);
                    gc.Free();
                }
            }
            else
                m_Colors = new short[0x8000];
        }

        public static void Save(string FileName)
        {
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (BinaryWriter bin = new BinaryWriter(fs))
                {
                    for (int i = 0; i < m_Colors.Length; ++i)
                    {
                        bin.Write(m_Colors[i]);
                    }
                }
            }
        }
    }
}
