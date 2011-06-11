/*
        Maintened by *Edwards

        2009-10-25
*/
use uo;
use os;

function DP_GetSettingsCfgFile()

	var cfg := ReadConfigFile( ":death:settings" );
	if( cfg.errortext )
		SysLog( "Error::DP_GetSettingsCfgFile() - Unable to open [:death:settings.cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function DP_GetSettingsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := DP_GetSettingsCfgFile();
	endif

	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::DP_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
