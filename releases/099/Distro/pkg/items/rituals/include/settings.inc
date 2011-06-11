/*
        Created by *Edwards

        2010-11-12
*/
use uo;
use os;
use cfgfile;

function RITUAL_GetSettingsCfgFile()

	var settings_path := ":rituals:rituals";
	var cfg := ReadConfigFile( settings_path );
	if( cfg.errortext )
		SysLog( "Error::RITUAL_GetSettingsCfgFile() - Unable to open ["+settings_path+".cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function RITUAL_GetSettingsCfgElem( elem_name, byref cfg_file := 0 )

	if( !cfg_file )
		cfg_file := RITUAL_GetSettingsCfgFile();
	endif
	
	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::RITUAL_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
