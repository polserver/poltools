use uo;
use os;
use util;
use cfgfile;

include "include/client";
include "include/objtype";

var npc_cfg_file := ReadConfigFile (":*:npcdesc");
var chest_cfg_file := ReadConfigFile (":questingnpcs:chest_lootgroups");
var itemgroup_cfg_file := ReadConfigFile (":questingnpcs:itemgroups");

Const UOBJ_GOLD_COIN := 0xeed;

///////////////////
//  used to make loot on corpses.  Probably only called by death.src.  Lootgroups for NPCs
//  can be found in npc_lootgroups.cfg
///////////////////

function MakeLootOnCorpse (corpse, npctemplate)
	var elem := FindConfigElem (npc_cfg_file, npctemplate);
	if (!elem)
		return 0;
	endif

	if (elem.gold)
		var gold := RandomDiceRoll (elem.gold);
		CreateItemInContainer (corpse, UOBJ_GOLD_COIN, gold);
	endif

	if (elem.chest_chance)
		MakeLockedChest (corpse, elem.chest_chance, elem.chest_quality);
	endif

	foreach str in GetConfigStringArray (elem, "item")
		MakeSpecifiedLootItem (corpse, str);
	endforeach

	foreach str in GetConfigStringArray (elem, "random")
		MakeItemsOutOfGroup (corpse, str);
	endforeach

	var quality := elem.magic_quality;
	if (quality)
		foreach str in GetConfigStringArray (elem, "magic_group")
			var temp_str := SplitWords (str);
			var group := temp_str [1];
			var chance := CInt (temp_str [2]);
			if (RandomInt (1000) + 1 <= chance)
				MakeMagicItemOutOfGroup (corpse, group, quality);
			endif
		endforeach
	endif
	return 1;

endfunction




///////////////////
//  used to make loot in chests.  Lootgroups can be found in chest_lootgroups.cfg
///////////////////

function MakeLootInChest (chest, lootgroup)

	var elem := FindConfigElem (chest_cfg_file, lootgroup);
	if (!elem)
		return 0;
	endif

	if (elem.gold)
		var gold := RandomDiceRoll (elem.gold);
		CreateItemInContainer (chest, UOBJ_GOLD_COIN, gold);
	endif

	foreach str in GetConfigStringArray (elem, "item")
		MakeSpecifiedLootItem (chest, str);
	endforeach

	foreach str in GetConfigStringArray (elem, "random")
		MakeItemsOutOfGroup (chest, str);
	endforeach

	var quality := elem.magic_quality;
	if (quality)
		foreach str in GetConfigStringArray (elem, "magic_group")
			var temp_str := SplitWords (str);
			var group := temp_str [1];
			var chance := CInt (temp_str [2]);
			if (RandomInt (1000) + 1 <= chance)
				MakeMagicItemOutOfGroup (chest, group, quality);
			endif
		endforeach
	endif
	return 1;

endfunction




///////////////////
//  creates specific item in container
///////////////////

function MakeSpecifiedLootItem (container, str)
	str := SplitWords (str);
	var itemname := str[1];
	var number := RandomDiceRoll (str[2]);
	var chance := CInt(str[3]);

	if (number > 20)
		number := 20;
	endif

	for i := 1 to number
		if (RandomInt(100) + 1 <= chance)
			var item := CreateItemInContainer (container, itemname, 1);
			item.movable := 1;
		endif
	endfor
	return 1;

endfunction




///////////////////
//  creates random items out of a group of possible items
///////////////////

function MakeItemsOutOfGroup (container, str)
	str := SplitWords (str);
	var groupname := str[1];
	var number := RandomDiceRoll (str[2]);
	var chance := CInt(str[3]);

	if (number > 100)
		number := 100;
	endif

	var elem := FindConfigElem (itemgroup_cfg_file, groupname);
	if (!elem)
		return 0;
	endif

	var possible_items_array := GetConfigStringArray (elem, "item");
	var total_items := len (possible_items_array);
	var item, itemtype, itemnumber;
	for i := 1 to number
		if (RandomInt (100) + 1 <= chance)
			itemnumber := RandomInt (total_items) + 1;
			itemtype := possible_items_array [itemnumber];
			if (CINT (itemtype))
				itemtype := CINT (itemtype);
			endif
			item := CreateItemInContainer (container, itemtype, 1);
			item.movable := 1;
		endif
	endfor
	return item;

endfunction




///////////////////
//  makes a magic item out of the group specified
///////////////////

function MakeMagicItemOutOfGroup (container, str, quality)

	var elem := FindConfigElem (itemgroup_cfg_file, str);
	if (!elem)
		return 0;
	endif

	var possible_items_array := GetConfigStringArray (elem, "item");
	var total_items := len (possible_items_array);
	var itemnumber := RandomInt (total_items) + 1;
	var itemtype := possible_items_array [itemnumber];

	var item := CreateItemInContainer (container, itemtype, 1);
	item.movable := 1;

	var parms := {};
	parms[1] := item;
	parms[2] := CInt (quality);

	run_script_to_completion (":loot:makeitemmagic", parms);
	return item;

endfunction




///////////////////
//  makes a locked chest that, when picked, spawns loot
///////////////////

function MakeLockedChest (container, chest_chance, chest_quality)

	if (chest_chance < RandomInt (100))
		return 0;
	endif

	var base_quality := CINT(chest_quality/2) + RandomInt(CINT(chest_quality/2));
	var random_quality := CINT (((RandomInt (10)+1) * (RandomInt (10)+1))/10) + 1;
	if (random_quality > base_quality)
		base_quality := random_quality;
	endif

	if (base_quality > 10)
		base_quality := 10;
	endif

	var chest := CreateItemInContainer (container, 0x7083, 1);
	if (!chest)
		return 0;
	endif

	SetObjProperty (chest, "mobchest_spawntime", ReadGameClock ());
	SetObjProperty (chest, "lockpicking", base_quality);
	SetObjProperty (chest, "questitem", 1);
	chest.locked := 1;
	chest.movable := 1;
	chest.name := "a locked chest";

	//make traps
	if (RandomInt (3) = 1)
		case (RandomInt (3))
			0:
				SetObjProperty (chest, "trap", "explosion");
			1:
				SetObjProperty (chest, "trap", "poison");
			2:
				SetObjProperty (chest, "trap", "djinni");
		endcase
	endif

	return 1;

endfunction
