/*===============================================================
* Current Version
* SLEEP.INC - v1.0
* Updated 9/11/2005 2:06AM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

/*
 * EnterSleepMode()
 *
 * Purpose
 * Starts the sleep script if the npc has one.
 *
 * Parameters
 *
 * Return value
 * Always returns 1
 *
 */
function EnterSleepMode()
	if ( !sleep_mode )
		if ( g_scripts["Sleep"] )
			var process := GetNerveProcess(npc, "Sleep");
			if ( !process )
				StartNerve(npc, "Sleep", g_scripts["Sleep"].script, array{g_settings, g_scripts}, g_scripts["Sleep"].flags);
			endif
		else
			foreach nerve in (LoadNerves(npc))
				if ( nerve.flags & NF_CONTINUE_ON_SLEEP )
					continue;
				else
					StopNerve(npc, _nerve_iter);
				endif
				SleepMS(2);
			endforeach
		endif
	
		//The brain is now in sleep mode, isn't it cute?
		sleep_mode := 1;
	endif
	
	return 1;
endfunction

/*
 * LeaveSleepMode()
 *
 * Purpose
 * Brings the npc out of sleep mode.
 * Also used to reset idle_ticks.
 *
 * Parameters
 *
 * Return value
 * Always returns 1
 *
 */
function LeaveSleepMode()
	if ( sleep_mode )
		StopNerve(npc, "Sleep");
	endif
	
	if ( g_scripts.Exists("LookAround") )
		if ( !GetNerveProcess(npc, "LookAround") )
			StartNerve(npc, "LookAround", g_scripts["LookAround"].script, array{g_settings, g_scripts}, g_scripts["LookAround"].flags);
		endif
	endif
	
	idle_ticks := 0;
	sleep_mode := 0;
endfunction

/*
 * ShouldSleep()
 *
 * Purpose
 * Determines if it is time to sleep
 *
 * Parameters
 *
 * Return Value
 * Returns 0 or 1
 *
 */
function ShouldSleep()
	if ( sleep_mode )
		// Already asleep.
		return 0;
	elseif ( Events_Waiting() )
		// Events are waiting to process.
		return 0;
	elseif ( GetNerveProcess(npc, "LookAround") )
		// Look around script is currently running.
		return 0;
	elseif ( g_settings["NoSleep"] )
		// NPC is not allowed to enter sleep mode.
		return 0;
	elseif ( idle_ticks < g_settings["IdleTicks"] )
		// Not enough idle ticks have gone by yet.
		return 0;
	endif
	
	return 1;
endfunction
