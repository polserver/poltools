/*
        Maintened by *Edwards
        
        2009-10-10
*/

function RunCycleScript()

	if( g_scripts["Cycle"] )
		var script := Run_Script_To_Completion( g_scripts["Cycle"].script, array{npc, g_settings, g_scripts} );
		if( script.errortext )
			NPC_ErrorSysLog( npc, "Error::RunCycleScript() - "+script.errortext );
		endif
	endif
	
	return 1;
endfunction
