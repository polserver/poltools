/*
        Maintened by *Edwards
        
        2009-06-04
*/
use uo;
use os;
use cfgfile;

function ACCT_GetSettingsCfgFile()

	var cfg := ReadConfigFile( ":accounts:settings" );
	if( cfg.errortext )
		SysLog( "Error::ACCT_GetSettingsCfgFile() - Unable to open [:accounts:settings.cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function ACCT_GetSettingsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := ACCT_GetSettingsCfgFile();
	endif
	
	var elem := cfg_file[elem_name];

	if( elem.errortext )
		SysLog( "Error::ACCT_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
