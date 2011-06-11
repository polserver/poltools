/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2008-10-19
        
        Last Modifications 2009-10-28
             * Cleaned codes for better performances
             * To-do remove GetInstrumentEffects()
*/
use uo;
use os;

include "include/itemUtil";
include ":attributes:attributes";
include ":brainAI:npcSettings";
include ":crafting:toolWear";
include ":spells:spells";

function CheckInstrument( mobile )

        var instrument;
        foreach item in EnumerateItemsInContainer( mobile.backpack )
                if( item.IsInstrument() )
                        //No choice to call ITEM_INCONTAINER even if it is a double check...
                        if( useItem_check( mobile, item, ITEM_INCONTAINER ))
                                instrument := item;
                                break;
                        endif
                endif
                SleepMS(5);
        endforeach
        
        if( !instrument )
                return 0;
        endif
        
        CheckToolWear( mobile, instrument, MUSICIANSHIP );
        
        return 1;
endfunction

function CheckStamina( mobile, info )
        
	var spell_elem := SPELL_GetSpellsCfgElem( info.spell_id ),
            stam := spell_elem.StamCost;

        if( GetObjProperty( mobile, BARD ))
                stam /= ClasseBonus( mobile, BARD );
        endif

        if( !AP_ConsumeVital( mobile, STAMINA, Cint( stam )))
                return 0;
        endif

        return 1;
endfunction

function cast_Song( mobile )

        if( GetObjProperty( mobile, BARD ) || GetObjProperty( mobile, PP ) || mobile.IsA( POLCLASS_NPC ))
                //Can cast the spell...
                return 1;
        endif
        
        return 0;
endfunction

function SONG_GetEfficiency( mobile, value )

	if( GetObjProperty( mobile, BARD ))
		value *= ClasseBonus( mobile, BARD );
	endif

	var penalty := CInt( SPELL_GetPenalty( mobile ));
	if( penalty >= 100 )
		return 0;
	endif

	return CInt( value * ( 100 - penalty ) / 100 );
endfunction

function SONG_GetAmount( mobile )

	var value := CInt( AP_GetSkill( mobile, MUSICIANSHIP ) / 10 ),
	    amount := SONG_GetEfficiency( mobile, RandomInt(6)+ value + 8 );

	if( !amount )
		amount := 1;
	endif

	return CInt( amount );
endfunction

function SONG_GetDuration( mobile )

        var value := CInt( AP_GetSkill( mobile, MUSICIANSHIP ) * 6 ),
	    duration := SONG_GetEfficiency( mobile, value );

	if( !duration )
		duration := 1;
	endif

	return CInt( duration );
endfunction

function SONG_MathDamage( mobile, circle, targRadius := 0 )

	if( targRadius )
                circle -= 3;
        elseif( circle < 1 )
                circle := 1;
        endif

        var modifier := CInt( AP_GetSkill( mobile, MUSICIANSHIP ) / 5 ),
            amount := RandomDiceRoll( circle * 3+"d5+"+modifier );

        if( amount > CInt( circle * ( 13 + circle )))
                amount := CInt( circle * ( 13 + circle ));
        endif

	amount := CInt( SONG_GetEfficiency( mobile, amount ) / 5 );

	return amount;
endfunction

function GetMobileDifficulty( mobile, creature )
        
        if( creature.IsA( POLCLASS_ITEM ))
                //There is no difficulty if it is an item.
                return 0;
        endif
        
        var difficulty := 0;
        if( creature.IsA( POLCLASS_NPC ))
                var npc_elem := NPC_GetNPCConfig( creature.npctemplate ),
                taming := CInt( npc_elem.TameSkill );
                
                difficulty := CInt( AP_GetStat( creature, STRENGTH ) / 10 );
                if( difficulty > 400 )
                        difficulty := 400;
                endif
                
                difficulty /= ClasseBonus( mobile, BARD );
                
                if( taming && taming < difficulty )
                        difficulty := taming;
                endif
        else
                difficulty := 120;
        endif
        
        return difficulty;
endfunction

function MainCheckTarget( mobile, targ, range )

        if( mobile.serial == targ.serial )
		SendSysMessage( mobile, "You should consider therapy!" );
		return 0;
	elseif( Distance( mobile, targ ) > range )
		SendSysMessage( mobile, "You are too far away." );
		return 0;
	elseif( !CheckLineOfSight( mobile, targ ))
		SendSysMessage( mobile, "You cannot see "+targ.desc+" from here." );
		return 0;
        endif

        return 1;
endfunction

function GetBardRange( mobile )

        var range := CInt( 8 * ClasseBonus( mobile, BARD ));
        
        return range;
endfunction
