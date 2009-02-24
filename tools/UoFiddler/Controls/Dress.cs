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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Ntx.GD;
using Ultima;

namespace FiddlerControls
{
    public partial class Dress : UserControl
    {
        public Dress()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        #region Draworder Arrays
        private static int[] draworder ={
            0x00,// - Background
            0x14,// - Back (Cloak)
            0x05,// - Chest Clothing/Female Chest Armor
            0x04,// - Pants
            0x03,// - Foot Covering/Armor
            0x18,// - Legs (inner)(Leg Armor)
            0x13,// - Arm Covering/Armor
            0x0D,// - Torso (inner)(Chest Armor)
            0x11,// - Torso (Middle)(Surcoat, Tunic, Full Apron, Sash)
            0x08,// - Ring
            0x09,// - Talisman
            0x0E,// - Bracelet
            0x07,// - Gloves
            0x17,// - Legs (outer)(Skirt/Kilt)
            0x0A,// - Neck Covering/Armor
            0x0B,// - Hair
            0x0C,// - Waist (Half-Apron)
            0x16,// - Torso (outer)(Robe)
            0x10,// - Facial Hair
            0x12,// - Earrings
            0x06,// - Head Covering/Armor
            0x01,// - Single-Hand item/weapon
            0x02,// - Two-Hand item/weapon (including Shield)
            0x15 // - BackPack
        };
        private static int[] draworder2 ={
            0x00,// - Background
            0x05,// - Chest Clothing/Female Chest Armor
            0x04,// - Pants
            0x03,// - Foot Covering/Armor
            0x18,// - Legs (inner)(Leg Armor)
            0x13,// - Arm Covering/Armor
            0x0D,// - Torso (inner)(Chest Armor)
            0x11,// - Torso (Middle)(Surcoat, Tunic, Full Apron, Sash)
            0x08,// - Ring
            0x09,// - Talisman
            0x0E,// - Bracelet
            0x07,// - Gloves
            0x17,// - Legs (outer)(Skirt/Kilt)
            0x0A,// - Neck Covering/Armor
            0x0B,// - Hair
            0x0C,// - Waist (Half-Apron)
            0x16,// - Torso (outer)(Robe)
            0x10,// - Facial Hair
            0x12,// - Earrings
            0x06,// - Head Covering/Armor
            0x01,// - Single-Hand item/weapon
            0x02,// - Two-Hand item/weapon (including Shield)
            0x14,// - Back (Cloak)
            0x15 // - BackPack
        };
        #endregion
        private System.Drawing.Point drawpoint = new System.Drawing.Point(0, 0);
        private System.Drawing.Point drawpointAni = new System.Drawing.Point(100, 100);

        private object[] layers=new object[25];
        private bool female = false;
        private bool elve = false;
        private bool showPD = true;
        private bool animate = false;
        private Timer m_Timer = null;
        private Bitmap[] m_Animation;
        private int m_FrameIndex;
        private int facing = 1;
        private int action = 1;
        private bool Loaded = false;
        private int[] hues = new int[25];
        [Browsable(false)]
        public int[] Hues { get { return hues; } set { hues = value; } }

        /// <summary>
        /// Reload when loaded
        /// </summary>
        public void Reload()
        {
            if (!Loaded)
                return;
            layers = new object[25];
            female = false;
            elve = false;
            showPD = true;
            animate = false;
            facing = 1;
            action = 1;
            if (m_Timer != null)
            {
                if (m_Timer.Enabled)
                    m_Timer.Stop();

                m_Timer.Dispose();
                m_Timer = null;
            }

            if (m_Animation != null)
            {
                for (int i = 0; i < m_Animation.Length; i++)
                {
                    if (m_Animation[i] != null)
                        m_Animation[i].Dispose();
                }
            }

            m_Animation = null;
            m_FrameIndex = 0;
            EquipTable.Initialize();
            GumpTable.Initialize();
            OnLoad(this, EventArgs.Empty);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            Options.LoadedUltimaClass["TileData"] = true;
            Options.LoadedUltimaClass["Art"] = true;
            Options.LoadedUltimaClass["Hues"] = true;
            Options.LoadedUltimaClass["Animations"] = true;
            Options.LoadedUltimaClass["Gumps"] = true;
            Loaded = true;
            extractAnimationToolStripMenuItem.Visible = false;
            extractAnimatedAnimationToolStripMenuItem.Visible = false;

            DressPic.Image = new Bitmap(DressPic.Width, DressPic.Height);
            pictureBoxDress.Image = new Bitmap(pictureBoxDress.Width, pictureBoxDress.Height);

            checkedListBoxWear.BeginUpdate();
            checkedListBoxWear.Items.Clear();
            for (int i = 0; i < layers.Length; ++i)
            {
                layers[i] = (object)0;
                checkedListBoxWear.Items.Add(String.Format("0x{0:X2}", i), true);
            }
            checkedListBoxWear.EndUpdate();

            groupBoxAnimate.Visible = false;
            animateToolStripMenuItem.Visible = false;
            FacingBar.Value = (facing + 3) & 7;
            ActionBar.Value = action;
            toolTip1.SetToolTip(FacingBar, FacingBar.Value.ToString());
            BuildDressList();
            DrawPaperdoll();
            this.Cursor = Cursors.Default;
        }

        private void DrawPaperdoll()
        {
            if (!showPD)
            {
                DrawAnimation();
                return;
            }
            using (Graphics graphpic = Graphics.FromImage(DressPic.Image))
            {
                graphpic.Clear(Color.Black);
                if (checkedListBoxWear.GetItemChecked(0))
                {
                    Bitmap background;
                    if (!female)
                        background = (!elve) ? Gumps.GetGump(0xC) : Gumps.GetGump(0xE);
                    else
                        background = (!elve) ? Gumps.GetGump(0xD) : Gumps.GetGump(0xF);
                    if (background != null)
                    {
                        if (hues[0] > 0)
                        {
                            Bitmap b = new Bitmap(background);
                            int hue = hues[0];
                            bool gray=false;
                            if ((hue & 0x8000) != 0)
                            {
                                hue ^= 0x8000;
                                gray = true;
                            }
                            Ultima.Hues.List[hue].ApplyTo(b, gray);
                            background = b;
                        }
                        graphpic.DrawImage(background, drawpoint);
                    }
                }
                for (int i = 1; i < draworder.Length; ++i)
                {
                    if ((int)layers[draworder[i]] != 0)
                    {
                        if (checkedListBoxWear.GetItemChecked(draworder[i]))
                        {
                            int ani = TileData.ItemTable[(int)layers[draworder[i]]].Animation;
                            int gump = ani + 50000;
                            int hue = 0;
                            ConvertBody(ref ani, ref gump, ref hue);
                            if (female)
                            {
                                gump += 10000;
                                if (!Gumps.IsValidIndex(gump))  // female gump.def entry?
                                    ConvertGump(ref gump, ref hue);
                                if (!Gumps.IsValidIndex(gump)) // nope so male gump
                                    gump -= 10000;
                            }

                            if (!Gumps.IsValidIndex(gump)) // male (or invalid female)
                                ConvertGump(ref gump, ref hue);

                            Bitmap bmp = new Bitmap(Gumps.GetGump(gump));
                            if (bmp == null)
                                continue;
                            if (hues[draworder[i]] > 0)
                                hue = hues[draworder[i]];
                            bool onlyHueGrayPixels = ((hue & 0x8000) != 0);
                            hue = (hue & 0x3FFF) - 1;
                            Hue hueObject = null;
                            if (hue >= 0 && hue < Ultima.Hues.List.Length)
                            {
                                hueObject = Ultima.Hues.List[hue];
                                hueObject.ApplyTo(bmp, onlyHueGrayPixels);
                            }
                            graphpic.DrawImage(bmp, drawpoint);
                        }
                    }
                }
            }
            DressPic.Invalidate();
        }

