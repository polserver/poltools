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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Ultima;

namespace Controls
{
    public partial class Dress : UserControl
    {
        public Dress()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private Bitmap bitpic;
        private Graphics graphpic;

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

        private static object[] layers=new object[25];
        private static bool female = false;
        private static bool elve = false;
        private static Point drawpoint = new Point(0, 0);
        private static Point drawpointAni = new Point(100, 50);
        private static bool showPD = true;
        private static bool animate = false;

        private Timer m_Timer = null;
        private Bitmap[] m_Animation;
        private int m_FrameIndex;
        private int facing = 1;
        private int action = 1;

        private void OnLoad(object sender, EventArgs e)
        {
            bitpic = new Bitmap(DressPic.Width, DressPic.Height);
            graphpic = Graphics.FromImage(bitpic);

            DressPic.BackColor = Color.Black;
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
        }

        private void DrawPaperdoll()
        {
            if (!showPD)
            {
                DrawAnimation();
                return;
            }
            graphpic.Clear(Color.Transparent);
            if (checkedListBoxWear.GetItemChecked(0))
            {
                if (!female)
                {
                    if (!elve)
                        graphpic.DrawImage(Gumps.GetGump(0xC), drawpoint);
                    else
                        graphpic.DrawImage(Gumps.GetGump(0xE), drawpoint);
                }
                else
                {
                    if (!elve)
                        graphpic.DrawImage(Gumps.GetGump(0xD), drawpoint);
                    else
                        graphpic.DrawImage(Gumps.GetGump(0xF), drawpoint);
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
                        int hue=0;
                        ConvertBody(ref ani, ref gump, ref hue);
                        ConvertGump(ref gump, ref hue);
                        if (female)
                        {
                            if (Gumps.IsValidIndex(gump + 10000))
                                gump += 10000;
                        }
                        Bitmap bmp = Gumps.GetGump(gump);
                        if (bmp == null)
                            continue;
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
            graphpic.Save();
            DressPic.Image = bitpic;
            DressPic.Update();
        }

        private void DrawAnimation()
        {
            if (animate)
            {
                DoAnimation();
                return;
            }
            graphpic.Clear(Color.Transparent);
            int hue = 0;
            int back = 0;
            if (checkedListBoxWear.GetItemChecked(0))
            {
                if (!female)
                {
                    if (!elve)
                        back = 400;
                    else
                        back = 605;
                }
                else
                {
                    if (!elve)
                        back = 401;
                    else
                        back = 606;
                }
            }
            Frame[] background = Animations.GetAnimation(back, action, facing, ref hue, false, true);
            Point draw = new Point(
                drawpointAni.X - background[0].Center.X,
                drawpointAni.Y - background[0].Center.Y);
            int[] animorder = draworder2;
            if (((facing - 3) & 7) >= 4 && ((facing - 3) & 7) <= 6)
                animorder = draworder;

            graphpic.DrawImage(background[0].Bitmap, draw);
            for (int i = 1; i < draworder.Length; ++i)
            {
                if ((int)layers[animorder[i]] != 0)
                {
                    if (checkedListBoxWear.GetItemChecked(animorder[i]))
                    {
                        int ani = TileData.ItemTable[(int)layers[animorder[i]]].Animation;
                        int gump = ani + 50000;
                        hue = 0;
                        ConvertBody(ref ani, ref gump, ref hue);
                        if (!Animations.IsActionDefined(ani, action, facing, 0, false))
                            continue;

                        Frame[] frames = Animations.GetAnimation(ani, action, facing, ref hue, false, true);
                        Bitmap bmp = frames[0].Bitmap;
                        if (bmp == null)
                            continue;
                        draw.X = drawpointAni.X - frames[0].Center.X;
                        draw.Y = drawpointAni.Y +background[0].Bitmap.Height- frames[0].Center.Y -frames[0].Bitmap.Height;
                            
                        graphpic.DrawImage(bmp, draw);
                    }
                }
            }
            graphpic.Save();
            DressPic.Image = bitpic;
            DressPic.Update();
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
                    {
                        if (!elve)
                            back = 400;
                        else
                            back = 605;
                    }
                    else
                    {
                        if (!elve)
                            back = 401;
                        else
                            back = 606;
                    }
                }
                Frame[] mobile = Animations.GetAnimation(back, action, facing, ref hue, false, false);
                Point draw = new Point();

                int count = mobile.Length;
                m_Animation = new Bitmap[count];
                int[] animorder = draworder2;
                if (((facing - 3) & 7) >= 4 && ((facing - 3) & 7) <= 6)
                    animorder = draworder;

                for (int i = 0; i < count; i++)
                {
                    m_Animation[i] = new Bitmap(DressPic.Width, DressPic.Height);
                    Graphics graph = Graphics.FromImage(m_Animation[i]);
                    graph.Clear(Color.Transparent);
                    draw.X = drawpointAni.X - mobile[i].Center.X;
                    draw.Y = drawpointAni.Y - mobile[i].Center.Y;
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
                                if (!Animations.IsActionDefined(ani, action, facing, 0, false))
                                    continue;

                                Frame[] frames = Animations.GetAnimation(ani, action, facing, ref hue, false, false);
                                if (frames[i].Bitmap == null)
                                    continue;
                                draw.X = drawpointAni.X - frames[i].Center.X;
                                draw.Y = drawpointAni.Y + mobile[i].Bitmap.Height - frames[i].Center.Y - frames[i].Bitmap.Height;

                                graph.DrawImage(frames[i].Bitmap, draw);
                            }
                        }
                    }
                    graph.Save();
                }
                m_FrameIndex = 0;
                m_Timer = new Timer();
                m_Timer.Interval = 1000 / count;
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
            DressPic.Image = m_Animation[m_FrameIndex];
            DressPic.Update();
        }

        private void BuildDressList()
        {
            treeViewItems.BeginUpdate();
            treeViewItems.Nodes.Clear();
            for (int i = 0; i<TileData.ItemTable.Length; ++i)
            {
                if (TileData.ItemTable[i].Wearable)
                {
                    int ani = TileData.ItemTable[i].Animation;
                    if (ani != 0)
                    {
                        int hue = 0;
                        int gump = ani + 50000;
                        ConvertBody(ref ani, ref gump, ref hue);
                        ConvertGump(ref gump, ref hue);
                        bool hasani = Animations.IsActionDefined(ani, 0, 0, 0, false);
                        bool hasgump = Gumps.IsValidIndex(gump);
                        TreeNode node = new TreeNode(String.Format("0x{0:X4} (0x{1:X2}) {2}",
                                    i, 
                                    TileData.ItemTable[i].Quality,
                                    TileData.ItemTable[i].Name));
                        node.Tag = i;
                        if (!hasani)
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
            treeViewItems.EndUpdate();
        }

        private void AfterSelectTreeView(object sender, TreeViewEventArgs e)
        {
            int ani = TileData.ItemTable[(int)e.Node.Tag].Animation;
            int gump = ani + 50000;
            int gumporig = gump;
            int hue=0;
            ConvertBody(ref ani, ref gump, ref hue);
            ConvertGump(ref gump, ref hue);
            if (female)
            {
                if (Gumps.IsValidIndex(gump + 10000))
                    gump += 10000;
            }

            Bitmap bmp=Gumps.GetGump(gump);
            Bitmap dress = new Bitmap(pictureBoxDress.Width, pictureBoxDress.Height);
            Graphics graph = Graphics.FromImage(dress);
            if (bmp == null)
                bmp=dress;
            bool onlyHueGrayPixels = ((hue & 0x8000) != 0);
            hue = (hue & 0x3FFF) - 1;
            Hue hueObject = null;
            if (hue >= 0 && hue < Ultima.Hues.List.Length)
            {
                hueObject = Ultima.Hues.List[hue];
                hueObject.ApplyTo(bmp, onlyHueGrayPixels);
            }
            int width=bmp.Width;
            int height=bmp.Height;
            if (width > dress.Width)
            {
                width = dress.Width;
                height = bmp.Height * bmp.Height / bmp.Width;
            }
            graph.DrawImage(bmp, new Rectangle(0,0, width, height));
            graph.Save();
            pictureBoxDress.Image = dress;
            pictureBoxDress.Update();
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
            TextBox.AppendText(String.Format("ValidGump: {0} ValidAnim: {1}",
                Gumps.IsValidIndex(gump).ToString(),
                Animations.IsActionDefined(ani, 0, 0, 0, false).ToString()));
        }

        private void OnChangeFemale(object sender, EventArgs e)
        {
            female = !female;
            RefreshDrawing();
        }

        private void OnClick_Dress(object sender, EventArgs e)
        {
            if (treeViewItems.SelectedNode == null)
                return;
            int objtype = (int)treeViewItems.SelectedNode.Tag;
            int layer = TileData.ItemTable[objtype].Quality;
            layers[layer] = (object)objtype;
            checkedListBoxWear.BeginUpdate();
            checkedListBoxWear.Items[layer]=String.Format("0x{0:X2} {1}", layer, TileData.ItemTable[objtype].Name);
            checkedListBoxWear.EndUpdate();
            RefreshDrawing();
        }

        private void OnChangeElve(object sender, EventArgs e)
        {
            elve = !elve;
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
            ConvertBody(ref ani, ref gumpid, ref hue);
            ConvertGump(ref gumpid, ref hue);
            if (female)
            {
                if (Gumps.IsValidIndex(gumpid + 10000))
                    gumpid+=10000;
            }
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
                Animations.IsActionDefined(ani, 0, 0, 0, false).ToString()));
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

        private void ConvertGump(ref int gumpid, ref int hue)
        {
            if (GumpTable.m_Entries.Contains(gumpid))
            {
                GumpTableEntry entry = (GumpTableEntry)GumpTable.m_Entries[gumpid];
                hue = entry.m_NewHue;
                gumpid = entry.m_NewID;
            }
        }

        private void ConvertBody(ref int animId,ref int gumpid, ref int hue)
        {
            if (!elve)
            {
                if (!female)
                {
                    if (EquipTable.human_male.Contains(animId))
                    {
                        EquipTableEntry entry = (EquipTableEntry)EquipTable.human_male[animId];
                        gumpid = entry.m_NewID;
                        hue = entry.m_NewHue;
                        animId = entry.m_NewAnim;
                    }
                }
                else
                {
                    if (EquipTable.human_female.Contains(animId))
                    {
                        EquipTableEntry entry = (EquipTableEntry)EquipTable.human_female[animId];
                        gumpid = entry.m_NewID;
                        hue = entry.m_NewHue;
                        animId = entry.m_NewAnim;
                    }
                }
            }
            else
            {
                if (!female)
                {
                    if (EquipTable.elven_male.Contains(animId))
                    {
                        EquipTableEntry entry = (EquipTableEntry)EquipTable.elven_male[animId];
                        gumpid = entry.m_NewID;
                        hue = entry.m_NewHue;
                        animId = entry.m_NewAnim;
                    }
                }
                else
                {
                    if (EquipTable.elven_female.Contains(animId))
                    {
                        EquipTableEntry entry = (EquipTableEntry)EquipTable.elven_female[animId];
                        gumpid = entry.m_NewID;
                        hue = entry.m_NewHue;
                        animId = entry.m_NewAnim;
                    }
                }
            }
        }

        private void OnChangeSort(object sender, EventArgs e)
        {
            if (LayerSort.Checked)
                treeViewItems.TreeViewNodeSorter=new LayerSorter();
            else
                treeViewItems.TreeViewNodeSorter=new ObjtypeSorter();
        }

        private void OnClick_ChangeDisplay(object sender, EventArgs e)
        {
            showPD = !showPD;
            if (showPD)
            {
                DressPic.BackColor = Color.Black;
                groupBoxAnimate.Visible = false;
                animateToolStripMenuItem.Visible = false;
                showAnimationToolStripMenuItem.Text = "Show Animation";
            }
            else
            {
                DressPic.BackColor = Color.WhiteSmoke;
                groupBoxAnimate.Visible = true;
                animateToolStripMenuItem.Visible = true;
                showAnimationToolStripMenuItem.Text = "Show Paperdoll";
            }
            RefreshDrawing();
        }

        private void OnClick_Animate(object sender, EventArgs e)
        {
            animate = !animate;
            RefreshDrawing();
        }

        private void RefreshDrawing()
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
		public static Hashtable m_Entries;

		static GumpTable()
        {
            string path = Client.GetFilePath("gump.def");

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
                        int index1 = line.IndexOf(" {");
                        int index2 = line.IndexOf("} ");

                        string param1 = line.Substring(0, index1);
                        string param2 = line.Substring(index1 + 2, index2 - index1 - 2);
                        string param3 = line.Substring(index2 + 2);

                        int iParam1 = Convert.ToInt32(param1);
                        int iParam2 = Convert.ToInt32(param2);
                        int iParam3 = Convert.ToInt32(param3);

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
        public int m_OldID;
        public int m_NewID;
        public int m_NewHue;

        public GumpTableEntry(int oldID, int newID, int newHue)
        {
            m_OldID = oldID;
            m_NewID = newID;
            m_NewHue = newHue;
        }
    }

    public class EquipTable
    {
        public static Hashtable human_male;
        public static Hashtable human_female;
        public static Hashtable elven_male;
        public static Hashtable elven_female;

        static EquipTable()
        {
            string path = Client.GetFilePath("equipconv.def");

            if (path == null)
                return;

            human_male = new Hashtable();
            human_female = new Hashtable();
            elven_male = new Hashtable();
            elven_female = new Hashtable();
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
                            human_male[animID] = entry;
                        else if (bodytype == 401)
                            human_female[animID] = entry;
                        else if (bodytype == 605)
                            elven_male[animID] = entry;
                        else if (bodytype == 606)
                            elven_female[animID] = entry;
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
        public int m_NewID;
        public int m_NewHue;
        public int m_NewAnim;

        public EquipTableEntry(int newID, int newHue, int newAnim)
        {
            m_NewID = newID;
            m_NewHue = newHue;
            m_NewAnim = newAnim;
        }
    }
}
