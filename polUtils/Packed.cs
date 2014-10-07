using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;

namespace POLUtils
{
    class PackedString
    {
        public static string Pack(object ob)
        {
            return Pack(ob, false);
        }
        public static string Pack(object obj, bool InArray)
        {
            if (obj == null) return "e0";

            Type objtype = obj.GetType();

            if (objtype.IsArray)
            {
                StringBuilder result = new StringBuilder();
                result.AppendFormat("a{0}:", ((Array)obj).Length);
                foreach (object elem in (Array)obj)
                {
                    result.Append(Pack(elem, true));
                }
                return result.ToString();
            }

            if (objtype == typeof(Hashtable)) // what are the others that should be here?
            {
                IDictionary dict = (IDictionary)obj;
                StringBuilder result = new StringBuilder();
                result.AppendFormat("d{0}:", dict.Keys.Count);

                foreach (DictionaryEntry elem in dict)
                {
                    result.Append(Pack(elem.Key, true));
                    result.Append(Pack(elem.Value, true));
                }
                return result.ToString();
            }

            if (objtype == typeof(System.String))
            {
                if (InArray)
                    return String.Format("S{0}:{1}", ((string)obj).Length, (string)obj);
                else
                    return "s" + (string)obj;
            }
            else if (objtype == typeof(System.Int32))
            {
                return "i" + obj.ToString();
            }
            else if (objtype == typeof(System.Double))
            {
                return "r" + ((Double)obj).ToString(NumberFormatInfo.InvariantInfo);
            }

            return "u"; // If you can´t guess...
        }

        private static object UnpackHelper(string packed)
        {
            switch (packed[0])
            {
                case 's':
                    return packed.Remove(0, 1);
                case 'i':
                    return Convert.ToInt32(packed.Remove(0, 1));
                case 'S':
                    string arg = packed.Remove(0, 1);
                    int pos = arg.IndexOf(':') + 1; // make it 1-based
                    if (pos < 2) throw new ArgumentException("S needs a length");
                    int len = Convert.ToInt32(arg.Substring(0, pos - 1));
                    return arg.Substring(pos, Math.Min(len, arg.Length - pos));
                case 'r':
                    return Convert.ToDouble(packed.Substring(1), NumberFormatInfo.InvariantInfo);

                default: return null;
            }
        }
        public static object Unpack(string packed)
        {
            switch (packed[0])
            {
                case 'a':
                    throw new NotImplementedException();
                case 'd':
                    throw new NotImplementedException();
                case 't':
                    throw new NotImplementedException();

                // basic types
                case 'i':
                case 'r':
                case 'S':
                case 's':
                    return UnpackHelper(packed);
            }

            return null;
        }
    }
}
