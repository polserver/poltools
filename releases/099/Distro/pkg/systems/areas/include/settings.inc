/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-12-21
*/
use uo;
use os;
use util;
use cfgfile;

function AM_GetSettingsCfgFile()

	var cfg := ReadConfigFile( ":areas:areas" );
	if( cfg.errortext )
		SysLog( "Error::AM_GetSettingsCfgFile() - Unable to open [:areas:areas.cfg] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function AM_GetSettingsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := AM_GetSettingsCfgFile();
	endif

	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::AM_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
	endif

	return elem;
endfunction

function AM_GetMerchantValue( area )

	var area_elem := AM_GetSettingsCfgElem( area );

	return CDbl( area_elem.MerchantValue );
endfunction
