/*
 * $Id: training.inc 891 2005-11-04 06:29:43Z muaddiblsd $
 *
 */
use uo;
use cfgfile;

include ":attributes:attributes";
include ":combat:weaponInfo";

/*
 * TP_CheckWeaponRange(mobile, weapon, targ)
 *
 * Purpose
 * Checks if the weapon the mobile is using can reach the target.
 *
 * Parameters
 * mobile:	Mobile using the weapon on the target.
 * weapon:	Weapon that is being used.
 * targ:	Object that is begin targetted.
 *
 * Return value
 * Returns 1 if the weapon can reach.
 * Returns 0 if the weapon can't reach.
 *
 */
function TP_CheckRange(mobile, weapon, targ)
	var wpn_range := CInt(CS_GetWeaponInfo(weapon, "MaxRange"));
	if ( !wpn_range )
		wpn_range := 1;
	endif
		
	if ( Distance(mobile, targ) > wpn_range )
		SendSysMessage(mobile, "You are too far away.");
		return 0;
	endif
	
	return wpn_range;
endfunction

/*
 * TP_CanAdvance(mobile, attrib_name)
 *
 * Purpose
 * Checks if a mobile's skill is low enough to gain from the training tool.
 *
 * Parameters
 * mobile:	Mobile doing the training.
 * attrib_name:	Attribute to advance.
 *
 * Return value
 * Returns ADV_SKILL if the skill can advance.
 * Returns ADV_DISABLE if the skill is too high.
 *
 */
function TP_CanAdvance(mobile, attrib_name)
	if ( CDbl(AP_GetTrueSkill(mobile, attrib_name)) > Cdbl(30) )
		// Skill is higher than 30%
		return ADV_DISABLE;
	else
		return ADV_SKILL;
	endif
endfunction
