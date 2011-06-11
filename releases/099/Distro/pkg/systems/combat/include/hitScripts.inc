/*
        Maintened by *Edwards
        For FantasiaShard.com
        
        Edwards@FantasiaShard.com
        
        2009-10-18
*/
use uo;
use os;

include ":armor:armorZones";
include ":brainai:npcUtil";

function GetWeaponHitScripts( mobile, object )

        var hit_script;
        if( mobile.IsA( POLCLASS_NPC ))
                var npc_cfg := NPC_GetNPCConfig( mobile.npctemplate );
                var script := npc_cfg.HitScript;
                if( script )
                        hit_script := script;
                endif
        elseif( object.IsA( POLCLASS_WEAPON ))
	        var script := GetObjProperty( object, "HitScript" );
                if( script )
                        hit_script := script;
                endif
        endif
        
	return hit_script;
endfunction

function GetArmorHitScript( attacker, defender, rawDamage )

        var onhit_script;
        
        if( defender.IsA( POLCLASS_NPC ))
                var npc_cfg := NPC_GetNPCConfig( defender.npctemplate );
                
                onhit_script := npc_cfg.OnHitScript;
                if( onhit_script )
                        var script := start_script( onhit_script, {attacker, defender, rawDamage} );
                        if( script.errortext )
                                SendSysMessage( defender, "Unable to start ["+onhit_script+"] -->"+script.errortext );
                        endif
                        return 1;
                endif
        else
                var equipment := CS_GetEquipmentInArmorZone( defender, CS_GetRandomArmorZone() );
                var object := equipment.RandomEntry();

                onhit_script := GetObjProperty( object, "OnHitScript" );
                if( onhit_script )
                        var script := start_script( onhit_script, {attacker, defender, rawDamage, object} );
                        if( script.errortext )
                                SendSysMessage( defender, "Unable to start ["+onhit_script+"] -->"+script.errortext );
                        endif
                        return 1;
                endif
        endif

	return 0;
endfunction
