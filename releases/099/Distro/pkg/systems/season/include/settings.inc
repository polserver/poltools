/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2010-01-17
*/
use uo;
use os;
use cfgfile;

function Season_GetSettingsCfgFile()

	var cfg := ReadConfigFile( ":season:settings" );
	if( cfg.errortext )
		SysLog( "Error::Season_GetSettingsCfgFile() - Unable to open [:season:settings.cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function Season_GetSettingsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := Season_GetSettingsCfgFile();
	endif

	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::Season_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
