/* $Id$
 *
 */

use uo;
use os;

include ":timedscripts:timedScripts";

/*
 * TargetEX(mobile, duration, flags)
 *
 * Purpose
 *
 * Parameters
 *
 * Return Value
 *
 */
function TargetEX(mobile, duration:=0, flags := TGTOPT_CHECK_LOS+TGTOPT_NEUTRAL)
	if ( duration > 0 )
		TS_StartTimer(mobile, "TargetControl", duration);
	endif

	return Target(mobile, flags);

	return 1;
endfunction

/*
 * TargetCoordinatesEX(mobile, duration, flags)
 *
 * Purpose
 *
 * Parameters
 *
 * Return Value
 *
 */
function TargetCoordinatesEX(mobile, duration:=0, flags := TGTOPT_CHECK_LOS)
	if ( duration > 0 )
		TS_StartTimer(mobile, "TargetControl", duration);
	endif

	var targ := TargetCoordinates(mobile);
	if ( flags & TGTOPT_CHECK_LOS )
		if ( !CheckLosAt(mobile, targ.x, targ.y, targ.z) )
			SendSysMessage(mobile, "That is not within your line of sight.");
			return error{"errortext":="Mobile's selection was not within LOS."};
		endif
	endif

	return targ;
endfunction