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
using System.Reflection;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace UoViewer
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            progresslabel.Visible = false;
            checkBoxCheckOnStart.Checked = Options.UpdateCheckOnStart;
        }

        private void OnChangeCheck(object sender, EventArgs e)
        {
            Options.UpdateCheckOnStart = !Options.UpdateCheckOnStart;
        }

        private void OnClickUpdate(object sender, EventArgs e)
        {
            progresslabel.Text = "Checking...";
            progresslabel.Visible = true;
            string error;
            Match match=Options.CheckForUpdate(out error);
            if (match == null)
            {
                MessageBox.Show("Error:\n"+error, "Check for Update");
                return;
            }
            if (match.Success)
            {
                string version = match.Result("${major}.${minor}${sub}");
                if (UoViewer.Version.Equals(version))
                    MessageBox.Show("Your Version is up-to-date", "Check for Update");
                else
                {
                    DialogResult result =
                        MessageBox.Show(String.Format(@"Your version differs: {0} Found: {1}"
                        , UoViewer.Version, version)+"\nDownload now?", "Check for Update", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                        DownloadFile(version,match.Result("${id}"));
                }
            }
            else
                MessageBox.Show("Failed to get Versioninfo", "Check for Update");
        }

        private void DownloadFile(string version,string id)
        {
            progresslabel.Text = "Starting download...";
            string filepath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(filepath, String.Format("UoViewer {0}.rar",version));

            WebClient web = new WebClient();
            web.DownloadProgressChanged += new DownloadProgressChangedEventHandler(OnDownloadProgressChanged);
            web.DownloadFileCompleted += new AsyncCompletedEventHandler(OnDownloadFileCompleted);
            web.DownloadFileAsync(new Uri(String.Format(@"http://forums.polserver.com/./download/file.php\?id={0}", id)),FileName);
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progresslabel.Text=String.Format("Downloading... bytes {0}/{1}", e.BytesReceived, e.TotalBytesToReceive);
        }

        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("An error occurred while downloading UOViewer\n" + e.Error.Message,
                    "Updater");
                return;
            }
            progresslabel.Text = "Finished Download";
        }

        private void OnClickLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://forums.polserver.com/viewtopic.php?f=1&t=2351&st=0&sk=t&sd=a");
        }
    }
}
