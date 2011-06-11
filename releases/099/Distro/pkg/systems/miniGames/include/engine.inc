/*
        Created by *Edwards
        
        2009-10-03
        
        Thanks to ZuluHotel and last maintened Montuz for the wonderfull idea
*/
use uo;
use os;

include ":miniGames:contestants";
include ":miniGames:equipment";
include ":miniGames:settings";
include "include/arrays";

const ID_RED := "Red";
const ID_BLUE := "Blue";

const ID_HUE_RED := 1198;
const ID_HUE_BLUE := 1170;

var game;
var g_datafile := DFOpenDataFile( ":miniGames:Main", DF_CREATE );

function EndGame( allContestants )

    	BroadcastToGamePlayers( "Ending match." );
     
    	Sleep(5);

    	RemoveContestants( AllContestants );
     
    	EraseGlobalProperty( "Red_Contestants" );
    	EraseGlobalProperty( "Blue_Contestants" );
    	EraseGlobalProperty( "Contestants" );
    	EraseGlobalProperty( "MatchInProgress" );
     
    	BroadcastToGamePlayers( "Match ended." );
     
        return 1;
endfunction

function BroadcastToGamePlayers( message )

    	var contestants := GetGlobalProperty( "Contestants" );
        
	foreach contestant in contestants
    		var cntstnt := SystemFind( contestant );
      
    		SendSysMessage( cntstnt, "[System] "+CStr( message ), 3, 80 );
      
    		SleepMS(5);
    	endforeach
    
    	return 1;
endfunction

function BroadcastGameToPlayers( message )
        
    	foreach contestant in EnumerateOnlineCharacters()
    		if( !GetObjProperty( contestant, "MiniGameChat" ))
    			SendSysMessage( contestant, "[System] "+CStr( message ), 3, 80 );
    		endif
      
                SleepMS(5);
    	endforeach
     
    	return 1;
endfunction

function ResurrectPlayer( who )

    	if( who.dead )
                Resurrect( who );
    		AP_RefreshVitals( who );

		var deathRobe := GetEquipmentByLayer( who, LAYER_ROBE );
      		if( deathRobe && !GetObjProperty( deathRobe, "Cursed" ))
      			var destroy := DestroyItem( deathRobe );
                        if( destroy.errortext )
                                 return 0;
                        endif
                endif
        endif

        return 1;
endfunction
