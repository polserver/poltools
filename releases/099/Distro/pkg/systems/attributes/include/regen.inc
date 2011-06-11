/*
        Maintened by *Edwards

        2009-02-09
*/

use uo;

function AP_SetVitalRegenRateMod( mobile, vital_name, number )

	vital_name := Lower( vital_name );
	number := CInt( number );

	var regen_prop := AP_LoadVitalRegenRateProp( mobile );

	if( number == 0 )
		// Remove keys with no modifier
		regen_prop.Erase( vital_name );
	else
		regen_prop[vital_name] := CInt( number );
	endif

	AP_SaveVitalRegenRateProp( mobile, regen_prop );
 
	return 1;
endfunction

function AP_GetVitalRegenRateMod( mobile, vital_name )

	vital_name := Lower( vital_name );

	var regen_prop := AP_LoadVitalRegenRateProp( mobile );
	return CInt( regen_prop[vital_name] );
endfunction

function AP_LoadVitalRegenRateProp( mobile )

	var regen_prop := GetObjProperty( mobile, "%RegenRates%" );
	if( Lower( TypeOf( regen_prop )) != "dictionary" )
		regen_prop := dictionary;
	endif

	return regen_prop;
endfunction

function AP_SaveVitalRegenRateProp( mobile, byref regen_prop )

	if( Lower( TypeOf( regen_prop )) != "dictionary" )
		return error{"errortext":="Regen prop must be a dictionary."};
	endif

	var result := SetObjProperty( mobile, "%RegenRates%", regen_prop );
	if( result != error )
		RecalcVitals( mobile );
	endif

	return result;
endfunction
