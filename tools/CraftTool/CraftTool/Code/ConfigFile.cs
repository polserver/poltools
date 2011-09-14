using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;

/*
 *	ConfigUtil.ConfigFile configfile = new ConfigUtil.ConfigFile();
 *	configfile.ReadConfigFile(filestream);
 *
 *	List<string> elemnames = configfile.GetConfigElemNames();
 *	foreach (string name in elemnames)
 *	{
 *		this.textBox1.AppendText(name + Environment.NewLine + "{"+Environment.NewLine);
 *		var elem = configfile.GetConfigElem(name);
 *		var propnames = elem.ListConfigElemProperties();
 *		foreach (string pname in propnames)
 *		{
 *			var values = elem.GetConfigStringList(pname);
 *			foreach (var value in values)
 *			{
 *				this.textBox1.AppendText("	" + pname + "	" + value + Environment.NewLine);
 *			}
 *		}
 *		this.textBox1.AppendText("}" + Environment.NewLine);
 *	}
 * 
 */

namespace ConfigUtil
{
	public class ConfigFile
	{
		protected List<ConfigElem> _cfgelems;
		string _filename;

		public ConfigFile() : this("")
		{
		}
		public ConfigFile(string filename)
		{
			_filename = filename;
			_cfgelems = new List<ConfigElem>();
		}
		~ConfigFile()
		{
		}

		public List<ConfigElem> GetConfigElemRefs()
		{
			return _cfgelems;
		}

		public List<string> GetConfigElemNames()
		{
			List<string> names = new List<string>();
			foreach (ConfigElem elem in _cfgelems)
			{
				names.Add(elem.name);
			}

			return names;
		}

		public ConfigElem GetConfigElem(string elem_name)
		{
			if (!_cfgelems.Exists(delegate(ConfigElem n) { return n.name == elem_name; }))
				throw new Exception("Could not find config element '" + elem_name + "'");

			return _cfgelems.Find(item => item.name == elem_name);
		}

