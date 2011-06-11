/*
        Created by *Edwards

        2008-10-24
        
        Last Modifications 2008-12-12
             * Revised code and fixed few CInt issues
             * Fixed value "value" in SPELL_GetResist()
             
        Last Modifications 2008-12-29
             * SPELL_GetResist() replace evalint for magery if mobile is NPC
             
        Last Modifications 2009-01-18
             * Less chance to resist spell now
             * [FIXED] Must success on checkskill to resist
             
        Last Modifications 2009-02-07
             * Added toggle for resisting spells with SPELL_ApplyDamage()

        Last Modifications 2009-10-26
             * Changed how to handle protections with new functions in :equipment:protections

        Last Modifications 2009-10-26
             * Removed toggle for resisting spells with SPELL_ApplyDamage()
        
        Thanks to ZuluHotel and PolDistroTeam for this excellent ideas
*/
use uo;
use os;
use util;
use basic;

include "include/string";
include ":attributes:attributes";
include ":classes:classes";
include ":damage:damage";
include ":combat:critical";
include ":equipment:protections";

const NPC_MODIFIER := 4;

function SPELL_GetEfficiency( mobile, value )

	if( GetObjProperty( mobile, MAGE ))
		value *= ClasseBonus( mobile, MAGE );
	elseif( GetObjProperty( mobile, WARRIOR ))
		value /= ClasseBonus( mobile, WARRIOR );
	endif

	var penalty := CInt( SPELL_GetPenalty( mobile ));
	if( penalty >= 100 )
		return 0;
	endif

	return CInt( value * ( 100 - penalty ) / 100 );
endfunction

function SPELL_GetPenalty( mobile )

	var penalty := 0;
	if( mobile.IsA( POLCLASS_NPC ))
                return 0;
        endif

        var cfg_desc := ReadConfigFile( ":*:itemdesc" );
        
	foreach item in ListEquippedItems( mobile )
                SleepMS(5);
                
                var cfg_penalty := cfg_desc[item.objtype].MagicPenalty;
		if( cfg_penalty )
			penalty += cfg_penalty;
			continue;
		endif
                
                var prop_penalty := GetObjProperty( item, "MagicPenalty" );
		if( prop_penalty )
			penalty += prop_penalty;
			continue;
		endif
	endforeach

	if( penalty > 100 )
		penalty := 100;
	endif

	return CInt( penalty );
endfunction

function SPELL_GetAmount( mobile )

	var value := CInt( AP_GetSkill( mobile, MAGERY ) / 10 ),
	    amount := SPELL_GetEfficiency( mobile, RandomInt(6) + value + 10 );

	if( !amount )
		amount := 1;
	endif

	return CInt( amount );
endfunction

function SPELL_GetDuration( mobile )

        var value := CInt( AP_GetSkill( mobile, MAGERY ) * 10 ),
	    duration := SPELL_GetEfficiency( mobile, value );
     
	if( !duration )
		duration := 1;
	endif

	return CInt( duration );
endfunction

function SPELL_MathDamage( mobile, circle, targRadius := 0 )

	if( targRadius )
                circle -= 3;
        endif
        
        if( circle < 1 )
                circle := 1;
        endif
        
        var modifier := CInt( AP_GetSkill( mobile, MAGERY ) / 5 ),
            amount := RandomDiceRoll(( circle * 3 )+"d5+"+modifier ),
            max_damage := CInt( circle * ( 10 + circle ));
            
        if( amount > max_damage )
                amount := max_damage;
        endif

	amount := CInt( SPELL_GetEfficiency( mobile, amount ) / 5 );

	return amount;
endfunction

function SPELL_ApplyDamage( targ, amount, type, mobile, element:=DMG_MAGIC )

	amount := PROT_GetElementalDamage( targ, amount, element );
        amount := CriticalHit( mobile, targ, amount );
        
        ApplyDamageEX( targ, CInt( amount ), DMG_MAGIC + type, mobile );

        return amount;
endfunction

function SPELL_GetImmunity( mobile, targ, circle )

	var perm_protection := GetMobileProtection( targ, "PermMagicProtection" ),
            targ_protection := GetMobileProtection( targ, "MagicProtection" ),
            item;

        if( targ.IsA( POLCLASS_NPC ))
                if( perm_protection >= circle || targ_protection >= circle )
 		        PrintTextAbovePrivate( targ, "The spell dissipates upon contact with "+targ.name+"'s magical barrier!", mobile );
                        return "Immuned";
                endif
                return 0;
        endif
        
	foreach thing in ListEquippedItems( targ )
                var item_perma := CInt( GetObjProperty( thing, "PermMagicProtection" )),
                    item_charge := CInt( GetObjProperty( thing, "MagicProtection" ));
                    
                if( perm_protection && item_perma == perm_protection )
                        item := thing;
                	break;
                elseif( targ_protection && item_charge == targ_protection )
			if( item_charge <= 0 )
				SetMobileProtection( targ, "MagicProtection", 0 );
				EraseObjProperty( thing, "MagicProtection" );
				PrintTextAbovePrivate( targ, UCFirst( thing.desc )+" is out of charges!", targ );
			else
				item := thing;
				break;
			endif
                endif
		SleepMS(5);
	endforeach
 
        if( !item )
                //Couldn't find any protection.
                return 0;
        elseif( GetObjProperty( item, "Cursed" ))
		PrintTextAbovePrivate( targ, "Your "+TruncateArticle( item.desc )+" increase the effect of the spell!", targ );
                return "Cursed";
        endif
        
        var item_perma := CInt( GetObjProperty( item, "PermMagicProtection" )),
            item_charge := CInt( GetObjProperty( item, "MagicProtection" ));

        if( perm_protection && item_perma < circle )
                return 0;
        elseif( targ_protection && item_charge )
                if( item_charge < circle )
                        return 0;
                endif

                item_charge -= circle;
                SetObjProperty( item, "MagicProtection", CInt( item_charge ));
        endif

	PrintTextAbovePrivate( targ, mobile.name+"'s spell is absorbed by your "+TruncateArticle( item.desc )+"!", targ );
 	if( mobile != targ )
 		PrintTextAbovePrivate( targ, "The spell dissipates upon contact with "+targ.name+"'s magical barrier!", mobile );
 	endif

        return "Immuned";
