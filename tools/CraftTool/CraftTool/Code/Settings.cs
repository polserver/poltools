using System;
using System.Collections;
using System.IO;
using ConfigUtil;
using System.Reflection;
using System.Windows.Forms;

namespace CraftTool
{
	public sealed class Settings
	{
		private static volatile Settings _global;
		private static object _syncroot = new Object();
		
		private string _filepath = Program.GetPath(false) + @"\craftToolSettings.cfg";
		private ConfigFile _settings_config = null;
		private ConfigElem _settings_elem = null;

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

		public ConfigFile configfile
		{
			get { return _settings_config; }
		}

		public ConfigElem settingselem
		{
			get { return _settings_elem; }
		}

		public string rootdir
		{
			get { return _root_dir; }
			set { _root_dir = value; }
		}

		public bool LoadSettings()
		{
			_settings_config = new ConfigFile(_filepath);
			if (File.Exists(_filepath))
				_settings_config.ReadConfigFile();
			else
				_settings_config.ReadConfigFile(global::CraftTool.Properties.Resources.craftToolSettings);
			
			_settings_elem = _settings_config.GetConfigElem("Settings");

			_root_dir = _settings_elem.GetConfigString("POLPath");

			return true;
		}
		public bool WriteSettingsFile()
		{
			return true;
		}
	}
}
