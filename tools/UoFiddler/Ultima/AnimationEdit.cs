using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ultima
{
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

        private static AnimIdx[] animcache;
        private static AnimIdx[] animcache2;
        private static AnimIdx[] animcache3;
        private static AnimIdx[] animcache4;
        private static AnimIdx[] animcache5;
        static AnimationEdit()
        {
            if (m_FileIndex.IdxLength>0)
                animcache = new AnimIdx[m_FileIndex.IdxLength / 12];
            if (m_FileIndex2.IdxLength > 0)
                animcache2 = new AnimIdx[m_FileIndex2.IdxLength / 12];
            if (m_FileIndex3.IdxLength > 0)
                animcache3 = new AnimIdx[m_FileIndex3.IdxLength / 12];
            if (m_FileIndex4.IdxLength > 0)
                animcache4 = new AnimIdx[m_FileIndex4.IdxLength / 12];
            if (m_FileIndex5.IdxLength > 0)
                animcache5 = new AnimIdx[m_FileIndex5.IdxLength / 12];
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
            if (m_FileIndex.IdxLength > 0)
                animcache = new AnimIdx[m_FileIndex.IdxLength / 12];
            if (m_FileIndex2.IdxLength > 0)
                animcache = new AnimIdx[m_FileIndex2.IdxLength / 12];
            if (m_FileIndex3.IdxLength > 0)
                animcache = new AnimIdx[m_FileIndex3.IdxLength / 12];
            if (m_FileIndex4.IdxLength > 0)
                animcache = new AnimIdx[m_FileIndex4.IdxLength / 12];
            if (m_FileIndex5.IdxLength > 0)
                animcache = new AnimIdx[m_FileIndex5.IdxLength / 12];
        }

        private static void GetFileIndex(int body, int fileType, int action, int direction, out FileIndex fileIndex, out int index)
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

        public static AnimIdx GetAnimation(int filetype, int body, int action, int dir)
        {
            AnimIdx[] cache;
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

            FileIndex fileIndex;
            int index;
            GetFileIndex(body, filetype, action, dir, out fileIndex, out index);

            if (cache != null)
            {
                if (cache[index] != null)
                    return cache[index];
            }
            return cache[index] = new AnimIdx(index, fileIndex, filetype);
        }

        public static bool IsActionDefinied(int filetype, int body, int action)
        {
            AnimIdx[] cache;
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

            FileIndex fileIndex;
            int index;
            GetFileIndex(body, filetype, action, 0, out fileIndex, out index);

            if (cache != null)
            {
                if (cache[index] != null)
                    return true;
            }

            int AnimCount = Animations.GetAnimLength(body, filetype);
            if (AnimCount < action)
                return false;

            int length, extra;
            bool patched;
            Stream stream = fileIndex.Seek(index, out length, out extra, out patched);
            if ((stream == null) || (length < 1))
                return false;

            stream.Close();
            return true;
        }

        public static void Save(int filetype, string path)
        {
            string filename;
            AnimIdx[] cache;
            FileIndex fileindex;
            switch (filetype)
            {
                case 1: filename = "anim"; cache = animcache; fileindex = m_FileIndex; break;
                case 2: filename = "anim2"; cache = animcache2; fileindex = m_FileIndex2; break;
                case 3: filename = "anim3"; cache = animcache3; fileindex = m_FileIndex3; break;
                case 4: filename = "anim4"; cache = animcache4; fileindex = m_FileIndex4; break;
                case 5: filename = "anim5"; cache = animcache5; fileindex = m_FileIndex5; break;
                default: filename = "anim"; cache = animcache; fileindex = m_FileIndex; break;
            }
            string idx = Path.Combine(path, filename + ".idx");
            string mul = Path.Combine(path, filename + ".mul");
            using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write),
                              fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (BinaryWriter binidx = new BinaryWriter(fsidx),
                                    binmul = new BinaryWriter(fsmul))
                {
                    for (int idxc = 0; idxc < fileindex.IdxLength / 12; idxc++)
                    {
                        AnimIdx anim;
                        if (cache != null)
                        {
                            if (cache[idxc] != null)
                                anim = cache[idxc];
                            else
                                anim = cache[idxc] = new AnimIdx(idxc, fileindex, filetype);
                        }
                        else
                            anim = cache[idxc] = new AnimIdx(idxc, fileindex, filetype);

                        if (anim == null)
                        {
                            binidx.Write((int)-1);
                            binidx.Write((int)-1);
                            binidx.Write((int)-1);
                        }
                        else
                            anim.Save(binmul, binidx);
                    }
                }
            }
        }
    }

    public sealed class AnimIdx
    {
        public int idxextra;
        public ushort[] Palette { get; private set; }
        public ArrayList Frames { get; private set; }

        public unsafe AnimIdx(int index, FileIndex fileIndex, int filetype)
        {
            Palette = new ushort[0x100];
            int length, extra;
            bool patched;
            Stream stream = fileIndex.Seek(index, out length, out extra, out patched);
            if ((stream == null) || (length < 1))
                return;

            idxextra = extra;
            using (BinaryReader bin = new BinaryReader(stream))
            {
                for (int i = 0; i < 0x100; ++i)
                    Palette[i] = (ushort)(bin.ReadUInt16() ^ 0x8000);

                int start = (int)bin.BaseStream.Position;
                int frameCount = bin.ReadInt32();

                int[] lookups = new int[frameCount];

                for (int i = 0; i < frameCount; ++i)
                    lookups[i] = start + bin.ReadInt32();

                Frames = new ArrayList();

                for (int i = 0; i < frameCount; ++i)
                {
                    stream.Seek(lookups[i], SeekOrigin.Begin);
                    Frames.Add(new FrameEdit(bin));
                }
            }
            stream.Close();
        }

        public unsafe Bitmap[] GetFrames()
        {
            if ((Frames == null) || (Frames.Count == 0))
                return null;
            Bitmap[] bits = new Bitmap[Frames.Count];
            for (int i = 0; i < bits.Length; i++)
            {
                FrameEdit frame = (FrameEdit)Frames[i];
                int width = frame.width;
                int height = frame.height;
                if (height == 0 || width == 0)
                    continue;
                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
                BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
                ushort* line = (ushort*)bd.Scan0;
                int delta = bd.Stride >> 1;

                int xBase = frame.Center.X - 0x200;
                int yBase = frame.Center.Y + height - 0x200;

                line += xBase;
                line += yBase * delta;
                for (int j = 0; j < frame.RawData.Length; j++)
                {
                    FrameEdit.Raw raw = frame.RawData[j];

                    ushort* cur = line + (((raw.offy) * delta) + ((raw.offx) & 0x3FF));
                    ushort* end = cur + (raw.run);

                    int ii = 0;
                    while (cur < end)
                    {
                        *cur++ = Palette[raw.data[ii]];
                        ii++;
                    }
                }
                bmp.UnlockBits(bd);
                bits[i] = bmp;
            }
            return bits;
        }

        public void AddFrame(Bitmap bit)
        {
            if (Frames == null)
                Frames = new ArrayList();
            Frames.Add(new FrameEdit(bit, Palette, 0, 0));
        }

        public void ReplaceFrame(Bitmap bit, int index)
        {
            if ((Frames == null) || (Frames.Count == 0))
                return;
            if (index > Frames.Count)
                return;
            Frames[index] = new FrameEdit(bit, Palette, ((FrameEdit)Frames[index]).Center.X, ((FrameEdit)Frames[index]).Center.Y);
        }

        public void RemoveFrame(int index)
        {
            if (Frames == null)
                return;
            if (index > Frames.Count)
                return;
            Frames.RemoveAt(index);
        }

        public void ClearFrames()
        {
            if (Frames == null)
                return;
            Frames.Clear();
        }

        public unsafe void ExportPalette(string filename,int type)
        {
            switch (type)
            {
                case 0:
                    using (StreamWriter Tex = new StreamWriter(new FileStream(filename, FileMode.Create, FileAccess.ReadWrite)))
                    {
                        for (int i = 0; i < 0x100; i++)
                        {
                            Tex.WriteLine(Palette[i]);
                        }
                    }
                    break;
                case 1:
                    {
                        Bitmap bmp = new Bitmap(0x100, 20, PixelFormat.Format16bppArgb1555);
                        BitmapData bd = bmp.LockBits(new Rectangle(0, 0, 0x100, 20), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
                        ushort* line = (ushort*)bd.Scan0;
                        int delta = bd.Stride >> 1;
                        for (int y = 0; y < bd.Height; ++y, line += delta)
                        {
                            ushort* cur = line;
                            for (int i = 0; i < 0x100; i++)
                            {
                                *cur++ = Palette[i];
                            }
                        }
                        bmp.UnlockBits(bd);
                        Bitmap b = new Bitmap(bmp);
                        b.Save(filename, ImageFormat.Bmp);
                        b.Dispose();
                        bmp.Dispose();
                        break;
                    }
                case 2:
                    {
                        Bitmap bmp = new Bitmap(0x100, 20, PixelFormat.Format16bppArgb1555);
                        BitmapData bd = bmp.LockBits(new Rectangle(0, 0, 0x100, 20), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
                        ushort* line = (ushort*)bd.Scan0;
                        int delta = bd.Stride >> 1;
                        for (int y = 0; y < bd.Height; ++y, line += delta)
                        {
                            ushort* cur = line;
                            for (int i = 0; i < 0x100; i++)
                            {
                                *cur++ = Palette[i];
                            }
                        }
                        bmp.UnlockBits(bd);
                        Bitmap b = new Bitmap(bmp);
                        b.Save(filename, ImageFormat.Tiff);
                        b.Dispose();
                        bmp.Dispose();
                        break;
                    }
            }
        }

        public void ReplacePalette(ushort[] palette)
        {
            Palette = palette;
        }

        public void Save(BinaryWriter bin, BinaryWriter idx)
        {
            if ((Frames == null) || (Frames.Count==0))
            {
                idx.Write((int)-1);
                idx.Write((int)-1);
                idx.Write((int)-1);
                return;
            }
            int start = (int)bin.BaseStream.Position;
            idx.Write(start);

            for (int i = 0; i < 0x100; ++i)
                bin.Write((ushort)(Palette[i] ^ 0x8000));
            int startpos = (int)bin.BaseStream.Position;
            bin.Write((int)Frames.Count);
            int seek = (int)bin.BaseStream.Position;
            int curr = (int)bin.BaseStream.Position + 4 * Frames.Count;
            foreach (FrameEdit frame in Frames)
            {
                bin.BaseStream.Seek(seek, SeekOrigin.Begin);
                bin.Write((int)(curr - startpos));
                seek = (int)bin.BaseStream.Position;
                bin.BaseStream.Seek(curr, SeekOrigin.Begin);
                frame.Save(bin);
                curr = (int)bin.BaseStream.Position;
            }

            start = (int)bin.BaseStream.Position - start;
            idx.Write((int)start);
            idx.Write((int)idxextra);
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
                while (i < raw.run)
                {
                    raw.data[i] = bin.ReadByte();
                    i++;
                }
                tmp.Add(raw);
            }
            RawData = (Raw[])tmp.ToArray(typeof(Raw));
            Center = new Point(xCenter, yCenter);
        }

        public unsafe FrameEdit(Bitmap bit, ushort[] palette, int centerx, int centery)
        {
            Center = new Point(centerx, centery);
            width = bit.Width;
            height = bit.Height;
            BitmapData bd = bit.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;
            ArrayList tmp = new ArrayList();

            int X = 0;
            for (int Y = 0; Y < bit.Height; ++Y, line += delta)
            {
                ushort* cur = line;
                int i = 0;
                int j = 0;
                X = 0;
                while (i < bit.Width)
                {
                    i = X;
                    for (i = X; i <= bit.Width; ++i)
                    {
                        //first pixel set
                        if (i < bit.Width)
                        {
                            if (cur[i] != 0)
                                break;
                        }
                    }
                    if (i < bit.Width)
                    {
                        for (j = (i + 1); j < bit.Width; ++j)
                        {
                            //next non set pixel
                            if (cur[j] == 0)
                                break;
                        }
                        Raw raw = new Raw();
                        raw.run = j - i;
                        raw.offx = j - raw.run - centerx;
                        raw.offx += 512;
                        raw.offy = Y - centery - bit.Height;
                        raw.offy += 512;
                        
                        int r = 0;
                        raw.data = new byte[raw.run];
                        while (r < raw.run)
                        {
                            ushort col = (ushort)(cur[r + i]);
                            raw.data[r] = GetPaletteIndex(palette,col);
                            ++r;
                        }
                        tmp.Add(raw);
                        X = j+1;
                        i = X;
                    }
                }
            }

            RawData = (Raw[])tmp.ToArray(typeof(Raw));
            bit.UnlockBits(bd);
        }

        public void ChangeCenter(int x, int y)
        {
            for (int i = 0; i < RawData.Length; i++)
            {
                RawData[i].offx += Center.X;
                RawData[i].offx -= x;
                RawData[i].offy += Center.Y;
                RawData[i].offy -= y;
            }
            Center = new Point(x, y);
        }

        private byte GetPaletteIndex(ushort[] palette, ushort col)
        {
            for (int i = 0; i < palette.Length; i++)
            {
                if (palette[i] == col)
                    return (byte)i;
            }
            return (byte)0;
        }

        public void Save(BinaryWriter bin)
        {
            bin.Write((short)Center.X);
            bin.Write((short)Center.Y);
            bin.Write((ushort)width);
            bin.Write((ushort)height);
            if (RawData != null)
            {
                for (int j = 0; j < RawData.Length; j++)
                {
                    int newHeader = RawData[j].run | (RawData[j].offy << 12) | (RawData[j].offx << 22);
                    newHeader ^= DoubleXor;
                    bin.Write((int)newHeader);
                    foreach (byte b in RawData[j].data)
                        bin.Write(b);
                }
            }
            bin.Write((int)0x7FFF7FFF);
        }
    }
}