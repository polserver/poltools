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
 * params:	Optional - dictionary ( dict[property] = value )
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