endfunction

function SPELL_Reflected( mobile, targ, circle )
 
	var perm_reflection := GetMobileProtection( targ, "PermSpellReflection" ),
            targ_reflection := GetMobileProtection( targ, "SpellReflection" ),
            item;

        if( targ.IsA( POLCLASS_NPC ))
                if( perm_reflection >= circle || targ_reflection >= circle )
                        SendSysMessage( mobile, "The spell is reflected back to you!" );
                        return "Reflected";
                endif
                return 0;
        endif
        
	foreach thing in ListEquippedItems( targ )
                var item_perma := CInt( GetObjProperty( thing, "PermSpellReflection" )),
                    item_charge := CInt( GetObjProperty( thing, "SpellReflection" ));

                if( perm_reflection && item_perma == perm_reflection )
                        item := thing;
                	break;
                elseif( targ_reflection && item_charge == targ_reflection )
			if( item_charge <= 0 )
				SetMobileProtection( targ, "SpellReflection", 0 );
				EraseObjProperty( thing, "SpellReflection" );
				PrintTextAbovePrivate( targ, UCFirst( thing.desc )+" is out of charges!", targ );
			else
				item := thing;
				break;
			endif
                endif
		SleepMS(5);
	endforeach
 
        if( !item )
                //Couldn't find any protection.
                return 0;
        elseif( GetObjProperty( item, "Cursed" ))
		PrintTextAbovePrivate( targ, "Your "+TruncateArticle( item.desc )+" prevent all reflections!", targ );
                return 0;
        endif

        var item_perma := CInt( GetObjProperty( item, "PermSpellReflection" )),
            item_charge := CInt( GetObjProperty( item, "SpellReflection" ));
                    
        if( perm_reflection && item_perma < circle )
                return 0;
        elseif( targ_reflection && item_charge )
                if( item_charge < circle )
                        return 0;
                endif
                
                item_charge -= circle;

                SetObjProperty( item, "SpellReflection", CInt( item_charge ));
        endif

        PrintTextAbovePrivate( targ, "Your "+TruncateArticle( item.desc )+" reflected the spell!", targ );
        SendSysMessage( mobile, "The spell is reflected back to you!" );
 
        return "Reflected";
endfunction

function SPELL_GetResist( mobile, targ, circle, value )

	var mobile_magery := AP_GetSkill( mobile, MAGERY ),
	    mobile_evalint := AP_GetSkill( mobile, EVALUATING_INTELLIGENCE ),
	    targ_resist := AP_GetSkill( targ, RESISTING_SPELLS );
     
        if( mobile.IsA( POLCLASS_NPC ))
                 mobile_evalint := mobile_magery;
        endif
     
	var chance := CInt(( targ_resist - (( mobile_magery / 4 ) + ( circle * 3 ))) * 0.8 );

	if( GetObjProperty( targ, MAGE ))
		chance *= ClasseBonus( targ, MAGE );
	elseif( GetObjProperty( targ, WARRIOR ) )
                chance /= ClasseBonus( targ, WARRIOR );
	endif

	if( GetObjProperty( mobile, MAGE ))
		chance /= ClasseBonus( mobile, MAGE );
	endif
        
        if( SkillCheck( targ, RESISTING_SPELLS, CInt( circle * 8 )) > 0 )
                if( RandomInt(100) <= chance )
                        if( !GetObjProperty( targ, WARRIOR ))
                                SendSysMessage( targ, "You feel yourself resisting magical energy!" );
                                PlaySoundEffect( targ, 0x01e7 );
                                PlayObjectCenteredEffect( targ, 0x01e2, 5, 5 );
                                
		                value /= 2;
		                if( value < 1 )
			                value := 1;
		                endif
                        endif
                endif
        endif

	value *= CDbl(( mobile_evalint - targ_resist ) / 200 ) + 1;
        if( !targ.IsA( POLCLASS_NPC ))
                value *= 0.55;
        else
                value *= NPC_MODIFIER;
        endif
 
	if( GetObjProperty( targ, WARRIOR ) )
                value *= ClasseBonus( targ, WARRIOR );
	endif

	return CInt( value );
endfunction
