/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-12-12

        Last Modifications 2010-01-26
             * Added check for guildstone()
             * Fixed DemolishHouse()
*/
use uo;
use os;

include ":gumps:gumpprompt";
include ":gumps:selectiongump";
include ":gumps:yesno";
include ":keys:key";
include ":multicommands:multicommands";
include ":statistics:account";

function AddCo( mobile, house )

        var text := "Warning: All functions will be given to co-owner excepted: demolish the house, change co-owner, transfer ownership and change master key.";
        GumpPrompt( mobile, text );

        SendSysMessage( mobile, "Select the new co-owner." );
        var targ := Target( mobile );
        if( !targ )
                SendSysMessage( mobile, "Cancelled." );
                return 0;
        elseif( !targ.acctname )
                SendSysMessage( mobile, "You should consider therapy!" );
                return 0;
        elseif( targ.acctname == mobile.acctname )
                SendSysMessage( mobile, "You already own this house!" );
                return 0;
        elseif( house.IsCoOwner( targ ))
                SendSysMessage( mobile, targ.name+" is already co-owner!" );
                return 0;
        elseif( house.IsFriend( targ ))
                SendSysMessage( mobile, targ.name+" is already a house friend!" );
                return 0;
        endif

        house.SetCoOwner( targ );
        SendSysMessage( mobile, "Co-owner "+targ.name+" is now added." );
        
	return 1;
endfunction

function RemoveCo( mobile, house )

        if( !house.GetCoOwner() )
                SendSysMessage( mobile, "The house has no co-owner." );
                return 0;
        endif
        
        SendSysMessage( mobile, "Co-owner "+house.GetCoOwner().name+" is now removed." );
        house.RemoveCoOwner();

	return 1;
endfunction

function AddFriend( mobile, house )

        var text := "Warning: Friends can open the secured chests.";
        GumpPrompt( mobile, text );

        SendSysMessage( mobile, "Select a new Friend." );
        var targ := Target( mobile );
        if( !targ )
                SendSysMessage( mobile, "Cancelled." );
                return 0;
        elseif( !targ.acctname )
                SendSysMessage( mobile, "You should consider therapy!" );
                return 0;
        elseif( targ.acctname == mobile.acctname )
                SendSysMessage( mobile, "You already own this house!" );
                return 0;
        elseif( house.IsCoOwner( targ ))
                SendSysMessage( mobile, targ.name+" is already co-owner!" );
                return 0;
        elseif( house.IsFriend( targ ))
                SendSysMessage( mobile, targ.name+" is already a house friend!" );
                return 0;
        endif

        house.AddFriend( targ );
        SendSysMessage( mobile, "Friend "+targ.name+" is now added." );

	return 1;
endfunction

function RemoveFriend( mobile, house )

        var index := house.GetFriends();
        if( !index.Size() )
                SendSysMessage( mobile, "This house has no friends." );
                return 0;
        endif

        var options := array{};
        foreach friend in index
                options.Append( friend.name );
                SleepMS(5);
        endforeach
        
	var selected := SelectionGump( mobile, "Remove House-Friends", options, BTN_TYPE_CHECKBOX );

        foreach friend in index
                if( friend.name in selected )
                       index.Erase( _friend_iter );
                endif
                
                SleepMS(5);
        endforeach
        
        house.SetFriendsList( index );

	return 1;
endfunction

function ListFriends( mobile, house )

        var index := house.GetFriends();
        if( !index.Size() )
                SendSysMessage( mobile, "This house has no friends." );
                return 0;
        endif

        var options := array{};
        foreach friend in index
                options.Append( friend.name );
                SleepMS(5);
        endforeach
        
        SelectionGump( mobile, "House-Friends", options, BTN_NOTYPE );

	return 1;
endfunction

