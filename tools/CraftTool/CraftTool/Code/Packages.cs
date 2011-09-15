using System;
using System.IO;
using System.Collections.Generic;
using ConfigUtil;

namespace POLTools.Package
{
	public class POLPackage
	{
		protected DirectoryInfo _dirinfo;
		protected bool _enabled;
		protected string _name;
		protected string _version;
		protected string _core_required;
				
		public POLPackage(string directory)
		{
			if (Directory.Exists(directory))
				_dirinfo = new DirectoryInfo(directory);
			else if (File.Exists(directory))
				_dirinfo = new FileInfo(directory).Directory;
			else
				throw new Exception("POLPackage - Invalid directory '" + directory + "'");
			
			try
			{
				LoadPackageConfig();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message+" File="+path+"/pkg.cfg");
			}
		}

		public string rootdir
		{
			get { return _dirinfo.Name; }
		}
		public string path
		{
			get { return _dirinfo.FullName; }
		}
		public DirectoryInfo directory
		{
			get { return _dirinfo; }
		}
		public bool enabled
		{
			get { return _enabled; }
		}
		public string name
		{
			get { return _name; }
		}
		public string version
		{
			get { return _version; }
		}
		public string corerequired
		{
			get { return _core_required; }
		}

		private void LoadPackageConfig()
		{
			FlatConfigFile pkgcfg = new FlatConfigFile(_dirinfo.FullName+"/pkg.cfg");
			pkgcfg.ReadConfigFile();

			_enabled = (pkgcfg.GetConfigInt("Enabled") > 0);
			_name = pkgcfg.GetConfigString("Name");
			if (pkgcfg.PropertyExists("Version"))
				_version = pkgcfg.GetConfigString("Version");
			if ( pkgcfg.PropertyExists("CoreRequired") )
				_core_required = pkgcfg.GetConfigString("CoreRequired");
		}

		public string GetPackagedConfigPath(string filename)
		{
			if (File.Exists(this.path + @"\" + filename))
				return this.path + @"\" + filename;
			else if (File.Exists(this.path + @"\config\" + filename))
				return this.path + @"\config\" + filename;
			else
				return null;
		}

		public ConfigFile LoadPackagedConfig(string filename)
		{
			ConfigFile config_file = null;

			string path = GetPackagedConfigPath(filename);
			if (path != null)
				config_file = new ConfigFile(path);
			
			return config_file;
		}
	}

	public class PackageCache
	{
		private static object _syncroot = new Object();
		private static volatile PackageCache _global;
		private Dictionary<string, POLPackage> _packages;

		private PackageCache()
		{
		}
		~PackageCache()
		{
		}

		public static PackageCache Global
		{
			get
			{
				if (_global == null)
				{
					lock (_syncroot)
					{
						if (_global == null)
						{
							_global = new PackageCache();
							_global.Initialize();
						}
					}
				}
				return _global;
			}
		}

		public List<POLPackage> packagelist
		{
			get
			{
				return new List<POLPackage>(_packages.Values);
			}
		}

		private void Initialize()
		{
			_packages = new Dictionary<string, POLPackage>();
		}

		public static POLPackage GetPackage(string name)
		{
			if (Global._packages.ContainsKey(name))
				return Global._packages[name];
			else
				return null;
		}

		public static bool LoadPackages(string rootdir)
		{
			Global._packages.Clear();
			return GetEnabledPackages(rootdir);
		}
			
		private static bool GetEnabledPackages(string rootdir)
		{
			List<string> pkg_cfgs = FileLister.FileSystemUtil.GetAllFileNames(rootdir, "pkg.cfg", SearchOption.AllDirectories);

			foreach (string filename in pkg_cfgs)
			{
				POLPackage pkg = new POLPackage(filename);
				if (pkg.enabled)
				{
					AddPackage(pkg);
				}
			}

			return true;
		}
		private static void AddPackage(POLPackage package)
		{
			Global._packages.Add(package.name, package);
		}
	}
}
