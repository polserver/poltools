﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Ultima.Helpers
{
    public static class Extensions
    {
        public static byte[] ToArray(this Bitmap bmp, PixelFormat? format = null)
        {
            if (bmp == null)
            {
                throw new ArgumentNullException(nameof(bmp));
            }

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadOnly, format ?? bmp.PixelFormat);
            try
            {
                int size = data.Stride * bmp.Height;
                byte[] buffer = new byte[size];
                Marshal.Copy(data.Scan0, buffer, 0, size);
                return buffer;
            }
            finally
            {
                bmp.UnlockBits(data);
            }
        }

        static readonly SHA256Managed Sha256 = new SHA256Managed();
        public static byte[] ToSha256(this byte[] buffer)
        {
            return Sha256.ComputeHash(buffer);
        }
    }
}
