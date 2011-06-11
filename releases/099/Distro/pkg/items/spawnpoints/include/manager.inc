/*
        Creatred by *Edwards

        2010-03-29
*/
use uo;
use os;
use util;

include ":spawnpoints:spawnpointConstants";

function SPM_SendCommand( event_id, spawnpoint )

	var event := struct{"type":=event_id, "spawnpoint":=spawnpoint};
 
	return SPM_GetProcess().SendEvent( event );
endfunction

function SPM_GetProcess()

	var pid := GetGlobalProperty( "#SpawnpointManagerPid" );
 
	return GetProcess( pid );
endfunction

function RegisterSpawnPoints()

        var datafile := SPM_GetDataFile(),
            data_serials := DFGetElemNames( datafile );
            
        list_serials := array{}; //Cleans the array.
            
        foreach serial in data_serials
                
                var spawnpoint := SystemFindObjectBySerial( CInt( serial ));
                if( spawnpoint )
                        list_serials.Append( serial );
                        CheckSpawnpoint( spawnpoint );
                else
                        SpawnpointRemoveData( serial );
                        DeleteSpawnpoint( spawnpoint );
                endif
                
                SleepMS(50);
        endforeach

	ReportText( "Spawnpoints manager initialized. "+list_serials.Size()+" points were registered.", REPORT_SYSLOG );

	return 1;
endfunction
