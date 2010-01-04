use cfgfile;

//returns all the items in container of objtype
function EnumerateItemsInContainerOfObjtype(container, objtype)

	var ret := { };
        foreach item in EnumerateItemsInContainer(container)
		if ( item.objtype == objtype )
			ret.append(item);
		endif
	endforeach

	return ret;

endfunction

//returns all the 'top-level' items in container
function ListRootItemsInContainer(container)

	var ret := { };
        foreach item in EnumerateItemsInContainer(container)
		if ( item.container == container )
			ret.append(item);
		endif
	endforeach

	return ret;

endfunction

//returns all the 'top-level' items in container of objtype
function ListRootItemsInContainerOfObjtype(container, objtype)

	var ret := { };
        foreach item in EnumerateItemsInContainer(container)
		if ( (item.container == container) && (item.objtype == objtype) )
			ret.append(item);
		endif
	endforeach

	return ret;

endfunction

function InInventory(inventory, it)

	foreach item in EnumerateItemsInContainer(inventory)
		if (item == it)
			return 1;
		endif
	endforeach

	return 0;

endfunction

function IsContainer( it )

	if (it.objtype == 0x7057)
		return 0;//trashcans don't count as containers
	endif

	var waslocked := it.locked;
	it.locked := 1;
	if (it.locked)
		it.locked := waslocked;
		return 1;
	else
		return 0;
	endif

endfunction

function IsDungeon( x, y )

	if ( (x >= 5120) && (x <= 6143) )
		if ( (y >= 0) && (y <= 2303) )
			return 1;
		endif
	endif

	return 0;

endfunction

function NoTeleportZone( x, y )

	if ( (x >= 5120) && (x <= 6143) )
		if ( (y >= 2080) && (y <= 2303) )
			return 1;
		endif
	endif

	return 0;

endfunction