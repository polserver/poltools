/*
        Created by *Edwards

        2008-04-25

        Thanks to ZuluHotel for this original idea.
*/
use uo;
use os;
use cfgfile;

/*
 * ELE_GetSettingsCfgFile(engine_name)
 *
 * Purpose
 * Reads in :elementals:configs/settings.cfg
 *
 * Parameters
 *
 * Return value
 * A config file reference.
 *
 */
function ELE_GetSettingsCfgFile()
	var cfg := ReadConfigFile(":elementals:settings");

	if ( cfg.errortext )
		SysLog("Error::ELE_GetSettingsCfgFile() - Unable to open [:elementals:settings.cfg] ->"+cfg.errortext);
	endif

	return cfg;
endfunction

/*
 * ELE_GetSettingsCfgElem(elem_name, cfg_file)
 *
 * Purpose
 * Retrieves an elem from a config file.
 *
 * Parameters
 * elem_name:	A string matching the elem name to be retrieved.
 * cfg_file:	Optional parameter - reference to a config already read in by ELE_GetSettingsCfgFile()
 *
 * Return value
 * A config file elem reference.
 *
 */
function ELE_GetSettingsCfgElem(elem_name, byref cfg_file:=0)
	if ( !cfg_file )
		cfg_file := ELE_GetSettingsCfgFile();
	endif

	var elem := cfg_file[elem_name];

	if ( elem.errortext )
		SysLog("Error::ELE_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext);
	endif

	return elem;
endfunction
