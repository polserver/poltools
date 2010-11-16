//$Id: checks.inc 812 2006-09-13 20:59:46Z austinheilman $

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

/* CP_IsLocked(container)
 *
 * Purpose
 * Determines if the container or its root containers are locked.
 *
 * Parameters
 * container:	Container to check the locked status of.
 *
 * Return Value
 * Returns 0 if nothing is locked.
 * Returns 1 if the container or a root container is locked.
 *
 */
function CP_IsLocked(container)
	if ( container.locked )
		return 1;
	elseif ( CP_RootContainerLocked(container) )
		return 1;
	else
		return 0;
	endif
endfunction

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
		SleepMS(2);
	endwhile

	return 0;
endfunction