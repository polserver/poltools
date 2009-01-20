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
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Ultima;

namespace UoFiddler
{
    public class Options 
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

        public Options() 
        {
            Load();
            if (m_UpdateCheckOnStart)
            {
                BackgroundWorker updater = new BackgroundWorker();
                updater.DoWork += new DoWorkEventHandler(Updater_DoWork);
                updater.RunWorkerCompleted+=new RunWorkerCompletedEventHandler(Updater_RunWorkerCompleted);
                updater.RunWorkerAsync();
            }
        }

        public static void Save()
        {
            string filepath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            string FileName = Path.Combine(filepath, "Options.xml");

            XmlDocument dom = new XmlDocument();
            XmlDeclaration decl = dom.CreateXmlDeclaration("1.0", "utf-8", null);
            dom.AppendChild(decl);
            XmlElement sr = dom.CreateElement("Options");

            XmlComment comment= dom.CreateComment("ItemSize controls the size of images in items tab");
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
            elem.SetAttribute("active", Ultima.Map.Felucca.Width==7168 ? true.ToString() : false.ToString());
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
                    for (int i=0;i<tool.Args.Count;i++)
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
            dom.Save(FileName);
        }

        private void Load()
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(Path.GetDirectoryName(path), "Options.xml");
            if (!System.IO.File.Exists(FileName))
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
                    Ultima.Map.Felucca.Width = 7168;
                    Ultima.Map.Trammel.Width = 7168;
                }
            }

            elem = (XmlElement)xOptions.SelectSingleNode("AlternativeDesign");
            if (elem != null)
            {
                FiddlerControls.Options.DesignAlternative = bool.Parse(elem.GetAttribute("active"));
            }

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
            Files.CheckForNewMapSize();
        }

        /// <summary>
        /// Checks polserver forum for updates
        /// </summary>
        /// <returns></returns>
        public static Match CheckForUpdate(out string error)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[8192];
            Match match;
            error = "";

            try
            {
                HttpWebRequest request = (HttpWebRequest)
                    WebRequest.Create(@"http://forums.polserver.com/viewtopic.php?f=1&t=2351&st=0&sk=t&sd=a");

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

                Regex reg = new Regex(@"<a href=""./download/file.php\?id=(?<id>[\d]+)&amp;sid=(?<sid>[\w]+)"">UOFiddler (?<major>\d).(?<minor>\d)(?<sub>\w)?.rar</a>", RegexOptions.Compiled);
                match = reg.Match(sb.ToString());
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

        private void Updater_DoWork(object sender, DoWorkEventArgs e)
        {
            string error;
            e.Result = CheckForUpdate(out error);
            if (e.Result == null)
                throw new Exception(error);

        }

        private void Updater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error!=null)
            {
                MessageBox.Show("Error:\n" + e.Error, "Check for Update");
                return;
            }
            Match match = (Match)e.Result;
            if (match.Success)
            {
                string version = match.Result("${major}.${minor}${sub}");
                if (UoFiddler.Version.Equals(version))
                    MessageBox.Show("Your Version is up-to-date", "Check for Update");
                else
                {
                    DialogResult result =
                        MessageBox.Show(String.Format(@"Your version differs: {0} Found: {1}"
                        , UoFiddler.Version, version) + "\nDownload now?", "Check for Update", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                        DownloadFile(version, match.Result("${id}"));
                }
            }
            else
                MessageBox.Show("Failed to get Versioninfo", "Check for Update");
        }

        #region Downloader
        private void DownloadFile(string version, string id)
        {
            string filepath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(filepath, String.Format("UOFiddler {0}.rar", version));

            WebClient web = new WebClient();
            web.DownloadFileCompleted += new AsyncCompletedEventHandler(OnDownloadFileCompleted);
            web.DownloadFileAsync(new Uri(String.Format(@"http://forums.polserver.com/download/file.php\?id={0}", id)), FileName);
        }

        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("An error occurred while downloading UOFiddler\n" + e.Error.Message,
                    "Updater");
                return;
            }
            MessageBox.Show("Finished Download","Updater");
        }
        #endregion
    }

    public class ExternTool
    {
        private string m_Name;
        private string m_FileName;
        private ArrayList m_Args;
        private ArrayList m_ArgsName;

        public string Name { get { return m_Name; } set { m_Name = value; } }
        public string FileName { get { return m_FileName; } set { m_FileName = value; } }
        public ArrayList Args { get { return m_Args; } set { m_Args = value; } }
        public ArrayList ArgsName { get { return m_ArgsName; } set { m_ArgsName = value; } }

        public ExternTool(string name, string filename)
        {
            m_Name = name;
            m_FileName = filename;
            m_Args = new ArrayList();
            m_ArgsName = new ArrayList();
        }

        public string FormatName()
        {
            return String.Format("{0}: {1}", m_Name, m_FileName);
        }
        public string FormatArg(int i)
        {
            return String.Format("{0}: {1}", m_ArgsName[i], m_Args[i]);
        }
    }
}
