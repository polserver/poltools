/*
        Made by *Edwards

        Great thanks to ZuluHotel, UnrealLegend and PolDistroTeam.
        
        Last Modifications 2010-02-23
*/
use uo;
use os;
use util;
use basic;

include ":areas:managment";
include ":armor:armorZones";
include ":combat:critical";
include ":combat:hitScripts";
include ":equipment:damageItem";
include ":equipment:protections";
include ":timedScripts:timedScripts";

const DIVIDER := 3;
const MULTIPLIER := 0.01;
const PLAYER_MODIFIER := 1.00;

function Damage_Modification( damageType, attacker, defender, weaponElem, typeSkill, typeVital )

        var attri := weaponElem.Attribute,
            rawDamage := RandomDiceRoll( weaponElem.Damage ),
            skill := AP_GetSkill( attacker, attri ),
            attackerWeapon := attacker.weapon;

        if( attacker.IsA( POLCLASS_NPC ))
                var npc := NPC_GetNPCConfig( attacker.npctemplate );

                attri := npc.AttackAttribute;
                skill := AP_GetSkill( attacker, attri );
                
                rawDamage := CDbl(( RandomDiceRoll( npc.AttackDamage ) + 10 ));
        else
                rawDamage += CDbl((( rawDamage + attackerWeapon.dmg_mod ) * attackerWeapon.hp ) / ( attackerWeapon.maxhp + attackerWeapon.maxhp_mod ));
        endif

        PlayHitSound( attacker, defender, weaponElem );

        rawDamage := CalcPercent( typeskill, rawDamage );
        rawDamage := CalcPercent( skill, rawDamage );
        rawDamage := CalcPercent( typeVital, rawDamage );
        rawDamage /= DIVIDER;

        rawDamage := CalcRawDamage( attacker, defender, rawDamage, damageType, attackerWeapon );

        var applyType;
        case( damageType )
               1:   applyType := DMG_PHYSICAL;
                    break;
               2:   applyType := DMG_RANGED;
                    break;
               3:   applyType := DMG_ASTRAL;
                    break;
        endcase

        ApplyDamageEX( defender, rawDamage, applyType, attacker );

        return 1;
endfunction

function CalcRawDamage( attacker, defender, rawDamage, damageType, attackerWeapon )

        VerifyPoison( attacker, defender, attackerWeapon );

        rawDamage := CalcByDist( attacker, defender, rawDamage, damageType );
        rawDamage := CalcByClasse( attacker, defender, rawDamage, damageType );
        rawDamage := CriticalHit( attacker, defender, rawDamage );
        rawDamage := ModifyByProt( defender, rawDamage, damageType );
        rawDamage := PiercingCheck( attacker, defender, rawDamage, damageType );
        rawDamage := CalcByArea( attacker, defender, rawDamage );
        
        DamageItem( attacker, attackerWeapon ); //Damage weapon only
        DamageItem( defender ); //Damage random armor
        
        if( RunScripts( attacker, defender, rawDamage ))
               return 0; //Do not apply damage twice...
        endif

        return CDbl( rawDamage / ( 0.50 + PLAYER_MODIFIER ));
endfunction

function CalcPercent( modulation, value )

        var percent := ( ( modulation / DIVIDER ) * MULTIPLIER ) + 1.0;
        var xDamage := ( value * percent );

        return CDbl( xDamage );
endfunction

function CalcByDist( attacker, defender, rawDamage, damageType )

        if( damageType == 2 && !attacker.IsA( POLCLASS_NPC ))//Ranged
                var dist := Distance( attacker, defender );
		if( dist <= 1 || dist > 10 )
		        rawDamage *= 0.25;
		endif
        endif
        
        return CDbl( rawDamage );
endfunction

function CalcByClasse( attacker, defender, rawDamage, damageType )
                
        if( damageType == 1 ) //Physical
                if( GetObjProperty( attacker, WARRIOR ))
		        rawDamage *= ClasseBonus( attacker, WARRIOR );
                endif
                if( GetObjProperty( defender, WARRIOR ))
		        rawDamage /= ClasseBonus( defender, WARRIOR );
                endif

                if( GetObjProperty( attacker, MAGE ))
                        rawDamage /= ClasseBonus( attacker, MAGE );
                endif
                if( GetObjProperty( defender, MAGE ))
                        rawDamage *= ClasseBonus( defender, MAGE );
                endif
        elseif( damageType == 2 ) //Ranged
                if( GetObjProperty( attacker, RANGER ))
		        rawDamage *= ClasseBonus( attacker, RANGER );
                endif

                if( GetObjProperty( attacker, MAGE ))
                        rawDamage /= ClasseBonus( attacker, MAGE );
                endif
                if( GetObjProperty( defender, MAGE ))
                        rawDamage *= ClasseBonus( defender, MAGE );
                endif
        elseif( damageType == 3 ) //Astral
                if( GetObjProperty( defender, MAGE ))
		        rawDamage /= ClasseBonus( defender, MAGE );
                endif
        endif

        return CDbl( rawDamage );
endfunction

