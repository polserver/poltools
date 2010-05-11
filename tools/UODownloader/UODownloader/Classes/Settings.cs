using System;
using System.Collections;
using System.IO;

namespace UODownloader
{
	public sealed class Settings
	{
		private static volatile Settings _global;
		private static object _syncroot = new Object();
		private Hashtable _settings = new Hashtable();

		private string _filepath = Program.GetPath()+@"\settings.cfg";

		private Settings()
		{
		}

		~Settings()
		{
			WriteSettingsFile();
		}

		public string FilePath
		{
			get
			{
				return _filepath;
			}
		}

		public static Settings Global
		{
			get
			{
				if (_global == null)
				{
					lock (_syncroot)
					{
						if (_global == null)
						{
						_global = new Settings();
							_global.LoadSettings();
						}
					}
				}
				return _global;
			}
		}

		public Hashtable settings
		{
			get
			{
				return _settings;
			}
		}
		
		public bool LoadSettings()
		{
			if ( !File.Exists(_filepath) )
			{
				if ( !MakeSettingsFile() )
				{
					throw new Exception("Could not create settings file.");
				}
			}

			try
			{
				TextReader tr = new StreamReader(_filepath);
				string line = "";
				while (line != null)
				{
					line = tr.ReadLine();
					if (line == null)
						continue;
					else if (line.Length < 1)
						continue;
					else if (line.StartsWith("#"))
						continue;
					else if (line.StartsWith("//"))
						continue;

					line = line.TrimStart(new char[]{' ', '\t'});
					string[] pair = line.Split(new char[] {'=' }, 2);
					pair[0] = pair[0].TrimEnd(new char[] {' ', '\t'});
					pair[1] = pair[1].TrimStart(new char[] {' ', '\t'});
					pair[1] = pair[1].TrimEnd(new char[] {' ', '\t', '\n', '\r' });

					_settings.Add(pair[0], pair[1]);
				}
				tr.Close();
			}
			catch (Exception ex)
			{
				ExceptionBox.ExceptionForm tmp = new ExceptionBox.ExceptionForm(ref ex);
				tmp.ShowDialog();
				return false;
			}

			return true;
		}
		public bool WriteSettingsFile()
		{
			string cfgstring = String.Empty;
			foreach (DictionaryEntry d in _settings)
			{
				cfgstring += d.Key + " = " + d.Value + Environment.NewLine;
			}
			
			try
			{
				TextWriter tw = new StreamWriter(_filepath);
				tw.Write(cfgstring);
				tw.Close();
			}
			catch (Exception ex)
			{
				ExceptionBox.ExceptionForm tmp = new ExceptionBox.ExceptionForm(ref ex);
				tmp.ShowDialog();
				return false;
			}

			return true;
		}

		//cfgstring = SettingSubstitution(cfgstring, "AssetPicturePath", "<AssetPicPath>");
		private string SettingSubstitution(string original, string hashkey, string find)
		{
			if (_settings.ContainsKey(hashkey))
			{
				original = original.Replace(find, _settings[hashkey].ToString());
				_settings.Remove(hashkey);
			}

			return original;
		}

		private bool MakeSettingsFile()
		{
			string cfgstring = String.Empty;
			
			try
			{
				TextWriter tw = new StreamWriter(Program.GetPath() + @"\settings.cfg");
				tw.WriteLine(cfgstring);
				tw.Close();
			}
			catch (Exception ex)
			{
				ExceptionBox.ExceptionForm tmp = new ExceptionBox.ExceptionForm(ref ex);
				tmp.ShowDialog();
				return false;
			}
			return true;
		}

	}
}
