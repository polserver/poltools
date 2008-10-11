using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;


namespace POLConfig
{
	class POLConfig
	{
		const uint FLAG_STRUCTUREDCFG = 0x1;
		const uint FLAG_FLATCFG = 0x2;

		private uint _flags = 0x0;
		private string _path = "";		

		public POLConfig(string path): this(path, FLAG_STRUCTUREDCFG)
		{
		}

		public POLConfig(string path, uint flags)
		{
			_flags = flags;
			_path = path;

			ReadConfigFile();
		}

		public bool ReadConfigFile()
		{
			if (!File.Exists(_path))
				return false;
			return true;
		}
	}
}
