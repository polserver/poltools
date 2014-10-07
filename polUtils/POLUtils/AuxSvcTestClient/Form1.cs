using System;
using System.Collections;
using System.Windows.Forms;

using POLUtils.AuxSvc;
using POLUtils.PackUnpack;

namespace AuxSvcTestClient
{
    public partial class AuxSvcTestClient : Form
    {
        public AuxSvcTestClient()
        {
            InitializeComponent();
        }

        private void testAuxButton_Click(object sender, EventArgs e)
        {
            // Create the AuxSvc Object for your program. This will automatically
            // try to connect to your host/port and prepare you to be able to 
            // send your information to POL.
            AuxSvcConnection TestAuxSvc = new AuxSvcConnection(serverHost.Text.ToString(), Convert.ToInt32(serverPort.Text));
            if (!TestAuxSvc.Active)
            {
                MessageBox.Show("Server appears to be offline. Make sure host and port are correct.");
                return;
            }
            TestAuxSvc.Write(PackUnpack.Pack(auxSendText.Text.ToString()));
            string ReadString = TestAuxSvc.Read();
            object ReadObject = PackUnpack.Unpack(ReadString.ToString());
            ArrayList ReadArray = (ArrayList)ReadObject;
            auxRecvText.Text = ReadArray[0] + " " + ReadArray[1] + " " + ReadArray[2];
            TestAuxSvc.Close();
        }
    }
}
