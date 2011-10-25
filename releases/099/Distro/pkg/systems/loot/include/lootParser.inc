/*
        Created by *Edwards

        2008-04-27
        
        Last Modifications 2009-01-31
             * Fixed Loot_ParseString() && Apply_Effect()
             
        Last Modifications 2009-06-08
             * Completed the config files
             * Completed the systems
             * Modified the % for almost every loot_effects()

        Last Modifications 2009-10-03
             * Changed all RandomInt() for RandomDiceRoll()
             * Modified all chances like it was with FS-6.1 ( easier to get items now )

        Last Modifications 2010-01-07
             * Added cursed randomisation ( 10% chances to be cursed )
             
        NpcDesc Entries:
        
        LootIndex               #
        MagicLevel              %
        MagicChance             %
*/
use uo;
use util;
use cfgfile;

include ":itemutils:itemdesc";
include ":itemutils:itemInfo";
include ":loot:loot_effects";
include ":areas:managment";

var magicLevel := 0;
var loot_index := 0;

function Loot_Generate( container, npc_cfg, useLoot_Index := 0 )

	if( !container.IsA( POLCLASS_CONTAINER ))
		PrintTextAbove( container, "Object passed was not a container." );
		return 0;
        elseif( GetObjProperty( container, "LFucker" ))
		return 0;
	endif

	var index_cfg := ReadConfigFile( ":loot:loot-index" );
	if( index_cfg.errortext )
		PrintTextAbove( container, "Error - Could not open loot-index.cfg -> "+index_cfg.errortext );
		return 0;
	endif
 
        if( !useLoot_Index )
                loot_index := CInt( npc_cfg.LootIndex );
                magicLevel := RandomDiceRoll( "1d4" );
        else
                loot_index := CInt( npc_cfg );
                loot_index := CInt( npc_cfg );
        endif

        if( !loot_index )
                return 0;
        elseif( A_IsIn( container, AREAS_DANGEROUS ))
                magicLevel += 1;
        endif
        
        if( GetObjProperty( "NPCBoss" ))
                magicLevel += 2;
        endif

	var index_elem := index_cfg[loot_index];
	if( !index_elem )
		PrintTextAbove( container, "Error - loot-index.cfg elem ["+loot_index+"] does not exist." );
                return 0;
	endif

	foreach line in GetConfigStringArray( index_elem, "Item" )
                var parsed := Loot_ParseString( line );
	        var i;
	        for( i:=1; i<= parsed.amount; i+=1 )
			var creation := Loot_CreateItem( container, parsed, magicLevel );
                	if( creation.errortext )
				PrintTextAbove( container, "Error -->"+creation.errortext );
                	endif
                        SleepMS(5);
                endfor
                
		SleepMS(2);
	endforeach

	foreach line in GetConfigStringArray( index_elem, "Stack" )
                var parsed := Loot_ParseString( line );
		var creation := Loot_CreateItem( container, parsed, magicLevel );
                if( creation.errortext )
			PrintTextAbove( container, "Error -->"+creation.errortext );
                endif
		SleepMS(2);
	endforeach

	foreach line in GetConfigStringArray( index_elem, "Random" )
                var parsed := Loot_ParseString( line );
	        var i;
	        for( i:=1; i<= parsed.amount; i+=1 )
		        var creation := Loot_CreateRandom( container, parsed, magicLevel );
                        if( creation.errortext )
			        PrintTextAbove( container, "Error -->"+creation.errortext );
                        endif
                        SleepMS(5);
                endfor
                
		SleepMS(2);
	endforeach

	foreach line in GetConfigStringArray( index_elem, "Group" )
		if( line != loot_index )
			//To do:
			// Check to make sure were not going back into the same group
			// to avoid a potential endless loop!
			Loot_Generate( container, line );
		endif
		SleepMS(2);
	endforeach

	return 1;
endfunction

function Loot_CreateItem( container, parsed, magicLevel )
 
        var item;
	if( Loot_ChanceCheck( parsed ))
		var graphic := GetObjTypeGraphic( parsed.objtype );
		var stackable := IU_GetTileInfo( graphic ).Stackable;
		if( stackable )
                        item := CreateItemInContainer( container, parsed.objtype, parsed.amount );
			if( item.errortext )
				PrintTextAbove( container, "Error - Could not create ["+parsed.name+"] ->"+item.errortext );
                		return 0;
			endif
		else
			item := CreateItemInContainer( container, parsed.name );
			if( item.errortext )
				PrintTextAbove( container, "Error - Could not create ["+parsed.name+"] ->"+item.errortext );
                		return 0;
			endif
   
			var is_magic := MagicAllowedandType( parsed.name );
			if( is_magic )
				Apply_Effect( item, is_magic, magicLevel );

                                if( RandomDiceRoll( "1d10" == 5 ))
                                        SetObjProperty( item, "Cursed", 1 );
                                endif
			endif
		endif
	endif

	return 1;
endfunction