        private void DrawAnimation()
        {
            if (animate)
            {
                DoAnimation();
                return;
            }
            using (Graphics graphpic = Graphics.FromImage(DressPic.Image))
            {
                graphpic.Clear(Color.WhiteSmoke);
                int hue = 0;
                int back = 0;
                if (checkedListBoxWear.GetItemChecked(0))
                {
                    if (!female)
                        back = (!elve) ? 400 : 605;
                    else
                        back = (!elve) ? 401 : 606;
                }
                Frame[] background;
                if (hues[0] > 0)
                {
                    hue = hues[0];
                    background = Animations.GetAnimation(back, action, facing, ref hue, true, true);
                }
                else
                    background = Animations.GetAnimation(back, action, facing, ref hue, false, true);

                System.Drawing.Point draw = new System.Drawing.Point();
                if (background != null)
                {
                    draw.X = drawpointAni.X - background[0].Center.X;
                    draw.Y = drawpointAni.Y - background[0].Center.Y - background[0].Bitmap.Height;
                    graphpic.DrawImage(background[0].Bitmap, draw);
                }
                int[] animorder = draworder2;
                if (((facing - 3) & 7) >= 4 && ((facing - 3) & 7) <= 6)
                    animorder = draworder;
                for (int i = 1; i < draworder.Length; ++i)
                {
                    if ((int)layers[animorder[i]] != 0)
                    {
                        if (checkedListBoxWear.GetItemChecked(animorder[i]))
                        {
                            if (TileData.ItemTable == null)
                                break;
                            int ani = TileData.ItemTable[(int)layers[animorder[i]]].Animation;
                            int gump = ani + 50000;
                            hue = 0;
                            ConvertBody(ref ani, ref gump, ref hue);
                            if (!Animations.IsActionDefined(ani, action, facing))
                                continue;

                            Frame[] frames;
                            if (hues[animorder[i]] > 0)
                            {
                                hue = hues[animorder[i]];
                                frames = Animations.GetAnimation(ani, action, facing, ref hue, true, true);
                            }
                            else
                                frames = Animations.GetAnimation(ani, action, facing, ref hue, false, true);
                            Bitmap bmp = frames[0].Bitmap;
                            if (bmp == null)
                                continue;
                            draw.X = drawpointAni.X - frames[0].Center.X;
                            draw.Y = drawpointAni.Y - frames[0].Center.Y - frames[0].Bitmap.Height;

                            graphpic.DrawImage(bmp, draw);
                        }
                    }
                }
            }
            DressPic.Invalidate();
        }

        private void DoAnimation()
        {
            if (m_Timer == null)
            {
                int hue = 0;
                int back = 0;
                if (checkedListBoxWear.GetItemChecked(0))
                {
                    if (!female)
                        back = (!elve) ? 400 : 605;
                    else
                        back = (!elve) ? 401 : 606;
                }
                Frame[] mobile;
                if (hues[0] > 0)
                {
                    hue = hues[0];
                    mobile = Animations.GetAnimation(back, action, facing, ref hue, true, false);
                }
                else
                    mobile = Animations.GetAnimation(back, action, facing, ref hue, false, false);
                System.Drawing.Point draw = new System.Drawing.Point();

                int count = mobile.Length;
                m_Animation = new Bitmap[count];
                int[] animorder = draworder2;
                if (((facing - 3) & 7) >= 4 && ((facing - 3) & 7) <= 6)
                    animorder = draworder;

                for (int i = 0; i < count; i++)
                {
                    m_Animation[i] = new Bitmap(DressPic.Width, DressPic.Height);
                    using (Graphics graph = Graphics.FromImage(m_Animation[i]))
                    {
                        graph.Clear(Color.WhiteSmoke);
                        draw.X = drawpointAni.X - mobile[i].Center.X;
                        draw.Y = drawpointAni.Y - mobile[i].Center.Y - mobile[i].Bitmap.Height;
                        graph.DrawImage(mobile[i].Bitmap, draw);
                        for (int j = 1; j < animorder.Length; ++j)
                        {
                            if ((int)layers[animorder[j]] != 0)
                            {
                                if (checkedListBoxWear.GetItemChecked(animorder[j]))
                                {
                                    int ani = TileData.ItemTable[(int)layers[animorder[j]]].Animation;
                                    int gump = ani + 50000;
                                    hue = 0;
                                    ConvertBody(ref ani, ref gump, ref hue);
                                    if (!Animations.IsActionDefined(ani, action, facing))
                                        continue;

                                    Frame[] frames;
                                    if (hues[animorder[j]] > 0)
                                    {
                                        hue = hues[animorder[j]];
                                        frames = Animations.GetAnimation(ani, action, facing, ref hue, true, false);
                                    }
                                    else
                                        frames = Animations.GetAnimation(ani, action, facing, ref hue, false, false);
                                    draw.X = draw.X;
                                    if ((frames.Length<i) || (frames[i].Bitmap == null))
                                        continue;
                                    draw.X = drawpointAni.X - frames[i].Center.X;
                                    draw.Y = drawpointAni.Y - frames[i].Center.Y - frames[i].Bitmap.Height;

                                    graph.DrawImage(frames[i].Bitmap, draw);
                                }
                            }
                        }
                    }
                }
                m_FrameIndex = 0;
                m_Timer = new Timer();
                m_Timer.Interval = 150;// 1000 / count;
                m_Timer.Tick += new EventHandler(AnimTick);
                m_Timer.Start();
            }
        }

