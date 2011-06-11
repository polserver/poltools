/*
        Maintened by *Edwards

        2009-10-25
*/
use uo;
use os;
use util;

include ":brainAI:npcUtil";
include ":death:settings";

function DP_PlayDeathSound( object )

	var death_sound := 0;

	if( object.IsA( POLCLASS_CORPSE ))
		var template := object.NPCTemplate();
		if( template )
			var cfg_info := object.NPCTemplate();
			var sounds := GetConfigStringArray( cfg_info, "DeathSound" );
			death_sound := sounds.RandomEntry();
		endif
	endif
 
	if( !death_sound )
		// No sound selected yet.
		if( object.graphic == 402 || object.corpsetype == 400 )
	                // Male ghost / corpse
			var cfg_elem := DP_GetSettingsCfgElem( "MaleSounds" );
			var sounds := GetConfigStringArray( cfg_elem, "Sound" );
			death_sound := sounds.RandomEntry();
		elseif( object.graphic == 403 || object.corpsetype == 401 )
	                // Female ghost / corpse
			var cfg_elem := DP_GetSettingsCfgElem( "FemaleSounds" );
			var sounds := GetConfigStringArray( cfg_elem, "Sound" );
			death_sound := sounds.RandomEntry();
		endif
	endif

	if( death_sound )
		PlaySoundEffect( object, death_sound );
	endif

	return death_sound;
endfunction
