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
 *---------------------------
 * Notes:
 * AOS deals with resistances as percentages. To keep POL more flexible, it uses multipliers. 
 * Stratics reports the max for any resistance type is 80% (80/100 = 0.8). 
 * % to Mult	(100-80)*0.010 = 0.2
 * Mult to %	100-(0.2*100) = 80
 *
 * Examples: 
 * NPC resists 25% (1/4) of cold damage.
 * Cold-ResistMult 0.75
 * (10 * 0.75 = 7.5) // Removes 2.5 points of damage. 
 *
 * NPC resists 50% (1/2) of cold damage.
 * Cold-ResistMult 0.5
 * (10 * 0.5 = 5) // Removes 5 points of damage.
 *
 * NPC resists 75% (3/4) of cold damage.
 * Cold-ResistMult 0.25
 * (10 * 0.25 = 2.5) // Removes 7.5 points of damage.
 *
 * NPC resists 80% (4/5 max AOS resitance)
 * Cold-ResistMult 0.80
 * (10 * 0.2 = 2) // Removes 8 points of damae.
 *
 * NPC is immune to fire damage
 * Fire-ResistMult 0.0
 * (10 * 0.0 = 0) Removes all points of damage.
 *
 * Energy damage actually heals
 * Energy-ResistMult -10.0
 * (10 * -10.0 = -10) Heals 10 points of damage.
 *
 * Physical damage is double
 * Physical-ResistMult 2.0
 * (10 * 2.0 = 20) Adds 10 points of damage.
 *
 */
function CalcDamageAmount(mobile, amount, type)
	amount := CInt(amount);
	if ( type == DMG_FORCED )
		return amount;
	endif
	
	var resistance_mult := GetResistance(mobile, type);	
	
	if ( resistance_mult == error )
		// Do nothing, there is no resistance.
	elseif ( resistance_mult == 0.0 )
		// Mobile is immune.
		amount := 0;
	elseif ( resistance_mult )
		// Mobile has a resistance multiplier
		amount := CInt(amount * resistance_mult);
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
	else
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

//
// Resistance Functions 
//

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
	var resistances := GetObjProperty(mobile, "Resistances");
	if ( Lower(TypeOf(resistances)) != "dictionary" )
		resistances := dictionary;
	endif
	
	if ( !type )
		var dmg_types := GetConfigStringDictionary(DMG_GetSettingsCfgElem("DamageTypes"), "Damage");
		var base_resist := ResistancePercentToMult(((AP_GetSkill(mobile, RESISTING_SPELLS) - 40) / 3) * 2); // from uo.stratics
		var max_resistances := GetConfigStringDictionary(DMG_GetSettingsCfgElem("Settings"), "MaxResistance");
		var resistance_mods := GetResistanceMods(mobile);
		foreach key in (dmg_types.Keys())
			// Set minimum resistance based on resisting spells (does NOT stack with anything else)
			if ( resistances[key] < base_resist )
				resistances[key] := base_resist;
			endif
			
			// Adjust resistances with resistance mods
			resistances[key] := resistances[key] + CDbl(resistance_mods[key]);
			
			// Cap resistance maximum as defined in settings.cfg
			if ( resistances[key] > max_resistances[key] )
				resistances[key] := max_resistances[key];
			endif
		endforeach

		return resistances;
	elseif ( resistances.Exists(type) )
		// Set minimum resistance based on magic resistances (does NOT stack with anything else)
		var base_resist := ResistancePercentToMult(((AP_GetSkill(mobile, RESISTING_SPELLS) - 40) / 3) * 2); // from uo.stratics
		if ( resistances[type] < base_resist )
			resistances[type] := base_resist;
		endif

		// Adjust resistance with resistance_mods
		var resistance_mods := GetResistanceMods(mobile, type);
		resistances[type] := resistances[type] + CDbl(resistance_mods[type]);

		var max_resistance := GetConfigStringDictionary(DMG_GetSettingsCfgElem("Settings"), "MaxResistance");		
		if ( resistances[type] > max_resistance[type] )
			resistances[type] := max_resistance[type];
		endif

		return CDbl(resistances[type]);
	elseif ( mobile.npctemplate )
		// Looks for type-ResistMult in the NPCDESC.CFG
		var npc_elem := NPC_GetNPCConfig(mobile);
		var mult := GetConfigReal(npc_elem, type+"-ResistMult");
		if (  mult != error )
			return CDbl(mult);
		else
			return mult;
		endif
	else
		return 1.0;
	endif
