/*
        Created by *Edwards

        2010-05-29
*/
use uo;
use os;
use cfgfile;

function OC_GetSettingsCfgFile()

	var cfg := ReadConfigFile( ":onlineCount:settings" );

	if( cfg.errortext )
		SysLog( "Error::OC_GetSettingsCfgFile() - Unable to open [:onlineCount:settings.cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function OC_GetSettingsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := OC_GetSettingsCfgFile();
	endif
	
	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::OC_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction

function OC_GetnameList()

	var cfg_elem := OC_GetSettingsCfgElem( "Bots" ),
            names_array := GetConfigStringArray( cfg_elem, "Name" );;

	return names_array;
endfunction
