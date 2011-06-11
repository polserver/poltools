/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2010-11-11
*/
use uo;
use os;

include "include/client";
include "include/itemUtil";
include "include/sysEvent";
include "include/yesNo";
include ":attributes:attributes";
include ":mounts:mounts";
include ":reputation:reputation";
include ":rituals:settings";
include ":spells:spells";

var magic_circle := array{},
    captor;

function PerformRitual( mobile, spell_id )

        SetObjProperty( mobile, "#Casting", GetPid() );
        
	if( !CheckRitualEquip( mobile, spell_id ))
		return 0;
        endif
        
        if( !CheckRitualCircle( mobile ))
		return 0;
        elseif( !DrawRitualCircle( mobile, spell_id ))
                DestroyDone( mobile, 1 );
		return 0;
	endif
 
        var item := GetRitualItem( mobile, spell_id );
	if( !item )
                if( !UndoRitualCircle( mobile ))
                	DestroyDone( mobile, 1 );
                endif
		return 0;
	elseif( !ProcessRitual( mobile, spell_id, item ))
                DestroyDone( mobile, 1, item );
		return 0;
	endif
 
        DestroyDone( mobile );
 
	return item;
endfunction

function CheckRitualEquip( mobile, spell_id )

        if( mobile.cmdlevel > 3 )
                return 1;
        endif
        
        if( spell_id != 135 )
		var staff := GetEquipmentByLayer( mobile, LAYER_HAND2 ),
	   	    robe  := GetEquipmentByLayer( mobile, LAYER_ROBE );
     
        	if( !robe.IsRitualRobe() )
			SendSysMessage( mobile, "You must wear a consecrated ritual robe to perform a ritual." );
			return 0;
		elseif( !staff.IsOakStaff() )
			SendSysMessage( mobile, "You must wear a consecrated young oak staff to perform a ritual." );
			return 0;
		elseif( GetObjProperty( robe, "Consecrated" ) != mobile.serial )
			SendSysMessage( mobile, "Your ritual robe is not consecrated." );
			return 0;
		elseif( GetObjProperty( staff, "Consecrated" ) != mobile.serial )
			SendSysMessage( mobile, "Your ritual oak staff isn't consecrated." );
			return 0;
		endif
        endif
         
	if( MP_GetMount( mobile ))
		SendSysMessage( mobile, "You can't perform a ritual while mounted." );
		return 0;
	endif

	return 1;
endfunction

function CheckRitualCircle( mobile )

	//Check if the caster want to use his personnal circle
        var existing_circle;
	foreach thing in ListItemsAtLocation( mobile.x, mobile.y, mobile.z, mobile.realm );
		if( thing.IsRitualRune() )
			existing_circle := thing;
			break;
		endif
  
                SleepMS(5);
	endforeach
 
        if( existing_circle )
		SendSysMessage( mobile, "You can't perform a ritual by using an existing magic circle." );
                return 0;
        elseif( Len( ListItemsNearLocation( mobile.x, mobile.y+5, mobile.z, 6, mobile.realm )) && mobile.cmdlevel < 4 )
		SendSysMessage( mobile, "You don't have the necessary space to draw a magic circle here." );
		return 0;
	endif

	return 1;
endfunction

