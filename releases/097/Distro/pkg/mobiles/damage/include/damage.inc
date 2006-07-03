/* $Id$
 *
 * Purpose
 * Handles damages and resistances to damage types
 *
 */

use uo;
use os;
use math;
use cfgfile;

include ":damage:settings";
include ":damage:report";
include ":brainAI:npcUtil";
include ":attributes:attributes";

/*
 * http://uo.stratics.com/content/guides/resistance.shtml
 * These could be attributes, but not every shard will want to
 * follow OSI. This gives more freedom to make custom resistances
 * and remove existing ones more easily.
 */
enum DAMAGE_TYPES
	DMG_FORCED	:= -1, // Ignores resistance
	DMG_PHYSICAL	:= "Physical",
	DMG_FIRE	:= "Fire",
	DMG_COLD	:= "Cold",
	DMG_POISON	:= "Poison",
	DMG_ENERGY	:= "Energy"
endenum

/*
 * ApplyDamageEX(mobile, amount, type, source)
 *
 * Purpose
 *
 * Parameters
 * mobile:
 * amount:
 * type:
 * source:
 *
 * Return value
 *
 */
function ApplyDamageEX(mobile, amount, type:=DMG_FORCED, source:=0)
	amount := CInt(amount);
	amount := CalcDamageAmount(mobile, amount, type);

	if ( amount == error )
		return 0;
	elseif ( amount < 0 )
		HealDamage(mobile, Abs(amount));
	else
		if ( source )
			SetLastDamageInfo(mobile, amount, type, source);
			SetScriptController(source);
		endif
		ApplyDamage(mobile, amount);
	endif

	return amount;
endfunction

/*
 * ApplyRawDamageEX(mobile, amount, type, source)
 *
 * Purpose
 *
 * Parameters
 * mobile:
 * amount:
 * type:
 * source:
 *
 * Return value
 *
 */
function ApplyRawDamageEX(mobile, amount, type:=DMG_FORCED, source:=0)
	amount := CInt(amount);
	amount := CalcDamageAmount(mobile, amount, type);

	if ( amount == error )
		return 0;
	elseif ( amount < 0 )
		HealDamage(mobile, Abs(amount));
	else
		if ( source )
			SetLastDamageInfo(mobile, amount, type, source);
			SetScriptController(source);
		endif
		ApplyRawDamage(mobile, amount);
	endif

	return amount;
endfunction

/*
 * CalcDamageAmount(mobile, amount, type)
 *
 * Purpose
 * Calculates the amount of damage based on the damage type and resistance.
 *
 * Parameters
 * mobile:	Mobile that is being damaged and to check resistances on.
 * amount:	Amount of base damage to deal.
 * type:	Type of damage that is being dealt.
 *
 * Return value
 * Integer
 *
 */
function CalcDamageAmount(mobile, amount, type)
	amount := CInt(amount);
	if ( type == DMG_FORCED )
		return amount;
	endif

	var resistance := CDbl(GetResistance(mobile, type));
	if ( resistance == error )
		// Do nothing, there is no resistance.
	else
		// Calculate percent that was resisted.
		// Examples:
		// Damage = 50
		// [ Resistance = 10 so Resisted = 5, Amount = 45 ]
		// [ Resistance = 100 so Resisted = 50, Amount = 0 ]
		// [ Resistance = -100 so Resisted = -50, Amount = -50 ]
		// [ Resistance = 47.3 so Resisted = 23.65, Amount = 26.35 ]
		var resisted := (resistance * 0.010) * CDbl(amount);
		amount -= resisted;
	endif

	return CInt(amount);
endfunction

/*
 * SetLastDamageInfo(mobile, amount, type, source)
 *
 * Purpose
 *
 * Parameters
 * mobile:
 * amount:
 * type:
 * source:
 *
 * Return value
 *
 */