function EjectPlayer( mobile, sign )

        SendSysMessage( mobile, "Select a player to eject from the house." );
        var targ := Target( mobile, TGTOPT_NOCHECK_LOS );
        if( !targ )
                SendSysMessage( mobile, "Cancelled." );
                return 0;
        elseif( !targ.acctname )
                SendSysMessage( mobile, "You should consider therapy!" );
                return 0;
        elseif( targ.acctname == mobile.acctname )
                SendSysMessage( mobile, "You cannot eject yourself!" );
                return 0;
        elseif(( targ.multi ).serial != sign.IsHouseSign() )
                SendSysMessage( mobile, "This player is not in the house!" );
                return 0;
        endif

        MoveObjectToLocation( targ, sign.x, sign.y, 0, sign.realm, MOVEOBJECT_FORCELOCATION );

	SendSysMessage( mobile, targ.name+" has been ejected!" );

	return 1;
endfunction

function BanPlayer( mobile, house, sign )

        SendSysMessage( mobile, "Select a player to ban form the house." );
        var targ := Target( mobile, TGTOPT_NOCHECK_LOS );
        if( !targ )
                SendSysMessage( mobile, "Cancelled." );
                return 0;
        elseif( targ.acctname == mobile.acctname )
                SendSysMessage( mobile, "You cannot ban yourself!" );
                return 0;
        elseif( targ.cmdlevel )
                SendSysMessage( mobile, "You should consider therapy." );
                return 0;
        elseif( house.IsCoOwner( targ ))
                SendSysMessage( mobile, "You can not ban the co-owner of this house." );
                return 0;
        elseif( house.IsFriend( targ ))
                SendSysMessage( mobile, "You can not ban a friend of this house." );
                return 0;
        elseif( house.IsBanned( targ ))
                SendSysMessage( mobile, targ.name+" is already banned from this house." );
                return 0;
        elseif( targ.IsA( POLCLASS_NPC ))
                SendSysMessage( mobile, "That would be wise to kill it instead." );
                return 0;
        endif
        
        house.AddBan( targ );
        
        if(( targ.multi ).serial == sign.IsHouseSign() )
                MoveObjectToLocation( targ, sign.x, sign.y, 0, sign.realm, MOVEOBJECT_FORCELOCATION );
                SendSysMessage( mobile, targ.name+" has been ejected and banned!" );
                return 1;
        endif
        
        SendSysMessage( mobile, targ.name+" has been banned!" );

	return 1;
endfunction

function UnbanPlayer( mobile, house )

        var index := house.GetBanned();
        if( !index.Size() )
                SendSysMessage( mobile, "This house has no banned players." );
                return 0;
        endif

        var options := array{};
        foreach banned in index
                options.Append( banned.name );
                SleepMS(5);
        endforeach

	var selected := SelectionGump( mobile, "Remove House-Banned Player", options, BTN_TYPE_CHECKBOX );

        foreach banned in index
                if( banned.name in selected )
                       index.Erase( _banned_iter );
                endif

                SleepMS(5);
        endforeach

        house.SetBanList( index );

	return 1;
endfunction

function ChangeName( mobile, house, sign )

        var new_name := SendTextEntryGump( mobile, "Enter a name for this house.", TE_CANCEL_ENABLE, TE_STYLE_NORMAL, 19 );
        if( !new_name )
                SendSysMessage( mobile, "Cancelled." );
                return 0;
        elseif( Len( SplitWords( new_name )) > 4 )
                SendSysMessage( mobile, "House name are limited to 4 words or less." );
                return 0;
        endif
        
        SetName( sign, new_name );
        house.SetHouseName( new_name );
        
        SendSysMessage( mobile, "House name is now changed." );

	return 1;
endfunction

