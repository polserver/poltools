using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace POLLaunch
{
	public class Settings
	{
		static Settings settings;
		Hashtable properties = new Hashtable();
		string filename = Program.GetPath()+"POLLaunch.cfg";
		
		private Settings()
		{
		}

		~Settings()
		{
			SaveConfiguration();
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
		}

		public Hashtable Properties
		{
			get
			{
				return properties;
			}
		}

		public bool LoadConfiguration()
		{
			if (!File.Exists(filename))
				return false;

			try
			{
				Global.properties.Clear();
				TextReader tr = new StreamReader(filename);
				string line = tr.ReadLine();
				while (line != null)
				{
					string[] pair = line.Split(new char[] { '=' }, 2);
					if (pair.Length == 1)
						continue;

					//pair[0] = pair[0].ToLower();
					pair[1] = pair[1].TrimEnd(new char[] { ' ', '\t', '\n', '\r' });
					Global.properties[pair[0]] = pair[1];
					line = tr.ReadLine();
				}
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
				foreach (DictionaryEntry k in Global.properties)
				{
					tw.WriteLine(k.Key + "=" + k.Value);
				}
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
			value = value.ToLower();
			if (value == "" || value == "0" || value == "false")
				return false;
			else
				return true;
		}
	}
}