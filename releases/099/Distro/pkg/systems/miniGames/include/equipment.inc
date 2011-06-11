/*
        Maintened by *Edwards
        
        2009-10-04
        
        Thanks to ZuluHotel and last maintened Montuz for the wonderfull idea
*/
use os;
use os;

function DestroyEquipment( who, item )

    	var cursed := GetObjProperty( item, "Cursed" );
    	if( cursed )
    		SetObjProperty( item, "Cursed", 3 );
    	endif

   	if( who.backpack )
    		var move_item := MoveItemToContainer( item, who.backpack );
                if( move_item.errortext )
                       SendSysMessage( who, "Error couldn't move item to your backpack -->"+move_item.errortext );
                       return 0;
                endif
    	endif

    	return DestroyItem( item );
endfunction
