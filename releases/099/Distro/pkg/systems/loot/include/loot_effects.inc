/*
        Created by *Edwards

        2008-04-27

        Last Modifications 2009-06-07
             * Fixed few details so now it uses magicLevel from NPCDesc

        Last Modifications 2009-10-03
             * Modified all RandomInt() for RandomDiceRoll()
             * Modified all chances like is was with FS-6.1 ( easier to get items now )
             * Added ApplyElementalProtection(), ApplyImmunity(), ApplyProtection()

        AddName( item )
        AddRandomColor( item )

        ApplyARMod( item )
        ApplyArSkillMod( item )
        ApplyDmgMod( item )
        ApplyElementalProtection( item )
        ApplyEnchant( item )
        ApplyHPMod( item )
        ApplyImmunity( item )
        ApplyMiscArMod( item )
        ApplyMiscSkillMod( item )
        ApplyProtection( item )
        ApplyStatMod( item )

        SetHitscript( item, modifier, multiplier )
        SetOnHitscript( item, modifier, multiplier )
        
        GetChanceLevel()
*/
use uo;
use os;

include ":attributes:attributes_ex";
include ":loot:lootParser";
include ":equipment:equipment";

const WEAPONS := "Weapons";
const ARMORS := "Armors";
const SHIELDS := "Shields";
const CLOTHINGS := "Clothings";
const JEWELRY := "Jewelry";
const PROFESSIONAL := "Professionnal";

const MAX_COLOR	:= 1000;

function AddName( item )

   	var old_desc := item.desc;
   	if( old_desc[1] == "a" || old_desc[1] == "an" )
      		var space := Find( item.desc, " ", 1 );
      		var newname := old_desc[space + 1, Len( item.desc )];
      		SetName( item, "a magic "+newname );
   	else
      		SetName( item, "magic "+old_desc );
   	endif

	if( GetObjProperty( item, "IsGMItem" ))
		return 0;
	endif

	SetObjProperty( item, "UnIDedMagicalItem", 1 );

        return 1;
endfunction

function AddRandomColor( item )

        SetObjProperty( item, "Undyeable", 1 );
	if( !GetObjProperty( item, "EnchantColor" ))
		var color := RandomDiceRoll( "1d"+MAX_COLOR+"+1" );
		SetObjProperty( item, "EnchantColor", color );
	endif

        return 1;
endfunction

function ApplyARMod( item )

       	item.ar_mod := CInt( GetChanceLevel() * 5 );

	//Check for another enchantment
   	if( RandomDiceRoll( "1d100+1" ) <= ( 10 * magicLevel ))
       		if(( RandomDiceRoll( "1d100+1" )) <= 75 )
       			ApplyHPMod( item );
		else
			ApplyArSkillMod( item );
		endif
   	endif

        return 1;
endfunction

function ApplyArSkillMod( item )

	if( RandomDiceRoll( "1d1000+1" ) <= 5 )
		ApplyStatMod( item );
		return 1;
	endif

	var skill_id;
	case( RandomDiceRoll( "1d2+1" ))
		1:
                        skill_id := 26;  //Resisting Spells
			break;

		default: skill_id := 27; //Tactics
	endcase

        var skill_name := AP_SkillIdToAttributeName( skill_id );
        AddModifierToEquipment( item, skill_name, GetChanceLevel() );

	//Check for another enchantment
	if( RandomDiceRoll( "1d100+1" ) <= CInt( 10 * magicLevel ))
		ApplyHPMod( item );
	endif

        return 1;
endfunction

function ApplyDmgMod( item )

	item.dmg_mod := CInt( GetChanceLevel() * 5 );

   	if( RandomDiceRoll( "1d100+1" ) <= CInt( 10 * magicLevel ))
       		if( RandomDiceRoll( "1d100+1" ) <= 75 )
       			ApplyHPMod( item );
		endif
   	endif

        return 1;
endfunction

function ApplyElementalProtection( item )

        var chance_case := RandomDiceRoll( "1d8+1" ),
            level := GetChanceLevel(),
            element;
            
	case( chance_case )
		1:	element := "FireProtection";
			break;

		2:	element := "WaterProtection";
			break;

		3:	element := "AirProtection";
			break;

		4:	element := "EarthProtection";
			break;

		5:	element := "NecroProtection";
			break;

		6:	element := "HolyProtection";
			break;

		7:	element := "HealingIncrease";
			break;

		8:	element := "PhysicalProtection";
			break;

		9:	element := "FreeAction";
                        level := 1;
			break;
	endcase

	SetObjProperty( item, element, level );

        return 1;
