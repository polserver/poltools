/*
        Maintened by *Edwards

        2009-02-10
*/
use uo;
use os;
use util;

include ":gumps:gumps";
include ":gumps:gumps_ex";
include ":security:report";

// SAVER EVENTS
CONST EVENT_SAVE_NOW		:= 0x1;
CONST EVENT_START_SHUTDOWN	:= 0x2;
CONST EVENT_CANCEL_SHUTDOWN	:= 0x4;
CONST EVENT_STOPSAVING  	:= 0x5;

function SVR_SendCommand( event_id, data:=0 )

	var event := struct{"type":=event_id, "data":=data};
	return SVR_GetProcess().SendEvent( event );
endfunction

function SVR_GetProcess()

	var pid := GetGlobalProperty( "#WorldSaverPid" );
	return GetProcess( pid );
endfunction

function ShouldSave( byref savetimes )

	if( savetimes.nextsave <= POLCore().systime )
		return 1;
	endif

	return 0;
endfunction

function GetWaitTime( byref savetimes )

	var wait_time := savetimes.nextsave - savetimes.lasttick;

	return wait_time;
endfunction

function UpdateTimes( byref savetimes )

	var cfg_settings := SVR_GetSettingsCfgElem( "Settings" );

	var wait := CInt( cfg_settings.SaveTime ) * 60;
	if( !wait )
		wait := 3600; // 1 hour if not set.
		ReportText( "Error - No SaveTime setting set in settings.cfg", REPORT_SYSLOG );
	endif

	savetimes.lasttick := POLCore().systime; 	// Time there was a save or event.
	savetimes.nextsave := POLCore().systime + wait;	// Time the next save will happen.

	return 1;
endfunction

function WarnPlayers();

        foreach player in EnumerateOnlineCharacters();
               var send := start_script( ":worldSaver:gumpSaver", player );
               if( send.errortext )
                       SysLog( "Error sending SaveGump -->"+send.errortext );
               endif
               SleepMS(100);
        endforeach

        return 1;
endfunction

function DoShutDown( countdown:=0 )

	countdown := CInt( countdown );

	for( countdown; countdown>1; countdown-=1 )
		ReportText( "SYSTEM: Zulu Hotel is restarting in "+countdown+" minutes.", REPORT_BROADCAST, 33 );
		Sleep(60);
	endfor
 
        var i;
        for( i:=60; i>1; i-=1 )
		ReportText( "SYSTEM: Zulu Hotel is restarting in "+i+" seconds.", REPORT_BROADCAST, 33 );
		Sleep(1);
        endfor

	ReportText( "SYSTEM: Shutting down now!", REPORT_ALL, 33 );
	Sleep(1);

	ShutDown();

	return 1;
endfunction

function DoTheSave()

        var acctWatch := start_script( ":accounts:acctWatcher/acctWatcher" );
        if( acctWatch.errortext )
               SysLog( "Error starting <:accounts:acctWatcher/acctWatcher> -->"+acctWatch.errortext );
        endif

        Set_Critical(1);
	var save_result := SaveWorldState();
        Set_Critical(0);

	if( save_result == error )
		ReportText( "!!! Error saving world state. Restarting Zulu Hotel in 20 seconds !!!", REPORT_ALL );
		Sleep(20);
		ShutDown();
	else
		var save_time := save_result.ElapsedMilliseconds;
		save_time := CDbl( CDbl( save_time ) / CDbl( 1000 ));
		save_time := FormatRealToString( save_time, 2 );

                Sleep(2);
		ReportText( "SYSTEM: Finished World Save in "+save_time+" seconds.", REPORT_BROADCAST, 33 );
	endif
 
        SaveServerUptime();

	return 1;
endfunction

function SaveServerUptime()

	var core_ref := POLCore();

        var max_uptime := CInt( GetGlobalProperty( "MaxUptime" ));
        if( !max_uptime )
                max_uptime := core_ref.uptime;
        elseif( max_uptime < CInt( core_ref.uptime ))
                max_uptime := core_ref.uptime;
        endif

        SetGlobalProperty( "MaxUptime", CInt( max_uptime ));

        return 1;
endfunction
