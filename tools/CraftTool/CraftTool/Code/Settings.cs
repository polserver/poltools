using System;
using System.IO;
using ConfigUtil;
using System.Collections.Generic;

namespace CraftTool
{
	public sealed class Settings
	{
		private static volatile Settings _global;
		private static object _syncroot = new Object();

		private Dictionary<string, string> _settings;
		
		private string _filepath = Program.GetPath(false) + @"\craftToolSettings.cfg";
		private string _root_dir = Program.GetPath(false);

		private Settings()
		{
		}
		~Settings()
		{
			WriteSettingsFile();
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

		public Dictionary<string, string> settings
		{
			get { return Global._settings; }
		}

		public string rootdir
		{
			get 
			{
				string directory = Global._settings["POLPath"];
				if (!Directory.Exists(directory))
					return Program.GetPath(false);
				else
					return directory;
			}
			set 
			{ 
				Global._root_dir = value;
				Global._settings["POLPath"] = value;
			}
		}

		public string GetSetting(string name)
		{
			if (Global._settings.ContainsKey(name))
				return Global._settings[name];
			else
				return null;
		}

		public void UpdateSetting(string key, string value)
		{
			Global._settings[key] = value;
		}

		public bool LoadSettings()
		{
			_settings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

			ConfigFile settings_config = new ConfigFile(Global._filepath);
			if (File.Exists(Global._filepath))
				settings_config.ReadConfigFile();
			else
				settings_config.ReadConfigFile(global::CraftTool.Properties.Resources.craftToolSettings);
			
			ConfigElem settings_elem = settings_config.GetConfigElem("Settings");

			foreach (string key in settings_elem.ListConfigElemProperties())
			{
				_settings[key] = settings_elem.GetConfigString(key);
			}

			return true;
		}
		public bool WriteSettingsFile()
		{
			ConfigFile new_config = new ConfigFile(Global._filepath);
			ConfigElem new_elem = new ConfigElem("Elem", "Settings");
			foreach (string key in Global._settings.Keys)
			{
				new_elem.AddConfigLine(key, Global._settings[key]);
			}

			new_config.AddConfigElement(new_elem);

			new_config.WriteConfigFile();
			return true;
		}
	}
}
