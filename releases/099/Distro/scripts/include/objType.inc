/*
        Created by *Edwards

        Last Modifications 2008-12-12
             * Moved few functions from packages here

        Last Modifications 2009-08-17
             * Added IsStackable()
*/
use uo;

const STARTWHEEL                        := 0x1015;
const ENDWHEEL                          := 0x101e;

const UOBJ_LOOM_START                   := 0x105f;
const UOBJ_LOOM_END                     := 0x1066;

CONST UOBJ_GOLDCOIN                     := 0xEED;

CONST UOBJECT_CAMPFIRE                  := 0xDE3;

CONST UOBJECT_DOORS_START	        := 0x0675;
CONST UOBJECT_DOORS_END		        := 0x06f4;

function IsWater( item )

	var info := GetMapInfo( item.x, item.y );
	var tile := info.landtile;

	if( tile >= 0x04c && tile <= 0x06f )
		return 1;
	elseif( tile >= 0x0a8 && tile <= 0x0ab )
		return 1;
	elseif( tile == 0x0136 && tile == 0x0137 )
		return 1;
	elseif( tile >= 0x3ff0 && tile <= 0x3ff3 )
    		return 1;
    	endif

	if( item.item )
		if(( item.objtype >= 0x346e ) && ( item.objtype <= 0x3485 ))
			return 1;
		elseif(( item.objtype >= 0x3494 ) && ( item.objtype <= 0x34ab ))
			return 1;
		elseif(( item.objtype >= 0x34b8 ) && ( item.objtype <= 0x34ca ))
			return 1;
		elseif(( item.objtype >= 0x1796 ) && ( item.objtype <= 0x17b2 ))
			return 1;
		endif
	endif

	return 0;
endfunction
