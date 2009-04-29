/***************************************************************************
 *
 * $Author: MuadDib & Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

using System.Windows.Forms;
using PluginInterface;

namespace FiddlerPlugin
{
    public class MultiEditorPlugin : IPlugin
    {
        public MultiEditorPlugin()
        {
        }

        string myName = "MultiEditorPlugin";
        string myDescription = "blubb";
        string myAuthor = "MuadDib & Turley";
        string myVersion = "0.0.1";
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
            //fired on fiddler startup
        }

        public override void Dispose()
        {
            //fired in Fiddler OnClosing
        }

        // the magic add a new tabpage at the end
        public override void ModifyTabPages(TabControl tabcontrol)
        {
            TabPage page = new TabPage();
            page.Tag = tabcontrol.TabCount + 1; // at end used for undock/dock feature to define the order
            page.Text = "Multi Editor";
            page.Controls.Add(new MultiEditor());
            tabcontrol.TabPages.Add(page);
        }

        public override void ModifyPluginToolStrip(ToolStripDropDownButton toolstrip)
        {
            //want an entry inside the plugin dropdown?
        }
    }
}
