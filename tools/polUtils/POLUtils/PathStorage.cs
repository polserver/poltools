/***************************************************************************
 *
 * $Author: MuadDib
 *
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/
using System;
using System.Collections.Generic;

namespace POLUtils.PathStorage
{
    /// <summary>
    ///     Class to handle storage/retrieval of Path Settings.
    /// </summary>
    public class PathStorage
    {
        /// <summary>
        ///     Dictionary to contain all the Pathname storage.
        /// </summary>
        /// <key>Path social name string</key>
        /// <value>Absolute Path String</value>
        private static Dictionary<string, string> PathSettings = new Dictionary<string, string>();

        /// <summary>
        ///     Default Constructor. Initializes the following Paths Storage Names:
        ///     UOPath
        ///     MULPath
        ///     POLPath
        ///     POLExePath
        ///     UOConvertExePath
        ///     ECompileExePath
        ///     RunECLExePath
        ///     POLConfigPath
        ///     POLDataPath
        ///     POLLogPath
        ///     POLRealmPath
        /// </summary>
        public PathStorage() { InitPathSettings(); }

        private static void InitPathSettings()
        {
            // I am lazy right now, sue me.
            PathSettings.Add("UOPath", null);
            PathSettings.Add("MULPath", null);
            PathSettings.Add("POLPath", null);
            PathSettings.Add("POLExePath", null);
            PathSettings.Add("POLCfgPath", null);
            PathSettings.Add("UOConvertExePath", null);
            PathSettings.Add("UOConvertCfgPath", null);
            PathSettings.Add("ECompileExePath", null);
            PathSettings.Add("ECompileCfgPath", null);
            PathSettings.Add("RunECLExePath", null);
            PathSettings.Add("POLConfigPath", null);
            PathSettings.Add("POLDataPath", null);
            PathSettings.Add("POLLogPath", null);
            PathSettings.Add("POLRealmPath", null);
        }

        /// <summary>
        ///     Get all Path Social Names From The Path Storage.
        /// </summary>
        /// <return>A String Array of all the Key Values for the Path Storage.</return>
        public string[] GetPathNames()
        {
            string[] PathNames = new string[PathSettings.Count];
            int position = 0;
            foreach (KeyValuePair<string, string> KVP in PathSettings)
            {
                PathNames.SetValue(KVP.Key.ToString(), position);
                position++;
            }
            return PathNames;
        }

        /// <summary>
        ///     Get the Path stored for a specific PathName.
        /// </summary>
        /// <param name="PathName">PathName to look up the Path for.</param>
        public string GetPath(string PathName)
        {
            if (PathSettings.ContainsKey(PathName))
            {
                return PathSettings[PathName].ToString();
            }
            else
                throw new NotSupportedException("PathName supplied to GetPath() is not contained in Path Settings.");
        }

        /// <summary>
        ///     Sets the Path stored for a specific PathName.
        /// </summary>
        /// <param name="PathName">PathName to set the Path for.</param>
        /// <param name="NewValue">New string value to set the Path for.</param>
        public bool SetPath(string PathName, string NewValue)
        {
            if (PathSettings.ContainsKey(PathName))
            {
                PathSettings[PathName] = NewValue;
                return true;
            }
            else
                throw new NotSupportedException("PathName supplied to SetPath() is not contained in Path Settings.");
        }

        /// <summary>
        ///     Bool for if a PathName contains any data.
        /// </summary>
        /// <param name="PathName">PathName to check.</param>
        public bool IsSet(string PathName)
        {
            if (PathSettings.ContainsKey(PathName))
            {
                if (PathSettings[PathName] != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                throw new NotSupportedException("PathName supplied to IsSet() is not contained in Path Settings.");

        }

        /// <summary>
        ///     Adds a new PathName to the Path Settings.
        /// </summary>
        /// <param name="PathName">PathName to add.</param>
        /// <param name="Value">Value to add to the PathName.</param>
        public bool AddPath(string PathName, string Value)
        {
            if (!PathSettings.ContainsKey(PathName))
            {
                try
                {
                    PathSettings.Add(PathName, Value);
                    return true;
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///     Remove a PathName from the Path Settings.
        /// </summary>
        /// <param name="PathName">PathName to remove.</param>
        public bool RemovePath(string PathName)
        {
            if (PathSettings.ContainsKey(PathName))
            {
                try
                {
                    PathSettings.Remove(PathName);
                    return true;
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