function ModifyByProt( defender, rawDamage, damageType )

	var protection;
        if( damageType == 1 || damageType == 2 ) //Physical or Ranged
                protection := GetMobileProtection( defender, "PhysicalProtection" );
        else
                protection := GetMobileProtection( defender, "PermMagicProtection" );
        endif
 
	if( protection )
		rawDamage *= ( 1 - protection * 0.05 );
	endif
 
        return rawDamage;
endfunction

function PiercingCheck( attacker, defender, rawDamage, damageType )

        var xHitScript := GetWeaponHitScripts( attacker, attacker.weapon );
        if( xHitScript == ":weapons:hitScripts/piercing" )
              SendSysMessage( attacker, "Your weapon pierces through the armor!" );
              return rawDamage;
        endif

        if( !GetObjProperty( defender, CRAFTER ))
                var armorDefender := CInt( defender.ar + defender.ar_mod );
                if( armorDefender )
                        if( armorDefender > 120 )
                                armorDefender := 120 + CInt(( armorDefender - 120 ) * 0.4 );
                        endif

                        rawDamage := DamageTakeCalc( armorDefender, rawDamage );
                endif
        endif
        
        rawDamage := ParryCheck( attacker, defender, rawDamage, damageType );
        
        return CDbl( rawDamage );
endfunction

function DamageTakeCalc( armorDefender, rawDamage )

        var percent := CDbl( 1 + armorDefender / DIVIDER ) * MULTIPLIER;
        If( percent >= 0.9 )
		percent := 0.9;
        endif

        rawDamage -= CDbl( rawDamage * percent );

        return CDbl( rawDamage );
endfunction

function ParryCheck( attacker, defender, rawDamage, damageType )

        var parry_Chance := AP_GetSkill( defender, PARRY ),
            attackChance := RandomInt( 300 );
            
        if( defender.shield || ( defender.IsA( POLCLASS_NPC ) && parry_Chance ))

                 if( GetObjProperty( defender, WARRIOR ))
                          parry_Chance *= ClasseBonus( defender, WARRIOR );
                 endif

                 if( damageType == 1 && GetObjProperty( attacker, WARRIOR ))
                          attackChance *= ClasseBonus( attacker, WARRIOR );
                 elseif( damageType == 2 && GetObjProperty( attacker, RANGER ))
                          attackChance *= ClasseBonus( attacker, RANGER ) - 100;
                 endif

                 if( attackChance >= parry_Chance )
                          if( SkillCheck( defender, PARRY, -1 ) > 0 )
                                    SendSysMessage( defender, "You successfully parry the attack." );
                                    return 0;
                          endif
                 endif
         endif
         
         return CDbl( rawDamage );
endfunction

function CalcByArea( attacker, defender, rawDamage )

         var dangerous_zone := A_IsIn( attacker, AREAS_DANGEROUS );
         
         if( attacker.IsA( POLCLASS_NPC ) && !defender.IsA( POLCLASS_NPC ))
                if( GetObjProperty( attacker, "NPCBoss" ))
                         rawDamage *= CDbl( 1.35 );
                endif
                
                if( dangerous_zone )
                        rawDamage *= CDbl( 1.15 );
                endif
         endif
         
         if( defender.IsA( POLCLASS_NPC ) && !attacker.IsA( POLCLASS_NPC ))
                if( GetObjProperty( defender, "NPCBoss" ))
                         rawDamage /= CDbl( 1.50 );
                endif

                if( dangerous_zone )
                        rawDamage /= CDbl( 1.15 );
                endif
         endif
         
         return CDbl( rawDamage );
endfunction

function VerifyPoison( attacker, defender, attackerWeapon )

        var level := 0,
            type := "DefaultPoison";
        
        if( attacker.IsA( POLCLASS_NPC ))
	        var npc_cfg := NPC_GetNPCConfig( attacker.npctemplate );
                type := CStr( npc_cfg.PoisonType );
                level := CInt( npc_cfg.PoisonLevel );
        else
                var charges := CInt( GetObjProperty( attackerWeapon, "PoisonCharges" ));
                if( !charges )
                        return 0;
                endif
                
                if( charges >= 1 )
                        level := CInt( GetObjProperty( attackerWeapon, "PoisonLevel" ));
                        SetObjProperty( attackerWeapon, "PoisonCharges", charges - 1 );
                else
                        EraseObjProperty( attackerWeapon, "PoisonCharges" );
                        EraseObjProperty( attackerWeapon, "PoisonLevel" );
                        return 0;
                endif
	endif
 
        if( level )
                var poison := TS_StartTimer( defender, type, "30d"+level, level, attacker );
                if( poison.errortext )
                          SysLog( "Error applying poison for "+attacker.name );
                endif
        endif
 
        return 1;
endfunction

function RunScripts( attacker, defender, rawDamage )

         var hitScript := GetWeaponHitScripts( attacker, attacker.weapon ),
             ret_val := 0;
             
         if( hitScript )
                var script := start_script( hitScript, {attacker, defender, attacker.weapon, rawDamage} );
                if( script.errortext )
                         SendSysMessage( attacker, "Unable to start ["+hitScript+"] -->"+script.errortext );
                endif
                
                ret_val := 1;
         endif
         
         GetArmorHitScript( attacker, defender, rawDamage );
         
         return ret_val;
endfunction