function DrawRitualCircle( mobile, spell_id )

        var elem_drawing := RITUAL_GetSettingsCfgElem( "DrawCircle" ),
            elem_settings := RITUAL_GetSettingsCfgElem( "General" ),
            speech_delay := elem_settings.DelaySpeech,
            base_light := elem_settings.LightLevel,
            base_hp := AP_GetVital( mobile, HITS ),
            current_speech := 1;

	var x := mobile.x,
	    y := mobile.y,
	    z := mobile.z,
            realm := mobile.realm,
	    center_y := y+5;

        CreateSpeechCaptor( mobile, x, center_y, z, spell_id );

	SendSysMessage( mobile, "You can start to draw the circle" );
	while( mobile.connected )
                
                var speech := GetConfigString( elem_drawing, "Speech"+current_speech );
                if( !speech )
                	break;
                elseif( !captor )
                	return 0;
                endif

		var text := GetConfigString( elem_drawing, "Print"+current_speech ),
		    x_mod := GetConfigInt( elem_drawing, "Xmod"+current_speech ),
		    y_mod := GetConfigInt( elem_drawing, "Ymod"+current_speech ),
		    facing := GetConfigInt( elem_drawing, "Facing"+current_speech ),
		    appear := GetConfigInt( elem_drawing, "Appear"+current_speech );

                SendSysMessage( mobile, "Ritual: "+speech, 3, 553 );

                var starting_delay := ReadGameClock();
		while( mobile.connected )

                	if( !captor )
                		return 0;
                	elseif( ReadGameClock() - starting_delay > speech_delay )
				return 0;
			elseif( AP_GetVital( mobile, HITS ) < base_hp )
				return 0;
			endif

		        var event := os::Wait_For_Event(1);
			if( event && event.source == mobile )
				if( event.type == SYSEVENT_SPEECH )
					if( event.text[speech] )
                                                break;
                                        else
						return 0;
                                        endif
				endif
			endif
		endwhile

                Sleep(1);

                if( mobile.x != ( x+x_mod ) || mobile.y != ( y+y_mod ))
			return 0;
                elseif( facing != mobile.facing )
			return 0;
                endif

		if( text )
			PrintTextAbove( mobile, text );
		endif
		if( appear )
			var itemx_mod := GetConfigInt( elem_drawing, "Xmod"+( current_speech+1 ));
			var itemy_mod := GetConfigInt( elem_drawing, "Ymod"+( current_speech+1 ));
   
			magic_circle.Append( CreateItemAtLocation( x+itemx_mod, y+itemy_mod, z, appear, 1, realm ));
		endif
  
		ChangeLightLevel( mobile, CInt( base_light*current_speech ));

		current_speech += 1;
	endwhile

	CreateMagicCircle( x, y, z, realm );
	CreateCandles( mobile, x, y, z, realm );
	CircleUnDecayable( mobile );

	return 1;
endfunction

function UndoRitualCircle( mobile, item:=0 )

        var north_rune := GetRitualRuneNorth( mobile );
        if( !north_rune )
		SendSysMessage( mobile, "Your circle was corrupted!" );
                DestroyDone( mobile, 1, item );
                return 0;
        endif
            
        var elem_drawing := RITUAL_GetSettingsCfgElem( "UndoCircle" ),
            elem_settings := RITUAL_GetSettingsCfgElem( "General" ),
            speech_delay := elem_settings.DelaySpeech,
            base_hp := AP_GetVital( mobile, HITS ),
            current_speech := 1,
            x := north_rune.x,
	    y := north_rune.y;;

	SendSysMessage( mobile, "You must now undo the magic circle to successfully finish the ritual." );
	while( mobile.connected )

                var speech := GetConfigString( elem_drawing, "Speech"+current_speech );
                if( !speech )
                	break;
                elseif( !captor )
                	return 0;
                endif

		var text := GetConfigString( elem_drawing, "Print"+current_speech ),
		    x_mod := GetConfigInt( elem_drawing, "Xmod"+current_speech ),
		    y_mod := GetConfigInt( elem_drawing, "Ymod"+current_speech ),
		    facing := GetConfigInt( elem_drawing, "Facing"+current_speech );

                SendSysMessage( mobile, "Ritual: "+speech, 3, 553 );

                var starting_delay := ReadGameClock();
		while( mobile.connected )

                	if( !captor )
                		return 0;
                	elseif( ReadGameClock() - starting_delay > speech_delay )
                                DestroyDone( mobile, 1, item );
				return 0;
			elseif( AP_GetVital( mobile, HITS ) < base_hp )
                                DestroyDone( mobile, 1, item );
				return 0;
			endif

		        var event := os::Wait_For_Event(1);
			if( event && event.source == mobile )
				if( event.type == SYSEVENT_SPEECH )
					if( event.text[speech] )
                                                break;
                                        else
                                                DestroyDone( mobile, 1, item );
						return 0;
                                        endif
				endif
			endif
		endwhile

                Sleep(1);

                if( mobile.x != ( x+x_mod ) || mobile.y != ( y+y_mod ))
                        DestroyDone( mobile, 1, item );
			return 0;
                elseif( facing != mobile.facing )
                        DestroyDone( mobile, 1, item );
			return 0;
                endif

		if( text )
			PrintTextAbove( mobile, text );
		endif

		current_speech += 1;
	endwhile
 
        Sleep(2);
        
        DestroyDone( mobile );

	return 1;
endfunction

