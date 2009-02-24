using System;
using System.Windows.Forms;

namespace UoFiddler
{
    public partial class HelpPage : Form
    {
        public HelpPage()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            string filepath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            webBrowser.Navigate("file://"+filepath+"help/index.html");
        }
    }
}
