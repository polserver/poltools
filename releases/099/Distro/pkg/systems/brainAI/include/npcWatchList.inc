/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-10
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

function NPC_LoadWatchList( npc )

	var watch_dict := GetObjProperty( npc, "#WatchList" );
	if( Lower( TypeOf( watch_dict )) != "dictionary" )
		watch_dict := dictionary;
	endif

	return watch_dict;
endfunction

function NPC_SaveWatchList( npc, dict_list )

	return SetObjProperty( npc, "#WatchList", dict_list );
endfunction

function NPC_AddToWatchList( npc, mobile )

	if( mobile.IsA( POLCLASS_MOBILE ))
		mobile := mobile.serial;
	endif

	var watch_dict := NPC_LoadWatchList( npc );
	if( !watch_dict.Exists( mobile ))
		watch_dict.Insert( mobile, POLCore().systime );
		NPC_SaveWatchList( npc, watch_dict );
		return 1;
	endif
 
	return 0;
endfunction

function NPC_RemoveFromWatchList( npc, mobile )

	if( mobile.IsA( POLCLASS_MOBILE ))
		mobile := mobile.serial;
	endif

	var watch_dict := NPC_LoadWatchList( npc );
	if( watch_dict.Exists( mobile ))
		watch_dict.Erase( mobile );
		NPC_SaveWatchList( npc, watch_dict );
		return 1;
	endif

	return 0;
endfunction

function NPC_IsInWatchList( npc, mobile )

	if( mobile.IsA( POLCLASS_MOBILE ))
		mobile := mobile.serial;
	endif

	var watch_dict := NPC_LoadWatchList( npc );
 
	return watch_dict.Exists( mobile );
endfunction

function NPC_ShouldWatch( npc, mobile, byref settings, byref scripts )
        
	if( !scripts["ShouldWatch"] )
		// Assume it wants to watch everything if it has no script.
		return 1;
	endif
 
	var result := Run_Script_To_Completion( scripts["ShouldWatch"].script, {npc, mobile, settings} );
	if( result.errortext )
		NPC_ErrorSysLog( npc, "Error::NPC_ShouldWatch() - "+result.errortext );
	endif
  
        return result;
endfunction
