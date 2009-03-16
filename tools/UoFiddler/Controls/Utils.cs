/***************************************************************************
 *
 * $Author: Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

using System.Drawing;
using System.Drawing.Imaging;

namespace FiddlerControls
{
    public static class Utils
    {
        /// <summary>
        /// Converts string to int with Hex recognition
        /// </summary>
        /// <param name="text">string to parse</param>
        /// <param name="result">out result</param>
        /// <param name="min">minvalue</param>
        /// <param name="max">maxvalue</param>
        /// <returns>bool could convert and between min/max</returns>
        public static bool ConvertStringToInt(string text, out int result, int min, int max)
        {
            bool candone;
            if (text.Contains("0x"))
            {
                string convert = text.Replace("0x", "");
                candone = int.TryParse(convert, System.Globalization.NumberStyles.HexNumber, null, out result);
            }
            else
                candone = int.TryParse(text, System.Globalization.NumberStyles.Integer, null, out result);

            if ((result > max) || (result < min))
                candone = false;

            return candone;
        }

        /// <summary>
        /// Converts string to int with Hex recognition
        /// </summary>
        /// <param name="text">string to parse</param>
        /// <param name="result">out result</param>
        /// <returns>bool could convert</returns>
        public static bool ConvertStringToInt(string text, out int result)
        {
            bool candone;
            if (text.Contains("0x"))
            {
                string convert = text.Replace("0x", "");
                candone = int.TryParse(convert, System.Globalization.NumberStyles.HexNumber, null, out result);
            }
            else
                candone = int.TryParse(text, System.Globalization.NumberStyles.Integer, null, out result);
            return candone;
        }

        public static unsafe Bitmap ConvertBmp(Bitmap bmp)
        {
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;

            Bitmap bmpnew = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppArgb1555);
            BitmapData bdnew = bmpnew.LockBits(new Rectangle(0, 0, bmpnew.Width, bmpnew.Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);

            ushort* linenew = (ushort*)bdnew.Scan0;
            int deltanew = bdnew.Stride >> 1;

            for (int Y = 0; Y < bmp.Height; ++Y, line += delta, linenew += deltanew)
            {
                ushort* cur = line;
                ushort* curnew = linenew;
                for (int X = 0; X < bmp.Width; X++)
                {
                    if (cur[X] != 32768)
                        curnew[X] = cur[X];
                }
            }
            bmp.UnlockBits(bd);
            bmpnew.UnlockBits(bdnew);
            return bmpnew;
        }
    }
}
