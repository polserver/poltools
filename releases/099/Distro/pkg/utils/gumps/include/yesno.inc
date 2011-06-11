/*
        Maintened by *Edwards
        
        2010-01-27
*/
use uo;
use os;

include ":gumps:gumps";
include ":gumps:gumps_ex";

function YesNo( mobile, prompt:="", yes_text:="Yes", no_text:="No", delay:=0 )

        var parms := array{mobile, prompt, yes_text, no_text, delay};
        
        var script := Run_Script( ":gumps:yesNoGump", parms );
        if( script.errortext )
                SendSysMessage( mobile, "Error starting script <:gumps:yesNoGump> -->"+script.errortext );
                return 0;
        endif
 
	return script;
endfunction
