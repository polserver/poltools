using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Ultima
{
    public sealed class UnicodeFonts
    {
        public static Bitmap GetCharImage(int font, int c)
        {
            string[] files = new string[]
            {
                "unifont.mul",
                "unifont1.mul",
                "unifont2.mul",
                "unifont3.mul",
                "unifont4.mul",
                "unifont5.mul",
                "unifont6.mul"
            };
            Bitmap charImage;

            string filePath = Client.GetFilePath(files[font]);
            if (filePath != null)
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryReader bin = new BinaryReader(fs);
                    fs.Seek((long)((c + 32) * 4), SeekOrigin.Begin);
                    int num2 = bin.ReadInt32();
                    fs.Seek((long)num2, SeekOrigin.Begin);
                    int xOffset = bin.ReadByte();
                    int yOffset = bin.ReadByte();
                    int Width = bin.ReadByte();
                    int Height = bin.ReadByte();
                    if ((Width == 0) || (Height == 0))
                    {
                        charImage = new Bitmap(3, 1, PixelFormat.Format32bppArgb);
                    }
                    else
                    {
                        Color col1 = System.Drawing.Color.FromArgb(255, 0, 0, 0);
                        charImage = new Bitmap(Width + 2, Height + 2, PixelFormat.Format32bppArgb);
                        for (int y = 0; y <= Height - 1; y++)
                        {
                            for (int x = 0; x <= ((Width - 1) / 8); x++)
                            {
                                byte P = bin.ReadByte();
                                for (int j = 0; j <= 7; j++)
                                {
                                    if (x * 8 + j < Width)
                                    {
                                        if ((P & (1 << (7 - j))) == (1 << (7 - j)))
                                            charImage.SetPixel(x * 8 + j + 1, y + 1, col1);

                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
                charImage = new Bitmap(3, 1, PixelFormat.Format32bppArgb);

            return charImage;
        }
    }
}
