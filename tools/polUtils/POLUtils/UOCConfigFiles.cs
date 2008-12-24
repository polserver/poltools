using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using POLUtils.UOConvert;
using POLUtils.UOConvert.UOCConfigFiles;

namespace POLUtils.UOConvert.UOCConfigFiles
{
    public class ConfigFiles
    {
        protected string _Filename = null;
        protected string _UOCCommand = null;

        public ConfigFiles() { }

        public string GetFilename()
        {
            return _Filename;
        }
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

    public class Multis : ConfigFiles
    {
        public Multis()
        {
            _Filename = "multis.cfg";
            _UOCCommand = "multis";
        }
    }

    public class Landtiles : ConfigFiles
    {
        public Landtiles()
        {
            _Filename = "landtiles.cfg";
            _UOCCommand = "landtiles";
        }
    }

    public class Tiledata : ConfigFiles
    {
        public Tiledata()
        {
            _Filename = "tiles.cfg";
            _UOCCommand = "tiles";
        }
    }
}
