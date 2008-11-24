using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Ultima
{
    public sealed class Animdata
    {
        public static Hashtable AnimData;
        unsafe static Animdata()
        {
            AnimData = new Hashtable();
            string path = Client.GetFilePath("animdata.mul");
            if (path != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryReader bin = new BinaryReader(fs);
                    int id=0;
                    byte unk, fcount, finter, fstart;
                    sbyte[] fdata;
                    while (bin.BaseStream.Length != bin.BaseStream.Position)
                    {
                        bin.ReadInt32(); // chunk header

                        // Read 8 tiles
                        for (int i = 0; i < 8; ++i)
                        {
                            fdata = new sbyte[64];
                            for (int j = 0; j < 64; ++j)
                            {
                                fdata[j] = bin.ReadSByte();
                            }
                            unk = bin.ReadByte();
                            fcount = bin.ReadByte();
                            finter = bin.ReadByte();
                            fstart = bin.ReadByte();
                            if (fcount > 0)
                                AnimData[id] = new Data(fdata, unk, fcount, finter, fstart);
                            ++id;
                        }
                    }
                }
            }
        }
        public unsafe static Data GetAnimData(int id)
        {
            if (AnimData.Contains(id))
                return ((Data)AnimData[id]);
            else
                return null;
        }

        public class Data
        {
            public sbyte[] FrameData;
            public byte unknown;
            public byte FrameCount;
            public byte FrameInterval;
            public byte FrameStart;
            public Data(sbyte[] frame, byte unk, byte fcount, byte finter, byte fstart)
            {
                FrameData = frame;
                unknown = unk;
                FrameCount = fcount;
                FrameInterval = finter;
                FrameStart = fstart;
            }
        }
    }
}
