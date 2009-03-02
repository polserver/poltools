/* $Id$
 *	  Author: MontuZ
 *	  Created: Wednesday, May 24 2006
 *	  Version: 0.01
 */
 
use uo;
use os;
use file;

CONST GUILD_REPORT_DISABLE	:= 0x0;
CONST GUILD_REPORT_CONSOLE	:= 0x1;
CONST GUILD_REPORT_SYSLOG	:= 0x2;
CONST GUILD_REPORT_BROADCAST	:= 0x4;
CONST GUILD_REPORT_ALL		:= 0x7;

function GUILD_ReportText(report_text:="?", flags:=GUILD_REPORT_DISABLE)
	if ( !flags )
		return 0;
	endif
	
	if ( flags & GUILD_REPORT_CONSOLE )
		Print(""+report_text);
	endif
	
	if ( flags & GUILD_REPORT_SYSLOG )
		var script_name := GetProcess(GetPid()).name;
		LogToFile("::log/guilds.log", "["+script_name+"]: "+report_text, LOG_DATETIME);
	endif
	
	if ( flags & GUILD_REPORT_BROADCAST )
		Broadcast(""+report_text);
	endif
	
	return 1;
endfunction
