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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Ultima;

namespace UoFiddler
{
    public static class Options
    {
        private static bool m_UpdateCheckOnStart = false;
        private static ArrayList m_ExternTools;

        public static ArrayList ExternTools
        {
            get { return m_ExternTools; }
            set { m_ExternTools = value; }
        }
        /// <summary>
        /// Definies if an Update Check should be made on startup
        /// </summary>
        public static bool UpdateCheckOnStart
        {
            get { return m_UpdateCheckOnStart; }
            set { m_UpdateCheckOnStart = value; }
        }

        public static void Startup()
        {
            Load();
            if (m_UpdateCheckOnStart)
            {
                using (BackgroundWorker updater = new BackgroundWorker())
                {
                    updater.DoWork += new DoWorkEventHandler(Updater_DoWork);
                    updater.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Updater_RunWorkerCompleted);
                    updater.RunWorkerAsync();
                }
            }
        }

        public static void Save()
        {
            string FileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Options.xml");

            XmlDocument dom = new XmlDocument();
            XmlDeclaration decl = dom.CreateXmlDeclaration("1.0", "utf-8", null);
            dom.AppendChild(decl);
            XmlElement sr = dom.CreateElement("Options");

            XmlComment comment = dom.CreateComment("ItemSize controls the size of images in items tab");
            sr.AppendChild(comment);
            XmlElement elem = dom.CreateElement("ItemSize");
            elem.SetAttribute("width", FiddlerControls.Options.ArtItemSizeWidth.ToString());
            elem.SetAttribute("height", FiddlerControls.Options.ArtItemSizeHeight.ToString());
            sr.AppendChild(elem);
            comment = dom.CreateComment("ItemClip images in items tab shrinked or clipped");
            sr.AppendChild(comment);
            elem = dom.CreateElement("ItemClip");
            elem.SetAttribute("active", FiddlerControls.Options.ArtItemClip.ToString());
            sr.AppendChild(elem);
            comment = dom.CreateComment("CacheData should mul entries be cached for faster load");
            sr.AppendChild(comment);
            elem = dom.CreateElement("CacheData");
            elem.SetAttribute("active", Files.CacheData.ToString());
            sr.AppendChild(elem);
            comment = dom.CreateComment("NewMapSize Felucca/Trammel width 7168?");
            sr.AppendChild(comment);
            elem = dom.CreateElement("NewMapSize");
            elem.SetAttribute("active", Map.Felucca.Width == 7168 ? true.ToString() : false.ToString());
            sr.AppendChild(elem);
            comment = dom.CreateComment("UseMapDiff should mapdiff files be used");
            sr.AppendChild(comment);
            elem = dom.CreateElement("UseMapDiff");
            elem.SetAttribute("active", Map.UseDiff.ToString());
            sr.AppendChild(elem);
            comment = dom.CreateComment("Alternative layout in item/landtile/texture tab?");
            sr.AppendChild(comment);
            elem = dom.CreateElement("AlternativeDesign");
            elem.SetAttribute("active", FiddlerControls.Options.DesignAlternative.ToString());
            sr.AppendChild(elem);
            comment = dom.CreateComment("Use Hashfile to speed up load?");
            sr.AppendChild(comment);
            elem = dom.CreateElement("UseHashFile");
            elem.SetAttribute("active", Files.UseHashFile.ToString());
            sr.AppendChild(elem);
            comment = dom.CreateComment("Should an Update Check be done on startup?");
            sr.AppendChild(comment);
            elem = dom.CreateElement("UpdateCheck");
            elem.SetAttribute("active", UpdateCheckOnStart.ToString());
            sr.AppendChild(elem);

            comment = dom.CreateComment("Definies the cmd to send Client to loc");
            sr.AppendChild(comment);
            comment = dom.CreateComment("{1} = x, {2} = y, {3} = z, {4} = mapid, {5} = mapname");
            sr.AppendChild(comment);
            elem = dom.CreateElement("SendCharToLoc");
            elem.SetAttribute("cmd", FiddlerControls.Options.MapCmd);
            elem.SetAttribute("args", FiddlerControls.Options.MapArgs);
            sr.AppendChild(elem);

            comment = dom.CreateComment("Definies the map names");
            sr.AppendChild(comment);
            elem = dom.CreateElement("MapNames");
            elem.SetAttribute("map0", FiddlerControls.Options.MapNames[0]);
            elem.SetAttribute("map1", FiddlerControls.Options.MapNames[1]);
            elem.SetAttribute("map2", FiddlerControls.Options.MapNames[2]);
            elem.SetAttribute("map3", FiddlerControls.Options.MapNames[3]);
            elem.SetAttribute("map4", FiddlerControls.Options.MapNames[4]);
            sr.AppendChild(elem);

            comment = dom.CreateComment("Extern Tools settings");
            sr.AppendChild(comment);
            if (ExternTools != null)
            {
                foreach (ExternTool tool in ExternTools)
                {
                    XmlElement xtool = dom.CreateElement("ExternTool");
                    xtool.SetAttribute("name", tool.Name);
                    xtool.SetAttribute("path", tool.FileName);
                    for (int i = 0; i < tool.Args.Count; i++)
                    {
                        XmlElement xarg = dom.CreateElement("Args");
                        xarg.SetAttribute("name", (string)tool.ArgsName[i]);
                        xarg.SetAttribute("arg", (string)tool.Args[i]);
                        xtool.AppendChild(xarg);
                    }
                    sr.AppendChild(xtool);
                }
            }

            comment = dom.CreateComment("Loaded Plugins");
            sr.AppendChild(comment);
            if (FiddlerControls.Options.PluginsToLoad != null)
            {
                foreach (string plug in FiddlerControls.Options.PluginsToLoad)
                {
                    XmlElement xplug = dom.CreateElement("Plugin");
                    xplug.SetAttribute("name", plug);
                    sr.AppendChild(xplug);
                }
            }

            comment = dom.CreateComment("Pathsettings");
            sr.AppendChild(comment);
            elem = dom.CreateElement("RootPath");
            elem.SetAttribute("path", Files.RootDir);
            sr.AppendChild(elem);
            ArrayList sorter = new ArrayList(Files.MulPath.Keys);
            sorter.Sort();
            foreach (string key in sorter)
            {
                XmlElement path = dom.CreateElement("Paths");
                path.SetAttribute("key", key.ToString());
                path.SetAttribute("value", Files.MulPath[key].ToString());
                sr.AppendChild(path);
            }
            dom.AppendChild(sr);

            comment = dom.CreateComment("Disabled Tab Views");
            sr.AppendChild(comment);
            foreach (KeyValuePair<int, bool> kvp in FiddlerControls.Options.ChangedViewState)
            {
                if (!kvp.Value)
                {
                    XmlElement viewstate = dom.CreateElement("TabView");
                    viewstate.SetAttribute("tab", kvp.Key.ToString());
                    sr.AppendChild(viewstate);
                }
            }

            dom.Save(FileName);
        }

