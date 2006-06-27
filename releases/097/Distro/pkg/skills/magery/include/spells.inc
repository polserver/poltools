use uo;
use os;
use util;
use cfgfile;

include ":attributes:attributes";
include ":magery:settings";
include ":magery:spells_ex";
include ":magery:report";
include "include/client";

CONST MAX_RANGE	:= 16;

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
		MS_ReportText("Error::MS_GetSpellsCfgFile() - Unable to open [:magery:spells.cfg] ->"+cfg.errortext, MS_REPORT_SYSLOG);
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
		MS_ReportText("Error::MS_GetSpellsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext, MS_REPORT_SYSLOG);
	endif

	return elem;
endfunction

/*
 * MS_SpellDebug(mobile, text)
 *
 * Purpose
 * Sends debug information to mobiles with a command level.
 *
 * Parameters
 * mobile:		Mobile that will see the debug message.
 * text:	Text to show.
 *
 * Return value
 * Returns 1 if the text was sent.
 * Returns 0 if the text was not sent.
 */
function MS_SpellDebug(mobile, text)
	if ( mobile.cmdlevel )
		SendSysMessage(mobile, ""+text);
		return 1;
	endif
	return 0;
endfunction

/*
 * MS_Target(mobile, targ, prompt, flags)
 *
 * Purpose
 * Used to handle targetting of other objects in spell scripts.
 *
 * Parameters
 * mobile:		Mobile that is doing the targetting
 * targ:	Pre-selected target (for NPCs)
 * prompt:	Message the user receives when targeting.
 * flags:	uo.em targeting flags.
 *
 * Return value
 * Returns an object reference to what was targeted.
 *
 */
function MS_Target(byref mobile, byref targ:=0, byref prompt := "Select a target.", byref flags:=TGTOPT_CHECK_LOS+TGTOPT_NEUTRAL)
	mobile.hidden := 0;

	if ( targ.IsA(POLCLASS_UOBJECT) )
		return targ;
	elseif ( mobile.npctemplate )
		return 0;
	endif

	var orig_x := mobile.x;
	var orig_y := mobile.y;

	SendSysMessage(mobile, prompt);

	var elem := MS_GetSettingsCfgElem("Settings");
	var max_range := elem.MaxRange;

	targ := Target(mobile, flags);
	if ( Distance(mobile, targ) > MAX_RANGE )
		SendSysMessage(mobile, "That is too far away.");
		return 0;
	elseif ( mobile.x != orig_x || mobile.y != orig_y )
		SendSysMessage(mobile, "You break your concentration.");
		return 0;
	else
		return targ;
	endif
endfunction

/*
 * MS_TargetCoordinates(mobile, targ, prompt)
 *
 * Purpose
 * Used to handle targetting of other objects in spell scripts.
 *
 * Parameters
 * mobile:		Mobile that is doing the targetting
 * targ:	Pre-selected target (for NPCs)
 * prompt:	Message the user receives when targeting.
 *
 * Return value
 * Returns an object reference to what was targeted.
 *
 */
function MS_TargetCoordinates(byref mobile, byref targ:=0, byref prompt := "Select a target.")
	mobile.hidden := 0;

	if ( targ )
		return targ;
	elseif ( mobile.npctemplate )
		return 0;
	endif

	var orig_x := mobile.x;
	var orig_y := mobile.y;

	SendSysMessage(mobile, prompt);
	targ := TargetCoordinates(mobile);

	if ( !targ )
		SendSysMessage(mobile, "Cancelled.");
		return 0;
	elseif ( CoordinateDistance(mobile.x, mobile.y, targ.x, targ.y) > MAX_RANGE )
		SendSysMessage(mobile, "That is too far away.");
		return 0;
	elseif ( mobile.x != orig_x || mobile.y != orig_y )
		SendSysMessage(mobile, "You lose concentration.");
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
 * caster:	mobile is casting the spells.
 * targ:	mobile the spell is effecting.
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
	// TODO: fix this function -> http://guide.uo.com/skill_26.html
	
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
 * MS_GetSpellDamage(mobile, spell_id)
 *
 * Purpose
 *   Calculates spell damage using base damage defined in spells.cfg and
 *   formula provided by stratics.
 *   Eval. Int. Bonus: ((EvalInt * 3) / 100) + 1
 *   Intelligence Bonus: (INT / 10) (Drop decimals from result)
 *   GM Inscription Bonus: 10% Damage Bonus
 *   Spell Damage Increase: Int. Bonus + Inscr. Bonus + Spell Damage Increase Items
 *   Total: round((Base Damage * Eval. Int. Bonus) * ((Spell Damage Increase / 100) + 1))
 *
 * Parameters
 *   mobile:		mobile refrence of caster
 *   spelli_id:		id of spell
 *
 * Returns
 *   dictionary of total damage and damage type
 *
 */
function MS_GetSpellDamage(mobile, spell_id)
	var info := MS_GetSpellInfo(spell_id);
	
	if ( info == error )
		return info;
	endif
	
	var random_damage := CDbl(RandomInt((info.max_damage - info.min_damage) * 10)) / 10;
	var base_damage := info.min_damage + random_damage;

	MS_SpellDebug(mobile, "min: "+info.min_damage+" max: "+info.max_damage+" rnd: "+random_damage);

	var eval_int_bonus := ((AP_GetSkill(mobile, EVALUATING_INTELLIGENCE) * 3) / 100) + 1;
	var intel_bonus := CInt(AP_GetStat(mobile, "Intelligence") / 10);

	// TODO: Get SDI items percentage (capped at 15% for pvp)
	var damage_increase := intel_bonus;
    
    if ( AP_GetSkill(mobile, INSCRIPTION) >= 100 )
      damage_increase := damage_increase + 10;
    endif
    
	var total_damage := CInt(base_damage * eval_int_bonus * ((damage_increase / 100) + 1));
	return dictionary{"damage"->total_damage, "type"->info.type};
endfunction

/*
 * MS_MobileCheck(mobile, targ)
 *
 * Purpose
 *   Checks if the target is a mobile and provides a central
 *   location for failure messages.
 *
 * Returns
 *   1 on success
 *
*/
function MS_MobileCheck(mobile, targ)
	if ( !targ.IsA(POLCLASS_MOBILE) )
		SendSysMessage(mobile, "This spell won't work on that!");
		return 0;
	endif
	
	return 1;
endfunction

/*
 * MS_GetSpellInfo(mobile, targ)
 *
 * Purpose
 *   Retrieves minimum and maximum damge and damage type from all spells.cfg
 *
 * Returns
 *   Dictionary with minimum and maximum damage and damage type
 *
*/
function MS_GetSpellInfo(spell_id)
	var cfg_file := ReadConfigFile(":*:spells.cfg");
	var elem := cfg_file[spell_id];

	if ( elem == error )
		MS_ReportText("Error: Could not find cfg elem for spell_id: "+spell_id+".");
		return {"errortext":="Could not find cfg elem for spell_id: "+spell_id+"."};
	endif
	
	var info := dictionary{"min_damage", "max_damage", "type"};
	info.min_damage := GetConfigReal(elem, "MinBaseDamage");
	info.max_damage := GetConfigReal(elem, "MaxBaseDamage");
	info.type := GetConfigString(elem, "DamageType;");

	if ( info.min_damage == error )
		MS_ReportText("Error: No minimum base damage defined for spell_id: "+spell_id+". Using: 0.");
		info.min_damage := 0;
	endif

	if ( info.max_damage == error )
		MS_ReportText("Error: No maximum base damage defined for spell_id: "+spell_id+". Using: 0.");
		info.max_damage := 1;
	endif

	if ( info.min_damage > info.max_damage )
		MS_ReportText("Error: Minimum base damage > maximum base damage for spell_id: "+spell_id+". Using: 0.");
		info.min_damage := 0;
		info.max_damage := 0;
	endif

	if ( info.type == error )
		MS_ReportText("Error: No damage type defined for spell_id: "+spell_id+".");
		return error{"errortext":="No damage type defined for spell_id: "+spell_id+"."};
	endif
	
	return info;
endfunction