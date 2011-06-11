/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-21
*/
use uo;
use util;
use cfgfile;

include "include/util_extend";

CONST MIN_COLOR := 2;
CONST MAX_COLOR := 1001;

function CanDye( item )

	var config := ReadConfigFile( ":*:itemdesc" );
	config := config[item.objtype];

        if( GetObjProperty( item, "Undyeable" ))
                return 0;
        elseif( config.CanDye )
		return 1;
	elseif( config.dyeable )
		return 1;
	endif
 
	return 0;
endfunction

function ValidDyeColor( mobile, color )

	if( mobile.cmdlevel >= 2 )
		return 1;
	elseif(( color >= MIN_COLOR ) && ( color <= MAX_COLOR ))
		return 1;
	else
		return 0;
	endif
endfunction

function RandomDyeColor()

	return RandomIntRange( MIN_COLOR, MAX_COLOR );
endfunction