        private static void Load()
        {
            string FileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Options.xml");
            if (!File.Exists(FileName))
                return;

            XmlDocument dom = new XmlDocument();
            dom.Load(FileName);
            XmlElement xOptions = dom["Options"];

            XmlElement elem = (XmlElement)xOptions.SelectSingleNode("ItemSize");
            if (elem != null)
            {
                FiddlerControls.Options.ArtItemSizeWidth = int.Parse(elem.GetAttribute("width"));
                FiddlerControls.Options.ArtItemSizeHeight = int.Parse(elem.GetAttribute("height"));
            }
            elem = (XmlElement)xOptions.SelectSingleNode("ItemClip");
            if (elem != null)
                FiddlerControls.Options.ArtItemClip = bool.Parse(elem.GetAttribute("active"));

            elem = (XmlElement)xOptions.SelectSingleNode("CacheData");
            if (elem != null)
                Files.CacheData = bool.Parse(elem.GetAttribute("active"));

            elem = (XmlElement)xOptions.SelectSingleNode("NewMapSize");
            if (elem != null)
            {
                if (bool.Parse(elem.GetAttribute("active")))
                {
                    Map.Felucca.Width = 7168;
                    Map.Trammel.Width = 7168;
                }
            }
            elem = (XmlElement)xOptions.SelectSingleNode("UseMapDiff");
            if (elem != null)
                Map.UseDiff = bool.Parse(elem.GetAttribute("active"));

            elem = (XmlElement)xOptions.SelectSingleNode("AlternativeDesign");
            if (elem != null)
                FiddlerControls.Options.DesignAlternative = bool.Parse(elem.GetAttribute("active"));

            elem = (XmlElement)xOptions.SelectSingleNode("UseHashFile");
            if (elem != null)
                Files.UseHashFile = bool.Parse(elem.GetAttribute("active"));

            elem = (XmlElement)xOptions.SelectSingleNode("UpdateCheck");
            if (elem != null)
                UpdateCheckOnStart = bool.Parse(elem.GetAttribute("active"));

            elem = (XmlElement)xOptions.SelectSingleNode("SendCharToLoc");
            if (elem != null)
            {
                FiddlerControls.Options.MapCmd = elem.GetAttribute("cmd");
                FiddlerControls.Options.MapArgs = elem.GetAttribute("args");
            }

            elem = (XmlElement)xOptions.SelectSingleNode("MapNames");
            if (elem != null)
            {
                FiddlerControls.Options.MapNames[0] = elem.GetAttribute("map0");
                FiddlerControls.Options.MapNames[1] = elem.GetAttribute("map1");
                FiddlerControls.Options.MapNames[2] = elem.GetAttribute("map2");
                FiddlerControls.Options.MapNames[3] = elem.GetAttribute("map3");
                FiddlerControls.Options.MapNames[4] = elem.GetAttribute("map4");
                FiddlerControls.Options.MapNames[5] = elem.GetAttribute("map5");
            }

            ExternTools = new ArrayList();
            foreach (XmlElement xTool in xOptions.SelectNodes("ExternTool"))
            {
                string name = xTool.GetAttribute("name");
                string file = xTool.GetAttribute("path");
                ExternTool tool = new ExternTool(name, file);
                foreach (XmlElement xArg in xTool.SelectNodes("Args"))
                {
                    string argname = xArg.GetAttribute("name");
                    string arg = xArg.GetAttribute("arg");
                    tool.Args.Add(arg);
                    tool.ArgsName.Add(argname);
                }
                ExternTools.Add(tool);
            }

            FiddlerControls.Options.PluginsToLoad = new ArrayList();
            foreach (XmlElement xPlug in xOptions.SelectNodes("Plugin"))
            {
                string name = xPlug.GetAttribute("name");
                FiddlerControls.Options.PluginsToLoad.Add(name);
            }

            elem = (XmlElement)xOptions.SelectSingleNode("RootPath");
            if (elem != null)
                Files.RootDir = elem.GetAttribute("path");

            foreach (XmlElement xPath in xOptions.SelectNodes("Paths"))
            {
                string key;
                string value;
                key = xPath.GetAttribute("key");
                value = xPath.GetAttribute("value");
                Files.MulPath[key] = value;
            }

            foreach (XmlElement xTab in xOptions.SelectNodes("TabView"))
            {
                int viewtab;
                viewtab = Convert.ToInt32(xTab.GetAttribute("tab"));
                FiddlerControls.Options.ChangedViewState[viewtab] = false;
            }

            Files.CheckForNewMapSize();
        }

