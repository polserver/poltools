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
using System.IO;

namespace POLUtils.ECompile
{
    /// <summary>
    ///     Provides an interface to manipulate the Ecompile.CFG file
    /// </summary>
    public class EConfig
    {
        private List<string> ConfigFile = new List<string>();

        private string ConfigFileName = null;

        private Dictionary<string, string> Options = new Dictionary<string, string>();

        // This stores all the command line flags for the options.
        private Dictionary<string, string> OptionFlag = new Dictionary<string, string>();

        private string[] OptionsDirNames = {
                                             "ModuleDirectory",
                                             "IncludeDirectory",
                                             "PolScriptRoot",
                                             "PackageRoot", // This needs special handling
                                         };

        private string[] OptionsDefaultNames = {
                                             "GenerateListing",
                                             "GenerateDebugInfo",
                                             "GenerateDebugTextInfo",
                                             "DisplayWarnings",
                                             "CompileAspPages",
                                             "AutoCompileByDefault",
                                             "UpdateOnlyOnAutoCompile",
                                             "OnlyCompileUpdatedScripts",
                                             "DisplaySummary",
                                             "GenerateDependencyInfo",
                                             "DisplayUpToDateScripts"
                                         };

        private List<string> OptionsPackageRoots = new List<string>();

        /// <summary>
        ///     Default Constructor
        /// </summary>
        public EConfig() { BuildFlagList(); }

        private void BuildFlagList()
        {
            OptionFlag.Add("GenerateListing", "-l");
            OptionFlag.Add("GenerateDebugInfo", "-x");
            OptionFlag.Add("GenerateDebugTextInfo", "-xt");
            OptionFlag.Add("DisplayWarnings", "-w");
            OptionFlag.Add("CompileAspPages", "-a");
            OptionFlag.Add("AutoCompileByDefault", "-A");
            OptionFlag.Add("UpdateOnlyOnAutoCompile", "-Au");
            OptionFlag.Add("OnlyCompileUpdatedScripts", "-u");
            OptionFlag.Add("DisplaySummary", "-s");
            OptionFlag.Add("GenerateDependencyInfo", "-D");
        }

