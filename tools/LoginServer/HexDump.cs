/***************************************************************************
 *
 * $Author: Nando_k - nando@polserver.com
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LoginServer
{
    class HexDump
    {
        static char ToSafeAscii(byte b)
        {
            int c = b;
            if (c >= 32 && c <= 126)
            {
                return (char)b;
            }
            return '.';
        }

        public static string Dump(byte[] bytes)
        {
            return Dump(bytes, 16, true, bytes.Length);
        }
        public static string Dump(byte[] bytes, int size)
        {
            return Dump(bytes, size, false, bytes.Length);
        }
        public static string Dump(byte[] bytes, int size, bool showline)
        {
            return Dump(bytes, size, showline, bytes.Length);
        }

        public static string Dump(byte[] bytes, int size, bool showline, int bytesleft)
        {
            if (size < 1) return String.Empty;
            if (bytesleft > bytes.Length) bytesleft = bytes.Length;

            StringBuilder sb = new StringBuilder();

            string hex = "";
            string ascii = "";

            int rows = bytesleft / size;
            if ((bytesleft % size) > 0)
            {
                rows++; // increase one more row if there´s more elements to be printed
            }


            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < size; i++)
                {
                    int index = i + j * size;

                    if (bytesleft < 1)
                    {
                        hex += "   ";
                        ascii += " ";
                        continue;
                    }

                    if (i > 0)
                        hex += " ";
                    hex += bytes[index].ToString("X2");


                    ascii += ToSafeAscii(bytes[index]);

                    bytesleft--;
                }
                if (showline)
                {
                    sb.AppendLine(String.Format("{0:0000}: {1} {2}", j*size, hex, ascii));   
                }
                else
                {
                    sb.AppendLine(String.Format("{0} {1}", hex, ascii));
                }
                hex = String.Empty;
                ascii = String.Empty;
            }

            return sb.ToString();
        }

        static public void DumpUnk(byte cmd, byte[] rest, int len, string who)
        {
            MemoryStream ms = new MemoryStream(len + 1);
            ms.WriteByte(cmd);
            ms.Write(rest, 0, len);

            byte[] tmp = ms.ToArray();

            Console.WriteLine("Unknown Packet 0x{0:X2} ({1} bytes) ({2}):", cmd, len + 1, who);
            Console.WriteLine(HexDump.Dump(tmp, 16, true, tmp.Length));
        }

    }
}