        /// <summary>
        /// Checks polserver forum for updates
        /// </summary>
        /// <returns></returns>
        public static string[] CheckForUpdate(out string error)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[8192];
            error = "";
            string[] match;

            try
            {
                HttpWebRequest request = (HttpWebRequest)
                    WebRequest.Create(@"http://uofiddler.polserver.com/latestversion");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();

                string tempString = null;
                int count = 0;

                do
                {
                    count = resStream.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        tempString = Encoding.ASCII.GetString(buf, 0, count);
                        sb.Append(tempString);
                    }
                }
                while (count > 0);

                match = sb.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);

                response.Close();
                resStream.Dispose();
            }
            catch (Exception e)
            {
                match = null;
                error = e.Message;
            }

            return match;
        }

        private static void Updater_DoWork(object sender, DoWorkEventArgs e)
        {
            string error;
            e.Result = CheckForUpdate(out error);
            if (e.Result == null)
                throw new Exception(error);

        }

        private static void Updater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error:\n" + e.Error, "Check for Update");
                return;
            }
            string[] match = (string[])e.Result;
            if (match != null)
            {
                if (UoFiddler.Version.Equals(match[0]))
                    MessageBox.Show("Your Version is up-to-date", "Check for Update");
                else
                {
                    DialogResult result =
                        MessageBox.Show(String.Format(@"Your version differs: {0} Found: {1}"
                        , UoFiddler.Version, match[0]) + "\nDownload now?", "Check for Update", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                        DownloadFile(match[1]);
                }
            }
            else
                MessageBox.Show("Failed to get Versioninfo", "Check for Update");
        }

        #region Downloader
        private static void DownloadFile(string file)
        {
            string FileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, file);
            using (WebClient web = new WebClient())
            {
                web.DownloadFileCompleted += new AsyncCompletedEventHandler(OnDownloadFileCompleted);
                web.DownloadFileAsync(new Uri(String.Format(@"http://downloads.polserver.com/browser.php?download=./Projects/uofiddler/{0}", file)), FileName);
            }
        }

        private static void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("An error occurred while downloading UOFiddler\n" + e.Error.Message,
                    "Updater");
                return;
            }
            MessageBox.Show("Finished Download", "Updater");
        }
        #endregion
    }

    public class ExternTool
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public ArrayList Args { get; set; }
        public ArrayList ArgsName { get; set; }

        public ExternTool(string name, string filename)
        {
            Name = name;
            FileName = filename;
            Args = new ArrayList();
            ArgsName = new ArrayList();
        }

        public string FormatName()
        {
            return String.Format("{0}: {1}", Name, FileName);
        }
        public string FormatArg(int i)
        {
            return String.Format("{0}: {1}", ArgsName[i], Args[i]);
        }
    }
}
