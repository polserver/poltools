/*
        Maintened by *Edwards

        2009-02-09
*/
use uo;
use os;
use cfgfile;
use vitals;
use attributes;

include ":attributes:advanceCheck";
include ":attributes:attributeConstants";
include ":attributes:caps";
include ":attributes:regen";
include ":attributes:settings";
include ":attributes:skillCheck";
include ":attributes:skills";
include ":attributes:stats";
include ":attributes:vitals";
include ":datafile:datafile";

function AP_GetAttributesCfgFile()

	var cfg_file := ReadConfigFile( ":attributes:attributes" );
	if( cfg_file.errortext )
		SysLog( "Error::AP_GetAttributesCfgFile() - Unable to open attributes.cfg ->"+cfg_file.errortext );
	endif

	return cfg_file;
endfunction

function AP_GetAttributeCfgElem( attribute_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := AP_GetAttributesCfgFile();
	endif

	var cfg_elem := cfg_file[attribute_name];
	if( cfg_elem.errortext )
		SysLog( "Error::AP_GetAttributeCfgElem() - unable to find cfg elem ["+attribute_name+"] ->"+cfg_elem.errortext );
	endif

	return cfg_elem;
endfunction

function AP_GetAttributesDataFile()

	var data_file := DFOpenDataFile( ":attributes:attributes", DF_CREATE );
	if( data_file.errortext )
		SysLog( "Error::AP_GetAttributesDataFile() - Unable to open attributes.txt ->"+data_file.errortext );
	endif

	return data_file;
endfunction

function AP_GetAttributeDataElem( elem_name )

	var data_file := AP_GetAttributesDataFile();
	var data_elem := DFFindElement( data_file, elem_name, DF_CREATE );

	if( data_elem.errortext )
		SysLog( "Error::AP_GetAttributeDataElem() - unable to find elem ["+elem_name+"] ->"+data_elem.errortext );
	endif

	return data_elem;
endfunction

function AP_ListAttributesByType( type:="" )

	if( !type )
		var cfg_file := ReadConfigFile( ":attributes:attributes" );
		return GetConfigStringKeys( cfg_file );
	endif

	type := Lower( type );
	var data_elem := AP_GetAttributeDataElem( "Categories" );
	return data_elem.GetProp( type );
endfunction
