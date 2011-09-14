using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ObjtypesTxt
{
	class ObjTypesTxt
	{
		protected List<ObjTypeRecord> _records;
		string _filename;

		public ObjTypesTxt(string filename)
		{
			_filename = filename;
			_records = new List<ObjTypeRecord>();
		}
		~ObjTypesTxt()
		{
		}

		public string filename
		{
			get { return _filename; }
		}

		public List<ObjTypeRecord> records
		{
			get { return _records; }
		}

		public bool ReadFile()
		{
			if ( !File.Exists(_filename) )
				throw new Exception("ReadFile() - File '"+_filename+"' does not exist.");

			try
			{
				StreamReader sr = new StreamReader(_filename);
				return ReadFile(sr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public bool ReadFile(StreamReader sr)
		{
			string curline = String.Empty;
			int line_num = 1;
			while ((curline = sr.ReadLine()) != null)
			{
				if (curline.StartsWith("#"))
					continue;

				ObjTypeRecord record = ObjTypeRecord.ParseRecordString(curline);

				_records.Add(record);
				line_num++;
			}

			return true;
		}

		public bool ContainsObjType(string hexobjtype)
		{
			if ( !OnlyHexInString(hexobjtype) )
				return false;
			
			hexobjtype = hexobjtype.Trim();
			return ContainsObjType(Convert.ToInt32(hexobjtype, 16));
		}
			
		public bool ContainsObjType(long objtype)
		{
			if (!_records.Exists(delegate(ObjTypeRecord n) { return n.objtype == objtype; }))
				return false;

			return true;
		}

		public ObjTypeRecord GetRecordByObjType(string hexobjtype)
		{
			if (!OnlyHexInString(hexobjtype))
				return null;

			hexobjtype = hexobjtype.Trim();
			return GetRecordByObjType(Convert.ToInt32(hexobjtype, 16));
		}
		public ObjTypeRecord GetRecordByObjType(long objtype)
		{
			if (!_records.Exists(delegate(ObjTypeRecord n) { return n.objtype == objtype; }))
				return null;

			return _records.Find(record => record.objtype == objtype);
		}

		protected bool OnlyHexInString(string test)
		{
			return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-FxX]+\b\Z");
		}

		public override string ToString()
		{
			string text = string.Empty;
			foreach (ObjTypeRecord record in _records)
				text += record.ToString() + Environment.NewLine;

			return text;
		}
	}

	class ObjTypeRecord
	{
		protected long _objtype;
		protected string _itemname;
		protected string _package;

		public ObjTypeRecord(long objtype, string itemname, string package)
		{
			_objtype = objtype;
			_itemname = itemname;
			_package = package;
		}

		public string objtypehex
		{
			get { return "0x"+_objtype.ToString("X"); }
		}
		public long objtype
		{
			get { return _objtype; }
		}
		public string itemname
		{
			get { return _itemname; }
		}
		public string package
		{
			get { return _package; }
		}

		static public ObjTypeRecord ParseRecordString(string line)
		{
			line = line.Trim();
			string[] split = line.Split(new Char[] {' ', '\t'}, 3);
			List<string> list = new List<string>(split);

			var objnumber = Convert.ToInt32(list[0], 16);
			string itemname = list[1];
			string package = string.Empty;
			if (list.Count > 2 )
				package = list[2];
			ObjTypeRecord record = new ObjTypeRecord(objnumber, itemname, package);

			return record;
		}

		public override string ToString()
		{
			return objtypehex + " " + _itemname + " " + _package;
		}
	}
}