function SetLastDamageInfo(mobile, amount, type, source)
	var last_damage := struct;
	last_damage.+serial	:= source.serial;
	last_damage.+time	:= POLCore().systime;
	last_damage.+amount	:= amount;
	last_damage.+type	:= type;

	if ( last_damage.acctname )
		last_damage.+acctname := source.acctname;
	endif
	if ( source.name )
		last_damage.+name := source.name;
	elseif ( source.IsA(POLCLASS_ITEM) )
		last_damage.+name := source.desc;
	endif

	SetObjProperty(mobile, "LastDamage", last_damage);

	return last_damage;
endfunction

/*
 * GetLastDamageInfo(mobile)
 *
 * Purpose
 *
 * Parameters
 * mobile:
 *
 * Return value
 *
 */
function GetLastDamageInfo(mobile)
	return GetObjProperty(mobile, "LastDamage");
endfunction

/*
 * GetResistanceNames()
 *
 * Purpose
 * Retrieves a list of resistance names from settings.cfg
 *
 * Parameters
 *
 * Return Value
 * Returns an array of strings
 *
 */
function GetResistanceNames()
	var cfg_elem := DMG_GetSettingsCfgElem("DamageTypes");

	return GetConfigStringArray(cfg_elem, "Type");
endfunction

/*
 * GetResistance(mobile, type)
 *
 * Purpose
 * Retrieves the resistances list for a mobile, taking into consideration
 * the magic resist skill.
 *
 *
 * Parameters
 * mobile:	Mobile to retrieve the resistances for.
 * type:	Optional argument.
 *		If 0 or blank, will return a dictionary of all resistances.
 *		If specified, will return a double for that value.
 *
 * Return value
 * dictionary or double
 *
 */
function GetResistance(mobile, type:=0)
	var resistances;
	if ( mobile.IsA(POLCLASS_NPC) )
		resistances := GetNPCResistance(mobile, type);
	else
		resistances := GetObjProperty(mobile, "%Resistances%");
	endif
	if ( Lower(TypeOf(resistances)) != "dictionary" )
		resistances := dictionary;
	endif

	if ( type )
		return resistances[type];
	else
		return resistances;
	endif
endfunction

/*
 * GetNPCResistance(template, type)
 *
 * Purpose
 * Retrieves base resistances from an npc's npcdesc.cfg
 *
 * Parameters
 * template:	Can take in a template name or an npc reference.
 * type:	Find a specific resistance value. If 0, will retrieve them all.
 *
 * Return Value
 * Returns a double or a dictionary.
 *
 */
function GetNPCResistance(template, type:=0)
	var cprop_resistances;

	if ( template.IsA(POLCLASS_NPC) )
		cprop_resistances := GetObjProperty(template, "%Resistances%");
		template := template.npctemplate;
	endif

	var npc_elem := NPC_GetNPCConfig(template);
	if ( type )
		if ( cprop_resistances.Exists(type) )
			return cprop_resistances[type];
		else
			return GetConfigReal(npc_elem, type+"-Resistance");
		endif
	else
		var resistances := dictionary;
		foreach name in ( GetResistanceNames() )
			if ( cprop_resistances.Exists(name) )
				resistances[name] := cprop_resistances[name];
			else
				resistances[name] := GetConfigReal(npc_elem, name+"-Resistance");
			endif
			SleepMS(2);
		endforeach
		return resistances;
	endif
endfunction

/*
 * SetResistance(mobile, name, value)
 *
 * Purpose
 * Sets a resistance value on a mobile.
 *
 * Parameters
 * mobile:	Mobile to set the resistance value for.
 * name:	Name of the resistance type to set.
 * value:	Value for the resistance.
 *
 * Return value
 * Returns result of SetObjProperty()
 *
 */
function SetResistance(mobile, name, value)
	var resistances := GetObjProperty(mobile, "%Resistances%");
	if ( Lower(TypeOf(resistances)) != "dictionary" )
		resistances := dictionary;
	endif

	resistances[name] := CDbl(value);

	return SetObjProperty(mobile, "Resistances", resistances);
endfunction

