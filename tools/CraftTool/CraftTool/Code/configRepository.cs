using System;
using System.Collections;
using System.Collections.Generic;
using ConfigUtil;

namespace POLTools.ConfigRepository
{
	class ConfigRepository
	{
		Dictionary<string, ConfigFile> _config_cache;
		
		public ConfigRepository()
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
				_config_cache.Add(path, config_file);
			}

			return config_file;
		}

		public bool UnloadConfigFile(string path)
		{
			return _config_cache.Remove(path);
		}			
	}
}
