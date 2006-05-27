/*===============================================================
* Current Version
* NPCUTIL.INC - v1.0
* Updated 9/25/2005 2:26PM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/
use uo;
use os;
use util;
use cfgfile;

include ":brainAI:npcUtil";
include ":brainAI:npcError";


/////////////////////////////////////////////////////////////////
// * Function List *
//
// NPC_SetupAmmo(mobile)
// NPC_SetupBackPack(mobile)
// NPC_SetupEquipment(mobile, elem_name:=0)
// NPC_SetupName(mobile, name_elem:=0)
// NPC_SetupRandomEquipment(mobile, elem_name:=0, flags:=EQUIP_NOGENDER)
//
/////////////////////////////////////////////////////////////////
// * Internal/Private Function List *
//
/////////////////////////////////////////////////////////////////
// * Global Variables *
//
// Constants for NPC_SetupRandomEquipment()
CONST EQUIP_NOGENDER	:= 0x0;	// Items suited for both genders
CONST EQUIP_MALE	:= 0x1; // Use male-only items (do any exist?)
CONST EQUIP_FEMALE	:= 0x2; // Use female-only items
CONST EQUIP_BOTHGENDER	:= 0x3; // Cross-dresser
//
/////////////////////////////////////////////////////////////////


/*
 * NPC_SetupBackPack(mobile)
 *
 * Purpose
 * Creates a backpack and places it on an npc. If it can't equip it, it removes it.
 *
 * Parameters
 * mobile:	Mobile to equip the backpack onto.
 *
 * Return value
 * Returns an item reference on success or if there already is one.
 *
 */
function NPC_SetupBackPack(mobile)
	if ( !mobile.backpack )
		var backpack := CreateItemAtLocation(mobile.x, mobile.y, mobile.z, "BackPack", 1);
		var result := EquipItem(mobile, backpack);
		if ( !result )
			DestroyItem(backpack);
			return result;
		else
			return backpack;
		endif
	endif

	return mobile.backpack;
endfunction

/*
 * NPC_SetupAmmunition(mobile)
 *
 * Purpose
 * Creates ammunition in the npc's pack based on its 'AmmoType' and 'AmmoAmount'
 * settings in its npcdesc.cfg template.
 *
 * mobile:	The npc that will be given the ammunition.
 *
 * Return value
 * Returns 1
 *
 */
function NPC_SetupAmmo(mobile)
	var npc_elem := NPC_GetNPCConfig(mobile);

	if ( npc_elem.MissileWeapon )
		var weapon := CreateItemAtLocation(mobile.x, mobile.y, mobile.z, npc_elem.MissileWeapon, 1, mobile.realm);
		if ( !EquipItem(mobile, weapon) )
			DestroyItem(weapon);
		endif
	endif

	if ( npc_elem.AmmoType )
		if ( CInt(npc_elem.AmmoAmount) > 0 )
			var ammo := CreateItemInBackpack(mobile, npc_elem.AmmoType, CInt(npc_elem.AmmoAmount));
			if ( !ammo )
				return ammo; // Will return an error with .errortext
			endif
			ammo.newbie := 1;

			return 1;
		endif
	endif
	
	return 1;
endfunction

/*
 * NPC_SetupEquipment(mobile, elem_name)
 *
 * Purpose
 * Equips an NPC with EquipFromTemplate() and ::equip.cfg
 *
 * Parameters
 * mobile:	The mobile to place equipment onto
 * elem_name:	Name of the equipment template to use.
 *		If no name is set, it will use the 'EquipTemplate' line in the npc's template.
 *
 * Return value
 * Returns 1 or error
 *
 */
function NPC_SetupEquipment(mobile, elem_name:=0)
	var equip_cfg := ReadConfigFile("::equip");

	if ( !elem_name )
		var npc_elem := NPC_GetNPCConfig(mobile);
		elem_name := npc_elem.EquipTemplate;
	endif

	if ( !elem_name )
		return NPC_SetupBackPack(mobile);
	elseif ( !equip_cfg[elem_name] )
		return error{"errortext":="Error::NPC_SetupEquipment() - There is no elem ["+elem_name+"] in '::equip.cfg'"};
	else
		return EquipFromTemplate(mobile, elem_name);
	endif

	return 0;
endfunction

