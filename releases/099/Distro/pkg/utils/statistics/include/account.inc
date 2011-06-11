/*
        Created by *Edwards

        2008-06-16
*/
use uo;
use os;

include ":datafile:datafile";

var stat_datafile := DFOpenDataFile( ":statistics:statistics", DF_CREATE );

function GetAccountProperty( acct_name, property )

        var data_elem := DFFindElement( stat_datafile, acct_name );

	return data_elem.GetProp( property );
endfunction

function SetAccountProperty( acct_name, property, value )

        var data_elem := DFFindElement( stat_datafile, acct_name, DF_CREATE );

	return data_elem.SetProp( property, value );
endfunction

function EraseAccountProperty( acct_name, property )

        var data_elem := DFFindElement( stat_datafile, acct_name );

	return data_elem.EraseProp( property );
endfunction
