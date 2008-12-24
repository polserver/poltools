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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using POLUtils.UOConvert.UOCRealms;
using POLUtils.UOConvert.UOCConfigFiles;

namespace POLUtils.UOConvert
{
    /// <summary>
    ///     Root UOConvert Class.
    /// </summary>
    public class UOConvert
    {
        /// <summary>
        ///     Public Member for retrieving required MUL files for UOConvert.
        ///     This only contains the Map0 (Main britannia map) out of all the
        ///     map types, since only 1 map is REALLY required.
        /// </summary>
        public static string[] RequiredMul = {
                                          "multi.mul",
                                          "multi.idx",
                                          "map0.mul",
                                          "mapdif0.mul",
                                          "mapdifl0.mul",
                                          "staidx0.mul",
                                          "stadif0.mul",
                                          "statics0.mul",
                                          "tiledata.mul"};

        /// <summary>
        ///     Default Constructor
        /// </summary>
        public UOConvert() { }

    }
}