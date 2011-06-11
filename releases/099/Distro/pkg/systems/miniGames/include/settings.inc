/*
        Created by *Edwards

        2009-10-07
*/
use uo;
use os;

function MG_GetSettingsCfgFile()

	var cfg := ReadConfigFile( ":miniGames:settings" );
	if( cfg.errortext )
		SysLog( "Error::MG_GetSettingsCfgFile() - Unable to open [:miniGames:settings.cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function MG_GetSettingsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := MG_GetSettingsCfgFile();
	endif

	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::MG_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
