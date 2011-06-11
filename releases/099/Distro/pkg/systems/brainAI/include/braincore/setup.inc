/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-10
*/
use polsys;

include ":timeutils:time";

function InitializeSettings()
	
	g_settings := NPC_LoadTemplateSettings( npc );
	g_settings := NPC_GetDefaultSettings( g_settings );
	g_scripts := NPC_LoadScriptSettings( npc );
		
	return 1;
endfunction

function InitializeEvents()

	if( master )
                EnableEvents( SYSEVENT_SPEECH, 12 );
                EnableEvents( SYSEVENT_DOUBLECLICKED, 1 );
        else
		if( g_scripts.Exists( "Listen" ))
			if( g_settings.Exists( "HearSpeech" ))
				EnableEvents( SYSEVENT_SPEECH, g_settings["ListenRange"] );
			endif
			if( g_settings.Exists( "HearGhosts" ))
				EnableEvents( SYSEVENT_GHOST_SPEECH, g_settings["ListenRange"] );
			endif
		endif
		if( g_scripts.Exists( "DblClick" ))
			EnableEvents( SYSEVENT_DOUBLECLICKED, 20 );
		endif
	endif

	if( g_scripts.Exists( "Combat" ))
		EnableEvents( SYSEVENT_ENGAGED+SYSEVENT_DAMAGED );
	endif
	if( g_scripts.Exists( "Disengaged" ))
		EnableEvents( SYSEVENT_DISENGAGED );
	endif
	if( g_scripts.Exists( "EnterArea" ))
		EnableEvents( SYSEVENT_ENTEREDAREA, g_settings["AreaSize"] );
	endif
	if( g_scripts.Exists( "GoneCriminal" ))
		EnableEvents( SYSEVENT_GONE_CRIMINAL, g_settings["AreaSize"] );
	endif
	if( g_scripts.Exists( "ItemGiven" ))
		EnableEvents( SYSEVENT_ITEM_GIVEN );
	endif
	if( g_scripts.Exists( "LeftArea" ))
		EnableEvents( SYSEVENT_LEFTAREA, g_settings["AreaSize"] );
	endif
	if( g_scripts.Exists( "Merchant" ))
		EnableEvents( SYSEVENT_MERCHANT_BOUGHT );
		EnableEvents( SYSEVENT_MERCHANT_SOLD );
	endif
	
	return 1;
endfunction

function StartupScripts()
 
	if( GetObjProperty( npc, "CreateTime" ))
		//This can only happen if Restart() was done on the npc or when POL starts up.
		if( g_scripts.Exists( "Restart" ))
			var restart_script := Run_Script_To_Completion( g_scripts["Restart"].script, {npc, g_settings, g_scripts} );
			if( restart_script.errortext )
				FatalError( "Error::StartupScripts() - Restart script failed: "+restart_script.errortext );
				return 0;
			endif
		endif
  
		EnterSleepMode();
	else
		SetObjProperty( npc, "CreateTime", POLCore().systime );

		if( g_scripts.Exists( "Init" ))
			var init_script := Run_Script_To_Completion( g_scripts["Init"].script, {npc, g_settings, g_scripts} );
			if( init_script.errortext )
				FatalError( "Error::StartupScripts() - Init script failed:"+init_script.errortext );
				return 0;
			endif
		endif
  
                InitializeEnterArea();
	endif
	
	return 1;
endfunction

function InitializeEnterArea()

	if( g_scripts.Exists( "EnterArea" ))

		var event := struct;
		event.+type := SYSEVENT_ENTEREDAREA;
		event.+source;

		foreach mobile in ( ListMobilesNearLocationEX( npc.x, npc.y, npc.z, CInt( g_settings["AreaSize"] ), LISTEX_FLAG_NORMAL+LISTEX_FLAG_HIDDEN+LISTEX_FLAG_GHOST+LISTEX_FLAG_CONCEALED ))
			if( mobile.serial == npc.serial )
				continue;
			else
				event.source := mobile;
				SendEvent( npc, event );
			endif

			SleepMS(10);
		endforeach
	endif

	return 1;
endfunction