endfunction

function ApplyEnchant( item )

        var chances := CInt( RandomDiceRoll( "1d100+1" ) * magicLevel );
	if( chances < 300 )
		ApplyProtection( item );
	elseif( chances < 500 )
		ApplyElementalProtection( item );
	else
		ApplyImmunity( item );
	endif

	//Check for another enchantment
    	if( RandomDiceRoll( "1d100+1" ) <= CInt( 10 * magicLevel ))
       		if( RandomDiceRoll( "1d100+1" ) <= 75 )
       			ApplyMiscSkillMod( item );
		else
			ApplyMiscArMod( item );
		endif
   	endif

        return 1;
endfunction

function ApplyHPMod( item )

	item.maxhp_mod := CInt( GetChanceLevel() * 10 );
   	item.hp := item.maxhp;

        return 1;
endfunction

function ApplyImmunity( item )

        var chance_case := RandomDiceRoll( "1d2+1" ),
            level := GetChanceLevel(),
            element;
            
	case( chance_case )
		1:	element := "PermSpellReflection";
			break;

		2:	element := "PermMagicProtection";
			break;

		3:	element := "PermPoisonProtection";
			break;
	endcase

	SetObjProperty( item, element, level );

        return 1;
endfunction

function ApplyMiscArMod( item )

        var level := GetChanceLevel();

        SetObjProperty( item, "ArBonus", level );

   	//Check for another enchantment
   	if( RandomDiceRoll( "1d100+1" ) <= CInt( 10 * magicLevel ))
		ApplyMiscSkillMod( item );
   	endif

        return 1;
endfunction

function ApplyMiscSkillMod( item )

	if( RandomDiceRoll( "1d1000+1" ) <= 5 )
		ApplyStatMod( item );
		return 1;
	endif

	var level := GetChanceLevel(),
	    skill_id := RandomDiceRoll( "1d49-1" );

        var skill_name := AP_SkillIdToAttributeName( skill_id );
        AddModifierToEquipment( item, skill_name, level );

        return 1;
endfunction

function ApplyProtection( item )

        var chance_case := RandomDiceRoll( "1d2+1" ),
            charges := CInt( GetChanceLevel() * 5 ),
            element;
            
	case( chance_case )
		1:	element := "SpellReflection";
			break;

		2:	element := "MagicProtection";
			break;

		3:	element := "PoisonProtection";
			break;
	endcase

	SetObjProperty( item, element, charges );

        return 1;
endfunction

function ApplyStatMod( item )

        var level := CInt( GetChanceLevel() * 5 );

 	case( RandomDiceRoll( "1d3" ))
		1:
                        AddModifierToEquipment( item, "Strength", level );
			break;
		2:
                        AddModifierToEquipment( item, "Dexterity", level );
			break;
                3:
                        AddModifierToEquipment( item, "Intelligence", level );
			break;
	endcase

	//Check for another enchantment
	if( RandomDiceRoll( "1d100+1" ) <= CInt( 2 * magicLevel ))
		ApplyHPMod( item );
	endif

        return 1;
endfunction

function SetHitscript( item )

        var effects_cfg := ReadConfigFile( ":loot:effects_hit" ),
            effects_hit := GetConfigIntKeys( effects_cfg ),
            effect := effects_hit.RandomEntry(),
            cfg_entry := effects_cfg[effect];

        if( cfg_entry.HitScriptType == "Greater" )
                //Add a diifficulty if the first random pick is within the "greater" effects.
                effect := effects_hit.RandomEntry();
        endif
        
        if( cfg_entry.errortext )
		return 0;
        endif

	//Apply the hitscript
	item.hitscript:= cfg_entry.hitscript;
	SetObjProperty( item, "HitScriptNumber", effect );

	var cprop := cfg_entry.CProp;
	if( cprop )
		var power := CInt( cfg_entry.CPropVal );
		if( !power )
			power := CInt(( RandomDiceRoll( "1d10+1" ) * magicLevel ) / 5 );
		endif
  
		if( cprop == "PoisonLevel" )
			power /= 2;
                        if( power > 6 )
                        	power := 6;
                        endif
		endif
  
		if( power < 1 )
			power := 1;
		endif

                if( cprop == "Hits" || cprop == "Mana" || cprop == "Stamina" )
			SetObjProperty( item, "DrainVital", cprop );
                else
			SetObjProperty( item, cprop, CInt( power ));
                endif
	endif

	var type := cfg_entry.Type;
        if( type )
		SetObjProperty( item, "SlayType", type );
        endif
        
	var spell := cfg_entry.Spellid;
        if( spell )
		SetObjProperty( item, "HitWithSpell", spell );
  
                var circle := CInt(( RandomDiceRoll( "1d100+1" ) * magicLevel ) / 100 ),
                    circle_mod := cfg_entry.AsCircleMod;
                    
                if( circle_mod )
                        circle += circle_mod;
                endif
                if( circle < 1 )
                        circle := 1;
                endif

		SetObjProperty( item, "EffectCircle", circle );
  
                var chance_effect := CInt( RandomDiceRoll( "1d5+3" ) * magicLevel );
		SetObjProperty( item, "ChanceOfEffect", chance_effect );
        endif

	//Check for another enchantment
	if( cfg_entry.Place == "Prefix" )
		if( RandomDiceRoll( "1d10+1" ) <= magicLevel )
			if( RandomDiceRoll( "1d100+1" ) <= 75 )
       				ApplyDmgMod( item );
                        endif
		endif

	else
		if( RandomDiceRoll( "1d10+1" ) <= magicLevel )
			if( RandomDiceRoll( "1d100+1" ) <= 90 )
       				ApplyHPMod( item );
			endif
		endif
	endif

	return 1;
