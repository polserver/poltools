//
// NPC_Event.inc
//
// Event IDs to be returned in 'type' member of wait_for_event()
// result returned within AI Brain-script.
//
// Created: 07/09/2001 15:42
// Updated: 2/13/2003 12:12AM
//

const NPCEVENT_MOVE				:= 0xA00003;	// Has NEMOVE_* helper constants
const NPCEVENT_OPPONENT				:= 0xA00004;	// Set New Opponent (.source)
const NPCEVENT_SPEAK				:= 0xA00005;	// Speak (.text)
const NPCEVENT_TURN				:= 0xA00006;	// Turn towards or away from an object
const NPCEVENT_WARMODE				:= 0xA00007;	// Set WarMode state (.value=0|1)
const NPCEVENT_WINFIGHT				:= 0xA00008;	// NPC killed '.source' mobile
const NPCEVENT_FLEE				:= 0xA00009;	// Tell the NPC to flee
const NPCEVENT_ATTACK				:= 0xA0000A;	// Tell the npc to attack something
const NPCEVENT_MOVE_XYZ				:= 0xA0000B;	// Tell the NPC to move to coordinates
const NPCEVENT_TURN_XYZ				:= 0xA0000C;	// Tell the NPC to turn towards coordinates
const NPCEVENT_WANDER				:= 0xA0000D;	// Tell the npc to wander

const NPCEVENT_ENDNERVE				:= 0xA0000F;	// Tell the brain to stop a nerve.
const NPCEVENT_STARTNERVE			:= 0xA00010;	// Tell the brain to start a nerve.
const NPCEVENT_DONE				:= 0xA00011;	// Brain tells a nerve that its instruction is done.

const NPCEVENT_SLEEP				:= 0xA00012;	// Tell the brain to go to sleep.
const NPCEVENT_WAKEUP				:= 0xA00013;	// Tell the brain to wake up.

const NPCEVENT_FWDNERVE				:= 0xA00014;	// Tell the brain to forward an event to a nerve.

// NPCEVENT_MOVE helper constants (.source = object)
const NEMOVE_WALK				:= 1;	// .speed (default)
const NEMOVE_RUN				:= 2;	// .speed
const NEMOVE_TOWARD 				:= 1;	// .direction (default)
const NEMOVE_AWAY				:= 2;	// .direction

// NE_TURN helper constants
const NETURN_TOWARD				:= 1;
const NETURN_AWAY				:= 2;

// WAKE CONSTANTS
// Wakeup is set to 0, since the core events wont send
// a wakeup parameter with the event.
const WAKEUP					:= 0;
const NOWAKE					:= 1;

// CLEAR THOUGHT CONSTANTS
const CLR_BRAIN					:= 1;
const CLR_NERVE					:= 2;
const CLR_BOTH					:= 3;
