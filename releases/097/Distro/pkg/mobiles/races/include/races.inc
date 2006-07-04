/* $Id$
 *
 */

use uo;
use cfgfile;

include ":datafile:datafile";
include "settings";

/*
 * RACE_GetDataFile(mobile)
 *
 * Purpose
 *
 * Parameters
 *
 * Return Value
 *
 */
function RACE_GetDataFile(source:="")
	var data_file := DFOpenDataFile(":races:races", DF_CREATE);

	if ( source )
		if ( source.IsA(POLCLASS_MOBILE) )
			source := source.serial;
		endif

		data_file := DFFindElement(data_file, Hex(CInt(source)), DF_CREATE);
	endif

	return data_file;
endfunction

/*
 * RACE_GetConfigFile(elem)
 *
 * Purpose
 *
 * Parameters
 *
 * Return Value
 *
 */
function RACE_GetConfigFile(elem_name:="")
	var race_cfg := ReadConfigFile(":races:races");
	if ( elem_name )
		return race_cfg[elem_name];
	else
		return race_cfg;
	endif
endfunction

/*
 * RACE_GetNames()
 *
 * Purpose
 *
 * Parameters
 *
 * Return Value
 *
 */
function RACE_GetNames()
	var race_cfg := RACE_GetConfigFile("Index");

	return GetConfigStringArray(race_cfg, "Race");
endfunction

/*
 * RACE_GetMobileRaceName()
 *
 * Purpose
 *
 * Parameters
 *
 * Return Value
 *
 */
function RACE_GetMobileRaceName(mobile)
	if ( GetObjProperty(mobile, "Race") )
		return GetObjProperty(mobile, "Race");
	elseif ( mobile.acctname )
		var data_elem := RACE_GetDataFile(mobile);
		return data_elem.GetProp("Race");
	endif
endfunction

/*
 * RACE_SetMobileRaceName()
 *
 * Purpose
 *
 * Parameters
 *
 * Return Value
 *
 */
function RACE_SetMobileRaceName(mobile, race_name)
	var settings := RACE_GetSettingsCfgElem("Settings");
	if ( settings.ShowTitle_Race)
		mobile.title_race := race_name;
	endif

	if ( mobile.acctname )
		var data_elem := RACE_GetDataFile(mobile);
		return data_elem.SetProp("Race", race_name);
	else
		return SetObjProperty(mobile, "Race", race_name);
	endif
endfunction