endfunction

function SetOnHitscript( item )

        var OnEffects_cfg := ReadConfigFile( ":loot:effects_onhit" ),
            OnEffects_hit := GetConfigIntKeys( OnEffects_cfg ),
            OnEffect := OnEffects_hit.RandomEntry(),
            cfg_entry := OnEffect[OnEffect];
            
        if( cfg_entry.OnHitScriptType == "Greater" )
                //Add a diifficulty if the first random pick is within the "greater" effects.
                OnEffect := OnEffects_hit.RandomEntry();
        endif

        if( cfg_entry.errortext )
		return 0;
        endif
        
	//Apply the hitscript
	SetObjProperty( item, "OnHitScriptNumber", OnEffect );

	var cprop := cfg_entry.CProp;
	if( cprop )
		var power := cfg_entry.CPropVal;
		if( !power )
			power := CInt(( RandomDiceRoll( "1d10+1" ) * magicLevel ) / 6 );
   
			var mult := cfg_entry.Multiplier;
			if( mult )
				power *= mult;
			endif
   
			if( cprop == "PoisonLevel" )
				power /= 2;
                        	if( power > 6 )
                        		power := 6;
                        	endif
			endif

			if( power < 1 )
				power := 1;
			endif
		endif

		SetObjProperty( item, cprop, CInt( power ));
	endif

	var type := cfg_entry.Type;
        if( type )
		SetObjProperty( item, "ProtectedType", type );
        endif

	var spell := cfg_entry.Spellid;
        if( spell )
		SetObjProperty( item, "HitWithSpell", spell );

                var circle := CInt(( RandomDiceRoll( "1d100+1" ) * magicLevel ) / 100 ),
                    circle_mod := cfg_entry.AsCircleMod;

                if( circle_mod )
                        circle += circle_mod;
                endif
                if( circle < 1 )
                        circle := 1;
                endif

		SetObjProperty( item, "EffectCircle", circle );

                var chance_effect := CInt( RandomDiceRoll( "1d5+3" ) * magicLevel );
		SetObjProperty( item, "ChanceOfEffect", chance_effect );
        endif

	//Check for another enchantment
	if( RandomDiceRoll( "1d10+1" ) <= magicLevel )
		if( RandomDiceRoll( "1d100+1" ) <= 90 )
       			ApplyHPMod( item );
		endif
	endif

	return 1;
endfunction

function GetChanceLevel()

        magicLevel += 2;
        
        var chances := CInt( RandomDiceRoll( "1d100+1" ) * magicLevel );
 
	case( magicLevel )
		5:	if( chances < 280 )
				chances += RandomDiceRoll( "1d70+110" );
			endif
			break;
		6:	if( chances < 280 )
				chances += RandomDiceRoll( "1d70+140" );
			endif
			break;
		7:	if( chances < 280 )
				chances += RandomDiceRoll( "1d70+170" );
			endif
			break;
                default:
                        break;
	endcase

        var level;
	if( chances < 140 )
       		level := 1;
   	elseif( chances < 280 )
       		level := 2;
   	elseif( chances < 420 )
       		level := 3;
   	elseif( chances < 560 )
       		level := 4;
   	elseif( chances < 630 )
       		level := 5;
	else
		level := 6;
   	endif
    
        return level;
endfunction
