// $Id$

/*===============================================================
* Current Version
* MERCHANTUTIL.INC - v1.0
* Updated 10/5/2005 6:15AM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

use uo;

include ":containers:storageAreas";

CONST CPM_FORSALE	:= "Merchant - ForSale";
CONST CPM_BUYING	:= "Merchant - Buying";
CONST CPM_SHOPPINGCART	:= "Merchant - ShoppingCart";
CONST CPM_HOLDING	:= "Merchant - Holding";

/*
 * NPC_MerchantForSaleContainer(npc)
 *
 * Purpose
 *
 * Parameters
 * npc:
 *
 * Return value
 * Returns a container
 *
 */
function CPM_DeleteContainers(npc)
	var container_id := CP_GetMobileContainerID(npc);
	CP_RemoveStorageAreaContainer(container_id, CPM_FORSALE);
	CP_RemoveStorageAreaContainer(container_id, CPM_BUYING);
	CP_RemoveStorageAreaContainer(container_id, CPM_SHOPPINGCART);
	CP_RemoveStorageAreaContainer(container_id, CPM_HOLDING);
endfunction

/*
 * CPM_GetMerchantContainer(npc, type)
 *
 * Purpose
 * Retrieves a merchant (vendor) container.
 *
 * Parameters
 * npc:		NPC that owns the container.
 * type:	Storage area to retrieve the container from.
 *		CPM_FORSALE		- Items the vendor is selling.
 *		CPM_BUYING		- Items the vendor will buy.
 *		CPM_SHOPPINGCART	- Items a buyer has selected (but not paid for).
 *		CPM_HOLDING		- Items the vendor is holding onto for its owner.
 *
 * Return value
 * Returns a container
 *
 */
function CPM_GetMerchantContainer(npc, type)
	var shared_container := GetObjProperty(npc, "MerchantContainer");
	if ( shared_container && type != CPM_SHOPPINGCART )
		//Note: The 'Shopping cart' container can never be shared between multiple merchants!
		return CP_GetStorageAreaContainer(shared_container, type, CP_CREATE);
	else
		return CP_GetStorageContainerForMobile(npc, type, CP_CREATE);
	endif
endfunction
