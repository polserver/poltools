use uo;
use os;
use cfgfile;

/*
 * SS_GetSettingsCfgFile(engine_name)
 *
 * Purpose
 * Reads in an engine's settings.cfg or if no engine name is passed, the main settings.cfg.
 *
 * Parameters
 * engine_name:	A string containing the name of an engine.
 *
 * Return value
 * A config file reference.
 *
 */
function SS_GetSettingsCfgFile(engine_name:="")
	var cfg_path := ":spawnsystem:config/settings";
	if ( engine_name )
		cfg_path := ":spawnsystem:engines/"+engine_name+"/settings";
	endif

	var cfg := ReadConfigFile(":spawnsystem:config/settings");

	if ( cfg.errortext )
		SysLog("Error::SS_GetSettingsCfgFile() - Unable to open "+cfg_path+".cfg ->"+cfg.errortext);
	endif

	return cfg;
endfunction

/*
 * SS_GetSettingsCfgElem(cfg_file, elem_name)
 *
 * Purpose
 * Retrieves an elem from a config file. 
 *
 * Parameters
 * cfg_file:	A config file reference.
 * elem_name:	A string matching the elem name to be retrieved.
 *
 * Return value
 * A config file elem reference.
 *
 */
function SS_GetSettingsCfgElem(cfg_file, elem_name)
	var cfg := SS_GetSettingsCfgFile();
	var elem := cfg[elem_name];

	if ( elem.errortext )
		SysLog("Error::SS_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] in "+cfg_file.name+" ->"+elem.errortext);
	endif

	return elem;
endfunction

/*
 * SS_LoadSettings(cfg_elem)
 *
 * Purpose
 * Parses settings in an engine's or the main spawner's settings.cfg
 *
 * Parameters
 * cfg_elem:	An elem containing the "Settings" lines to be used.
 *
 * Return value
 * A dictionary containing mixed variable types.
 *
 */
function SS_LoadSettings(cfg_elem)
	var settings := dictionary;

	foreach line in GetConfigStringArray(cfg_elem, "Setting")
		line := SplitWords(line);
		var key := line[1];
		var value := UnPack(line[2]);

		settings[key] := value;
		sleepms(2);
	endforeach

	return settings;
endfunction
