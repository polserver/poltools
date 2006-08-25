/* $Id$
 *
 */

use uo;
use util;
use cfgfile;
use os;

include ":itemutils:itemdesc";
include ":itemutils:itemInfo";

function Loot_Generate(container, loot_index)
	if ( !container.IsA(POLCLASS_CONTAINER) )
		return error{"errortext" := "Object passed was not a container."};
	endif

	var index_cfg := ReadConfigFile(":loot:loot-index");
	if ( index_cfg.errortext )
		return error{"errortext" := "Error - Could not open loot-index.cfg -> "+index_cfg.errortext};
	endif

	var index_elem := index_cfg[loot_index];
	if ( !index_elem )
		return error{"errortext" := "Error - loot-index.cfg elem ["+index_elem+"] does not exist."};
	endif

	foreach line in GetConfigStringArray(index_elem, "Item")
		Loot_CreateItem(container, line);
		SleepMS(2);
	endforeach
	/*
	foreach line in GetConfigStringArray(index_elem, "Stack")
		Loot_CreateStack(container, line);
		SleepMS(2);
	endforeach
	*/
	foreach line in GetConfigStringArray(index_elem, "Random")
		Loot_CreateRandom(container, line);
		SleepMS(2);
	endforeach

	foreach line in GetConfigStringArray(index_elem, "Group")
		if ( line != loot_index )
			//To do:
			// Check to make sure were not going back into the same group
			// to avoid a potential endless loop!
			Loot_Generate(container, line);
		endif
		SleepMS(2);
	endforeach

	return 1;
endfunction

function Loot_CreateItem(container, line)
	var parsed := Loot_ParseString(line);

	if ( Loot_ChanceCheck(parsed.chance) )
		var graphic := GetObjTypeGraphic(parsed.objtype);
		var stackable := IU_GetTileInfo(graphic).Stackable;
		if ( stackable )
			return CreateItemInContainer(container, parsed.objtype, parsed.amount);
		else
			var i;
			for ( i:=1; i<= parsed.amount; i+=1 )
				var item := CreateItemInContainer(container, parsed.name);
				if ( item.errortext )
					return error{"errortext" := "Error - Could not create ["+parsed.name+"] ->"+item.errortext};
				endif
				SleepMS(2);
			endfor

			return 1;
		endif
	endif

	return 0;
endfunction

/*
function Loot_CreateStack(container, line)
	var parsed := Loot_ParseString(line);
	if ( Loot_ChanceCheck(parsed.chance) )
		var item := CreateItemInContainer(container, parsed.name, parsed.amount);
		if ( item.errortext )
			return error{"errortext" := "Error - Could not create ["+parsed.name+"] ->"+item.errortext};
		endif
	endif

	return 1;
endfunction
*/

function Loot_CreateRandom(container, line)
	var parsed := Loot_ParseString(line);
	var groups_cfg := ReadConfigFile(":loot:loot-groups");
	if ( groups_cfg.errortext )
		return error{"errortext" := "Error - Could not open loot-groups.cfg -> "+groups_cfg.errortext};
	endif

	var groups_elem := groups_cfg[parsed.name];
	if ( !groups_elem )
		return error{"errortext" := "Error - loot-groups.cfg elem ["+parsed.name+"] does not exist."};
	endif

	if ( Loot_ChanceCheck(parsed.chance) )
		var items := GetConfigStringArray(groups_elem, "Item");
		var num_items := items.Size();
		var i;
		for ( i:=1; i<=parsed.amount; i+=1 )
			Loot_CreateItem(container, items[RandomInt(num_items)+1]);
			SleepMS(2);
		endfor
	endif

	return 1;
endfunction

function Loot_ChanceCheck(chance)
	if ( RandomInt(1000)+1 <= CInt(CDbl(chance)*10.0) )
		return 1;
	else
		return 0;
	endif
endfunction

function Loot_ParseString(line)
	line := SplitWords(line);

	var name := line[1];
	var objtype;
	if ( GetObjTypeByName(name) )
		objtype := GetObjTypeByName(name);
	else
		objtype := CInt(name);
	endif

	var amount := CStr(line[2]);
	if ( !amount )
		amount := 1;
	else
		amount := RandomDiceRoll(amount);
	endif

	var chance := CDbl(line[3]);
	if ( !chance )
		chance := 100.0;
	endif

	var result := struct;
	result.+name := name;
	result.+objtype := objtype;
	result.+amount := amount;
	result.+chance := chance;

	return result;
endfunction
