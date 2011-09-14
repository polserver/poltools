using System;
using System.IO;
using ConfigUtil;

namespace POLTools.Package
{
	public class POLPackage
	{
		protected DirectoryInfo _dirinfo;
		protected bool _enabled;
		protected string _name;
		protected double _version;
		protected double _core_required;
		
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
				throw ex;
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
		public double version
		{
			get { return _version; }
		}
		public double corerequired
		{
			get { return _core_required; }
		}

		private void LoadPackageConfig()
		{
			FlatConfigFile pkgcfg = new FlatConfigFile(_dirinfo.FullName+"/pkg.cfg");
			pkgcfg.ReadConfigFile();

			_enabled = (pkgcfg.GetConfigInt("Enabled") > 0);
			_name = pkgcfg.GetConfigString("Name");
			_version = pkgcfg.GetConfigDouble("Version");
			_core_required = pkgcfg.GetConfigDouble("CoreRequired");
		}

	}
}
