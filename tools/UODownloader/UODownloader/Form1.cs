using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using FileDownloaderApp;
using System.Threading;

namespace UODownloader
{
	public partial class Form1 : Form
	{
		private FileDownloader _downloader;

		public Form1()
		{
			InitializeComponent();
			this.Icon = global::UODownloader.Properties.Resources.c25;
			this.notifyIcon1.Icon = this.Icon;

			_downloader = new FileDownloader(true);
			_downloader.ProgressChanged += new EventHandler(downloader_ProgressChanged);
			_downloader.StateChanged += new EventHandler(downloader_StateChanged);
			_downloader.FileDownloadStarted += new EventHandler(downloader_FileDownloadStarted);
			_downloader.FileDownloadSucceeded += new EventHandler(downloader_FileDownloadCompleted);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			if (!File.Exists(Settings.Global.FilePath))
			{
				Forms.SettingsForm settings_form = new Forms.SettingsForm();
				settings_form.ShowDialog(this);
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			exitToolStripMenuItem_Click(sender, e);
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Forms.AboutBox1 about_form = new Forms.AboutBox1();
			about_form.ShowDialog(this);
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Forms.SettingsForm settings_form = new Forms.SettingsForm();
			settings_form.ShowDialog(this);
		}

		private void BTN_UpdateNews_Click(object sender, EventArgs e)
		{
			BTN_UpdateNews.Enabled = false;
			
			textBox1.Clear();
			string url = (string)Settings.Global.settings["UpdateURL"] + "/updatenews.txt";
			textBox1.AppendText(ReadWebPage(url));
			
			BTN_UpdateNews.Enabled = true;
		}

		private void BTN_Start_Click(object sender, EventArgs e)
		{
			BTN_Start.Enabled = false;
			BTN_Pause.Enabled = BTN_Stop.Enabled = true;
			string url = (string)Settings.Global.settings["UpdateURL"];
			try
			{
				if (Directory.Exists(Settings.Global.settings["GameDirectory"].ToString()))
					_downloader.LocalDirectory = Settings.Global.settings["GameDirectory"].ToString();
				else
				{
					textBox2.AppendText("WARNING - Directory '" + Settings.Global.settings["GameDirectory"].ToString()+"' does not exist!" + Environment.NewLine);
					textBox2.AppendText("Saving files to '" + Program.GetPath() + "'" + Environment.NewLine);
					_downloader.LocalDirectory = Program.GetPath();
				}


				_downloader.Files.Clear();
				
				textBox2.Clear();
				textBox2.AppendText("Downloading manifest file..." + Environment.NewLine);
				string manifest = ReadWebPage(url + "/manifest.txt");
				textBox2.AppendText(manifest);
				textBox2.Select(0, 0);
								
				foreach (string line in manifest.Split('\n'))
				{
					String trimmed = line.Trim(' ', '\r');
					if (trimmed.Length > 0)
					{
						_downloader.Files.Add(new FileDownloader.FileInfo(url+"/"+trimmed));
					}
				}
				
				_downloader.Start();
			}
			catch (Exception ex)
			{
				ExceptionBox.ExceptionForm exb = new ExceptionBox.ExceptionForm(ref ex);
				exb.ShowDialog(this);
			}
		}

		private void BTN_Pause_Click(object sender, EventArgs e)
		{
			_downloader.Pause();
		}

		private void BTN_Resume_Click(object sender, EventArgs e)
		{
			_downloader.Resume();
		}

		private void BTN_Stop_Click(object sender, EventArgs e)
		{
			_downloader.Stop();
		}


		private string ReadWebPage(string url)
		{
			string result = String.Empty;
			try
			{
				//http://msdn.microsoft.com/en-us/library/system.net.webrequest.aspx
				// Create a request for the URL. 		
				WebRequest request = WebRequest.Create(url);
				// If required by the server, set the credentials.
				//request.Credentials = CredentialCache.DefaultCredentials;
				// Get the response.
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				// Get the stream containing content returned by the server.
				Stream data_stream = response.GetResponseStream();
				// Open the stream using a StreamReader for easy access.
				StreamReader reader = new StreamReader(data_stream);
				// Read the content.
				result = reader.ReadToEnd();

				// Cleanup the streams and the response.
				reader.Close();
				data_stream.Close();
				response.Close();
			}
			catch (Exception ex)
			{
				ExceptionBox.ExceptionForm exb = new ExceptionBox.ExceptionForm(ref ex);
				exb.ShowDialog(this);
			}

			result = result.Replace("\n", Environment.NewLine);
			return result;
		}

		/*
		 * Downloader event handlers
		 */

		// Display of the file info after the download started
		private void downloader_FileDownloadStarted(object sender, EventArgs e)
		{
			label3.Text = _downloader.CurrentFile.Path;
			label4.Text = _downloader.LocalDirectory + @"\" + _downloader.CurrentFile.Name;
			label6.Text = _downloader.CurrentFileSize.ToString() + " bytes";
		}
		
		// Occurs every time of block of data has been downloaded, and can be used to display the progress with
        // Note that you can also create a timer, and display the progress every certain interval
        // Also note that the progress properties return a size in bytes, which is not really user friendly to display
        //      The FileDownloader class provides static functions to format these byte amounts to a more readible format, either in binary or decimal notatio
		private void downloader_ProgressChanged(object sender, EventArgs e)
		{
			// Current File Information
			label7.Text = String.Format("Downloaded   {0} of {1} ({2}%)", FileDownloader.FormatSizeDecimal(_downloader.CurrentFileProgress), FileDownloader.FormatSizeDecimal(_downloader.CurrentFileSize), _downloader.CurrentFilePercentage()) + String.Format(" @ {0}/s", FileDownloader.FormatSizeDecimal(_downloader.DownloadSpeed));
			int percent = (int)_downloader.CurrentFilePercentage();
			if (percent > 0 && percent<=100)
				progressBar1.Value = percent;

			// Overall Information

			label16.Text = String.Format("Downloaded   {0} of {1} ({2}%)", FileDownloader.FormatSizeDecimal(_downloader.TotalProgress), FileDownloader.FormatSizeDecimal(_downloader.TotalSize), _downloader.TotalPercentage());
			progressBar2.Value = (int)_downloader.TotalPercentage();
		}

		// This event is fired every time the paused or busy state is changed, and used here to set the controls of the interface
		// This makes it enuivalent to a void handling both downloader.IsBusyChanged and downloader.IsPausedChanged
		private void downloader_StateChanged(object sender, EventArgs e)
		{
			// Setting the buttons
			BTN_Start.Enabled = _downloader.CanStart;
			BTN_Stop.Enabled = _downloader.CanStop;
			BTN_Pause.Enabled = _downloader.CanPause;
			BTN_Resume.Enabled = _downloader.CanResume;
		}

		private void downloader_FileDownloadCompleted(object sender, EventArgs e)
		{
		}
	}
}
