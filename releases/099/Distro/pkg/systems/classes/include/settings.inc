/*
        Created by *Edwards
        
        2009-06-05
*/
use uo;
use os;
use cfgfile;

function Classes_GetSettingsCfgFile()

	var cfg := ReadConfigFile( ":classes:settings" );
	if( cfg.errortext )
		SysLog( "Error::Classes_GetSettingsCfgFile() - Unable to open [:classes:settings.cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function Classes_GetSettingsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := Classes_GetSettingsCfgFile();
	endif
	
	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::Classes_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
