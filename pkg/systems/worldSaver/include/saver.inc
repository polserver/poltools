/* $Id$
 *
 */

use uo;
use os;

// SAVER EVENTS
const EVENT_SAVE_NOW		:= 0x1;
const EVENT_START_SHUTDOWN	:= 0x2;
const EVENT_CANCEL_SHUTDOWN	:= 0x4;

/*
 * SVR_SendCommand(event_id, data)
 *
 * Purpose
 * Sends a command to the world saver. 
 *
 * Parameters
 * event_id:	An event constant ID#
 * data:	Data to send with the event ID.
 *
 * Return Values
 * Returns 1
 *
 */
function SVR_SendCommand(event_id, data:=0)
	var event := struct{"type":=event_id, "data":=data};
	
	return SVR_GetProcess().SendEvent(event);
endfunction

/* 
 * SVR_GetProcess()
 *
 * Purpose
 * Retrieves a script object for the world saver.
 *
 * Parameters
 *
 * Return Values
 * Returns a script object
 *
 */
function SVR_GetProcess()
	var pid := GetGlobalProperty("#WorldSaverPid");
	return GetProcess(pid);
endfunction
