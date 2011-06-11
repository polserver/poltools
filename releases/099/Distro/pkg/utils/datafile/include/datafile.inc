/* 
 * Purpose
 * TODO
 *
 * Function List *
 * DFFindElement(byref file_ref, elem_name, create)
 * DFGetElemNames(byref file_ref)
 * DFGetElemProps(elem_ref)
 * DFGetProp(byref elem_ref, prop_name, create, assign_val)
 * DFOpenDataFile(file_name, create, flags)
 * CfgToDF(cfg_name, df_name, propnames)
 * DFToCfg(df_name, cfg_name)
 *
 * Private Function List *
 * DF_OutPut(text)
 *
 * Global Variables *
 * Datafile.em CONSTants
 * CONST DF_KEYTYPE_STRING       := 0x00; // default
 * CONST DF_KEYTYPE_INTEGER      := 0x01;
 *
 */
use uo;
use os;
use file;
use basic;
use cfgfile;
use datafile;

// Determines if a file or elem will be created when a read attempt is made.
CONST DF_NO_CREATE	:= 0;
CONST DF_CREATE		:= 1;

// Set to 1 in your script to turn on the logging calls.
var DF_DEBUG_MODE	:= 0;

function DFOpenDataFile(file_name, create:=DF_NO_CREATE, flags:=DF_KEYTYPE_STRING)
	var data_file := OpenDataFile(file_name);
	if ( (!data_file) && (create) )
		DF_OutPut("Debug::DFOpenDataFile() - Creating data file: "+file_name);
		CreateDataFile(file_name, flags);
		data_file := OpenDataFile(file_name);
	endif

	if ( data_file )
		return data_file;
	elseif ( create )
		var errmsg := error{"errortext":="Error::DFOpenDataFile() - Could not open "+file_name+" : "+data_file.errortext};
		DF_OutPut(errmsg.errortext);
		return errmsg;
	endif
endfunction

function DFPurgeFile(byref file_ref)
	foreach elem_name in ( DFGetElemNames(file_ref) )
		file_ref.DeleteElement(elem_name);
		SleepMS(2);
	endforeach

	return 1;
endfunction

function DFGetElemNames(byref file_ref)
	var elem_keys := file_ref.Keys();
	if ( elem_keys == error )
		var errmsg := error{"errortext":="Error::DFGetElemNames() - Could not return elem keys :"+elem_keys.errortext};
		DF_OutPut(errmsg.errortext);
		return errmsg;
	else
		return elem_keys;
	endif
endfunction

function DFFindElement(byref file_ref, elem_name, create:=DF_NO_CREATE)
	var temp := file_ref.FindElement(CStr(elem_name));
	if ( !temp && create )
		DF_OutPut("Debug::DFFindElement() - Creating elem: "+CStr(elem_name));
		file_ref.CreateElement(CStr(elem_name));
		temp := file_ref.FindElement(CStr(elem_name));
	endif

	if ( temp )
		return temp;
	elseif (create)
		var errmsg := error{"errortext":="Error::DFFindElement() - Could not open data elem ["+elem_name+"] - "+temp.errortext};
		DF_OutPut(errmsg.errortext);
		return errmsg;
	endif
endfunction

function DFGetElemProps(elem_ref)
	var prop_names := elem_ref.PropNames();
	if ( prop_names == error )
		var errmsg := error{"errortext":="Error::DFGetElemProps() - Could not return prop names :"+prop_names.errortext};
		DF_OutPut(errmsg.errortext);
		return errmsg;
	else
		return prop_names;
	endif
endfunction

function DFGetProp(byref elem_ref, prop_name, create:=DF_NO_CREATE, assign_val:=0)
	var temp := elem_ref.GetProp(prop_name);

	if ( (temp==error) && (create) )
		elem_ref.SetProp(prop_name, assign_val);
		temp := elem_ref.GetProp(prop_name);
	endif

	if ( temp || (create) && (!assign_val) )
		// Second case is for creating and returning
		// what ever you get. Used for cases where the
		// value is '0' but not an error.
		return temp;
	elseif ( create )
		var errmsg := error{"errortext":="Error::DFGetProp() - Could not read data file property "+prop_name+":"+temp.errortext};
		DF_OutPut(errmsg.errortext);
		return errmsg;
	endif
endfunction

function DF_OutPut(text)
	if ( DF_DEBUG_MODE )
		//SysLog(text);
		var script_name := GetProcess(GetPid()).name;
		LogToFile("::log/dataFile.log", "["+script_name+"]: "+text, LOG_DATETIME);
	endif

	return 1;
endfunction
