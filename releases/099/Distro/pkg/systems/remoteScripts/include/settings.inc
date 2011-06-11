/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2011-13-04
*/
use uo;
use os;
use cfgfile;

function RS_GetSettingsCfgFile()
 
	var cfg := ReadConfigFile( ":remoteScripts:settings" );
	if( cfg.errortext )
		ReportText( "Error::RS_GetSettingsCfgFile() - Unable to open [:remoteScripts:settings] -->"+cfg.errortext, REPORT_SYSLOG );
	endif

	return cfg;
endfunction

function RS_GetSettingsCfgElem( elem_name, byref cfg_file := 0 )

	if( !cfg_file )
		cfg_file := RS_GetSettingsCfgFile();
	endif

	var elem := cfg_file[elem_name];
	if( elem.errortext )
		ReportText( "Error::RS_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] -->"+elem.errortext, REPORT_SYSLOG );
	endif

	return elem;
endfunction

function RS_GetScripts()

	var cfg_file := RS_GetSettingsCfgFile();
 
        var get_scripts := GetConfigStringKeys( cfg_file );
	if( get_scripts.errortext )
		ReportText( "Error::RS_GetScripts() - Unable to read elems -->"+get_scripts.errortext, REPORT_SYSLOG );
	endif
 
        return get_scripts;
endfunction
