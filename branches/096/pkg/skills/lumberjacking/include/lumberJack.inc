use uo;
use os;
use util;
use cfgfile;
use file;

include ":datafile:datafile";
include ":lumberjacking:settings";

/*
 * LJ_IsTree(graphic)
 *
 * Purpose
 * Determines if the graphic can be used for lumberjacking.
 * Uses settings.cfg elem TreeList
 *
 * Parameters
 * graphic:	Graphic to verify
 *
 * Return value
 * Returns 1 if the object can be used.
 *
 */
function LJ_IsTree(graphic)
	var cfg_elem := LJ_GetSettingsCfgElem("TreeList");
	graphic := CStr(graphic);

	return ( graphic in GetConfigStringArray(cfg_elem, "Graphic") );
endfunction

/*
 * LJ_InRect(x_pos, y_pos, rect_string)
 *
 * Purpose
 * Determines if a coordinate is within an area.
 *
 * Parameters
 * x_pos:	X position of the coordinate to verify.
 * y_pos:	Y position of the coordinate to verify.
 * rect_string:	Rect line from WoodRegions.cfg
 *
 * Return value
 * Returns 1 if the coordinates are in the rect.
 *
 */
function LJ_InRect(x_pos, y_pos, rect)
	var x1 := CInt(rect[1]);
	var y1 := CInt(rect[2]);
	var x2 := CInt(rect[3]);
	var y2 := CInt(rect[4]);

	return ( (x_pos >= x1 && x_pos <= x2) && (y_pos >= y1 && y_pos <= y2) );
endfunction



/*
 * LJ_GetDataFile()
 *
 * Purpose
 * Retrieves the lumberjacking datafile
 *
 * Parameters
 *
 * Return value
 * data file
 *
 */
function LJ_GetDataFile()
	var data_file := DFOpenDataFile(":lumberjacking:TreeSettings", DF_CREATE);

	return data_file;
endfunction

/*
 * LJ_GetDataFile()
 *
 * Purpose
 * Retrieves an elem from the lumberjacking data file.
 *
 * Parameters
 * elem_name:	Name of the elem to retrieve.
 *
 * Return value
 * Data elem
 *
 */
function LJ_GetDataElem(elem_name)
	var data_file := LJ_GetDataFile();
	var data_elem := DFFindElement(data_file, elem_name, DF_CREATE);

	return data_elem;
endfunction

/*
 * LJ_GetWoodType(realm, x_pos, y_pos)
 *
 * Purpose
 * Retrieves the type of wood set for an X and Y coordinate.
 *
 * Parameters
 * realm:	Name of the realm the coordinates are on.
 * x_pos:	X coordinate to use.
 * y_pos:	Y coordinate to use.
 *
 * Return value
 * returns an object type number
 *
 */
function LJ_GetWoodType(realm, x_pos, y_pos)
	var pos_string := CStr(realm+" "+x_pos+"x"+y_pos);

	var data_elem := LJ_GetDataElem(pos_string);

	var type := data_elem.GetProp("Type");
	if ( !type )
		type := LJ_PickWoodType(realm, x_pos, y_pos);
		LJ_SetWoodType(realm, x_pos, y_pos, type);
	endif

	return type;
endfunction

/*
 * LJ_SetWoodType(realm, x_pos, y_pos, wood_type)
 *
 * Purpose
 * Sets the wood type for an X Y coordinate on a realm.
 * 
 * Parameters
 * realm:	Name of the realm to set the wood type on.
 * x_pos:	X coordinate to set the wood type on.
 * y_pos:	Y coordinate to set the wood type on.
 * wood_type:	object NAME of the wood to set.
 *
 * Return value
 * Returns 1
 *
 */
function LJ_SetWoodType(realm, x_pos, y_pos, wood_type)
	var pos_string := CStr(realm+" "+x_pos+"x"+y_pos);
	
	var data_elem := LJ_GetDataElem(pos_string);
	
	return data_elem.SetProp("Type", wood_type);
endfunction

/*
 * LJ_SetWoodAmount(realm, x_pos, y_pos, amount)
 *
 * Purpose
 * Sets the wood amount for an X Y coordinate on a realm.
 * 
 * Parameters
 * realm:	Name of the realm to set the wood amount on.
 * x_pos:	X coordinate to set the wood amount on.
 * y_pos:	Y coordinate to set the wood amount on.
 * amount:	Amount of wood to make available.
 *
 * Return value
 * Returns 1
 *
 */
function LJ_SetWoodAmount(realm, x_pos, y_pos, amount)
	var pos_string := CStr(realm+" "+x_pos+"x"+y_pos);
	
	var data_elem := LJ_GetDataElem(pos_string);
	
	data_elem.SetProp("Amount", CInt(amount));
endfunction

/*
 * LJ_GetWoodAmount(realm, x_pos, y_pos)
 *
 * Purpose
 * Retrieves the amount of wood set for an X and Y coordinate.
 *
 * Parameters
 * realm:	Name of the realm the coordinates are on.
 * x_pos:	X coordinate to use.
 * y_pos:	Y coordinate to use.
 *
 * Return value
 * returns a number
 *
 */
function LJ_GetWoodAmount(realm, x_pos, y_pos)
	var pos_string := CStr(realm+" "+x_pos+"x"+y_pos);
	
	var data_elem := LJ_GetDataElem(pos_string);
	
	var amount := data_elem.GetProp("Amount");
	if ( amount == error )
		var settings := LJ_GetSettingsCfgElem("Settings");
		amount := CInt(settings.MaxWood);
		LJ_SetWoodAmount(realm, x_pos, y_pos, amount);
	endif
	
	return CInt(amount);
endfunction

/*
 * LJ_PickWoodType(realm, x_pos, y_pos)
 *
 * Purpose
 * Selects a random wood type for a tree based on
 * the realm and region coordinates in woodRegions.cfg
 *
 * Parameters
 * realm:	Name of the realm the coordinates are on.
 * x_pos:	X coordinate to use.
 * y_pos:	Y coordinate to use.
 *
 * Return value
 * returns an object type number
 *
 */
function LJ_PickWoodType(realm, x_pos, y_pos)
	var wood_cfg := ReadConfigFile(":lumberjacking:config/wood-"+realm);
	if ( !wood_cfg )
		return GetObjTypeByName("Log");
	endif

	var wood_found := array{"Log"};
	foreach region_name in ( GetConfigStringKeys(wood_cfg) )
		var region_elem := wood_cfg[region_name];
		if ( !LJ_InRect(x_pos, y_pos, region_elem.Rect) )
			continue;
		endif

		var wood_types := GetConfigStringArray(region_elem, "WoodType");
		if ( wood_types.Size() >= 1 )
			wood_found := wood_found+wood_types;
		endif
		SleepMS(2);
	endforeach

	var selected_wood := wood_found[RandomInt(wood_found.Size())+1];
	selected_wood := GetObjTypeByName(selected_wood);
	if ( !selected_wood )
		return GetObjTypeByName("Log");
	endif
	
endfunction

