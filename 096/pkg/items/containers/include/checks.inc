//$Id: checks.inc 386 2005-10-03 00:22:39Z muaddiblsd $

/*===============================================================
* Current Version
* CHECKS.INC - v1.0
* Updated 9/28/2005 12:15PM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

use uo;
use os;

/*
 * CP_RootContainerLocked(object)
 *
 * Purpose
 * Determines if any containers the object is in are locked.
 *
 * Parameters
 * Object:	Object to check the containers of.
 *
 * Return value
 * Returns 1 if a locked container is found.
 *
 */
function CP_RootContainerLocked(object)
	var temp := object;
	while ( temp.container )
		if ( (temp.container).locked )
			return 1;
		else 
			// Move onto checking the next container.
			temp := temp.container;
		endif
		sleepms(2);
	endwhile
	
	return 0;
endfunction