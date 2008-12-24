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
using System.IO;
using System.Collections.Generic;
using System.Text;

using POLUtils.UOConvert;
using POLUtils.UOConvert.UOCConfigFiles;

namespace POLUtils.UOConvert.UOCConfigFiles
{

    /// <summary>
    ///     Inheritable Public and Private Entities for UOConvert Config File Information.
    ///     No default constructor as this Class is for Inheritable Purposes only.
    /// </summary>
    public class ConfigFiles
    {
        /// <summary>
        ///     Protected Config File Filename Storage
        /// </summary>
        protected string _Filename = null;
        /// <summary>
        ///     Protected Config File UOConvert Commandline Flag Storage
        /// </summary>
        protected string _UOCCommand = null;

        /// <summary>
        ///     Config File Method to retrieve the Filename Stored.
        /// </summary>
        public string GetFilename()
        {
            return _Filename;
        }
        /// <summary>
        ///     Config File Method to retrieve the UOConvert Commandline Flag Stored.
        /// </summary>
        public string GetUOCCommand()
        {
            return _UOCCommand;
        }
/*        public void BuildConfig(string UOConvertPath, string ConfigPath, bool UseUOPath)
        {
            string FileName = ConfigPath + "\\" + this.GetFilename();
            string OldFileName = UOConvertPath + "\\" + this.GetFilename();
            UOConvert.RunConverter(this.GetUOCCommand() + " <uopath>", UseUOPath);

            try
            {
                File.Delete(FileName);
            }
            catch (Exception)
            {
                throw new Exception("BuildConfig() could not delete " + FileName);
            }

            try
            {
                File.Move(OldFileName, FileName);
            }
            catch (Exception)
            {
                throw new Exception("BuildConfig() could not move " + OldFileName + " to " + FileName);
            }

        } */
    }

    /// <summary>
    ///     Multi Config File information for UOConvert. Inherits from ConfigFiles Class
    /// </summary>
    public class Multis : ConfigFiles
    {
        /// <summary>
        ///     Default Constructor to access Multis UOConvert Config File Information
        /// </summary>
        public Multis()
        {
            _Filename = "multis.cfg";
            _UOCCommand = "multis";
        }
    }

    /// <summary>
    ///     Landtiles Config File information for UOConvert. Inherits from ConfigFiles Class
    /// </summary>
    public class Landtiles : ConfigFiles
    {
        /// <summary>
        ///     Default Constructor to access Landtile UOConvert Config File Information
        /// </summary>
        public Landtiles()
        {
            _Filename = "landtiles.cfg";
            _UOCCommand = "landtiles";
        }
    }

    /// <summary>
    ///     Tiles Config File information for UOConvert. Inherits from ConfigFiles Class
    /// </summary>
    public class Tiledata : ConfigFiles
    {
        /// <summary>
        ///     Default Constructor to access Tiledata UOConvert Config File Information
        /// </summary>
        public Tiledata()
        {
            _Filename = "tiles.cfg";
            _UOCCommand = "tiles";
        }
    }
}