        /// <summary>
        ///     Opens the Ecompile.Cfg file for reading.
        /// </summary>
        /// <param name="ConfigPath">Absolute path to Config File</param>
        /// <returns>Bool</returns>
        public bool LoadConfig(string ConfigPath)
        {
            if (File.Exists(ConfigPath))
            {
                try
                {
                    StreamReader TmpConfigFile = File.OpenText(ConfigPath);
                    string Line = "";
                    while ((Line = TmpConfigFile.ReadLine()) != null) 
                    {
                        ConfigFile.Add(@Line.Trim());
                    }
                    ReadOptions();
                    ConfigFileName = ConfigPath;
                    TmpConfigFile.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            else
                return false;
        }

        private void ReadOptions()
        {
            foreach (string ConfigLine in ConfigFile)
            {
                string OptionFound = null;
                string OptionEntry = null;
                foreach (string OptionName in OptionsDirNames)
                {
                    if (ConfigLine.ToLower().Contains(OptionName.ToLower()) && !ConfigLine.Contains("#"))
                    {
                        OptionFound = OptionName;
                        break;
                    }
                }
                if (OptionFound == null)
                {
                    foreach (string OptionName in OptionsDefaultNames)
                    {
                        if (ConfigLine.ToLower().Contains(OptionName.ToLower()) && !ConfigLine.Contains("#"))
                        {
                            OptionFound = OptionName;
                            break;
                        }
                    }
                }
                if (OptionFound == null)
                    continue;
                OptionEntry = @ConfigLine.Substring(OptionFound.Length + 1);
                OptionEntry = OptionEntry.Trim();
                if (OptionFound.ToLower().Contains("packageroot"))
                {
                    OptionsPackageRoots.Add(OptionEntry);
                    continue;
                }

                if (Options.ContainsKey(OptionFound))
                {
                    Options[OptionFound].Replace(Options[OptionFound], OptionEntry);   
                }
                else
                    Options.Add(OptionFound, OptionEntry);
            }
        }

        /// <summary>
        ///     Reads commandline flag for given Option
        /// </summary>
        /// <param name="OptionName">OptionName string</param>
        /// <returns>Commandline flag in string form</returns>
        public string CommandFlag(string OptionName)
        {
            return OptionFlag[OptionName];
        }

        /// <summary>
        ///     Reads value for given Option
        /// </summary>
        /// <param name="OptionName">OptionName string</param>
        /// <returns>Option value</returns>
        public string Option(string OptionName)
        {
            return Options[OptionName];
        }

        /// <summary>
        ///     Changes the value for a given Option
        /// </summary>
        /// <param name="OptionName">OptionName string</param>
        /// <param name="NewValue">New string value to place for option</param>
        public void Option(string OptionName, string NewValue)
        {
            Options[OptionName] = NewValue;
        }

        /// <summary>
        ///     Changes the value for a given Option
        /// </summary>
        /// <param name="OptionName">OptionName string</param>
        /// <param name="NewValue">Bool Value to be converted to 0/1 String</param>
        public void Option(string OptionName, bool NewValue)
        {
            if (NewValue)
                Options[OptionName] = "1";
            else
                Options[OptionName] = "0";
        }

        /// <summary>
        ///     List of Package Roots
        /// </summary>
        /// <returns>A generic List of string values</returns>
        public List<string> GetPackageRoots()
        {
            return OptionsPackageRoots;
        }

        /// <summary>
        ///     Adds a new Value to the Package Roots List
        /// </summary>
        /// <param name="NewValue">New Value to add</param>
        public bool AddPackageRootItem(string NewValue)
        {
            try
            {
                OptionsPackageRoots.Add(NewValue);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Remove a specific String Value from the List
        /// </summary>
        /// <param name="Value">String Value to remove</param>
        public bool RemovePackageRootItem(string Value)
        {
            try
            {
                OptionsPackageRoots.Remove(Value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Clears all Values from the List
        /// </summary>
        /// <returns>Bool for success</returns>
        public bool RemoveAllPackageRootItems()
        {
            try
            {
                OptionsPackageRoots.Clear();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        ///     Saves the Ecompile.Cfg back to file with any changes. File Output
        ///     is tabbed so that settings are in a straight column and all original
        ///     lines that were NOT settings are retained as original.
        /// </summary>
        /// <returns>Bool for success</returns>
        public bool SaveConfig()
        {
            // Save Order: Directory Options, Package Roots, Base Options.
            // We saved the ConfigFile in it's entirety. Let's just parse
            // through it to retain any custom comments etc. 
            string[] SaveContents = new string[ConfigFile.Count];
            ConfigFile.CopyTo(SaveContents);
            int LineIndex = 0;
            foreach (string ConfigLine in ConfigFile)
            {
                foreach (string OptionName in OptionsDirNames)
                {
                    if (ConfigLine.ToLower().Contains(OptionName.ToLower()) && !ConfigLine.Contains("#") && OptionName != "PackageRoot")
                    {
                        // We found a Directory Option! REPLACE IT!
                        int i = (28 - OptionName.Length);
                        string TabString = "";
                        while (i > 0)
                        {
                            TabString += "\t";
                            i -= 4;
                        }
                        SaveContents[LineIndex] = OptionName + TabString + Options[OptionName];
                        break;
                    }
                }
                // Now we add all the Package Roots. BOOO

                // Now we add all the Default Option Stuff
                foreach (string OptionName in OptionsDefaultNames)
                {
                    if (ConfigLine.ToLower().Contains(OptionName.ToLower()) && !ConfigLine.Contains("#"))
                    {
                        // We found a Default Option! REPLACE IT!
                        int i = (28 - OptionName.Length);
                        string TabString = "";
                        while(i > 0)
                        {
                            TabString += "\t";
                            i -= 4;
                        }
                        SaveContents[LineIndex] = OptionName + TabString + Options[OptionName];
                        break;
                    }
                }
                LineIndex++;
            }

            // Now we write it all back to the ecompile.cfg. OHHH! AHHH! Go away.
            if (ConfigFileName != null)
            {
                try
                {
                    File.WriteAllLines(ConfigFileName, SaveContents);
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            return false;
        }

    }
}
