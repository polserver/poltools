use uo;
use os;
use file;

CONST SAVER_REPORT_DISABLE	:= 0x0;
CONST SAVER_REPORT_CONSOLE	:= 0x1;
CONST SAVER_REPORT_SYSLOG	:= 0x2;
CONST SAVER_REPORT_BROADCAST	:= 0x4;
CONST SAVER_REPORT_ALL		:= 0x7;

function SVR_ReportText(report_text:="?", flags:=SAVER_REPORT_DISABLE)
	if ( !flags )
		return 0;
	endif
	
	if ( flags & SAVER_REPORT_CONSOLE )
		Print(""+report_text);
	endif
	
	if ( flags & SAVER_REPORT_SYSLOG )
		var script_name := GetProcess(GetPid()).name;
		LogToFile("::log/worldSaver.log", "["+script_name+"]: "+report_text, LOG_DATETIME);
	endif
	
	if ( flags & SAVER_REPORT_BROADCAST )
		Broadcast(""+report_text);
	endif
	
	return 1;
endfunction
