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
        public unsafe static Bitmap GetMultiMap()
        {
            string path = Files.GetFilePath("Multimap.rle");
            if (path != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryReader bin = new BinaryReader(fs);
                    int width, height;
                    byte pixel;
                    int count;
                    int x,y,i;
                    x=y=0;
                    Color c = new Color();
                    width = bin.ReadInt32();
                    height = bin.ReadInt32();
                    Bitmap multimap = new Bitmap(width,height,PixelFormat.Format32bppArgb);
                    while (bin.BaseStream.Length != bin.BaseStream.Position)
                    {
                        pixel = bin.ReadByte();
                        count = (pixel & 0x7f);
                        if ((pixel & 0x80) != 0)
                            c = Color.Black;
                        else
                            c = Color.White;
                        for (i = 0; i < count; i++)
                        {
                            multimap.SetPixel(x, y, c);
                            ++x;
                            if (x >= width)
                            {
                                ++y;
                                x = 0;
                            }
                        }
                    }
                    return multimap;
                }
            }
            return null;
        }

        /// <summary>
        /// Saves Bitmap to rle Format
        /// </summary>
        /// <param name="image"></param>
        /// <param name="bin"></param>
        public unsafe static void SaveMultiMap(Bitmap image, BinaryWriter bin)
        {
            bin.Write(2560); // width
            bin.Write(2048); // height
            int maxloc = 2560 * 2048;
            int loc = 0;
            byte data, mask;
            Color curcolor;
            while (loc < maxloc)
            {
                curcolor = image.GetPixel((loc % 2560), (loc / 2560));
                if (curcolor.Name != "ffffffff")
                    mask = 0x80;
                else
                    mask = 0x00;
                data = 1;
                int temp = loc + 1;
                if (temp < maxloc)
                {
                    while ((image.GetPixel(temp % 2560, temp / 2560) == curcolor) && (data < 0x7F))
                    {
                        ++data;
                        ++loc;
                        ++temp;
                        if (temp >= maxloc)
                            break;
                    }
                }
                data |= mask;
                bin.Write(data);
                ++loc;
            }
        }
    }
}