function CreateMagicCircle( x, y, z, realm )

	magic_circle.Append( CreateItemAtLocation( x+2, y+7, z, 0x1cf1, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+1, y+7, z, 0x1cf2, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x, y+7, z, 0x1cf3, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x-1, y+7, z, 0x1cf4, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x-2, y+7, z, 0x1cf5, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x-2, y+6, z, 0x1cf6, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x-2, y+5, z, 0x1cf7, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x-2, y+4, z, 0x1cf8, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x-2, y+3, z, 0x1cf9, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x-1, y+3, z, 0x1cfa, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x-1, y+2, z, 0x1cfb, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x, y+2, z, 0x1cfc, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+1, y+2, z, 0x1cfd, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+2, y+2, z, 0x1cfe, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+3, y+2, z, 0x1cff, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+3, y+3, z, 0x1d00, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+3, y+4, z, 0x1d01, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+3, y+5, z, 0x1d02, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+3, y+6, z, 0x1d03, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+2, y+6, z, 0x1d04, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+1, y+6, z, 0x1d05, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x, y+6, z, 0x1d06, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x-1, y+6, z, 0x1d07, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x-1, y+5, z, 0x1d08, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x-1, y+4, z, 0x1d09, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x, y+3, z, 0x1d0a, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+1, y+3, z, 0x1d0b, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+2, y+3, z, 0x1d0c, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+2, y+4, z, 0x1d0d, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+2, y+5, z, 0x1d0e, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+1, y+5, z, 0x1d0f, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x, y+5, z, 0x1d10, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x, y+4, z, 0x1d11, 1, realm ));
	magic_circle.Append( CreateItemAtLocation( x+1, y+4, z, 0x1d12, 1, realm ));

        return 1;
endfunction

function CreateCandles( mobile, x, y, z, realm )

	PerformAction( mobile, ANIM_CAST_AREA );
        Sleep(1);
	magic_circle.Append( CreateItemAtLocation(  x, y+2, z, 0x1854, 1, realm ));
	SetName( magic_circle[ Len( magic_circle )], "magic candle" );
	PlaySoundEffect( magic_circle[Len( magic_circle )], 0x022b );

	PerformAction( mobile, ANIM_CAST_AREA );
        Sleep(1);
	magic_circle.Append( CreateItemAtLocation( x+3, y+3, z, 0x1854, 1, realm ));
	SetName( magic_circle[ Len( magic_circle )], "magic candle" );
	PlaySoundEffect( magic_circle[Len( magic_circle )], 0x022b );

	PerformAction( mobile, ANIM_CAST_AREA );
        Sleep(1);
	magic_circle.Append( CreateItemAtLocation( x+2, y+6, z, 0x1854, 1, realm ));
	SetName( magic_circle[ Len( magic_circle )], "magic candle" );
	PlaySoundEffect( magic_circle[Len( magic_circle )], 0x022b );

	PerformAction( mobile, ANIM_CAST_AREA );
        Sleep(1);
	magic_circle.Append( CreateItemAtLocation( x-1, y+7, z, 0x1854, 1, realm ));
	SetName( magic_circle[ Len( magic_circle )], "magic candle" );
	PlaySoundEffect( magic_circle[Len( magic_circle )], 0x022b );

	PerformAction( mobile, ANIM_CAST_AREA );
        Sleep(1);
	magic_circle.Append( CreateItemAtLocation( x-2, y+4, z, 0x1854, 1, realm ));
	SetName( magic_circle[ Len( magic_circle )], "magic candle" );
	PlaySoundEffect( magic_circle[Len( magic_circle )], 0x022b );
 
	Sleep(1);

        return 1;
endfunction

function GetRitualRuneNorth( mobile )

	foreach thing in ( ListItemsNearLocation( mobile.x, mobile.y+5, mobile.z, 6, mobile.realm ))
		if( thing.IsRitualRune() == "north" )
			return thing;
		endif

                SleepMS(5);
	endforeach
 
        return 0;
endfunction

function CreateSpeechCaptor( mobile, x, y, z, spell_id )

	captor := CreateItemAtLocation( x, y, z, 0xBA2B, 1, mobile.realm );
        if( captor.errortext )
                SendSysMessage( mobile, "Error creating ritual captor -->"+captor.errortext );
                return 0;
        endif

	RegisterForSpeechEvents( captor, 6 );
	EnableEvents( SYSEVENT_SPEECH, 6 );
        
        SetObjProperty( captor, "#MasterSerial", mobile.serial );
        SetObjProperty( captor, "SpellId", spell_id );
 
	magic_circle.Append( captor );

        return 1;
