/*
        Maintened by *Edwards

        2009-02-09
*/
use uo;
use os;
use vitals;
use cfgfile;

function AP_GetVitalsCfgFile()

	var cfg_file := ReadConfigFile( ":attributes:vitals" );
	if( cfg_file.errortext )
		SysLog( "Error::AP_GetVitalsCfgFile() - Unable to open vitals.cfg ->"+cfg_file.errortext );
	endif

	return cfg_file;
endfunction

function AP_GetVitalCfgElem( vital_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := AP_GetVitalsCfgFile();
	endif

	var cfg_elem := cfg_file[vital_name];
	if( cfg_elem.errortext )
		SysLog( "Error::AP_GetVitalsCfgElem() - unable to find cfg elem ["+vital_name+"] ->"+cfg_elem.errortext );
	endif

	return cfg_elem;
endfunction

function AP_GetVitalNames()

	return GetConfigStringKeys( AP_GetVitalsCfgFile() );
endfunction

function AP_GetVital( mobile, vital_name )

	return CDbl( GetVital( mobile, vital_name )) / 100.0;
endfunction

function AP_GetVitalMaximumValue( mobile, vital_name )

	return CDbl( GetVitalMaximumValue( mobile, vital_name )) / 100.0;
endfunction

function AP_GetVitalRegenRate( mobile, vital_name )

	return GetVitalRegenRate( mobile, vital_name );
endfunction

function AP_SetVital( mobile, vital_name, amount )

	return SetVital( mobile, vital_name, CInt( CDbl( amount ) * 100.0 ));
endfunction

function AP_ConsumeVital( mobile, vital_name, points )

	points := CDbl( points );
	var cur_amount := AP_GetVital( mobile, vital_name );

	if( points > cur_amount )
		return error{ "errortext":="Insufficient points - unable to consume." };
	else
		cur_amount := cur_amount - points;
		AP_SetVital( mobile, vital_name, cur_amount );
		return 1;
	endif
endfunction

function AP_RefreshVitals( mobile )

	var vital_cfg := ReadConfigFile( ":attributes:vitals" );
	foreach elem_name in GetConfigStringKeys( vital_cfg )
		AP_SetVital( mobile, elem_name, AP_GetVitalMaximumValue( mobile, elem_name ));
		SleepMS(2);
	endforeach

	return 1;
endfunction