        private void AnimTick(object sender, EventArgs e)
        {
            m_FrameIndex++;

            if (m_FrameIndex >= m_Animation.Length)
                m_FrameIndex = 0;

            if (m_Animation == null)
                return;
            if (m_Animation[m_FrameIndex] == null)
                return;
            using (Graphics graph = Graphics.FromImage(DressPic.Image))
            {
                graph.DrawImage(m_Animation[m_FrameIndex],drawpoint);
            }
            DressPic.Invalidate();
        }

        private void AfterSelectTreeView(object sender, TreeViewEventArgs e)
        {
            int ani = TileData.ItemTable[(int)e.Node.Tag].Animation;
            int gump = ani + 50000;
            int gumporig = gump;
            int hue = 0;
            Animations.Translate(ref ani);
            ConvertBody(ref ani, ref gump, ref hue);
            if (female)
            {
                gump += 10000;
                if (!Gumps.IsValidIndex(gump))  // female gump.def entry?
                    ConvertGump(ref gump, ref hue);
                if (!Gumps.IsValidIndex(gump)) // nope so male gump
                    gump -= 10000;
            }

            if (!Gumps.IsValidIndex(gump)) // male (or invalid female)
                ConvertGump(ref gump, ref hue);

            using (Graphics graph = Graphics.FromImage(pictureBoxDress.Image))
            {
                graph.Clear(Color.Transparent);
                Bitmap bmp = Gumps.GetGump(gump);
                if (bmp != null)
                {
                    bool onlyHueGrayPixels = ((hue & 0x8000) != 0);
                    hue = (hue & 0x3FFF) - 1;
                    Hue hueObject = null;
                    if (hue >= 0 && hue < Ultima.Hues.List.Length)
                    {
                        hueObject = Ultima.Hues.List[hue];
                        hueObject.ApplyTo(bmp, onlyHueGrayPixels);
                    }
                    int width = bmp.Width;
                    int height = bmp.Height;
                    if (width > pictureBoxDress.Width)
                    {
                        width = pictureBoxDress.Width;
                        height = bmp.Height * bmp.Height / bmp.Width;
                    }
                    if (height > pictureBoxDress.Height)
                    {
                        height = pictureBoxDress.Height;
                        width = pictureBoxDress.Width * bmp.Width / bmp.Height;
                    }
                    graph.DrawImage(bmp, new Rectangle(0, 0, width, height));
                }
            }
            pictureBoxDress.Invalidate();
            TextBox.Clear();
            TextBox.AppendText(String.Format("Objtype: 0x{0:X4}  Layer: 0x{1:X2}\n",
                (int)e.Node.Tag,
                TileData.ItemTable[(int)e.Node.Tag].Quality));
            TextBox.AppendText(String.Format("GumpID: 0x{0:X4} (0x{1:X4}) Hue: {2}\n",
                gump,
                gumporig,
                hue + 1));
            TextBox.AppendText(String.Format("Animation: 0x{0:X4} (0x{1:X4})\n",
                ani,
                TileData.ItemTable[(int)e.Node.Tag].Animation));
            TextBox.AppendText(String.Format("ValidGump: {0} ValidAnim: {1}\n",
                Gumps.IsValidIndex(gump).ToString(),
                Animations.IsActionDefined(ani, 0, 0).ToString()));
            TextBox.AppendText(String.Format("ValidLayer: {0}",
                (Array.IndexOf(draworder, TileData.ItemTable[(int)e.Node.Tag].Quality) == -1 ? false : true)));
        }

