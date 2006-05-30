use uo;
use os;
use util;
use cfgfile;

include ":attributes:attributes";
include ":magery:include/settings";
include "include/client";

const MAX_RANGE	:= 16;

/*
 * MS_GetSpellsCfgFile()
 *
 * Purpose
 * Gets the spells.cfg file from the magery package.
 *
 * Parameters
 *
 * Return value
 * Config file
 *
 */
function MS_GetSpellsCfgFile()
	var cfg := ReadConfigFile(":magery:spells");

	if ( cfg.errortext )
		SysLog("Error::MS_GetSpellsCfgFile() - Unable to open [:magery:spells.cfg] ->"+cfg.errortext);
	endif

	return cfg;
endfunction

/*
 * MS_GetSpellsCfgElem(elem_name, cfg_file)
 *
 * Purpose
 * Retrieves an elem from a config file.
 *
 * Parameters
 * elem_name:	A string matching the elem name to be retrieved.
 * cfg_file:	Optional parameter - reference to a config already read in by MS_GetSpellsCfgFile()
 *
 * Return value
 * A config file elem reference.
 *
 */
function MS_GetSpellsCfgElem(elem_name, byref cfg_file:=0)
	if ( !cfg_file )
		cfg_file := MS_GetSpellsCfgFile();
	endif

	var elem := cfg_file[elem_name];

	if ( elem.errortext )
		SysLog("Error::MS_GetSpellsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext);
	endif

	return elem;
endfunction

/*
 * MS_SpellDebug(who, text)
 *
 * Purpose
 * Sends debug information to mobiles with a command level.
 *
 * Parameters
 * who:		Mobile that will see the debug message.
 * text:	Text to show.
 *
 * Return value
 * Returns 1 if the text was sent.
 * Returns 0 if the text was not sent.
 */
function MS_SpellDebug(who, text)
	if ( who.cmdlevel )
		SendSysMessage(who, ""+text);
		return 1;
	endif
	return 0;
endfunction

/*
 * MS_Target(who, targ, prompt, flags)
 *
 * Purpose
 * Used to handle targetting of other objects in spell scripts.
 *
 * Parameters
 * who:		Mobile that is doing the targetting
 * targ:	Pre-selected target (for NPCs)
 * prompt:	Message the user receives when targeting.
 * flags:	uo.em targeting flags.
 *
 * Return value
 * Returns an object reference to what was targeted.
 *
 */
function MS_Target(byref who, byref targ:=0, byref prompt := "Select a target.", byref flags:=TGTOPT_CHECK_LOS+TGTOPT_NEUTRAL)
	who.hidden := 0;

	if ( targ.IsA(POLCLASS_UOBJECT) )
		return targ;
	elseif ( who.npctemplate )
		return 0;
	endif

	var orig_x := who.x;
	var orig_y := who.y;

	SendSysMessage(who, prompt);

	var elem := MS_GetSettingsCfgElem("Settings");
	var max_range := elem.MaxRange;

	targ := Target(who, flags);
	if ( Distance(who, targ) > MAX_RANGE )
		SendSysMessage(who, "That is too far away.");
		return 0;
	elseif ( who.x != orig_x || who.y != orig_y )
		SendSysMessage(who, "You break your concentration.");
		return 0;
	else
		return targ;
	endif
endfunction

/*
 * MS_TargetCoordinates(who, targ, prompt)
 *
 * Purpose
 * Used to handle targetting of other objects in spell scripts.
 *
 * Parameters
 * who:		Mobile that is doing the targetting
 * targ:	Pre-selected target (for NPCs)
 * prompt:	Message the user receives when targeting.
 *
 * Return value
 * Returns an object reference to what was targeted.
 *
 */
function MS_TargetCoordinates(byref who, byref targ:=0, byref prompt := "Select a target.")
	who.hidden := 0;

	if ( targ )
		return targ;
	elseif ( who.npctemplate )
		return 0;
	endif

	var orig_x := who.x;
	var orig_y := who.y;

	SendSysMessage(who, prompt);
	targ := TargetCoordinates(who);

	if ( !targ )
		SendSysMessage(who, "Cancelled.");
		return 0;
	elseif ( CoordinateDistance(who.x, who.y, targ.x, targ.y) > MAX_RANGE )
		SendSysMessage(who, "That is too far away.");
		return 0;
	elseif ( who.x != orig_x || who.y != orig_y )
		SendSysMessage(who, "You lose concentration.");
		return 0;
	else
		return targ;
	endif
endfunction

/*
 * MS_AdjustSpellPoints(mage, targ, points)
 *
 * Purpose
 * Adjusts the effectiveness of a spell based on the target's magic resistance.
 *
 * Parameters
 * caster:	Who is casting the spells.
 * targ:	Who the spell is effecting.
 * points:	Strength of the spell.
 *
 * Return value
 * Integer - adjusted point
 *
 * Notes:
 * Magic resistance does not work like this since AoS. This function remains
 * just in case someone should want to use it.
 *
 */
function MS_AdjustSpellPoints(mage, targ, points)
	var mage_skill := AP_GetSkill(mage, MAGERY);
	var defense := AP_GetSkill(targ, RESISTING_SPELLS);

	// Failed MR check results in only half your resistance being used!
	if ( SkillCheck(targ, RESISTING_SPELLS, -1) < 0 )
		defense := CInt(defense / 2);
	endif

	// Old formulas
	// CInt(points * (mage_skill - defense) / 100);
	// CInt(points * ((mage_skill * 1.3) - defense) / ( mage_skill * 1.3 ))
	var effect_level := CInt(points * (mage_skill-defense) / mage_skill );
	return effect_level;
endfunction

/*
 * MS_ReleaseScript(pid)
 *
 * Purpose
 * Informs the spell starter that the caster can cast another spell.
 * This allows the spell script to continue running without blocking future castings.
 *
 * Parameters
 * pid:	PId to the spell starter that started the spell.
 *
 * Return value
 * Returns 1
 *
 */
function ReleaseScript(pid)
	var process := GetProcess(pid);
	process.SendEvent("done");

	return 1;
endfunction

/*
 * MS_GetSpellDamage(character, targ, base_damage, dmg_type)
 *
 * Purpose
 *   Calculates the damage of a spell using the following logic
 *   Eval. Int. Bonus: ((EvalInt * 3) / 100) + 1
 *   Intelligence Bonus: (INT / 10) (Drop decimals from result)
 *   GM Inscription Bonus: 10% Damage Bonus
 *   Spell Damage Increase: Int. Bonus + Inscr. Bonus + Spell Damage Increase Items
 *   Total: round((Base Damage * Eval. Int. Bonus) * ((Spell Damage Increase / 100) + 1))
 *
 * Parameters
 *   character:		character object refrence of spell caster
 *   target:		npc object refrence of target
 *   base_damage:	randomly determined base damage for the spell
 *
 * Note:
 *   base_damage is a double
 *
 * Returns
 *   integer of total damage
 *
 */
function MS_GetSpellDamage(byref character, targ, base_damage)
	var damage_increase := CInt(AP_GetStat(character, "Intelligence") / 10);
    
    if (AP_GetSkill(character, INSCRIPTION) == 100)
      damage_increase := damage_increase + CInt((damage_increase / 10));
    endif
    
    // Get SDI items percentage (capped at 15% for pvp)
	// damage_increase := damage_increase + MS_GetSDIPercentage(character);
	
	var total_damage := CInt(base_damage * ((AP_GetSkill(character, EVALUATING_INTELLIGENCE) * 3) / 100 + 1) * ((damage_increase / 100) + 1));
	return total_damage;
endfunction

/*
 * MS_MobileCheck(character, targ)
 *
 * Purpose
 *   Checks if the target is a mobile and provides a centralized location for failure messages.
 *
 * Returns
 *   1 on success
 *
*/
function MS_MobileCheck(byref character, byref targ)
	if (!targ.IsA(POLCLASS_MOBILE))
		SendSysMessage(character, "This spell won't work on that!");
		return 0;
	endif
	
	return 1;
endfunction
