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
	public class POLConfigFile
	{
		public enum FlagOpts
		{
			read_structured = 0x1,
			read_flat = 0x2,
		}

		private FlagOpts _flags = 0x0;
		private string _path = "";
		private bool _read = false;
		private Hashtable _entries = new Hashtable(); // Stores actual data.
		private List<object> _write_order = new List<object>(); // Stores the order of data (POLConfigLine or POLConfigElem)

		public POLConfigFile(string path): this(path, FlagOpts.read_structured)
		{
		}

        static public bool SplitLine(string line, out string propname, out string value, out string comment)
        {
            propname = "";
            value = "";
            comment = "";
            
            int pos = 0;

            if ((pos = line.IndexOf('#')) > 0) {
                comment = line.Substring(pos);
                line = line.Substring(0, pos);
            }
            else if ((pos = line.IndexOf("//")) > 0)
            {
                comment = line.Substring(pos);
                line = line.Substring(0, pos);
            }

            if ((pos = line.IndexOf(' ')) > 0) // first space separates prop from value
            {
                propname = line.Substring(0, pos);
                value = line.Substring(pos+1);
            }
            else
            {
                propname = line; // if there´s no space, just make propname what is on the line
                value = "";
            }

            return true;
        }

        static bool IsComment(string line)
        {
            if (line[0] == '#' || line.Substring(0, 2) == @"//")
                return true;
            
            return false;
        }

		public POLConfigFile(string path, FlagOpts flags)
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

        public List<Object> Structure {
         get { return _write_order;}
        }

		public bool ReadConfigFile()
		{
			if (!File.Exists(_path))
				return false;
			else if (_read)
				return false;
			else
				_read = true;

			try
			{
				StreamReader sr = new StreamReader(_path);
				string line = "";
				POLConfigElem cur_elem = null;
				while ((line = sr.ReadLine()) != null)
				{
					//Remove any leading or trailing white space.
					line = line.Trim(new char[] { ' ', '\t' });

                    if (line.Length == 0)
                    {
                        if (cur_elem == null)
                            _write_order.Add(new POLConfigLine(null, ""));
                        else
                            cur_elem.AddComment("");

                        continue;
                    }

					if ( cur_elem == null )
                    {
                        if (IsComment(line)) // Comment line outside an elem
                            _write_order.Add(new POLConfigLine(null, line));
                        {
                            int pos = 0;
                            if ((pos = line.IndexOf('{')) > 0)
                            {
                                cur_elem = new POLConfigElem(line.Substring(0, pos));
                            }
                        }
                    }
					else
					{
                        if (line[0] == '}')
                        {
                            _write_order.Add(cur_elem);
                            cur_elem = null;
                        }
                        else if (IsComment(line))
                            cur_elem.AddComment(line);
                        else
                        {
                            string name = "";
                            string value = "";
                            string comment = "";
                            if (SplitLine(line, out name, out value, out comment))
                            {
                                cur_elem.AddProperty(name, value, comment);
                            }
                        }
					}				
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
        
        public void DumpConfigStream(TextWriter stream)
        {
            if (!_read)
                return;

            foreach (object o in this.Structure)
            {
                if (o.GetType() == typeof(POLConfigElem))
                {
                    if (((POLConfigElem)o).Prefix != "")
                        stream.WriteLine("{0} {1} {2}", ((POLConfigElem)o).Prefix, ((POLConfigElem)o).ElemName, '{');
                    else
                        stream.WriteLine("{0} {1}", ((POLConfigElem)o).ElemName, '{');

                    foreach (object x in ((POLConfigElem)o).Structure)
                    {
                        stream.WriteLine("\t {0}", x.ToString());
                    }

                    stream.WriteLine("}");
                }
                else stream.WriteLine(o.ToString());
            }
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
        private string _elem_prefix = "";
		private string _elem_name = "";
		private Hashtable _properties = new Hashtable(); // Stores POLConfigLine() values
		List<object> _write_order = new List<object>(); // Stores the order of data (POLConfigLine)

		public POLConfigElem(string elem_name)
		{
            elem_name = elem_name.Trim(new char[] {' ', '\t'});
            string[] names = elem_name.Split(' ');
            if (names.Length > 1)
            {
                _elem_prefix = names[0];
                _elem_name = names[1];
            }
            else 
            {
                _elem_prefix = "";
                _elem_name = elem_name;
            }

			//_elem_name = elem_name;
		}

		public String ElemName
		{
			get { return _elem_name; }
		}
        public String Prefix
        {
            get { return _elem_prefix; }
        }

        public List<object> Structure
        {
            get { return _write_order; }
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
				return Convert.ToInt32(_properties[property_name]);
			else
				return 0;
		}

		public double GetConfigFloat(string property_name)
		{
			if (_properties.ContainsKey(property_name))
				return Convert.ToDouble(_properties[property_name], System.Globalization.NumberFormatInfo.InvariantInfo);
			else
				return 0.0;
		}

		public string GetConfigString(string property_name)
		{
			if (_properties.ContainsKey(property_name))
				return Convert.ToString(_properties[property_name]);
			else
				return "";
		}

        public void AddProperty(string name, string value)
        {
            AddProperty(name, value, "");
        }
        public void AddProperty(string name, string value, string comment)
        {
            _write_order.Add(new POLConfigLine(name, value, comment));
            if (!_properties.ContainsKey(name))
                _properties[name] = new POLConfigLine(name, value, comment);
        }

		public void AddComment(string comment)
		{
			_write_order.Add(new POLConfigLine(null, comment));
		}
	}

	struct POLConfigLine
	{
        public string _name;
		public string _value;
		public string _comments;

		public POLConfigLine(string name, string value, string comments)
		{
            _name = name;
			_value = value;
			_comments = comments;
		}
        public POLConfigLine(string value, string comments) : this(null, value, comments) 
        { }
        public override string ToString()
        {
            if (_name != "")
                return _name + " " + _value + _comments;
            else
                return _value + _comments;
        }
	}

	struct POLElemKey
	{
		public string _prefix;
		public string _suffix;

		public POLElemKey(string prefix, string suffix)
		{
			_prefix = prefix;
			_suffix = suffix;
		}
	}
}
