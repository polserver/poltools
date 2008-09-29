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
		const string filename = "POLLaunch.cfg";
		
		private Settings()
		{
			//if (!File.Exists(filename))
			//	SaveConfiguration();
			//else
			//	LoadConfiguration();
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
				TextReader tr = new StreamReader(filename);
				string line = tr.ReadLine();
				while (line != null)
				{
					string[] pair = line.Split(new char[] { '=' }, 2);
					if (pair.Length == 1)
						continue;

					//pair[0] = pair[0].ToLower();
					pair[1] = pair[1].TrimEnd(new char[] { ' ', '\t', '\n', '\r' });
					//switch (pair[0])
					{
						Global.Properties.Add(pair[0], pair[1]);
						/*
						case "uopath": this.uo_path = pair[1]; break;
						case "polpath": this.pol_path = pair[1]; break;
						case "polexepath": this.polexe_path = pair[1]; break;
						case "uoconvertexepath": this.uocnvrtexe_path = pair[1]; break;
						case "ecompileexepath": this.ecompileexe_path = pair[1]; break;
						case "showpoltabfirst": this.show_pol_tab_first = ToBoolean(pair[1]); break;
						*/
					}
					line = tr.ReadLine();
				}
				tr.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ERROR");
			}

			return true;
		}

		public bool SaveConfiguration()
		{
			if (File.Exists(filename))
			{
				try
				{
					File.Delete(filename);
				}
				catch { }
			}
			try
			{
				TextWriter tw = new StreamWriter(filename);
				foreach (DictionaryEntry k in Global.Properties)
				{
					tw.WriteLine(k.Key + "=" + k.Value);
				}
				/*
				tw.WriteLine("UOPath=" + this.uo_path);
				tw.WriteLine("POLPath=" + this.pol_path);
				tw.WriteLine("POLExePath" + this.polexe_path);
				tw.WriteLine("UOConvertExePath=" + this.uocnvrtexe_path);
				tw.WriteLine("EcompileExePath=" + this.ecompileexe_path);
				tw.WriteLine("ShowPOLTabFirst=" + this.show_pol_tab_first);
				*/
				tw.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error");
			}
			return true;
		}

		private bool ToBoolean(string value)
		{
			value = value.ToLower();
			if (value == "1" || value == "true")
				return true;
			else
				return false;
		}
	}
}