/*===============================================================
* Current Version
* CYCLESCRIPT.INC - v1.0
* Updated 9/6/2005 7:13AM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

/*
 * RunCycleScript()
 *
 * Purpose
 * Runs the cycle script if one is present.
 *
 * Parameters
 *
 * Return value
 * Always returns 1
 *
 */
function RunCycleScript()
	if ( g_scripts["Cycle"] )
		var script := Run_Script_To_Completion(g_scripts["Cycle"].script, array{npc, g_settings, g_scripts});
		
		if ( script.errortext )
			NPC_ErrorSysLog(npc, "Error::RunCycleScript() - "+script.errortext);
		endif
	endif
	
	return 1;
endfunction
