/* $Id$
 *
 */

use uo;
use cfgfile;

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
	// elseif ( HasLawfullyDamaged(attacker, defender) )
	//	Havnt gotten to this yet!
	//	return 0;
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

	return ( CInt(aggressor_list[attacker.serial]) < ReadGameClock() );
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
 * Adds attacker to the defender's reportable list.
 *
 */
function AddToBeReportable(defender, attacker)
	var reportables := GetObjProperty(defender, "#Reportables");
	if ( !reportables )
		reportables := dictionary;
	endif

	reportables[attacker.serial] := ReadGameClock();
endfunction
