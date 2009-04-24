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

using System;
using System.IO;
using System.Windows.Forms;
using Ultima;

namespace FiddlerControls
{
    public partial class MapReplace : Form
    {
        private Ultima.Map workingmap;
        public MapReplace(Ultima.Map currmap)
        {
            InitializeComponent();
            workingmap = currmap;
            numericUpDownX1.Maximum = workingmap.Width;
            numericUpDownX2.Maximum = workingmap.Width;
            numericUpDownY1.Maximum = workingmap.Height;
            numericUpDownY2.Maximum = workingmap.Height;
            this.Text = String.Format("MapReplace ID:{0}",workingmap.FileIndex);
        }

        private void OnClickBrowse(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select directory containing the map files";
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() == DialogResult.OK)
                textBox1.Text = dialog.SelectedPath;
        }

        private void OnClickCopy(object sender, EventArgs e)
        {
            string path = textBox1.Text;
            if (!Directory.Exists(path))
                return;
            int x1 = (int)numericUpDownX1.Value;
            int x2 = (int)numericUpDownX2.Value;
            int y1 = (int)numericUpDownY1.Value;
            int y2 = (int)numericUpDownY2.Value;
            if ((x1<0) || (x1>workingmap.Width))
                return;
            if ((x2<0) || (x2>workingmap.Width))
                return;
            if ((y1 < 0) || (y1 > workingmap.Height))
                return;
            if ((y2 < 0) || (y2 > workingmap.Height))
                return;
            if (x1 > x2)
                return;
            if (y1 > y2)
                return;

            x1 >>= 3;
            x2 >>= 3;
            y1 >>= 3;
            y2 >>= 3;

            int blocky = workingmap.Height >> 3;
            int blockx = workingmap.Width >> 3;

            progressBar1.Step = 1;
            progressBar1.Value = 0;
            progressBar1.Maximum=0;
            if (checkBoxMap.Checked)
                progressBar1.Maximum += blocky * blockx;
            if (checkBoxStatics.Checked)
                progressBar1.Maximum += blocky * blockx;
            
            if (checkBoxMap.Checked)
            {
                string copymap = Path.Combine(path, String.Format("map{0}.mul", workingmap.FileIndex));
                if (!File.Exists(copymap))
                    return;
                FileStream m_map_copy = new FileStream(copymap, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader m_mapReader_copy = new BinaryReader(m_map_copy);

                string mapPath = Ultima.Files.GetFilePath(String.Format("map{0}.mul", workingmap.FileIndex));
                FileStream m_map;
                BinaryReader m_mapReader;
                if (mapPath != null)
                {
                    m_map = new FileStream(mapPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    m_mapReader = new BinaryReader(m_map);
                }
                else
                    return;

                string mul = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, String.Format("map{0}.mul", workingmap.FileIndex));
                using (FileStream fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    using (BinaryWriter binmul = new BinaryWriter(fsmul))
                    {
                        for (int x = 0; x < blockx; x++)
                        {
                            for (int y = 0; y < blocky; y++)
                            {
                                if ((x1 <= x) && (x <= x2) && (y1 <= y) && (y <= y2))
                                {
                                    m_mapReader_copy.BaseStream.Seek(((x * blocky) + y) * 196, SeekOrigin.Begin);
                                    int header = m_mapReader_copy.ReadInt32();
                                    binmul.Write(header);
                                }
                                else
                                {
                                    m_mapReader.BaseStream.Seek(((x * blocky) + y) * 196, SeekOrigin.Begin);
                                    int header = m_mapReader.ReadInt32();
                                    binmul.Write(header);
                                }
                                for (int i = 0; i < 64; i++)
                                {
                                    short tileid;
                                    sbyte z;
                                    if ((x1 <= x) && (x <= x2) && (y1 <= y) && (y <= y2))
                                    {
                                        tileid = m_mapReader_copy.ReadInt16();
                                        z = m_mapReader_copy.ReadSByte();
                                    }
                                    else
                                    {
                                        tileid = m_mapReader.ReadInt16();
                                        z = m_mapReader.ReadSByte();
                                    }
                                    if ((tileid < 0) || (tileid >= 0x4000))
                                        tileid = 0;
                                    if (z < -128)
                                        z = -128;
                                    if (z > 127)
                                        z = 127;
                                    binmul.Write(tileid);
                                    binmul.Write(z);
                                }
                                progressBar1.PerformStep();
                            }
                        }
                    }
                }
                m_mapReader.Close();
                m_mapReader_copy.Close();
            }
            if (checkBoxStatics.Checked)
            {
                string indexPath = Files.GetFilePath(String.Format("staidx{0}.mul", workingmap.FileIndex));
                FileStream m_Index;
                BinaryReader m_IndexReader;
                if (indexPath != null)
                {
                    m_Index = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    m_IndexReader = new BinaryReader(m_Index);
                }
                else
                    return;

                string staticsPath = Files.GetFilePath(String.Format("statics{0}.mul", workingmap.FileIndex));

                FileStream m_Statics;
                BinaryReader m_StaticsReader;
                if (staticsPath != null)
                {
                    m_Statics = new FileStream(staticsPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    m_StaticsReader = new BinaryReader(m_Statics);
                }
                else
                    return;


                string copyindexPath = Path.Combine(path, String.Format("staidx{0}.mul", workingmap.FileIndex));
                if (!File.Exists(copyindexPath))
                    return;
                FileStream m_Index_copy = new FileStream(copyindexPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader m_IndexReader_copy = new BinaryReader(m_Index_copy);

                string copystaticsPath = Path.Combine(path, String.Format("statics{0}.mul", workingmap.FileIndex));
                if (!File.Exists(copystaticsPath))
                    return;
                FileStream m_Statics_copy = new FileStream(copystaticsPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader m_StaticsReader_copy = new BinaryReader(m_Statics_copy);

                string idx = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, String.Format("staidx{0}.mul", workingmap.FileIndex));
                string mul = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, String.Format("statics{0}.mul", workingmap.FileIndex));
                using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write),
                                  fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    using (BinaryWriter binidx = new BinaryWriter(fsidx),
                                        binmul = new BinaryWriter(fsmul))
                    {
                        for (int x = 0; x < blockx; x++)
                        {
                            for (int y = 0; y < blocky; y++)
                            {
                                int lookup, length, extra;
                                if ((x1 <= x) && (x <= x2) && (y1 <= y) && (y <= y2))
                                {
                                    m_IndexReader_copy.BaseStream.Seek(((x * blocky) + y) * 12, SeekOrigin.Begin);
                                    lookup = m_IndexReader_copy.ReadInt32();
                                    length = m_IndexReader_copy.ReadInt32();
                                    extra = m_IndexReader_copy.ReadInt32();
                                }
                                else
                                {
                                    m_IndexReader.BaseStream.Seek(((x * blocky) + y) * 12, SeekOrigin.Begin);
                                    lookup = m_IndexReader.ReadInt32();
                                    length = m_IndexReader.ReadInt32();
                                    extra = m_IndexReader.ReadInt32();
                                }

                                if (lookup < 0 || length <= 0)
                                {
                                    binidx.Write((int)-1); // lookup
                                    binidx.Write((int)-1); // length
                                    binidx.Write((int)-1); // extra
                                }
                                else
                                {
                                    if ((x1 <= x) && (x <= x2) && (y1 <= y) && (y <= y2))
                                        m_Statics_copy.Seek(lookup, SeekOrigin.Begin);
                                    else
                                        m_Statics.Seek(lookup, SeekOrigin.Begin);

                                    int fsmullength = (int)fsmul.Position;
                                    int count = length / 7;
                                    if (RemoveDupl.Checked)
                                    {
                                        StaticTile[] tilelist = new StaticTile[count];
                                        int j = 0;
                                        for (int i = 0; i < count; i++)
                                        {
                                            StaticTile tile = new StaticTile();
                                            if ((x1 <= x) && (x <= x2) && (y1 <= y) && (y <= y2))
                                            {
                                                tile.m_ID = m_StaticsReader_copy.ReadInt16();
                                                tile.m_X = m_StaticsReader_copy.ReadByte();
                                                tile.m_Y = m_StaticsReader_copy.ReadByte();
                                                tile.m_Z = m_StaticsReader_copy.ReadSByte();
                                                tile.m_Hue = m_StaticsReader_copy.ReadInt16();
                                            }
                                            else
                                            {
                                                tile.m_ID = m_StaticsReader.ReadInt16();
                                                tile.m_X = m_StaticsReader.ReadByte();
                                                tile.m_Y = m_StaticsReader.ReadByte();
                                                tile.m_Z = m_StaticsReader.ReadSByte();
                                                tile.m_Hue = m_StaticsReader.ReadInt16();
                                            }

                                            if ((tile.m_ID >= 0) && (tile.m_ID < 0x4000))
                                            {
                                                if (tile.m_Hue < 0)
                                                    tile.m_Hue = 0;
                                                bool first = true;
                                                for (int k = 0; k < j; k++)
                                                {
                                                    if ((tilelist[k].m_ID == tile.m_ID)
                                                        && ((tilelist[k].m_X == tile.m_X) && (tilelist[k].m_Y == tile.m_Y))
                                                        && (tilelist[k].m_Z == tile.m_Z)
                                                        && (tilelist[k].m_Hue == tile.m_Hue))
                                                    {
                                                        first = false;
                                                        break;
                                                    }
                                                }
                                                if (first)
                                                {
                                                    tilelist[j] = tile;
                                                    j++;
                                                }
                                            }
                                        }
                                        if (j > 0)
                                        {
                                            binidx.Write((int)fsmul.Position); //lookup
                                            for (int i = 0; i < j; i++)
                                            {
                                                binmul.Write(tilelist[i].m_ID);
                                                binmul.Write(tilelist[i].m_X);
                                                binmul.Write(tilelist[i].m_Y);
                                                binmul.Write(tilelist[i].m_Z);
                                                binmul.Write(tilelist[i].m_Hue);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bool firstitem = true;
                                        for (int i = 0; i < count; i++)
                                        {
                                            short graphic, shue;
                                            byte sx, sy;
                                            sbyte sz;
                                            if ((x1 <= x) && (x <= x2) && (y1 <= y) && (y <= y2))
                                            {
                                                graphic = m_StaticsReader_copy.ReadInt16();
                                                sx = m_StaticsReader_copy.ReadByte();
                                                sy = m_StaticsReader_copy.ReadByte();
                                                sz = m_StaticsReader_copy.ReadSByte();
                                                shue = m_StaticsReader_copy.ReadInt16();
                                            }
                                            else
                                            {
                                                graphic = m_StaticsReader.ReadInt16();
                                                sx = m_StaticsReader.ReadByte();
                                                sy = m_StaticsReader.ReadByte();
                                                sz = m_StaticsReader.ReadSByte();
                                                shue = m_StaticsReader.ReadInt16();
                                            }

                                            if ((graphic >= 0) && (graphic < 0x4000)) //legal?
                                            {
                                                if (shue < 0)
                                                    shue = 0;
                                                if (firstitem)
                                                {
                                                    binidx.Write((int)fsmul.Position); //lookup
                                                    firstitem = false;
                                                }
                                                binmul.Write(graphic);
                                                binmul.Write(sx);
                                                binmul.Write(sy);
                                                binmul.Write(sz);
                                                binmul.Write(shue);
                                            }
                                        }
                                    }
                                    fsmullength = (int)fsmul.Position - fsmullength;
                                    if (fsmullength > 0)
                                    {
                                        binidx.Write(fsmullength); //length
                                        binidx.Write(extra); //extra
                                    }
                                    else
                                    {
                                        binidx.Write((int)-1); //lookup
                                        binidx.Write((int)-1); //length
                                        binidx.Write((int)-1); //extra
                                    }
                                }
                                progressBar1.PerformStep();
                            }
                        }
                    }
                }
                m_IndexReader.Close();
                m_StaticsReader.Close();
                m_Index_copy.Close();
                m_StaticsReader_copy.Close();
            }

            MessageBox.Show(String.Format("Files saved to {0}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
    }
}
