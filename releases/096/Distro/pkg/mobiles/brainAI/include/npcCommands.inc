/*===============================================================
* Current Version
* npcCOMMANDS.INC - v1.0
* Updated 8/31/2005 6:38AM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

use uo;
use os;

include "include/sysEvent";
include ":brainAI:eventid";

/////////////////////////////////////////////////////////////////
// * Function List *
//
// AI_Attack(npc, opponent, no_wake:=WAKEUP)
// AI_ClearThoughts(npc, flags)
// AI_EndNerve(npc, nerve_name, no_wake:=WAKEUP)
// AI_Flee(npc, opponent, no_wake:=WAKEUP)
// AI_FowardEvent(npc, nerve_name, mask, data, no_wake:=WAKEUP)
// AI_Move(npc, object, to_from:=NEMOVE_TOWARD, speed:=NEMOVE_RUN, no_wake:=WAKEUP, loop:=0)
// AI_MoveXY(npc, x, y, to_from:=NEMOVE_TOWARD, speed:=NEMOVE_RUN, no_wake:=WAKEUP, loop:=0)
// AI_SetOpponent(npc, opponent, no_wake:=WAKEUP)
// AI_Sleep(npc)
// AI_Speak(npc, text, text_type:=SPEAK_TEXTTYPE_DEFAULT, do_event:=SPEAK_DOEVENT_DISABLE, no_wake:=WAKEUP)
// AI_StartNerve(npc, nerve_name, script_name, data, no_wake:=WAKEUP)
// AI_Turn(npc, turn_to, direction:=NETURN_TOWARD, no_wake:=WAKEUP)
// AI_TurnXY(npc, x, y, direction:=NETURN_TOWARD, no_wake:=WAKEUP)
// AI_WakeUp(npc)
// AI_Wander(npc, no_wake:=WAKEUP, loop:=0)
// AI_WarMode(npc, mode:=0, no_wake:=WAKEUP)
// 
/////////////////////////////////////////////////////////////////

/*
 * AI_Attack(npc, opponent, no_wake)
 *
 * Purpose
 * Triggers the NPC's combat script on opponent.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 * opponent:	What the NPC's combat nerve will attack.
 * no_wake:	Optional wakeup flag:
 *		WAKEUP - Will stop the sleep script if it is running - Default.
 *		NOWAKE - Will not stop the sleep script if it is running.
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_Attack(npc, opponent, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= SYSEVENT_ENGAGED;
	ev.+source	:= opponent;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction

/*
 * AI_ClearThoughts(npc, flags)
 *
 * Purpose
 * Clears the event queue for the script calling it and or the brain.
 *
 * Parameters
 * npc:		A reference to the NPC.
 * flags:	CLR_* flag for what scripts should have their event queues cleared.
 *		CLR_BRAIN - Clears the brain's queue.
 *		CLR_NERVE - Clears the nerve's queue.
 *		CLR_BOTH  - Clears the queue for both.
 *
 * Return value
 * Always returns 1
 *
 */
function AI_ClearThoughts(npc, flags)
	var brain	:= flags & CLR_BRAIN;
	var nerve	:= flags & CLR_NERVE;
	var both	:= flags & CLR_BOTH;

	if ( both || brain )
		brain := NPC.process;
		brain.clear_event_queue();
	endif

	if ( both || nerve )
		nerve := GetProcess(GetPid());
		nerve.clear_event_queue();
	endif

	return 1;
endfunction

