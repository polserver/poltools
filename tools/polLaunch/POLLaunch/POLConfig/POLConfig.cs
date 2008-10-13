using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

//
// Note: This is not how config files are used in the pol core.
// Its much more advanced and WAY faster, there. 
//
// To make it write safe (not lose comments)
// Config File [hash] (can contain a config elem or a config line class) 
// Config Elem [hash] (contains either a config line class, or an array of them)
// Config Line -> A struct containing 2 strings, a value and a comment
//
// POLConfigFile also has a LIST to make sure the file doesn't 
// lose the order of how things were read in.
//

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
		Hashtable _entries = new Hashtable(); // Stores actual data.
		List<object> _write_order;

		public POLConfig(string path): this(path, FlagOpts.read_structured)
		{
		}

		public POLConfig(string path, FlagOpts flags)
		{
			_flags = _flags | flags;
			_path = path;
			
			ReadConfigFile();
		}

		public string Path
		{
			get { return _path; }
		}

		public FlagOpts Flags
		{
			get { return _flags; }
		}

		public bool ReadConfigFile()
		{
			if (!File.Exists(_path))
				return false;
			try
			{
				StreamReader sr = new StreamReader(_path);
				string line = "";
				while ((line = sr.ReadLine()) != null)
				{
					if (line[0] == '#' || (line.Substring(0, 1) == @"//" ) ) // Comment line outisde an elem
						continue;

					//Remove any leading white space.
					line.TrimStart(new char[]{' ','\t'});
				}
				sr.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "POLConfig ReadConfigFile() Error");
				return false;
			}
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
		private string _elem_name = "";
		private Hashtable _properties = new Hashtable();

		public POLConfigElem(string elem_name)
		{
			_elem_name = elem_name;
		}

		public String ElemName
		{
			get { return _elem_name; }
		}

		public string[] ListConfigElemProps()
		{
			string[] keys = new string[_properties.Count];
			int pos = 0;
			foreach (string key in _properties.Keys)
				keys[pos++] = key;
			return keys;
		}

		public int GetConfigInt(string property_name)
		{
			if (_properties.ContainsKey(property_name))
				return (int)_properties[property_name];
			else
				return 0;
		}

		public double GetConfigFloat(string property_name)
		{
			if (_properties.ContainsKey(property_name))
				return (double)_properties[property_name];
			else
				return 0.0;
		}

		public string GetConfigString(string property_name)
		{
			if (_properties.ContainsKey(property_name))
				return (string)_properties[property_name];
			else
				return "";
		}
	}

	struct POLConfigLine
	{
		public string _value;
		public string _comments;

		POLConfigLine(string value, string comments)
		{
			_value = value;
			_comments = comments;
		}
	}
}