endfunction

function CircleUnDecayable( mobile )

	foreach item in magic_circle
 
                SetObjProperty( item, "#PermMagicCircle", mobile.serial );
                item.movable := 0;
		item.DecayAt := 0;
                item.SaveOnExit := 0;
                
                SleepMS(5);
	endforeach

        return 1;
endfunction

function DestroyDone( mobile, forced:=0, item:=0 )

	foreach item in magic_circle
		DestroyItem( item );
                SleepMS(5);
	endforeach
 
        if( item )
                DestroyItem( item );
        endif
        
        if( forced )
		PrintTextAbove( mobile, "*The mystic energy go out of control and unleash itself upon "+mobile.name+"!*" );
                if( mobile.cmdlevel < 4 )
		        mobile.Kill();
                endif
        endif
 
        return 1;
endfunction

function ChangeLightLevel( mobile, level )

        foreach player in ListMobilesNearLocation( mobile.x, mobile.y, mobile.z, 10, mobile.realm );
                SleepMS(5);
                if( player.IsA( POLCLASS_NPC ))
                        continue;
                endif

                player.SetLightLevel( level, -1 );
        endforeach
        
        return 1;
endfunction

function GetRitualItem( mobile, spell_id )

	var ritual_cfg := RITUAL_GetSettingsCfgElem( spell_id ),
	    targ_options := GetConfigString( ritual_cfg, "TargOptions" ),
            item;

	while( mobile.connected )

		SendSysMessage( mobile, "Choose the item you wish to enchant." );
		item := Target( mobile );
                if( !item )
			SendSysMessage( mobile, "The ritual is cancelled." );
                        return 0;
       		elseif( !useItem_check( mobile, item, ITEM_INCONTAINER ))
			continue;
                elseif( GetObjProperty( item, "UnEnchantable" ) || item.newbie )
			SendSysMessage( mobile, "You can't enchant this item." );
			continue;
                endif

                case( targ_options )
                        "crystal":
                                if( !item.IsCrystalMana() )
				        SendSysMessage( mobile, "You must target a mana crystal." );
                                        continue;
                                endif
                                break;
                        "ritual":
                                if( !item.IsRitualRobe() && !item.IsOakStaff() )
				        SendSysMessage( mobile, "You must target a ritual robe or a young oak staff." );
                                        continue;
                                endif
                                break;
                        "jewel":
                                if( !item.IsJewel() )
				        SendSysMessage( mobile, "You must target a jewel." );
                                        continue;
                                endif
                                break;
                        "armor":
                                if( !item.IsBoneArmor() && !item.IsChainmailArmor() && !item.IsPlatemailArmor() &&
                                    !item.IsLeatherArmor() && !item.IsRingmailArmor() && !item.IsShield() && !item.IsStuddedLeatherArmor() )
				        SendSysMessage( mobile, "You must target an armor." );
                                        continue;
                                endif
                                break;
                        "hp":
                                if( !item.hp )
				        SendSysMessage( mobile, "You must target an item with HP." );
                                        continue;
                                endif
                                break;
                        "jewelArmor":
                                if( !item.IsBoneArmor() && !item.IsChainmailArmor() && !item.IsPlatemailArmor() && !item.IsJewel() &&
                                    !item.IsLeatherArmor() && !item.IsRingmailArmor() && !item.IsShield() && !item.IsStuddedLeatherArmor() )
				        SendSysMessage( mobile, "You must target a piece of armor or a jewel." );
                                        continue;
                                endif
                                break;
                        "weapon":
                                if( !item.IsA( POLCLASS_WEAPON ))
				        SendSysMessage( mobile, "You must target a weapon." );
                                        continue;
                                endif
                                break;
                        "cloth":
                                if( !item.IsCloth() )
				        SendSysMessage( mobile, "You must target clothes." );
                                        continue;
                                endif
                                break;
                        "equippable":
                                if( item.IsA( POLCLASS_EQUIPMENT ))
				        SendSysMessage( mobile, "You must target an equipment." );
                                        continue;
                                endif
                                break;
                        "weaponArmor":
                                if( !item.IsBoneArmor() && !item.IsChainmailArmor() && !item.IsPlatemailArmor() && !item.IsJewel() && !item.IsA( POLCLASS_WEAPON )
                                    && !item.IsLeatherArmor() && !item.IsRingmailArmor() && !item.IsShield() && !item.IsStuddedLeatherArmor() )
				        SendSysMessage( mobile, "You must target a piece of armor or a jewel." );
                                        continue;
                                endif
                                break;
                        default:
                                break;
                endcase

                var cursed := GetObjProperty( item, "Cursed" );
		if( cursed && targ_options != "cursed" )
			SendSysMessage( mobile, "You can't target cursed items." );
			continue;
		endif

                return item;
	endwhile

        return 0;