endfunction

/*
 * SetResistance(mobile, name, value)
 *
 * Purpose
 * Sets a resistance multiplier on a mobile.
 *
 * mobile:	Mobile to set the resistance multiplier for.
 * name:	Name of the resistance type to set.
 * value:	Multiplier for the resistance.
 *
 * Return value
 * Returns result of SetObjProperty()
 *
 */
function SetResistance(mobile, name, value)
	var resistances := GetObjProperty(mobile, "Resistances");
	if ( Lower(TypeOf(resistances)) != "dictionary" )
		resistances := dictionary;
	endif
	
	resistances[name] := CDbl(value);
	
	return SetObjProperty(mobile, "Resistances", resistances);
endfunction

/*
 * ModifyResistance(mobile, name, value)
 * Purpose
 * Modifies a resistance multiplier on a mobile.
 *
 * mobile:	Mobile to set the resistance multiplier for.
 * name:	Name of the resistance type to change.
 * value:	Adjustment to the multiplier.
 *
 * Return value
 * Returns result of SetObjProperty()
 *
 */
function ModifyResistance(mobile, name, value)
	var resistances := GetObjProperty(mobile, "Resistances");
	if ( Lower(TypeOf(resistances)) != "dictionary" )
		resistances := dictionary;
	endif
	
	var cur_value := CDbl(resistances[name]);
	value := cur_value + CDbl(value);
	
	resistances[name] := value;
	
	return SetObjProperty(mobile, "Resistances", resistances);
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
	var resistance_mods := GetObjProperty(mobile, "ResistanceMods");
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
 * Sets a resistance mod multiplier on a mobile.
 *
 * mobile:	Mobile to set the resistance mod multiplier for.
 * name:	Name of the resistance mod type to set.
 * value:	Multiplier for the resistance mod.
 *
 * Return value
 * Returns result of SetObjProperty()
 *
 */
function SetResistanceMod(mobile, name, value)
	var resistance_mods := GetObjProperty(mobile, "ResistanceMods");
	if ( Lower(TypeOf(resistance_mods)) != "dictionary" )
		resistance_mods := dictionary;
	endif
	
	resistance_mods[name] := CDbl(value);
	
	return SetObjProperty(mobile, "ResistanceMods", resistance_mods);
endfunction

/*
 * ModifyResistance(mobile, name, value)
 * Purpose
 * Modifies a resistance mod multiplier on a mobile.
 *
 * mobile:	Mobile to set the resistance mod multiplier for.
 * name:	Name of the resistance mod type to change.
 * value:	Adjustment to the multiplier.
 *
 * Return value
 * Returns result of SetObjProperty()
 *
 */
function ModifyResistanceMod(mobile, name, value)
	var resistance_mods := GetObjProperty(mobile, "ResistanceMods");
	if ( Lower(TypeOf(resistance_mods)) != "dictionary" )
		resistance_mods := dictionary;
	endif
	
	var cur_value := CDbl(resistance_mods[name]);
	value := cur_value + CDbl(value);
	
	resistance_mods[name] := value;
	
	return SetObjProperty(mobile, "ResistanceMods", resistance_mods);
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

/*
 * ResistancePercentToMult(percent)
 *
 * Purpose
 * Turns a percentage into a resistance multiplier.
 *
 * Parameters
 * percent
 *
 * Return value
 * Double resistance multiplier
 * 
 */
function ResistancePercentToMult(percent)
	if ( percent > 0 )
		return CDbl(100 - percent) * 0.010;
	else
		return CDbl(100 + percent) * 0.010;
	endif
endfunction

/*
 * ResistancePercentToMult(percent)
 *
 * Purpose
 * Turns a resistance multiplier into a percentage.
 *
 * Parameters
 * mult
 *
 * Return value
 * Int percentage
 * 
 */
function ResistanceMultToPercent(mult)
	if ( mult > 0 )
		return CInt(100 - (mult * 100));
	else
		return CInt(100 + (mult * 100));
	endif
endfunction
