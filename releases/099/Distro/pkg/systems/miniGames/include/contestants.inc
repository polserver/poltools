/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-04

        Thanks to ZuluHotel and last maintened Montuz for the wonderfull idea
*/
use os;
use os;
use cfgfile;

include ":containers:storageAreas";
include "include/client";
include ":mounts:mounts";

const MIN_PLAYER := 2;

function SelectTeams( queued_Players )

        var data_elem := DFFindElement( g_datafile, game ),
    	    red_Contestants := array{},
    	    blue_Contestants := array{},
    	    contestants := array{};

    	var qTotal := queued_Players.Size(),
    	    TSize := ( qTotal / MIN_PLAYER );

    	var largest_game := data_elem.GetProp( "LargestMatch" );
    	if( !largest_game )
    		largest_game := MIN_PLAYER;
    	endif

    	if( TSize > largest_game )
    		data_elem.SetProp( "LargestMatch", TSize );
    	endif

    	var iR := 0;
    	repeat( iR := iR + 1 );

    		red_Contestants.Append( queued_Players[iR] );
    		contestants.Append( queued_Players[iR] );

    		var red_Player := SystemFind( queued_Players[iR] );
    		if( red_Player )
    	    		SetObjProperty( red_Player, ID_INGAME, ID_RED );
    		endif

    		queued_Players.Erase( iR );

    		SleepMS( 10 );
    	until( iR >= TSize );

    	var iB := 0;
    	repeat( iB := iB + 1 );

    		blue_Contestants.Append( queued_Players[iB] );
    		contestants.Append( queued_Players[iB] );

    		var blue_Player := SystemFind( queued_Players[iB] );
    		if( blue_Player )
    	    		SetObjProperty( blue_Player, ID_INGAME, ID_BLUE );
    		endif

    		SleepMS( 10 );
    	until( iB >= TSize );

	data_elem.SetProp( "Red_Contestants", red_Contestants );
    	data_elem.SetProp( "Blue_Contestants", blue_Contestants );
    	data_elem.SetProp( "Contestants", contestants );
    	data_elem.SetProp( "MatchInProgress", 1 );

    	ReadyContestants( contestants );

    	return 1;
endfunction

function ReadyContestants( allContestants )

    	foreach contestant in allContestants
     
            	var cntstnt := SystemFind( contestant );
                if( cntstnt.errortext )
                        SysLog( "Error SystemFind -->"+cntstnt.errortext );
                endif
             
                MP_Unmount( cntstnt );
    		CheckContestant( cntstnt );
    		DressConstestant( cntstnt );
    		SendContestantToBase( cntstnt );

                SleepMS(5);
    	endforeach

        return 1;
endfunction

function RemoveContestants( allContestants )

    	foreach contestant in allContestants
     
            	var cntstnt := SystemFind( contestant );
                if( cntstnt.errortext )
                        SysLog( "Error SystemFindCW -->"+cntstnt.errortext );
                endif
                
            	RemoveContestant( cntstnt );

                SleepMS(5);
    	endforeach

        return 1;
endfunction

function CheckContestants( group )

   	if( group.Size() == 0 || !group || group.Size() < 0 )
   		return 0;
   	else
   		return 1;
   	endif
endfunction

function DressConstestant( who )

    	var mage := GetObjProperty( who, "IsMage" ),
    	    warrior := GetObjProperty( who, "IsWarrior" ),
    	    ranger := GetObjProperty( who, "IsRanger" ),
    	    bard := GetObjProperty( who, "IsBard" ),
    	    team := GetObjProperty( who, ID_INGAME );

        var color;
	case( team )
    		ID_RED:  color := ID_HUE_RED;
    		ID_BLUE: color := ID_HUE_BLUE;
    	endcase

	if( mage )
    		//DressCWMage( who, CW_Color );
	elseif( warrior )
    		//DressCWWarrior( who, CW_Color );
	elseif( ranger )
    		//DressCWRanger( who, CW_Color );
	elseif( bard )
    		//DressCWBard( who, CW_Color );
	else
    		//DressCWPP( who, CW_Color );
	endif

        return 1;
endfunction

function SendContestantToBase( who )

    	var cfg_Arenas := ReadConfigFile( ":miniGames:arena" );
    	if( cfg_Arenas.errortext )
                SendSysMessage( who, "Error reading config <:miniGames:arena> -->"+cfg_Arenas.errortext );
    		return 0;
    	endif

        var team := GetObjProperty( who, ID_INGAME );
    	if( team.errortext )
                SendSysMessage( who, "Error reading property <"+ID_INGAME+"> -->"+team.errortext );
    		return 0;
    	endif
     
        ResurrectPlayer( who );

        var where;
	if( team == ID_RED )
    		var red_Player_StartLoc := cfg_Arenas[1].Red_Start_Loc;
    	    	if( red_Player_StartLoc.errortext )
                    	SendSysMessage( who, "Error reading elem for <red_Player_StartLoc> -->"+red_Player_StartLoc.errortext );
    		    	return 0;
    	    	endif

    		where := SplitWords( red_Player_StartLoc );
    	elseif( team == ID_BLUE )
   		var blue_Player_StartLoc := cfg_Arenas[1].Blue_Start_Loc;
    		if( blue_Player_StartLoc.errortext )
                    	SendSysMessage( who, "Error reading elem for <blue_Player_StartLoc> -->"+blue_Player_StartLoc.errortext );
    		    	return 0;
    		endif

    		where := SplitWords( blue_Player_StartLoc );
        endif
     
	var moved := MoveObjectToLocation( who, CInt( where[1] ), CInt( where[2] ), CInt( where[3] ), CStr( where[4] ), MOVEOBJECT_FORCELOCATION );
        if( !moved )
                SendSysMessage( who, "Error moving <"+who.name+"> to StartLoc -->"+moved.errortext );
    		return 0;
        endif

        return 0;
