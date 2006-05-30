/* $Id$
 *
 *
 */

use uo;
use os;
use util;
use polsys;

// For: EnumerateOnlineCharactersABC()
//
const SORT_ASCENDING  := 0x0; // Default
const SORT_DESCENDING := 0x1;

/*
 * EnumerateOnlineCharactersABC(dir)
 *
 * Purpose
 * Builds a list of online characters in ABC order.
 *
 * Parameters
 * dir:	Determines if the list should be made ascending or descending.
 *
 * Return Value:
 * Returns an array of mobile references.
 *
 */
function EnumerateOnlineCharactersABC(dir:=SORT_ASCENDING)
	var dict := dictionary;
	var z_list := EnumerateOnlineCharacters();
	foreach player in z_list
		var key := Upper(player.name);
		if ( !dict.Exists(key) )
			dict[key] := array{};
		endif
		dict[key].Append(player);
		SleepMS(2);
	endforeach

	z_list := array{};
	foreach list in ( dict )
		foreach player in ( list )
			if ( dir == SORT_ASCENDING )
				z_list.Append(player);
			else
				z_list.Insert(1, player);
			endif
			SleepMS(2);
		endforeach
		SleepMS(2);
	endforeach

	return z_list;
endfunction

/*
 * EnumeratePhysicalItems(mobile)
 *
 * Purpose
 * Builds a list of equipped items on a mobile that are safe to access.
 *
 * Parameters
 * mobile:	Mobile to get the equipment on.
 *
 * Return Value:
 * Returns an array of item references.
 *
 */
function EnumeratePhysicalItems(mobile)
	var equip_list := array{};

	// Hair, beard, backpack, mount
	var skip_layers := array{11, 16, 21, 25};
	var i;

	for ( i:=0; i<=25; i:=i+1 )
		if ( i in skip_layers ) 
			continue;
		else
			var item := GetEquipmentByLayer(mobile, i);
			if ( item )
				equip_list.Append(item);
			endif
		endif
		SleepMS(2);
	endfor

	return equip_list;
endfunction

/*
 * Just a development function to test lists..
 * Dont get many people online on my computer, but wanna make stuff work
 * on other shards!
 *
 */
function BuildFakePlayerList(amount:=0)
	var list := array{};
	var player := struct{"name", "serial", "x", "y", "z", "realm"};
	var realm_list := Realms();
	var realm_names := realm_list.Keys();
	
	for ( amount; amount>0; amount:=amount-1)
		player.name := "FakePlayer-"+amount;
		player.serial := amount;
		player.realm := realm_names[RandomInt(realm_names.Size())];
		player.x := RandomInt(realm_list[player.realm].width)+1;
		player.y := RandomInt(realm_list[player.realm].height)+1;
		player.z := CInt(GetMapInfo(player.x, player.y, player.realm).z);
		
		list.Append(player);
		SleepMS(2);
	endfor
	
	return list;
endfunction
	