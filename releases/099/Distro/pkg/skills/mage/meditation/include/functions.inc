/*
        Maintened by *Edwards
        For FantasiaShard.com
        
        Edwards@FantasiaShard.com
        
        2008-10-13
        
        Last Modifications 2009-01-18
             * Finally Added code so we now regenerate MANA!! WOOOOT ! ...
             
        Last Modifications 2009-08-20
             * Generalized canMedit()
*/
use uo;
use os;
use cfgfile;

include ":attributes:attributes";
include ":classes:classes";
include "include/client";
include ":spells:spells_math";

function StartMeditating( mobile )

	var x := mobile.x,
	    y := mobile.y,
	    health := GetVital( mobile, HITS ),
	    delay := 5,
	    classe := GetObjProperty( mobile, MAGE ),
	    skill := AP_GetSkill( mobile, MEDITATION ),
	    base := CInt(( skill / 25 ) + ( AP_GetStat( mobile, INTELLIGENCE ) / 35 ));
     
        if( classe )
		delay /= ClasseBonus( mobile, MAGE );
		base *= ClasseBonus( mobile, MAGE );
	endif
 
        SendSysMessage( mobile, "You enter a meditative trance." );
	SetObjProperty( mobile, "#LastMeditation", ReadGameClock() + 30 );
 
        PrintTextAbove( mobile, "*Meditating*" );
        PlaySoundEffect( mobile, 0x00fa );

        while( mobile.x == x && mobile.y == y && health <= GetVital( mobile, HITS ) && canMedit( mobile ))
               
               var modifier := 100 - CInt( SPELL_GetPenalty( mobile ) * 2 );
               if( modifier > 0 )
			var restored := CInt( base * modifier / 100 );
			if( restored )
                                AP_SetVital( mobile, MANA, AP_GetVital( mobile, MANA ) + restored );
                                if( AP_GetVital( mobile, MANA ) > AP_GetVitalMaximumValue( mobile, MANA ))
			                 AP_SetVital( mobile, MANA, AP_GetVitalMaximumValue( mobile, MANA ));
                                         break;
                                endif
			else
				SendSysMessage( mobile, "Regenerative forces cannot penetrate your armor." );
				break;
			endif
               else
			SendSysMessage( mobile, "Regenerative forces cannot penetrate your armor." );
			break;
               endif

               Sleep( CInt( delay ));
	endwhile
 
        SendSysMessage( mobile, "You stop meditating..." );
 
	return 1;
endfunction

function canMedit( mobile )

	if( AP_GetVital( mobile, MANA ) >= AP_GetVitalMaximumValue( mobile, MANA ))
		SendSysMessage( mobile, "You are at peace." );
		return 0;
        elseif( mobile.poisoned )
                SendSysMessage( mobile, "You can't meditate while poisoned." );
                return 0;
        elseif( mobile.warmode )
                SendSysMessage( mobile, "You can't meditate in war mode." );
                return 0;
        elseif( !InvalidInHands( mobile ))
                SendSysMessage( mobile, "You can't meditate with something in hands." );
                return 0;
        endif
        
        return 1;
endfunction

function InvalidInHands( mobile )

	var hand1 := GetEquipmentByLayer( mobile, LAYER_HAND1 ),
	    hand2 := GetEquipmentByLayer( mobile, LAYER_HAND2 ),
            cfg := ReadConfigFile( ":*:itemdesc" );

        if(( hand1 && cfg[hand1.objtype].CanCastIfInHand != 1 ) || ( hand2 && cfg[hand2.objtype].CanCastIfInHand != 1 ))
                return 0;
	endif

	return 1;
endfunction