        private void OnClick_Animate(object sender, EventArgs e)
        {
            animate = !animate;
            if (animate)
            {
                extractAnimationToolStripMenuItem.Visible = true;
                extractAnimatedAnimationToolStripMenuItem.Visible = true;
            }
            else
            {
                extractAnimationToolStripMenuItem.Visible = false;
                extractAnimatedAnimationToolStripMenuItem.Visible = false;
            }
            RefreshDrawing();
        }

        private void OnChangeFemale(object sender, EventArgs e)
        {
            female = !female;
            RefreshDrawing();
        }

        private void OnChangeElve(object sender, EventArgs e)
        {
            elve = !elve;
            RefreshDrawing();
        }

        private void OnClick_Dress(object sender, EventArgs e)
        {
            if (treeViewItems.SelectedNode == null)
                return;
            int objtype = (int)treeViewItems.SelectedNode.Tag;
            int layer = TileData.ItemTable[objtype].Quality;
            if (Array.IndexOf(draworder, layer) == -1)
                return;
            layers[layer] = (object)objtype;
            checkedListBoxWear.BeginUpdate();
            checkedListBoxWear.Items[layer] = String.Format("0x{0:X2} {1}", layer, TileData.ItemTable[objtype].Name);
            checkedListBoxWear.EndUpdate();
            RefreshDrawing();
        }

        private void OnClick_UnDress(object sender, EventArgs e)
        {
            if (checkedListBoxWear.SelectedIndex == -1)
                return;
            int layer = checkedListBoxWear.SelectedIndex;
            checkedListBoxWear.Items[checkedListBoxWear.SelectedIndex] = String.Format("0x{0:X2}", layer);
            layers[layer] = (object)0;
            RefreshDrawing();
        }

        private void checkedListBox_Change(object sender, EventArgs e)
        {
            RefreshDrawing();
            if (checkedListBoxWear.SelectedIndex == -1)
                return;
            int layer = checkedListBoxWear.SelectedIndex;
            int objtype = (int)layers[layer];
            int ani = TileData.ItemTable[objtype].Animation;
            int gumpidorig = ani + 50000;
            int gumpid = gumpidorig;
            int hue = 0;
            Animations.Translate(ref ani);
            ConvertBody(ref ani, ref gumpid, ref hue);
            if (female)
            {
                gumpid += 10000;
                if (!Gumps.IsValidIndex(gumpid))  // female gump.def entry?
                    ConvertGump(ref gumpid, ref hue);
                if (!Gumps.IsValidIndex(gumpid)) // nope so male gump
                    gumpid -= 10000;
            }

            if (!Gumps.IsValidIndex(gumpid)) // male (or invalid female)
                ConvertGump(ref gumpid, ref hue);

            TextBox.Clear();
            TextBox.AppendText(String.Format("Objtype: 0x{0:X4}  Layer: 0x{1:X2}\n",
                objtype,
                layer));
            TextBox.AppendText(String.Format("GumpID: 0x{0:X4} (0x{1:X4}) Hue: {2}\n",
                gumpid,
                gumpidorig,
                hue));
            TextBox.AppendText(String.Format("Animation: 0x{0:X4} (0x{1:X4})\n",
                ani,
                TileData.ItemTable[objtype].Animation));
            TextBox.AppendText(String.Format("ValidGump: {0} ValidAnim: {1}",
                Gumps.IsValidIndex(gumpid).ToString(),
                Animations.IsActionDefined(ani, 0, 0).ToString()));
        }

        private void OnChangeSort(object sender, EventArgs e)
        {
            if (LayerSort.Checked)
                treeViewItems.TreeViewNodeSorter = new LayerSorter();
            else
                treeViewItems.TreeViewNodeSorter = new ObjtypeSorter();
        }

        private void OnClick_ChangeDisplay(object sender, EventArgs e)
        {
            showPD = !showPD;
            if (showPD)
            {
                groupBoxAnimate.Visible = false;
                animateToolStripMenuItem.Visible = false;
                showAnimationToolStripMenuItem.Text = "Show Animation";
            }
            else
            {
                groupBoxAnimate.Visible = true;
                animateToolStripMenuItem.Visible = true;
                showAnimationToolStripMenuItem.Text = "Show Paperdoll";
            }
            RefreshDrawing();
        }

