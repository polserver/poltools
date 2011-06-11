/*
        Maintened by *Edwards

        2009-02-10
*/
use uo;
use os;
use cfgfile;

function SS_GetSettingsCfgFile( engine_name:="" )

	var cfg_path := ":spawnsystem:settings";
	if( engine_name )
		cfg_path := ":spawnsystem:engines/"+engine_name+"/settings";
	endif

	var cfg := ReadConfigFile( ":spawnsystem:settings" );
	if( cfg.errortext )
		SysLog( "Error::SS_GetSettingsCfgFile() - Unable to open "+cfg_path+".cfg ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function SS_GetSettingsCfgElem( cfg_file, elem_name )

	var cfg := SS_GetSettingsCfgFile();
	var elem := cfg[elem_name];
	if( elem.errortext )
		SysLog( "Error::SS_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] in "+cfg_file.name+" ->"+elem.errortext );
	endif

	return elem;
endfunction
