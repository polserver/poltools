using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using POLUtils.ECompile;

namespace POLLaunch.DataBackup
{
    public class ZipFileArchiver
    {
        #region Constructors
        /// <summary>
        /// Base constructor - initializes all fields to default values
        /// </summary>
        public ZipFileArchiver()
        {
            // Do nothing.
        }

        #endregion
        #region Argument Parsing
        /// <summary>
        ///     Build the fileSpecs_ List of files to add to the archive.
        /// </summary>		
        /// <returns>
        ///     true if it was able to fully build the list.
        /// </returns>
        bool BuildFileList()
        {
            if (bool.Parse(Settings.Global.DataBackup["BackupData"]))
            {
                // Ok, they want to backup the Data Directory. Let's Scan this
                // directory to get all the files and move on.
                foreach (string filename in FileSystemUtil.GetAllFileNames(Settings.Global.Properties["POLPath"] + @"\data"))
                {
                    fileSpecs_.Add(filename);
                }
            }
            if (bool.Parse(Settings.Global.DataBackup["BackupScripts"]))
            {
                // Ok, they want to backup all the Scripts. Let's Scan this
                // directory to get all the files and move on. Let's remember, we also
                // need to check everything within each Package Root. OH MY!
                fileSpecs_.Add(Settings.Global.Properties["POLPath"]+@"\pol.cfg");
                fileSpecs_.Add(Settings.Global.Properties["POLPath"] + @"\uoconvert.cfg");
                foreach (string filename in FileSystemUtil.GetAllFileNames(Settings.Global.Properties["POLPath"] + @"\config"))
                {
                    fileSpecs_.Add(filename);
                }
                foreach (string filename in FileSystemUtil.GetAllFileNames(Settings.Global.Properties["POLPath"] + @"\regions"))
                {
                    fileSpecs_.Add(filename);
                }
                foreach (string filename in FileSystemUtil.GetAllFileNames(Settings.Global.Properties["POLPath"] + @"\scripts"))
                {
                    fileSpecs_.Add(filename);
                }
                // Now we read the PackageRoot Entries from Ecompile.Cfg in order to get
                // all the user's "pkg" directories. OH YAY!
                EConfig MyConfig = new EConfig();
                MyConfig.LoadConfig(Settings.Global.Properties["ECompileCfgPath"]);
                foreach (string pathname in MyConfig.GetPackageRoots())
                {
                    foreach (string filename in FileSystemUtil.GetAllFileNames(pathname))
                    {
                        fileSpecs_.Add(filename);
                    }
                }
            }
            if (bool.Parse(Settings.Global.DataBackup["BackupRealms"]))
            {
                foreach (string filename in FileSystemUtil.GetAllFileNames(Settings.Global.Properties["POLPath"] + @"\realm"))
                {
                    fileSpecs_.Add(filename);
                }
            }
            if (bool.Parse(Settings.Global.DataBackup["BackupLogs"]))
            {
                foreach (string filename in FileSystemUtil.GetAllFileNames(Settings.Global.Properties["POLPath"] + @"\log"))
                {
                    fileSpecs_.Add(filename);
                }
            }
            return (fileSpecs_.Count > 0);
        }

        #endregion
        #region Creation
        /// <summary>
        /// Create archives based on specifications passed and internal state
        /// </summary>		
        void Create(string zipFileName, ArrayList fileSpecs, ref ToolStripProgressBar ProgressBar)
        {
            if (Path.GetExtension(zipFileName).Length == 0)
            {
                zipFileName = Path.ChangeExtension(zipFileName, ".zip");
            }

            try
            {
                using (ZipFile zf = ZipFile.Create(zipFileName))
                {
                    zf.BeginUpdate();

                    activeZipFile_ = zf;

                    foreach (string spec in fileSpecs)
                    {
                        // This can fail with wildcards in spec...
                        string path = Path.GetDirectoryName(Path.GetFullPath(spec));
                        string fileSpec = Path.GetFileName(spec);

                        zf.NameTransform = new ZipNameTransform(Settings.Global.Properties["POLPath"]);

                        FileSystemScanner scanner = new FileSystemScanner(fileSpec);
                        scanner.ProcessFile = new ProcessFileHandler(ProcessFile);
                        scanner.Scan(path, false);
                        ProgressBar.PerformStep();
                    }
                    zf.CommitUpdate();
                    ProgressBar.PerformStep();
                }
            }
            catch (Exception ex)
            {
                ExceptionBox.ExceptionForm tmp = new ExceptionBox.ExceptionForm(ref ex);
                tmp.ShowDialog();
            }
        }

        #endregion
        #region Adding
        /// <summary>
        /// Callback for adding a new file.
        /// </summary>
        /// <param name="sender">The scanner calling this delegate.</param>
        /// <param name="args">The event arguments.</param>
        void ProcessFile(object sender, ScanEventArgs args)
        {
            activeZipFile_.Add(args.Name);
        }
        #endregion
        #region Class Execute Command
        /// <summary>
        ///     Handle Filenames and execute Creation of the Zip unless none was
        ///     passed.
        /// </summary>		
        public void Execute(string newZipFilename, ref ToolStripProgressBar ProgressBar)
        {
            if (BuildFileList())
            {
                if (fileSpecs_.Count == 0)
                {
//                        Console.Out.WriteLine("Nothing to do");
                }
                else
                {
                    ProgressBar.Minimum = 0;
                    ProgressBar.Maximum = fileSpecs_.Count+1;
                    ProgressBar.Visible = true;
                    ProgressBar.Step = 1;
                    Create(newZipFilename, fileSpecs_, ref ProgressBar);
                    ProgressBar.Value = 0;
                    ProgressBar.Visible = false;

                }
            }
        }

