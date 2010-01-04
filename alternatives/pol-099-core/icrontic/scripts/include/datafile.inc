/////////////////////////////////
// 1.4
//
// DataFile.inc
//    Austin Heilman
//    Austin@tsse.net
//
// Created: 9/18/2001 8:21PM
// Updated: 7/2/2002 4:33PM
//
// =Revision 1.4
//        -- Added DFToCfg(datafile, cfgfile);
//        -- Added CfgToDF(cfgfile, datafile, propnames);
//
// =Revision 1.3
//        -- Added a global variable called 'DF_DEBUG_MODE'
//           Setting it to 0 in your script will make the functions
//           no longer SysLog() information to the console. By default
//           it is enabled.
//        -- Fixed a bug in DFSetProp() when I improved the .errortext
//           return stuff.
//
// =Revision 1.2
//	Austin
//        -- Added a flags option for DFOpenDataFile()
//        -- Added DFGetElemNames()
//        -- Changed the error stuff, to return .errortext
//
/////////////////////////////////
//
// Notes:
//    - Datafiles are package specific.
//      If you want to get a datafile from another package
//      you pretty much need to write a script that returns the
//      information and is run with a run_script_to_complection
//      *or* use start_script, pass the pid of the script that is
//      doing the starting and wait for the started script to send
//      an event back with the needed information. The first method
//      is easier, but also remember that it runs critical. The second
//      is softer, but not as pretty. This is due to a core bug that
//      prevents you from doing something like OpenDataFile(":pkg:datafile");
//
/////////////////////////////////

use datafile;
use cfgfile;
use os;

// **************************************************************************
// * DFOPenDataFile(filename, create, flags);
// * DFGetElemNames(file_ref);
// * DFFindElement(file_ref, elem_name, create);
// * DFGetProp(elem_ref, prop_name, create, assignval);
// * DFToCfg(datafile, cfgfile);
// * CfgToDF(cfgfile, datafile, propnames);
// **************************************************************************

const DF_CREATE		:= 1;
const DF_NO_CREATE	:= 0;
// Datafile.em constants
//const DF_KEYTYPE_STRING       := 0x00; // default
//const DF_KEYTYPE_INTEGER      := 0x01;

// Set to 0 in your script to turn off SysLog() calls.
var DF_DEBUG_MODE	:= 1;

//
// DFOpenDataFile( <string>, <int>, <int> )
//    Used to open a datafile.
//
function DFOpenDataFile(filename, create:=DF_NO_CREATE, flags:=DF_KEYTYPE_STRING)
	var temp := OpenDataFile(filename);
	if ( (!temp) && (create) )
		DF_OutPut("Creating data file: " + filename);
		CreateDataFile(filename, flags);
		temp := OpenDataFile(filename);
	endif

	if ( temp )
		return temp;
	elseif ( create )
		var errmsg := error;
		errmsg.+errortext := "Could not open " + filename + ":" + temp.errortext;
		DF_OutPut(errmsg.errortext);
		return errmsg;
	endif
endfunction

//
// DFGetElemNames( <datafile reference> )
//    Returns an array of strings that are the names of
//    the elem keys in the passed datafile.
//
function DFGetElemNames(byref file_ref)
	var elem_keys := file_ref.keys();
	if ( elem_keys == error )
		var errmsg := error;
		errmsg.+errortext := "Could not return elem keys :"+elem_keys.errortext;
		DF_OutPut(errmsg.errortext);
		return errmsg;
	else
		return elem_keys;
	endif
endfunction

//
// DFFindElement( <datafile reference>, <string>, <int> )
//    Returns elem information from the passed datafile.
//
function DFFindElement(byref file_ref, elem_name, create:=DF_NO_CREATE)
	var temp := file_ref.FindElement( cstr(elem_name) );
	if (!temp && create)
		DF_OutPut("Creating elem: " +  cstr(elem_name) );
		file_ref.CreateElement( cstr(elem_name) );
		temp := file_ref.FindElement( cstr(elem_name) );
	endif

	if ( temp )
		return temp;
	elseif (create)
		var errmsg := error;
		errmsg.+errortext := "Could not open data elem " + elem_name + ":" + temp.errortext;
		DF_OutPut(errmsg.errortext);
		return errmsg;
	endif
endfunction

//
// DFGetElemProps( <datafile elem reference> )
//    Returns an array of strings that are the names of CProps in the passed elem.
//
function DFGetElemProps(byref elem_ref)
	var prop_names := elem_ref.keys();
	if ( prop_names == error )
		var errmsg := error;
		errmsg.+errortext := "Could not return prop names :"+prop_names.errortext;
		DF_OutPut(errmsg.errortext);
		return errmsg;
	else
		return prop_names;
	endif
endfunction

