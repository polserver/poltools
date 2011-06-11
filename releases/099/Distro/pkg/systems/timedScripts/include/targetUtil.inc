/*
        Maintened by *Edwards

        2009-02-11
*/
use uo;
use os;

include ":timedscripts:timedScripts";

function TargetEX( mobile, duration:=0, flags := TGTOPT_CHECK_LOS+TGTOPT_NEUTRAL )

	if( duration > 0 )
		if( CInt( TS_GetTimerDuration( mobile, "TargetControl" )) > 0 )
			return 0;
		endif

		TS_StartTimer( mobile, "TargetControl", duration );
	endif

	var targ := Target( mobile, flags );

	if( duration > 0 )
		// Remove the target timer so new targets can be set up.
		if( CInt( TS_GetTimerDuration( mobile, "TargetControl" )) > 0 )
			// If the timer is still running, stop it.
			TS_LowerDuration( mobile, "TargetControl", -1 );
		endif
	endif

	return targ;
endfunction

function TargetCoordinatesEX( mobile, duration:=0 )

	if( duration > 0 )
		if( CInt( TS_GetTimerDuration( mobile, "TargetControl" )) > 0 )
			return 0;
		endif

		TS_StartTimer( mobile, "TargetControl", duration );
	endif

	var targ := TargetCoordinates( mobile );

	if( duration > 0 )
		// Remove the target timer so new targets can be set up.
		if( CInt( TS_GetTimerDuration( mobile, "TargetControl" )) > 0 )
			// If the timer is still running, stop it.
			TS_LowerDuration( mobile, "TargetControl", -1 );
		endif
	endif

	return targ;
endfunction
