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


// AUSTIN: I will take these out as soon as a tileEffects.inc replacement is made.
enum TILE_EFFECTS
	// * general *
	FX_SPLASH         :=  0x352d, //
	FX_EXPLODE_1      :=  0x36ca, //
	FX_EXPLODE_2      :=  0x36bd, //
	FX_EXPLODE_3      :=  0x36b0, //
	FX_FIREBALL       :=  0x36d4, //
	FX_MAGIC_ARROW    :=  0x36e4, //
	FX_FIRE_BOLT      :=  0x36f4, // Firesnake. (pagan?)
	FX_EXPLODE_BALL   :=  0x36fe, // Not used.

	FX_FLAMESTRIKE    :=  0x3709, //
	FX_SMOKE          :=  0x3728,
	FX_TELE_VANISH    :=  0x372a, // Teleport vanish.
	FX_SPELL_FAIL     :=  0x3735, //
	FX_BLESS_EFFECT   :=  0x373a, //
	FX_CURSE_EFFECT   :=  0x374a, //
	FX_SPARK_EFFECT   :=  0x375a, //
	FX_HEAL_EFFECT    :=  0x376a, //
	FX_MARK_EFFECT    :=  0x3779,
	FX_BLUEMOONSTART  :=  0x3789, // Deathvortex?
	FX_ENERGY_BOLT    :=  0x379f, //
	FX_BLADES_EMERGE  :=  0x37a0, // Not used.
	FX_GLOW           :=  0x37b9, // Not used.
	FX_GLOW_SPIKE     :=  0x37c3, // Not used.
	FX_DEATH_FUNNEL   :=  0x37cc, //
	FX_BLADES         :=  0x37eb, //
	FX_STATIC         :=  0x3818, // Not used. (pink static)
	FX_POISON_F_EW    :=  0x3914, // Poison field.
	FX_POISON_F_NS    :=  0x3920, // Poison field.
	FX_ENERGY_F_EW    :=  0x3947, // Energy field.
	FX_ENERGY_F_NS    :=  0x3956, // Energy field.
	FX_PARA_F_EW      :=  0x3967, // Paralyze field.
	FX_PARA_F_NS      :=  0x3979, // Paralyze field.
	FX_FIRE_F_EW      :=  0x398c, // Fire field.
	FX_FIRE_F_NS      :=  0x3996,  // Fire field.

	FX_POISON_GAS     := 0x113a
endenum


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
function MS_Target(mobile, targ:=0, prompt := "Select a target.", flags:=TGTOPT_CHECK_LOS+TGTOPT_NEUTRAL)
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
function MS_TargetCoordinates(mobile, targ:=0, prompt := "Select a target.")
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

	if ( !targ )
		SendSysMessage(mobile, "Cancelled.");
		return 0;
	elseif ( CoordinateDistance(mobile.x, mobile.y, targ.x, targ.y) > settings_elem.MaxRange )
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
 * MS_CalcSpellDamage(mobile, spell_id)
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
 *   targ:			target of spell
 *   min_damage:	minimum base damage
 *   max_damage:	maximum base damage
 *
 * Returns
 *   total damage
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