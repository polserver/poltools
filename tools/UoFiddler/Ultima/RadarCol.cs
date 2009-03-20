using System.IO;

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
            return m_Colors[index + 0x4000];
        }
        public static short GetLandColor(int index)
        {
            return m_Colors[index];
        }

        public static void SetItemColor(int index, short value)
        {
            m_Colors[index + 0x4000] = value;
        }
        public static void SetLandColor(int index,short value)
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
                    using (BinaryReader bin = new BinaryReader(fs))
                    {
                        m_Colors = new short[bin.BaseStream.Length/2];
                        int i = 0;
                        while (bin.BaseStream.Length != bin.BaseStream.Position)
                        {
                            m_Colors[i] = bin.ReadInt16();
                            i++;
                        }
                    }
                }
            }
        }

        public static void Save(string FileName)
        {
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryWriter bin = new BinaryWriter(fs);
                for (int i = 0; i < m_Colors.Length; i++)
                {
                    bin.Write(m_Colors[i]);
                }
            }
        }
    }
}
