//$Id: settings.inc 87 2006-05-23 19:04:43Z sroyalty $

/*===============================================================
* Current Version
* SETTINGS.INC - v1.0
* Updated 5/21/2006 10:55AM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

use uo;
use os;
use cfgfile;

/*
 * Guild_GetSettingsCfgFile(engine_name)
 *
 * Purpose
 * Reads in :guilds:configs/settings.cfg
 *
 * Parameters
 *
 * Return value
 * A config file reference.
 *
 */
function Guild_GetSettingsCfgFile()
	var cfg := ReadConfigFile(":guilds:settings");

	if ( cfg.errortext )
		SysLog("Error::Guild_GetSettingsCfgFile() - Unable to open [:guilds:settings.cfg] ->"+cfg.errortext);
	endif

	return cfg;
endfunction

/*
 * Guild_GetSettingsCfgElem(elem_name, cfg_file)
 *
 * Purpose
 * Retrieves an elem from a config file. 
 *
 * Parameters
 * elem_name:	A string matching the elem name to be retrieved.
 * cfg_file:	Optional parameter - reference to a config already read in by Guild_GetSettingsCfgFile()
 *
 * Return value
 * A config file elem reference.
 *
 */
function Guild_GetSettingsCfgElem(elem_name, byref cfg_file:=0)
	if ( !cfg_file )
		cfg_file := Guild_GetSettingsCfgFile();
	endif
	
	var elem := cfg_file[elem_name];

	if ( elem.errortext )
		SysLog("Error::Guild_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext);
	endif

	return elem;
endfunction
