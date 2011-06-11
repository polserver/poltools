/*
        Maintened by *Edwards

        2009-07-26
*/
use uo;

include ":containers:storageAreas";

const CPM_FORSALE	:= "Merchant-ForSale";
const CPM_BUYING	:= "Merchant-Buying";
const CPM_SHOPPINGCART	:= "Merchant-ShoppingCart";
const CPM_HOLDING	:= "Merchant-Holding";

function CPM_DeleteContainers( npc )

	var container_id := CP_GetMobileContainerID( npc );
	var area_names := array{CPM_FORSALE, CPM_BUYING, CPM_SHOPPINGCART, CPM_HOLDING};
 
	foreach area_name in ( area_names )
		if( GetObjProperty( npc, "Shared-"+area_name ))
			// Dont want to delete shared storage areas.
			continue;
		elseif( CP_GetStorageAreaContainer( container_id, area_name ))
			CP_RemoveStorageAreaContainer( container_id, area_name );
		endif
		SleepMS(2);
	endforeach

	return 1;
endfunction

function CPM_GetMerchantContainer( npc, type, create:=CP_NOCREATE )

	var shared_container := GetObjProperty( npc, "Shared-"+type );
	if( shared_container && type != CPM_SHOPPINGCART )
		//Note: The 'Shopping cart' container can never be shared between multiple merchants!
		return CP_GetStorageAreaContainer( shared_container, type, create );
	else
		return CP_GetStorageContainerForMobile( npc, type, create );
	endif
endfunction
