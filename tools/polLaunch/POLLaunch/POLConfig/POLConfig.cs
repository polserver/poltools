using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;


namespace POLConfig
{
	class POLConfig
	{
		public enum FlagOpts
		{
			read_structured = 0x1,
			read_flat = 0x2,
		}

		FlagOpts _flags = 0x0;
		private string _path = "";		

		public POLConfig(string path): this(path, FlagOpts.read_structured)
		{
		}

		public POLConfig(string path, FlagOpts flags)
		{
			_flags = _flags | flags;
			_path = path;
			MessageBox.Show("Flags=" + _flags.ToString());
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
