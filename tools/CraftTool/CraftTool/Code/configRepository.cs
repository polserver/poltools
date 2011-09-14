using System;
using System.Collections;
using System.Collections.Generic;
using ConfigUtil;

namespace POLTools.ConfigRepository
{
	class ConfigRepository
	{
		private static volatile ConfigRepository _global;
		private static object _syncroot = new Object();

		Dictionary<string, ConfigFile> _config_cache = null;
		
		public ConfigRepository()
		{
		}
		~ConfigRepository()
		{
		}

		public static ConfigRepository global
		{
			get
			{
				if (_global == null)
				{
					lock (_syncroot)
					{
						if (_global == null)
						{
							_global = new ConfigRepository();
							_global.Initialize();
						}
					}
				}
				return _global;
			}
		}
		private void Initialize()
		{
			_config_cache = new Dictionary<string, ConfigFile>();
		}

		public ConfigFile LoadConfigFile(string path)
		{
			ConfigFile config_file = null;
			if (_config_cache.ContainsKey(path))
				config_file = _config_cache[path];
			else
			{
				config_file = new ConfigFile(path);
				config_file.ReadConfigFile();
				AddConfigFile(config_file);
			}

			return config_file;
		}

		public bool UnloadConfigFile(string path)
		{
			return _config_cache.Remove(path);
		}

		public bool AddConfigFile(ConfigFile config_file)
		{
			if (!_config_cache.ContainsKey(config_file.fullpath))
			{
				_config_cache.Add(config_file.fullpath, config_file);
				return true;
			}
			else
				return false;
		}
	}
}
