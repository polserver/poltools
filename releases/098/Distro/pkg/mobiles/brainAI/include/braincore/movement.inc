/*===============================================================
* Current Version
* MOVEMENT.INC - v1.0
* Updated 9/1/2005 6:42AM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

/*
 * Movement(ev)
 *
 * Purpose
 * Handles NPC movenpcnt to/from an object for the brain.
 *
 * Parameters
 * ev:	Strucuture containing event information.
 *
 * Return value
 * Always returns 1
 *
 */
function Movement(ev)
	var old_pri := Set_Priority(50);

	case ( ev.speed )
		NEMOVE_WALK:
			case ( ev.direction )
				NEMOVE_AWAY:
					repeat
						WalkAwayFrom(ev.source);
						SleepMS(3);
					until ( !MoveLoopCheck(ev) );
				NEMOVE_TOWARD:
					repeat
						WalkToward(ev.source);
						SleepMS(3);
					until ( !MoveLoopCheck(ev) );
			endcase
		NEMOVE_RUN:
			case( ev.direction )
				NEMOVE_AWAY:
					repeat
						RunAWayFrom(ev.source);
						SleepMS(3);
					until ( !MoveLoopCheck(ev) );
				NEMOVE_TOWARD:
					repeat
						RunToward(ev.source);
						SleepMS(3);
					until ( !MoveLoopCheck(ev) );
			endcase
	endcase

	Set_Priority(old_pri);

	return 1;
endfunction

/*
 * MovementXY(ev)
 *
 * Purpose
 * Handles NPC movement to/from a coordinate for the brain.
 *
 * Parameters
 * ev:	Strucuture containing event information.
 *
 * Return value
 * Always returns 1
 *
 */
function MovementXY(ev)
	var old_pri := Set_Priority(50);

	case ( ev.speed )
		NEMOVE_WALK:
			case ( ev.direction )
				NEMOVE_AWAY:
					repeat
						WalkAwayFromLocation(ev.x, ev.y);
						SleepMS(3);
					until ( !MoveLoopCheck(ev) );
				NEMOVE_TOWARD:
					repeat
						WalkTowardLocation(ev.x, ev.y);
						SleepMS(3);
					until ( !MoveLoopCheck(ev) );
			endcase
		NEMOVE_RUN:
			case( ev.direction )
				NEMOVE_AWAY:
					repeat
						RunAwayFromLocation(ev.x, ev.y);
						SleepMS(3);
					until ( !MoveLoopCheck(ev) );
				NEMOVE_TOWARD:
					repeat
						RunTowardLocation(ev.x, ev.y);
						SleepMS(3);
					until ( !MoveLoopCheck(ev) );
			endcase
	endcase

	Set_Priority(old_pri);

	return 1;
endfunction

/*
 * MovementWander(ev)
 *
 * Purpose
 * Handles NPC wander movement for the brain.
 *
 * Parameters
 * ev:	Strucuture containing event information.
 *
 * Return value
 * Always returns 1
 *
 */
function MovementWander(ev)
	repeat
		Wander();
		SleepMS(3);
	until ( !MoveLoopCheck(ev) );
endfunction

/*
 * MoveLoop(ev)
 *
 * Purpose
 * Determines if the npc should continue trying to move
 * or if something else needs the brain's attention.
 *
 * Parameters
 * ev:	Strucuture containing event information.
 *
 * Return value
 * Returns 1 if the npc may continue moving.
 *
 */
function MoveLoopCheck(byref ev)
	if ( Events_Waiting() )
		return 0;
	elseif ( ev.loop < 1 )
		return 0;
	elseif ( npc.frozen )
		return 0;
	elseif ( npc.paralyzed )
		return 0;
	endif
	if ( ev.source )
		// Dont need to worry about distance in an XY movement check.
		// It wont run in circles around XY coordinates like it does an object.
		if ( ev.direction == NEMOVE_TOWARD )
			if ( Distance(npc, ev.source) <= 1 )
				return 0;
			endif
		endif
	endif

	ev.loop -= 1;

	return 1;
endfunction
