/*
        Maintened by *Edwards

        2009-10-10
*/
use uo;
use os;

include "include/sysEvent";
include ":brainAI:eventid";

/////////////////////////////////////////////////////////////////
// * Function List *
//
// AI_Attack(npc, opponent, no_wake:=NOWAKE)
// AI_ClearThoughts(npc, flags)
// AI_EndNerve(npc, nerve_name, no_wake:=NOWAKE)
// AI_Flee(npc, opponent, no_wake:=NOWAKE)
// AI_FowardEvent(npc, nerve_name, mask, data, no_wake:=NOWAKE)
// AI_Move(npc, x, y, to_from:=NEMOVE_TOWARD, speed:=NEMOVE_RUN, loop:=0, no_wake:=NOWAKE)
// AI_SetAnchor( npc, distance_start, percent_subtract, no_wake:=NOWAKE)
// AI_SetOpponent(npc, opponent, no_wake:=NOWAKE)
// AI_Sleep(npc)
// AI_Speak(npc, text, text_type:=SPEAK_TEXTTYPE_DEFAULT, do_event:=SPEAK_DOEVENT_DISABLE, no_wake:=NOWAKE)
// AI_StartNerve(npc, nerve_name, script_name, data, no_wake:=NOWAKE)
// AI_Turn(npc, turn_to, direction:=NETURN_TOWARD, no_wake:=NOWAKE)
// AI_TurnXY(npc, x, y, direction:=NETURN_TOWARD, no_wake:=NOWAKE)
// AI_WakeUp(npc)
// AI_Wander(npc, no_wake:=NOWAKE, loop:=0)
// AI_WarMode(npc, mode:=0, no_wake:=NOWAKE)
//
/////////////////////////////////////////////////////////////////

function AI_Attack( npc, opponent, no_wake:=WAKEUP )

	var ev		:= struct;
	ev.+type	:= SYSEVENT_ENGAGED;
	ev.+source	:= opponent;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess( GetPid() );

	return SendEvent( npc, ev );
endfunction

function AI_ClearThoughts( npc, flags )

	var brain	:= flags & CLR_BRAIN;
	var nerve	:= flags & CLR_NERVE;
	var both	:= flags & CLR_BOTH;

	if( both || brain )
		brain := NPC.process;
		brain.clear_event_queue();
	endif

	if( both || nerve )
		nerve := GetProcess( GetPid() );
		nerve.clear_event_queue();
	endif

	return 1;
endfunction

function AI_EndNerve( npc, nerve_name, no_wake:=NOWAKE )

	var ev		:= struct;
	ev.+type	:= NPCEVENT_ENDNERVE;
	ev.+name	:= nerve_name;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess( GetPid() );

	return SendEvent( npc, ev );
endfunction

function AI_FowardEvent( npc, nerve_name, mask, data, no_wake:=NOWAKE )

	var ev		:= struct;
	ev.+type	:= NPCEVENT_FWDNERVE;
	ev.+NerveName	:= nerve_name;
	ev.+mask	:= mask;
	ev.+data	:= data;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess( GetPid() );

	return SendEvent( npc, ev );
endfunction

function AI_Move( npc, x, y, z, to_from:=NEMOVE_TOWARD, speed:=NEMOVE_RUN, loop:=0, no_wake:=NOWAKE )

	var ev		:= struct;
	ev.+type	:= NPCEVENT_MOVE;
	ev.+x		:= x;
	ev.+y		:= y;
	ev.+z		:= z;
	ev.+direction	:= to_from;
	ev.+speed	:= speed;
	ev.+WakeUp	:= no_wake;
	ev.+loop	:= loop;
	ev.+process	:= GetProcess( GetPid() );

	return SendEvent( npc, ev );
endfunction

function AI_SetAnchor( npc, x, y, distance_start, percent_subtract, no_wake:=NOWAKE )

	var ev		:= struct;
	ev.+type	:= NPCEVENT_ANCHOR;
	ev.+x		:= x;
	ev.+y		:= y;
	ev.+distance	:= distance_start;
	ev.+percent	:= percent_subtract;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess( GetPid() );
 
	return SendEvent( npc, ev );
endfunction

function AI_SetOpponent( npc, opponent, no_wake:=WAKEUP )

	var ev		:= struct;
	ev.+type	:= NPCEVENT_OPPONENT;
	ev.+source	:= opponent;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess( GetPid() );

	return SendEvent( npc, ev );
endfunction

function AI_Sleep( npc, no_wake:=NOWAKE )

	var ev		:= struct;
	ev.+type	:= NPCEVENT_SLEEP;
	ev.+WakeUp	:= NOWAKE;
	ev.+process	:= GetProcess( GetPid() );

	return SendEvent( npc, ev );
endfunction

function AI_Speak( npc, text, text_type:=SPEAK_TEXTTYPE_DEFAULT, do_event:=SPEAK_DOEVENT_DISABLE, no_wake:=NOWAKE )

	var ev		:= struct;
	ev.+type	:= NPCEVENT_SPEAK;
	ev.+text	:= text;
	ev.+texttype	:= text_type;
	ev.+doevent	:= do_event;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess( GetPid() );

	return SendEvent( npc, ev );
endfunction

function AI_StartNerve( npc, nerve_name, script_name, data, no_wake:=NOWAKE )

	var ev		:= struct;
	ev.+type	:= NPCEVENT_STARTNERVE;
	ev.+name	:= nerve_name;
	ev.+script	:= script_name;
	ev.+data	:= data;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess( GetPid() );

	return SendEvent( npc, ev );
endfunction

function AI_Turn( npc, object, direction:=NETURN_TOWARD, no_wake:=NOWAKE )

	var ev		:= struct;
	ev.+type	:= NPCEVENT_TURN;
	ev.+source	:= object;
	ev.+direction	:= direction;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess( GetPid() );

	return SendEvent( npc, ev );
endfunction

function AI_TurnXY( npc, x, y, direction:=NETURN_TOWARD, no_wake:=NOWAKE )

	var ev		:= struct;
	ev.+type	:= NPCEVENT_TURN_XY;
	ev.+x		:= x;
	ev.+y		:= y;
	ev.+direction	:= direction;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess( GetPid() );

	return SendEvent( npc, ev );
endfunction

function AI_WakeUp( npc )

	var ev		:= struct;
	ev.+type	:= NPCEVENT_WAKEUP;
	ev.+WakeUp	:= WAKEUP;
	ev.+process	:= GetProcess( GetPid() );

	return SendEvent( npc, ev );
endfunction

function AI_Wander( npc, loop:=0, no_wake:=NOWAKE )

        if( GetObjProperty( npc, "#Follow" ))
                return 0;
        endif

	var ev		:= struct;
	ev.+type	:= NPCEVENT_WANDER;
	ev.+WakeUp	:= no_wake;
	ev.+loop	:= CInt( loop );
	ev.+process	:= GetProcess( GetPid() );

	return SendEvent( npc, ev );
endfunction

function AI_WarMode( npc, mode:=0, no_wake:=NOWAKE )

	var ev		:= struct;
	ev.+type	:= NPCEVENT_WARMODE;
	ev.+value	:= mode;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess( GetPid() );

	return SendEvent( npc, ev );
endfunction