//
// DFGetProp( <datafile elem reference>, <string>, <int>, <anything> )
//    Used to get a cprop in the elem reference.
//    If the cprop can not be found then it will be set to what ever 
//    is in 'assignval' (assuming the create flag is not '0').
//
function DFGetProp(byref elem_ref, prop_name, create:=DF_NO_CREATE, assignval:=0)
	var temp := elem_ref.GetProp(prop_name);

	if ( (!temp) && (create) )
		elem_ref.SetProp(prop_name, assignval);
		temp := elem_ref.GetProp(prop_name);
	endif

	if ( temp || (create) && (!assignval) )
		// Second case is for creating and returning
		// what ever you get. Used for cases where the
		// value is '0' but not an error.
		return temp;
	elseif (create)
		var errmsg := error;
		errmsg.+errortext := "Could not read data file property " + prop_name + ":" + temp.errortext;
		DF_OutPut(errmsg.errortext);
		return errmsg;
	endif
endfunction

// -=Experimental Function=-
// Convert a datafile to a config file.
//
// DFToCfg( <string>, <string> )
//    1. Pass two strings. 
//       One string is the name of the datafile.
//       The other string is the name of the config file.
//     Note: you can also pass a datafile reference and/or a cfg file reference instead of strings.
//
//    2. The datafile's properties will be broken down and copied into the config file.
//       All of the datafiles CProp values will go through the Pack() function to ensure 
//       they will go into the config file correctly.
//
//    This function does not do any error checking so when you write to a config file
//    it is best to make sure the cfg file is blank so you dont risk duplicate keys.
//    

function DFToCfg(datafile, cfgfile)
	if ( lower(TypeOf(datafile))["string"] )
		datafile := DFOpenDataFile(datafile, DF_CREATE);
	endif
	if ( lower(TypeOf(cfgfile)) != "string" )
		var errmsg := error;
		errmsg.+errortext := "DFToCfg() cfgfile parameter passed was not a string.";
		DF_OutPut(errmsg.errortext);
		return errmsg;
	endif

	foreach elem_name in DFGetElemNames(datafile)
		var elem := DFFindElement(datafile, elem_name);

		var elem_info := array;
		var prop_info := array;
		prop_info.+name;
		prop_info.+value;

		foreach prop_name in DFGetElemProps(elem)
			prop_info.name := prop_name;
			prop_info.value := Pack(DFGetProp(elem, prop_name));
			sleepms(2);
		endforeach
		AppendConfigFileElem(cfgfile, "Elem", elem_name, elem_info);
		sleepms(2);
	endforeach
endfunction

// -=Experimental Function=-
// Convert a config file to a data file.
//
// CfgToDF( <string>, <string>, <array[strings]>)
//    1. Pass a config file name, datafile name, and the lines in the elems to copy over.
//           An example would be CfgToDF("itemdesc", "archive", {"Name", "Desc", "Graphic", "Color"})
//      Note: You can also pass a datafile reference instead of a string.
//            The config file in this one *must* be passed as a string though to use
//            the AppendConfigFileElem() function in cfgfile.em
//
//    2. The config file's elems will be broken down. If it has any lines in that elem that match
//       the lines passed in 'propnames' they will be copied over into the datafile.
//       
//       If there are multiple entries in the config file, of the same name then it will store them
//       in an array.
//
//       All config file lines are stored as simple datatypes in the datafile.
//       If you write to an existing datafile, its cprops will be overwritten.
//
function CfgToDF(cfgfile, datafile, propnames)
	if ( propnames.size() < 1 )
		var errmsg := error;
		errmsg.+errortext := "CfgToDF() Size of linenames array is < 0";
		DF_OutPut(errmsg.errortext);
		return errmsg;
	endif
	if ( lower(TypeOf(cfgfile))["string"] )
		cfgfile := ReadConfigFile(cfgfile);
	endif
	if ( lower(TypeOf(datafile))["string"] )
		datafile := DFOpenDataFile(datafile, DF_CREATE);
	endif

	foreach elem_name in GetConfigStringKeys(cfgfile)
		var df_elem := DFFindElement(datafile, elem_name, DF_CREATE);
		var elem := cfgfile[elem_name];

		foreach prop_name in propnames
			var entries := GetConfigStringArray(elem, prop_name);
			var prop_value;

			if ( entries.size() > 1 )
				prop_value := array;
				foreach line in GetConfigStringArray(elem, prop_name)
					prop_value.Append(line);
				endforeach
			else
				prop_value := entries[1];
			endif

			if ( prop_value != error )
				df_elem.SetProp(prop_name, prop_value);
			endif
			sleepms(2);
		endforeach
		sleepms(2);
	endforeach
endfunction

function DF_OutPut(text)
	if ( DF_DEBUG_MODE )
		SysLog(text);
	endif
endfunction

