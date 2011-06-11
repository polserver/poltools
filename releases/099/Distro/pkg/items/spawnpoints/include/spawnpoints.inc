/*
        Creatred by *Edwards

        2010-03-29
*/
use uo;
use os;

include ":datafile:datafile";
include ":loot:lootParser";
include ":security:report";
include ":spawnpoints:settings";
include ":spawnpoints:spawning";
include ":spawnsystem:groups";
include ":timeutils:time";

function SpawnpointSaveData( spawnpoint )

        var datafile := SPM_GetDataFile(),
            data_elem := DFFindElement( datafile, spawnpoint.serial, DF_CREATE );

        data_elem.SetProp( "Realm", spawnpoint.realm );
        data_elem.SetProp( "X", spawnpoint.x );
        data_elem.SetProp( "Y", spawnpoint.y );
        data_elem.SetProp( "Z", spawnpoint.z );

	return 1;
endfunction

function SpawnpointRemoveData( spawnpoint_serial )

        var data_file := DFOpenDataFile( ":spawnpoints:spawnpoints", DF_NO_CREATE );
        if( data_file )
		var result := data_file.DeleteElement( CStr( spawnpoint_serial ));
                if( result.errortext )
                        ReportText( "Error couldn't remove spawnpoint serial "+spawnpoint_serial+" from datafile -->"+result.errortext, REPORT_SYSLOG );
                        return 0;
                endif
	endif

	return 1;
endfunction

function ValidateSpawnpoint( spawnpoint, sp_settings )

	var next_validate := CInt( GetObjProperty( spawnpoint, "#NextValidate" ));
        if( next_validate > ReadGameClock() )
                return 0;
        else
                SetObjProperty( spawnpoint, "#NextValidate", ReadGameClock() + SEC_MINUTE );
        endif

	var spawn_serials := GetObjProperty( spawnpoint, "SpawnSerials" );
	if( !spawn_serials )
		spawn_serials := array{};
	endif

        var validate_serials := array{};

	foreach serial in ( spawn_serials )
		var spawn := SystemFindObjectBySerial( serial );
                if( spawn )
			case( sp_settings[1] )
				"Item":
					if( !spawn.container )
						validate_serials.Append( serial );
					endif

				"Container":
					if( !TheresPlayersNear( spawn ))
						if( !spawn.locked )
							DestroyItem( spawn );
						elseif( !Len( EnumerateItemsInContainer( spawn )))
							DestroyItem( spawn );
						else
							validate_serials.Append( serial );
						endif
					else
						validate_serials.Append( serial );
					endif

				default: validate_serials.Append( serial );
			endcase
                endif
                SleepMS(5);
	endforeach

	SetObjProperty( spawnpoint, "SpawnSerials", validate_serials );

	return 1;
endfunction

function TheresPlayersNear( spawn )

	foreach mobile in ListMobilesNearLocationEx( spawn.x, spawn.y, spawn.z, 10, LISTEX_FLAG_NORMAL+LISTEX_FLAG_HIDDEN, spawn.realm )
		if( !mobile.IsA( POLCLASS_NPC ))
			if( !mobile.cmdlevel )
				return 1;
			endif
		endif
                SleepMS(5);
	endforeach

	return 0;
endfunction

function CheckSpawn( spawnpoint )

        var pid := CInt( GetObjProperty( spawnpoint, "#SpawnpointDelay" )),
            process := GetProcess( pid );

        if( process )
                return 0;
        endif
        
        var delay;
        
	var sp_settings := GetSpawnpointSettings( spawnpoint );
	if( sp_settings[6] )
		var next_spawn := GetObjProperty( spawnpoint, "#NextSpawn" );
		if( next_spawn )
			if( next_spawn > ReadGameClock() )
				delay := CInt( next_spawn - ReadGameClock() );
			endif
                else
                        delay := CInt( sp_settings[6] * SEC_MINUTE );
		endif
	endif

	SetObjProperty( spawnpoint, "#NextSpawn", delay );
	DelaySpawning( spawnpoint, delay );

	return 1;
endfunction

function DelaySpawning( spawnpoint, delay )

        delay += 5; // +5 to prevent lagging issues to alter spawning

	var start := Start_Script( ":spawnpoints:manager/delay", array{spawnpoint, delay} );
        if( start.errortext )
                ReportText( "Error startint script <:spawnpoints:manager/delay> -->"+start.errortext, REPORT_SYSLOG );
                return 0;
        endif

        return 1;
endfunction

function DespawnAll( spawnpoint )

	var sp_settings := GetSpawnpointSettings( spawnpoint );
	var spawn_serials := GetObjProperty( spawnpoint, "SpawnSerials" );

	foreach serial in spawn_serials
		var spawn := SystemFindObjectBySerial( serial );
		case( sp_settings[1] )
			"NPC":
			"Group":	SetObjProperty( spawn, "GuardKill", 1 );
					spawn.Kill();
					break;
			"Container":
			"Item":		DestroyItem( spawn );
					break;
		endcase

               	SleepMS(5);
	endforeach

	EraseObjProperty( spawnpoint, "SpawnSerials" );

	return 1;
endfunction

function SP_GetCoords( spawnpoint, sp_settings )

         var range := sp_settings[3],
             x := spawnpoint.x,
             y := spawnpoint.y;

         var coords := struct;
             coords.x1 := x - range;
             coords.y1 := y - range;
             coords.x2 := x + range;
             coords.y2 := y + range;

         return coords;
endfunction

function SP_OnDeath( corpse )

        var spawnpoint := SystemFindObjectBySerial( CInt( GetObjProperty( corpse, "spawnpoint_serial" )));
        if( spawnpoint )
                SPM_SendCommand( EVENT_SPAWNPOINT_DEATH, spawnpoint );
        endif

        return 1;
endfunction
