/*
        Maintened by *Edwards

        2009-12-30
*/
use uo;
use os;

include ":champSpawn:settings";

function GetSpawn( altar )

        var spawn := GetObjProperty( altar, "Spawn" );
        if( spawn.errortext )
		PrintTextAbove( altar, "Error could not ready property spawn -->"+spawn.errortext );
		return 0;
        endif
        
	var spawn_elem := CS_GetSettingsCfgElem( spawn );
	if( spawn_elem.errortext )
		return 0;
	endif

	var spawn_name := "level"+GetLevel( altar );

	var spawns := GetConfigStringArray( spawn_elem, spawn_name );
	if( spawns.errortext )
		PrintTextAbove( altar, "Error could not get spawn name <"+spawn_name+"> -->"+spawns.errortext );
		return 0;
	endif

	return spawns.RandomEntry();
endfunction

function GetChamp( altar )

        var spawn := GetObjProperty( altar, "Spawn" );
        if( spawn.errortext )
		PrintTextAbove( altar, "Error could not ready property spawn -->"+spawn.errortext );
		return 0;
        endif

	var spawn_elem := CS_GetSettingsCfgElem( spawn );
	if( spawn_elem.errortext )
		return 0;
	endif

	var spawn_name := "champion";

	var spawns := GetConfigStringArray( spawn_elem, spawn_name );
	if( !spawns )
		PrintTextAbove( altar, "Error could not get spawn name <"+spawn_name+"> -->"+spawns.errortext );
		return 0;
	endif

	return ( spawns[1] );
endfunction

function SpawnChampion( altar )

	var creature := CreateSpawn( altar, GetChamp( altar ));
	SetObjProperty( altar, "#Champ", creature );
 
        return 1;
endfunction

function GetLevel( altar )

	var redSkulls := CInt( GetObjProperty( altar, "lrskull" ));
	if( redSkulls < 6 )
		return 0;
	elseif( redSkulls < 10 )
		return 1;
	elseif( redSkulls < 14 )
		return 2;
	else
		return 3;
	endif
endfunction

function GetMaxMonsters( altar )

	return ( 250 - GetLevel( altar ) * 12 );
endfunction

function EnableAltar( altar )

	var pentagram := GetObjProperty( altar, "pentagram" );
	foreach piece in pentagram
		piece := SystemFindObjectBySerial( piece );
		piece.invisible := 0;
		piece.color := 0;
  
                SleepMS(5);
	endforeach
 
	SetObjProperty( altar, "status", "active" );
 
        return 1;
endfunction

function DisableAltar( altar )

	var pentagram := GetObjProperty( altar, "pentagram" );
	foreach piece in pentagram
		piece := SystemFindObjectBySerial( piece );
		piece.color := 980;
		piece.invisible := 1;
  
                SleepMS(5);
	endforeach
 
	SetObjProperty( altar, "NextActiv", ReadGameClock() + ACTIVE_EACH );
	SetObjProperty( altar, "status", "disable" );
	EraseObjProperty( altar, "#Completed" );
	EraseObjProperty( altar, "#Champ" );
 
        return 1;
endfunction

function GetRedSkullLocation( altar, index )

	var x_pos,
            y_pos;
            
	if( index < 5 )
		x_pos := index - 2;
		y_pos := -2;
	elseif( index < 9 )
		x_pos := 2;
		y_pos := index-6;
	elseif( index < 13 )
		x_pos := 10 - index;
		y_pos := 2;
	else
		x_pos := -2;
		y_pos := 14 - index;
	endif

	return {altar.x+x_pos, altar.y+y_pos, altar.z};
endfunction

function GetWhiteSkullLocation( altar, index )

	var x_pos := 0,
            y_pos := 0;
            
	case( index )
		default:
		0:      x_pos := -1;
                        y_pos := -1;
                        break;
                        
		1:      x_pos :=  1;
                        y_pos := -1;
                        break;
                        
		2:      x_pos :=  1;
                        y_pos :=  1;
                        break;
                        
		3:      x_pos := -1;
                        y_pos :=  1;
                        break;
	endcase
 
	return {altar.x+x_pos, altar.y+y_pos, altar.z};
