/* $Id: merchant.inc 1469 2009-03-11 18:47:02Z muaddib_pol $
 *
 */

use util;

include ":brainai:npcUtil";
include ":containers:containers";

/*
 * HasSellableItems(container, backpack)
 *
 * Purpose:
 * Determines if the player has atleast 1 item the merchant can buy.
 * The SendBuyWindow() will come up even if the merchant is selling nothing.
 * However, the SendSellWindow() will only appear if the player actually has
 * items the merchant will want to buy.
 *
 * Parameters
 * container:	Container with items the merchant is interested in.
 * backpack:	Player backpack / container the merchant will buy from.
 *
 * Return value
 * Returns 1 if atleast one item was found.
 * Returns 0 if no matching items were found.
 *
 */

function HasSellableItems(merchanttype, backpack)
	var backpack_items := ListRootItemsInContainer(backpack);
	var buysell_cfg := ReadConfigFile(":merchants:buySell");
	var prodgroups := FindConfigElem(buysell_cfg, merchanttype);
	var buys := GetConfigStringArray(prodgroups, "Buys");
	var normcost, normsell, percentage;

	//  If not buying lets call the whole thing off.
	if ( buys.Size() < 1 )
		return 0;
	endif

	var iteminit := 0;
	var item_mark := -1;

	// set buy price for each item to be 0 to start. This means it won't be sellable.
	foreach item in ( backpack_items )
		item.buyprice := iteminit;
		SleepMS(2);
	endforeach

	var buynobuyable := array;
	var objtype;
	var counter := 0;

	// Populate the array of items the vendor will buy.
	foreach prodgroup in ( buys )
		var cfg_elem := FindConfigElem(buysell_cfg, prodgroup);
		foreach itemstr in ( GetConfigStringArray(cfg_elem, "Item") )
			itemstr := SplitWords(itemstr);
			objtype := itemstr[1];
			if ( CInt(objtype) )
				objtype := CInt(objtype);
			else
				objtype := GetObjtypeByName(objtype);
			endif
			buynobuyable.Append(objtype);
			SleepMS(2);
		endforeach
		SleepMS(2);
	endforeach

	// compare all the items in the top level of the backpack to
	// things the vendor can buy and set the price.
	if ( buynobuyable.Size() )
		foreach item in ( backpack_items )
			if ( (item.objtype in buynobuyable) && (!item.newbie) )
				counter := counter + 1;
				if ( !item.quality )
					item.quality := 1;
				endif
				var itemconfig := ReadConfigFile(":*:itemdesc");
				normcost := itemconfig[item.objtype].VendorBuysFor;
				normsell := itemconfig[item.objtype].VendorSellsFor;
				normcost := normcost * item.quality;
				normsell := normsell * item.quality;
				percentage := ((item.hp * 10) / item.maxhp);
				if ( percentage )
					normcost := CInt((normcost * percentage) / 10);
					normsell := CInt((normsell * percentage) / 10);
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

function CPM_GetMerchantType(npc)
	var npc_cfg := NPC_GetNPCConfig(npc);
	var type := npc_cfg.MerchantType;
	return type;
endfunction

function CPM_GetMerchantWealth(npc)
	var npc_cfg := NPC_GetNPCConfig(npc);
	var type := Lower(npc_cfg.wealth);
	return type;
endfunction

function RestockInventory(merchanttype, byref inventory)
	var buysell_cfg := ReadConfigFile(":merchants:buySell");

	foreach item in ( ListRootItemsInContainer(inventory) )
		DestroyItem(item);
		SleepMS(2);
	endforeach

	foreach productgroup in ( ProductGroups(buysell_cfg, merchanttype) )
		Restock(buysell_cfg, inventory, productgroup);
		SleepMS(2);
	endforeach

	foreach item in ( ListRootItemsInContainer(inventory) )
		if ( item.quality )
			item.quality := randomint (50) + 50;
			item.hp := item.maxhp;
		endif
		SleepMS(2);
	endforeach

	return 1;
endfunction

function ProductGroups(byref buysell_cfg, mtype)
	var mtype_elem := FindConfigElem(buysell_cfg, mtype);
	if ( !mtype_elem )
		return 0;
	endif

	return GetConfigStringArray (mtype_elem, "Sells");
endfunction

function Restock(byref buysell_cfg, byref inventory, byref productgroup)
	var pg_elem := FindConfigElem(buySell_cfg, productgroup);
	foreach itemstr in ( GetConfigStringArray(pg_elem, "Item") )
		SpawnItem(inventory, itemstr);
		SleepMS(2);
	endforeach

	foreach itemstr in ( GetConfigStringArray(pg_elem, "Multibag") )
		var baglist := SplitWords(itemstr);
		var spawnelem := FindConfigElem(buySell_cfg, baglist[1]);
		var desc := spawnelem.desc;
		var stuffarray := GetConfigStringArray(spawnelem, "Item");
		SpawnMultiBag(inventory, desc, stuffarray, CInt(baglist[2]));
		SleepMS(2);
	endforeach

	return 1;
endfunction

function SpawnMultiBag(byref inventory, desc, stuffarray, number)
	var itemdesc := ReadConfigFile (":*:itemdesc");

	for i := 1 to number
		var reg_bag := CreateItemInContainer(inventory, 0x0E76, 1);
		if ( !reg_bag )
			return 0;
		endif

		reg_bag.name := desc;
		reg_bag.sellprice := 10;

		foreach item in ( stuffarray )
			var parms := SplitWords(item);
			var elem := FindConfigElem(itemdesc, CInt(parms[1]));
			if ( !elem )
				return 0;
			endif

			var item_price := GetConfigInt(elem, "VendorSellsFor");
			if ( !item_price )
				return 0;
			endif

			var it := CreateItemInContainer(reg_bag, CInt(parms[1]), CInt(parms[2]));
			if ( !it )
				return 0;
			endif

			reg_bag.sellprice := reg_bag.sellprice + ( item_price * it.amount );
			SleepMS(2);
		endforeach
	endfor

	return 1;
endfunction

function SpawnItem(byref inventory, itemstr)
	var ii := SplitWords(itemstr);
	var objtype := ii[1];

	if ( CInt(objtype) )
		objtype := CInt(objtype);
	else
		objtype := GetObjtypeByName(objtype);
	endif

	var count := CInt(ii[2]);
	CreateItemInInventory(inventory, objtype, count);

	return 1;
endfunction


