using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;

//
// Note: This is not how config files are used in the pol core.
// Its much more advanced and WAY faster, there. 
//

namespace POLConfig
{
	class POLConfig
	{
		Hashtable _entries = new Hashtable();
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
			
			ReadConfigFile();
		}

		public bool ReadConfigFile()
		{
			if (!File.Exists(_path))
				return false;
			return true;
		}

		public void FindConfigElem(string elem_name)
		{
			// Will implement a ConfigElem class
		}

		public int GetConfigMaxIntKey()
		{
			int highest = 0;
			foreach (int key in _entries.Keys)
			{
				if (key > highest)
					highest = key;
			}
			return highest;
		}

		public string[] GetConfigStringKeys()
		{
			string[] keys = new string[_entries.Count];
			int pos = 0;
			foreach (string key in _entries.Keys)
				keys[pos++] = key;
			return keys;
		}

		public int[] GetConfigIntKeys()
		{
			int[] keys = new int[_entries.Count];
			int pos = 0;
			foreach (int key in _entries.Keys)
				keys[pos++] = key;
			return keys;
		}
	}

	class POLConfigElem
	{
		string _elem_name = "";
		Hashtable _properties = new Hashtable();
	}
}