function Loot_CreateRandom( container, parsed, magicLevel )
 
	var groups_cfg := ReadConfigFile( ":loot:loot-groups" );
	if( groups_cfg.errortext )
		PrintTextAbove( container, "Error - Could not open loot-groups.cfg -> "+groups_cfg.errortext );
                return 0;
	endif

	var groups_elem := groups_cfg[parsed.name];
	if( !groups_elem )
		PrintTextAbove( container, "Error - loot-groups.cfg elem ["+parsed.name+"] does not exist." );
                return 0;
	endif

	var items := GetConfigStringArray( groups_elem, "Item" );
        var randItem := items.RandomEntry();
        var elem := SplitWords( randItem );
        
        var item_chances := CDbl( elem[2] );
        if( !item_chances )
		item_chances := 100;
        endif
        
        var i;
        for( i:=0; i<=5; i+=1 )
		if( RandomDiceRoll( "1d100" ) <= item_chances )
                	parsed.name := elem[1];
                        Loot_CreateItem( container, parsed, magicLevel );
                        break;
		endif
                SleepMS(5);
        endfor

	return 1;
endfunction

function Loot_ChanceCheck( parsed )

	if( RandomDiceRoll( "1d1000" ) <= CInt( CDbl( parsed.chance ) * 10.0 ))
		return 1;
	else
		return 0;
	endif
endfunction

function Loot_ParseString( line )

	line := SplitWords( CStr( line ));
 
	var amount := RandomDiceRoll( CStr( line[1] ));
	if( !amount )
		amount := 1;
	endif
 
        if( RandomDiceRoll( "1d100" ) <= 10 )
                amount *= 2;
        endif

	var name := CStr( line[2] );
	var objtype;
	if( GetObjTypeByName( name ))
		objtype := GetObjTypeByName( name );
	else
		objtype := CInt( name );
	endif
 
        var chance := CDbl( line[3] );
	if( !chance )
		chance := 100.0;
	endif

	var result := struct;
        result.+name := name;
        result.+objtype := objtype;
        result.+chance := chance;
 	result.+amount := amount;

	return result;
endfunction

function MagicAllowedandType( objname )

	var magic_cfg := ReadConfigFile( ":loot:enchantable" );
        if( magic_cfg.errortext )
	       SysLog( "Error reading cfg file <loot:enchantable> -->"+magic_cfg.errortext );
               return 0;
        endif

   	var first_elem := FindConfigElem( magic_cfg, WEAPONS );
   	var first_data := GetConfigInt( first_elem, objname );
   	if( first_data )
      		return WEAPONS;
   	endif

   	var second_elem := FindConfigElem( magic_cfg, ARMORS );
   	var second_data := GetConfigInt( second_elem, objname );
   	if( second_data )
      		return ARMORS;
   	endif

   	var third_elem := FindConfigElem( magic_cfg, SHIELDS );
   	var third_data := GetConfigInt( third_elem, objname );
   	if( third_data )
      		return SHIELDS;
   	endif

	var fourth_elem := FindConfigElem( magic_cfg, CLOTHINGS );
	var fourth_data := GetConfigInt( fourth_elem, objname );
	if( fourth_data )
		return CLOTHINGS;
	endif

	var fifth_elem := FindConfigElem( magic_cfg, JEWELRY );
	var fifth_data := GetConfigInt( fifth_elem, objname );
	if( fifth_data )
		return JEWELRY;
	endif

	var sixth_elem := FindConfigElem( magic_cfg, PROFESSIONAL );
	var sixth_data := GetConfigInt( sixth_elem, objname );
	if( sixth_data )
		return PROFESSIONAL;
	endif

   	return 0;
endfunction

function Apply_Effect( item, magic, magicLevel )

        var chances := CInt( RandomDiceRoll( "1d100" ) * magicLevel );

        case( magic )
           	WEAPONS:
                	if( chances < 200 )
				ApplyHPMod( item );
			elseif( chances < 500 )
				ApplyDmgMod( item );
			else
				SetHitscript( item );
			endif
			break;
   
           	ARMORS:
	                if( chances < 300 )
				ApplyHPMod( item );
			elseif( chances < 500 )
				ApplyARMod( item );
			elseif( chances < 600 )
				ApplyArSkillMod( item );
			else
				SetOnHitscript( item );
			endif
			break;
   
	 	SHIELDS:
	 		if( chances < 200 )
				ApplyHPMod( item );
                        else
				ApplyARMod( item );
			endif
			break;
   
		CLOTHINGS:
			if( chances < 400 )
				ApplyMiscArMod( item );
			else
				ApplyMiscSkillMod( item );
			endif

			AddRandomColor( item );
			break;
   
		JEWELRY:
	                if( chances < 400 )
				ApplyMiscArMod( item );
			elseif( chances < 600 )
				ApplyMiscSkillMod( item );
			else
                                ApplyEnchant( item );
			endif
			break;
   
		PROFESSIONAL:
                        if( chances < 300 )
				ApplyHPMod( item );
                        else
				ApplyMiscSkillMod( item );
                        endif
			break;
	        default:
        endcase
        
        AddName( item );

        return 1;
endfunction
