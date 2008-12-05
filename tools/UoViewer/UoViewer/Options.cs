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
using System.IO;
using System.Xml;
using Ultima;

namespace UoViewer
{
    public class Options 
    {
        public Options() 
        {
 //           Files.LoadMulPath();
            Load();
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
            elem.SetAttribute("width", Controls.Options.ArtItemSizeWidth.ToString());
            elem.SetAttribute("height", Controls.Options.ArtItemSizeHeight.ToString());
            sr.AppendChild(elem);
            comment = dom.CreateComment("ItemClip images in items tab shrinked or clipped");
            sr.AppendChild(comment);
            elem = dom.CreateElement("ItemClip");
            elem.SetAttribute("active", Controls.Options.ArtItemClip.ToString());
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
            comment = dom.CreateComment("Alternative layout in items tab?");
            sr.AppendChild(comment);
            elem = dom.CreateElement("AlternativeDesign");
            elem.SetAttribute("active", Controls.Options.DesignAlternative.ToString());
            sr.AppendChild(elem);
            comment = dom.CreateComment("Use Hashfile to speed up load?");
            sr.AppendChild(comment);
            elem = dom.CreateElement("UseHashFile");
            elem.SetAttribute("active", Files.UseHashFile.ToString());
            sr.AppendChild(elem);
            comment = dom.CreateComment("Pathsettings");
            sr.AppendChild(comment);

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
                Controls.Options.ArtItemSizeWidth = int.Parse(elem.GetAttribute("width"));
                Controls.Options.ArtItemSizeHeight = int.Parse(elem.GetAttribute("height"));
            }
            elem = (XmlElement)xOptions.SelectSingleNode("ItemClip");
            if (elem != null)
                Controls.Options.ArtItemClip = bool.Parse(elem.GetAttribute("active"));

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
                Controls.Options.DesignAlternative = bool.Parse(elem.GetAttribute("active"));
                UoViewer.AlternativeDesign = Controls.Options.DesignAlternative;
            }

            elem = (XmlElement)xOptions.SelectSingleNode("UseHashFile");
            if (elem != null)
                Files.UseHashFile = bool.Parse(elem.GetAttribute("active"));

            foreach (XmlElement xPath in xOptions.SelectNodes("Paths"))
            {
                string key;
                string value;
                key = xPath.GetAttribute("key");
                value = xPath.GetAttribute("value");
                Files.MulPath[key] = value;
            }

            if (Files.GetFilePath("map1.mul") != null)
            {
                if (Ultima.Map.Trammel.Width == 7168)
                    Ultima.Map.Trammel = new Ultima.Map(1, 1, 7168, 4096);
                else
                    Ultima.Map.Trammel = new Ultima.Map(1, 1, 6144, 4096);
            }
        }
    }
}
