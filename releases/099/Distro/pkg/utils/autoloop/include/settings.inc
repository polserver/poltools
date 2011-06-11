/*
        Created by *Edwards

        2011-04-01
*/
use uo;
use os;
use cfgfile;

function ALoop_GetSettingsCfgFile()

	var settings_path := ":autoloop:settings";
	var cfg := ReadConfigFile( settings_path );

	if( cfg.errortext )
		SysLog( "Error::ALoop_GetSettingsCfgFile() - Unable to open ["+settings_path+".cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function ALoop_GetSettingsCfgElem( elem_name, byref cfg_file := 0 )

	if( !cfg_file )
		cfg_file := ALoop_GetSettingsCfgFile();
	endif
	
	var elem := cfg_file[elem_name];

	if( elem.errortext )
		SysLog( "Error::ALoop_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
