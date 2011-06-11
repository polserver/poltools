/*
        Maintened by *Edwards
        For FantasiaShard.com
        
        Edwards@FantasiaShard.com
        
        2009-08-26
        
        Last Modifications 2009-10-19
             * Added CS_GetEquipmentInArmorZone() in DealAcidDamage()

        Last Modifications 2009-10-26
             * Added elemental damage functions
*/
use uo;
use cfgfile;
use util;
use vitals;

include "include/packets";
include ":armor:armorZones";
include ":brainAI:npcUtil";
include ":damage:damage";
include ":equipment:damageItem";
include ":equipment:protections";

function SpitAcid( spitter, spit_on )

	if( !CheckLineOfSight( spitter, spit_on ))
		return 0;
	endif
 
	PlayMovingEffectXYZHued( spitter, spit_on, 0x36d4, 5, 1, 1, 0x47, 0 );
        PlaySoundEffect( spitter, 399 );
 
	var acid_pool := CreateItemAtLocation( spit_on.x, spit_on.y, GetMapInfo( spit_on.x, spit_on.y ).z, "acid",  1, spit_on.realm );
	var dice_dealt := DealAcidDamage( spitter, spit_on, acid_pool );
	if( TypeOf( dice_dealt ) == "String" && !GetObjProperty( acid_pool, "%AcidDice%" ))
		SetObjProperty( acid_pool, "%AcidDice%", dice_dealt );
	endif
 
	return acid_pool;
endfunction

function DealAcidDamage( npc, mobile, acid_pool, acid_damage:=0 )

	var acid_dice := GetAcidDiceFromTemplate( npc );
	if( TypeOf( acid_dice ) != "String" )
		acid_dice := GetAcidDiceFromItem( acid_pool );
	endif
 
	if( acid_damage )
		acid_damage := RandomDiceRoll( acid_damage );
	elseif( TypeOf( acid_dice ) == "String" )
		acid_damage := RandomDiceRoll( acid_dice );
	else
		return 0;
	endif
        
	if( RandomInt(10) > 5 )
		DamageItem( mobile );
	endif

        var amount := PROT_GetElementalDamage( mobile, acid_damage, DMG_ACID );
        ApplyDamageEX( mobile, amount, DMG_ACID, npc );
 
	return acid_dice;
endfunction

function GetAcidDiceFromTemplate( npc )

	if( npc.IsA( POLCLASS_NPC ))
		var npc_info := NPC_GetNPCConfig( npc.npctemplate );
		if( npc_info.AcidDamage )
			return npc_info.AcidDamage;
		endif
	endif
 
	return 0;
endfunction

function GetAcidDiceFromItem( acid_pool )

	if( acid_pool.IsA( POLCLASS_ITEM ))
		if( TypeOf( GetObjProperty( acid_pool, "%AcidDice%" )) == "String" )
			return GetObjProperty( acid_pool, "%AcidDice%" );
		endif
	endif
 
	return 0;
endfunction
