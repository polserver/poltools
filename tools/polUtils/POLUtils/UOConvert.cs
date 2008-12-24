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

using POLUtils.UOConvert.UOCRealms;
using POLUtils.UOConvert.UOCConfigFiles;

namespace POLUtils.UOConvert
{
    public class UOConvert
    {
        /// <summary>
        ///     Dictionary to contain all the UOConvert Generated \Config\*.cfg storage.
        /// </summary>
        public ConfigFiles ConfigFiles = new ConfigFiles();

        /// <summary>
        ///     Dictionary to contain all the Realms storage.
        /// </summary>
        public Realms Realms = new Realms();

        /// <summary>
        ///     Default Constructor
        /// </summary>
        public UOConvert() { }

    }
}