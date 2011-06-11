/*
        Created by *Edwards

        2009-10-19
*/
use uo;
use os;

function SendReportGump( mobile )

        var script := Start_Script( ":death:deathScripts/reportMurder", mobile );
        if( script.errortext )
	        SendSysMessage( mobile, "Error starting script <:death:deathScripts/reportMurder> -->"+script.errortext );
        endif
        
        return 1;
endfunction
