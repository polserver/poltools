using System;
using System.Collections;
using System.Collections.Generic;
using ConfigUtil;
using POLTools.Package;

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
			if (global._config_cache.ContainsKey(path))
				config_file = global._config_cache[path];
			else
			{
				config_file = new ConfigFile(path);
				config_file.ReadConfigFile();
				global.AddConfigFile(config_file);
			}

			return config_file;
		}

		public bool UnloadConfigFile(string path)
		{
			return global._config_cache.Remove(path);
		}

		public bool AddConfigFile(ConfigFile config_file)
		{
			if (!global._config_cache.ContainsKey(config_file.fullpath))
			{
				global._config_cache.Add(config_file.fullpath, config_file);
				return true;
			}
			else
				return false;
		}

		public bool ContainsPath(string path)
		{
			return global._config_cache.ContainsKey(path);
		}

		public List<ConfigElem> GetElemsForConfigFile(string filename)
		{
			filename = filename.ToLower();
			List<ConfigElem> elems = new List<ConfigElem>();
			foreach ( ConfigFile config_file in global._config_cache.Values )
			{
				if (config_file.filename.ToLower() == filename)
					elems.AddRange(config_file.GetConfigElemRefs());
			}

			return elems;
		}

		public ConfigElem GetElemFromConfigFiles(string filename, string elem_name)
		{
			List<ConfigElem> elems = GetElemsForConfigFile(filename);
			foreach (ConfigFile config_file in global._config_cache.Values)
			{
				try
				{
					ConfigElem elem = config_file.GetConfigElem(elem_name);
					if (elem != null)
						return elem;
				}
				catch
				{
					continue;
				}
			}

			return null;
		}
	}
}
