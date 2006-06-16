//$Id: settings.inc 87 2006-05-23 19:04:43Z sroyalty $

/*===============================================================
* Current Version
* SETTINGS.INC - v1.0
* Updated 5/13/2006 8:39AM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

use uo;
use os;
use cfgfile;

/*
 * CP_GetSettingsCfgFile(engine_name)
 *
 * Purpose
 * Reads in :containers:configs/settings.cfg
 *
 * Parameters
 *
 * Return value
 * A config file reference.
 *
 */
function CP_GetSettingsCfgFile()
	var cfg := ReadConfigFile(":containers:config/settings");

	if ( cfg.errortext )
		SysLog("Error::CP_GetSettingsCfgFile() - Unable to open [:containers:config/settings.cfg] ->"+cfg.errortext);
	endif

	return cfg;
endfunction

/*
 * CP_GetSettingsCfgElem(elem_name, cfg_file)
 *
 * Purpose
 * Retrieves an elem from a config file. 
 *
 * Parameters
 * elem_name:	A string matching the elem name to be retrieved.
 * cfg_file:	Optional parameter - reference to a config already read in by CP_GetSettingsCfgFile()
 *
 * Return value
 * A config file elem reference.
 *
 */
function CP_GetSettingsCfgElem(elem_name, byref cfg_file:=0)
	if ( !cfg_file )
		cfg_file := CP_GetSettingsCfgFile();
	endif
	
	var elem := cfg_file[elem_name];

	if ( elem.errortext )
		SysLog("Error::CP_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext);
	endif

	return elem;
endfunction
