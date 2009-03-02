/*===============================================================
* Current Version
* SETUP.INC - v1.0
* Updated 8/31/2005 4:54AM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

/*
 * InitializeSettings()
 * 
 * Purpose
 * Initializes g_settings in the brain.
 *
 * Parameters
 *
 * Return value
 * Always returns 1
 *
 */
function InitializeSettings()
	
	g_settings := NPC_LoadTemplateSettings(npc);
	g_settings := NPC_GetDefaultSettings(g_settings);
	g_scripts := NPC_LoadScriptSettings(npc);
		
	return 1;
endfunction

/*
 * InitializeEvents()
 *
 * Purpose
 * Enables system events the core sends based on g_settings
 *
 * Parameters
 *
 * Return value
 * Always returns 1
 *
 */
function InitializeEvents()
	//
	// * Events that use the core 'speech' range.
	//
	if ( g_scripts.Exists("Listen") )
		if ( g_settings["HearSpeech"] )
			EnableEvents(SYSEVENT_SPEECH, g_settings["ListenRange"]);
		endif
		if ( g_settings["HearGhosts"] )
			EnableEvents(SYSEVENT_GHOST_SPEECH, g_settings["ListenRange"]);
		endif
	endif
		
	//
	// * Events that use the core 'area size' range.
	//
	if ( g_scripts.Exists("EnterArea") )
		EnableEvents(SYSEVENT_ENTEREDAREA, g_settings["AreaSize"]);
	endif
	if ( g_scripts.Exists("LeftArea") )
		EnableEvents(SYSEVENT_LEFTAREA, g_settings["AreaSize"]);
	endif
	if ( g_scripts.Exists("GoneCriminal") )
		EnableEvents(SYSEVENT_GONE_CRIMINAL, g_settings["AreaSize"]);
	endif
	
	//
	// * Events that do not have a core range check.
	//
	if ( g_scripts.Exists("DblClick") )
		EnableEvents(SYSEVENT_DOUBLECLICKED, 20);
	endif
	if ( g_scripts.Exists("ItemGiven") )
		EnableEvents(SYSEVENT_ITEM_GIVEN);
	endif
	if ( g_scripts.Exists("Disengaged") )
		EnableEvents(SYSEVENT_DISENGAGED);
	endif
	if ( g_scripts.Exists("Merchant") )
		EnableEvents(SYSEVENT_MERCHANT_BOUGHT);
		EnableEvents(SYSEVENT_MERCHANT_SOLD);
	endif
	if ( g_scripts.Exists("Combat") )
		EnableEvents(SYSEVENT_ENGAGED+SYSEVENT_DAMAGED);
	endif
	
	return 1;
endfunction

/*
 * StartupScripts()
 *
 * Purpose
 * Runs the restart_script or init_script. Sets up the npc_info cprop.
 *
 * Parameters
 *
 * Return value
 * Returns 1 on success
 *
 */
function StartupScripts()
	if ( GetObjProperty(npc, "CreateTime") )
		//This can only happen if Restart() was done on the npc or when POL starts up.
		
		if ( g_scripts.Exists("Restart") )
			var restart_script := Run_Script_To_Completion(g_scripts["Restart"].script, {npc, g_settings, g_scripts});
			if ( restart_script.errortext )
				FatalError("Error::StartupScripts() - Restart script failed: "+restart_script.errortext);
				return 0;
			endif
		else
			KillNerves(npc);
		endif
	else
		SetObjProperty(npc, "CreateTime", POLCore().systime);
		InitializeEnterArea();
		
		if ( g_scripts.Exists("Init") )
			var init_script := Run_Script_To_Completion(g_scripts["Init"].script, {npc, g_settings, g_scripts});
			if ( init_script.errortext )
				FatalError("Error::StartupScripts() - Init script failed:"+init_script.errortext);
				return 0;
			endif
		endif
	endif
	
	SleepMS(20);
		
	if ( g_scripts.Exists("LookAround") )
		StartNerve(npc, "LookAround", g_scripts["LookAround"].script, array{g_settings, g_scripts}, g_scripts["LookAround"].flags);
	endif		
	
	return 1;
endfunction

/*
 * StartupScripts()
 *
 * Purpose
 * Informs the newly created NPC of surrounding mobiles through its EnterArea script.
 *
 * Parameters
 *
 * Return value
 * Returns 1
 *
 */
function InitializeEnterArea()
	// Kludge to know what is in the NPC's area when it is created.
	if ( g_scripts.Exists("EnterArea") )
		var event := struct;
		event.+type := SYSEVENT_ENTEREDAREA;
		event.+source;
		foreach mobile in ( ListMobilesNearLocationEX(npc.x, npc.y, npc.z, CInt(g_settings["AreaSize"]), LISTEX_FLAG_NORMAL+LISTEX_FLAG_HIDDEN+LISTEX_FLAG_GHOST+LISTEX_FLAG_CONCEALED) )
			if ( mobile == npc )
				continue;
			else
				event.source := mobile;
				SendEvent(npc, event);
			endif
			SleepMS(10);
		endforeach
	endif
	
	return 1;
endfunction