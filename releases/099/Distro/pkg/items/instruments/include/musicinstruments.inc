
use uo;

CONST	SUCCESS_SOUND 	:=	0X1;
CONST	FAIL_SOUND 	:=	0X2;


/*
 * MIP_FindInBackPack(mobile)
 * 
 * Purpose
 * Find an instrument inside the mobile.
 *
 * Parameters
 * mobile: a mobile reference
 *
 * Return value
 * Returns the reference of an instrument in the mobile
 * Returns 0 if there is no intrument in the mobile
 *
 */

function MIP_FindInBackPack(mobile)
	return Instruments_FindInContainer( mobile.backpack );
endfunction

/*
 * MIP_FindInContainer(container)
 * 
 * Purpose
 * Find an instrument inside the container.
 *
 * Parameters
 * container: a Container reference
 *
 * Return value
 * Returns the reference of an instrument in the container
 * Returns 0 if there is no intrument in the container
 *
 */

function MIP_FindInContainer(container)
	foreach item in EnumerateItemsInContainer(container)
		if( item.IsInstrument() )
			return item;		
		endif
	endforeach
	return 0;
endfunction

/*
 * MIP_FindAllInContainer(container)
 * 
 * Purpose
 * Find all the instruments inside a container.
 *
 * Parameters
 * container: a Container reference
 *
 * Return value
 * Returns the reference of an array of all the instruments in the container
 *
 */

function MIP_FindAllInContainer(container)
	var instruments := array;
	foreach item in EnumerateItemsInContainer(container)
		if( item.IsInstrument() )
			instruments.append(item);		
		endif
	endforeach
	return instruments;
endfunction


