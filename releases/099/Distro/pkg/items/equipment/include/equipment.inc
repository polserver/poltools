/*
        Maintened by *Edwards
        For FantasiaShard.com
        
        Edwards@FantasiaShard.com
        
        2009-02-22
        
        Last Modifications 2009-11-16
             * Added now defaultdex from crafted items will apply
               Note: Cannot mix defaultdex with stats/skills mods together
               
        Last Modifications 2010-03-17
             * Added check if can apply mods for the skill
*/
use uo;
use cfgfile;

include ":attributes:attributes";
include ":attributes:titles";

const EQUIP_ITEMDESC_MODS	:= 0x1;
const EQUIP_CUSTOM_MODS	        := 0x2;
const EQUIP_BOTH_MODS		:= 0x3;
const EQUIP_CSTM_OR_ITEMDESC	:= 0x4;

function ApplyEquipmentMods( mobile, item, flags := EQUIP_CSTM_OR_ITEMDESC )

	var attrib_mods := GetEquipmentModifiers( item, flags );

	foreach modifier in ( attrib_mods )
                if( !modifier )
                        continue;
                endif
  
		var attrib_name := _modifier_iter,
                    amount := CInt( modifier ),
                    skill_val := AP_GetTrueSkill( mobile, attrib_name ),
                    cur_val := AP_GetSkillMod( mobile, attrib_name ),
  		    new_val := cur_val + amount;
                
                if( attrib_name in AP_ListAttributesByType( "Skill" ))
                	if( AP_CheckSkillModulation( skill_val, amount ))
                        	SetObjProperty( item, "EquippedMods", amount );
				AP_SetStatMod( mobile, attrib_name, new_val );
                	endif
                else
                        SetObjProperty( item, "EquippedMods", amount );
			AP_SetStatMod( mobile, attrib_name, new_val );
                endif

		SleepMS(2);
	endforeach

	return 1;
endfunction

function RemoveEquipmentMods( mobile, item, flags:=EQUIP_CSTM_OR_ITEMDESC )

	var attrib_mods := GetEquipmentModifiers( item, flags );

	foreach modifier in ( attrib_mods )
                if( !modifier )
                        continue;
                endif
                
		var attrib_name := _modifier_iter,
		    amount := CInt( modifier ),
		    cur_val := AP_GetSkillMod( mobile, attrib_name ),
  		    new_val := cur_val - amount;

                if( GetObjProperty( item, "EquippedMods" ))
			AP_SetStatMod( mobile, attrib_name, new_val );
                endif

		SleepMS(2);
	endforeach
 
        EraseObjProperty( item, "EquippedMods" );

	return 1;
endfunction

function GetEquipmentModifiers( item, flags:=EQUIP_CSTM_OR_ITEMDESC )

	var cstm_mods;
	if( flags & EQUIP_CUSTOM_MODS || flags & EQUIP_CSTM_OR_ITEMDESC )
		cstm_mods := GetObjProperty( item, "AttribMods" );
		if( Lower( TypeOf( cstm_mods )) != "dictionary" )
			cstm_mods := dictionary;
		endif
		
		if( flags & EQUIP_CSTM_OR_ITEMDESC )
			return cstm_mods;
		endif
	endif
	
	var itemdesc_mods := dictionary;
	if( flags & EQUIP_ITEMDESC_MODS || flags & EQUIP_CSTM_OR_ITEMDESC )
 
		var item_cfg := ReadConfigFile( ":*:itemdesc" );
		    item_cfg := item_cfg[item.objtype];
      
		var modifier := Abs( item_cfg.DefaultDex ),
                    cust_modifier := CInt( GetObjProperty( item, "DefaultDex" ));

                if( cust_modifier )
                         modifier := modifier - cust_modifier;
                endif
                
                itemdesc_mods["dexterity"] := -modifier;
	endif
	
	if( flags & EQUIP_BOTH_MODS )
		// Note: Custom mods override itemdesc.cfg ones here.
		foreach modifier in ( cstm_mods )
			itemdesc_mods.Insert( _modifier_iter, modifier );
			SleepMS(2);
		endforeach
	endif
	
	if( itemdesc_mods.Size() > 0 )
		return itemdesc_mods;
	else
		return cstm_mods;
	endif
endfunction

function AddModifierToEquipment( item, attrib_name, modifier )
	
	var cstm_mods := GetObjProperty( item, "AttribMods" );
	if( Lower( TypeOf( cstm_mods )) != "dictionary" )
		cstm_mods := dictionary;
	endif
	
	modifier := CDbl( modifier );
	if( modifier == 0.0 )
		return error{"errortext":="No modifier value was set."};
	elseif( !AP_GetAttributeCfgElem( attrib_name ))
		return error{"errortext":="That is not a valid attribute name."};
	endif
	
	cstm_mods[attrib_name] := modifier;
	
	return SetObjProperty( item, "AttribMods", cstm_mods );
endfunction

function RemoveModifierFromEquipment( item, attrib_name )
	
	var cstm_mods := GetObjProperty( item, "AttribMods" );
	cstm_mods.Erase( attrib_name );
	
	if( cstm_mods > 0 )
		return SetObjProperty( item, "AttribMods", cstm_mods );
	else
		// No point in keeping an empty dictionary cprop.
		return EraseObjProperty( item, "AttribMods" );
	endif
endfunction