function TransferOwnership( mobile, house, sign )

	SendSysMessage( mobile, "Transfer the house to whom?" );
	var targ := Target( mobile );
        if( !targ )
                SendSysMessage( mobile, "Cancelled." );
                return 0;
        elseif( !targ.IsA( POLCLASS_MOBILE ))
                SendSysMessage( mobile, "You must select a player." );
                return 0;
        elseif( targ.IsA( POLCLASS_NPC ))
                SendSysMessage( mobile, "You must select a player." );
                return 0;
        elseif( GetAccountProperty( targ.acctname, "HasHouse" ) && targ.cmdlevel < 3 )
                SendSysMessage( mobile, targ.name+" is already owning a house." );
                return 0;
        elseif( Distance( mobile, sign ) > 5 )
                SendSysMessage( mobile, " You are too far away from the house's sign." );
                return 0;
        elseif( Distance( targ, sign ) > 5 )
                SendSysMessage( mobile, targ.name+" is too far away from the house's sign." );
                return 0;
        elseif( !YesNo( targ, "Would you like to take the ownership of the house?", "I want", "I do not want" ))
                SendSysMessage( mobile, targ.name+" declined the transfer." );
                return 0;
        endif
        
        DestroyGuildStone( house );

        //So if sign is used by staff, it will find the reel owner...
        var owner_serial := house.GetOwner().serial,
            owner := SystemFindObjectBySerial( owner_serial );
            
        EraseAccountProperty( owner.acctname, "HasHouse" );
        
        house.SetOwner( targ );
        SetAccountProperty( targ.acctname, "HasHouse", house.serial );
        
        SendSysMessage( mobile, "House transfer was successful." );
        
        SendSysMessage( targ, "You have taken ownership of the house." );
       	SendSysMessage( targ, "It would be wise to change the locks soon." );

	return 1;
endfunction

function DemolishHouse( mobile, house )

        if( !YesNo( mobile, "Are you sure you want to destroy the house and all it's components? ( Everything inside the house will be destroyed )", "Yes, I am", "No, I am not." ))
                SendSysMessage( mobile, "Cancelled." );
                return 0;
        endif
        
        //Make sure to delete the guildstone before house.items because we need to delete few properties...
        DestroyGuildStone( house );

        //So if sign is used by staff, it will find the reel owner...
        var owner_serial := house.GetOwner().serial,
            owner := SystemFindObjectBySerial( owner_serial );

        EraseAccountProperty( owner.acctname, "HasHouse" );
        
	foreach obj in ( house.items )
		DestroyItem( obj );
		SleepMS(5);
	endforeach

	foreach obj in ( house.components )
		DestroyItem( obj );
		SleepMS(5);
	endforeach
 
        var deed_objtype := house.GetHouseDeedObjType();
            
        var dest := Multi_DestroyMulti( house );
        if( dest.errortext )
                SendSysMessage( mobile, "Error destroying the multi -->"+dest.errortext );
                return 0;
        endif
        
        var created := CreateItemInBackpack( mobile, deed_objtype, 1 );
        if( !created )
                CreateItemAtLocation( mobile.x, mobile.y, mobile.z, deed_objtype, 1, mobile.realm );
        endif

	return 1;
endfunction

function ChangeKey( mobile, house )

        SendSysMessage( mobile, "Select the new master key." );
        var targ := Target( mobile );
        if( !targ )
                SendSysMessage( mobile, "Cancelled." );
                return 0;
        elseif( !targ.IsKey() )
                SendSysMessage( mobile, "This is not a key!" );
                return 0;
        endif

        var lock_id := AllocLockId();
        targ.SetLockId( lock_id );

        foreach item in ( house.components )
                if( item.IsDoor() )
                        item.locked := 1;
                        SetObjProperty( item, "LockId", lock_id );
                endif
                SleepMS(5);
        endforeach
        
        SendSysMessage( mobile, "Locks have been changed." );

	return 1;
endfunction

function ItemManagment( mobile )

        if( GetProcess( CInt( GetObjProperty( mobile, "#HouseManagerPid" ))))
		return 0;
	endif
 
        var script := start_script( ":housing:itemManager/itemManager", mobile );
        if( script.errortext )
                SendSysMessage( mobile, "Error starting script <:housing:itemManager/itemManager> -->"+script.errortext );
                return 0;
        endif

	return 1;
endfunction

function DestroyGuildStone( house )

        var guildstone_serial := house.GetGuildStone();
        if( guildstone_serial )
                DestroyItem( SystemFindObjectBySerial( guildstone_serial ));
        endif

        return 1;
endfunction
