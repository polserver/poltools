/*
        Maintened by *Edwards

        Last Modifications 2008-12-26
             * Added GetPotionStrength()
             * Removed type parm with CheckToDrink()
                  
        Last Modifications 2009-01-18
             * Magic Fish are now reading these functions, just make sure it tells the good things.
             * Also empty_bottle() has been modified with sfx and fx
                  
        Last Modifications 2010-03-09
             * Removed check for free hands.
*/
use uo;
use os;
use cfgfile;

include ":attributes:attributes";
include "include/client";

function CheckToDrink( character )
 
	if( GetObjProperty( character, "#CanDrink" ))
		if( CInt( GetObjProperty( character, "#CanDrink" )) > ReadGameClock() )
			SendSysMessage( character, "You must wait until you can use another of that." );
                        return 0;
		else
			SetObjProperty( character, "#CanDrink", ReadGameClock() + 10 );
		endif
	else
		SetObjProperty( character, "#CanDrink", ReadGameClock() + 10 );
	endif
 
	return 1;
endfunction

function empty_bottle( character, potion )

        if( potion.IsPotion() )
                PerformAction( character, ANIM_EAT );
		PlaySoundEffect( character, 0x32 );
	        CreateItemInBackpack( character, 0x0f0e, 1 );
	        SubtractAmount( potion, 1 );
        elseif( potion.IsMagicFish() )
                PerformAction( character, ANIM_EAT );
	        case( RandomInt(3) + 1 )
		    1:
			PlaySoundEffect( character, 0x3b );
			break;
		    2:
			PlaySoundEffect( character, 0x3c );
			break;
		    3:
			PlaySoundEffect( character, 0x3d );
			break;
	        endcase
         
	        SubtractAmount( potion, 1 );
        endif
        
        return 1;
endfunction

function GetPotionStrength( potion )

	var cfg := ReadConfigFile( ":alchemy:itemdesc" );
 
	var power := CInt( cfg[potion.objtype].Strength );
	if( !power )
		power := 1;
	endif

        var bonus := CInt( GetObjProperty( potion, "ByTrueMage" ));
	if( bonus )
                if( bonus > 10 )
                	bonus := 10;
                endif
                
		power += bonus;
	endif

	return power;
endfunction
