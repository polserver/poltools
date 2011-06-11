/*
        Maintened by *Edwards

        2009-10-10
*/
use uo;
use polsys;

include ":attributes:attributes";
include ":brainAI:npcInit";
include ":brainAI:npcWatchList";
include ":containers:canRemove";
include ":detecthidden:detectHidden";

function NPC_GetFleeLevel( npc, flee_percent )

	return CInt( CDbl( AP_GetVitalMaximumValue( npc, "Hits" )) * ( CDbl( flee_percent ) / 100.0 ));
endfunction

function NPC_WeaponRanges( npc, byref min_range, byref max_range )

	// To support having distance for archery weapons or being close up for a melee weapon.
	min_range := CInt( CS_GetWeaponInfo( npc.weapon, "MinRange" ));
	max_range := CInt( CS_GetWeaponInfo( npc.weapon, "MaxRange" ));
 
	if( !max_range )
		max_range := 1;
	endif

	return 1;
endfunction

function DoneFighting( npc, opponent, byref dist, byref settings )

	if( !opponent )
		return 1;
        elseif( opponent.dead )
		return 1;
        elseif( opponent.concealed )
                return 1;
	elseif( dist > 30 )
		return 1;
	elseif( GetObjProperty( npc, "#Peacemaking" ) > ReadGameClock() )
		return 1;
        elseif( opponent.hidden )
                if( !DetectHidden( npc, opponent ))
                        pathfind_ticker += 1;
                else
                        pathfind_ticker := 0;
                endif
        elseif( !CheckForPathLOS( npc, opponent, settings ))
                pathfind_ticker += 1;
        else
                pathfind_ticker := 0;
        endif

        if( pathfind_ticker >= 60 )
                return 1;
        endif

	return 0;
endfunction

function CheckForPathLOS( npc, destination, settings )

	if( !CheckLosAt( npc, destination.x, destination.y, destination.z ))
		var pathfind_range := CInt( settings["AreaSize"] );
        	if( pathfind_range < 5 )
                	pathfind_range := 5;
        	endif

        	var flags := FP_IGNORE_MOBILES;
        	if( settings["OpenDoors"] )
                	flags += FP_IGNORE_DOORS;
        	endif

                if( !GetNewPath( npc, destination.x, destination.y, destination.z, pathfind_range, flags ))
			return 0;
                endif
	endif
 
        return 1;
endfunction

function ShouldFlee( npc, opponent, byref settings )

        //While fixing the flee mode.
        return 0;

        if( opponent.IsA( POLCLASS_NPC ))
                return 0;
        endif

        var flee_level := NPC_GetFleeLevel( npc, settings["FleeLevel"] );
	if( flee_level > AP_GetVital( npc, "Hits" ))
		return 1;
	endif

	return 0;
endfunction

function Virtual_Looting( npc, corpse, byref settings )

        var npc_cfg := NPC_GetNPCConfig( npc.npctemplate );

	if( !settings["IsLooter"] )
                return 0;
        elseif( GetObjProperty( npc, "#LastLooting" ) > ReadGameClock() )
                return 0;
        elseif( npc_cfg.NoCorpse || npc_cfg.NoCorpse || GetObjProperty( npc, "SummonedBy" ))
                return 0;
        endif
        
        var dist := Distance( npc, corpse );
        AI_Move( npc, corpse.x, corpse.y, corpse.z, NEMOVE_TOWARD, NEMOVE_WALK, dist+1 );
        
        //List the items inside the corpse, backpack, .. container
        var InContainer := array{};
        foreach thing in EnumerateItemsInContainer( corpse )
                if( FindAllowedItems( thing ))
                        InContainer.Append( thing );
                endif
                
                SleepMS(5);
        endforeach
        
        var stolen_item := InContainer[RandomInt( InContainer.Size() )+1],
	    npc_backpack := NPC_SetupBackPack( npc );
            
        var move_item := MoveItemToContainer( stolen_item, npc_backpack );
        if( move_item.errortext )
                PrintTextAbove( npc, "Error looting items -->"+move_item.errortext );
                return 0;
        endif

        SetObjProperty( npc, "#LastLooting", ReadGameClock() + 20 );
        AddLooter( npc, corpse, stolen_item );
        AI_Speak( npc, "*Yoink*", SPEAK_TEXTTYPE_YELL, SPEAK_DOEVENT_ENABLE );

        return 1;
endfunction

function FindAllowedItems( item )

	if( !item.movable )
		return 0;
	elseif( item.newbie )
		return 0;
	elseif( item.IsA( POLCLASS_CONTAINER ))
		return 0;
	elseif( IsStackable( item, item ))
		return 0;
	elseif( GetObjProperty( item, "CantSteal" ))
		return 0;
	endif

	return 1;
endfunction

function GetNewPath( npc, x, y, z, pathfind_range, flags )

	var path := FindPath( npc.x, npc.y, npc.z, x, y, z, npc.realm, flags, pathfind_range );
        if( !path )
                //Automatic Anti-Walls if NPC has MAGERY
                if( AP_GetSkill( npc, MAGERY ))
                        var search_field := ListItemsNearLocation( npc.x, npc.y, npc.z, 5, npc.realm );
                        foreach item in search_field
                                if( GetObjProperty( item, "SummonedField" ))
                                        //Starts spell Dispel Field
                                        var script := Start_Script( ":magery:spellStarter", array{ npc, 34, 0, item } );
	                                if( script.errortext )
		                                PrintTextAbove( npc, script.errortext );
                                        endif
                                        break;
                                endif

                                SleepMS(5);
                        endforeach
                endif

                //Could not find any path to the destination, stop moving...
                return 0;
        endif

        var move_x := CInt( path[1].x ),
            mobe_y := CInt( path[1].y ),
            mobe_z := CInt( path[1].z );

        var search_doors := ListItemsAtLocation( move_x, mobe_y, mobe_z, npc.realm );
        if( search_doors.Size() )
                foreach item in search_doors
                        if( item.IsDoor() )
                                if( item.locked )
                                        return 0;
                                elseif( item.IsOpen() )
                                        break;
                                endif

                                UseItem( item, npc );
                                break;
                        else
                                continue;
                        endif

                        SleepMS(5);
                endforeach
        endif

        return array{move_x, mobe_y};
endfunction

function NPC_SetAnchorInfos( npc, gsettings )

        var starting_point := GetObjProperty( npc, "StartingPoint" );
        if( !starting_point )
                var location := struct;
                    location.+x := npc.x;
                    location.+y := npc.y;

                starting_point := location;
                SetObjProperty( npc, "StartingPoint", location );
        endif

        AI_SetAnchor( npc, starting_point.x, starting_point.y, CInt( gsettings["AreaSize"] ), 80 );
        
        return 1;
endfunction

function NPC_SetuptCombat( npc )

        var battle_cry := GetObjProperty( npc, "BattleCryWord" );
        if( battle_cry )
                AI_Speak( npc, battle_cry.RandomEntry() );
        endif

	if( npc.hidden )
                npc.hidden := 0;
		PrintTextAbove( npc, "*Springs from hiding!*", 3, 55 );
	endif
 
        return 1;
endfunction
