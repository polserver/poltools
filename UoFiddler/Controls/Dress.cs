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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Ultima;

namespace FiddlerControls
{
    public partial class Dress : UserControl
    {
        public Dress()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            FiddlerControls.Events.FilePathChangeEvent += OnFilePathChangeEvent;
        }

        private static readonly int[] Draworder ={
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

        private static readonly int[] Draworder2 ={
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

        private readonly Point _drawpoint = new Point(0, 0);
        private Point _drawpointAni = new Point(100, 100);

        private object[] _layers = new object[25];
        private readonly bool[] _layervisible = new bool[25];
        private bool _female;
        private bool _elve;
        private bool _showPd = true;
        private bool _animate;
        private Timer _mTimer;
        private Bitmap[] _mAnimation;
        private int _mFrameIndex;
        private int _facing = 1;
        private int _action = 1;
        private bool _loaded;
        private readonly int[] _hues = new int[25];
        private int _mount;

        public void SetHue(int index, int color)
        {
            _hues[index] = color;
        }

        /// <summary>
        /// Reload when loaded
        /// </summary>
        private void Reload()
        {
            if (!_loaded)
            {
                return;
            }

            _loaded = false;
            _layers = new object[25];
            _female = false;
            _elve = false;
            _showPd = true;
            _animate = false;
            _facing = 1;
            _action = 1;
            if (_mTimer != null)
            {
                if (_mTimer.Enabled)
                {
                    _mTimer.Stop();
                }

                _mTimer.Dispose();
                _mTimer = null;
            }

            if (_mAnimation != null)
            {
                for (int i = 0; i < _mAnimation.Length; ++i)
                {
                    _mAnimation[i]?.Dispose();
                }
            }

            _mAnimation = null;
            _mFrameIndex = 0;
            EquipTable.Initialize();
            GumpTable.Initialize();
            OnLoad(this, EventArgs.Empty);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Options.LoadedUltimaClass["TileData"] = true;
            Options.LoadedUltimaClass["Art"] = true;
            Options.LoadedUltimaClass["Hues"] = true;
            Options.LoadedUltimaClass["Animations"] = true;
            Options.LoadedUltimaClass["Gumps"] = true;

            extractAnimationToolStripMenuItem.Visible = false;

            DressPic.Image = new Bitmap(DressPic.Width, DressPic.Height);
            pictureBoxDress.Image = new Bitmap(pictureBoxDress.Width, pictureBoxDress.Height);

            checkedListBoxWear.BeginUpdate();
            checkedListBoxWear.Items.Clear();
            for (int i = 0; i < _layers.Length; ++i)
            {
                _layers[i] = 0;
                checkedListBoxWear.Items.Add($"0x{i:X2}", true);
                _layervisible[i] = true;
            }
            checkedListBoxWear.EndUpdate();

            groupBoxAnimate.Visible = false;
            animateToolStripMenuItem.Visible = false;
            FacingBar.Value = (_facing + 3) & 7;
            ActionBar.Value = _action;
            toolTip1.SetToolTip(FacingBar, FacingBar.Value.ToString());
            BuildDressList();
            DrawPaperdoll();
            _loaded = true;
            Cursor.Current = Cursors.Default;
        }

        private void OnFilePathChangeEvent()
        {
            Reload();
        }

        private void DrawPaperdoll()
        {
            if (!_showPd)
            {
                DrawAnimation();
                return;
            }
            using (Graphics graphpic = Graphics.FromImage(DressPic.Image))
            {
                graphpic.Clear(Color.Black);
                if (_layervisible[0])
                {
                    Bitmap background = !_female ? !_elve ? Gumps.GetGump(0xC) : Gumps.GetGump(0xE) : !_elve ? Gumps.GetGump(0xD) : Gumps.GetGump(0xF);
                    if (background != null)
                    {
                        if (_hues[0] > 0)
                        {
                            Bitmap b = new Bitmap(background);
                            int hue = _hues[0];
                            bool gray = false;
                            if ((hue & 0x8000) != 0)
                            {
                                hue ^= 0x8000;
                                gray = true;
                            }
                            Ultima.Hues.List[hue].ApplyTo(b, gray);
                            background = b;
                        }
                        graphpic.DrawImage(background, _drawpoint);
                    }
                }
                for (int i = 1; i < Draworder.Length; ++i)
                {
                    if ((int)_layers[Draworder[i]] == 0)
                    {
                        continue;
                    }

                    if (!_layervisible[Draworder[i]])
                    {
                        continue;
                    }

                    int ani = TileData.ItemTable[(int)_layers[Draworder[i]]].Animation;
                    int gump = ani + 50000;
                    int hue = 0;

                    ConvertBody(ref ani, ref gump, ref hue);

                    if (_female)
                    {
                        gump += 10000;
                        if (!Gumps.IsValidIndex(gump))  // female gump.def entry?
                        {
                            ConvertGump(ref gump, ref hue);
                        }

                        if (!Gumps.IsValidIndex(gump)) // nope so male gump
                        {
                            gump -= 10000;
                        }
                    }

                    if (!Gumps.IsValidIndex(gump)) // male (or invalid female)
                    {
                        ConvertGump(ref gump, ref hue);
                    }

                    if (!Gumps.IsValidIndex(gump))
                    {
                        continue;
                    }

                    using (Bitmap bmp = new Bitmap(Gumps.GetGump(gump)))
                    {
                        if (_hues[Draworder[i]] > 0)
                        {
                            hue = _hues[Draworder[i]];
                        }

                        bool onlyHueGrayPixels = (hue & 0x8000) != 0;
                        hue = (hue & 0x3FFF) - 1;
                        if (hue >= 0 && hue < Ultima.Hues.List.Length)
                        {
                            Hue hueObject = Ultima.Hues.List[hue];
                            hueObject.ApplyTo(bmp, onlyHueGrayPixels);
                        }
                        graphpic.DrawImage(bmp, _drawpoint);
                    }
                }
            }
            DressPic.Invalidate();
        }

        private void DrawAnimation()
        {
            if (_animate)
            {
                DoAnimation();
                return;
            }
            using (Graphics graphPic = Graphics.FromImage(DressPic.Image))
            {
                graphPic.Clear(Color.WhiteSmoke);
                int hue = 0;
                int back = 0;
                if (_layervisible[0])
                {
                    back = !_female ? !_elve ? 400 : 605 : !_elve ? 401 : 606;
                }
                Frame[] background;
                if (_hues[0] > 0)
                {
                    hue = _hues[0];
                    background = Animations.GetAnimation(back, _action, _facing, ref hue, true, true);
                }
                else
                {
                    background = Animations.GetAnimation(back, _action, _facing, ref hue, false, true);
                }

                Point draw = new Point();
                if (_mount != 0)
                {
                    if (_action >= 23 && _action <= 29) //mount animations
                    {
                        int mountaction;
                        switch (_action)
                        {
                            case 23:
                                mountaction = 0;
                                break;
                            case 24:
                                mountaction = 1;
                                break;
                            case 25:
                                mountaction = 2;
                                break;
                            default:
                                mountaction = 5;
                                break;
                        }
                        if (Animations.IsActionDefined(_mount, mountaction, _facing))
                        {
                            hue = 0;
                            Frame[] mountframe = Animations.GetAnimation(_mount, mountaction, _facing, ref hue, false, false);
                            if (mountframe.Length > 0 && mountframe[0].Bitmap != null)
                            {
                                draw.X = _drawpointAni.X - mountframe[0].Center.X;
                                draw.Y = _drawpointAni.Y - mountframe[0].Center.Y - mountframe[0].Bitmap.Height;
                                graphPic.DrawImage(mountframe[0].Bitmap, draw);
                            }
                        }
                    }
                }
                if (background != null)
                {
                    draw.X = _drawpointAni.X - background[0].Center.X;
                    draw.Y = _drawpointAni.Y - background[0].Center.Y - background[0].Bitmap.Height;
                    graphPic.DrawImage(background[0].Bitmap, draw);
                }
                int[] animorder = Draworder2;
                if (((_facing - 3) & 7) >= 4 && ((_facing - 3) & 7) <= 6)
                {
                    animorder = Draworder;
                }

                for (int i = 1; i < Draworder.Length; ++i)
                {
                    if ((int)_layers[animorder[i]] != 0)
                    {
                        if (_layervisible[animorder[i]])
                        {
                            if (TileData.ItemTable == null)
                            {
                                break;
                            }

                            int ani = TileData.ItemTable[(int)_layers[animorder[i]]].Animation;
                            int gump = ani + 50000;
                            hue = 0;
                            ConvertBody(ref ani, ref gump, ref hue);
                            if (!Animations.IsActionDefined(ani, _action, _facing))
                            {
                                continue;
                            }

                            Frame[] frames;
                            if (_hues[animorder[i]] > 0)
                            {
                                hue = _hues[animorder[i]];
                                frames = Animations.GetAnimation(ani, _action, _facing, ref hue, true, true);
                            }
                            else
                            {
                                frames = Animations.GetAnimation(ani, _action, _facing, ref hue, false, true);
                            }

                            Bitmap bmp = frames[0].Bitmap;
                            if (bmp == null)
                            {
                                continue;
                            }

                            draw.X = _drawpointAni.X - frames[0].Center.X;
                            draw.Y = _drawpointAni.Y - frames[0].Center.Y - frames[0].Bitmap.Height;

                            graphPic.DrawImage(bmp, draw);
                        }
                    }
                }
            }
            DressPic.Invalidate();
        }

        private void DoAnimation()
        {
            if (_mTimer == null)
            {
                int hue = 0;
                int back = !_female ? !_elve ? 400 : 605 : !_elve ? 401 : 606;
                Frame[] mobile;
                if (_hues[0] > 0)
                {
                    hue = _hues[0];
                    mobile = Animations.GetAnimation(back, _action, _facing, ref hue, true, false);
                }
                else
                {
                    mobile = Animations.GetAnimation(back, _action, _facing, ref hue, false, false);
                }

                Point draw = new Point();

                int count = mobile.Length;
                _mAnimation = new Bitmap[count];
                int[] animorder = Draworder2;
                if (((_facing - 3) & 7) >= 4 && ((_facing - 3) & 7) <= 6)
                {
                    animorder = Draworder;
                }

                for (int i = 0; i < count; ++i)
                {
                    _mAnimation[i] = new Bitmap(DressPic.Width, DressPic.Height);
                    using (Graphics graph = Graphics.FromImage(_mAnimation[i]))
                    {
                        graph.Clear(Color.WhiteSmoke);
                        if (_mount != 0)
                        {
                            if (_action >= 23 && _action <= 29) //mount animations
                            {
                                int mountaction;
                                switch (_action)
                                {
                                    case 23:
                                        mountaction = 0;
                                        break;
                                    case 24:
                                        mountaction = 1;
                                        break;
                                    case 25:
                                        mountaction = 2;
                                        break;
                                    default:
                                        mountaction = 5;
                                        break;
                                }
                                if (Animations.IsActionDefined(_mount, mountaction, _facing))
                                {
                                    hue = 0;
                                    Frame[] mountframe = Animations.GetAnimation(_mount, mountaction, _facing, ref hue, false, false);
                                    if (mountframe.Length > i && mountframe[i].Bitmap != null)
                                    {
                                        draw.X = _drawpointAni.X - mountframe[i].Center.X;
                                        draw.Y = _drawpointAni.Y - mountframe[i].Center.Y - mountframe[i].Bitmap.Height;
                                        graph.DrawImage(mountframe[i].Bitmap, draw);
                                    }
                                }
                            }
                        }
                        draw.X = _drawpointAni.X - mobile[i].Center.X;
                        draw.Y = _drawpointAni.Y - mobile[i].Center.Y - mobile[i].Bitmap.Height;
                        graph.DrawImage(mobile[i].Bitmap, draw);
                        for (int j = 1; j < animorder.Length; ++j)
                        {
                            if ((int)_layers[animorder[j]] != 0)
                            {
                                if (_layervisible[animorder[j]])
                                {
                                    int ani = TileData.ItemTable[(int)_layers[animorder[j]]].Animation;
                                    int gump = ani + 50000;
                                    hue = 0;
                                    ConvertBody(ref ani, ref gump, ref hue);
                                    if (!Animations.IsActionDefined(ani, _action, _facing))
                                    {
                                        continue;
                                    }

                                    Frame[] frames;
                                    if (_hues[animorder[j]] > 0)
                                    {
                                        hue = _hues[animorder[j]];
                                        frames = Animations.GetAnimation(ani, _action, _facing, ref hue, true, false);
                                    }
                                    else
                                    {
                                        frames = Animations.GetAnimation(ani, _action, _facing, ref hue, false, false);
                                    }

                                    if (frames.Length < i || frames[i].Bitmap == null)
                                    {
                                        continue;
                                    }

                                    draw.X = _drawpointAni.X - frames[i].Center.X;
                                    draw.Y = _drawpointAni.Y - frames[i].Center.Y - frames[i].Bitmap.Height;

                                    graph.DrawImage(frames[i].Bitmap, draw);
                                }
                            }
                        }
                    }
                }
                _mFrameIndex = 0;
                _mTimer = new Timer
                {
                    Interval = 150// 1000 / count;
                };
                _mTimer.Tick += AnimTick;
                _mTimer.Start();
            }
        }

        private void AnimTick(object sender, EventArgs e)
        {
            ++_mFrameIndex;

            if (_mFrameIndex >= _mAnimation.Length)
            {
                _mFrameIndex = 0;
            }

            if (_mAnimation?[_mFrameIndex] == null)
            {
                return;
            }

            using (Graphics graph = Graphics.FromImage(DressPic.Image))
            {
                graph.DrawImage(_mAnimation[_mFrameIndex], _drawpoint);
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
            if (_female)
            {
                gump += 10000;
                if (!Gumps.IsValidIndex(gump))  // female gump.def entry?
                {
                    ConvertGump(ref gump, ref hue);
                }

                if (!Gumps.IsValidIndex(gump)) // nope so male gump
                {
                    gump -= 10000;
                }
            }

            if (!Gumps.IsValidIndex(gump)) // male (or invalid female)
            {
                ConvertGump(ref gump, ref hue);
            }

            using (Graphics graph = Graphics.FromImage(pictureBoxDress.Image))
            {
                graph.Clear(Color.Transparent);
                Bitmap bmp = Gumps.GetGump(gump);
                if (bmp != null)
                {
                    bool onlyHueGrayPixels = (hue & 0x8000) != 0;
                    hue = (hue & 0x3FFF) - 1;
                    if (hue >= 0 && hue < Ultima.Hues.List.Length)
                    {
                        Hue hueObject = Ultima.Hues.List[hue];
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
            TextBox.AppendText(
                $"Objtype: 0x{(int)e.Node.Tag:X4}  Layer: 0x{TileData.ItemTable[(int)e.Node.Tag].Quality:X2}\n");
            TextBox.AppendText($"GumpID: 0x{gump:X4} (0x{gumporig:X4}) Hue: {hue + 1}\n");
            TextBox.AppendText($"Animation: 0x{ani:X4} (0x{TileData.ItemTable[(int)e.Node.Tag].Animation:X4})\n");
            TextBox.AppendText(
                $"ValidGump: {Gumps.IsValidIndex(gump)} ValidAnim: {Animations.IsActionDefined(ani, 0, 0)}\n");
            TextBox.AppendText(
                $"ValidLayer: {Array.IndexOf(Draworder, TileData.ItemTable[(int)e.Node.Tag].Quality) != -1}");
        }

        private void OnClick_Animate(object sender, EventArgs e)
        {
            _animate = !_animate;
            extractAnimationToolStripMenuItem.Visible = _animate;
            RefreshDrawing();
        }

        private void OnChangeFemale(object sender, EventArgs e)
        {
            _female = !_female;
            RefreshDrawing();
        }

        private void OnChangeElve(object sender, EventArgs e)
        {
            _elve = !_elve;
            RefreshDrawing();
        }

        private void OnClick_Dress(object sender, EventArgs e)
        {
            if (treeViewItems.SelectedNode == null)
            {
                return;
            }

            int objType = (int)treeViewItems.SelectedNode.Tag;
            int layer = TileData.ItemTable[objType].Quality;
            if (Array.IndexOf(Draworder, layer) == -1)
            {
                return;
            }

            _layers[layer] = objType;
            checkedListBoxWear.BeginUpdate();
            checkedListBoxWear.Items[layer] = $"0x{layer:X2} {TileData.ItemTable[objType].Name}";
            checkedListBoxWear.EndUpdate();
            RefreshDrawing();
        }

        private void OnClick_UnDress(object sender, EventArgs e)
        {
            if (checkedListBoxWear.SelectedIndex == -1)
            {
                return;
            }

            int layer = checkedListBoxWear.SelectedIndex;
            checkedListBoxWear.Items[checkedListBoxWear.SelectedIndex] = $"0x{layer:X2}";
            _layers[layer] = 0;
            RefreshDrawing();
        }

        private void OnClickUndressAll(object sender, EventArgs e)
        {
            for (int i = 0; i < _layers.Length; ++i)
            {
                _layers[i] = 0;
                checkedListBoxWear.Items[i] = $"0x{i:X2}";
            }

            RefreshDrawing();
        }

        private void CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_loaded)
            {
                _layervisible[e.Index] = e.NewValue == CheckState.Checked;
                RefreshDrawing();
            }
        }

        private void CheckedListBox_Change(object sender, EventArgs e)
        {
            //RefreshDrawing();
            if (checkedListBoxWear.SelectedIndex == -1)
            {
                return;
            }

            int layer = checkedListBoxWear.SelectedIndex;
            int objtype = (int)_layers[layer];
            int ani = TileData.ItemTable[objtype].Animation;
            int gumpidorig = ani + 50000;
            int gumpid = gumpidorig;
            int hue = 0;
            Animations.Translate(ref ani);
            ConvertBody(ref ani, ref gumpid, ref hue);
            if (_female)
            {
                gumpid += 10000;
                if (!Gumps.IsValidIndex(gumpid))  // female gump.def entry?
                {
                    ConvertGump(ref gumpid, ref hue);
                }

                if (!Gumps.IsValidIndex(gumpid)) // nope so male gump
                {
                    gumpid -= 10000;
                }
            }

            if (!Gumps.IsValidIndex(gumpid)) // male (or invalid female)
            {
                ConvertGump(ref gumpid, ref hue);
            }

            TextBox.Clear();
            TextBox.AppendText($"Objtype: 0x{objtype:X4}  Layer: 0x{layer:X2}\n");
            TextBox.AppendText($"GumpID: 0x{gumpid:X4} (0x{gumpidorig:X4}) Hue: {hue}\n");
            TextBox.AppendText($"Animation: 0x{ani:X4} (0x{TileData.ItemTable[objtype].Animation:X4})\n");
            TextBox.AppendText(
                $"ValidGump: {Gumps.IsValidIndex(gumpid)} ValidAnim: {Animations.IsActionDefined(ani, 0, 0)}");
        }

        private void OnChangeSort(object sender, EventArgs e)
        {
            treeViewItems.TreeViewNodeSorter = LayerSort.Checked ? new LayerSorter() : (IComparer)new ObjTypeSorter();
        }

        private void OnClick_ChangeDisplay(object sender, EventArgs e)
        {
            _showPd = !_showPd;
            if (_showPd)
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

        private void BuildDressList()
        {
            treeViewItems.BeginUpdate();
            treeViewItems.Nodes.Clear();

            if (TileData.ItemTable != null)
            {
                for (int i = 0; i < TileData.ItemTable.Length; ++i)
                {
                    if (!TileData.ItemTable[i].Wearable)
                    {
                        continue;
                    }

                    int ani = TileData.ItemTable[i].Animation;
                    if (ani == 0)
                    {
                        continue;
                    }

                    int hue = 0;
                    int gump = ani + 50000;
                    ConvertBody(ref ani, ref gump, ref hue);
                    if (!Gumps.IsValidIndex(gump))
                    {
                        ConvertGump(ref gump, ref hue);
                    }

                    bool hasAnimation = Animations.IsActionDefined(ani, 0, 0);
                    bool hasGump = Gumps.IsValidIndex(gump);
                    TreeNode node = new TreeNode(
                        $"0x{i:X4} (0x{TileData.ItemTable[i].Quality:X2}) {TileData.ItemTable[i].Name}")
                    {
                        Tag = i
                    };

                    if (Array.IndexOf(Draworder, TileData.ItemTable[i].Quality) == -1)
                    {
                        node.ForeColor = Color.DarkRed;
                    }
                    else if (!hasAnimation)
                    {
                        node.ForeColor = !hasGump ? Color.Red : Color.Orange;
                    }
                    else if (hasAnimation && !hasGump)
                    {
                        node.ForeColor = Color.Blue;
                    }

                    treeViewItems.Nodes.Add(node);
                }
            }
            treeViewItems.EndUpdate();
        }

        public void RefreshDrawing()
        {
            if (_mTimer != null)
            {
                if (_mTimer.Enabled)
                {
                    _mTimer.Stop();
                }

                _mTimer.Dispose();
                _mTimer = null;
            }

            if (_mAnimation != null)
            {
                for (int i = 0; i < _mAnimation.Length; ++i)
                {
                    _mAnimation[i]?.Dispose();
                }
            }

            _mAnimation = null;
            _mFrameIndex = 0;

            DrawPaperdoll();
        }

        private void OnScroll_Facing(object sender, EventArgs e)
        {
            _facing = (FacingBar.Value - 3) & 7;
            toolTip1.SetToolTip(FacingBar, FacingBar.Value.ToString());
            RefreshDrawing();
        }

        private void OnScroll_Action(object sender, EventArgs e)
        {
            string[] tip = new[]{"Walk_01","WalkStaff_01","Run_01","RunStaff_01","Idle_01","Idle_01",
                         "Fidget_Yawn_Stretch_01","CombatIdle1H_01","CombatIdle1H_01","AttackSlash1H_01",
                         "AttackPierce1H_01","AttackBash1H_01","AttackBash2H_01","AttackSlash2H_01",
                         "AttackPierce2H_01","CombatAdvance_1H_01","Spell1","Spell2","AttackBow_01",
                         "AttackCrossbow_01","GetHit_Fr_Hi_01","Die_Hard_Fwd_01","Die_Hard_Back_01",
                         "Horse_Walk_01","Horse_Run_01","Horse_Idle_01","Horse_Attack1H_SlashRight_01",
                         "Horse_AttackBow_01","Horse_AttackCrossbow_01","Horse_Attack2H_SlashRight_01",
                         "Block_Shield_Hard_01","Punch_Punch_Jab_01","Bow_Lesser_01","Salute_Armed1h_01",
                         "Ingest_Eat_01"};
            toolTip1.SetToolTip(ActionBar, ActionBar.Value + " " + tip[ActionBar.Value]);
            _action = ActionBar.Value;
            RefreshDrawing();
        }

        private void OnResizepictureDress(object sender, EventArgs e)
        {
            if (treeViewItems.SelectedNode == null)
            {
                return;
            }

            pictureBoxDress.Image = new Bitmap(pictureBoxDress.Width, pictureBoxDress.Height);
            AfterSelectTreeView(this, new TreeViewEventArgs(treeViewItems.SelectedNode));
        }

        private void OnResizeDressPic(object sender, EventArgs e)
        {
            DressPic.Image = new Bitmap(DressPic.Width, DressPic.Height);
            if (_loaded) // inital event
            {
                RefreshDrawing();
            }
        }

        private static void ConvertGump(ref int gumpId, ref int hue)
        {
            if (!GumpTable.Entries.Contains(gumpId))
            {
                return;
            }

            GumpTableEntry entry = (GumpTableEntry)GumpTable.Entries[gumpId];
            hue = entry.NewHue;
            gumpId = entry.NewId;
        }

        private void ConvertBody(ref int animId, ref int gumpId, ref int hue)
        {
            if (!_elve)
            {
                if (!_female)
                {
                    if (!EquipTable.HumanMale.Contains(animId))
                    {
                        return;
                    }

                    EquipTableEntry entry = (EquipTableEntry)EquipTable.HumanMale[animId];
                    gumpId = entry.NewId;
                    hue = entry.NewHue;
                    animId = entry.NewAnim;
                }
                else
                {
                    if (!EquipTable.HumanFemale.Contains(animId))
                    {
                        return;
                    }

                    EquipTableEntry entry = (EquipTableEntry)EquipTable.HumanFemale[animId];
                    gumpId = entry.NewId;
                    hue = entry.NewHue;
                    animId = entry.NewAnim;
                }
            }
            else
            {
                if (!_female)
                {
                    if (!EquipTable.ElvenMale.Contains(animId))
                    {
                        return;
                    }

                    EquipTableEntry entry = (EquipTableEntry)EquipTable.ElvenMale[animId];
                    gumpId = entry.NewId;
                    hue = entry.NewHue;
                    animId = entry.NewAnim;
                }
                else
                {
                    if (!EquipTable.ElvenFemale.Contains(animId))
                    {
                        return;
                    }

                    EquipTableEntry entry = (EquipTableEntry)EquipTable.ElvenFemale[animId];
                    gumpId = entry.NewId;
                    hue = entry.NewHue;
                    animId = entry.NewAnim;
                }
            }
        }

        private HuePopUpDress _showForm;

        private void OnClickHue(object sender, EventArgs e)
        {
            if (checkedListBoxWear.SelectedIndex == -1)
            {
                return;
            }

            if (_showForm?.IsDisposed == false)
            {
                return;
            }

            int layer = checkedListBoxWear.SelectedIndex;
            _showForm = new HuePopUpDress(this, _hues[layer], layer)
            {
                TopMost = true
            };
            _showForm.Show();
        }

        private void OnKeyDownHue(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (checkedListBoxWear.SelectedIndex == -1)
            {
                return;
            }

            if (Utils.ConvertStringToInt(toolStripTextBox1.Text, out int index, 0, Ultima.Hues.List.Length))
            {
                _hues[checkedListBoxWear.SelectedIndex] = index;
                RefreshDrawing();
            }
        }

        private void OnClickExtractImageBmp(object sender, EventArgs e)
        {
            string path = Options.OutputPath;
            if (_showPd)
            {
                string fileName = Path.Combine(path, "Dress PD.bmp");
                DressPic.Image.Save(fileName, ImageFormat.Bmp);
                MessageBox.Show(
                    $"Paperdoll saved to {fileName}",
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
            }
            else
            {
                string fileName = Path.Combine(path, "Dress IG.bmp");
                if (_animate)
                {
                    _mAnimation[0].Save(fileName, ImageFormat.Bmp);
                }
                else
                {
                    DressPic.Image.Save(fileName, ImageFormat.Bmp);
                }

                MessageBox.Show(
                    $"InGame saved to {fileName}",
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
            }
        }

        private void OnClickExtractImageTiff(object sender, EventArgs e)
        {
            string path = Options.OutputPath;
            if (_showPd)
            {
                string fileName = Path.Combine(path, "Dress PD.tiff");
                DressPic.Image.Save(fileName, ImageFormat.Tiff);
                MessageBox.Show(
                    $"Paperdoll saved to {fileName}",
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
            }
            else
            {
                string fileName = Path.Combine(path, "Dress IG.tiff");
                if (_animate)
                {
                    _mAnimation[0].Save(fileName, ImageFormat.Tiff);
                }
                else
                {
                    DressPic.Image.Save(fileName, ImageFormat.Tiff);
                }

                MessageBox.Show(
                    $"InGame saved to {fileName}",
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
            }
        }

        private void OnClickExtractImageJpg(object sender, EventArgs e)
        {
            string path = Options.OutputPath;
            if (_showPd)
            {
                string fileName = Path.Combine(path, "Dress PD.jpg");
                DressPic.Image.Save(fileName, ImageFormat.Jpeg);
                MessageBox.Show(
                    $"Paperdoll saved to {fileName}",
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
            }
            else
            {
                string fileName = Path.Combine(path, "Dress IG.jpg");
                if (_animate)
                {
                    _mAnimation[0].Save(fileName, ImageFormat.Jpeg);
                }
                else
                {
                    DressPic.Image.Save(fileName, ImageFormat.Jpeg);
                }

                MessageBox.Show(
                    $"InGame saved to {fileName}",
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
            }
        }

        private void OnClickExtractAnimBmp(object sender, EventArgs e)
        {
            string path = Options.OutputPath;
            const string fileName = "Dress Anim";

            for (int i = 0; i < _mAnimation.Length; ++i)
            {
                _mAnimation[i].Save(Path.Combine(path, $"{fileName}-{i}.bmp"), ImageFormat.Bmp);
            }
            MessageBox.Show(
                $"InGame Anim saved to '{fileName}-X.bmp'",
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void OnClickExtractAnimTiff(object sender, EventArgs e)
        {
            string path = Options.OutputPath;
            const string fileName = "Dress Anim";

            for (int i = 0; i < _mAnimation.Length; ++i)
            {
                _mAnimation[i].Save(Path.Combine(path, $"{fileName}-{i}.tiff"), ImageFormat.Tiff);
            }
            MessageBox.Show(
                $"InGame Anim saved to '{fileName}-X.tiff'",
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void OnClickExtractAnimJpg(object sender, EventArgs e)
        {
            string path = Options.OutputPath;
            const string fileName = "Dress Anim";

            for (int i = 0; i < _mAnimation.Length; ++i)
            {
                _mAnimation[i].Save(Path.Combine(path, $"{fileName}-{i}.jpg"), ImageFormat.Jpeg);
            }
            MessageBox.Show(
                $"InGame Anim saved to '{fileName}-X.jpg'",
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void OnClickBuildAnimationList(object sender, EventArgs e)
        {
            AnimEntry[] animEntries = new AnimEntry[1000];
            for (int i = 0; i < animEntries.Length; ++i)
            {
                animEntries[i] = new AnimEntry
                {
                    Animation = i,
                    FirstGump = i + 50000,
                    FirstGumpFemale = i + 60000
                };
                if (EquipTable.HumanMale.Contains(i))
                {
                    animEntries[i].EquipTable[400] = (EquipTableEntry)EquipTable.HumanMale[i];
                }

                if (EquipTable.HumanFemale.Contains(i))
                {
                    animEntries[i].EquipTable[401] = (EquipTableEntry)EquipTable.HumanFemale[i];
                }

                if (EquipTable.ElvenMale.Contains(i))
                {
                    animEntries[i].EquipTable[605] = (EquipTableEntry)EquipTable.ElvenMale[i];
                }

                if (EquipTable.ElvenFemale.Contains(i))
                {
                    animEntries[i].EquipTable[606] = (EquipTableEntry)EquipTable.ElvenFemale[i];
                }

                IDictionaryEnumerator itr;
                if (EquipTable.Misc.Contains(i))
                {
                    itr = ((Hashtable)EquipTable.Misc[i]).GetEnumerator();
                    while (itr.MoveNext())
                    {
                        animEntries[i].EquipTable[itr.Key] = (EquipTableEntry)itr.Value;
                    }
                }
                itr = animEntries[i].EquipTable.GetEnumerator();
                if (animEntries[i].EquipTable.Count == 0)
                {
                    if (GumpTable.Entries.Contains(animEntries[i].FirstGump))
                    {
                        animEntries[i].GumpDef[0] = (GumpTableEntry)GumpTable.Entries[animEntries[i].FirstGump];
                    }
                }
                else
                {
                    while (itr.MoveNext())
                    {
                        int newGump = ((EquipTableEntry)itr.Value).NewId;
                        if (GumpTable.Entries.Contains(newGump))
                        {
                            animEntries[i].GumpDef[itr.Key] = (GumpTableEntry)GumpTable.Entries[newGump];
                        }
                    }
                }
                itr.Reset();
                if (animEntries[i].EquipTable.Count == 0)
                {
                    int tmp = i;
                    animEntries[i].TranslateAnim[0] = new TranslateAnimEntry();
                    ((TranslateAnimEntry)animEntries[i].TranslateAnim[0]).BodyDef = BodyTable.m_Entries.ContainsKey(tmp);
                    Animations.Translate(ref tmp);
                    ((TranslateAnimEntry)animEntries[i].TranslateAnim[0]).FileIndex = BodyConverter.Convert(ref tmp);
                    ((TranslateAnimEntry)animEntries[i].TranslateAnim[0]).BodyAndConf = tmp;
                }
                else
                {
                    while (itr.MoveNext())
                    {
                        int tmp = ((EquipTableEntry)itr.Value).NewAnim;
                        animEntries[i].TranslateAnim[itr.Key] = new TranslateAnimEntry();
                        ((TranslateAnimEntry)animEntries[i].TranslateAnim[itr.Key]).BodyDef = BodyTable.m_Entries.ContainsKey(tmp);
                        Animations.Translate(ref tmp);
                        ((TranslateAnimEntry)animEntries[i].TranslateAnim[itr.Key]).FileIndex = BodyConverter.Convert(ref tmp);
                        ((TranslateAnimEntry)animEntries[i].TranslateAnim[itr.Key]).BodyAndConf = tmp;
                    }
                }
            }

            string fileName = Path.Combine(Options.OutputPath, "animationlist.html");
            using (StreamWriter tex = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write), System.Text.Encoding.GetEncoding(1252)))
            {
                tex.WriteLine("<html> <body> <table border='1' rules='all' cellpadding='2'>");
                tex.WriteLine("<tr>");
                tex.WriteLine("<td>Anim</td>");
                tex.WriteLine("<td>Gump male/female</td>");
                tex.WriteLine("<td>equipconv<br/>model:anim,gump,hue</td>");
                tex.WriteLine("<td>gump.def<br/>gump,hue</td>");
                tex.WriteLine("<td>body.def/bodyconv<br/>[model:]fileindex,anim</td>");
                tex.WriteLine("<td>tiledata def</td>");
                tex.WriteLine("</tr>");
                for (int i = 1; i < animEntries.Length; ++i)
                {
                    tex.WriteLine("<tr>");
                    tex.Write("<td>");
                    bool openFont = false;

                    if (!Animations.IsActionDefined(i, 0, 0))
                    {
                        tex.Write("<font color=#FF0000>");
                        openFont = true;
                    }

                    tex.Write(i);
                    if (openFont)
                    {
                        tex.Write("</font>");
                    }

                    tex.Write("</td>");
                    if (i >= 400)
                    {
                        tex.Write("<td>");
                        openFont = false;
                        if (!Gumps.IsValidIndex(animEntries[i].FirstGump))
                        {
                            tex.Write("<font color=#FF0000>");
                            openFont = true;
                        }
                        tex.Write(animEntries[i].FirstGump);
                        if (openFont)
                        {
                            tex.Write("</font>");
                        }

                        tex.Write("/");
                        openFont = false;
                        if (!Gumps.IsValidIndex(animEntries[i].FirstGumpFemale))
                        {
                            tex.Write("<font color=#FF0000>");
                            openFont = true;
                        }
                        tex.Write(animEntries[i].FirstGumpFemale);
                        if (openFont)
                        {
                            tex.Write("</font>");
                        }

                        tex.Write("</td>");
                    }
                    else
                    {
                        tex.Write("<td></td>");
                    }

                    IDictionaryEnumerator itr = animEntries[i].EquipTable.GetEnumerator();
                    tex.Write("<td>");
                    while (itr.MoveNext())
                    {
                        if ((int)itr.Key != 0)
                        {
                            tex.Write(itr.Key + ":");
                        }

                        openFont = false;
                        if (animEntries[i].TranslateAnim.ContainsKey(itr.Key))
                        {
                            if (!Animations.IsAnimDefinied(((TranslateAnimEntry)animEntries[i].TranslateAnim[itr.Key]).BodyAndConf, 0, 0,
                                ((TranslateAnimEntry)animEntries[i].TranslateAnim[itr.Key]).FileIndex))
                            {
                                tex.Write("<font color=#FF0000>");
                                openFont = true;
                            }
                        }
                        tex.Write(((EquipTableEntry)itr.Value).NewAnim);
                        if (openFont)
                        {
                            tex.Write("</font>");
                        }

                        tex.Write(",");
                        openFont = false;
                        if (!Gumps.IsValidIndex(animEntries[i].FirstGumpFemale))
                        {
                            tex.Write("<font color=#FF0000>");
                            openFont = true;
                        }
                        tex.Write(((EquipTableEntry)itr.Value).NewId);
                        if (openFont)
                        {
                            tex.Write("</font>");
                        }

                        tex.Write(",");
                        tex.Write(((EquipTableEntry)itr.Value).NewHue);
                        tex.Write("<br/>");
                    }
                    tex.Write("</td>");
                    itr = animEntries[i].GumpDef.GetEnumerator();
                    tex.Write("<td>");
                    while (itr.MoveNext())
                    {
                        if ((int)itr.Key != 0)
                        {
                            tex.Write(itr.Key + ":");
                        }

                        openFont = false;
                        if (!Gumps.IsValidIndex(((GumpTableEntry)itr.Value).NewId))
                        {
                            tex.Write("<font color=#FF0000>");
                            openFont = true;
                        }
                        tex.Write(((GumpTableEntry)itr.Value).NewId);
                        if (openFont)
                        {
                            tex.Write("</font>");
                        }

                        tex.Write("," + ((GumpTableEntry)itr.Value).NewHue + "<br/>");
                    }
                    tex.Write("</td>");
                    itr = animEntries[i].TranslateAnim.GetEnumerator();
                    tex.Write("<td>");
                    while (itr.MoveNext())
                    {
                        if (((TranslateAnimEntry)itr.Value).FileIndex == 1 && !((TranslateAnimEntry)itr.Value).BodyDef)
                        {
                            continue;
                        }

                        if ((int)itr.Key != 0)
                        {
                            tex.Write(itr.Key + ":");
                        }

                        tex.Write(((TranslateAnimEntry)itr.Value).FileIndex
                            + "," + ((TranslateAnimEntry)itr.Value).BodyAndConf + "<br/>");
                    }
                    tex.Write("</td>");
                    tex.Write("<td>");
                    if (i >= 400)
                    {
                        for (int j = 0; j < TileData.ItemTable.Length; ++j)
                        {
                            if (TileData.ItemTable[j].Animation == i)
                            {
                                tex.Write("0x{0:X4} {1}<br/>", j, TileData.ItemTable[j].Name);
                            }
                        }
                    }
                    tex.Write("</td>");
                    tex.WriteLine("</tr>");
                }
                tex.WriteLine("</table> </body> </html>");
            }

            MessageBox.Show(
                $"Report saved to '{fileName}'",
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void MountTextBoxOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (!Utils.ConvertStringToInt(textBoxMount.Text, out int index, 0, 0xFFFF))
            {
                return;
            }

            if (!Animations.IsActionDefined(index, 0, 0))
            {
                return;
            }

            _mount = index;
            RefreshDrawing();
        }
    }

    public class TranslateAnimEntry
    {
        public int FileIndex { get; set; }
        public int BodyAndConf { get; set; }
        public bool BodyDef { get; set; }
    }

    public class AnimEntry
    {
        public struct EquipTableDef { public int Gump; public int Anim; }
        public int Animation { get; set; }
        public int FirstGump { get; set; } //+50000
        public int FirstGumpFemale { get; set; }//+60000
        public Hashtable EquipTable { get; set; } //equipconv.def with model
        public Hashtable GumpDef { get; set; } //gump.def if gump invalid (only for paperdoll)
        public Hashtable TranslateAnim { get; set; }//body.def or bodyconv.def

        public AnimEntry()
        {
            EquipTable = new Hashtable();
            GumpDef = new Hashtable();
            TranslateAnim = new Hashtable();
        }
    }

    public class ObjTypeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;
            return string.CompareOrdinal(tx.Text, ty.Text);
        }
    }

    public class LayerSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            int layerX = TileData.ItemTable[(int)tx.Tag].Quality;
            int layerY = TileData.ItemTable[(int)ty.Tag].Quality;

            if (layerX == layerY)
            {
                return 0;
            }

            if (layerX < layerY)
            {
                return -1;
            }

            return 1;
        }
    }

    public static class GumpTable
    {
        public static Hashtable Entries { get; }

        // Seems only used if Gump is invalid
        static GumpTable()
        {
            Entries = new Hashtable();
            Initialize();
        }

        public static void Initialize()
        {
            string path = Files.GetFilePath("gump.def");
            if (path == null)
            {
                return;
            }

            using (StreamReader ip = new StreamReader(path))
            {
                string line;
                while ((line = ip.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                    {
                        continue;
                    }

                    try
                    {
                        // <ORIG BODY> {<NEW BODY>} <NEW HUE>
                        int index1 = line.IndexOf("{", StringComparison.Ordinal);
                        int index2 = line.IndexOf("}", StringComparison.Ordinal);

                        string param1 = line.Substring(0, index1);
                        string param2 = line.Substring(index1 + 1, index2 - index1 - 1);
                        string param3 = line.Substring(index2 + 1);

                        int iParam1 = Convert.ToInt32(param1.Trim());
                        int iParam2 = Convert.ToInt32(param2.Trim());
                        int iParam3 = Convert.ToInt32(param3.Trim());

                        Entries[iParam1] = new GumpTableEntry(iParam1, iParam2, iParam3);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }
    }

    public class GumpTableEntry
    {
        public int OldId { get; }
        public int NewId { get; }
        public int NewHue { get; }

        public GumpTableEntry(int oldId, int newId, int newHue)
        {
            OldId = oldId;
            NewId = newId;
            NewHue = newHue;
        }
    }

    public static class EquipTable
    {
        public static Hashtable HumanMale { get; }
        public static Hashtable HumanFemale { get; }
        public static Hashtable ElvenMale { get; }
        public static Hashtable ElvenFemale { get; }
        public static Hashtable Misc { get; }

        static EquipTable()
        {
            HumanMale = new Hashtable();
            HumanFemale = new Hashtable();
            ElvenMale = new Hashtable();
            ElvenFemale = new Hashtable();
            Misc = new Hashtable();
            Initialize();
        }

        public static void Initialize()
        {
            string path = Files.GetFilePath("equipconv.def");
            if (path == null)
            {
                return;
            }

            using (StreamReader ip = new StreamReader(path))
            {
                string line;
                while ((line = ip.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                    {
                        continue;
                    }
                    //#bodyType	#equipmentID	#convertToID	#GumpIDToUse	#hue
                    //GumpID (0 = equipmentID + 50000, -1 = convertToID + 50000, other numbers are the actual gumpID )

                    try
                    {
                        string[] split = Regex.Split(line, @"\s+");

                        int bodyType = Convert.ToInt32(split[0]);
                        int animId = Convert.ToInt32(split[1]);
                        int convertId = Convert.ToInt32(split[2]);
                        int gumpId = Convert.ToInt32(split[3]);
                        int hue = Convert.ToInt32(split[4]);

                        if (gumpId == 0)
                        {
                            gumpId = animId + 50000;
                        }
                        else if (gumpId == -1)
                        {
                            gumpId = convertId + 50000;
                        }

                        EquipTableEntry entry = new EquipTableEntry(gumpId, hue, convertId);
                        switch (bodyType)
                        {
                            case 400:
                                HumanMale[animId] = entry;
                                break;
                            case 401:
                                HumanFemale[animId] = entry;
                                break;
                            case 605:
                                ElvenMale[animId] = entry;
                                break;
                            case 606:
                                ElvenFemale[animId] = entry;
                                break;
                            default:
                                {
                                    if (!Misc.Contains(animId))
                                    {
                                        Misc[animId] = new Hashtable();
                                    } ((Hashtable)Misc[animId])[bodyType] = entry;
                                    break;
                                }
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }
    }

    public class EquipTableEntry
    {
        public int NewId { get; }
        public int NewHue { get; }
        public int NewAnim { get; }

        public EquipTableEntry(int newId, int newHue, int newAnim)
        {
            NewId = newId;
            NewHue = newHue;
            NewAnim = newAnim;
        }
    }
}
