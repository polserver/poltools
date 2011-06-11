/*
        Maintened by *Edwards

        2009-02-09
*/
use uo;
use os;
use cfgfile;

function AP_GetSettingsCfgFile()

	var cfg := ReadConfigFile( ":attributes:settings" );
	if( cfg.errortext )
		SysLog( "Error::AP_GetSettingsCfgFile() - Unable to open [:attributes:settings.cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function AP_GetSettingsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := AP_GetSettingsCfgFile();
	endif

	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::AP_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
