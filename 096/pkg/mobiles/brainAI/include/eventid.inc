/*===============================================================
* Current Version
* EVENTID.INC - v1.0
* Updated 8/31/2005 5:50AM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

//
// * Custom brain AI event IDs
//
CONST NPCEVENT_MOVE				:= 0xA00003;	// Has NEMOVE_* helper CONSTants
CONST NPCEVENT_OPPONENT				:= 0xA00004;	// Set New Opponent (.source)
CONST NPCEVENT_SPEAK				:= 0xA00005;	// Speak (.text)
CONST NPCEVENT_TURN				:= 0xA00006;	// Turn towards or away from an object
CONST NPCEVENT_WARMODE				:= 0xA00007;	// Set WarMode state (.value=0|1)
CONST NPCEVENT_ATTACK				:= 0xA0000A;	// Tell the npc to attack something
CONST NPCEVENT_MOVE_XY				:= 0xA0000B;	// Tell the NPC to move to coordinates
CONST NPCEVENT_TURN_XY				:= 0xA0000C;	// Tell the NPC to turn towards coordinates
CONST NPCEVENT_WANDER				:= 0xA0000D;	// Tell the npc to wander

CONST NPCEVENT_ENDNERVE				:= 0xA0000F;	// Tell the brain to stop a nerve.
CONST NPCEVENT_STARTNERVE			:= 0xA00010;	// Tell the brain to start a nerve.
CONST NPCEVENT_DONE				:= 0xA00011;	// Brain tells a nerve that its instruction is done.

CONST NPCEVENT_SLEEP				:= 0xA00012;	// Tell the brain to go to sleep.
CONST NPCEVENT_WAKEUP				:= 0xA00013;	// Tell the brain to wake up.

CONST NPCEVENT_FWDNERVE				:= 0xA00014;	// Tell the brain to forward an event to a nerve.

//
// * NPCEVENT_MOVE helper CONSTants (.source = object)
//
CONST NEMOVE_WALK				:= 1;	// .speed (default)
CONST NEMOVE_RUN				:= 2;	// .speed
CONST NEMOVE_TOWARD 				:= 1;	// .direction (default)
CONST NEMOVE_AWAY				:= 2;	// .direction

//
// * NE_TURN helper CONSTants
//
CONST NETURN_TOWARD				:= 1;
CONST NETURN_AWAY				:= 2;

//
// * AWAKEN CONSTANTS
// 
CONST WAKEUP					:= 0;
CONST NOWAKE					:= 1;

//
// * CLEAR THOUGHT CONSTANTS
//
CONST CLR_BRAIN					:= 1;
CONST CLR_NERVE					:= 2;
CONST CLR_BOTH					:= 3;

//
//* NPC.EM CONSTants (only brain can access npc.em)
//
// CONSTants for texttype flags in the AI_Speak() function
CONST SPEAK_TEXTTYPE_DEFAULT			:= "default";
CONST SPEAK_TEXTTYPE_WHISPER			:= "whisper";
CONST SPEAK_TEXTTYPE_YELL			:= "yell";

// CONSTants for doevent flag in AI_Speak() function
CONST SPEAK_DOEVENT_DISABLE			:= 0x0;
CONST SPEAK_DOEVENT_ENABLE			:= 0x1;
