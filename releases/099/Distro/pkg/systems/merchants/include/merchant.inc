/*
        Maintened by *Edwards
        For FantasiaShard.com
        
        Edwards@FantasiaShard.com
        
        2009-07-26
        
        Last Modifications 2009-12-21
             * Added areas_value with stuff from NPC.
             * Cleaned codes for better performances
             
        Last Modifications 2010-03-12
             * Added Check_Restock()
             
        Last Modifications 2010-03-18
             * Added CanBuyItem()
*/
use util;

include ":areas:managment";
include ":areas:settings";
include ":brainAI:npcCommands";
include ":brainAI:npcUtil";
include ":containers:containers";

const MERCHANT_RESTOCK := 60*45; // 45minutes

function HasSellableItems( merchanttype, backpack )

	var backpack_items := ListRootItemsInContainer( backpack ),
	    buysell_cfg := ReadConfigFile( ":merchants:buySell" ),
	    prodgroups := FindConfigElem( buysell_cfg, merchanttype ),
	    buys := GetConfigStringArray( prodgroups, "Buys" ),
	    normcost, normsell, percentage;

	//  If not buying lets call the whole thing off.
	if( buys.Size() < 1 )
		return 0;
	endif

	var iteminit := 0,
	    item_mark := -1;

	// set buy price for each item to be 0 to start. This means it won't be sellable.
	foreach item in ( backpack_items )
		item.buyprice := iteminit;
		SleepMS(2);
	endforeach

	var buynobuyable := array{},
	    objtype,
	    counter := 0;

	// Populate the array of items the vendor will buy.
	foreach prodgroup in ( buys )
		var cfg_elem := FindConfigElem( buysell_cfg, prodgroup );
  
		foreach itemstr in ( GetConfigStringArray( cfg_elem, "Item" ))
			itemstr := SplitWords( itemstr );
			objtype := itemstr[1];
   
			if( CInt( objtype ))
				objtype := CInt( objtype );
			else
				objtype := GetObjtypeByName( objtype );
			endif
   
			buynobuyable.Append( objtype );
   
			SleepMS(2);
		endforeach
  
		SleepMS(2);
	endforeach

	// compare all the items in the top level of the backpack to
	// things the vendor can buy and set the price.
	if( buynobuyable.Size() )
		foreach item in ( backpack_items )
			if(( item.objtype in buynobuyable ) && ( !item.newbie ))
				counter := counter + 1;
				if( !item.quality )
					item.quality := 1;
				endif
    
				var itemconfig := ReadConfigFile( ":*:itemdesc" );
    
				normcost := itemconfig[item.objtype].VendorBuysFor;
				normsell := itemconfig[item.objtype].VendorSellsFor;
    
				normcost := normcost * item.quality;
				normsell := normsell * item.quality;
    
				percentage := CDbl(( item.hp * 10 ) / item.maxhp );
    
				if( percentage )
					normcost := CInt(( normcost * percentage ) / 10 );
					normsell := CInt(( normsell * percentage ) / 10 );
     
					item.buyprice := normcost;
					item.sellprice := normsell;
				else
					item.buyprice := item_mark;
				endif
			endif
			SleepMS(2);
		endforeach
	endif

	return counter;
endfunction

function CanBuyItem( merchanttype, item )

	var buysell_cfg := ReadConfigFile( ":merchants:buySell" ),
	    prodgroups := FindConfigElem( buysell_cfg, merchanttype ),
	    buys := GetConfigStringArray( prodgroups, "Buys" );

	//  If not buying lets call the whole thing off.
	if( buys.Size() < 1 )
		return 0;
	endif
 
	// Populate the array of items the vendor will buy.
        var ret_val := 0;
	foreach prodgroup in ( buys )
		var cfg_elem := FindConfigElem( buysell_cfg, prodgroup );

		foreach itemstr in ( GetConfigStringArray( cfg_elem, "Item" ))
			itemstr := SplitWords( itemstr );
   
			var objtype := itemstr[1];
			if( CInt( objtype ))
				objtype := CInt( objtype );
			else
				objtype := GetObjtypeByName( objtype );
			endif

			if( objtype == item.objtype )
				ret_val := 1;
                        endif

			SleepMS(2);
		endforeach

		SleepMS(2);
	endforeach
     
	return ret_val;
endfunction

function CPM_GetMerchantType( npc )

	var npc_cfg := NPC_GetNPCConfig( npc );
 
	return npc_cfg.MerchantType;
endfunction

function RestockInventory( npc, merchanttype, byref inventory )

	var buysell_cfg := ReadConfigFile( ":merchants:buySell" );

	foreach item in ( ListRootItemsInContainer( inventory ))
		DestroyItem( item );
		SleepMS(2);
	endforeach

	foreach productgroup in ( ProductGroups( buysell_cfg, merchanttype ))
		Restock( npc, buysell_cfg, inventory, productgroup );
		SleepMS(2);
	endforeach

	return 1;
endfunction