/*
 * AI_EndNerve(npc, nerve_name, no_wake)
 *
 * Purpose
 * Used to tell the brain to stop a nerve script.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 * nerve_name:	String containing the name of the nerve (dictionary key).
 * no_wake:	Optional wakeup flag:
 *		WAKEUP - Will stop the sleep script if it is running - Default.
 *		NOWAKE - Will not stop the sleep script if it is running.
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_EndNerve(npc, nerve_name, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_ENDNERVE;
	ev.+name	:= nerve_name;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction

/*
 * AI_ForwardEvent(npc, nerve_name, mask, data, no_wake)
 *
 * Purpose
 * Used to send an event from one nerve to another.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 * nerve_name:	String containing the name of the nerve to forward the event to (dictionary key).
 * mask:	The type of event the nerve should handle the forward as. (example: SYSEVENT_LEFTAREA)
 * data:	Data that the nerve will receive.
 * no_wake:	Optional wakeup flag:
 *		WAKEUP - Will stop the sleep script if it is running - Default.
 *		NOWAKE - Will not stop the sleep script if it is running.
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_FowardEvent(npc, nerve_name, mask, data, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_FWDNERVE;
	ev.+NerveName	:= nerve_name;
	ev.+mask	:= mask;
	ev.+data	:= data;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction

/*
 * AI_Move(npc, opponent, to_from, speed, no_wake, loop)
 *
 * Purpose
 * Commands an NPC to move toward / away from an item or NPC.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 * object:	Item or NPC the NPC will move away from/toward.
 * to_from:	Direction the NPC will go:
 *		NEMOVE_TOWARD - Will make the NPC move toward the object - Default
 *		NEMOVE_AWAY   - Will make the NPC move away from the object.
 * speed:	Speed the NPC will move at:
 *		NEMOVE_RUN  - NPC will run
 *		NEMOVE_WALK - NPC will walk
 * no_wake:	Optional wakeup flag::
 *		WAKEUP - Will stop the sleep script if it is running - Default.
 *		NOWAKE - Will not stop the sleep script if it is running.
 * loop:	Number of steps for the NPC to take toward/away from the object.
 *		Note: Loop can be interrupted and end prematurely!
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_Move(npc, object, to_from:=NEMOVE_TOWARD, speed:=NEMOVE_RUN, no_wake:=WAKEUP, loop:=0)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_MOVE;
	ev.+source	:= object;
	ev.+direction	:= to_from;
	ev.+speed	:= speed;
	ev.+WakeUp	:= no_wake;
	ev.+loop	:= loop;
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction

/*
 * AI_MoveXY(npc, x, y, to_from, speed, no_wake, loop)
 *
 * Purpose
 * Commands an NPC to move toward or away from an XY coordinate.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 * x:		X coordinate to move away from/toward
 * y:		Y coordinate to move away from/toward
 * to_from:	Direction the NPC will go:
 *		NEMOVE_TOWARD - Will make the NPC move toward the object - Default
 *		NEMOVE_AWAY   - Will make the NPC move away from the object.
 * speed:	Speed the NPC will move at:
 *		NEMOVE_RUN  - NPC will run
 *		NEMOVE_WALK - NPC will walk
 * no_wake:	Optional wakeup flag::
 *		WAKEUP - Will stop the sleep script if it is running - Default.
 *		NOWAKE - Will not stop the sleep script if it is running.
 * loop:	Number of steps for the NPC to take toward/away from the object.
 *		Note: Loop can be interrupted and end prematurely!
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_MoveXY(npc, x, y, to_from:=NEMOVE_TOWARD, speed:=NEMOVE_RUN, no_wake:=WAKEUP, loop:=0)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_MOVE_XY;
	ev.+x		:= x;
	ev.+y		:= y;
	ev.+direction	:= to_from;
	ev.+speed	:= speed;
	ev.+WakeUp	:= no_wake;
	ev.+loop	:= loop;
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction

/*
 * AI_SetOpponent(npc, opponent, no_wake)
 *
 * Purpose
 * Places the NPC into war mode and sets an opponent.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 * opponent:	NPC reference that will be considered an opponent.
 * no_wake:	Optional wakeup flag:
 *		WAKEUP - Will stop the sleep script if it is running - Default.
 *		NOWAKE - Will not stop the sleep script if it is running.
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_SetOpponent(npc, opponent, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_OPPONENT;
	ev.+source	:= opponent;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction

/*
 * AI_Sleep(npc)
 *
 * Purpose
 * Instructs the NPC to go into sleep mode - shuts down all nerves.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_Sleep(npc)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_SLEEP;
	ev.+WakeUp	:= NOWAKE;
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction

/*
 * AI_Speak(npc, text, text_type, do_event, no_wake)
 *
 * Purpose
 * Commands the NPC to speak.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 * text:	The text the NPC will speak.
 * text_type:	text_type for NPC.em :
 *		SPEAK_TEXTTYPE_DEFAULT - Default
 *		SPEAK_TEXTTYPE_WHISPER
 *		SPEAK_TEXTTYPE_YELL
 * do_event:	Flags for sending a system event for speech to surrounding NPCs.
 *		SPEAK_DOEVENT_DISABLE - Default
 *		SPEAK_DOEVENT_ENABLE
 * no_wake:	Optional wakeup flag:
 *		WAKEUP - Will stop the sleep script if it is running - Default.
 *		NOWAKE - Will not stop the sleep script if it is running.
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_Speak(npc, text, text_type:=SPEAK_TEXTTYPE_DEFAULT, do_event:=SPEAK_DOEVENT_DISABLE, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_SPEAK;
	ev.+text	:= text;
	ev.+texttype	:= text_type;
	ev.+doevent	:= do_event;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction

/*
 * AI_StartNerve(npc, nerve_name, script_name, data, no_wake)
 *
 * Purpose
 * Used to start a custom nerve up from within a nerve.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 * nerve_name:	String containing the name of the nerve (dictionary key).
 * script_name:	Path of the script to start.
 * data:	Data to be sent to the nerve once it starts.
 * no_wake:	Optional wakeup flag:
 *		WAKEUP - Will stop the sleep script if it is running - Default.
 *		NOWAKE - Will not stop the sleep script if it is running.
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_StartNerve(npc, nerve_name, script_name, data, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_STARTNERVE;
	ev.+name	:= nerve_name;
	ev.+script	:= script_name;
	ev.+data	:= data;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction

/*
 * AI_Turn(npc, turn_to, direction, no_wake)
 *
 * Purpose
 * Commands the NPC to face away from / toward an object.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 * object:	item or moble reference to turn away from / toward.
 * direction:	Direction the NPC will face:
 *		NETURN_TOWARD - Will make the NPC turn toward the object - Default.
 		NETURN_AWAY   - Will make the NPC turn away from the object.
 * no_wake:	Optional wakeup flag:
 *		WAKEUP - Will stop the sleep script if it is running - Default.
 *		NOWAKE - Will not stop the sleep script if it is running.
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_Turn(npc, object, direction:=NETURN_TOWARD, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_TURN;
	ev.+source	:= object;
	ev.+direction	:= direction;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction

/*
 * AI_TurnXY(npc, x, y, direction, no_wake)
 *
 * Purpose
 * Commands the NPC to face away from / toward a coordinate.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 * x:		X coordinate to turn away from / toward.
 * y:		Y coordinate to turn away from / toward.
 * direction:	Direction the NPC will face:
 *		NETURN_TOWARD - Will make the NPC turn toward the object - Default.
 		NETURN_AWAY   - Will make the NPC turn away from the object.
 * no_wake:	Optional wakeup flag:
 *		WAKEUP - Will stop the sleep script if it is running - Default.
 *		NOWAKE - Will not stop the sleep script if it is running.
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_TurnXY(npc, x, y, direction:=NETURN_TOWARD, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_TURN_XY;
	ev.+x		:= x;
	ev.+y		:= y;
	ev.+direction	:= direction;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction

/*
 * AI_WakeUp(npc)
 *
 * Purpose
 * Sends an event to the NPC causing its sleep script to end.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_WakeUp(npc)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_WAKEUP;
	ev.+WakeUp	:= WAKEUP;
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction

/*
 * AI_Wander(npc, opponent, no_wake)
 *
 * Purpose
 * Commands the NPC to wander one step in a random direction.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 * no_wake:	Optional wakeup flag:
 *		WAKEUP - Will stop the sleep script if it is running - Default.
 *		NOWAKE - Will not stop the sleep script if it is running.
 * loop:	Number of times to attempt to wander.
 *		Note: Loop can be interrupted and end prematurely!
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_Wander(npc, no_wake:=WAKEUP, loop:=0)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_WANDER;
	ev.+WakeUp	:= no_wake;
	ev.+loop	:= CInt(loop);
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction

/*
 * AI_WarMode(npc, mode, no_wake)
 *
 * Purpose
 * Toggles warmode on the NPC. Setting to 0 also clears the opponent.
 *
 * Parameters
 * npc:		The NPC that will receive the command.
 * mode:	0 - Takes the NPC out of warmode and clears the opponent - Default.
 *		1 - Places the NPC into warmode.
 * no_wake:	Optional wakeup flag:
 *		WAKEUP - Will stop the sleep script if it is running - Default.
 *		NOWAKE - Will not stop the sleep script if it is running.
 *
 * Return value
 * Result of SendEvent()
 *
 */
function AI_WarMode(npc, mode:=0, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_WARMODE;
	ev.+value	:= mode;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());

	return SendEvent(npc, ev);
endfunction
