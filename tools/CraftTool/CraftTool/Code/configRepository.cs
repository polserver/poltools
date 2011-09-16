using System;
using System.Collections.Generic;
using System.Text;
using ConfigUtil;
using System.IO;

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
			_config_cache = new Dictionary<string, ConfigFile>(StringComparer.InvariantCultureIgnoreCase);
		}

		public bool IsPathCached(string path)
		{
			return global._config_cache.ContainsKey(path);
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

		public List<ConfigElem> GetElemsFromConfigFiles(string filename)
		{
			List<ConfigElem> elems = new List<ConfigElem>();
			foreach ( ConfigFile config_file in global._config_cache.Values )
			{
				if (config_file.filename.Equals(filename, StringComparison.CurrentCultureIgnoreCase))
				{
					elems.AddRange(config_file.GetConfigElemRefs());
				}
			}

			return elems;
		}

		public List<String> GetElemNamesFromConfigFiles(string filename)
		{
			List<String> elem_names = new List<String>();
			foreach (ConfigFile config_file in global._config_cache.Values)
			{
				if (config_file.filename.Equals(filename, StringComparison.CurrentCultureIgnoreCase) )
				{
					foreach (ConfigElem elem in config_file.GetConfigElemRefs())
						elem_names.Add(elem.name);
				}
			}

			return elem_names;
		}
		

		public ConfigElem FindElemInConfigFiles(string filename, string elem_name)
		{
			foreach (ConfigFile config_file in global._config_cache.Values)
			{
				if (config_file.filename.Equals(filename, StringComparison.CurrentCultureIgnoreCase))
				{
					if (config_file.ElemNameExists(elem_name))
					{
						return config_file.GetConfigElem(elem_name);
					}
				}
			}

			return null;
		}

		public List<string> GetConfigPropertyValuesFromLoadedConfigFiles(string filename, string property_name, bool single, bool allowduplicates)
		{
			List<string> values = new List<string>();
			foreach (ConfigFile config_file in global._config_cache.Values)
			{
				if (config_file.filename.Equals(filename, StringComparison.CurrentCultureIgnoreCase))
				{
					foreach (ConfigElem elem in config_file.GetConfigElemRefs())
					{
						if (!elem.PropertyExists(property_name))
							continue;
						else if (single)
						{
							string value = elem.GetConfigString(property_name);
							if (!allowduplicates)
								if (values.Exists(delegate(string n) { return n.Equals(value, StringComparison.CurrentCultureIgnoreCase); }))
									continue;
							values.Add(value);
						}
						else
						{
							foreach (string value in elem.GetConfigStringList(property_name))
							{
								if (!allowduplicates)
									if (values.Exists(delegate(string n) { return n.Equals(value, StringComparison.CurrentCultureIgnoreCase); }))
										break;
								values.Add(value);
							}
						}
					}
				}
			}

			return values;
		}

		public int LoadConfigFileFromPackages(string filename)
		{
			int count = 0;

			foreach (POLTools.Package.POLPackage package in POLTools.Package.PackageCache.Global.packagelist)
			{
				string filepath = package.GetPackagedConfigPath(filename);
				if (this.ContainsPath(filepath))
					count++;
				else if (filepath == null)
					continue;
				else
				{
					ConfigFile configfile = package.LoadPackagedConfig(filename);
					AddConfigFile(configfile);
					count++;
				}
			}

			return count;
		}

		public static bool WriteConfigFile(ConfigFile cfg_file)
		{
			File.WriteAllText(cfg_file.fullpath, cfg_file.ToString());

			return true;
		}
	}
}
