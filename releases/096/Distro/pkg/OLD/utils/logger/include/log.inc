/* $Id: log.inc 87 2006-05-23 19:04:43Z sroyalty $
 *
 * Purpose
 * Provide various functions and constants to log stuff
 *
 * TODO:
 * Implement log roll over.
 *
 */
use basic;
use file;

include "include/time";

enum LOG_FILES
	LOG_MAIN        := "::log/mainlog.log",
	LOG_ERROR       := "::log/errorlog.log",
	LOG_WARNING     := "::log/warning.log",
	LOG_ACCESS      := "::log/access.log",
	LOG_COMMAND     := "::log/commands.log"
endenum


/*
 * Log(file, line)
 *
 * Author: Folko
 *
 * Purpose
 * Adds a timestamp to a string and logs it to a file.
 *
 * Parameters
 * file:    The logfile where the string will be logged to
 * line:    The string to log
 *
 * Return value
 * No return value.
 * 
 */
function Log(file, line)
	LogToFile(file, AddTimeStamp(line));

	return;
endfunction


/*
 * GetLogInfo(what)
 *
 * Author: Folko
 *
 * Purpose
 * Returns name and serial for logging purposes.
 *
 * Parameters
 * what:    The thing of which to return name and serial
 *
 * Return value
 * String describing the thing for logging
 * 
 */
function GetLogInfo(what)
	if(what.isA(POLCLASS_MOBILE))
		return(what.name + "(" + Hex(what.serial) + ")");
	else
		return(what.desc + "(" + Hex(what.serial) + ")");
	endif

	return;
endfunction
