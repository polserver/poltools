/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2011-01-26
*/
use uo;
use os;

include "include/itemUtil";
include ":gumps:yesno";
include ":spells:spells_ex";
include ":timedScripts:timedScripts";
include ":totem:totem";

function Totem_Hide( npc )

        //Assumes npc is already a totem.
        
        TS_StartTimer( npc, "Invisibility", CInt( 10 ), 0 );

        return 1;
endfunction

function Totem_CastSpell( npc, master, spell_id )

        //Assumes npc is already a totem.

        var circle := SPELL_GetCircle( spell_id );
        if( circle > Totem_GetLevel( npc ))
                return 0;
        endif

	var script := Run_Script( ":magery:spellStarter", {npc, spell_id, 0, master, 0, 0, 0, 1} );
	if( script.errortext )
		PrintTextAbove( npc, "Error - Could not start spell -->"+script.errortext );
		return 0;
	endif

	return 1;
endfunction

function Totem_Loot( npc )

        //Assumes npc is already a totem.

        if( Totem_GetLevel( npc ) < 7 )
                return 0;
        endif

        NPC_SetupBackPack( npc );

	var near_items := ListItemsNearLocation( npc.x, npc.y, npc.z, 4, npc.realm );
        if( !near_items.Size() )
                return 0;
        endif

	StopActivities( npc );

        foreach item in near_items
                SleepMS(5);

                if( item.IsTotemItem() )
                        continue;
                elseif( item.IsA( POLCLASS_CORPSE ) && !A_IsIn( item, AREAS_NO_LOOTING ))

                        if( !Totem_RunToIt( npc, item ))
                                continue;
                        endif

                        var corpse_items := EnumerateItemsInContainer( item );
                        if( !corpse_items.Size() )
                                continue;
                        endif

                        foreach thing in corpse_items
                                SleepMS(5);

                                if( thing.container.serial != item.serial )
                                        continue;
                                elseif( !useItem_check( npc, item, DEFAULT_ITEM ))
		       	                continue;
                                endif

                                var move := MoveItemToContainer( thing, npc.backpack );
                                if( move )
                                        AI_Speak( npc, "*Yoink*" );
                                        Sleep(1);
                                endif
                        endforeach
                else
                        if( !useItem_check( npc, item, ITEM_WILD ))
		       	        continue;
                        elseif( !Totem_RunToIt( npc, item ))
                                continue;
                        endif

                        var move := MoveItemToContainer( item, npc.backpack );
                        if( move )
                                AI_Speak( npc, "*Yoink*" );
                                Sleep(1);
                        endif
                endif

                SleepMS(5);
        endforeach

        return 1;
endfunction

function Totem_GetItem( npc, master )

        //Assumes npc is already a totem.

        if( Totem_GetLevel( npc ) < 7 )
                return 0;
        endif

        var targ := Target( master );
        if( !targ )
                SendSysMessage( master, "Cancelled." );
                return 0;
        elseif( !targ.IsA( POLCLASS_ITEM ))
                SendSysMessage( master, "You should consider therapy." );
                return 0;
        elseif( !useItem_check( master, targ, ITEM_WILD ))
                return 0;
        endif

	StopActivities( npc );

        if( !Totem_RunToIt( npc, targ ))
                return 0;
        endif

        var move := MoveItemToContainer( targ, npc.backpack );
        if( move )
                AI_Speak( npc, "*Yoink*" );
                Sleep(1);
        endif

        return 1;
endfunction

function Totem_GiveLoot( npc, master )

        //Assumes npc is already a totem.

        var list_items := EnumerateItemsInContainer( npc.backpack );
        if( !list_items.Size() )
                return 0;
        endif

        var targ := Target( master );
        if( !targ )
                SendSysMessage( master, "Cancelled." );
                return 0;
        elseif( !targ.IsA( POLCLASS_MOBILE ) || targ.IsA( POLCLASS_NPC ))
                SendSysMessage( master, "You must select an active player." );
                return 0;
        elseif( targ.serial != master.serial && !YesNo( targ, "Do you accept items from "+master.name+"'s totem?", "Yes, of course!", "Nah, no need.", 10 ))
                SendSysMessage( master, targ.name+" does not accept." );
                return 0;
        endif

	StopActivities( npc );

        if( !Totem_RunToIt( npc, targ ))
                return 0;
        endif

        foreach item in list_items
                SleepMS(5);

                if( item.IsTotemItem() )
                        continue;
                elseif( item.container.serial != npc.serial )
                        continue;
                endif

                var move := MoveItemToContainer( item, targ.backpack );
                if( move )
                        Sleep(1);
                endif
        endforeach

        return 0;
endfunction

function Totem_DropContainer( npc )

        //Assumes npc is already a totem.

        var list_items := EnumerateItemsInContainer( npc.backpack );
        if( !list_items.Size() )
                return 0;
        endif

	StopActivities( npc );

        foreach item in list_items
                SleepMS(5);

                if( item.IsTotemItem() )
                        continue;
                elseif( item.container.serial != npc.serial )
                        continue;
                elseif( !useItem_check( npc, item, DEFAULT_ITEM ))
                        continue;
                endif

                var move := MoveObjectToLocation( item, npc.x, npc.y, npc.z, npc.realm, MOVEOBJECT_FORCELOCATION );
                if( move )
                        Sleep(1);
                endif
        endforeach

        return 1;
endfunction

function Totem_Help( npc )

        var level_string := array{};
            level_string.Append( "hide " );
            level_string.Append( "heal - nightsight " );
            level_string.Append( "agility - cunning - cure - protection - strength " );
            level_string.Append( "bless " );
            level_string.Append( "greaterheal " );
            level_string.Append( "attack " );
            level_string.Append( "reveal " );
            level_string.Append( "loot - boost " );

        AI_Speak( npc, "I will obey the following orders:" );

        var level := Totem_GetLevel( npc )+1,
            commands := "";

        var i;
        for( i:=1; i<=level; i+=1 )
                AI_Speak( npc, level_string[i] );
                Sleep(1);
        endfor

        return 1;
endfunction

function Totem_RunToIt( npc, item )

	var i;
	for( i:=4; i>0; i-=1 )
		if ( Distance( npc, item ) < 2 )
			return 1;
		else
			AI_Move( npc, item.x, item.y, item.z, NEMOVE_TOWARD, NEMOVE_RUN, 1 );
		endif
	endfor

        return 1;
endfunction
