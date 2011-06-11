/*
        Created by *Edwards
        For FantasiaShard.com
        
        Edwards@FantasiaShard.com
        
        2011-04-09
*/
use uo;
use os;

include ":attributes:attributes";
include ":classes:classes";

function DetectHidden( mobile, hider )

        if( !hider.hidden )
                //Just make sure our hider is hidden before performing the check.
                return 1; //Already revealed.
        endif

	var mobile_skill := AP_GetSkill( mobile, DETECTING_HIDDEN ),
	    hider_skill  := AP_GetSkill( hider, HIDING );

	mobile_skill *= ClasseBonus( mobile, THIEF );
	hider_skill *= ClasseBonus( hider, THIEF );
 
        if( mobile.IsA( POLCLASS_NPC ))
                mobile_skill *= 2;
        endif
 
	var chance := CInt( mobile_skill - hider_skill );
        if( chance < 0 )
               chance := 5;
        elseif( chance < 10 )
               chance := 10;
        endif

	if( RandomInt( 100 ) > chance && !mobile.multi )
	       return 0;
	endif
 
        hider.hidden := 0;
        PrintTextAbovePrivate( hider, "You have been discovered!", hider );

	return 1;
endfunction