/*
 * ModifyResistance(mobile, name, value)
 * Purpose
 * Modifies a resistance value on a mobile.
 *
 * mobile:	Mobile to set the resistance value for.
 * name:	Name of the resistance type to change.
 * value:	Adjustment to the value.
 *
 * Return value
 * Returns result of SetObjProperty()
 *
 */
function ModifyResistance(mobile, name, value)
	var resistances := GetObjProperty(mobile, "%Resistances%");
	if ( Lower(TypeOf(resistances)) != "dictionary" )
		resistances := dictionary;
	endif

	var cur_value := CDbl(resistances[name]);
	value := cur_value + CDbl(value);

	resistances[name] := value;

	return SetObjProperty(mobile, "%Resistances%", resistances);
endfunction

/*
 * GetResistanceMods(mobile, type)
 *
 * Purpose
 * Retrieves the resistance mods list for a mobile. This prop is used
 * to store resistance modifications that must be applied AFTER
 * a mobile's base resist such as reactive armor and magic reflection
 *
 *
 * Parameters
 * mobile:	Mobile to retrieve the resistances for.
 * type:	Optional argument.
 *		If 0 or blank, will return a dictionary of all resistance mods.
 *		If specified, will return a double for that value.
 *
 * Return value
 * dictionary or double
 *
 */
function GetResistanceMods(mobile, type:=0)
	var resistance_mods := GetObjProperty(mobile, "%ResistanceMods%");
	if ( Lower(TypeOf(resistance_mods)) != "dictionary" )
		resistance_mods := dictionary;
	endif

	if ( !type )
		return resistance_mods;
	elseif ( resistance_mods.Exists(type) )
		return CDbl(resistance_mods[type]);
	else
		return 1.0;
	endif
endfunction

/*
 * SetResistanceMods(mobile, name, value)
 *
 * Purpose
 * Sets a resistance modifier on a mobile.
 *
 * mobile:	Mobile to set the resistance mod value for.
 * name:	Name of the resistance mod type to set.
 * value:	Value for the resistance mod.
 *
 * Return value
 * Returns result of SetObjProperty()
 *
 */
function SetResistanceMod(mobile, name, value)
	var resistance_mods := GetResistanceMods(mobile);
	if ( Lower(TypeOf(resistance_mods)) != "dictionary" )
		resistance_mods := dictionary;
	endif

	resistance_mods[name] := CDbl(value);

	return SetObjProperty(mobile, "%ResistanceMods%", resistance_mods);
endfunction

/*
 * ModifyResistance(mobile, name, value)
 * Purpose
 * Modifies a resistance mod value on a mobile.
 *
 * mobile:	Mobile to set the resistance mod value for.
 * name:	Name of the resistance mod type to change.
 * value:	Adjustment to the value.
 *
 * Return value
 * Returns result of SetObjProperty()
 *
 */
function ModifyResistanceMod(mobile, name, value)
	var resistance_mods := GetResistanceMods(mobile);
	if ( Lower(TypeOf(resistance_mods)) != "dictionary" )
		resistance_mods := dictionary;
	endif

	var cur_value := CDbl(resistance_mods[name]);
	value := cur_value + CDbl(value);

	resistance_mods[name] := value;

	return SetObjProperty(mobile, "%ResistanceMods%", resistance_mods);
endfunction

/*
 * GetDamageTypes(mobile)
 *
 * Purpose
 * Retrieves the DamageTypes prop from the mobile if NPC, from the mobile.weapon if not
 * If no DamageTypes is found, returns all damage as physical
 *
 * mobile: mobile refrence
 *
 * Return value:
 * Dictionary with the type of damage and porcentage, eg.: "DMG_FIRE -> 0.5 DMG_COLD -> 0.5"
*/

function GetDamageTypes(mobile)
	var damage_types := dictionary;

	if ( mobile.npctemplate )
		damage_types := GetObjProperty(mobile, "DamageTypes");
	else
		damage_types := GetObjProperty(mobile.weapon, "DamageTypes");
	endif

	if ( !damage_types )
		damage_types := dictionary;
		damage_types[DMG_PHYSICAL] := 1.0;
	endif

	return damage_types;
endfunction
