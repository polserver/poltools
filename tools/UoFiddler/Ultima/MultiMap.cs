using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ultima
{
    public sealed class MultiMap
    {
        static MultiMap()
        {
        }

        /// <summary>
        /// Returns Bitmap
        /// </summary>
        /// <returns></returns>
        public static unsafe Bitmap GetMultiMap()
        {
            string path = Files.GetFilePath("Multimap.rle");
            if (path != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BinaryReader bin = new BinaryReader(fs))
                    {
                        int width, height;
                        byte pixel;
                        int count;
                        int x, i;
                        x = 0;
                        ushort c = 0;
                        width = bin.ReadInt32();
                        height = bin.ReadInt32();
                        Bitmap multimap = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
                        BitmapData bd = multimap.LockBits(new Rectangle(0, 0, multimap.Width, multimap.Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
                        ushort* line = (ushort*)bd.Scan0;
                        int delta = bd.Stride >> 1;

                        ushort* cur = line;
                        while (bin.BaseStream.Length != bin.BaseStream.Position)
                        {
                            pixel = bin.ReadByte();
                            count = (pixel & 0x7f);

                            if ((pixel & 0x80) != 0)
                                c = 0x8000;//Color.Black;
                            else
                                c = 0xffff;//Color.White;
                            for (i = 0; i < count; i++)
                            {
                                cur[x] = c;
                                ++x;
                                if (x >= width)
                                {
                                    cur += delta;
                                    x = 0;
                                }
                            }
                        }
                        multimap.UnlockBits(bd);
                        return multimap;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Saves Bitmap to rle Format
        /// </summary>
        /// <param name="image"></param>
        /// <param name="bin"></param>
        public static unsafe void SaveMultiMap(Bitmap image, BinaryWriter bin)
        {
            bin.Write(2560); // width
            bin.Write(2048); // height
            byte data = 1;
            byte mask = 0x0;
            ushort curcolor = 0;
            BitmapData bd = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;
            ushort* cur = line;
            curcolor = cur[0]; //init
            for (int y = 0; y < image.Height; y++, line += delta)
            {
                cur = line;
                for (int x = 0; x < image.Width; x++)
                {
                    ushort c = cur[x];

                    if (c == curcolor)
                    {
                        data++;
                        if (data == 0x7f)
                        {
                            if (curcolor == 0xffff)
                                mask = 0x0;
                            else
                                mask = 0x80;
                            data |= mask;
                            bin.Write(data);
                            data = 1;
                        }
                    }
                    else
                    {
                        if (curcolor == 0xffff)
                            mask = 0x0;
                        else
                            mask = 0x80;
                        data |= mask;
                        bin.Write(data);
                        curcolor = c;
                        data = 1;
                    }
                }
            }
            if (curcolor == 0xffff)
                mask = 0x0;
            else
                mask = 0x80;
            data |= mask;
            bin.Write(data);
            image.UnlockBits(bd);
        }
    }
}
