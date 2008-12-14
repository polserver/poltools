using System.IO;
using System.Text;

namespace Ultima
{
    public sealed class Skills
    {
        private static FileIndex m_FileIndex = new FileIndex("skills.idx", "skills.mul", 55, -1);

        public Skills()
        {
        }

        /// <summary>
        /// ReReads skills.mul
        /// </summary>
        public static void Reload()
        {
            m_FileIndex = new FileIndex("skills.idx", "skills.mul", 55, -1);
        }
        public struct SkillInfo
        {
            public bool IsAction;
            public string Name;
        }

        /// <summary>
        /// Returns <see cref="SkillInfo"/> of index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static SkillInfo GetSkill(int index)
        {
            int length, extra;
            bool patched;
            SkillInfo info=new SkillInfo();
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);
            if (stream == null)
                return info;

            BinaryReader bin = new BinaryReader(stream);
            info.IsAction = bin.ReadBoolean();
            info.Name = ReadNameString(bin,length-1);

            return info;
        }

        private static byte[] m_StringBuffer = new byte[40];
        private static string ReadNameString(BinaryReader bin,int length)
        {
            bin.Read(m_StringBuffer, 0, length);
            int count;
            for (count = 0; count < length && m_StringBuffer[count] != 0; ++count) ;

            return Encoding.Default.GetString(m_StringBuffer, 0, count);
        }
    }
}
