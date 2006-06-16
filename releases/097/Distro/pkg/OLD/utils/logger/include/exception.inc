/* $Id: exception.inc 87 2006-05-23 19:04:43Z sroyalty $
 *
 * Purpose
 * Provides various functions and constants for exceptions.
 *
 */
use uo;

include ":logger:log";


/*
 * Exception(what)
 *
 * Author: Folko
 *
 * Purpose
 * Raises and exception and logs it to the error log.
 *
 * Parameters
 * what:        A string describing the error
 *
 * Return value
 * Always returns an error-object
 *
 * Notes
 * Only use this for script errors etc., we don't want spam in the error log.
 *
 */
function Exception(what)
	print("******Exception: " + what + "******");
	Log(LOG_ERROR, "Exception: " + what);

	return(error);
endfunction
