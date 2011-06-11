/*
        Maintened by *Edwards

        2011-01-25
*/
use uo;

include ":gumps:gumps";
include ":gumps:gumps_ex";

function YesNo( mobile, prompt, delay:=0 )

        var parms := array{mobile, prompt, delay};

        var script := Run_Script( ":gumps:yesNoMiniGump", parms );
        if( script.errortext )
                SendSysMessage( mobile, "Error starting script <:gumps:yesNoMiniGump> -->"+script.errortext );
                return 0;
        endif

	return script;
endfunction
