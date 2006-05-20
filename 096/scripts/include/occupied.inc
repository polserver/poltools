/* $Id$
 *
 * Purpose:
 * Used to determine is a player is 'busy'
 * E.G. - Meditating, casting a spell or something.
 *
 */

use uo;

/* 
 * IsOccupied(mobile, doing:=0)
 *
 * Purpose 
 * Determines if a mobile is already doing something.
 *
 * Parameters
 * mobile:	Mobile to check on
 * doing:	Activity to check on.
 *		If nothing is passed, uses mobile.attached
 *
 */
function IsOccupied(mobile, doing:=0)
	if ( !doing )
		return mobile.attached;
	endif
	
	var activity_list := GetObjProperty(mobile, "#Occupied");
	var timer := activity_list[doing];
	
	if ( timer == -1 ) // Timer doesn't expire.
		return 1;
	elseif ( timer < polcore().systime )
		return 1;
	endif
	
	return 0;
endfunction

/*
 * SomeFunctionToAddADoingTimer()
 *
 */