endfunction

function CreateNextWhiteSkull( altar )

	var lw_skull := CInt( GetObjProperty( altar, "lwskull" ));
	var skulls := array{};

	skulls := GetObjProperty( altar, "wskulls" );
	if( lw_skull == 4 )
		WipeItemsArray( skulls );

		SetObjProperty( altar, "wskulls", skulls );
		EraseObjProperty( altar, "lwskull" );

		CreateNextRedSkull( altar );
		return 1;
	endif

	var location := GetRedSkullLocation( altar, lw_skull + 1 );
	var skull := CreateItemAtLocation( location[1], location[2], location[3], 0x1854, 1, altar.realm );

	skull.movable := 0;
	if( !skulls )
		skulls := {skull.serial};
	else
		skulls.Append( skull.serial );
	endif

        PlaySoundEffect( skull, 0x29 );
	PlayStationaryEffect( skull.x, skull.y, skull.z, 0x3728, 7, 0x10, 0, skull.realm );

	SetObjProperty( altar, "lwskull", lw_skull + 1 );
	SetObjProperty( altar, "wskulls", skulls );

	return 1;
endfunction

function CreateNextRedSkull( altar )

	var lr_skull := CInt( GetObjProperty( altar, "lrskull" ));

	var skulls := GetObjProperty( altar, "rskulls" );
        if( !skulls )
                skulls := array{};
        endif
        
	var w_skulls := GetObjProperty( altar, "wskulls" );
        if( !w_skulls )
                w_skulls := array{};
        endif
        
	WipeItemsArray( w_skulls );
 
	SetObjProperty( altar, "wskulls", w_skulls );
	EraseObjProperty( altar, "lwskull" );
 
	if( lr_skull > 16 )
		WipeItemsArray( skulls );

		SetObjProperty( altar, "rskulls", skulls );
		EraseObjProperty( altar, "lrskull" );

		SetObjProperty( altar, "#Completed", 1 );
		return 1;
	endif

	var location := GetWhiteSkullLocation( altar, lr_skull + 1 );
	var skull := CreateItemAtLocation( location[1], location[2], location[3], 0x1854, 1, altar.realm );

	skull.color := 0x26;
	skull.movable := 0;
	if( !skulls )
		skulls := {skull.serial};
	else
		skulls.Append( skull.serial );
	endif

        PlaySoundEffect( skull, 0x29 );
	PlayStationaryEffect( skull.x, skull.y, skull.z, 0x3728, 7, 0x10, 0, skull.realm );

	SetObjProperty( altar, "lrskull", lr_skull + 1 );
	SetObjProperty( altar, "rskulls", skulls );

	return 1;
endfunction

function DestroyLastRedSkull( altar )

	var l_skull := CInt( GetObjProperty( altar, "lrskull" ));
	if( !l_skull )
		return 0;
	endif
 
	var skulls := GetObjProperty( altar, "rskulls" );
	if( !skulls )
		skulls := array{};
	endif
 
	var skull := skulls[l_skull];
 
	DestroyItem( SystemFindObjectBySerial( skull ));
	skulls.Erase( l_skull );

	SetObjProperty( altar, "lrskull", l_skull - 1 );
	SetObjProperty( altar, "rskulls", skulls );

	return 1;
endfunction

function WipeItemsArray( skulls )

	foreach serial in skulls
		DestroyItem( SystemFindObjectBySerial( serial ));
  
                SleepMS(5);
	endforeach

	return 1;
endfunction

function CreateSpawn( altar, template )

         var range := 20;
         var x,
             y,
             z,
             realm,
             npc;

         while( altar )
               x := altar.x + RandomInt( 2 * range ) - range;
               y := altar.y + RandomInt( 2 * range ) - range;
               z := GetWorldHeight( x, y );
               realm := altar.realm;

               if( ListMobilesNearLocation( x, y, z, 3, realm ) < 1 )
                      npc :=  CreateNPCFromTemplate( template, x, y, z, 0, realm );
                      if( npc )
                             break;
                      endif
               endif

               SleepMS(10);
         endwhile

         return npc.serial;
endfunction
