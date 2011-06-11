/*
        Modified by *Edwards

        2008-04-07

        Thanks to ZuluHotel for this idea
*/

use uo;
use os;
use cfgfile;

function GetAreasCfgFile()

	var cfg_file := ReadConfigFile( ":areas:areas" );
	if( cfg_file.errortext )
		SysLog( "Error::GetAreasCfgFile() - Unable to open areas.cfg ->"+cfg_file.errortext );
	endif

	return cfg_file;
endfunction

function GetAreasCfgElem( area_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := GetAreasCfgFile();
	endif

	var cfg_elem := cfg_file[area_name];
	if( cfg_elem.errortext )
		SysLog( "Error::GetAreasCfgElem() - unable to find cfg elem ["+area_name+"] ->"+cfg_elem.errortext );
	endif

	return cfg_elem;
endfunction

function ListAreasByType()

	var cfg_elem := GetAreasCfgElem( "Settings" );
        var cfg_areas := GetConfigStringArray( cfg_elem, "AreaName" );
        
        if( cfg_areas.errortext )
		SysLog( "Error::ListAreasByType() -->"+cfg_areas.errortext );
	endif
 
	return cfg_areas;
endfunction

function GetAreaSettings( area_name )

        var cfg_area := GetAreasCfgElem( area_name );
        if( cfg_area.errortext )
		SysLog( "Error::GetAreaSettings() -->"+cfg_area.errortext );
	endif

	return cfg_area;
endfunction

function A_IsInBox( byref object, byref x1, byref x2, byref y1, byref y2, byref realm )

        if( object.realm == realm )
	        if( object.x >= x1 && object.x <= x2 && object.y >= y1 && object.y <= y2 )
			return 1;
		endif
	endif

	return 0;
endfunction
