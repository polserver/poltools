/*
        Maintened by *Edwards

        2009-10-10
        
        Last Modifications 2009-10-26
             * Added NPC_SetupProtections();
*/
use uo;
use os;
use util;
use cfgfile;

include ":brainAI:npcUtil";
include ":brainAI:npcError";
include ":equipment:protections";
include ":itemid:itemID";
include ":mounts:mounts";

/////////////////////////////////////////////////////////////////
// * Function List *
//
// NPC_SetupAmmo(mobile)
// NPC_SetupBackPack(mobile)
// NPC_SetupName(mobile, name_elem:=0)
// NPC_SetupRandomEquipment(mobile, elem_name:=0, flags:=EQUIP_NOGENDER)
// NPC_SetupProtections( npc );
//
/////////////////////////////////////////////////////////////////

// * Global Variables *
//
// Constants for NPC_SetupRandomEquipment()
CONST EQUIP_NOGENDER	:= 0x0;	// Items suited for both genders
CONST EQUIP_MALE	:= 0x1; // Use male-only items (do any exist?)
CONST EQUIP_FEMALE	:= 0x2; // Use female-only items
CONST EQUIP_BOTHGENDER	:= 0x3; // Cross-dresser

function NPC_SetupBackPack( mobile )

	if( !mobile.backpack )
		var backpack := CreateItemAtLocation( mobile.x, mobile.y, mobile.z, "BackPack", 1, mobile.realm );
		var result := EquipItem( mobile, backpack );
		if( !result )
			DestroyItem( backpack );
			return result;
		else
			return backpack;
		endif
	endif

	return mobile.backpack;
endfunction

function NPC_SetupAmmo( mobile )

	var npc_elem := NPC_GetNPCConfig( mobile );
 
	if( npc_elem.MissileWeapon )
		var weapon := CreateItemAtLocation( mobile.x, mobile.y, mobile.z, npc_elem.MissileWeapon, 1, mobile.realm );
		if( !EquipItem( mobile, weapon ))
			DestroyItem( weapon );
                        return weapon;
		endif
  
                weapon.newbie := 1;
                
	elseif( npc_elem.AmmoType )
		if( CInt( npc_elem.AmmoAmount ) > 0 )
			var ammo := CreateItemInBackpack( mobile, npc_elem.AmmoType, CInt( npc_elem.AmmoAmount ));
			if( !ammo )
				return ammo;
			endif
   
			ammo.newbie := 1;
		endif
	endif

	return 1;
endfunction

function NPC_SetupRandomEquipment( mobile, elem_name:=0, flags:=EQUIP_NOGENDER )

	if( !elem_name )
		var npc_elem := NPC_GetNPCConfig( mobile );
		elem_name := npc_elem.EquipTemplate;
	endif
 
	var index_cfg := ReadConfigFile( ":equipment:equipIndex" );
	if( index_cfg.errortext )
		return error{"errortext":="Could not open equipIndex.cfg -> "+index_cfg.errortext};
        endif
 
	var group_cfg := ReadConfigFile( ":equipment:equipGroups" );
	if( group_cfg.errortext )
		return error{"errortext":="Could not open equipGroups.cfg -> "+group_cfg.errortext};
        endif

	var index_elem := index_cfg[elem_Name];
	if( !index_elem )
		return error{"errortext":="equipIndex.cfg elem ["+index_elem+"] does not exist."};
	endif
 
	foreach entry in ( GetConfigStringDictionary( index_elem, "EquipGroup" ))
		SleepMS(5);
  
		if( RandomDiceRoll( "1d100+1" ) <= CInt( entry ))
  
			var group_elem := group_cfg[CStr( _entry_iter )];
			if( !group_elem )
				PrintTextAbove( mobile, "equipGroups.cfg elem ["+_entry_iter+"] does not exist." );
				continue;
			endif

			var item_list := array{};
			if( !flags )
				var temp := GetConfigStringArray( group_elem, "Item" );
				if( temp )
					item_list += temp;
				endif
			elseif( flags & EQUIP_MALE )
				var temp := GetConfigStringArray( group_elem, "Male" );
				if( temp )
					item_list += temp;
				endif
			elseif( flags & EQUIP_FEMALE )
				var temp := GetConfigStringArray( group_elem, "Female" );
				if( temp )
					item_list += temp;
				endif
			endif

			var item := CreateItemAtLocation( mobile.x, mobile.y, mobile.z, item_list.RandomEntry(), 1, mobile.realm );
                	if( item.errortext )
                		PrintTextAbove( mobile, "Error could not create <"+_entry_iter+"> -->"+item.errortext );
                		continue;
                	elseif( !EquipItem( mobile, item ))
				PrintTextAbove( mobile, "Unable to equip - "+item.desc+" - destroying." );
				DestroyItem( item );
                        	continue;
                	endif

                        SetObjProperty( item, "NoLoot", 1 );
                        IDItem( mobile, item );
		endif
	endforeach

	foreach entry in ( GetConfigStringDictionary( index_elem, "EquipItem" ))
		SleepMS(5);
                
		var item := CreateItemAtLocation( mobile.x, mobile.y, mobile.z, _entry_iter, 1, mobile.realm );
                if( item.errortext )
                	PrintTextAbove( mobile, "Error could not create <"+_entry_iter+"> -->"+item.errortext );
                	continue;
                elseif( !EquipItem( mobile, item ))
			PrintTextAbove( mobile, "Unable to equip - "+item.desc+" - destroying." );
			DestroyItem( item );
                        continue;
                endif
                
                item.color := CInt( entry );

                SetObjProperty( item, "NoLoot", 1 );
                IDItem( mobile, item );
	endforeach

	foreach entry in ( GetConfigStringDictionary( index_elem, "EquipMount" ))
		SleepMS(5);

		var item := MP_Mount( mobile, _entry_iter );
                if( item.errortext )
                	PrintTextAbove( mobile, "Error could not mount <"+_entry_iter+"> -->"+item.errortext );
                	continue;
                endif

                item.color := CInt( entry );

                SetObjProperty( item, "NoLoot", 1 );
	endforeach

	return 1;
endfunction

function NPC_SetupName( mobile, name_elem:=0 )

	if( !mobile.name["<random>"] )
		return 1;
	elseif( !name_elem )
		var npc_elem := NPC_GetNPCConfig( mobile );
		name_elem := npc_elem.NameTemplate;
		if( name_elem.errortext )
			return name_elem;
		endif
	endif

	var npc_names := ReadConfigFile( ":brainAI:names" );
	if( !npc_names[name_elem] )
		return error{"errortext" := "There is no elem ["+name_elem+"] in NAMES.CFG"};
	endif

	var name_list := GetConfigStringArray( npc_names[name_elem], "Name" );
	if ( name_list.Size() < 1 )
		return error{"errortext":="No names were found in the ["+name_elem+"] elem for names.cfg"};
	endif

	var my_name := mobile.name;
	while( my_name["<random>"] )
		var replacement := name_list[RandomInt( name_list.Size() )+1];
		my_name["<random>"] := replacement;
		SleepMS(2);
	endwhile

	SetName( mobile, my_name );

	return my_name;
endfunction

function NPC_SetupProtections( npc )

        var possibles := GetProtections(),
            protections := GetObjProtections( npc );
        
        foreach protection in possibles
                var level := GetObjProperty( npc, protection );
                if( level )
                        protections[protection] := level;
                        EraseObjProperty( npc, protection );
                endif
                
                SleepMS(5);
        endforeach
        
        SetMobileProtections( npc, protections );
        
        return 1;
endfunction
