using System;
using System.Collections;
using System.IO;
using ConfigUtil;

namespace CraftTool
{
	public sealed class Settings
	{
		private static volatile Settings _global;
		private static object _syncroot = new Object();
		
		private string _filepath = Program.GetPath(false) + @"\craftToolSettings.cfg";

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

		public bool LoadSettings()
		{
			ConfigFile settings_config = new ConfigFile(_filepath);
			return true;
		}
		public bool WriteSettingsFile()
		{
			return true;
		}
	}
}
