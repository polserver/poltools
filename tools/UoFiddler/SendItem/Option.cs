using System;
using System.Windows.Forms;

namespace FiddlerPlugin
{
    public partial class Option : Form
    {
        public Option()
        {
            InitializeComponent();
            this.Icon = FiddlerControls.Options.GetFiddlerIcon();
            cmdtext.Text = SendItemPlugin.Cmd;
            argstext.Text = SendItemPlugin.CmdArg;
            SendOnClick.Checked = SendItemPlugin.OverrideClick;
        }

        private void OnClickSave(object sender, EventArgs e)
        {
            SendItemPlugin.Cmd = cmdtext.Text;
            SendItemPlugin.CmdArg = argstext.Text;
            SendItemPlugin.OverrideClick = SendOnClick.Checked;
            Close();
        }
    }
}
