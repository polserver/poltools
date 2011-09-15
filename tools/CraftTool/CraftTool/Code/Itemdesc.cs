using System;
using System.IO;
using System.Collections.Generic;
using ConfigUtil;
using POLTools.Package;
using System.Windows.Forms;

namespace POLTools.Itemdesc
{
	public class ItemdescCache
	{
		private static object _syncroot = new Object();
		private static volatile ItemdescCache _global;
		private Dictionary<string, string> _objtypes; // Itemname/Objtype->Objtype
		private Dictionary<string, ConfigElem> _itemdesc; // Objtype -> Elem

		private ItemdescCache()
		{
		}
		~ItemdescCache()
		{
		}

		public static ItemdescCache Global
		{
			get
			{
				if (_global == null)
				{
					lock (_syncroot)
					{
						if (_global == null)
						{
							_global = new ItemdescCache();
							_global.Initialize();
						}
					}
				}
				return _global;
			}
		}
		private void Initialize()
		{
			_objtypes = new Dictionary<string, string>();
			_itemdesc = new Dictionary<string, ConfigElem>();
		}

		public List<ConfigElem> GetAllObjTypeElems()
		{
			return new List<ConfigElem>(_itemdesc.Values);
		}

		public string GetObjTypeByName(string name)
		{
			if (Global._objtypes.ContainsKey(name.ToLower()))
				return Global._objtypes[name];
			return null;
		}

		public ConfigElem GetElemForObjType(string objtype)
		{
			if (Global._itemdesc.ContainsKey(objtype))
				return Global._itemdesc[objtype];
			return null;
		}

		public bool LoadItemdescFiles()
		{
			_objtypes.Clear();
			_itemdesc.Clear();

			foreach (POLPackage package in PackageCache.Global.packagelist)
			{
				string itemdesc_path = package.GetPackagedConfigPath("itemdesc.cfg");
				if (itemdesc_path == null)
					continue;
				ConfigFile config_file = ConfigRepository.ConfigRepository.global.LoadConfigFile(itemdesc_path);
				if (config_file == null)
					continue;
				
				foreach (ConfigElem config_elem in config_file.GetConfigElemRefs())
				{
					_itemdesc.Add(config_elem.name, config_elem);
					if (config_elem.PropertyExists("Name"))
					{
						_objtypes.Add(config_elem.name, config_elem.name);
						foreach (string value in config_elem.GetConfigStringList("Name"))
						{
							_objtypes.Add(value, config_elem.name);
						}
					}
				}
			}
			return true;
		}
	}
}
