/* $Id: classes.inc 547 2006-07-04 07:56:00Z austinheilman $
 *
 */

use uo;
use cfgfile;

include ":datafile:datafile";
include "settings";

/*
 * CLASS_GetDataFile(mobile)
 *
 * Purpose
 *
 * Parameters
 *
 * Return Value
 *
 */
function CLASS_GetDataFile(source:="")
	var data_file := DFOpenDataFile(":classes:classes", DF_CREATE);

	if ( source )
		if ( source.IsA(POLCLASS_MOBILE) )
			source := source.serial;
		endif

		data_file := DFFindElement(data_file, Hex(CInt(source)), DF_CREATE);
	endif

	return data_file;
endfunction

/*
 * CLASS_GetConfigFile(elem)
 *
 * Purpose
 *
 * Parameters
 *
 * Return Value
 *
 */
function CLASS_GetConfigFile(elem_name:="")
	var class_cfg := ReadConfigFile(":classes:classes");
	if ( elem_name )
		return class_cfg[elem_name];
	else
		return class_cfg;
	endif
endfunction

/*
 * CLASS_GetNames()
 *
 * Purpose
 *
 * Parameters
 *
 * Return Value
 *
 */
function CLASS_GetNames()
	var class_cfg := CLASS_GetConfigFile("Index");

	return GetConfigStringArray(class_cfg, "Class");
endfunction

/*
 * CLASS_GetMobileClassName()
 *
 * Purpose
 *
 * Parameters
 *
 * Return Value
 *
 */
function CLASS_GetMobileClassName(mobile)
	if ( GetObjProperty(mobile, "Class") )
		return GetObjProperty(mobile, "Class");
	elseif ( mobile.acctname )
		var data_elem := CLASS_GetDataFile(mobile);
		return data_elem.GetProp("Class");
	endif
endfunction

/*
 * CLASS_SetMobileClassName()
 *
 * Purpose
 *
 * Parameters
 *
 * Return Value
 *
 */
function CLASS_SetMobileClassName(mobile, class_name)
	var settings := CLASS_GetSettingsCfgElem("Settings");

	if ( mobile.acctname )
		var data_elem := CLASS_GetDataFile(mobile);
		return data_elem.SetProp("Class", class_name);
	else
		return SetObjProperty(mobile, "Class", class_name);
	endif
endfunction
