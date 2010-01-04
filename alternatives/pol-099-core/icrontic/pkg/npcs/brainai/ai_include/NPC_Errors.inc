//////////////
//
// NPC_Errors.inc
//
// Created 7/19/2002 2:33PM
// Updated 7/19/2002 2:33PM
//
// By TJ Houstin
// http://www.tsse.net
//
////////////////////////////////
// 
// This is for handling errors on npc creation.
// 
///////////////////////////////

// Kill me and log the reason to the SysLog
// (conceal me, if required [default: yes])
//
function KillMe(errmsg, conceal:=1)
	Say(errmsg);
	DoSysLog(errmsg);
	SetProperty("GuardKill", 1);
	if (conceal)
		Self().cmdlevel := 1;
		Self().concealed := 1;
	endif
	Self().disable("invul");
	ApplyRawDamage(Self(), GetVitalMaximumValue(Self(), "Hits"));
endfunction

//
// Log a message (and some of my details) to SysLog
//
function DoSysLog( errmsg )
	syslog( "-Brain Error-" );
	syslog( errmsg );
	syslog( "NPCTemplate: " + Self().npctemplate );
	syslog( "NPCName    : " + Self().name );
	SysLog( "NPCSerial  : " + Self().serial );
	syslog( "------------" );
endfunction
