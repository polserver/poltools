/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2008-06-16
*/
use uo;
use os;

include ":datafile:datafile";
include ":statistics:account";

var g_datafile := DFOpenDataFile( ":statistics:statistics", DF_CREATE );

function GetMonthlyOnlineTimeRanking( acctserial, monthstring )

	var total_PlayTime := GetAccountProperty( acctserial, monthstring );
	if( !total_PlayTime )
		total_PlayTime := 0;
	endif

	var search_OnlineTime,
            position := 1;

	foreach account in ListAccounts()
		if( account.serial != acctserial )
			data_elem := DFFindElement( g_datafile, account.serial );
			if( data_elem )
				search_OnlineTime := entry.GetProp( monthstring );
				if( search_OnlineTime && search_OnlineTime > total_PlayTime )
					position += 1;
				endif
			endif
		endif
                SleepMS(10);
	endforeach

	return position;
endfunction

function getReelPlayTime( chr )

        var script := Start_Script( ":statistics:playTime/endTimer", chr );
        if( script.errortext )
	       SysLog( script.errortext );
        endif

        var script2 := Start_Script( ":statistics:playTime/startTimer", chr );
        if( script2.errortext )
	       SysLog( script2.errortext );
        endif

        return 1;
endfunction