/*
 * NPC_SetupRandomEquipment(mobile, elem_name, flags)
 *
 * Purpose
 * Equips an NPC randomy-yet-still-fashionably.
 *
 * Parameters
 * mobile:	The mobile to place equipment onto
 * elem_name:	Name of the equipment template to use.
 *		If no name is set, it will use the 'EquipTemplate' line in the npc's template.
 * flags:	Flags to control which lines to use in equipGroups.cfg
 *		EQUIP_NOGENDER		- 'Item' lines that work for both genders (this list is used with all flags).
 *		EQUIP_MALE		- 'Male' lines for male-only equipment.
 *		EQUIP_FEMALE		- 'Female' lines for female-only equipment.
 *		EQUIP_BOTHGENDER	- Uses all entries to allow for cross dressing npcs.
 *
 * Return value
 * Returns 1 or error
 *
 */
function NPC_SetupRandomEquipment(mobile, elem_name:=0, flags:=EQUIP_NOGENDER)
	var index_cfg := ReadConfigFile(":brainAI:config/equipIndex");
	var group_cfg := ReadConfigFile(":brainAI:config/equipGroups");
	if ( index_cfg.errortext )
		return error{"errortext":="Could not open equipIndex.cfg -> "+index_cfg.errortext};
	elseif ( group_cfg.errortext )
		return error{"errortext":="Could not open equipGroups.cfg -> "+group_cfg.errortext};
	elseif ( !elem_name )
		var npc_elem := NPC_GetNPCConfig(mobile);
		elem_name := npc_elem.EquipTemplate;
	endif
	
	var index_elem := index_cfg[elem_Name];
	if ( !index_elem )
		return error{"errortext":="equipIndex.cfg elem ["+index_elem+"] does not exist."};
	endif

	foreach entry in ( GetConfigStringDictionary(index_elem, "Equip") )
		if ( RandomInt(100)+1 <= CInt(entry) )
			var group_elem := group_cfg[CStr(_entry_iter)];
			if ( !group_elem )
				PrintTextAbove(mobile, "equipGroups.cfg elem ["+_entry_iter+"] does not exist.");
				continue;
			endif

			var item_list := array{};
			if ( !flags )
				item_list := item_list + GetConfigStringArray(group_elem, "Item");
			endif
			if ( flags & EQUIP_MALE )
				item_list := item_list + GetConfigStringArray(group_elem, "Male");
			endif
			if ( flags & EQUIP_FEMALE )
				item_list := item_list + GetConfigStringArray(group_elem, "Female");
			endif

			var item := item_list[RandomInt(item_list.Size())+1];
			item := CreateItemAtLocation(mobile.x, mobile.y, mobile.z, item, 1, mobile.realm);
			item.movable := 1;
			if ( item.errortext )
				PrintTextAbove(mobile, "Unable to create ["+item+"] ->"+item.errortext);
			elseif ( !EquipItem(mobile, item) )
				PrintTextAbove(mobile, "Unable to equip - "+item.desc+" - destroying.");
				DestroyItem(item);
			endif
		endif
		SleepMS(2);
	endforeach

	return 1;
endfunction

/*
 * NPC_SetupName(mobile, name_elem)
 *
 * Purpose
 * Replaces every instance of <random> in an npc's name with a valid name using ::names.cfg
 *
 * Parameters
 * mobile:	The mobile to do a name check on.
 * name_elem:	Name elem in names.cfg to use
 *		If no name is set, it will use the 'NameList' line in the npc's template.
 *
 * Return value
 * Returns 1 or error
 *
 */
function NPC_SetupName(mobile, name_elem:=0)
	if ( !mobile.name["<random>"] )
		return 1;
	elseif ( !name_elem )
		var npc_elem := NPC_GetNPCConfig(mobile);
		name_elem := npc_elem.NameTemplate;
		if ( name_elem.errortext )
			return name_elem;
		endif
	endif

	var npc_names := ReadConfigFile(":brainAI:config/names");
	if ( !npc_names[name_elem] )
		return error{"errortext" := "There is no elem ["+name_elem+"] in NAMES.CFG"};
	endif

	var name_list := GetConfigStringArray(npc_names[name_elem], "Name");
	if ( name_list.Size() < 1 )
		return error{"errortext":="No names were found in the ["+name_elem+"] elem for names.cfg"};
	endif

	var my_name := mobile.name;
	while ( my_name["<random>"] )
		var replacement := name_list[RandomInt(name_list.Size())+1];
		my_name["<random>"] := replacement;
		SleepMS(2);
	endwhile

	SetName(mobile, my_name);

	return my_name;
endfunction
