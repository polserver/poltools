using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ultima;
using System.Xml;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;

namespace Controls
{
    public partial class MobList : UserControl
    {
        public MobList()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            
        }
        private string[][] AnimNames = {
            new string[]{"Walk","Idle","Die1","Die2","Attack1","Attack2","Attack3","AttackBow","AttackCrossBow",
                         "AttackThrow","GetHit","Pillage","Stomp","Cast2","Cast3","BlockRight","BlockLeft","Idle",
                         "Fidget","Fly","TakeOff","GetHitInAir"}, //Monster
            new string[]{"Walk","Run","Idle","Idle","Fidget","Attack1","Attack2","GetHit","Die1"},//sea
            new string[]{"Walk","Run","Idle","Eat","Alert","Attack1","Attack2","GetHit","Die1","Idle","Fidget",
                         "LieDown","Die2"},//animal
            new string[]{"Walk_01","WalkStaff_01","Run_01","RunStaff_01","Idle_01","Idle_01",
                         "Fidget_Yawn_Stretch_01","CombatIdle1H_01","CombatIdle1H_01","AttackSlash1H_01",
                         "AttackPierce1H_01","AttackBash1H_01","AttackBash2H_01","AttackSlash2H_01",
                         "AttackPierce2H_01","CombatAdvance_1H_01","Spell1","Spell2","AttackBow_01",
                         "AttackCrossbow_01","GetHit_Fr_Hi_01","Die_Hard_Fwd_01","Die_Hard_Back_01",
                         "Horse_Walk_01","Horse_Run_01","Horse_Idle_01","Horse_Attack1H_SlashRight_01",
                         "Horse_AttackBow_01","Horse_AttackCrossbow_01","Horse_Attack2H_SlashRight_01",
                         "Block_Shield_Hard_01","Punch_Punch_Jab_01","Bow_Lesser_01","Salute_Armed1h_01",
                         "Ingest_Eat_01"}//human
        };


        private Bitmap m_MainPicture;
        private Bitmap bmp;
        private int m_CurrentSelect = 0;
        private int m_CurrentSelectAction = 0;
        private bool m_Animate = false;
        private int m_FrameIndex;
        private Bitmap[] m_Animation;
        private bool m_ImageInvalidated = true;
        private Timer m_Timer = null;
        private Frame[] frames;
        private static Brush borderBrush = Brushes.Gray;
        private Pen borderPen = new Pen(borderBrush);
        private int customHue = 0;
        private int DefHue = 0;
        private int facing = 1;
        private bool sortalpha = false;

        private void OnLoad(object sender, EventArgs e)
        {
            if (!LoadXml())
                return;

            LoadListView();

            m_CurrentSelect = 0;
            m_CurrentSelectAction = 0;
            TreeViewMobs.SelectedNode = TreeViewMobs.Nodes[0];
            FacingBar.Value = (facing + 3) & 7;
        }

        public void ChangeHue(int select)
        {
            customHue = select+1;
            CurrentSelect = CurrentSelect;
        }
        private bool Animate
        {
            get { return m_Animate; }
            set
            {
                if (m_Animate != value)
                {
                    m_Animate = value;
                    StopAnimation();
                    m_ImageInvalidated = true;
                    MainPictureBox.Refresh();
                }
            }
        }

