use uo;
use os;
use file;

CONST SPAWN_REPORT_DISABLE	:= 0;
CONST SPAWN_REPORT_CONSOLE	:= 1;
CONST SPAWN_REPORT_SYSLOG	:= 2;
CONST SPAWN_REPORT_BROADCAST	:= 4;

function SS_ReportText(report_text:="?", flags:=SPAWN_REPORT_SYSLOG)
	if ( flags & SPAWN_REPORT_CONSOLE )
		Print(""+report_text);
	endif
	
	if ( flags & SPAWN_REPORT_SYSLOG )
		var script_name := GetProcess(GetPid()).name;
		LogToFile("::log/spawnSystem.log", "["+script_name+"]: "+report_text, LOG_DATETIME);
	endif
	
	if ( flags & SPAWN_REPORT_BROADCAST )
		foreach mobile in EnumerateOnlineCharacters()
			if ( mobile.cmdlevel )
				SendSysMessage(mobile, ""+report_text);
			endif
			sleepms(5);
		endforeach
	endif
	
	return 1;
endfunction
