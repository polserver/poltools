use uo;
use os;
use cfgfile;

function SPELL_GetSettingsCfgFile()

	var settings_path := ":spells:settings";
	var cfg := ReadConfigFile( settings_path );

	if( cfg.errortext )
		SysLog( "Error::SPELL_GetSettingsCfgFile() - Unable to open ["+settings_path+".cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function SPELL_GetSettingsCfgElem( elem_name, byref cfg_file := 0 )

	if( !cfg_file )
		cfg_file := SPELL_GetSettingsCfgFile();
	endif
	
	var elem := cfg_file[elem_name];

	if( elem.errortext )
		SysLog( "Error::SPELL_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
