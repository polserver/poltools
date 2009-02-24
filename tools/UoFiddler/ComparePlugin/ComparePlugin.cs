using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using PluginInterface;
using Ultima;
using ComparePlugin;

namespace FiddlerPlugin
{
    public class ComparePlugin : IPlugin
    {
        string myName = "ComparePlugin";
        string myDescription = "Compares 2 art files (Adds 2 new Tabs)";
        string myAuthor = "Turley";
        string myVersion = "1.0.0";
        IPluginHost myHost = null;

        /// <summary>
        /// Name of the plugin
        /// </summary>
        public override string Name { get { return myName; } }
        /// <summary>
        /// Description of the Plugin's purpose
        /// </summary>
        public override string Description { get { return myDescription; } }
        /// <summary>
        /// Author of the plugin
        /// </summary>
        public override string Author { get { return myAuthor; } }
        /// <summary>
        /// Version of the plugin
        /// </summary>
        public override string Version { get { return myVersion; } }
        /// <summary>
        /// Host of the plugin.
        /// </summary>
        public override IPluginHost Host { get { return myHost; } set { myHost = value; } }

        public override void Initialize()
        {
        }

        public override void Dispose()
        {
        }

        public override void Reload()
        {
        }

        public override void OnDesignChange()
        {
        }

        public override void ModifyTabPages(TabControl tabcontrol)
        {
            TabPage page = new TabPage();
            page.Tag = tabcontrol.TabCount+1;
            page.Text = "Compare Items";
            CompareItem compArt = new CompareItem();
            compArt.Dock = System.Windows.Forms.DockStyle.Fill;
            page.Controls.Add(compArt);
            tabcontrol.TabPages.Add(page);
            TabPage page2 = new TabPage();
            page2.Tag = tabcontrol.TabCount + 1;
            page2.Text = "Compare Land";
            CompareLand compLand = new CompareLand();
            compLand.Dock = System.Windows.Forms.DockStyle.Fill;
            page2.Controls.Add(compLand);
            tabcontrol.TabPages.Add(page2);
        }

        public override void ModifyPluginToolStrip(ToolStripDropDownButton toolstrip)
        {
        }

        public override void ModifyItemShowContextMenu(ContextMenuStrip strip)
        {
        }
    }
}
