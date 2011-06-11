/*
        Created by *Edwards
        For FantasiaShard.com
        
        Edwards@FantasiaShard.com
        
        2010-08-28
        
        Idea from Loony the magic. He took it from Diablo. Well, it's now UO feature.. :)
*/
use util;

include ":containers:containers";
include ":loot:lootParser";

const MERCHANT_RESTOCK := 60*45; // 45minutes

function Swindler_Check_Restock( npc, forsale )

        var next_restock := CInt( GetObjProperty( npc, "NextRestock" ));
        if( next_restock <= ReadGameClock() )
                Swindler_RestockInventory( npc, forsale );
                SetObjProperty( npc, "NextRestock", ReadGameClock() + MERCHANT_RESTOCK );
        endif

	return CPM_GetMerchantContainer( npc, CPM_SHOPPINGCART, CP_CREATE );
endfunction

function Swindler_RestockInventory( npc, byref inventory )

	foreach item in ( ListRootItemsInContainer( inventory ))
		DestroyItem( item );
		SleepMS(2);
	endforeach
        
        var i;
        for( i:=1; i<=5; i+=1 )
                var level := RandomInt(5)+1;
                Loot_Generate( inventory, level, 1 );
                SleepMS(5);
        endfor
        
	foreach item in ( ListRootItemsInContainer( inventory ))
		SetPrice( npc, item );
		SleepMS(2);
	endforeach

	return 1;
endfunction

function SetPrice( npc, item )

	var itemdesc := ReadConfigFile( ":*:itemdesc" ),
            cfg_elem := itemdesc[item.objtype];
            
	if( cfg_elem.errortext )
                AI_Speak( npc, "Error reading itemdesc entry for <"+item.objtype+"> -->"+cfg_elem.errortext );
		return 0;
	endif

	var item_price := CInt( cfg_elem.VendorSellsFor );
	if( !item_price )
                DestroyItem( item );
		return 0;
	endif

        item.sellprice := RandomDiceRoll( item_price+"d5000+"+( item_price * 10 ));

	return 1;
endfunction
