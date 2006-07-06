/* $Id$
 *
 */

include ":datafile:datafile";

function GetHelpDataFile()
	return DFOpenDataFile(":help:helpsys", DF_CREATE);
endfunction

function LoadHelpInfo(elem_name, prop_name)
	var data_file := GetHelpDataFile();
	var data_elem := DFFindElement(data_file, elem_name, DF_CREATE);
	return data_elem.GetProp(prop_name);
endfunction

function SaveHelpInfo(elem_name, prop_name, byref data)
	var data_file := GetHelpDataFile();
	var data_elem := DFFindElement(data_file, elem_name, DF_CREATE);
	return data_elem.SetProp(prop_name, data);
endfunction
