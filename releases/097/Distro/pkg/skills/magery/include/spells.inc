// $Id$

use uo;
use os;
use util;
use cfgfile;

include "include/client";
include "include/sounds";
include ":magery:settings";
include ":magery:spells_ex";
include ":magery:report";
include ":attributes:attributes";


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
function MS_Target(mobile, targ:=0, prompt:="Select a target.", flags:=TGTOPT_CHECK_LOS + TGTOPT_NEUTRAL)
	mobile.hidden := 0;

	if ( targ.IsA(POLCLASS_UOBJECT) )
		return targ;
	elseif ( mobile.npctemplate )
		return 0;
	endif

	var orig_x := mobile.x;
	var orig_y := mobile.y;
	var orig_realm := mobile.realm;
	var settings_elem := MS_GetSettingsCfgElem("Settings");
	var allow_precast := CInt(settings_elem.AllowPrecasting);

	SendSysMessage(mobile, prompt);
	
	targ := Target(mobile, flags);

	if ( Distance(mobile, targ) > settings_elem.MaxRange )
		SendSysMessage(mobile, "That is too far away.");
		return 0;
	endif
	
	if ( !allow_precast )
		if ( mobile.x != orig_x || mobile.y != orig_y || mobile.realm != orig_realm )
			SendSysMessage(mobile, "Your concentration is disturbed, thus ruining thy spell.");
			PlaySoundEffect(mobile, SFX_SPELL_FAIL);
			return 0;
		endif
	endif

	return targ;
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
function MS_TargetCoordinates(mobile, targ:=0, prompt:="Select a target.")
	mobile.hidden := 0;

	if ( targ )
		return targ;
	elseif ( mobile.npctemplate )
		return 0;
	endif

	var orig_x := mobile.x;
	var orig_y := mobile.y;
	var orig_realm := mobile.realm;
	var settings_elem := MS_GetSettingsCfgElem("Settings");
	var allow_precast := CInt(settings_elem.AllowPrecasting);
	
	SendSysMessage(mobile, prompt);
	targ := TargetCoordinates(mobile);

	if ( CoordinateDistance(mobile.x, mobile.y, targ.x, targ.y) > settings_elem.MaxRange )
		SendSysMessage(mobile, "That is too far away.");
		return 0;
	endif
	
	if ( !allow_precast )
		if ( mobile.x != orig_x || mobile.y != orig_y || mobile.realm != orig_realm )
			SendSysMessage(mobile, "Your concentration is disturbed, thus ruining thy spell.");
			PlaySoundEffect(mobile, SFX_SPELL_FAIL);
			return 0;
		endif
	endif
	
	return targ;
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
 * MS_GetSpellDamage(mobile, targ, base_damage)
 *
 * Purpose
 *   Calculates spell damage using formula provided by uo.stratics.
 *   Eval. Int. Bonus: ((EvalInt * 3) / 100) + 1
 *   Intelligence Bonus: (INT / 10) (Drop decimals from result)
 *   GM Inscription Bonus: 10% Damage Bonus
 *   Spell Damage Increase: Int. Bonus + Inscr. Bonus + Spell Damage Increase Items
 *   Total: round((Base Damage * Eval. Int. Bonus) * ((Spell Damage Increase / 100) + 1))
 *
 * Parameters
 *   mobile:		mobile refrence of caster
 *   targ:			target of spell
 *   base_damage: base damage
 *
 * Returns
 *   double
 *
 */
function MS_GetSpellDamage(mobile, targ, base_damage)
	var eval_int_bonus := CInt((AP_GetSkill(mobile, EVALUATING_INTELLIGENCE) * 3) / 100) + 1;
	var intel_bonus := CInt(AP_GetStat(mobile, "Intelligence") / 10);

	// TODO: Get SDI items percentage
	var sdi_bonus := 0;
	
	// SDI bonus capped at 15% for PvP
	if ( mobile.IsA(POLCLASS_MOBILE) && !mobile.IsA(POLCLASS_NPC) && targ.IsA(POLCLASS_MOBILE) && !targ.IsA(POLCLASS_NPC) )
		if ( sdi_bonus > 15 )
			sdi_bonus := 15;
		endif
	endif
	
	var damage_increase := intel_bonus + sdi_bonus;

	if ( AP_GetSkill(mobile, INSCRIPTION) >= 100 )
		damage_increase := damage_increase + 10;
	endif
	
	var total_damage := CInt(base_damage * eval_int_bonus * ((CDbl(damage_increase) / 100) + 1));
	return CDbl(total_damage);
