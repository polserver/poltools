/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2011-01-26
*/
use uo;
use os;

include ":attributes:attributes";
include ":classes:classes";

const LEVEL_MULTIPLIER := 1500;

function IsTotem( npc )

         var totem := GetObjProperty( npc, "IsTotem" );
         if( !totem )
                  return 0;
         endif
         
         return 1;
endfunction

function Totem_Find( master )

         var totem_serial := CInt( GetObjProperty( master, "HasTotem" ));
         if( !totem_serial )
                  return 0;
         endif
         
         var totem := SystemFindObjectBySerial( totem_serial );
         if( !totem )
                  return 0;
         endif

         return totem;
endfunction

function Totem_GetLevel( npc )

         var level := CInt( GetObjProperty( npc, "TotemLevel" ));
         if( !level )
                  level := 0;
         endif
         
         return level;
endfunction

function Totem_GetExp( npc )

         var exp := CInt( GetObjProperty( npc, "TotemExp" ));
         if( !exp )
                  exp := 0;
         endif
         
         return exp;
endfunction

function Totem_AddExp( npc, exp )

         if( Distance( npc, npc.master ) > 10 )
                  return 0;
         endif

         var current_level := Totem_GetLevel( npc ),
             current_exp := Totem_GetExp( npc ),
             new_exp := CInt( current_exp + exp ),
             next_level := CInt(( current_level + 1 ) * 5 * LEVEL_MULTIPLIER );
             
         if( new_exp >= next_level )
                  Totem_LevelUP( npc, current_level );
         endif
         
         SetObjProperty( npc, "TotemExp", new_exp );
         
         return 1;
endfunction

function Totem_LevelUP( npc, current_level )

         var new_level := CInt( current_level + 1 );
         if( new_level > 10 )
                  //Avoid errors, there is only 10 levels.
                  return 0;
         endif
         
         PlaySoundEffect( npc, 0x1F3 );
	 PlayObjectCenteredEffect( npc, 0x373A, 7, 14 );
         PlayMovingEffectXYZ( npc.x, npc.y, npc.z+75, npc.x, npc.y, npc.z, 0x373A, 7, 14, 0, npc.realm );
         
         case( new_level )
                  2:
                        npc.graphic := 39;
                        break;
                  4:
                        npc.graphic := 149;
                        break;
                  6:
                        npc.graphic := 128;
                        break;
                  8:
                        npc.graphic := 176;
                        break;
                  10:
                        AP_SetVitalRegenRateMod( npc, HITS, 5 );
                        npc.graphic := 4;
                        break;
         endcase

         AP_SetTrueSkill( npc, RESISTING_SPELLS, CInt( AP_GetSkill( npc, RESISTING_SPELLS ) + ( new_level * 7 )));
         npc.run_speed := CInt( npc.run_speed + ( 10 * new_level ));
         npc.ar_mod := CInt( new_level * 7 );
         
         SetObjProperty( npc, "TotemLevel", new_level );

         return 1;
endfunction

function Totem_UpdateTotemXP( attacker, corpse )
         
         var totem := Totem_Find( attacker );
         if( !totem )
                  return 0;
         endif
         
         var template := corpse.NPCTemplate(),
             hit_points := CInt( template.HITS );
             
         return Totem_AddExp( totem, CInt( hit_points / 100 ));
endfunction

function Totem_GetTotemItem( npc )

         var item_serial := GetObjProperty( npc, "TotemItemSerial" );
         if( !item_serial )
                  return 0;
         endif
         
         var totem_item := SystemFindObjectBySerial( item_serial );
         if( !totem_item )
                  return 0;
         endif
         
         return totem_item;
endfunction

function Totem_UnShrink( master )

	var totem := Totem_Find( master );
        if( !totem )
                return 0;
        endif

        var totem_item := Totem_GetTotemItem( totem );
        var move_item := MoveItemToContainer( totem_item, totem.backpack );
        if( move_item.errortext )
                SendSysMessage( master, "Error moving totem item -->"+move_item.errortext );
                return 0;
        endif
        
        totem_item.invisible := 1;
        totem.hidden := 0;
        totem.frozen := 0;
        SetName( totem, master.name );

        MoveObjectToLocation( totem, master.x, master.y, master.z, master.realm, MOVEOBJECT_FORCELOCATION );

	return 1;
endfunction

function Totem_Shrink( master )

	var totem := Totem_Find( master );
        if( !totem )
                return 0;
        endif

        var totem_item := Totem_GetTotemItem( totem );
        var move_item := MoveItemToContainer( totem_item, master.backpack );
        if( move_item.errortext )
                SendSysMessage( master, "Error moving totem item -->"+move_item.errortext );
                return 0;
        endif

        MoveObjectToLocation( totem, 5140, 1773, 0, "britannia", MOVEOBJECT_FORCELOCATION );
        SetObjProperty( totem, "MustSleep", master.serial );

        totem_item.invisible := 0;
        totem.hidden := 1;
        totem.frozen := 1;

	return 1;
endfunction

function Totem_Resurrect( npc, targ, master_serial )

	SetObjProperty( npc, MAGE, CInt( GetObjProperty( targ, MAGE )));
	SetObjProperty( npc, "TotemLevel", Totem_GetLevel( targ ));
	SetObjProperty( npc, "TotemExp", Totem_GetExp( targ ));
        
        //Update master new totem' serial
        var master := SystemFindObjectBySerial( master_serial, SYSFIND_SEARCH_OFFLINE_MOBILES );
        if( master.errortext )
                PrintTextAbove( npc, "Error couldn't find master -->"+master.errortext );
                return 0;
        endif
        
        if( Totem_Find( master ))
                SendSysMessage( master, "You already have a totem." );
                return 0;
        endif
        
        SetObjProperty( master, "HasTotem", npc.serial );
        
        //Update Totem Item Serial

        var totem_item := Totem_GetTotemItem( targ );
	SetObjProperty( npc, "TotemItemSerial", totem_item );
        
        var move_item := MoveItemToContainer( totem_item, npc.backpack );
        if( move_item.errortext )
                PrintTextAbove( npc, "Error moving totem item -->"+move_item.errortext );
                return 0;
        endif
        
        return 1;
endfunction
