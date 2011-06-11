/*
        Maintened by *Edwards
        
        2008-12-12
        
        Last Modifications 2008-12-12
             * Now every "resurrection scripts" must return 1 for player being resurected
             * Fixed Okay button gumps
             * Added security check with x,y,z,realm positions
             
        Last Modifications 2009-02-21
             * Removed gump thingy.
             * Added the check if player want to see controlgump.
*/
use uo;

include ":gumps:gumps";
include ":gumps:gumps_ex";
include ":control:internal/icp-util";
include ":control:internal/cmdbar";
include "include/sounds";

function ResurrectionGump( mobile )

        var result := Resurrection( mobile );
        if( result.errortext )
                return 0;
        endif
        
	return 1;
endfunction

function Resurrection( mobile )

	if( !mobile.IsA( POLCLASS_MOBILE ))
		return error{ "errortext":="object is not a mobile." };
	elseif( !mobile.dead )
		return error{ "errortext":="mobile is not dead." };
	endif

	var result := Resurrect( mobile );
        if( result.errortext )
                SendSysMessage( mobile, "Error couldn't ressurect -->"+result.errortext );
                return 0;
        endif

	mobile.graphic := mobile.trueobjtype;
	PlaySoundEffect( mobile, SFX_RESURRECT );

        SetObjProperty( mobile, "#JustRess", ReadGameClock() + 20 );
 
        if( mobile.cmdlevel )
	        if( !ICP_LoadUserSetting(mobile, "NoCmdBarLogin") && !mobile.dead && mobile.cmdlevel )
		         Start_Script(":control:commands/coun/cmdbar", mobile);
	        endif
        endif

	return 1;
endfunction

