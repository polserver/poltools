/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2010-01-03
*/
use uo;
use polsys;
use util;

include ":brainAI:npcCommands";

const START_MESSAGE := 0x1;
const END_MESSAGE := 0x2;
const FAIL_MESSAGE := 0x3;

function SetupChestGold( location, level )

         var location_elem := TRaid_GetCfgElem( location, ":townRaid:locations" ),
             chest_loc := SplitWords( location_elem.GoalPos ),
             realm := location_elem.Realm,
             locx := CInt( chest_loc[1] ),
             locy := CInt( chest_loc[2] ),
             locz := GetWorldHeight( locx, locy );

         var chest := CreateItemAtLocation( locx, locy, locz, 0xffe3, 1, realm );
         if( chest.errortext )
                SysLog( "Error creating townRaid chest -->"+chest.errortext );
                return 0;
         endif

         var level_elem := TRaid_GetCfgElem( level, ":townRaid:levels" ),
            gold := CInt( level_elem.GoldAmount );

         SetObjProperty( chest, "GoldAmount", gold );

         chest.movable := 0;
         chest.saveonexit := 0;
         chest.locked := 1;

         return chest;
endfunction

function SendStatusMessage( location, status )

        var location_elem := TRaid_GetCfgElem( location, ":townRaid:locations" );

        var message;
        case( status )
                START_MESSAGE:
                         message := location_elem.StartMessage;
                         break;
                END_MESSAGE:
                         message := location_elem.EndMessage;
                         break;
                FAIL_MESSAGE:
                         message := location_elem.FailMessage;
                         break;
        endcase

        Broadcast( "Town Raid: "+message, 3, 20 );

        return 1;
endfunction

function CreateSpawn( chest, location, raiders )

         var group_elem := TRaid_GetCfgElem( raiders, ":spawnsystem:defaultGroups" );
         var elems := GetConfigStringArray( group_elem, "Entry" );
         if( elems.errortext )
                SysLog( "Error reading entry -->"+elems.errortext );
         endif

         var spawn := elems.RandomEntry();
         
         var location_elem := TRaid_GetCfgElem( location, ":townRaid:locations" ),
             spawn_loc := SplitWords( location_elem.SpawnRect ),
             realm := location_elem.Realm,
             locx_min := CInt( spawn_loc[1] ),
             locy_min := CInt( spawn_loc[2] ),
             locx_max := CInt( spawn_loc[3] ),
             locy_max := CInt( spawn_loc[4] );

         var locx := RandomInt( locx_max-locx_min )+locx_min,
             locy := RandomInt( locy_max-locy_min )+locy_min,
             locz := GetWorldHeight( locx, locy );

         var npc := CreateNPCFromTemplate( spawn, locx, locy, locz, 0, realm );
         if( npc.errortext )
                  SysLog( "Error creating townRaid npc <"+spawn+"> -->"+npc.errortext );
                  return 0;
         endif
         
         npc.saveonexit := 0;
         
         SetDestinations( npc, location_elem );
         
         SetObjProperty( npc, "GuardKill", 1 );
         SetObjProperty( npc, "GuardIgnore", 1 );
         SetObjProperty( npc, "IsRaider", 1 );
         
         if( !GetObjProperty( chest, "RaidersAttack" ))
                  SetObjProperty( npc, "#Peacemaking", ReadGameClock() + 300 );
         endif

         return npc.serial;
endfunction

function SetDestinations( npc, byref location_elem )

         var way_points := GetConfigStringArray( location_elem, "Waypoint" );
         
         var destination := array{};
         foreach coords in way_points
                  destination.Append( coords );
                  SleepMS(5);
         endforeach
             
         SetObjProperty( npc, "WayPoints", destination );
         
         var script := start_script( ":townRaid:raiders/move", npc );
         if( script.errortext )
                  PrintTextAbove( npc, "Error starting script <:townRaid:raiders/move> -->"+script.errortext );
         endif
             
         return 1;
endfunction

function KillChildren( children )
                           
         foreach serial in children
                  var creature := SystemFindObjectBySerial( serial );
                  SetObjProperty( creature, "GuardKill", 1 );
                  creature.Kill();
                  
                  SleepMS(5);
         endforeach

         return 1;
endfunction
