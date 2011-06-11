/*
        Maintened by *Edwards
        For FantasiaShard.com
        
        Edwards@FantasiaShard.com
        
        2009-10-20
*/
use uo;
use os;
use cfgfile;

function BBRD_GetSettingsCfgFile()

	var cfg := ReadConfigFile( ":bulletinBoard:settings" );
	if( cfg.errortext )
		SysLog( "Error::BBRD_GetSettingsCfgFile() - Unable to open [:bulletinboard:settings.cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function BBRD_GetSettingsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := BBRD_GetSettingsCfgFile();
	endif

	var elem := cfg_file[elem_name];

	if( elem.errortext )
		SysLog( "Error::BBRD_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
