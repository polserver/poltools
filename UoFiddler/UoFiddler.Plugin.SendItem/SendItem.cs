﻿/***************************************************************************
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
using System.Xml;
using Ultima;
using UoFiddler.Controls.Classes;
using UoFiddler.Controls.Plugin;
using UoFiddler.Controls.Plugin.Interfaces;
using UoFiddler.Controls.UserControls;
using UoFiddler.Plugin.SendItem.Forms;
using Events = UoFiddler.Controls.Plugin.PluginEvents;

namespace UoFiddler.Plugin.SendItem
{
    public class SendItemPluginBase : PluginBase
    {
        public SendItemPluginBase()
        {
            _refMarker = this;
            Events.DesignChangeEvent += Events_DesignChangeEvent;
            Events.ModifyItemShowContextMenuEvent += Events_ModifyItemShowContextMenuEvent;
        }

        private static SendItemPluginBase _refMarker;
        private static bool _overrideClick;

        public static string Cmd { get; set; } = ".create";

        public static string CmdArg { get; set; } = "0x{1:X4}";

        public static bool OverrideClick
        {
            get => _overrideClick;
            set
            {
                if (value != _overrideClick)
                    _refMarker.ChangeOverrideClick(value, false);
                _overrideClick = value;
            }
        }

        /// <summary>
        /// Name of the plugin
        /// </summary>
        public override string Name { get; } = "SendItemPlugin";

        /// <summary>
        /// Description of the Plugin's purpose
        /// </summary>
        public override string Description { get; } = "Send custom Cmd to Client with selected ObjectType in Items tab";

        /// <summary>
        /// Author of the plugin
        /// </summary>
        public override string Author { get; } = "Turley";

        /// <summary>
        /// Version of the plugin
        /// </summary>
        public override string Version { get; } = "1.0.1";

        /// <summary>
        /// Host of the plugin.
        /// </summary>
        public override IPluginHost Host { get; set; } = null;

        public override void Initialize()
        {
            LoadXml();
            ChangeOverrideClick(OverrideClick, true);
        }

        private void PlugOnDoubleClick(object sender, MouseEventArgs e)
        {
            ItemShowContextClicked(this, EventArgs.Empty);
        }

        public override void Dispose()
        {
            SaveXml();
        }

        public override void ModifyTabPages(TabControl tabControl)
        {
        }

        private void Events_DesignChangeEvent()
        {
            ChangeOverrideClick(OverrideClick, true);
        }

        public override void ModifyPluginToolStrip(ToolStripDropDownButton toolStrip)
        {
            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = "Send Item"
            };
            item.Click += ToolStripClick;
            toolStrip.DropDownItems.Add(item);
        }

        private void ChangeOverrideClick(bool value, bool init)
        {
            if (Options.DesignAlternative)
            {
                ItemShowAlternative itemShowAltControl = Host.GetItemShowAltControl();
                PictureBox itemShowAltPictureBox = Host.GetItemShowAltPictureBox();
                if (value)
                {
                    itemShowAltPictureBox.MouseDoubleClick -= itemShowAltControl.OnMouseDoubleClick;
                    itemShowAltPictureBox.MouseDoubleClick += PlugOnDoubleClick;
                }
                else if (!init)
                {
                    itemShowAltPictureBox.MouseDoubleClick -= PlugOnDoubleClick;
                    itemShowAltPictureBox.MouseDoubleClick += itemShowAltControl.OnMouseDoubleClick;
                }
            }
            else
            {
                ItemShow itemShowControl = Host.GetItemShowControl();
                ListView itemShowListView = Host.GetItemShowListView();
                if (value)
                {
                    itemShowListView.MouseDoubleClick -= itemShowControl.ListView_DoubleClicked;
                    itemShowListView.MouseDoubleClick += PlugOnDoubleClick;
                }
                else if (!init)
                {
                    itemShowListView.MouseDoubleClick -= PlugOnDoubleClick;
                    itemShowListView.MouseDoubleClick += itemShowControl.ListView_DoubleClicked;
                }
            }
        }

        private static void ToolStripClick(object sender, EventArgs e)
        {
            new Option().Show();
        }

        private void Events_ModifyItemShowContextMenuEvent(ContextMenuStrip strip)
        {
            ToolStripMenuItem item = new ToolStripMenuItem { Text = "Send Item to Client" };
            item.Click += ItemShowContextClicked;
            strip.Items.Add(item);
        }

        private void ItemShowContextClicked(object sender, EventArgs e)
        {
            int currSelected = Options.DesignAlternative
                ? Host.GetSelectedItemShowAlternative()
                : Host.GetSelectedItemShow();

            if (currSelected <= -1)
            {
                return;
            }

            if (Client.Running)
            {
                string format = "{0} " + CmdArg;
                Client.SendText(string.Format(format, Cmd, currSelected));
            }
            else
            {
                MessageBox.Show(
                    "No Client running/or not recognized",
                    "SendItem",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
        }

        private static void LoadXml()
        {
            string path = Options.AppDataPath;
            string fileName = Path.Combine(path, "plugins/SendItem.xml");
            if (!File.Exists(fileName))
            {
                return;
            }

            XmlDocument dom = new XmlDocument();
            dom.Load(fileName);

            XmlElement xOptions = dom["Options"];

            XmlElement elem = (XmlElement)xOptions?.SelectSingleNode("SendItem");
            if (elem == null)
            {
                return;
            }

            Cmd = elem.GetAttribute("cmd");
            CmdArg = elem.GetAttribute("args");
            OverrideClick = bool.Parse(elem.GetAttribute("overrideclick"));
        }

        private static void SaveXml()
        {
            string path = Options.AppDataPath;
            string fileName = Path.Combine(path, "plugins/senditem.xml");

            XmlDocument dom = new XmlDocument();

            XmlDeclaration decl = dom.CreateXmlDeclaration("1.0", "utf-8", null);
            dom.AppendChild(decl);

            XmlElement sr = dom.CreateElement("Options");

            XmlComment comment = dom.CreateComment("Defines the cmd for Item create");
            sr.AppendChild(comment);

            comment = dom.CreateComment("{1} = item objecttype");
            sr.AppendChild(comment);

            XmlElement elem = dom.CreateElement("SendItem");
            elem.SetAttribute("cmd", Cmd);
            elem.SetAttribute("args", CmdArg);
            elem.SetAttribute("overrideclick", OverrideClick.ToString());
            sr.AppendChild(elem);

            dom.AppendChild(sr);
            dom.Save(fileName);
        }
    }
}