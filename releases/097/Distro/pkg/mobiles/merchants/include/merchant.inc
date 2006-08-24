/* $Id$
 *
 */

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

function HasSellableItems(container, backpack)
	foreach item in ( EnumerateItemsInContainer(container) )
		if ( item.container != container )
			continue;
		endif

		var props := dictionary{"objtype"->item.objtype, "container"->backpack};
		var plr_has := AmountInContainer(backpack, props);
		if ( plr_has > 0 )
			return 1;
			break;
		endif
		SleepMS(2);
	endforeach

	return 0;
endfunction
