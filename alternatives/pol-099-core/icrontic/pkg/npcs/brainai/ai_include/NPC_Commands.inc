//////////////
//
// NPC_Commands.inc
//
// Created 7/19/2002 2:33PM
// Updated 2/13/2003 12:12AM
//
/////////////

include "NPC_Event";
include "include/sysevent";

use uo;
use os;

// 095 specific function. 
// Clears the event queue for the nerve and/or the brain.
// See npc_event.inc for the CLR_* flags.
function AI_ClearThoughts(mobile, flags)
	var brain := flags & CLR_BRAIN;
	var nerve := flags & CLR_NERVE;
	var both := flags & CLR_BOTH;
	
	if ( both || brain )
		var brain := mobile.process;
		brain.clear_event_queue();
	endif
	
	if ( both || nerve )
		var nerve := GetProcess(GetPid());
		nerve.clear_event_queue();
	endif
endfunction

//Command the npc to move to another object or mobile
//Pass the npc to command, the object its moving to/from,
//to move toward/away from the object, and the speed to do it.
function AI_Move(mobile, object, to_from := NEMOVE_TOWARD, speed := NEMOVE_RUN, no_wake:=WAKEUP, loop:=0)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_MOVE;
	ev.+source	:= object;
	ev.+direction	:= to_from;
	ev.+speed	:= speed;
	ev.+WakeUp	:= no_wake;
	ev.+loop	:= loop;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Command the npc to move toward/away from an X,Y coordinate
//Pass the npc to command, the object its moving to/from,
//to move toward/away from the coordinate, and the speed to do it.
function AI_Move_XYZ(mobile, x, y, to_from := NEMOVE_TOWARD, speed := NEMOVE_RUN, no_wake:=WAKEUP, loop:=0)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_MOVE_XYZ;
	ev.+x		:= x;
	ev.+y		:= y;
	ev.+direction	:= to_from;
	ev.+speed	:= speed;
	ev.+WakeUp	:= no_wake;
	ev.+loop	:= loop;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Instructs the npc to wander a little.
function AI_Wander(mobile, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_WANDER;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Tell the npc to face an object or mobile.
function AI_Turn(mobile, turn_to, direction := NETURN_TOWARD, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_TURN;
	ev.+source	:= turn_to;
	ev.+direction	:= direction;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Tell the npc to face an X,Y coordinate
function AI_Turn_XYZ(mobile, x, y, direction := NETURN_TOWARD, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_TURN_XYZ;
	ev.+x		:= x;
	ev.+y		:= y;
	ev.+direction	:= direction;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Set the NPC's opponent
//This also enables warmode
function AI_SetOpponent(mobile, opponent, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_OPPONENT;
	ev.+source	:= opponent;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Toggle combat mode for the npc.
//Turning it off also clears the opponent
function AI_Warmode(mobile, mode:=1, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_WARMODE;
	ev.+value	:= mode;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());

	SendEvent(mobile, ev);
endfunction

//Takes the npc out of warmode.
//Similar to doing AI_WarMode(mobile, 0)
function AI_Disengage(mobile, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_WARMODE;
	ev.+value	:= 0;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Command the npc to speak.
function AI_Speak(mobile, text:="!?", no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_SPEAK;
	ev.+text	:= text;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Instructs the brain to start a new nerve.
//nerve_name is the name of the nerve, script_name
//is the path of the script to start, data is the information
//to send to the nerve once it is started. If nerve_name already
//exists as an active nerve, the data is sent on to it.
function AI_StartNerve(mobile, nerve_name, script_name, data, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_STARTNERVE;
	ev.+name	:= nerve_name;
	ev.+script	:= script_name;
	ev.+data	:= data;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Instructs the brain to stop a nerve
function AI_EndNerve(mobile, nerve_name, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_ENDNERVE;
	ev.+name	:= nerve_name;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());

	SendEvent(mobile, ev);
endfunction


//Tell the brain that the npc has won a fight.
//The npc will run its win script, if it has one.
function AI_WinFight(mobile, opponent, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_WINFIGHT;
	ev.+source	:= opponent;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Attack an opponent.
//Starts the npc's combat nerve, if it has one.
function AI_Attack(mobile, opponent, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= SYSEVENT_ENGAGED;
	ev.+source	:= opponent;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Tells the brain that it is time to flee.
//This will instruct the brain to run the npc's flee nerve.
function AI_Flee(mobile, opponent, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_FLEE;
	ev.+source	:= opponent;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Forwards information to a nerve.
//Nerve_name is the name of the nerve to forward information to.
//Mask is the event the nerve should receive.
//Data is the information sent to the nerve.
//If there is no nerve running under nerve_name, the data is discarded.
function AI_FowardEvent(mobile, nerve_name, mask, data, no_wake:=WAKEUP)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_FWDNERVE;
	ev.+NerveName	:= nerve_name;
	ev.+mask	:= mask;
	ev.+data	:= data;
	ev.+WakeUp	:= no_wake;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Forces the npc to go to sleep and run its sleep script.
function AI_Sleep(mobile)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_SLEEP;
	ev.+WakeUp	:= NOWAKE;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

//Forces the npc to stop its sleep script, and begin running
//its cycle script.
function AI_WakeUp(mobile)
	var ev		:= struct;
	ev.+type	:= NPCEVENT_WAKEUP;
	ev.+WakeUp	:= WAKEUP;
	ev.+process	:= GetProcess(GetPid());
	
	SendEvent(mobile, ev);
endfunction

