using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Ultima
{
    public class UnicodeFonts
    {
        private static BinaryReader reader;
        private static FileStream stream;
        private static BinaryReader[] readerArray = new BinaryReader[3];
        private static FileStream[] streamArray = new FileStream[3];
        private static CharInfo[] uniCharCache = new CharInfo[0x3e9];

        public static Bitmap GetCharImage(int font, char c)
        {
            Bitmap charImage;

            if (stream == null)
            {
                Init();
            }

            stream = streamArray[font];
            reader = readerArray[font];

            int index = c;
            stream.Seek((long)(index * 4), SeekOrigin.Begin);
            int num2 = reader.ReadInt32();
            stream.Seek((long)num2, SeekOrigin.Begin);
            CharInfo info = new CharInfo();
            uniCharCache[index] = info;
            info.xOffset = reader.ReadByte();
            info.yOffset = reader.ReadByte();
            info.Width = reader.ReadByte();
            info.Height = reader.ReadByte();
            //int num3 = info.Height - info.BaseLine;
            info.xOffset = 0; //ignore only one character
            info.yOffset = 0;
            
            if ((info.Width + info.Height) != 0)
            {
                //charImage = new Bitmap((info.Width + (info.xOffset * 2)) + 2, (info.Height + info.yOffset) + 2, PixelFormat.Format32bppArgb);
                charImage = new Bitmap(30 , 30, PixelFormat.Format32bppArgb);
                Graphics graphics = Graphics.FromImage(charImage);
                graphics.Clear(Color.Red);
                graphics.Dispose();
                int height = info.Height - 1;
                byte[] scanline = new byte[32];
                bool drawpixel = false;
                
                    for (int i = 0; i <= height; i++)
                    {
                        int width = (info.Width + 7) / 8;//((info.Width - 1)/8);
                        scanline = reader.ReadBytes(width);

                        int pslpadding = 0;
                        // ((xWidth - 1) / 8) + 1 bytes
                        int psloffset = 0;
                        for (int k = 0; k < scanline.Length; k++)
                        {
                            //Bits are loaded high to low; bit 7 (mask: 0x80) would be the first pixel.
                            //If the bit value is 0, color is transparent, else, color is forecolor.
                            //byte num6 = 0;
                            //int num5 = k % 8;
                            //if (num5 == 0)
                            //{
                            //byte num6 = reader.ReadByte();
                            //}
                            //if ((((byte)(num6 >> ((7 - num5) & 7))) & 1) == 1)
                            //if (num6!=(byte)0)
                            drawpixel = (scanline[k] & (0x80 >> pslpadding++)) != 0;
                            if (drawpixel)//((scanline[k] & (0x80 >> pslpadding++)) != 0)
                            {
                                charImage.SetPixel((k + info.xOffset) + 1, (i + info.yOffset) + 1, Color.LightGray);
                                charImage.SetPixel((k + info.xOffset) + 2, (i + info.yOffset) + 1, Color.LightGray);
                                charImage.SetPixel((k + info.xOffset) + 1, (i + info.yOffset) + 2, Color.LightGray);
                                charImage.SetPixel((k + info.xOffset) +2, (i + info.yOffset) + 2, Color.LightGray);
                            }
                        }
                    }

                int num11 = charImage.Width - 1;
                for (int j = 0; j <= num11; j++)
                {
                    int num10 = charImage.Height - 1;
                    for (int m = 0; m <= num10; m++)
                    {
                        if (charImage.GetPixel(j, m).ToArgb() == -65536)
                        {
                            bool flag = false;
                            if ((j < (charImage.Width - 1)) && (charImage.GetPixel(j + 1, m).ToArgb() == -2894893))
                            {
                                charImage.SetPixel(j, m, Color.Black);
                                flag = true;
                            }
                            if ((!flag && (j > 0)) && (charImage.GetPixel(j - 1, m).ToArgb() == -2894893))
                            {
                                charImage.SetPixel(j, m, Color.Black);
                                flag = true;
                            }
                            if ((!flag && (m < (charImage.Height - 1))) && (charImage.GetPixel(j, m + 1).ToArgb() == -2894893))
                            {
                                charImage.SetPixel(j, m, Color.Black);
                                flag = true;
                            }
                            if ((!flag && (m > 0)) && (charImage.GetPixel(j, m - 1).ToArgb() == -2894893))
                            {
                                charImage.SetPixel(j, m, Color.Black);
                            }
                        }
                    }
                }
            }
            else
            {
                charImage = new Bitmap(3, 1, PixelFormat.Format32bppArgb);
                charImage.MakeTransparent();
            }
            charImage.MakeTransparent(Color.Red);
            info.Cache = charImage;
            return charImage;
        }

        public static Bitmap GetStringImage(int Font, string Text)
        {
            int height = 0;
            int num2 = 0;
            Bitmap[] bitmapArray = new Bitmap[(Text.Length - 1) + 1];
            int num9 = Text.Length;
            for (int i = 0; i < num9; i++)
            {
                bitmapArray[i] = GetCharImage(Font, Text[i]);
                num2 += bitmapArray[i].Width;
                if (bitmapArray[i].Height > height)
                {
                    height = bitmapArray[i].Height;
                }
            }
            Bitmap image = new Bitmap(num2, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(image);
            int num8 = Text.Length;
            int num3 = 0;
            for (int j = 0; j < num8; j++)
            {
                graphics.DrawImage(bitmapArray[j], num3, 0);
                num3 += bitmapArray[j].Width;
            }
            int num7 = Text.Length;
            for (int k = 0; k < num7; k++)
            {
                bitmapArray[k].Dispose();
            }
            graphics.Dispose();
            return image;
        }

        public static void Init()
        {
            streamArray[0] = new FileStream(Client.GetFilePath("unifont.mul"), FileMode.Open, FileAccess.Read);
            streamArray[1] = new FileStream(Client.GetFilePath("unifont1.mul"), FileMode.Open, FileAccess.Read);
            streamArray[2] = new FileStream(Client.GetFilePath("unifont2.mul"), FileMode.Open, FileAccess.Read);
            readerArray[0] = new BinaryReader(streamArray[0]);
            readerArray[1] = new BinaryReader(streamArray[1]);
            readerArray[2] = new BinaryReader(streamArray[2]);
        }

        private class CharInfo
        {
            public Bitmap Cache;
            public int Height;
            public int Width;
            public int xOffset;
            public int yOffset;
            

            public override string ToString()
            {
                string str = "";
                return str;
            }
        }

        private class FontSet
        {
            public UnicodeFonts.CharInfo[] cinfo = new UnicodeFonts.CharInfo[0x10001];
        }
    }

 

}
