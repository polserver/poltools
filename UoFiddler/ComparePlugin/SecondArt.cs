﻿using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Ultima;

namespace ComparePlugin
{
    internal static class SecondArt
    {
        private static SecondFileIndex _mFileIndex;
        private static Bitmap[] _mCache;

        private static byte[] _mStreamBuffer;
        private static byte[] _validbuffer;

        public static void SetFileIndex(string idxPath, string mulPath)
        {
            _mFileIndex = new SecondFileIndex(idxPath, mulPath, 0x14000);
            _mCache = new Bitmap[0x14000];
        }

        public static int GetMaxItemId()
        {
            if (GetIdxLength() == 0xC000)
            {
                return 0x7FFF;
            }

            if (GetIdxLength() == 0x13FDC)
            {
                return 0xFFDB;
            }

            return 0x3FFF;
        }

        private static ushort GetLegalItemId(int itemId)
        {
            if (itemId < 0)
            {
                return 0;
            }

            int max = GetMaxItemId();
            if (itemId > max)
            {
                return 0;
            }

            return (ushort)itemId;
        }

        private static int GetIdxLength()
        {
            return (int)(_mFileIndex.IdxLength / 12);
        }

        public static unsafe bool IsValidStatic(int index)
        {
            index = GetLegalItemId(index);
            index += 0x4000;

            if (_mCache[index] != null)
            {
                return true;
            }

            Stream stream = _mFileIndex.Seek(index, out _, out _);

            if (stream == null)
            {
                return false;
            }

            if (_validbuffer == null)
            {
                _validbuffer = new byte[4];
            }

            stream.Seek(4, SeekOrigin.Current);
            stream.Read(_validbuffer, 0, 4);
            fixed (byte* b = _validbuffer)
            {
                short* dat = (short*)b;
                return *dat++ > 0 && *dat > 0;
            }
        }

        public static Bitmap GetStatic(int index)
        {
            index = GetLegalItemId(index);
            index += 0x4000;

            if (_mCache[index] != null)
            {
                return _mCache[index];
            }

            Stream stream = _mFileIndex.Seek(index, out int length, out _);
            if (stream == null)
            {
                return null;
            }

            if (Files.CacheData)
            {
                return _mCache[index] = LoadStatic(stream, length);
            }
            else
            {
                return LoadStatic(stream, length);
            }
        }

        // TODO: unused method?
        // public static byte[] GetRawStatic(int index)
        // {
        //     index = GetLegalItemId(index);
        //     index += 0x4000;
        //
        //     var stream = _mFileIndex.Seek(index, out var length, out _);
        //     if (stream == null)
        //     {
        //         return null;
        //     }
        //
        //     var buffer = new byte[length];
        //     stream.Read(buffer, 0, length);
        //     return buffer;
        // }

        private static unsafe Bitmap LoadStatic(Stream stream, int length)
        {
            Bitmap bmp;
            if (_mStreamBuffer == null || _mStreamBuffer.Length < length)
            {
                _mStreamBuffer = new byte[length];
            }

            stream.Read(_mStreamBuffer, 0, length);
            stream.Close();

            fixed (byte* data = _mStreamBuffer)
            {
                ushort* binData = (ushort*)data;
                int count = 2;
                // bin.ReadInt32(); // TODO: ???
                int width = binData[count++];
                int height = binData[count++];

                if (width <= 0 || height <= 0)
                {
                    return null;
                }

                int[] lookups = new int[height];

                int start = height + 4;

                for (int i = 0; i < height; ++i)
                {
                    lookups[i] = start + binData[count++];
                }

                bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
                BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);

                ushort* line = (ushort*)bd.Scan0;
                int delta = bd.Stride >> 1;

                for (int y = 0; y < height; ++y, line += delta)
                {
                    count = lookups[y];

                    ushort* cur = line;
                    int xOffset, xRun;

                    while ((xOffset = binData[count++]) + (xRun = binData[count++]) != 0)
                    {
                        if (xOffset > delta)
                        {
                            break;
                        }

                        cur += xOffset;
                        if (xOffset + xRun > delta)
                        {
                            break;
                        }

                        ushort* end = cur + xRun;

                        while (cur < end)
                        {
                            *cur++ = (ushort)(binData[count++] ^ 0x8000);
                        }
                    }
                }
                bmp.UnlockBits(bd);
            }
            return bmp;
        }

        public static bool IsValidLand(int index)
        {
            index &= 0x3FFF;
            return _mCache[index] != null || _mFileIndex.Valid(index, out _, out _);
        }

        public static Bitmap GetLand(int index)
        {
            index &= 0x3FFF;

            if (_mCache[index] != null)
            {
                return _mCache[index];
            }

            Stream stream = _mFileIndex.Seek(index, out int length, out _);
            if (stream == null)
            {
                return null;
            }

            return Files.CacheData
                ? _mCache[index] = LoadLand(stream, length)
                : LoadLand(stream, length);
        }

        // TODO: unused method?
        // public static byte[] GetRawLand(int index)
        // {
        //     index &= 0x3FFF;
        //
        //     var stream = _mFileIndex.Seek(index, out var length, out _);
        //     if (stream == null)
        //     {
        //         return null;
        //     }
        //
        //     var buffer = new byte[length];
        //     stream.Read(buffer, 0, length);
        //     return buffer;
        // }

        private static unsafe Bitmap LoadLand(Stream stream, int length)
        {
            Bitmap bmp = new Bitmap(44, 44, PixelFormat.Format16bppArgb1555);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, 44, 44), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
            if (_mStreamBuffer == null || _mStreamBuffer.Length < length)
            {
                _mStreamBuffer = new byte[length];
            }

            stream.Read(_mStreamBuffer, 0, length);
            stream.Close();
            fixed (byte* binData = _mStreamBuffer)
            {
                ushort* bdata = (ushort*)binData;
                int xOffset = 21;
                int xRun = 2;

                ushort* line = (ushort*)bd.Scan0;
                int delta = bd.Stride >> 1;

                for (int y = 0; y < 22; ++y, --xOffset, xRun += 2, line += delta)
                {
                    ushort* cur = line + xOffset;
                    ushort* end = cur + xRun;

                    while (cur < end)
                    {
                        *cur++ = (ushort)(*bdata++ | 0x8000);
                    }
                }

                xOffset = 0;
                xRun = 44;

                for (int y = 0; y < 22; ++y, ++xOffset, xRun -= 2, line += delta)
                {
                    ushort* cur = line + xOffset;
                    ushort* end = cur + xRun;

                    while (cur < end)
                    {
                        *cur++ = (ushort)(*bdata++ | 0x8000);
                    }
                }
            }
            bmp.UnlockBits(bd);
            return bmp;
        }
    }
}