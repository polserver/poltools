/*
        Maintened by *Edwards

        2009-10-22
*/
use uo;
use os;
use polsys;
use cfgfile;

include ":datafile:datafile";

// constants for the moongate gump.
CONST MG_CANCEL_BTN	:= 0x0;
CONST MG_OKAY_BTN	:= 0x10;
CONST MG_LOCATION_START := 0xA00;

function MG_GetDataFile()

	var data_file := DFOpenDataFile( ":moongates:gateinfo", DF_CREATE );
	return data_file;
endfunction

function MG_GetDataElem( elem_name )

	var data_file := MG_GetDataFile();
	var realm_elem := DFFindElement( data_file, elem_name, DF_CREATE );

	return realm_elem;
endfunction

function MG_FindDestination( button_id )

	// Not too happy with how I designed this part. - Austin
	// Should probably save a list in a datafile and directly access it by the 'select' variable.
	
	var config := ReadConfigFile( ":moongates:moongate" );
	var btn_id := 1;
 
	foreach realm in ( Realms() )
		SleepMS(2);
  
		realm := realm; // To avoid a compiler warning.
		var cfg_elem := config[_realm_iter];
                if( cfg_elem.errortext )
                        //No files for realm.
                	continue;
                endif
                
		foreach entry in ( GetConfigStringDictionary( cfg_elem, "Town" ))
			if( btn_id == button_id )
				var coords := SplitWords( entry );
				var location := struct;
				location.+x := CInt( coords[1] );
				location.+y := CInt( coords[2] );
				location.+z := CInt( coords[3] );
				location.+realm := CStr( _realm_iter );
				return location;
			endif
			btn_id := btn_id+1;
			SleepMS(2);
		endforeach
	endforeach
	
        return 1;
endfunction

function JustTeleported( mobile )

	var tele_time := CInt( GetObjProperty( mobile, "#Teleported" ));
	if( tele_time > ReadMillisecondClock()-800 )
		// Mobile just teleported. Too soon to teleport again.
		return 1;
	else
		// Safe to teleport the mobile
		return 0;
	endif
endfunction
