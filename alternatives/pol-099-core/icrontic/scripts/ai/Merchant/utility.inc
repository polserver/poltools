use basic;

//function GetArgument( text , argNbr )
//    local argArray := SplitWords( text );
//    if ( argNbr >= 1 AND argNbr <= len( argArray ) )
//        return argArray[ argNbr ];
//    else
//        return "";
//    endif
//
//endfunction

function find_or_create_storage( areaname )
	var area;
	area := FindStorageArea( areaname );
	
	if (!area)
		area := CreateStorageArea( areaname );
        if (area)
            print ( "Storage Area '" + areaname + "' created." );
        endif
	endif

	return area;
endfunction

function find_or_create_item( storage, name, objtype )
	var item;
	item := FindRootItemInStorageArea( storage, name );
	
	if (item)
		return item;
    endif

	// didn't find, so create
	item := CreateRootItemInStorageArea( storage, name, objtype );
	
	return item;
endfunction


function IsLocationAccessible( character, x, y, z )
    var xd, yd, zd;
    
    xd := character.x - x;
    if (xd < -1 or xd > 1) 
        return 0;
    endif
    
    yd := character.y - y;
    if (yd < -1 or yd > 1) 
        return 0;
    endif
    
    zd := character.z - z;
    if (zd < -10 or zd > 10)
        return 0;
    endif

    return CheckLosAt( character, x, y, z );
endfunction

function min(x, y)
    if (x<= y)
        return x;
    else
        return y;
    endif
endfunction
