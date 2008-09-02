/*
 * $Id: npcWatchList.inc 867 2006-10-03 22:22:04Z austinheilman $
 *---------------------------------
 * Current Version
 * NPCWATCHLIST.INC - v1.0
 * Updated 9/13/2005 3:21AM
 *
 * -- Revision v1.0 --
 * Austin:
 *  Created include file
 *---------------------------------
 */

use uo;
use os;

include ":brainAI:npcError";

/////////////////////////////////////////////////////////////////
// * Function List *
//
// NPC_AddToWatchList(npc, mobile)
// NPC_IsInWatchList(npc, mobile)
// NPC_LoadWatchList(npc)
// NPC_RemoveFromWatchList(npc, mobile)
// NPC_SaveWatchList(npc, dict_list)
// NPC_ShouldWatch(npc, mobile, settings, scripts)
//
/////////////////////////////////////////////////////////////////
//
// Note: Once a function leaves scope, it stops being critical.
//       As such, no Set_Critical(0) statements are made.
//
/*
 * NPC_LoadWatchList(npc)
 *
 * Purpose
 * Loads the watch list on an NPC.
 *
 * Parameters
 * npc:	NPC to load the watch list from.
 *
 * Return value
 * Returns a dictionary
 *
 */
function NPC_LoadWatchList(npc)
	Set_Critical(1);
	var watch_dict := GetObjProperty(npc, "#WatchList");
	if ( Lower(TypeOf(watch_dict)) != "dictionary" )
		watch_dict := dictionary;
	endif

	return watch_dict;
endfunction

/*
 * NPC_SaveWatchList(npc)
 *
 * Purpose
 * Saves the NPC's watch list.
 *
 * Parameters
 * npc:		NPC to save the watch list on.
 * dict_list:	Dictionary containing serials the npc is watching.
 *
 * Return value
 * Returns the result of SetObjProperty()
 *
 */
function NPC_SaveWatchList(npc, dict_list)
	return SetObjProperty(npc, "#WatchList", dict_list);
endfunction

/*
 * NPC_AddToWatchList(npc, mobile)
 *
 * Purpose
 * Adds a mobile to the NPC's watch list.
 *
 * Parameters
 * npc:		NPC that will have its list added to.
 * mobile:	Mobile to add to the watch list.
 *
 * Return value
 * Returns 1 if the mobile was added.
 *
 */
function NPC_AddToWatchList(npc, mobile)
	Set_Critical(1);
	if ( mobile.IsA(POLCLASS_MOBILE) )
		mobile := mobile.serial;
	endif

	var watch_dict := NPC_LoadWatchList(npc);
	if ( !watch_dict.Exists(mobile) )
		watch_dict.Insert(mobile, POLCore().systime);
		NPC_SaveWatchList(npc, watch_dict);

		return 1;
	else
		return 0;
	endif
endfunction

/*
 * NPC_RemoveFromWatchList(npc)
 *
 * Purpose
 * Removes a mobile from the NPC's watch list.
 *
 * Parameters
 * npc:		NPC that will have its list modified.
 * mobile:	Mobile to remove from the watch list.
 *
 * Return value
 * Returns 1 if the mobile was removed.
 *
 */
function NPC_RemoveFromWatchList(npc, mobile)
	Set_Critical(1);
	if ( mobile.IsA(POLCLASS_MOBILE) )
		mobile := mobile.serial;
	endif

	var watch_dict := NPC_LoadWatchList(npc);
	if ( watch_dict.Exists(mobile) )
		watch_dict.Erase(mobile);
		NPC_SaveWatchList(npc, watch_dict);

		return 1;
	else
		return 0;
	endif
endfunction

/*
 * NPC_IsInWatchList(npc, mobile)
 *
 * Purpose
 * Checks if a mobile is in an NPC's watch list.
 *
 * Parameters
 * npc:		NPC with the list to check
 * mobile:	Mobile that will be checked for in the list.
 *
 * Return value
 * Returns 1 if the mobile is in the list.
 *
 */
function NPC_IsInWatchList(npc, mobile)
	Set_Critical(1);
	if ( mobile.IsA(POLCLASS_MOBILE) )
		mobile := mobile.serial;
	endif

	var watch_dict := NPC_LoadWatchList(npc);
	return watch_dict.Exists(mobile);
endfunction

/*
 * NPC_ShouldWatch(npc, mobile, settings, scripts)
 *
 * Purpose
 * Checks if the NPC should watch the mobile.
 *
 * Parameters
 * npc:		NPC that will do the checking
 * mobile:	Mobile to check to be added to the watch list.
 * settings:	Dictionary of the NPC's settings.
 * scripts:	Dictionary of the npcs script settings.
 *
 * Return value
 * Returns 1 if the mobile is a potential target.
 *
 */
function NPC_ShouldWatch(npc, mobile, byref settings, byref scripts)
	if ( !scripts["ShouldWatch"] )
		// Assume it wants to watch everything if it has no script.
		return 1;
	endif

	var result := Run_Script_To_Completion(scripts["ShouldWatch"].script, {npc, mobile, settings});
	if ( result.errortext )
		NPC_ErrorSysLog(npc, "Error::NPC_ShouldWatch() - "+result.errortext);
	endif

	return result;
endfunction
