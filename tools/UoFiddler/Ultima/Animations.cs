using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ultima
{
    /// <summary>
    /// Contains translation tables used for mapping body values to file subsets.
    /// <seealso cref="Animations" />
    /// </summary>
    public sealed class BodyConverter
    {
        public static int[] Table1 { get; private set; }
        public static int[] Table2 { get; private set; }
        public static int[] Table3 { get; private set; }
        public static int[] Table4 { get; private set; }

        private BodyConverter()
        {
        }

        static BodyConverter()
        {
            Initialize();
        }

        /// <summary>
        /// Fills bodyconv.def Tables
        /// </summary>
        public static void Initialize()
        {
            string path = Files.GetFilePath("bodyconv.def");

            if (path == null)
                return;

            ArrayList list1 = new ArrayList(), list2 = new ArrayList(), list3 = new ArrayList(), list4 = new ArrayList();
            int max1 = 0, max2 = 0, max3 = 0, max4 = 0;

            using (StreamReader ip = new StreamReader(path))
            {
                string line;

                while ((line = ip.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                        continue;

                    try
                    {
                        string[] split = line.Split('\t');

                        int original = System.Convert.ToInt32(split[0]);
                        int anim2 = System.Convert.ToInt32(split[1]);
                        int anim3;
                        int anim4;
                        int anim5;

                        try
                        {
                            anim3 = System.Convert.ToInt32(split[2]);
                        }
                        catch
                        {
                            anim3 = -1;
                        }

                        try
                        {
                            anim4 = System.Convert.ToInt32(split[3]);
                        }
                        catch
                        {
                            anim4 = -1;
                        }

                        try
                        {
                            anim5 = System.Convert.ToInt32(split[4]);
                        }
                        catch
                        {
                            anim5 = -1;
                        }

                        if (anim2 != -1)
                        {
                            if (anim2 == 68)
                                anim2 = 122;

                            if (original > max1)
                                max1 = original;

                            list1.Add(original);
                            list1.Add(anim2);
                        }

                        if (anim3 != -1)
                        {
                            if (original > max2)
                                max2 = original;

                            list2.Add(original);
                            list2.Add(anim3);
                        }

                        if (anim4 != -1)
                        {
                            if (original > max3)
                                max3 = original;

                            list3.Add(original);
                            list3.Add(anim4);
                        }

                        if (anim5 != -1)
                        {
                            if (original > max4)
                                max4 = original;

                            list4.Add(original);
                            list4.Add(anim5);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            Table1 = new int[max1 + 1];

            for (int i = 0; i < Table1.Length; ++i)
                Table1[i] = -1;

            for (int i = 0; i < list1.Count; i += 2)
                Table1[(int)list1[i]] = (int)list1[i + 1];

            Table2 = new int[max2 + 1];

            for (int i = 0; i < Table2.Length; ++i)
                Table2[i] = -1;

            for (int i = 0; i < list2.Count; i += 2)
                Table2[(int)list2[i]] = (int)list2[i + 1];

            Table3 = new int[max3 + 1];

            for (int i = 0; i < Table3.Length; ++i)
                Table3[i] = -1;

            for (int i = 0; i < list3.Count; i += 2)
                Table3[(int)list3[i]] = (int)list3[i + 1];

            Table4 = new int[max4 + 1];

            for (int i = 0; i < Table4.Length; ++i)
                Table4[i] = -1;

            for (int i = 0; i < list4.Count; i += 2)
                Table4[(int)list4[i]] = (int)list4[i + 1];
        }

        /// <summary>
        /// Checks to see if <paramref name="body" /> is contained within the mapping table.
        /// </summary>
        /// <returns>True if it is, false if not.</returns>
        public static bool Contains(int body)
        {
            if (Table1 != null && body >= 0 && body < Table1.Length && Table1[body] != -1)
                return true;

            if (Table2 != null && body >= 0 && body < Table2.Length && Table2[body] != -1)
                return true;

            if (Table3 != null && body >= 0 && body < Table3.Length && Table3[body] != -1)
                return true;

            if (Table4 != null && body >= 0 && body < Table4.Length && Table4[body] != -1)
                return true;

            return false;
        }

        /// <summary>
        /// Attempts to convert <paramref name="body" /> to a body index relative to a file subset, specified by the return value.
        /// </summary>
        /// <returns>A value indicating a file subset:
        /// <list type="table">
        /// <listheader>
        /// <term>Return Value</term>
        /// <description>File Subset</description>
        /// </listheader>
        /// <item>
        /// <term>1</term>
        /// <description>Anim.mul, Anim.idx (Standard)</description>
        /// </item>
        /// <item>
        /// <term>2</term>
        /// <description>Anim2.mul, Anim2.idx (LBR)</description>
        /// </item>
        /// <item>
        /// <term>3</term>
        /// <description>Anim3.mul, Anim3.idx (AOS)</description>
        /// </item>
        /// <item>
        /// <term>4</term>
        /// <description>Anim4.mul, Anim4.idx (SE)</description>
        /// </item>
        /// <item>
        /// <term>5</term>
        /// <description>Anim5.mul, Anim5.idx (ML)</description>
        /// </item>
        /// </list>
        /// </returns>
        public static int Convert(ref int body)
        {
            if (Table1 != null && body >= 0 && body < Table1.Length)
            {
                int val = Table1[body];

                if (val != -1)
                {
                    body = val;
                    return 2;
                }
            }

            if (Table2 != null && body >= 0 && body < Table2.Length)
            {
                int val = Table2[body];

                if (val != -1)
                {
                    body = val;
                    return 3;
                }
            }

            if (Table3 != null && body >= 0 && body < Table3.Length)
            {
                int val = Table3[body];

                if (val != -1)
                {
                    body = val;
                    return 4;
                }
            }

            if (Table4 != null && body >= 0 && body < Table4.Length)
            {
                int val = Table4[body];

                if (val != -1)
                {
                    body = val;
                    return 5;
                }
            }

            return 1;
        }

        /// <summary>
        /// Converts backward
        /// </summary>
        /// <param name="FileType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetTrueBody(int FileType, int index)
        {
            switch (FileType)
            {
                default:
                case 1:
                    return index;
                case 2:
                    if (Table1 != null && index >= 0)
                    {
                        for (int i = 0; i < Table1.Length; i++)
                        {
                            if (Table1[i] == index)
                                return i;
                        }
                    }
                    break;
                case 3:
                    if (Table2 != null && index >= 0)
                    {
                        for (int i = 0; i < Table2.Length; i++)
                        {
                            if (Table2[i] == index)
                                return i;
                        }
                    }
                    break;
                case 4:
                    if (Table3 != null && index >= 0)
                    {
                        for (int i = 0; i < Table3.Length; i++)
                        {
                            if (Table3[i] == index)
                                return i;
                        }
                    }
                    break;
                case 5:
                    if (Table4 != null && index >= 0)
                    {
                        for (int i = 0; i < Table4.Length; i++)
                        {
                            if (Table4[i] == index)
                                return i;
                        }
                    }
                    break;

            }
            return -1;
        }
    }

    public class Animations
    {
        private static FileIndex m_FileIndex = new FileIndex("Anim.idx", "Anim.mul", 0x40000, 6);
        //public static FileIndex FileIndex{ get{ return m_FileIndex; } }

        private static FileIndex m_FileIndex2 = new FileIndex("Anim2.idx", "Anim2.mul", 0x10000, -1);
        //public static FileIndex FileIndex2{ get{ return m_FileIndex2; } }

        private static FileIndex m_FileIndex3 = new FileIndex("Anim3.idx", "Anim3.mul", 0x20000, -1);
        //public static FileIndex FileIndex3{ get{ return m_FileIndex3; } }

        private static FileIndex m_FileIndex4 = new FileIndex("Anim4.idx", "Anim4.mul", 0x20000, -1);
        //public static FileIndex FileIndex4{ get{ return m_FileIndex4; } }

        private static FileIndex m_FileIndex5 = new FileIndex("Anim5.idx", "Anim5.mul", 0x20000, -1);
        //public static FileIndex FileIndex5 { get { return m_FileIndex5; } }

        /// <summary>
        /// Rereads AnimX files and bodyconv, body.def
        /// </summary>
        public static void Reload()
        {
            m_FileIndex = new FileIndex("Anim.idx", "Anim.mul", 0x40000, 6);
            m_FileIndex2 = new FileIndex("Anim2.idx", "Anim2.mul", 0x10000, -1);
            m_FileIndex3 = new FileIndex("Anim3.idx", "Anim3.mul", 0x20000, -1);
            m_FileIndex4 = new FileIndex("Anim4.idx", "Anim4.mul", 0x20000, -1);
            m_FileIndex5 = new FileIndex("Anim5.idx", "Anim5.mul", 0x20000, -1);

            BodyConverter.Initialize();
            BodyTable.Initialize();
        }

        /// <summary>
        /// Returns Framelist
        /// </summary>
        /// <param name="body"></param>
        /// <param name="action"></param>
        /// <param name="direction"></param>
        /// <param name="hue"></param>
        /// <param name="preserveHue">No Hue override <see cref="bodydev"/></param>
        /// <param name="FirstFrame"></param>
        /// <returns></returns>
        public static Frame[] GetAnimation(int body, int action, int direction, ref int hue, bool preserveHue, bool FirstFrame)
        {
            if (preserveHue)
                Translate(ref body);
            else
                Translate(ref body, ref hue);

            int fileType = BodyConverter.Convert(ref body);

            FileIndex fileIndex;
            int index;
            GetFileIndex(body, action, direction, fileType, out fileIndex, out index);

            int length, extra;
            bool patched;
            Stream stream = fileIndex.Seek(index, out length, out extra, out patched);

            if (stream == null)
                return null;

            bool flip = direction > 4;
            Frame[] frames;
            using (BinaryReader bin = new BinaryReader(stream))
            {
                ushort[] palette = new ushort[0x100];

                for (int i = 0; i < 0x100; ++i)
                    palette[i] = (ushort)(bin.ReadUInt16() ^ 0x8000);

                int start = (int)bin.BaseStream.Position;
                int frameCount = bin.ReadInt32();

                int[] lookups = new int[frameCount];

                for (int i = 0; i < frameCount; ++i)
                    lookups[i] = start + bin.ReadInt32();

                bool onlyHueGrayPixels = (hue & 0x8000) != 0;

                hue = (hue & 0x3FFF) - 1;

                Hue hueObject;

                if (hue >= 0 && hue < Hues.List.Length)
                    hueObject = Hues.List[hue];
                else
                    hueObject = null;

                if (FirstFrame)
                    frameCount = 1;
                frames = new Frame[frameCount];

                for (int i = 0; i < frameCount; ++i)
                {
                    bin.BaseStream.Seek(lookups[i], SeekOrigin.Begin);
                    frames[i] = new Frame(palette, bin, flip);

                    if (hueObject != null)
                    {
                        if (frames[i] != null)
                        {
                            if (frames[i].Bitmap != null)
                                hueObject.ApplyTo(frames[i].Bitmap, onlyHueGrayPixels);
                        }
                    }
                }
                bin.Close();
            }
            return frames;
        }

        public static Frame[] GetAnimation(int body, int action, int direction, int fileType)
        {
            FileIndex fileIndex;
            int index;
            GetFileIndex(body, action, direction, fileType, out fileIndex, out index);

            int length, extra;
            bool patched;

            Stream stream = fileIndex.Seek(index, out length, out extra, out patched);

            if (stream == null)
                return null;

            bool flip = direction > 4;

            using (BinaryReader bin = new BinaryReader(stream))
            {
                ushort[] palette = new ushort[0x100];

                for (int i = 0; i < 0x100; ++i)
                    palette[i] = (ushort)(bin.ReadUInt16() ^ 0x8000);

                int start = (int)bin.BaseStream.Position;
                int frameCount = bin.ReadInt32();

                int[] lookups = new int[frameCount];

                for (int i = 0; i < frameCount; ++i)
                    lookups[i] = start + bin.ReadInt32();

                Frame[] frames = new Frame[frameCount];

                for (int i = 0; i < frameCount; ++i)
                {
                    bin.BaseStream.Seek(lookups[i], SeekOrigin.Begin);
                    frames[i] = new Frame(palette, bin, flip);
                }

                return frames;
            }
        }

        private static int[] m_Table;

        /// <summary>
        /// Translates body (body.def)
        /// </summary>
        /// <param name="body"></param>
        public static void Translate(ref int body)
        {
            if (m_Table == null)
                LoadTable();
            if (body <= 0 || body >= m_Table.Length)
            {
                body = 0;
                return;
            }

            body = m_Table[body] & 0x7FFF;
        }

        /// <summary>
        /// Translates body and hue (body.def)
        /// </summary>
        /// <param name="body"></param>
        /// <param name="hue"></param>
        public static void Translate(ref int body, ref int hue)
        {
            if (m_Table == null)
                LoadTable();
            if (body <= 0 || body >= m_Table.Length)
            {
                body = 0;
                return;
            }

            int table = m_Table[body];

            if ((table & (1 << 31)) != 0)
            {
                body = table & 0x7FFF;

                int vhue = (hue & 0x3FFF) - 1;

                if (vhue < 0 || vhue >= Hues.List.Length)
                    hue = (table >> 15) & 0xFFFF;
            }
        }

        private static void LoadTable()
        {
            int count = 400 + ((m_FileIndex.Index.Length - 35000) / 175);

            m_Table = new int[count];

            for (int i = 0; i < count; ++i)
            {
                object o = BodyTable.m_Entries[i];

                if (o == null || BodyConverter.Contains(i))
                {
                    m_Table[i] = i;
                }
                else
                {
                    BodyTableEntry bte = (BodyTableEntry)o;

                    m_Table[i] = bte.OldID | (1 << 31) | ((bte.NewHue & 0xFFFF) << 15);
                }
            }
        }

        /// <summary>
        /// Is Body with action and direction definied
        /// </summary>
        /// <param name="body"></param>
        /// <param name="action"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static bool IsActionDefined(int body, int action, int direction)
        {
            Translate(ref body);
            int fileType = BodyConverter.Convert(ref body);
            FileIndex fileIndex;
            int index;
            GetFileIndex(body, action, direction, fileType, out fileIndex, out index);

            int length, extra;
            bool patched;
            Stream stream = fileIndex.Seek(index, out length, out extra, out patched);
            bool def = true;
            if (stream == null)
                def = false;
            else
                stream.Close();
            return def;
        }

        /// <summary>
        /// Is Animation in given animfile definied
        /// </summary>
        /// <param name="body"></param>
        /// <param name="action"></param>
        /// <param name="dir"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static bool IsAnimDefinied(int body, int action, int dir, int fileType)
        {
            FileIndex fileIndex;
            int index;
            GetFileIndex(body, action, dir, fileType, out fileIndex, out index);

            int length, extra;
            bool patched;
            Stream stream = fileIndex.Seek(index, out length, out extra, out patched);
            bool def = true;
            if ((stream == null) || (length == 0))
                def = false;
            if (stream != null)
                stream.Close();
            return def;
        }

        /// <summary>
        /// Returns Animationcount in given animfile
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static int GetAnimCount(int fileType)
        {
            int count;
            switch (fileType)
            {
                default:
                case 1:
                    count = 400 + (int)(m_FileIndex.IdxLength - 35000 * 12) / (12 * 175);
                    break;
                case 2:
                    count = 200 + (int)(m_FileIndex2.IdxLength - 22000 * 12) / (12 * 65);
                    break;
                case 3:
                    count = 400 + (int)(m_FileIndex3.IdxLength - 35000 * 12) / (12 * 175);
                    break;
                case 4:
                    count = 400 + (int)(m_FileIndex4.IdxLength - 35000 * 12) / (12 * 175);
                    break;
                case 5:
                    count = 400 + (int)(m_FileIndex5.IdxLength - 35000 * 12) / (12 * 175);
                    break;
            }
            return count;
        }

        /// <summary>
        /// Actioncount of given Body in given anim file
        /// </summary>
        /// <param name="body"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static int GetAnimLength(int body, int fileType)
        {
            int length = 0;
            switch (fileType)
            {
                default:
                case 1:
                    if (body < 200)
                        length = 22; //high
                    else if (body < 400)
                        length = 13; //low
                    else
                        length = 35; //people
                    break;
                case 2:
                    if (body < 200)
                        length = 22; //high
                    else
                        length = 13; //low
                    break;
                case 3:
                    if (body < 300)
                        length = 13;
                    else if (body < 400)
                        length = 22;
                    else
                        length = 35;
                    break;
                case 4:
                    if (body < 200)
                        length = 22;
                    else if (body < 400)
                        length = 13;
                    else
                        length = 35;
                    break;
                case 5:
                    if (body < 200)
                        length = 22;
                    else if (body < 400)
                        length = 13;
                    else
                        length = 35;
                    break;
            }
            return length;
        }

        /// <summary>
        /// Gets Fileseek index based on fileType,body,action,direction
        /// </summary>
        /// <param name="body"></param>
        /// <param name="action"></param>
        /// <param name="direction"></param>
        /// <param name="fileType">animX</param>
        /// <param name="fileIndex"></param>
        /// <param name="index"></param>
        private static void GetFileIndex(int body, int action, int direction, int fileType, out FileIndex fileIndex, out int index)
        {
            switch (fileType)
            {
                default:
                case 1:
                    fileIndex = m_FileIndex;
                    if (body < 200)
                        index = body * 110;
                    else if (body < 400)
                        index = 22000 + ((body - 200) * 65);
                    else
                        index = 35000 + ((body - 400) * 175);

                    break;
                case 2:
                    fileIndex = m_FileIndex2;
                    if (body < 200)
                        index = body * 110;
                    else
                        index = 22000 + ((body - 200) * 65);

                    break;
                case 3:
                    fileIndex = m_FileIndex3;
                    if (body < 300)
                        index = body * 65;
                    else if (body < 400)
                        index = 33000 + ((body - 300) * 110);
                    else
                        index = 35000 + ((body - 400) * 175);

                    break;
                case 4:
                    fileIndex = m_FileIndex4;
                    if (body < 200)
                        index = body * 110;
                    else if (body < 400)
                        index = 22000 + ((body - 200) * 65);
                    else
                        index = 35000 + ((body - 400) * 175);

                    break;
                case 5:
                    fileIndex = m_FileIndex5;
                    if ((body < 200) && (body != 34)) // looks strange, though it works.
                        index = body * 110;
                    else if (body < 400)
                        index = 22000 + ((body - 200) * 65);
                    else
                        index = 35000 + ((body - 400) * 175);

                    break;
            }

            index += action * 5;

            if (direction <= 4)
                index += direction;
            else
                index += direction - (direction - 4) * 2;
        }

        /// <summary>
        /// Returns Filename body is in
        /// </summary>
        /// <param name="body"></param>
        /// <returns>anim{0}.mul</returns>
        public static string GetFileName(int body)
        {
            Translate(ref body);
            int fileType = BodyConverter.Convert(ref body);

            if (fileType == 1)
                return "anim.mul";
            else
                return String.Format("anim{0}.mul", fileType);
        }

        public static void DefragAnim(string path, int index)
        {
            string name;
            if (index == 1)
                name = "anim";
            else
                name = String.Format("anim{0}", index);

            string pathmul = Files.GetFilePath(name + ".mul");
            if (pathmul == null)
                return;
            string pathidx = Files.GetFilePath(name + ".idx");
            if (pathidx == null)
                return;
            string idx = Path.Combine(path, name + ".idx");
            string mul = Path.Combine(path, name + ".mul");
            using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write),
                              fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write),
                              m_Mul = new FileStream(pathmul, FileMode.Open, FileAccess.Read, FileShare.Read),
                              m_Index = new FileStream(pathidx, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (BinaryWriter binidx = new BinaryWriter(fsidx),
                                    binmul = new BinaryWriter(fsmul))
                {
                    using (BinaryReader m_MulReader = new BinaryReader(m_Mul),
                                        m_IndexReader = new BinaryReader(m_Index))
                    {
                        while (m_Index.Length != m_Index.Position)
                        {
                            int lookup = m_IndexReader.ReadInt32();
                            int length = m_IndexReader.ReadInt32();
                            int extra = m_IndexReader.ReadInt32();
                            if (lookup < 0 || lookup >= m_Mul.Length || length == 0)
                            {
                                binidx.Write(lookup);
                                binidx.Write(length);
                                binidx.Write(extra);
                            }
                            else
                            {
                                binidx.Write((int)fsmul.Position);
                                binidx.Write(length);
                                binidx.Write(extra);
                                byte[] buffer = new byte[length];
                                m_Mul.Seek(lookup, SeekOrigin.Begin);
                                m_MulReader.Read(buffer, 0, length);
                                binmul.Write(buffer);
                            }
                        }
                    }
                }
            }
        }
    }

    public sealed class Frame
    {
        public Point Center { get; set; }
        public Bitmap Bitmap { get; set; }

        private const int DoubleXor = (0x200 << 22) | (0x200 << 12);

        public static readonly Frame Empty = new Frame();
        //public static readonly Frame[] EmptyFrames = new Frame[1] { Empty };

        private Frame()
        {
            Bitmap = new Bitmap(1, 1);
        }

        public unsafe Frame(ushort[] palette, BinaryReader bin, bool flip)
        {
            int xCenter = bin.ReadInt16();
            int yCenter = bin.ReadInt16();

            int width = bin.ReadUInt16();
            int height = bin.ReadUInt16();
            if (height == 0 || width == 0)
                return;
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;

            int header;

            int xBase = xCenter - 0x200;
            int yBase = (yCenter + height) - 0x200;

            if (!flip)
            {
                line += xBase;
                line += yBase * delta;

                while ((header = bin.ReadInt32()) != 0x7FFF7FFF)
                {
                    header ^= DoubleXor;

                    ushort* cur = line + ((((header >> 12) & 0x3FF) * delta) + ((header >> 22) & 0x3FF));
                    ushort* end = cur + (header & 0xFFF);
                    while (cur < end)
                        *cur++ = palette[bin.ReadByte()];
                }
            }
            else
            {
                line -= xBase - width + 1;
                line += yBase * delta;

                while ((header = bin.ReadInt32()) != 0x7FFF7FFF)
                {
                    header ^= DoubleXor;

                    ushort* cur = line + ((((header >> 12) & 0x3FF) * delta) - ((header >> 22) & 0x3FF));
                    ushort* end = cur - (header & 0xFFF);

                    while (cur > end)
                        *cur-- = palette[bin.ReadByte()];
                }

                xCenter = width - xCenter;
            }

            bmp.UnlockBits(bd);

            Center = new Point(xCenter, yCenter);
            Bitmap = bmp;
        }
    }

    public sealed class BodyTableEntry
    {
        public int OldID { get; set; }
        public int NewID { get; set; }
        public int NewHue { get; set; }
        public BodyTableEntry(int oldID, int newID, int newHue)
        {
            OldID = oldID;
            NewID = newID;
            NewHue = newHue;
        }
    }

    public sealed class BodyTable
    {
        public static Hashtable m_Entries;

        static BodyTable()
        {
            Initialize();
        }

        public static void Initialize()
        {
            m_Entries = new Hashtable();

            string filePath = Files.GetFilePath("body.def");

            if (filePath == null)
                return;

            using (StreamReader def = new StreamReader(filePath))
            {
                string line;

                while ((line = def.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                        continue;

                    try
                    {
                        int index1 = line.IndexOf("{");
                        int index2 = line.IndexOf("}");

                        string param1 = line.Substring(0, index1);
                        string param2 = line.Substring(index1 + 1, index2 - index1 - 1);
                        string param3 = line.Substring(index2 + 1);

                        int indexOf = param2.IndexOf(',');

                        if (indexOf > -1)
                            param2 = param2.Substring(0, indexOf).Trim();

                        int iParam1 = Convert.ToInt32(param1.Trim());
                        int iParam2 = Convert.ToInt32(param2.Trim());
                        int iParam3 = Convert.ToInt32(param3.Trim());

                        m_Entries[iParam1] = new BodyTableEntry(iParam2, iParam1, iParam3);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }

    public sealed class AnimationEdit
    {
        private static FileIndex m_FileIndex = new FileIndex("Anim.idx", "Anim.mul", 0x40000, 6);
        //public static FileIndex FileIndex{ get{ return m_FileIndex; } }

        private static FileIndex m_FileIndex2 = new FileIndex("Anim2.idx", "Anim2.mul", 0x10000, -1);
        //public static FileIndex FileIndex2{ get{ return m_FileIndex2; } }

        private static FileIndex m_FileIndex3 = new FileIndex("Anim3.idx", "Anim3.mul", 0x20000, -1);
        //public static FileIndex FileIndex3{ get{ return m_FileIndex3; } }

        private static FileIndex m_FileIndex4 = new FileIndex("Anim4.idx", "Anim4.mul", 0x20000, -1);
        //public static FileIndex FileIndex4{ get{ return m_FileIndex4; } }

        private static FileIndex m_FileIndex5 = new FileIndex("Anim5.idx", "Anim5.mul", 0x20000, -1);
        //public static FileIndex FileIndex5 { get { return m_FileIndex5; } }

        private static AnimEdit[] animcache;
        private static AnimEdit[] animcache2;
        private static AnimEdit[] animcache3;
        private static AnimEdit[] animcache4;
        private static AnimEdit[] animcache5;
        static AnimationEdit()
        {
            int count;
            count=Animations.GetAnimCount(1);
            if (count > 0)
                animcache = new AnimEdit[count];
            count = Animations.GetAnimCount(2);
            if (count > 0)
                animcache2 = new AnimEdit[count];
            count = Animations.GetAnimCount(3);
            if (count > 0)
                animcache3 = new AnimEdit[count];
            count = Animations.GetAnimCount(4);
            if (count > 0)
                animcache4 = new AnimEdit[count];
            count = Animations.GetAnimCount(5);
            if (count > 0)
                animcache5 = new AnimEdit[count];
        }
        /// <summary>
        /// Rereads AnimX files
        /// </summary>
        public static void Reload()
        {
            m_FileIndex = new FileIndex("Anim.idx", "Anim.mul", 0x40000, 6);
            m_FileIndex2 = new FileIndex("Anim2.idx", "Anim2.mul", 0x10000, -1);
            m_FileIndex3 = new FileIndex("Anim3.idx", "Anim3.mul", 0x20000, -1);
            m_FileIndex4 = new FileIndex("Anim4.idx", "Anim4.mul", 0x20000, -1);
            m_FileIndex5 = new FileIndex("Anim5.idx", "Anim5.mul", 0x20000, -1);
        }

        private static void GetFileIndex(int body, int fileType, out FileIndex fileIndex, out int index)
        {
            switch (fileType)
            {
                default:
                case 1:
                    fileIndex = m_FileIndex;
                    if (body < 200)
                        index = body * 110;
                    else if (body < 400)
                        index = 22000 + ((body - 200) * 65);
                    else
                        index = 35000 + ((body - 400) * 175);

                    break;
                case 2:
                    fileIndex = m_FileIndex2;
                    if (body < 200)
                        index = body * 110;
                    else
                        index = 22000 + ((body - 200) * 65);

                    break;
                case 3:
                    fileIndex = m_FileIndex3;
                    if (body < 300)
                        index = body * 65;
                    else if (body < 400)
                        index = 33000 + ((body - 300) * 110);
                    else
                        index = 35000 + ((body - 400) * 175);

                    break;
                case 4:
                    fileIndex = m_FileIndex4;
                    if (body < 200)
                        index = body * 110;
                    else if (body < 400)
                        index = 22000 + ((body - 200) * 65);
                    else
                        index = 35000 + ((body - 400) * 175);

                    break;
                case 5:
                    fileIndex = m_FileIndex5;
                    if ((body < 200) && (body != 34)) // looks strange, though it works.
                        index = body * 110;
                    else if (body < 400)
                        index = 22000 + ((body - 200) * 65);
                    else
                        index = 35000 + ((body - 400) * 175);

                    break;
            }
        }

        public static AnimEdit GetAnimation(int filetype, int body)
        {
            AnimEdit[] cache;
            switch (filetype)
            {
                case 1:
                    cache = animcache;
                    break;
                case 2:
                    cache = animcache2;
                    break;
                case 3:
                    cache = animcache3;
                    break;
                case 4:
                    cache = animcache4;
                    break;
                case 5:
                    cache = animcache5;
                    break;
                default:
                    cache = animcache;
                    break;
            }
            if (cache != null)
            {
                if (cache[body]!=null)
                    return cache[body];
            }
            FileIndex fileIndex;
            int index;
            GetFileIndex(body, filetype, out fileIndex, out index);

            int length, extra;
            bool patched;

            Stream stream = fileIndex.Seek(index, out length, out extra, out patched);

            if ((stream == null) || (length==0))
                return null;
            stream.Close();
            return cache[body]=new AnimEdit(body, fileIndex, filetype, index);
        }

        public static bool IsAnimDefinied(int filetype, int body)
        {
            FileIndex fileIndex;
            int index;
            GetFileIndex(body, filetype, out fileIndex, out index);

            int length, extra;
            bool patched;

            Stream stream = fileIndex.Seek(index, out length, out extra, out patched);

            if ((stream == null) || (length == 0))
                return false;
            stream.Close();
            return true;
        }

        public static bool IsActionDefinied(int filetype, int body, int action)
        {
            FileIndex fileIndex;
            int index;
            GetFileIndex(body, filetype, out fileIndex, out index);

            int length, extra;
            bool patched;

            Stream stream = fileIndex.Seek(index, out length, out extra, out patched);

            if ((stream == null) || (length == 0))
                return false;
            stream.Close();

            int AnimCount = Animations.GetAnimLength(body, filetype);
            if (AnimCount < action)
                return false;

            stream = fileIndex.Seek(index+action*5, out length, out extra, out patched);
            if ((stream == null) || (length == 0))
                return false;
            stream.Close();
            return true;
        }

        public static void Save(int filetype, string path)
        {
            string filename;
            switch (filetype)
            {
                case 1: filename = "anim"; break;
                case 2: filename = "anim2"; break;
                case 3: filename = "anim3"; break;
                case 4: filename = "anim4"; break;
                case 5: filename = "anim5"; break;
                default: filename = "anim"; break;
            }
            string idx = Path.Combine(path, filename + ".idx");
            string mul = Path.Combine(path, filename + ".mul");
            using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write),
                              fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (BinaryWriter binidx = new BinaryWriter(fsidx),
                                    binmul = new BinaryWriter(fsmul))
                {
                    int count=Animations.GetAnimCount(filetype);
                    for (int body = 0; body < count; body++)
                    {
                        AnimEdit edit = GetAnimation(filetype, body);
                        if (edit == null)
                        {
                            for (int i = 0; i < Ultima.Animations.GetAnimLength(body, filetype); i++)
                            {
                                for (int d = 0; d < 5; d++)
                                {
                                    binidx.Write((int)-1);
                                    binidx.Write((int)-1);
                                    binidx.Write((int)-1);
                                }
                            }
                        }
                        else
                            edit.Save(binmul, binidx);
                    }
                    
                }
            }
        }
    }

    public sealed class AnimEdit
    {
        public struct Actions
        {
            public FramesEdit[] Directions;
        }
        public int Body { get; private set; }
        public Actions[] Action;
        public unsafe AnimEdit(int body, FileIndex fileIndex, int filetype, int index)
        {
            Body = body;
            int AnimCount = Ultima.Animations.GetAnimLength(body, filetype);
            Action = new Actions[AnimCount];
            for (int i = 0; i < AnimCount; i++)
            {
                int length, extra;
                bool patched;
                Action[i].Directions = new FramesEdit[5];
                for (int d = 0; d < 5; d++)
                {
                    Stream stream = fileIndex.Seek(index, out length, out extra, out patched);
                    index++;
                    if ((stream == null) || (length < 1))
                        continue;
                    using (BinaryReader bin = new BinaryReader(stream))
                    {
                        Action[i].Directions[d] = new FramesEdit(bin);
                        Action[i].Directions[d].extra = extra;
                    }
                    stream.Close();
                }
            }
        }

        public void Save(BinaryWriter bin, BinaryWriter idx)
        {
            for (int i = 0; i < Action.Length; i++)
            {
                for (int d = 0; d < 5; d++)
                {
                    if (Action[i].Directions[d] == null)
                    {
                        idx.Write((int)-1);
                        idx.Write((int)-1);
                        idx.Write((int)-1);
                    }
                    else
                    {
                        int start = (int)bin.BaseStream.Position;
                        idx.Write(start);
                        Action[i].Directions[d].Save(bin);
                        start = (int)bin.BaseStream.Position - start;
                        idx.Write((int)start);
                        idx.Write((int)Action[i].Directions[d].extra);
                    }
                }
            }
        }

        public unsafe Bitmap[] GetFrames(int action, int direction)
        {
            FramesEdit frames = Action[action].Directions[direction];
            if ((frames == null) || (frames.Frames.Length == 0))
                return null;
            Bitmap[] bits = new Bitmap[frames.Frames.Length];
            for (int i = 0; i < bits.Length; i++)
            {
                int width = frames.Frames[i].width;
                int height = frames.Frames[i].height;
                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
                BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
                ushort* line = (ushort*)bd.Scan0;
                int delta = bd.Stride >> 1;

                int xBase = frames.Frames[i].Center.X - 0x200;
                int yBase = (frames.Frames[i].Center.Y + height) - 0x200;

                line += xBase;
                line += yBase * delta;
                for (int j = 0; j < frames.Frames[i].RawData.Length; j++)
                {
                    FrameEdit.Raw raw = frames.Frames[i].RawData[j];
                    
                    ushort* cur = line + (((raw.offy) * delta) + ((raw.offx) & 0x3FF));
                    ushort* end = cur + (raw.run);

                    int ii=0;
                    while (cur < end)
                    {
                        *cur++ = frames.Palette[raw.data[ii]];
                        ii++;
                    }
                }
                bmp.UnlockBits(bd);
                bits[i] = bmp;
            }
            return bits;
        }
    }

    public sealed class FramesEdit
    {
        public ushort[] Palette = new ushort[0x100];
        public FrameEdit[] Frames;
        public int extra;

        public FramesEdit(BinaryReader bin)
        {
            for (int i = 0; i < 0x100; ++i)
                Palette[i] = (ushort)(bin.ReadUInt16() ^ 0x8000);

            int start = (int)bin.BaseStream.Position;
            int frameCount = bin.ReadInt32();

            int[] lookups = new int[frameCount];

            for (int i = 0; i < frameCount; ++i)
                lookups[i] = start + bin.ReadInt32();

            Frames = new FrameEdit[frameCount];

            for (int i = 0; i < frameCount; ++i)
            {
                bin.BaseStream.Seek(lookups[i], SeekOrigin.Begin);
                Frames[i] = new FrameEdit(bin);
            }
        }

        public void Save(BinaryWriter bin)
        {
            for (int i = 0; i < 0x100; ++i)
                bin.Write((ushort)(Palette[i] ^ 0x8000));
            int start = (int)bin.BaseStream.Position;
            bin.Write((int)Frames.Length);
            int seek = (int)bin.BaseStream.Position;
            int curr = (int)bin.BaseStream.Position + 4 * Frames.Length;
            for (int i = 0; i < Frames.Length; i++)
            {
                bin.BaseStream.Seek(seek, SeekOrigin.Begin);
                bin.Write((int)(curr - start));
                seek = (int)bin.BaseStream.Position;
                bin.BaseStream.Seek(curr, SeekOrigin.Begin);
                Frames[i].Save(bin);
                curr = (int)bin.BaseStream.Position;
            }
        }
    }

    public sealed class FrameEdit
    {
        private const int DoubleXor = (0x200 << 22) | (0x200 << 12);
        public struct Raw
        {
            public int run;
            public int offx;
            public int offy;
            public byte[] data;
        }
        public Raw[] RawData { get; private set; }
        public Point Center { get; set; }
        public int width;
        public int height;

        public unsafe FrameEdit(BinaryReader bin)
        {
            int xCenter = bin.ReadInt16();
            int yCenter = bin.ReadInt16();

            width = bin.ReadUInt16();
            height = bin.ReadUInt16();
            if (height == 0 || width == 0)
                return;
            int header;

            ArrayList tmp = new ArrayList();
            while ((header = bin.ReadInt32()) != 0x7FFF7FFF)
            {
                Raw raw = new Raw();
                header ^= DoubleXor;
                raw.run = (header & 0xFFF);
                raw.offy = ((header >> 12) & 0x3FF);
                raw.offx = ((header >> 22) & 0x3FF);

                int i = 0;
                raw.data = new byte[raw.run];
                while (i<raw.run)
                {
                    raw.data[i] = bin.ReadByte();
                    i++;
                }
                tmp.Add(raw);
            }
            RawData = new Raw[tmp.Count];
            int j = 0;
            foreach (Raw c in tmp)
            {
                RawData[j] = c;
                j++;
            }
            Center = new Point(xCenter, yCenter);
        }

        public void Save(BinaryWriter bin)
        {
            bin.Write((short)Center.X);
            bin.Write((short)Center.Y);
            bin.Write((ushort)width);
            bin.Write((ushort)height);
            //if (height == 0 || width == 0)
            //    return;
            if (RawData != null)
            {
                for (int j = 0; j < RawData.Length; j++)
                {
                    int newHeader = RawData[j].run | (RawData[j].offy << 12) | (RawData[j].offx << 22);
                    newHeader ^= DoubleXor;
                    bin.Write((int)newHeader);
                    foreach (byte b in RawData[j].data)
                        bin.Write((byte)b);
                }
            }
            bin.Write((int)0x7FFF7FFF);
        }
    }
}