        private void OnClickExtractImage(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            if (showPD)
            {
                string FileName = Path.Combine(path, "Dress PD.tiff");
                DressPic.Image.Save(FileName, ImageFormat.Tiff);
                MessageBox.Show(
                    String.Format("Paperdoll saved to {0}", FileName), 
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
            }
            else
            {
                string FileName = Path.Combine(path, "Dress IG.tiff");
                if (animate)
                    m_Animation[0].Save(FileName, ImageFormat.Tiff);
                else
                    DressPic.Image.Save(FileName, ImageFormat.Tiff);
                MessageBox.Show(
                    String.Format("InGame saved to {0}", FileName), 
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
            }
        }

        private void OnClickExtractAnim(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = "Dress Anim";

            for (int i = 0; i < m_Animation.Length; ++i)
            {
                m_Animation[i].Save(String.Format("{0}-{1}.tiff", FileName, i), ImageFormat.Tiff);
            }
            MessageBox.Show(
                String.Format("InGame Anim saved to '{0}-X.tiff'", FileName), 
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void onClickExtractAnimatedAnimation(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string gif = Path.Combine(path, "Dress Anim.gif");
            string temp = Path.Combine(path, "temp.png");
            using (FileStream fs = File.OpenWrite(gif))
            {
                m_Animation[0].Save(temp, ImageFormat.Png);
                GD host = new GD(GD.FileType.Png, temp);
                host.GifAnimBegin(fs);
                GD prev = null;
                for (int i = 0; i < m_Animation.Length; ++i)
                {
                    m_Animation[i].Save(temp, ImageFormat.Png);
                    GD frame = new GD(GD.FileType.Png, temp);
                    frame.GifAnimAdd(fs, 1, 0, 0, 15, GD.Disposal.None, prev);
                    prev = frame;
                }
                File.Delete(temp);
                host.GifAnimEnd(fs);
                fs.Close();
            }
            MessageBox.Show(
                String.Format("InGame Anim saved to '{0}'", gif),
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void BuildDressList()
        {
            treeViewItems.BeginUpdate();
            treeViewItems.Nodes.Clear();
            if (TileData.ItemTable != null)
            {
                for (int i = 0; i < TileData.ItemTable.Length; ++i)
                {
                    if (TileData.ItemTable[i].Wearable)
                    {
                        int ani = TileData.ItemTable[i].Animation;
                        if (ani != 0)
                        {
                            int hue = 0;
                            int gump = ani + 50000;
                            ConvertBody(ref ani, ref gump, ref hue);
                            if (!Gumps.IsValidIndex(gump))
                                ConvertGump(ref gump, ref hue);
                            bool hasani = Animations.IsActionDefined(ani, 0, 0);
                            bool hasgump = Gumps.IsValidIndex(gump);
                            TreeNode node = new TreeNode(String.Format("0x{0:X4} (0x{1:X2}) {2}",
                                        i,
                                        TileData.ItemTable[i].Quality,
                                        TileData.ItemTable[i].Name));
                            node.Tag = i;
                            if (Array.IndexOf(draworder, (int)TileData.ItemTable[i].Quality) == -1)
                                node.ForeColor = Color.DarkRed;
                            else if (!hasani)
                            {
                                if (!hasgump)
                                    node.ForeColor = Color.Red;
                                else
                                    node.ForeColor = Color.Orange;
                            }
                            else if (hasani && !hasgump)
                                node.ForeColor = Color.Blue;
                            treeViewItems.Nodes.Add(node);
                        }
                    }
                }
            }
            treeViewItems.EndUpdate();
        }

        public void RefreshDrawing()
        {
            if (m_Timer != null)
            {
                if (m_Timer.Enabled)
                    m_Timer.Stop();

                m_Timer.Dispose();
                m_Timer = null;
            }

            if (m_Animation != null)
            {
                for (int i = 0; i < m_Animation.Length; i++)
                {
                    if (m_Animation[i] != null)
                        m_Animation[i].Dispose();
                }
            }

            m_Animation = null;
            m_FrameIndex = 0;

            DrawPaperdoll();
        }

        private void OnScroll_Facing(object sender, EventArgs e)
        {
            facing = (FacingBar.Value - 3) & 7;
            toolTip1.SetToolTip(FacingBar, FacingBar.Value.ToString());
            RefreshDrawing();
        }

        private void OnScroll_Action(object sender, EventArgs e)
        {
            string[] tip =new string[]{"Walk_01","WalkStaff_01","Run_01","RunStaff_01","Idle_01","Idle_01",
                         "Fidget_Yawn_Stretch_01","CombatIdle1H_01","CombatIdle1H_01","AttackSlash1H_01",
                         "AttackPierce1H_01","AttackBash1H_01","AttackBash2H_01","AttackSlash2H_01",
                         "AttackPierce2H_01","CombatAdvance_1H_01","Spell1","Spell2","AttackBow_01",
                         "AttackCrossbow_01","GetHit_Fr_Hi_01","Die_Hard_Fwd_01","Die_Hard_Back_01",
                         "Horse_Walk_01","Horse_Run_01","Horse_Idle_01","Horse_Attack1H_SlashRight_01",
                         "Horse_AttackBow_01","Horse_AttackCrossbow_01","Horse_Attack2H_SlashRight_01",
                         "Block_Shield_Hard_01","Punch_Punch_Jab_01","Bow_Lesser_01","Salute_Armed1h_01",
                         "Ingest_Eat_01"};
            toolTip1.SetToolTip(ActionBar,ActionBar.Value.ToString()+" "+tip[ActionBar.Value]);
            action = ActionBar.Value;
            RefreshDrawing();
        }

        private void OnResizepictureDress(object sender, EventArgs e)
        {
            if (treeViewItems.SelectedNode != null)
            {
                pictureBoxDress.Image = new Bitmap(pictureBoxDress.Width, pictureBoxDress.Height);
                AfterSelectTreeView(this, new TreeViewEventArgs(treeViewItems.SelectedNode));
            }
        }

        private void OnResizeDressPic(object sender, EventArgs e)
        {
            if (checkedListBoxWear.Items.Count > 0) // inital event
            {
                DressPic.Image = new Bitmap(DressPic.Width, DressPic.Height);
                RefreshDrawing();
            }
        }

        private void ConvertGump(ref int gumpid, ref int hue)
        {
            if (GumpTable.Entries.Contains(gumpid))
            {
                GumpTableEntry entry = (GumpTableEntry)GumpTable.Entries[gumpid];
                hue = entry.NewHue;
                gumpid = entry.NewID;
            }
        }

        private void ConvertBody(ref int animId, ref int gumpid, ref int hue)
        {
            if (!elve)
            {
                if (!female)
                {
                    if (EquipTable.Human_male.Contains(animId))
                    {
                        EquipTableEntry entry = (EquipTableEntry)EquipTable.Human_male[animId];
                        gumpid = entry.NewID;
                        hue = entry.NewHue;
                        animId = entry.NewAnim;
                    }
                }
                else
                {
                    if (EquipTable.Human_female.Contains(animId))
                    {
                        EquipTableEntry entry = (EquipTableEntry)EquipTable.Human_female[animId];
                        gumpid = entry.NewID;
                        hue = entry.NewHue;
                        animId = entry.NewAnim;
                    }
                }
            }
            else
            {
                if (!female)
                {
                    if (EquipTable.Elven_male.Contains(animId))
                    {
                        EquipTableEntry entry = (EquipTableEntry)EquipTable.Elven_male[animId];
                        gumpid = entry.NewID;
                        hue = entry.NewHue;
                        animId = entry.NewAnim;
                    }
                }
                else
                {
                    if (EquipTable.Elven_female.Contains(animId))
                    {
                        EquipTableEntry entry = (EquipTableEntry)EquipTable.Elven_female[animId];
                        gumpid = entry.NewID;
                        hue = entry.NewHue;
                        animId = entry.NewAnim;
                    }
                }
            }
        }

        private HuePopUpDress showform = null;
        private void onClickHue(object sender, EventArgs e)
        {
            if (checkedListBoxWear.SelectedIndex == -1)
                return;

            if ((showform == null) || (showform.IsDisposed))
            {
                int layer = checkedListBoxWear.SelectedIndex;
                showform = new HuePopUpDress(this, hues[layer],layer);
                showform.TopMost = true;
                showform.Show();
            }
        }
    }

    public class ObjtypeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;
            return string.Compare(tx.Text, ty.Text);
        }
    }

    public class LayerSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;
            int layerx = TileData.ItemTable[(int)tx.Tag].Quality;
            int layery = TileData.ItemTable[(int)ty.Tag].Quality;
            if (layerx == layery)
                return 0;
            else if (layerx < layery)
                return -1;
            else
                return 1; 
        }
    }

    public class GumpTable
	{
        private static Hashtable m_Entries;
        public static Hashtable Entries { get { return m_Entries; } }

        // Seems only used if Gump is invalid
		static GumpTable()
        {
            Initialize();
        }
        public static void Initialize()
        {
            string path = Files.GetFilePath("gump.def");

            if (path == null)
                return;

            m_Entries = new Hashtable();
            using (StreamReader ip = new StreamReader(path))
            {
                string line;

                while ((line = ip.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                        continue;

                    try
                    {
                        // <ORIG BODY> {<NEW BODY>} <NEW HUE>
                        int index1 = line.IndexOf("{");
                        int index2 = line.IndexOf("}");

                        string param1 = line.Substring(0, index1);
                        string param2 = line.Substring(index1 + 1, index2 - index1 - 1);
                        string param3 = line.Substring(index2 + 1);

                        int iParam1 = Convert.ToInt32(param1.Trim());
                        int iParam2 = Convert.ToInt32(param2.Trim());
                        int iParam3 = Convert.ToInt32(param3.Trim());

                        m_Entries[iParam1] = new GumpTableEntry(iParam1, iParam2, iParam3);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
    public class GumpTableEntry
    {
        private int m_OldID;
        private int m_NewID;
        private int m_NewHue;

        public int OldID { get { return m_OldID; } }
        public int NewID { get { return m_NewID; } }
        public int NewHue { get { return m_NewHue; } }

        public GumpTableEntry(int oldID, int newID, int newHue)
        {
            m_OldID = oldID;
            m_NewID = newID;
            m_NewHue = newHue;
        }
    }

    public class EquipTable
    {
        private static Hashtable m_human_male;
        private static Hashtable m_human_female;
        private static Hashtable m_elven_male;
        private static Hashtable m_elven_female;

        public static Hashtable Human_male { get { return m_human_male; } }
        public static Hashtable Human_female { get { return m_human_female; } }
        public static Hashtable Elven_male { get { return m_elven_male; } }
        public static Hashtable Elven_female { get { return m_elven_female; } }

        static EquipTable()
        {
            Initialize();
        }

        public static void Initialize()
        {
            string path = Files.GetFilePath("equipconv.def");

            if (path == null)
                return;

            m_human_male = new Hashtable();
            m_human_female = new Hashtable();
            m_elven_male = new Hashtable();
            m_elven_female = new Hashtable();
            using (StreamReader ip = new StreamReader(path))
            {
                string line;

                while ((line = ip.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                        continue;
                    //#bodyType	#equipmentID	#convertToID	#GumpIDToUse	#hue
                    //GumpID (0 = equipmentID + 50000, -1 = convertToID + 50000, other numbers are the actual gumpID )

                    try
                    {
                        string[] split = line.Split('\t');

                        int bodytype = Convert.ToInt32(split[0]);
                        int animID = Convert.ToInt32(split[1]);
                        int convertID = Convert.ToInt32(split[2]);
                        int gumpID = Convert.ToInt32(split[3]);
                        int hue = Convert.ToInt32(split[4]);
                        if (gumpID == 0)
                            gumpID = animID + 50000;
                        else if (gumpID == -1)
                            gumpID = convertID + 50000;

                        EquipTableEntry entry = new EquipTableEntry(gumpID, hue, convertID);
                        if (bodytype == 400)
                            m_human_male[animID] = entry;
                        else if (bodytype == 401)
                            m_human_female[animID] = entry;
                        else if (bodytype == 605)
                            m_elven_male[animID] = entry;
                        else if (bodytype == 606)
                            m_elven_female[animID] = entry;
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
    public class EquipTableEntry
    {
        private int m_NewID;
        private int m_NewHue;
        private int m_NewAnim;

        public int NewID { get { return m_NewID; } }
        public int NewHue { get { return m_NewHue; } }
        public int NewAnim { get { return m_NewAnim; } }

        public EquipTableEntry(int newID, int newHue, int newAnim)
        {
            m_NewID = newID;
            m_NewHue = newHue;
            m_NewAnim = newAnim;
        }
    }
}
