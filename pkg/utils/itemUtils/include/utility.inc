/* $Id: utility.inc 912 2005-11-05 23:23:40Z austin $
 *
 */
use basic;

include ":itemutils:myUtil";


function find_or_create_storage( areaname )
	var area;
	area := FindStorageArea( areaname );
	if(!area)
		area := CreateStorageArea( areaname );
	endif
	return area;
endfunction


function find_or_create_item(storage, name, objtype := 0x966a)
	var item := FindRootItemInStorageArea(storage, name);
	if(item)
		return item;
	endif
	item := CreateRootItemInStorageArea( storage, name, objtype);
	return item;
endfunction


function IsLocationAccessible(character, x, y, z)
	var xd, yd, zd;
	xd := character.x - x;
	if(xd < -1 || xd > 1)
		return 0;
	endif
	yd := character.y - y;
	if(yd < -1 || yd > 1)
		return 0;
	endif
	zd := character.z - z;
	if(zd < -10 || zd > 10)
		return 0;
	endif
	return CheckLosAt( character, x, y, z );
endfunction


function min(x, y)
	if(x<= y)
		return x;
	else
		return y;
	endif
endfunction