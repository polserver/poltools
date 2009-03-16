using System.IO;
using System.Text;
using System.Collections;
using System;

namespace Ultima
{
    public sealed class Skills
    {
        private static FileIndex m_FileIndex = new FileIndex("skills.idx", "skills.mul", 55, 16);

        private static ArrayList m_SkillEntries;
        public static ArrayList SkillEntries
        {
            get
            {
                if (m_SkillEntries == null)
                {
                    m_SkillEntries = new ArrayList();
                    for (int i = 0; i < 55; i++)
                    {
                        m_SkillEntries.Add(GetSkill(i));
                    }
                }
                return m_SkillEntries;
            }
            set { m_SkillEntries = value; }
        }

        public Skills()
        {

        }

        /// <summary>
        /// ReReads skills.mul
        /// </summary>
        public static void Reload()
        {
            m_FileIndex = new FileIndex("skills.idx", "skills.mul", 55, 16);
            m_SkillEntries = new ArrayList();
            for (int i = 0; i < 55; i++)
            {
                m_SkillEntries.Add(GetSkill(i));
            }
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

            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);
            if (stream == null)
                return null;


            BinaryReader bin = new BinaryReader(stream);
            bool action = bin.ReadBoolean();
            string name = ReadNameString(bin, length - 1);
            return new SkillInfo(index, name, action, extra);
        }

        private static byte[] m_StringBuffer = new byte[1024];
        private static string ReadNameString(BinaryReader bin, int length)
        {
            bin.Read(m_StringBuffer, 0, length);
            int count;
            for (count = 0; count < length && m_StringBuffer[count] != 0; ++count) ;

            return Encoding.Default.GetString(m_StringBuffer, 0, count);
        }

        public static void Save(string path)
        {
            string idx = Path.Combine(path, "skills.idx");
            string mul = Path.Combine(path, "skills.mul");
            using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryWriter binidx = new BinaryWriter(fsidx);
                using (FileStream fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    BinaryWriter binmul = new BinaryWriter(fsmul);
                    for (int i = 0; i < 55; i++)
                    {
                        SkillInfo skill = (SkillInfo)m_SkillEntries[i];
                        if (skill == null)
                        {
                            binidx.Write((int)-1); // lookup
                            binidx.Write((int)-1); // length
                            binidx.Write((int)-1); // extra
                        }
                        else
                        {
                            binidx.Write((int)fsmul.Position); //lookup
                            int length = (int)fsmul.Position;
                            binmul.Write(skill.IsAction);

                            byte[] namebytes = Encoding.Default.GetBytes(skill.Name);
                            binmul.Write(namebytes);
                            binmul.Write((byte)0); //nullterminated

                            length = (int)fsmul.Position - length;
                            binidx.Write(length);
                            binidx.Write(skill.Extra);
                        }
                    }
                }
            }
        }
    }

    public sealed class SkillInfo
    {
        private string m_Name;

        public int Index { get; private set; }
        public bool IsAction { get; set; }
        public string Name
        {
            get { return m_Name; }
            set
            {
                if (value == null)
                    m_Name = "";
                else
                    m_Name = value;
            }
        }
        public int Extra { get; private set; }


        public SkillInfo(int nr, string name, bool action, int extra)
        {
            Index = nr;
            m_Name = name;
            IsAction = action;
            Extra = extra;
        }
    }
}
