/* $Id: report.inc 783 2006-08-31 10:08:57Z austinheilman $
 *
 */

use uo;
use os;
use file;

CONST DECAY_REPORT_DISABLE	:= 0x0;
CONST DECAY_REPORT_CONSOLE	:= 0x1;
CONST DECAY_REPORT_SYSLOG	:= 0x2;
CONST DECAY_REPORT_BROADCAST	:= 0x4;
CONST DECAY_REPORT_ALL		:= 0x7;

function DECAY_ReportText(report_text:="?", flags:=DECAY_REPORT_DISABLE)
	if ( !flags )
		return 0;
	endif

	if ( flags & DECAY_REPORT_CONSOLE )
		Print(""+report_text);
	endif

	if ( flags & DECAY_REPORT_SYSLOG )
		var script_name := GetProcess(GetPid()).name;
		LogToFile("::log/decayCycle.log", "["+script_name+"]: "+report_text, LOG_DATETIME);
	endif

	if ( flags & DECAY_REPORT_BROADCAST )
		Broadcast(""+report_text);
	endif

	return 1;
endfunction
