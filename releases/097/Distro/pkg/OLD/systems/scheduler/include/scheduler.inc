/* $Id: scheduler.inc 87 2006-05-23 19:04:43Z sroyalty $
 *
 * Purpose
 * Provide various functions and constants for the automatic script loader / scheduler
 *
 * Notes
 * A PKG can register multiple scripts which are to be loaded when the server is started.
 * I decided to use this method instead of start.src because start.src starts all scripts
 * at the same time unless you run them critically.
 * To register scripts, simply add a file "autoload.cfg" to your PKG and add entries in this
 * form:
 *
 * Autoload PKGName
 * {
 *     Script       ScriptName      // The script to be started
 *     Resistent    1/0             // Does the script finish or loop?
 *     Name         FriendlyName    // A short, one word name for the action, like "Saver"
 *     Priority     0-255           // Priority, higher = started earlier
 * }
 *
 * When a script is started by the scheduler, it has to call Wait(SCHED_LOAD) immediately in
 * order to wait for it's scheduling time.
 * After that, let the script do it's work. If it's a resistent script, call Send(SCHED_LOOP)
 * right before you enter the script's loop.
 * If the script is not resistent, call Send(SCHED_DONE) before returning. *Always* send it,
 * even if the script fails, else the scheduler won't start the other scripts because it will
 * wait for yours!
 *
 */
use os;

include ":logger:exception";
include "include/properties";

const SCHED_TIMEOUT := 5;               // Timeout for Wait_For_Event()

enum SCHED_EVENTS
    SCHED_LOAD  := 1,
    SCHED_DONE  := 2,
    SCHED_LOOP  := 3
endenum


/*
 * Wait(what)
 *
 * Purpose
 * Waits for a signal.
 *
 * Parameters
 * what:    The signal to wait for
 *
 * Return value
 * No return value.
 *
 */
function Wait(what, time := SCHED_TIMEOUT)
	while( Wait_For_Event(time) != what )
	endwhile

	return 1;
endfunction


/*
 * Send(what)
 *
 * Purpose
 * Send a signal to the scheduler.
 *
 * Parameters
 * what:    The signal to send
 *
 * Return value
 * No return value.
 *
 */
function Send(what)
	var sched := GetProcess(GetGlobalProperty(TEMPPROP_SCHED_PID));
	if (!sched)
		return(Exception("Send(): Couldn't query scheduler"));
	endif
	sched.sendevent(what);

	return 1;
endfunction
