using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Ultima
{
    public class Animdata
    {
        public static Data[] AnimData= new Data[0x4000];
        unsafe static Animdata()
        {
            string path = Client.GetFilePath("animdata.mul");
            if (path != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryReader bin = new BinaryReader(fs);
                    int id=0;
                    while (bin.BaseStream.Length != bin.BaseStream.Position)
                    {
                        bin.ReadInt32(); // chunk header

                        // Read 8 tiles
                        for (int i = 0; i < 8; ++i)
                        {
                            Data info = new Data();
                            for (int j = 0; j < 64; ++j)
                            {
                                info.FrameData[j] = bin.ReadSByte();
                            }
                            info.unknown = bin.ReadByte();
                            info.FrameCount = bin.ReadByte();
                            info.FrameInterval = bin.ReadByte();
                            info.FrameStart = bin.ReadByte();
                            if (info.FrameCount > 0)
                                AnimData[id] = info;
                            else
                                AnimData[id] = null;
                            ++id;
                        }
                    }
                }
            }
        }
        public unsafe static Data GetAnimData(int id)
        {
            return (AnimData[id]);
        }

        public class Data
        {
            public sbyte[] FrameData=new sbyte[64];
            public byte unknown;
            public byte FrameCount;
            public byte FrameInterval;
            public byte FrameStart;
        }
    }
}
