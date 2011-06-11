/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-07-26

        Last Modifications 2009-07-26
             * Added CheckName()
*/
use uo;
use os;
use cfgfile;

include ":datafile:datafile";
include "include/string";

function NC_GetDataFile()

	return DFOpenDataFile( ":newcharacter:createinfo", DF_CREATE );
endfunction

function NC_GetStartupSkills( mobile )

	if( !mobile.acctname )
		return error{ "errortext":="Object passed was not a player-character" };
	endif
	
	var data_file := NC_GetDataFile();
	var data_elem := DFFindElement( data_file, Hex(mobile.serial ));
	
	return data_elem.GetProp( "StartSkills" );
endfunction

function NC_SetStartupSkills( mobile, skill_names )

	if( !mobile.acctname )
		return error{ "errortext":="Object passed was not a player-character" };
	endif
	
	var data_file := NC_GetDataFile();
	var data_elem := DFFindElement( data_file, Hex(mobile.serial), DF_CREATE );
	
	return data_elem.SetProp( "StartSkills", skill_names );
endfunction

function CheckName( mobile )

	var script := start_script( ":newcharacter:nameCheck/nameCheck", mobile );
	if( script.errortext )
		SendSysMessage( mobile, "Error starting script <:newcharacter:nameCheck/nameCheck> -->"+script.errortext );
	endif
 
	return 1;
endfunction