function ProductGroups( byref buysell_cfg, mtype )

	var mtype_elem := FindConfigElem( buysell_cfg, mtype );
	if( !mtype_elem )
		return 0;
	endif

	return GetConfigStringArray( mtype_elem, "Sells" );
endfunction

function Restock( npc, byref buysell_cfg, byref inventory, byref productgroup )

	var pg_elem := FindConfigElem( buySell_cfg, productgroup);
	foreach itemstr in ( GetConfigStringArray( pg_elem, "Item" ))
		SpawnItem( npc, inventory, itemstr );
		SleepMS(2);
	endforeach

	foreach itemstr in ( GetConfigStringArray( pg_elem, "Multibag" ))
		var baglist := SplitWords( itemstr );
		var spawnelem := FindConfigElem( buySell_cfg, baglist[1] );
		var desc := spawnelem.desc;
		var stuffarray := GetConfigStringArray( spawnelem, "Item" );
                
		SpawnMultiBag( npc, inventory, desc, stuffarray, CInt( baglist[2] ));
		SleepMS(2);
	endforeach

	return 1;
endfunction

function SpawnMultiBag( npc, byref inventory, desc, stuffarray, number )

	for i := 1 to number
		var reg_bag := CreateItemInContainer( inventory, 0x0E76, 1 );
		if( reg_bag.errortext )
                        AI_Speak( npc, "Error creating multibag for <"+desc+"> -->"+reg_bag.errortext );
			return 0;
		endif

		reg_bag.name := desc;

		foreach item in ( stuffarray )
			var parms := SplitWords( item );

			var it := CreateItemInContainer( reg_bag, CInt( parms[1] ), CInt( parms[2] ));
			if( it.errortext )
                                AI_Speak( npc, "Error creating item for <"+desc+"> -->"+it.errortext );
				continue;
			endif

                        var item_price := GetSellPrice( npc, parms[1] );
			reg_bag.sellprice := 10 + CInt( item_price * it.amount );

			SleepMS(2);
		endforeach
	endfor

	return 1;
endfunction

function SpawnItem( npc, byref inventory, itemstr )

	var parms := SplitWords( itemstr );
	var objtype := parms[1];

	if( CInt( objtype ))
		objtype := CInt( objtype );
	else
		objtype := GetObjtypeByName( objtype );
	endif
 
	var count := CInt( parms[2] );
	var item := CreateItemInInventory( inventory, objtype, count );
        var item_price := GetSellPrice( npc, parms[1] );
        
        item.sellprice := item_price;

	return 1;
endfunction

function GetSellPrice( npc, objtype )

	var itemdesc := ReadConfigFile( ":*:itemdesc" );
	var elem := FindConfigElem( itemdesc, CInt( objtype ));
	if( elem.errortext )
                AI_Speak( npc, "Error reading itemdesc entry for <"+objtype+"> -->"+elem.errortext );
		return 0;
	endif
 
	var item_price := GetConfigInt( elem, "VendorSellsFor" );
	if( !item_price )
                SysLog( "No item price for objtype "+objtype+" -->"+ elem.name );
		return 0;
	endif
 
	var area := A_IsWhere( npc ),
            area_value := AM_GetMerchantValue( area );

	if( area_value )
		item_price := item_price * area_value;
		if( item_price < 1 )
			item_price := 1;
		endif
	endif

	return CInt( item_price );
endfunction

function GetBuyPrice( npc, item )

	var itemdesc := ReadConfigFile( ":*:itemdesc" );
	var elem := FindConfigElem( itemdesc, CInt( item.objtype ));
	if( elem.errortext )
                AI_Speak( npc, "Error reading itemdesc entry for <"+item.objtype+"> -->"+elem.errortext );
		return 0;
	endif

	var item_price := GetConfigInt( elem, "VendorSellsFor" );
	if( !item_price )
                SysLog( "No item price for objtype "+item.objtype+" -->"+ elem.name );
		return 0;
	endif
 
        var quality := CDbl( item.quality );
        if( quality )
                item_price := CInt( item_price * quality );
        endif
 
        var item_value := CInt( item_price * 75 / 100 );
        if( !item.IsA( POLCLASS_EQUIPMENT ))
                return item_value;
        endif
        
        var item_diff := CInt( item_price - item_value );

        var crafter_level := GetObjProperty( item, "ByCrafter" );
        if( crafter_level )
                item_diff *= CDbl( crafter_level * 0.15 );
        endif

	return CInt( item_value + item_diff );
endfunction

function Check_Restock( npc, merchant_type, forsale )

        var next_restock := CInt( GetObjProperty( npc, "NextRestock" ));
        if( next_restock <= ReadGameClock() )
                RestockInventory( npc, merchant_type, forsale );
                SetObjProperty( npc, "NextRestock", ReadGameClock() + MERCHANT_RESTOCK );
        endif

	return CPM_GetMerchantContainer( npc, CPM_SHOPPINGCART, CP_CREATE );
endfunction
