/* $Id$
 *
 * Current Version
 * SETTINGS.INC - v1.0
 * Updated 5/10/2006 8:29PM
 *
 * -- Revision v1.0 --
 * Austin:
 *  Created include file
 ===============================================================*/

use uo;
use os;
use cfgfile;

/*
 * BBRD_GetSettingsCfgFile(engine_name)
 *
 * Purpose
 * Reads in :bulletinboard:settings.cfg
 *
 * Parameters
 *
 * Return value
 * A config file reference.
 *
 */
function BBRD_GetSettingsCfgFile()
	var cfg := ReadConfigFile(":bulletinboard:settings");

	if ( cfg.errortext )
		SysLog("Error::BBRD_GetSettingsCfgFile() - Unable to open [:bulletinboard:settings.cfg] ->"+cfg.errortext);
	endif

	return cfg;
endfunction

/*
 * BBRD_GetSettingsCfgElem(elem_name, cfg_file)
 *
 * Purpose
 * Retrieves an elem from a config file.
 *
 * Parameters
 * elem_name:	A string matching the elem name to be retrieved.
 * cfg_file:	Optional parameter - reference to a config already read in by BBRD_GetSettingsCfgFile()
 *
 * Return value
 * A config file elem reference.
 *
 */
function BBRD_GetSettingsCfgElem(elem_name, byref cfg_file:=0)
	if ( !cfg_file )
		cfg_file := BBRD_GetSettingsCfgFile();
	endif

	var elem := cfg_file[elem_name];

	if ( elem.errortext )
		SysLog("Error::BBRD_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext);
	endif

	return elem;
endfunction

