using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;

namespace POLLaunch
{
    [Serializable]
    [XmlRoot("configurations")]
	public class Settings
	{
		static Settings settings;
		MyHashtable properties = new MyHashtable();
        List<Realm> realms = new List<Realm>();
     
		string filename = Program.GetPath()+"POLLaunch.xml";
		
		private Settings()
		{
		}

		~Settings()
		{
			SaveConfiguration();
		}

        public static bool IsNull
        {
            get
            {
                if (Settings.settings == null)
                    return true;
                else
                    return false;
            }
        }

		public static Settings Global
		{
			get
			{
				if (settings == null)
				{
					settings = new Settings();
                    settings.LoadConfiguration();
				}
				return settings;
			}
            set
            {
                settings = value;
            }
		}

        [XmlArray("properties")]
        [XmlArrayItem("property")]
		public MyHashtable Properties
		{
			get
			{
				return properties;
			}
		}

        public List<Realm> Realms
        {
            get
            {
                return realms;
            }
        }

		public bool LoadConfiguration()
		{
			if (!File.Exists(filename))
				return false;

           try
            {
                StreamReader tr = new StreamReader(filename);
                XmlTextReader xr = new XmlTextReader(tr);
                XmlSerializer xs = new XmlSerializer(typeof(Settings));
                object c;
                if (xs.CanDeserialize(xr))
                {
                    c = xs.Deserialize(xr); // Don´t know why this didn´t work directly
                    Settings.Global = (Settings)c;
                }
                xr.Close();
                tr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "LoadConfiguration() Error");
                return false;
            }

			return true;
		}

		public bool SaveConfiguration()
		{
            try
            {
                TextWriter tw = new StreamWriter(filename);
                XmlSerializer xs = new XmlSerializer(this.GetType());
                xs.Serialize(tw, Settings.Global);
                tw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SaveConfiguration() Error");
                return false;
            }

			return true;
		}

		public bool ToBoolean(string value)
		{
            if (value == null) return false; // Who would send a null reference to this function? haha.. =D

			value = value.ToLower();
			if (value == "" || value == "0" || value == "false")
				return false;
			else
				return true;
		}
	}

    [Serializable]
    public class Realm
    {
        public string Name;
        public int MapID;
        public bool UseDif;
        public int Width;
        public int Height;

        public Realm() { }

        public Realm(string Name, int MapID, bool UseDif, int Width, int Height)
        {
            this.Name = Name;
            this.MapID = MapID;
            this.UseDif = UseDif;
            this.Width = Width;
            this.Height = Height;
        }
    }
}