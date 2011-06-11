/*
        Maintened by *Edwards

        2009-10-10
*/

function EnterSleepMode()

	if( !sleep_mode )
		if( g_scripts["Sleep"] )
			StartNerve( npc, "Sleep", g_scripts["Sleep"].script, array{g_settings, g_scripts}, g_scripts["Sleep"].flags );
		endif
	
		sleep_mode := 1;
	endif
	
	return 1;
endfunction

function LeaveSleepMode()

	idle_ticks := 0;
	sleep_mode := 0;

	StopNerve( npc, "Sleep" );

	if( g_scripts.Exists( "LookAround" ))
		StartNerve( npc, "LookAround", g_scripts["LookAround"].script, array{g_settings, g_scripts}, g_scripts["LookAround"].flags );
	endif

	return 1;
endfunction

function ShouldSleep()

	if( sleep_mode )
		// Already asleep.
		return 0;
	elseif( Events_Waiting() )
		// Events are waiting to process.
		return 0;
	elseif( g_settings["NoSleep"] )
		// NPC is not allowed to enter sleep mode.
		return 0;
	elseif( GetNerveProcess( npc, "LookAround" ))
		// Look around script is currently running.
		return 0;
	elseif( idle_ticks < g_settings["IdleTicks"] )
		// Not enough idle ticks have gone by yet.
		return 0;
        elseif( GetObjProperty( npc, "MustSleep" ))
		return 1;
	endif
	
	return 1;
endfunction
