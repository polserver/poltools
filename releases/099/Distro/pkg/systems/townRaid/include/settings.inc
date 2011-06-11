/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2010-01-03
*/
use uo;
use os;
use cfgfile;

function TRaid_GetCfgElem( elem_name, cfg )

        var cfg_file := ReadConfigFile( cfg );
	if( cfg_file.errortext )
		SysLog( "Error::TRaid_GetCfgElem() - Unable to read config file ["+cfg_file+"] ->"+cfg_file.errortext );
	endif

	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::TRaid_GetCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction
