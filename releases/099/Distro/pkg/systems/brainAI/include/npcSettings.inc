/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-10
*/
use uo;
use os;
use cfgfile;

include ":brainAI:npcUtil";

function NPC_GetDefaultSettings( settings:=0 )

	if( Lower( TypeOf( settings )) != "dictionary" )
		settings := dictionary;
	endif

	if( CInt( settings.Exists( "CycleWait" )) <= 0 )
		settings["CycleWait"] := 1;
	endif

	if( !settings.Exists( "NoSleep" ))
		if( CInt( settings.Exists( "SleepWait" )) <= 0 )
			settings["SleepWait"] := 180;
		endif
		if( CInt( settings.Exists( "IdleTicks" )) <= 0 )
			settings["IdleTicks"] := 120;
		endif
	endif

	return settings;
endfunction

function NPC_LoadTemplateSettings( source )

	var npc_elem := NPC_GetNPCConfig( source );
	var setting_list := GetConfigStringDictionary( npc_elem, "AISetting" );

	var load_settings := dictionary;
	foreach setting_name in ( setting_list.Keys() )
		load_settings[setting_name] := UnPack( setting_list[setting_name] );
		SleepMS(10);
	endforeach

	if( source.IsA( POLCLASS_NPC ))
		var cstm_settings := GetObjProperty( source, "AISettings" );
		foreach key in ( cstm_settings.Keys() )
			if( cstm_settings[key] == error )
				load_settings.Erase( key );
			else
				load_settings[key] := cstm_settings[key];
			endif

			SleepMS(10);
		endforeach
	endif

	return load_settings;
endfunction
