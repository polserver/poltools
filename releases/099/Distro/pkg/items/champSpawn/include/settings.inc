/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-12-30
*/
use uo;
use os;
use cfgfile;

function CS_GetSettingsCfgFile()

	var cfg := ReadConfigFile( ":champSpawn:spawns" );
	if( cfg.errortext )
		SysLog( "Error::CS_GetSettingsCfgFile() - Unable to open [:champSpawn:spawns] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function CS_GetSettingsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := CS_GetSettingsCfgFile();
	endif

	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::CS_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
