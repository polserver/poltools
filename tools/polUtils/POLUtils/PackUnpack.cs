/***************************************************************************
 *
 * $Author: Nando
 *
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;


namespace POLUtils.PackUnpack
{
    /// <summary>
    ///     Class for Packing and Unpacking POL Packed Data Strings typcially used
    ///     in Aux Service Communications and CProp Storage in Config Files.
    /// </summary>
    public class PackUnpack
    {

        /// <summary>
        ///     The default constructor is marked as private to
        ///     ensure nothing funky. :)
        /// </summary>
        private PackUnpack()
        {
        }

        /// <summary>
        ///     Attempts to convert known Objects to a POL Style Packed Data String.
        ///     Known Objects:
        ///         Array
        ///         Dictionary
        ///         String
        ///         Integer
        ///         Double
        /// </summary>
        /// <param name="ob">
        ///     The Object to convert to POL Packed Data (Such as String, Int, Dictionary, etc)
        /// </param>
        public static string Pack(object ob)
        {
            return Pack(ob, false);
        }

        /// <summary>
        ///     Attempts to convert known Objects to a POL Style Packed Data String.
        /// </summary>
        /// <param name="obj">
        ///     The Object to convert to POL Packed Data (Such as String, Int, Dictionary, etc)
        /// </param>
        /// <param name="InArray">
        ///     Tells the function if this Object was inside of an Array
        /// </param>
        private static string Pack(object obj, bool InArray)
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

        /// <summary>
        ///     Attempts to convert a POL Packed Data String to the equivelant C# Object.
        ///     Known Objects:
        ///         Array
        ///         String
        ///         Integer
        ///         Double
        /// </summary>
        /// <param name="packed">
        ///     The POL Packed Data String to convert.
        /// </param>
        public static object Unpack(string packed)
        {
            switch (packed[0])
            {
                case 'd':
                    throw new NotImplementedException();
                case 't':
                    throw new NotImplementedException();

                // basic types
                case 'a':
                case 'i':
                case 'r':
                case 'S':
                case 's':
                    return UnpackHelper(packed);
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a POL Packed Data String to the equivelant C# Object.
        /// </summary>
        /// <param name="packed">
        ///     The POL Packed Data String to convert.
        /// </param>
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
                case 'a':
                    string PLine = packed.Remove(0, 1); // removes 'a'
                    ArrayList result = new ArrayList();
                    int ALPos = PLine.IndexOf(':');
                    if (ALPos < 1) throw new ArgumentException("a needs a length");
                    int PALength = Convert.ToInt32(PLine.Substring(0, ALPos));
                    PLine = PLine.Remove(0, PLine.IndexOf(':')+1); // Remove a<length>:
                    int counter;
                    for (counter = 0; counter < PALength; counter++ )
                    {
                        // Get current element to unpack.
                        // ElemLPos is invalid for 'r' and 'i'
                        int ElemLPos = PLine.IndexOf(':');
                        switch (PLine[0])
                        {
                            case 'a':
                                throw new NotImplementedException();
                            case 'd':
                                throw new NotImplementedException();
                            case 't':
                                throw new NotImplementedException();

                            // basic types
                            case 'r':
                            case 'i':
                                int ElemEnd = 0;
                                for (int j = 1; j < PLine.Length; j++ )
                                {
                                    switch (PLine[j])
                                    {
                                        case 'd':
                                        case 't':
                                        case 'a':
                                        case 'i':
                                        case 'r':
                                        case 'S':
                                        case 's':
                                            // We found the next entry, stop here.
                                            ElemEnd = j-1;
                                            break;
                                    }
                                }
                                if (ElemEnd < 1)
                                {
                                    // We are on the LAST element of the packed string
                                    ElemEnd = PLine.Length-1; // Since last, use the Length.
                                }
                                if (PLine[0] == 'r')
                                {
                                    result.Add(Convert.ToDouble(PLine.Substring(1, ElemEnd)));
                                }
                                else
                                {
                                    result.Add(Convert.ToInt32(PLine.Substring(1, ElemEnd)));
                                }
                                PLine = PLine.Remove(0, (ElemEnd+1)); // remove the element
                                break;
                            case 'S':
                                PLine = PLine.Remove(0, 1);
                                int ElemPos = PLine.IndexOf(':') + 1; // make it 1-based
                                if (ElemPos < 2) throw new ArgumentException("S needs a length");
                                int ElemLen = Convert.ToInt32(PLine.Substring(0, ElemPos - 1));
                                result.Add(PLine.Substring(ElemPos, Math.Min(ElemLen, PLine.Length - ElemPos)));
                                PLine = PLine.Remove(0, (ElemLen + ElemPos));
                                break;
                        }

                    }
                    return result;
                    
                default: return null;
            }
        }
    }
}