        private void StopAnimation()
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
        }

        private int CurrentSelect
        {
            get { return m_CurrentSelect; }
            set
            {
                m_CurrentSelect = value;
                if (m_Timer != null)
                {
                    if (m_Timer.Enabled)
                        m_Timer.Stop();

                    m_Timer.Dispose();
                    m_Timer = null;
                }
                SetPicture();
                MainPictureBox.Refresh();
            }
        }

        private void SetPicture()
        {
            frames = null;
            if (m_MainPicture != null)
                m_MainPicture.Dispose();
            if (m_CurrentSelect != 0)
            {
                if (Animate)
                    m_MainPicture = DoAnimation();
                else
                {
                    int body = m_CurrentSelect;
                    Animations.Translate(ref body);
                    int hue = customHue;
                    if (hue != 0)
                        frames = Animations.GetAnimation(m_CurrentSelect, m_CurrentSelectAction, facing, ref hue, true, false);
                    else
                    {
                        frames = Animations.GetAnimation(m_CurrentSelect, m_CurrentSelectAction, facing, ref hue, false, false);
                        DefHue = hue;
                    }

                    if (frames != null)
                    {
                        if (frames[0].Bitmap != null)
                        {
                            m_MainPicture = new Bitmap(frames[0].Bitmap);
                            BaseGraphicLabel.Text = "BaseGraphic: " + body.ToString();
                            GraphicLabel.Text = "Graphic: " + m_CurrentSelect.ToString() + String.Format("(0x{0:X})", m_CurrentSelect);
                            HueLabel.Text = "Hue: " + (hue + 1).ToString();
                        }
                        else
                            m_MainPicture = null;
                    }
                    else
                        m_MainPicture = null;
                }
            }
        }

        private Bitmap DoAnimation()
        {
            if (m_Timer == null)
            {
                int body = m_CurrentSelect;
                Animations.Translate(ref body);
                int hue = customHue;
                if (hue != 0)
                    frames = Animations.GetAnimation(m_CurrentSelect, m_CurrentSelectAction, facing, ref hue, true, false);
                else
                {
                    frames = Animations.GetAnimation(m_CurrentSelect, m_CurrentSelectAction, facing, ref hue, false, false);
                    DefHue = hue;
                }

                if (frames == null)
                    return null;
                BaseGraphicLabel.Text = "BaseGraphic: " + body.ToString();
                GraphicLabel.Text = "Graphic: " + m_CurrentSelect.ToString() + String.Format("(0x{0:X})", m_CurrentSelect);
                HueLabel.Text = "Hue: " + (hue + 1).ToString();
                int count = frames.Length;
                m_Animation = new Bitmap[count];

                for (int i = 0; i < count; i++)
                {
                    m_Animation[i] = frames[i].Bitmap;
                }

                m_Timer = new Timer();
                m_Timer.Interval = 1000 / count;
                m_Timer.Tick += new EventHandler(AnimTick);
                m_Timer.Start();

                m_FrameIndex = 0;
                LoadListViewFrames(); // FrameTab neuladen
                if (m_Animation[0] != null)
                    return new Bitmap(m_Animation[0]);
                else
                    return null;
            }
            else
            {
                if (m_Animation[m_FrameIndex] != null)
                    return new Bitmap(m_Animation[m_FrameIndex]);
                else
                    return null;
            }
        }

        private void AnimTick(object sender, EventArgs e)
        {
            m_FrameIndex++;

            if (m_FrameIndex == m_Animation.Length)
                m_FrameIndex = 0;

            m_ImageInvalidated = true;
            MainPictureBox.Refresh();
        }

        private void OnPaint_MainPicture(object sender, PaintEventArgs e)
        {
            if (m_ImageInvalidated)
                SetPicture();
            if (m_MainPicture != null)
            {
                Point location = Point.Empty;
                Size size = Size.Empty;
                size = m_MainPicture.Size;
                location.X = (MainPictureBox.Width - m_MainPicture.Width) / 2;
                location.Y = (MainPictureBox.Height - m_MainPicture.Height) / 2;

                Rectangle destRect = new Rectangle(location, size);

                e.Graphics.DrawImage(m_MainPicture, destRect, 0, 0, m_MainPicture.Width, m_MainPicture.Height, System.Drawing.GraphicsUnit.Pixel);
            }
            else
                m_MainPicture = null;
        }

        private void TreeViewMobs_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                m_CurrentSelectAction = (int)e.Node.Tag;
                CurrentSelect = (int)e.Node.Parent.Tag;
            }
            else
            {
                m_CurrentSelectAction = 0;
                CurrentSelect = (int)e.Node.Tag;
            }
        }

        private void Animate_Click(object sender, EventArgs e)
        {
            m_Animate = !m_Animate;
        }


        private bool LoadXml()
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            string FileName = Path.Combine(path, "UoViewer.xml");
            if (!(File.Exists(FileName)))
                return false;
            TreeViewMobs.BeginUpdate();
            TreeViewMobs.Nodes.Clear();
            XmlDocument dom = new XmlDocument();
            dom.Load(FileName);
            XmlElement xMobs = dom["Mobs"];
            TreeNode node;
            foreach (XmlElement xMob in xMobs.SelectNodes("Mob"))
            {
                string name;
                int value;
                name = xMob.GetAttribute("name");
                value = int.Parse(xMob.GetAttribute("body"));
                node = new TreeNode(name);
                node.Tag = value;
                TreeViewMobs.Nodes.Add(node);
                int type = int.Parse(xMob.GetAttribute("type"));

                for (int i = 0; i < AnimNames[type].GetLength(0); i += 1)
                {
                    if (Ultima.Animations.IsActionDefined(value, i, 0, 0, false))
                    {
                        node = new TreeNode(i.ToString() + " " + AnimNames[type][i]);
                        node.Tag = i;
                        TreeViewMobs.Nodes[TreeViewMobs.Nodes.Count - 1].Nodes.Add(node);
                    }
                }
            }
            TreeViewMobs.EndUpdate();
            return true;
        }

        private void LoadListView()
        {
            listView.BeginUpdate();
            listView.Clear();
            ListViewItem item;
            foreach (TreeNode node in TreeViewMobs.Nodes)
            {
                item = new ListViewItem("("+node.Tag+")", 0);
                item.Tag = node.Tag;
                listView.Items.Add(item);
            }
            listView.EndUpdate();
        }

        private void selectChanged_listView(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
                TreeViewMobs.SelectedNode = TreeViewMobs.Nodes[listView.SelectedItems[0].Index];
        }

        private void listView_DoubleClick(object sender, MouseEventArgs e)
        {
            tabControl1.SelectTab(tabPage1);
        }

        private void extract_Image_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, String.Format("Mob {0}.jpg", m_CurrentSelect));

            if (Animate)
            {
                Bitmap newbit = new Bitmap(m_Animation[0].Width, m_Animation[0].Height);
                Graphics newgraph = Graphics.FromImage(newbit);
                newgraph.FillRectangle(Brushes.White, 0, 0, newbit.Width, newbit.Height);
                newgraph.DrawImage(m_Animation[0],new Point(0,0));
                newgraph.Save();
                newbit.Save(FileName, ImageFormat.Jpeg);
            }
            else
            {
                Bitmap newbit = new Bitmap(m_MainPicture.Width, m_MainPicture.Height);
                Graphics newgraph = Graphics.FromImage(newbit);
                newgraph.FillRectangle(Brushes.White, 0, 0, newbit.Width, newbit.Height);
                newgraph.DrawImage(m_MainPicture,new Point(0,0));
                newgraph.Save();
                newbit.Save(FileName, ImageFormat.Jpeg);
            }
        }

        private void listViewdrawItem(object sender, DrawListViewItemEventArgs e)
        {
            int graphic = (int)e.Item.Tag;
            int hue = 0;
            frames = Animations.GetAnimation(graphic, 0, 1, ref hue, false, true);

            if (frames == null)
                return;
            bmp = frames[0].Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;

            if (width > e.Bounds.Width)
                width = e.Bounds.Width;

            if (height > e.Bounds.Height)
                height = e.Bounds.Height;

            e.Graphics.DrawImage(bmp, e.Bounds.X, e.Bounds.Y, width, height);
            e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter);
            if (listView.SelectedItems.Contains(e.Item))
                e.DrawFocusRectangle();
            else
                e.Graphics.DrawRectangle(borderPen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
        }

        private HuePopUp showform = null;
        private void OnClick_Hue(object sender, EventArgs e)
        {
            if ((showform == null) || (showform.IsDisposed))
            {
                if (customHue==0)
                    showform = new HuePopUp(this,DefHue+1);
                else
                    showform = new HuePopUp(this, customHue-1);
                showform.Show();
            }
        }

        private void LoadListViewFrames()
        {
            listView1.BeginUpdate();
            listView1.Clear();
            ListViewItem item;
            for (int frame = 0; frame < m_Animation.Length;frame++ )
            {
                item = new ListViewItem(frame.ToString(),0);
                item.Tag = frame;
                listView1.Items.Add(item);
            }
            listView1.EndUpdate();
        }
        private void Frames_ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            bmp = m_Animation[(int)e.Item.Tag];
            int width = bmp.Width;
            int height = bmp.Height;

            if (width > e.Bounds.Width)
                width = e.Bounds.Width;

            if (height > e.Bounds.Height)
                height = e.Bounds.Height;

            e.Graphics.DrawImage(bmp, e.Bounds.X, e.Bounds.Y, width, height);
            e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter);
            e.Graphics.DrawRectangle(borderPen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
        }

        private void OnScrollFacing(object sender, EventArgs e)
        {
            facing = (FacingBar.Value - 3) & 7;
            CurrentSelect = CurrentSelect;
        }

        private void OnClick_Sort(object sender, EventArgs e)
        {
            sortalpha = !sortalpha;
            if (!sortalpha)
                TreeViewMobs.TreeViewNodeSorter = new GraphicSorter();
            else
                TreeViewMobs.TreeViewNodeSorter = new AlphaSorter();
            TreeViewMobs.Sort();
            LoadListView();
        }
    }

    public class AlphaSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;
            return string.Compare(tx.Text, ty.Text);
        }
    }

    public class GraphicSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;
            if ((int)tx.Tag == (int)ty.Tag)
                return 0;
            else if ((int)tx.Tag < (int)ty.Tag)
                return -1;
            else
                return 1;
        }
    }
}
