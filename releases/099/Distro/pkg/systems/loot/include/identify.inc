/*
        Maintened by *Edwards

        2009-08-17
        
        Last Modifications 2009-09-29
             * Added AddArticle() when renaming item

        Last Modifications 2009-10-03
             * Fixed problem when Id warding items that has charges instead of levels
*/
use uo;
use os;
use cfgfile;

include "include/string";
include ":attributes:attributes";
include ":equipment:equipment";
include ":loot:identify_ex";

function NameByEnchant( item )

	EraseObjProperty( item, "UnIDedMagicalItem" );
      
        var base_name := GetObjProperty( item, "BaseName" );
        if( !base_name )
                var item_cfg := ReadConfigFile( ":*:itemdesc" ),
	            item_name := item_cfg[item.objtype].desc;

		SetName( item, item_name );
	else
		SetName( item, base_name );
	endif

        var enchanted_name := GetObjProperty( item, "Enchanted" );;
	if( enchanted_name )
		SetName( item, enchanted_name+" "+item.desc );
	endif

        var enchanted_color := GetObjProperty( item, "EnchantColor" );
	if( enchanted_color )
		item.color := enchanted_color;
                EraseObjProperty( item, "EnchantColor" );
	endif

        var isGMItem := GetObjProperty( item, "IsGMItem" );
	if( isGMItem )
		SetName( item, isGMItem );
		return 1;
	endif

	var hitScript := GetObjProperty( item, "HitScriptNumber" );
	if( hitScript )
		RenameByHitscript( item, hitScript );
	endif

	var onHitScript := GetObjProperty( item, "OnHitScriptNumber" );
	if( onHitScript )
		RenameByOnHitscript( item, onHitScript );
	endif

	var dmg_mode := item.dmg_mod;
	if( dmg_mode )
		RenameByMod( item, "Damage", dmg_mode );
	endif

	var ar_mode := item.ar_mod;
	if( ar_mode )
		RenameByMod( item, "Ar", ar_mode );
	endif

	var ar_bonus := GetObjProperty( item, "ArBonus" );
	if( ar_bonus )
		RenameByMod( item, "ArBonus", CInt( ar_bonus * 5 ));
	endif

	var hp_mode := item.maxhp_mod;
	if( hp_mode )
		RenameByMod( item, "MaxHP", CInt( hp_mode / 2 ));
	endif

        var skill_mod := GetEquipmentModifiers( item );
        if( skill_mod.Size() )
                var skill_keys := skill_mod.Keys();
                foreach modifier in skill_keys
                        RenameByAttriMod( item, modifier, skill_mod[modifier] );
                        SleepMS(5);
                endforeach
        endif

	foreach protection in immunities
                var mobile_prot := GetObjProperty( item, protection );
		if( mobile_prot )
			RenameByImmunity( item, protection, mobile_prot );
		endif
                SleepMS(5);
	endforeach
 
	foreach prot in wardings
                var charges := GetObjProperty( item, prot );
		if( charges )
			RenameByWardings( item, prot );
		endif
                SleepMS(5);
	endforeach

	foreach mod in regenerations
                var mobile_mod := GetObjProperty( item, mod );
		if( mobile_mod )
			RenameByImmunity( item, mod, mobile_mod );
		endif
                SleepMS(5);
	endforeach

	return 1;
endfunction

function RenameByHitscript( item, hitScript )

        var enchantment_cfg := ReadConfigFile( ":loot:effects_hit" );
        if( enchantment_cfg.errortext )
		PrintTextAbove( item, "Error reading config <:loot:effects_hit> -->"+enchantment_cfg.errortext );
		return 0;
        endif

	var chant_elem := FindConfigElem( enchantment_cfg, hitScript );
        if( chant_elem.errortext )
		PrintTextAbove( item, "Error reading chant_elem <"+hitScript+"> -->"+chant_elem.errortext );
		return 0;
        endif

	SetAdjective( item, chant_elem, "Name" );
	SetObjProperty( item, "HitScript", chant_elem.HitScript );

        return 1;
endfunction

function RenameByOnHitscript( item, onHitScript )

        var enchantment_cfg := ReadConfigFile( ":loot:effects_onhit" );
        if( enchantment_cfg.errortext )
		PrintTextAbove( item, "Error reading config <:loot:effects_onhit> -->"+enchantment_cfg.errortext );
		return 0;
        endif

	var chant_elem := FindConfigElem( enchantment_cfg, onHitScript );
        if( chant_elem.errortext )
		PrintTextAbove( item, "Error reading chant_elem <"+onHitScript+"> -->"+chant_elem.errortext );
		return 0;
        endif

	SetAdjective( item, chant_elem, "Name" );
	SetObjProperty( item, "OnHitScript", chant_elem.OnHitScript );

        return 1;
endfunction

function GetEnchantmentElem( item, elem )

        var enchantment_cfg := ReadConfigFile( ":loot:enchant_desc" );
        if( enchantment_cfg.errortext )
		PrintTextAbove( item, "Error reading config <:loot:enchant_desc> -->"+enchantment_cfg.errortext );
		return 0;
        endif

	var chant_elem := FindConfigElem( enchantment_cfg, elem );
        if( chant_elem.errortext )
		PrintTextAbove( item, "Error reading chant_elem <"+elem+"> -->"+chant_elem.errortext );
		return 0;
        endif

        return chant_elem;
endfunction

function RenameByAttriMod( item, attribute, value )

        if( !value )
                return 0;
        endif

        if( attribute in AP_ListAttributesByType( "Stat" ))
                value := CInt( value / 5 );
        endif

        var mod_elem := GetEnchantmentElem( item, attribute );
	var name := CStr ( "Name"+value );

	SetAdjective( item, mod_elem, name );

        return 1;
endfunction

function RenameByMod( item, mod, value )

        if( !value )
                return 0;
        endif

        var mod_elem := GetEnchantmentElem( item, mod );
        
	var temp := CInt( value / 5 );
        if( temp )
                var name := CStr( "Name"+temp );
                SetAdjective( item, mod_elem, name );
        endif

        return 1;
endfunction

function RenameByImmunity( item, immunity, power )

        var immunity_elem := GetEnchantmentElem( item, immunity );
	var name := CStr( "Name"+power );

	SetAdjective( item, immunity_elem, name );

        return 1;
endfunction

function RenameByWardings( item, immunity )

        var immunity_elem := GetEnchantmentElem( item, immunity );

	SetAdjective( item, immunity_elem, "Name" );

        return 1;
endfunction

function SetAdjective( item, elem, value )

	var colorkey := "Color";

	if( GetObjProperty( item, "Cursed" ))
		value := "C"+value;
		colorkey := "C"+colorkey;
	endif

	var position := GetConfigString( elem, "Place" );
        if( position.errortext )
		PrintTextAbove( item, "Error reading position <Place> -->"+position.errortext );
		return 0;
        endif

	var adjective := GetConfigString( elem, value );
        if( adjective.errortext )
		PrintTextAbove( item, "Error reading adjective <"+value+"> -->"+adjective.errortext );
		return 0;
        endif

	if( position == "Prefix" )
		SetName( item, Lower( adjective+" "+TruncateArticle( item.desc )));
	else
		SetName( item, Lower( TruncateArticle( item.desc )+" "+adjective ));
	endif

        SetName( item, AddArticle( item.desc ));
 
        var color_elem := GetConfigString( elem, colorkey );
	var color_item := CInt( color_elem );
	if( color_item )
		item.color := color_item;
	endif

        return 1;
endfunction