		public bool ReadConfigFile()
		{
			try
			{
				StreamReader sr = new StreamReader(_filename);
				return ReadConfigFile(sr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public bool ReadConfigFile(string contents)
		{
			try
			{
				byte[] bytearr = Encoding.ASCII.GetBytes(contents);
				MemoryStream stream = new MemoryStream(bytearr);
				StreamReader sr = new StreamReader(stream);
				return ReadConfigFile(sr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public bool ReadConfigFile(StreamReader sr)
		{
			string curline = String.Empty;
			ConfigElem curelem = null;
			int line_num = 1;
			bool in_elem = false;
			while ((curline = sr.ReadLine()) != null)
			{
				//Remove any leading or trailing white space.
				curline = curline.Trim(new char[] { ' ', '\t' });

				if (curline.Length == 0) // Blank line
					continue;
				if (curline.StartsWith("#")) // Skip comment lines.
					continue;
				else if (curline.StartsWith("//")) // Skip comment lines
					continue;
				else if (curelem == null) 
				{
					// FIX-ME!
					// What if there is "Elem Name" then another "Elem Name" and finally a { ?

					// Data not in an  { } element. Should be the element's name.
					CfgPair pair = CfgPair.ParseCfgLine(curline);
					curelem = new ConfigElem(pair.first, pair.second.ToString());
					_cfgelems.Add(curelem);
				}
				else if (curline[0] == '{')
				{
					if ( in_elem ) // Sanity check
					{
						throw new Exception("ReadConfigFile() - Fail on line "+line_num+"."+Environment.NewLine+"Found an open '{' before closing previous elem.");
						//return false;
					}
					if ( curelem == null ) // Sanity check
					{
						throw new Exception("ReadConfigFile() - Fail on line "+line_num+"."+Environment.NewLine+"Found an open '{' with no config elem name set.");
						//return false;
					}
					in_elem = true;
				}
				else if ( curline[0] == '}')
				{
					curelem = null;
					in_elem = false;
				}
				else if (curelem != null && in_elem)
				{
					CfgPair pair = CfgPair.ParseCfgLine(curline);
					curelem.AddConfigLine(pair);
				}
				else
				{
					throw new Exception("ReadConfigFile() - Fail on line " + line_num + "." + Environment.NewLine + "Found data outside of a config elem.");
				}

				line_num++;
			}

			return true;
		}
	}

	public class ConfigElem
	{
		protected List<CfgPair> _cfgpairs;
		protected string _type;
		protected string _name;

		public ConfigElem(string type, string name)
		{
			_type = type;
			_name = name;

			_cfgpairs = new List<CfgPair>();
		}
		~ConfigElem()
		{
		}

		public string type
		{
			get
			{
				return _type;
			}
		}
		public string name
		{
			get
			{
				return _name;
			}
		}

		public void AddConfigLine(string key, string value)
		{
			CfgPair pair = new CfgPair(key, value);
			_cfgpairs.Add(pair);
		}
		public void AddConfigLine(CfgPair pair)
		{
			_cfgpairs.Add(pair);
		}

		public object GetConfigObject(string key)
		{
			key = key.ToLower();
			CfgPair pair = _cfgpairs.Find(item => item.first.ToLower() == key);
			return pair.second;
		}

		public string GetConfigString(string key)
		{
			object value = GetConfigObject(key);
			return value.ToString();
		}

		public long GetConfigInt(string key)
		{
			object value = GetConfigObject(key);
			return Convert.ToInt64(value, System.Globalization.NumberFormatInfo.InvariantInfo);
		}

		public double GetConfigDouble(string key)
		{
			object value = GetConfigObject(key);
			return Convert.ToDouble(value, System.Globalization.NumberFormatInfo.InvariantInfo);
		}

		public List<object> GetConfigObjectList(string key)
		{
			key = key.ToLower();
			List<object> objects = new List<object>();
			
			foreach (CfgPair pair in _cfgpairs)
			{
				if (pair.first.ToLower() == key)
				{
					objects.Add(pair.second);
				}
			}
			return objects;
		}

		public List<string> GetConfigStringList(string key)
		{
			return GetConfigObjectList(key).ConvertAll<string>(delegate(object x) { return x.ToString(); });
		}

		public List<string> ListConfigElemProperties()
		{
			List<string> properties = new List<String>();
			foreach (CfgPair pair in _cfgpairs)
			{
				if ( !properties.Exists(delegate(string n) { return n == pair.first.ToLower(); }) )
				{
					properties.Add(pair.first.ToLower());
				}
			}
			return properties;
		}		
	}

	public struct CfgPair
	{
		string _first;
		object _second;

		public CfgPair(string key, string value)
		{
			_first = key;
			_second = value;
		}

		public string first
		{
			get
			{
				return _first;
			}
		}
		public object second
		{
			get
			{
				return _second;
			}
		}

		public static CfgPair ParseCfgLine(string line)
		{
			string propname = String.Empty;
			string value = String.Empty;
			int pos = 0;
			
			// Ensure there is no B.S. white space
			line = line.Trim(new char[] { ' ', '\t' });

			if ((pos = line.IndexOf('#')) > 0) // Read up until there is a comment on the line.
			{
				line = line.Substring(0, pos);
			}
			else if ((pos = line.IndexOf("//")) > 0) // Read up until there is a comment on the line.
			{
				line = line.Substring(0, pos);
			}
			
			if ( (pos = line.IndexOfAny(new char[]{' ','\t'})) > 0 ) // Read the line to the first instance of whitespace for the 'key'
			{
				propname = line.Substring(0, pos);
				value = line.Substring(pos+1); // Will contain whitespace between key and value
				value = value.Trim(new char[] { ' ', '\t' }); // Eliminate the oreo cream filling.
			}
			else // There was no whitespace so it is a property with no value.
			{
				propname = line;
				value = "";
			}
			return new CfgPair(propname, value);
		}
	}
}
