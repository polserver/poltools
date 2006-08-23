/* $Id$
 *
 */

use uo;
use os;
use cfgfile;

enum NPCALIGNMENT
	ALIGNMENT_NEUTRAL 	:= 0,
	ALIGNMENT_EVIL		:= 1,
	ALIGNMENT_GOOD		:= 2
endenum

/*
 * IsInnocent(mobile_a, mobile_b)
 *
 * Purpose
 * Determines if the defender was innocent to the attacker.
 *
 */
function IsInnocentTo(defender, attacker)
	if ( defender.murderer )
		// Defender is a murderer and as such, fair game.
		return 0;
	elseif ( defender.criminal )
		// Defender is a criminal and as such, fair game.
		return 0;
	elseif ( IsAggressorTo(attacker, defender) )
		// Defender started the fight, and is fair game.
		return 0;
	// elseif ( IsGuildAlly() )
	//	return 0;
	// elseif ( IsGuildEnemy() )
	//	return 0;
	elseif ( IsLawfulDamagerTo(attacker, defender) )
		// Attacker has lawfully damaged the defender.
		return 0;
	else
		return 1;
	endif
endfunction


/*
 * Purpose
 * Sets attacker as aggressive towards the defender.
 * Lasts for AggressorFlagTimeout seconds in repsys.cfg
 *
 */
function AddAggressorTo(defender, attacker)
	var aggressor_list := GetObjProperty(defender, "#Aggressors");
	if ( !aggressor_list )
		aggressor_list := dictionary;
	endif

	var rep_cfg := ReadConfigFile("::repsys");
	if ( !rep_cfg )
		return rep_cfg; // Contains error;
	endif
	var general_elem := rep_cfg["General"];
	if ( !general_elem )
		return general_elem; // Contains error;
	endif

	// Store's how long the attacker will be an aggressor.
	aggressor_list[attacker.serial] := ReadGameClock() + CInt(general_elem.AggressorFlagTimeout);

	return SetObjProperty(defender, "#Aggressors", aggressor_list);
endfunction

/*
 * Purpose
 * Determines if the attacker is aggressive to the defender.
 *
 *
 */
function IsAggressorTo(attacker, defender)
	var aggressor_list := GetObjProperty(defender, "#Aggressors");

	var expire_time := CInt(aggressor_list[attacker.serial]);
	if ( !expire_time )
		return 0;
	else
		return ( expire_time < ReadGameClock() );
	endif
endfunction

/*
 * Purpose
 * Clears the aggressor list on the mobile.
 *
 */
function ClearAggressors(mobile)
	return EraseObjProperty(mobile, "#Aggressors");
endfunction


/*
 * Purpose
 * Adds an attacker to the defender's to-be-reportable list.
 * When the defender dies, the to-be list becomes the main reportable-list.
 *
 */
function AddToBeReportable(defender, attacker)
	var tobe_list := GetObjProperty(defender, "#TBReportables");
	if ( !tobe_list )
		tobe_list := array;
	endif

	if ( !(attacker.serial in tobe_list) )
		tobe_list.Append(attacker.serial);
	endif

	return SetObjProperty(defender, "#TBReportables", tobe_list);
endfunction

/*
 * Purpose
 * Commits the reportables list. Occurs when the mobile dies.
 * Will build a list thats equivalent to mobile.reportables
 *
 */
function CommitReportables(mobile)
	var tobe_list := GetObjProperty(mobile, "#TBReportables");
	if ( !tobe_list )
		// No reportables to commit.
		return 1;
	endif

	var new_list := array;
	var info := struct{"serial", "gameclock"};

	foreach serial in ( tobe_list )
		info.serial := CInt(serial);
		info.gameclock := ReadGameClock();
		SleepMS(2);
	endforeach

	SetObjProperty(mobile, "#Reportables", new_list);
	EraseObjProperty(mobile, "#TBReportables");

	return 1;
endfunction


/*
 * Purpose
 * Sets attacker as a lawful damager towards the defender.
 * Lasts for AggressorFlagTimeout seconds in repsys.cfg
 *
 */
function AddLawfullyDamagedTo(defender, attacker)
	var lawfuldmg_list := GetObjProperty(defender, "#LawfulDamagers");
	if ( !lawfuldmg_list )
		lawfuldmg_list := dictionary;
	endif

	var rep_cfg := ReadConfigFile("::repsys");
	if ( !rep_cfg )
		return rep_cfg; // Contains error;
	endif
	var general_elem := rep_cfg["General"];
	if ( !general_elem )
		return general_elem; // Contains error;
	endif

	// Store's how long the attacker will be an aggressor.
	lawfuldmg_list[attacker.serial] := ReadGameClock() + CInt(general_elem.AggressorFlagTimeout);

	return SetObjProperty(defender, "#LawfulDamagers", lawfuldmg_list);
endfunction

/*
 * Purpose
 * Determines if the attacker had lawfully damaged the defender.
 *
 *
 */
function IsLawfulDamagerTo(attacker, defender)
	var lawfuldmg_list := GetObjProperty(defender, "#LawfulDamagers");

	var expire_time := CInt(lawfuldmg_list[attacker.serial]);
	if ( !expire_time )
		return 0;
	else
		return ( expire_time < ReadGameClock() );
	endif
endfunction