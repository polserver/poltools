/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        Last Modifications 03-31-2011
*/
use uo;

include ":areas:managment";
include ":totem:totem";

function CanAttack( attacker, defender )

        if( attacker.serial == defender.serial )
		return 0;
    	elseif( attacker.dead || defender.dead )
		return 0;
        elseif( defender.concealed )
		return 0;
        elseif( defender.cmdlevel > attacker.cmdlevel )
                return 0;
        elseif( A_IsIn( defender, AREAS_NO_DAMAGE ) && ( !defender.IsA( POLCLASS_NPC ) || IsTotem( defender )))
                return 0;
	endif

        return 1;
endfunction
