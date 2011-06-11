/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-02-10
*/
use uo;
use os;
use cfgfile;

function SVR_GetSettingsCfgFile()

	var cfg := ReadConfigFile( ":worldsaver:settings" );

	if( cfg.errortext )
		SysLog( "Error::SVR_GetSettingsCfgFile() - Unable to open [:worldsaver:settings.cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function SVR_GetSettingsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := SVR_GetSettingsCfgFile();
	endif
	
	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::SVR_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
