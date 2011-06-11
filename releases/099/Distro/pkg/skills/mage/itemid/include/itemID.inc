/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-09-29
        
        Last Modifications by MrJim 2010-12-04
             * Charges, % and poison level given everytime you use item id on the item.
*/
use uo;
use os;

include "include/itemUtil";
include ":attributes:attributes";
include ":loot:identify";

function IDItem( mobile, item )
 
 	var charges := GetObjProperty( item, "Charges" ),
            chance_effect := GetObjProperty( item, "ChanceOfEffect" ),
	    poison_lvl := GetObjProperty( item, "PoisonLevel" );

        if( GetObjProperty( item, "UnIDedMagicalItem" ) ||
                GetObjProperty( item, "IsGMItem" ) ||
                GetObjProperty( item, "EnchantColor" ))
                
                NameByEnchant( item );
        endif

        if( item.container.IsA( POLCLASS_CORPSE ) && GetObjProperty( item.container, "npctemplate" ))
                PrintTextAbove( item.container, item.desc );
        endif
        
        SendSysMessage( mobile, "It appears to be "+item.desc+"." );

        if( charges )
	        SendSysMessage( mobile, "There is "+charges+" charges left." );
        elseif( chance_effect )
	        SendSysMessage( mobile, "It has "+chance_effect+"% chances of effects," );
        elseif( poison_lvl )
	        SendSysMessage( mobile, "Cursed with poison level "+poison_lvl+"." );
        endif

        SetObjProperty( item, "IDed", mobile.serial );
 
	return 1;
endfunction

function checkID( mobile, item )

        var is_id := CInt( GetObjProperty( item, "IDed" ));
        if( is_id )
                return 0;
        elseif( !canID( item ))
                SetObjProperty( item, "IDed", mobile.serial );
                return 0;
        endif

        return 1;
endfunction

function canID( item )

        if( item.IsA( POLCLASS_CONTAINER ))
                return 0;
        elseif( IsStackable( item, item ))
                return 0;
        elseif( item.IsDecoDeed() )
                return 0;
        elseif( !item.movable )
                return 0;
        elseif( item.IsRecallRune() )
                return 0;
        endif

        return 1;
endfunction