endfunction

function RemoveContestant( who )
     
    	var cfg_Arenas := ReadConfigFile( ":miniGames:arena" );
    	if( cfg_Arenas.errortext )
                SendSysMessage( who, "Error reading config <:miniGames:arena> -->"+cfg_Arenas.errortext );
    		return 0;
    	endif
     
        var data_elem := DFFindElement( g_datafile, game ),
            red_Contestants := data_elem.GetProp( "Red_Contestants" ),
            blue_Contestants := data_elem.GetProp( "Blue_Contestants" ),
            contestants := data_elem.GetProp( "Contestants" );

        var check := GetObjProperty( who, ID_INGAME );
	if( check )
    		var hair := GetEquipmentByLayer( who, LAYER_HAIR ),
    		    beard := GetEquipmentByLayer( who, LAYER_BEARD );

		foreach worn in ListEquippedItems( who )
    			if( worn && worn != hair && worn != beard && worn != who.backpack )
      				if( GetObjProperty( worn, "MiniGame" ))
      					DestroyEquipment( who, worn );
      				endif
    			endif
       
                        SleepMS(5);
    		endforeach

		foreach item in EnumerateItemsInContainer( who.backpack );
    			if( GetObjProperty( item, "MiniGame" ))
    				DestroyEquipment( who, item );
    			endif

                        SleepMS(5);
    		endforeach

		RemoveFromArray( red_Contestants, who.serial );
    		RemoveFromArray( blue_Contestants, who.serial );
    		RemoveFromArray( contestants, who.serial );

    		data_elem.SetProp( "Red_Contestants", red_Contestants );
    		data_elem.SetProp( "Blue_Contestants", blue_Contestants );
   		data_elem.SetProp( "Contestants", contestants );

    		var main_StartLoc := cfg_Arenas[1].ArenaHome,
    		    where := SplitWords( main_StartLoc );
      
	        var moved := MoveObjectToLocation( who, CInt( where[1] ), CInt( where[2] ), CInt( where[3] ), CStr( where[4] ), MOVEOBJECT_FORCELOCATION );
                if( !moved )
                        SendSysMessage( who, "Error moving <"+who.name+"> to red_Player_StartLoc -->"+moved.errortext );
    		        return 0;
                endif
                
    		EraseObjProperty( who, ID_INGAME );
    	endif

        return 1;
endfunction

function CheckContestant( who )

    	var hair := GetEquipmentByLayer( who, LAYER_HAIR ),
    	    beard := GetEquipmentByLayer( who, LAYER_BEARD );

        var xBankbox := CP_GetStorageContainerForMobile( who, CP_DEFAULT_AREA, CP_CREATE );
        if( !xBankbox )
                return 0;
        endif

        if( !who.dead )
		var in_cont := CreateItemInContainer( xBankbox, 0xE76 ); //bag
                if( in_cont.errortext )
                        SendSysMessage( who, "Error creating bag into bankbox of "+who.name+" -->"+in_cont.errortext );
                        return 0;
                endif

		foreach item in EnumerateItemsInContainer( who.backpack );
    			var move := MoveItemToContainer( item, in_cont );
                	if( move.errortext )
                        	SendSysMessage( who, "Error moving <"+item.desc+"> into bankbox of "+who.name+" -->"+move.errortext );
                        	return 0;
                        endif
                        
    			SleepMS(5);
    		endforeach

		foreach worn in ListEquippedItems( who )
    			if( worn && worn != hair && worn != beard && worn != who.backpack )
    				var move := MoveItemToContainer( worn, in_cont );
                		if( move.errortext )
                        		SendSysMessage( who, "Error moving <"+worn.desc+"> into bankbox of "+who.name+" -->"+move.errortext );
                        	endif
    				SleepMS(5);
    			endif
    		endforeach

    		SendSysMessage( who, "Everything you had on you was moved into your bankbox safely.", 3, 80 );
	endif
 
    	return 1;
endfunction

function SystemFind( xSerial )

    	var what := SystemFindObjectBySerial( xSerial[1] );
    	if( what.errortext )
                SysLog( "Error finding object -->"+what.errortext );
    		return 0;
    	endif
     
    	return what;
endfunction

function RewardTeamContestants( team )

	foreach contestant in team
    		var cntstnt := SystemFind( contestant );

    		var xBankbox := CP_GetStorageContainerForMobile( cntstnt, CP_DEFAULT_AREA, CP_CREATE );
                if( !xBankbox )
                        return 0;
                endif

    		SendSysMessage( cntstnt, "Congrats, You've earned some tokens!", 3, 80 );

    		SleepMS(5);
	endforeach

        return 1;
endfunction
