//$Id: settings.inc 373 2006-06-17 18:27:33Z austinheilman $

/*===============================================================
* Current Version
* SETTINGS.INC - v1.0
* Updated 5/12/2006 2:29PM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

use uo;
use os;
use cfgfile;

function TIME_GetSettingsCfgFile()
	var cfg := ReadConfigFile(":timeutils:settings");

	if ( cfg.errortext )
		SysLog("Error::TIME_GetSettingsCfgFile() - Unable to open [:timeutils:settings.cfg] ->"+cfg.errortext);
	endif

	return cfg;
endfunction

function TIME_GetSettingsCfgElem(elem_name, byref cfg_file:=0)
	if ( !cfg_file )
		cfg_file := TIME_GetSettingsCfgFile();
	endif
	
	var elem := cfg_file[elem_name];

	if ( elem.errortext )
		SysLog("Error::TIME_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext);
	endif

	return elem;
endfunction
