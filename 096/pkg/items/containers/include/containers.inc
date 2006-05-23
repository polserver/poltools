/* 
 * $Id: containers.inc 891 2005-11-04 06:29:43Z muaddiblsd $
 *
 */

use uo;
use os;

/*
 * AmountInContainer(container, obj_type, params)
 *
 * Purpose
 * Finds the amount of items with a certain objtype and 
 * optional additional properties that are in a container.
 *
 * Parameters
 * container:	Container to search
 * obj_type:	Objtype to find
 * properties:	Optional - dictionary ( dict[property] = value )
 *		Example: dict{"color"->0} // only count if uncolored
 *
 * Return value
 * Returns an integer
 *
 */
function AmountInContainer(container, obj_type, properties:=0)
	var amount := 0;
	foreach item in EnumerateItemsInContainer(container)
		if( item.objtype == obj_type )
			if ( !properties )
				amount := amount + item.amount;
			else
				foreach property in ( properties.keys() )
					if ( item.Get_Member(property) == properties[property] )
						amount := amount + item.amount;
					endif
					sleepms(2);
				endforeach
			endif
		endif
		sleepms(2);
	endforeach
	
	return amount;
endfunction

/*
 * FindMatchesInContainer(container, params)
 *
 * Purpose
 * Builds a list of items with properties matching 'params'
 *
 * Parameters
 * container:	Container to search
 * properties:	Optional - dictionary ( dict[property] = value )
 *		Examples: dict{"color"->0} // only count if uncolored
 *			  dict{"objtype"->0x123A, "color"->45}
 *			  dict{"container"->itemref}
 *
 * Return value
 * Returns an array of item refs.
 *
 */
function FindMatchesInContainer(container, properties:=0)
	var item_list := array{};
	
	MAIN: foreach item in ( EnumerateItemsInContainer(container) )
		foreach property in ( properties.Keys() )
			if ( item.Get_Member(property) != properties[property] )
				continue MAIN;
			endif
			
			sleepms(2);
		endforeach
		
		item_list.Append(item);
		
		sleepms(2);
	endforeach
	
	return item_list;
endfunction

/*
 * ListRootItemsInContainer(container)
 *
 * Purpose
 * Returns an array of all of the top level items in a container.
 *
 * Parameters
 * container:	Container to search
 *
 * Return value
 * Returns an array of item refs.
 *
 */
function ListRootItemsInContainer(container)
	var item_list := array{};
	
        foreach item in ( EnumerateItemsInContainer(container) )
		if ( item.container == container )
			item_list.Append(item);
		endif
		sleepms(2);
	endforeach

	return item_list;
endfunction

