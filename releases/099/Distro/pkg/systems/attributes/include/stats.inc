/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-02-09
*/
use uo;

function AP_GetStat( who, stat_name )

	return AP_GetTrueStat( who, stat_name ) + AP_GetStatMod( who, stat_name );
endfunction

function AP_GetTrueStat( who, stat_name )

	return CDbl( GetAttributeBaseValue( who, stat_name )) / 10.0;
endfunction

function AP_GetStatMod( who, stat_name )

	return CDbl( GetAttributeTemporaryMod( who, stat_name )) / 10.0;
endfunction

function AP_SetTrueStat( mobile, attributeid, points )

        var result;
	if( points >= 0 )
		result := SetAttributeBaseValue( mobile, attributeid, CInt( CDbl( points ) * 10 + 0.1 ));
	else
		result := SetAttributeBaseValue( mobile, attributeid, CInt( CDbl( points ) * 10 ));
	endif
 
	RecalcVitals( mobile );
        return result;
endfunction

function AP_SetStatMod( mobile, stat_name, points )

	SetAttributeTemporaryMod( mobile, stat_name, CInt( CDbl( points ) * 10.0 ));
	RecalcVitals( mobile );
endfunction

function AP_ModifyStatMod( mobile, stat_name, points )

	var orig_mod := AP_GetStatMod( mobile, stat_name );
	var new_mod := CDbl( points ) + CDbl( orig_mod );

	AP_SetStatMod( mobile, stat_name, new_mod );

	return orig_mod;
endfunction
