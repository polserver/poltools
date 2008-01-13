use uo;
use os;
use file;

CONST MERC_REPORT_DISABLE	:= 0x0;
CONST MERC_REPORT_CONSOLE	:= 0x1;
CONST MERC_REPORT_SYSLOG	:= 0x2;
CONST MERC_REPORT_BROADCAST	:= 0x4;
CONST MERC_REPORT_ALL		:= 0x7;

function MERC_ReportText(report_text:="?", flags:=MERC_REPORT_DISABLE)
	if ( !flags )
		return 0;
	endif

	if ( flags & MERC_REPORT_CONSOLE )
		Print(""+report_text);
	endif

	if ( flags & MERC_REPORT_SYSLOG )
		var script_name := GetProcess(GetPid()).name;
		LogToFile("::log/merchants.log", "["+script_name+"]: "+report_text, LOG_DATETIME);
	endif

	if ( flags & MERC_REPORT_BROADCAST )
		Broadcast(""+report_text);
	endif

	return 1;
endfunction
