/*
 * $Id$
 *
 */

use uo;
use os;
use cfgfile;

include ":datafile:datafile";

/* NC_GetDataFile()
 * 
 * Purpose
 * Retrives :newcharacter:createinfo
 *
 * Parameters
 *
 * Return Value
 * Data file
 * 
 */
function NC_GetDataFile()
	return DFOpenDataFile(":newcharacter:createinfo", DF_CREATE);
endfunction

/*
 * NC_GetStartupSkills(mobile)
 *
 * Purpose
 * Retrieves the skills the player selected on creation.
 *
 * Parameters
 * mobile:	Mobile to retrieve the skill list for.
 *
 * Return value
 * Array of strings
 *
 */
function NC_GetStartupSkills(mobile)
	if ( !mobile.acctname )
		return error{"errortext":="Object passed was not a player-character"};
	endif
	
	var data_file := NC_GetDataFile();
	var data_elem := DFFindElement(data_file, Hex(mobile.serial));
	
	return data_elem.GetProp("StartSkills");
endfunction

/*
 * NC_SetStartupSkills(mobile, skill_names)
 *
 * Purpose
 * Stores the skills the player selected on creation.
 *
 * Parameters
 * mobile:	Mobile to retrieve the skill list for.
 * skill_names:	Array containing attribute names chosen.
 *
 * Return value
 * 1
 *
 */
function NC_SetStartupSkills(mobile, skill_names)
	if ( !mobile.acctname )
		return error{"errortext":="Object passed was not a player-character"};
	endif
	
	var data_file := NC_GetDataFile();
	var data_elem := DFFindElement(data_file, Hex(mobile.serial), DF_CREATE);
	
	return data_elem.SetProp("StartSkills", skill_names);
endfunction
