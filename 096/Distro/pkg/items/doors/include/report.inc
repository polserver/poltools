use uo;
use os;
use file;

CONST DOOR_REPORT_DISABLE	:= 0x0;
CONST DOOR_REPORT_CONSOLE	:= 0x1;
CONST DOOR_REPORT_SYSLOG	:= 0x2;
CONST DOOR_REPORT_BROADCAST	:= 0x4;
CONST DOOR_REPORT_ALL		:= 0x7;

function DOOR_ReportText(report_text:="?", flags:=DOOR_REPORT_DISABLE)
	if ( !flags )
		return 0;
	endif
	
	if ( flags & DOOR_REPORT_CONSOLE )
		Print(""+report_text);
	endif
	
	if ( flags & DOOR_REPORT_SYSLOG )
		var script_name := GetProcess(GetPid()).name;
		LogToFile("::log/doors.log", "["+script_name+"]: "+report_text, LOG_DATETIME);
	endif
	
	if ( flags & DOOR_REPORT_BROADCAST )
		Broadcast(""+report_text);
	endif
	
	return 1;
endfunction
