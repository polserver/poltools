/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-22
*/
use uo;
use os;

function RACE_GetSettingsCfgFile()

	var cfg := ReadConfigFile( ":races:settings" );
	if( cfg.errortext )
		SysLog( "Error::RACE_GetSettingsCfgFile() - Unable to open [:races:settings.cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function RACE_GetSettingsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := RACE_GetSettingsCfgFile();
	endif

	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::RACE_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