        #endregion
        #region Support Routines
        byte[] GetBuffer()
        {
            if (buffer_ == null)
            {
                buffer_ = new byte[bufferSize_];
            }

            return buffer_;
        }
        #endregion
        #region Static support routines
        ///<summary>
        /// Calculate compression ratio as a percentage
        /// Doesnt allow for expansion (ratio > 100) as the resulting strings can get huge easily
        /// </summary>
        static int GetCompressionRatio(long packedSize, long unpackedSize)
        {
            int result = 0;
            if ((unpackedSize > 0) && (unpackedSize >= packedSize))
            {
                result = (int)Math.Round((1.0 - ((double)packedSize / (double)unpackedSize)) * 100.0);
            }
            return result;
        }

        /// <summary>
        /// Interpret attributes in conjunction with operatingSystem
        /// </summary>
        /// <param name="operatingSystem">The operating system.</param>
        /// <param name="attributes">The external attributes.</param>
        /// <returns>A string representation of the attributres passed.</returns>
        static string InterpretExternalAttributes(int operatingSystem, int attributes)
        {
            string result = string.Empty;
            if ((operatingSystem == 0) || (operatingSystem == 10))
            {
                if ((attributes & 0x10) != 0)
                    result = result + "D";
                else
                    result = result + "-";

                if ((attributes & 0x08) != 0)
                    result = result + "V";
                else
                    result = result + "-";

                if ((attributes & 0x01) != 0)
                    result = result + "r";
                else
                    result = result + "-";

                if ((attributes & 0x20) != 0)
                    result = result + "a";
                else
                    result = result + "-";

                if ((attributes & 0x04) != 0)
                    result = result + "s";
                else
                    result = result + "-";

                if ((attributes & 0x02) != 0)
                    result = result + "h";
                else
                    result = result + "-";

                // Device
                if ((attributes & 0x4) != 0)
                    result = result + "d";
                else
                    result = result + "-";

                // OS is NTFS
                if (operatingSystem == 10)
                {
                    // Encrypted
                    if ((attributes & 0x4000) != 0)
                    {
                        result += "E";
                    }
                    else
                    {
                        result += "-";
                    }

                    // Not content indexed
                    if ((attributes & 0x2000) != 0)
                    {
                        result += "n";
                    }
                    else
                    {
                        result += "-";
                    }

                    // Offline
                    if ((attributes & 0x1000) != 0)
                    {
                        result += "O";
                    }
                    else
                    {
                        result += "-";
                    }

                    // Compressed
                    if ((attributes & 0x0800) != 0)
                    {
                        result += "C";
                    }
                    else
                    {
                        result += "-";
                    }

                    // Reparse point
                    if ((attributes & 0x0400) != 0)
                    {
                        result += "R";
                    }
                    else
                    {
                        result += "-";
                    }

                    // Sparse
                    if ((attributes & 0x0200) != 0)
                    {
                        result += "S";
                    }
                    else
                    {
                        result += "-";
                    }

                    // Temporary
                    if ((attributes & 0x0100) != 0)
                    {
                        result += "T";
                    }
                    else
                    {
                        result += "-";
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Determine if string is numeric [0-9]+
        /// </summary>
        /// <param name="rhs">string to test</param>
        /// <returns>true iff rhs is numeric</returns>
        static bool IsNumeric(string rhs)
        {
            bool result;
            if (rhs != null && rhs.Length > 0)
            {
                result = true;
                for (int i = 0; i < rhs.Length; ++i)
                {
                    if (!char.IsDigit(rhs[i]))
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Make external attributes suitable for a <see cref="ZipEntry"/>
        /// </summary>
        /// <param name="info">The <see cref="FileInfo"/> to convert</param>
        /// <returns>Returns External Attributes for Zip use</returns>
        static int MakeExternalAttributes(FileInfo info)
        {
            return (int)info.Attributes;
        }

        /// <summary>
        /// Convert a wildcard expression to a regular expression
        /// </summary>
        /// <param name="wildcard">The wildcard expression to convert.</param>
        /// <returns>A regular expression representing the converted wildcard expression.</returns>
        static string WildcardToRegex(string wildcard)
        {
            int dotPos = wildcard.IndexOf('.');
            bool dotted = (dotPos >= 0) && (dotPos < wildcard.Length - 1);
            string converted = wildcard.Replace(".", @"\.");
            converted = converted.Replace("?", ".");
            converted = converted.Replace("*", ".*");
            converted = converted.Replace("(", @"\(");
            converted = converted.Replace(")", @"\)");
            if (dotted)
            {
                converted += "$";
            }

            return converted;
        }

        #endregion
        #region Instance Fields
        /// <summary>
        /// File specifications possibly with wildcards from command line
        /// </summary>
        ArrayList fileSpecs_ = new ArrayList();

        /// <summary>
        /// The currently active <see cref="ZipFile"/>.
        /// </summary>
        /// <remarks>Used for callbacks/delegates</remarks>
        ZipFile activeZipFile_;

        /// <summary>
        /// Buffer used during some operations
        /// </summary>
        byte[] buffer_;

        /// <summary>
        /// The size of buffer to provide. <see cref="GetBuffer"></see>
        /// </summary>
        int bufferSize_ = 4096;
        #endregion
    }
}