endfunction

function ProcessRitual( mobile, spell_id, item )

        var north_rune := GetRitualRuneNorth( mobile );
        if( !north_rune )
		SendSysMessage( mobile, "Your circle was corrupted!" );
                return 0;
        endif

	var x := north_rune.x,
	    y := north_rune.y,
	    z := north_rune.z,
            realm := north_rune.realm;
 
        if( item )
		item.movable := 1;
		MoveObjectToLocation( item, x, y+5, z, realm, MOVEOBJECT_FORCELOCATION );
		item.movable := 0;
        endif
        
	var total_mana := 0;

	if( YesNo( mobile, "Use mana crystals?" ))
		while( mobile.connected )
			SendSysMessage( mobile, "Select a mana crystal to access, Hit Esc to cancel." );
			var crystal := Target( mobile );
			if( !crystal )
                                SendSysMessage( mobile, "Cancelled the use of a mana crystal." );
				break;
       			elseif( !useItem_check( mobile, crystal, ITEM_INCONTAINER ))
				continue;
                        elseif( mobile.IsCrystalMana() )
				SendSysMessage( mobile, "That is not a mana crystal." );
				continue;
                        endif
                        
                        var crystal_level := CInt( GetObjProperty( crystal, "ManaLevel" ));
                        if( !crystal_level )
				SendSysMessage( mobile, "This crystal have no mystic energy left to use." );
				continue;
			endif
   
                        total_mana += crystal_level;
                        EraseObjProperty( crystal, "ManaLevel" );
		endwhile
	endif

	PrintTextAbove( item, "*The ritual has started*" );
	Sleep(5);

        var ritual_cfg := RITUAL_GetSettingsCfgElem( spell_id ),
            elem_settings := RITUAL_GetSettingsCfgElem( "General" ),
            speech_delay := elem_settings.DelaySpeech,
            base_hp := AP_GetVital( mobile, HITS ),
            current_speech := 1;
        
	var cloud_z := 1;
	while( GetConfigString( ritual_cfg, "Speech"+cloud_z ))
		cloud_z += 1;
                SleepMS(5);
	endwhile

	var mana_cloud := CreateItemAtLocation( item.x, item.y, item.z+( 4 * cloud_z ), 0x3789, 1, realm );
        if( mana_cloud.errortext )
                PrintTextAbove( item, "Error creating mana cloud -->"+mana_cloud.errortext );
        else
		SetName( mana_cloud, "mana cloud" );
  
		mana_cloud.movable := 0;
		mana_cloud.saveonexit := 0;
        	magic_circle.Append( mana_cloud );
        endif

	while( mobile.connected )

                var speech := GetConfigString( ritual_cfg, "Speech"+current_speech );
                if( !speech )
                	break;
                elseif( !captor )
                	return 0;
                endif

		var text := GetConfigString( ritual_cfg, "Print"+current_speech ),
		    x_mod := GetConfigInt( ritual_cfg, "Xmod"+current_speech ),
		    y_mod := GetConfigInt( ritual_cfg, "Ymod"+current_speech ),
		    facing := GetConfigInt( ritual_cfg, "Facing"+current_speech ),
		    action := GetConfigInt( ritual_cfg, "Action"+current_speech ),
		    appear := GetConfigInt( ritual_cfg, "Appear"+current_speech ),
		    anim := GetConfigInt( ritual_cfg, "Anim"+current_speech ),
		    sfx := GetConfigInt( ritual_cfg, "Sfx"+current_speech );

                SendSysMessage( mobile, "Ritual: "+speech, 3, 553 );

                var starting_delay := ReadGameClock();
		while( mobile.connected )

                	if( !captor )
                		return 0;
                	elseif( ReadGameClock() - starting_delay > speech_delay )
				return 0;
			elseif( AP_GetVital( mobile, HITS ) < base_hp )
				return 0;
			endif

		        var event := os::Wait_For_Event(1);
			if( event && event.source == mobile )
				if( event.type == SYSEVENT_SPEECH )
					if( event.text[speech] )
                                                break;
                                        else
						return 0;
                                        endif
				endif
			endif
		endwhile

                Sleep(1);

                if( mobile.x != ( x+x_mod ) || mobile.y != ( y+y_mod ))
			return 0;
                elseif( facing != mobile.facing )
			return 0;
                endif
                
		if( text )
			PrintTextAbove( mobile, text );
		endif
		if( action )
			PerformAction( mobile, action );
		endif
		if( anim )
			PlayObjectCenteredEffect( mobile, anim, 0xa, 0xa );
		endif
		if( sfx )
			PlaySoundEffect( mobile, sfx );
		endif

                
		var mana_amount := CInt( AP_GetVital( mobile, MANA ) / 4 );
                AP_ConsumeVital( mobile, MANA, mana_amount );
		total_mana += mana_amount;
  
		PlayMovingEffect( mobile, mana_cloud, 0x3789, 0xa, 0xa );
  
		if( item )
			PrintTextAbove( item, "*the mystic energy channels above the item*" );
		endif

		if( appear )
			var the_items := SplitWords( appear ),
			    item_x_mods := SplitWords( GetConfigString( ritual_cfg, "IXmod"+current_speech )),
			    item_y_mods := SplitWords( GetConfigString( ritual_cfg, "IYmod"+current_speech ));

			var i;
			for( i:=1; i<=Len( the_items ); i+=1 )
				var to_be_created := CInt( the_items[i] ),
				    item_x_mod := CInt( item_x_mods[i] ),
				    item_y_mod := CInt( item_y_mods[i] );
         
				if( !to_be_created )
					to_be_created := GetObjtypeByName( the_items[i] );
				endif

				magic_circle.Append( CreateItemAtLocation( x+item_x_mod, y+item_y_mod, z, to_be_created, 1, realm ));
				magic_circle[Len( magic_circle )].movable := 0;

				SleepMS(5);
			endfor
		endif

		if( item )
			item.movable := 1;
			MoveObjectToLocation( item, item.x, item.y, item.z+2, realm, MOVEOBJECT_FORCELOCATION );
			item.movable := 0;
		endif

		mana_cloud.movable := 1;
		MoveObjectToLocation( mana_cloud, mana_cloud.x, mana_cloud.y, mana_cloud.z-2, realm, MOVEOBJECT_FORCELOCATION );
		mana_cloud.movable := 0;

		current_speech += 1;
		Sleep(2);
	endwhile
 
	Sleep(7);

	PrintTextAbove( item, "*All the mystic energy goes through "+item.desc+" as the final ritual process begins*" );
	PlaySoundEffect( mobile, 0x01fe );
 
	var mana_part := CInt( total_mana / 10 );

        var i;
	for( i:=1; i<=10; i+=1 )
		PlayMovingEffect( mana_cloud, mobile, 0x3789, 0xa, 0xa );
                AP_ConsumeVital( mobile, MANA, mana_part );
		total_mana -= mana_part;
		SleepMS(500);
	endfor

	DestroyItem( mana_cloud );
 
        if( !RitualSkillCheck( mobile, spell_id, item ))
                return 0;
        endif

        UndoRitualCircle( mobile, item );
        ChangeLightLevel( mobile, 0 );
        
	item.movable := 1;
	MoveItemToContainer( item, mobile.backpack );

	return 1;
endfunction

function RitualSkillCheck( mobile, spell_id, item )

	var spell_elem := SPELL_GetSpellsCfgElem( spell_id ),
            mana_required := spell_elem.ManaCost,
            spell_diff := spell_elem.Difficulty;
        
        if( mobile.cmdlevel < 4 )
		if( !AP_ConsumeVital( mobile, MANA, CInt( mana_required )))
                	SendSysMessage( mobile, "You don't have enough mana to cast that spell!" );
                	DestroyDone( mobile, 1, item );
                	return 0;
        	elseif( !SkillCheck( mobile, MAGERY, spell_diff ))
                	SendSysMessage( mobile, "The spell fizzles." );
                	DestroyDone( mobile, 1, item );
                	return 0;
        	endif
        endif

	Sleep(5);

	PrintTextAbove( item, "The item absorbed the enchantment successfully!" );
 
        PlaySoundEffect( item, 0x1FB );
        PlayObjectCenteredEffectEx( item, 0x373A, 4, 14 );

	Award_Reputation( mobile, FAME, CInt( spell_diff ));

	return 1;
endfunction
