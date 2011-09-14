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

		public static List<POLPackage> GetEnabledPackages(string rootdir)
		{
			List<POLPackage> pkg_list = new List<POLPackage>();
			List<string> pkg_cfgs = FileLister.FileSystemUtil.GetAllFileNames(rootdir, "pkg.cfg", SearchOption.AllDirectories);

			foreach (string filename in pkg_cfgs)
			{
				POLPackage pkg = new POLPackage(filename);
				pkg_list.Add(pkg);
			}

			return pkg_list;
		}
	}
}