endfunction


/*
 * MS_MobileCheck(mobile, targ)
 *
 * Purpose
 *   Checks if the target is a mobile and provides a central
 *   location for failure messages.
 *
 * Parameters
 *   mobile:		mobile refrence of caster
 *   targ:			target refrence
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
 * MS_GetPoisonLevel(mobile, targ)
 *
 * Purpose
 *   Calculates poison level based on uo.stratics:
 *  ((Magery + Poisoning) / 2):
 *  <65.1 = level 1
 *  65.1 - 85 = level 2
 *  85.1 - 99.9 = level 3
 *  100+ = level 4
 *
 * Parameters
 *   mobile:		mobile refrence of caster
 *   targ:			target refrence
 *
 * Returns
 *   int
 *
*/
function MS_GetPoisonLevel(mobile, targ)
	var poison_level := CDbl(AP_GetSkill(mobile, MAGERY) + AP_GetSkill(mobile, POISONING)) / 2;

	if ( poison_level < 65.1 )
		poison_level := 1;
	elseif ( poison_level >= 65.1 && poison_level <= 85 )
		poison_level := 2;
	elseif ( poison_level > 85 && poison_level <= 99.9 )
		poison_level := 3;
	elseif ( poison_level >= 100 )
		poison_level := 4;
	endif
	
	return poison_level;
endfunction


function MS_PlaySpellSFX(spellid, targ)
	var cfg_elem := MS_GetSpellsCfgElem(spellid);
	var sfx := GetConfigInt(cfg_elem, "SFX");
	
	//Print("Playing SFX:"+sfx+" for spellid:"+spellid);
	return PlaySoundEffect(targ, sfx);
endfunction


function MS_PlaySpellGFX(spellid, targ, source, speed:=7, loop:=14, explode:=0, realm:=_DEFAULT_REALM)
	var cfg_elem := MS_GetSpellsCfgElem(spellid);
	var gfx := GetConfigInt(cfg_elem, "GFX");
	var gfx_type := GetConfigInt(cfg_elem, "GFXType");
	
	//Print("Playing GFX:"+gfx+" spellid:"+spellid+" gfx type:"+gfx_type+" source:"+source.name+" targ:"+targ.name);
	case ( gfx_type )
		0: return 1;
		1: return PlayObjectCenteredEffect(targ, gfx, speed, loop);
		2: return PlayMovingEffect(source, targ, gfx, speed, loop, explode);
		3: return PlayStationaryEffect(targ.x, targ.y, targ.z, gfx, speed, loop, 0, targ.realm);
		4: return PlayMovingEffectXYZ( source.x, source.y, source.z, targ.x, targ.y, targ.z, gfx, speed, loop, explode, realm);
	endcase

	return 1;
endfunction


// Function for quickly grabbing nearby mobiles that are not in excludes or that are in includes
function MS_GetValidTargets(targ, area_size:=1, check_los:=1, excludes:=array{}, includes:=array{})
	var targs := array{};
	
	if ( !targ.realm )
		targs := ListMobilesNearLocation(targ.x, targ.y, targ.z, area_size);
	else
		targs := ListMobilesNearLocation(targ.x, targ.y, targ.z, area_size, targ.realm);
	endif
	
	// Remove targs with serial in exclude if exclude exists or not in includes if includes exists
	for i := 1 to ( targs.Size() )
		if ( (excludes.Size() > 0 && targs[i].serial in excludes) || (includes.Size() > 0 && !(targs[i].serial in includes)))
			targs.Erase(i);
		endif
	endfor
	
	return targs;
endfunction


// Find friendlies and add them to the list of includes
// Note: A friendly is anything non-criminal and non-murderer. I guess.
function MS_GetFriendlies(targ, area_size:=1)
	var friendlies;
	if ( !targ.realm )
		friendlies := ListMobilesNearLocation(targ.x, targ.y, targ.z, area_size);
	else
		friendlies := ListMobilesNearLocation(targ.x, targ.y, targ.z, area_size, targ.realm);
	endif
	
	for i := 1 to friendlies.Size()
		if ( friendlies[i].criminal || friendlies[1].murderer )
				friendlies.Erase(i);
		endif
	endfor
	
	return friendlies;
endfunction
	