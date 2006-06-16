/*
 * $Id: death.inc 87 2006-05-23 19:04:43Z sroyalty $
 *
 */


use uo;
use os;
use util;

include ":brainAI:npcUtil";

/*
 * DP_PlayDeathSound(corpse, ghost:=0)
 *
 * Purpose
 * Plays a sound effect when something dies.
 *
 * Parameters
 * object:	Either a corpse or a ghost to play the sound on.
 *
 * Return value
 * Sound number that was played
 */
function DP_PlayDeathSound(object)
	var death_sound := 0;
	
	if ( object.IsA(POLCLASS_CORPSE) )
		var template := GetObjProperty(object, "npctemplate"); // Set by core
		if ( template )
			var cfg_info := NPC_GetNPCConfig(template);
			var sounds := GetConfigStringArray(cfg_info, "DeathSound");
			death_sound := CInt(sounds[RandomInt(sounds.Size())+1]);
		endif
	endif
	if ( !death_sound )
		// No sound selected yet.
		if ( object.graphic == 402 || object.corpsetype == 400 )
			 // Male ghost / corpse
			var sounds := array{347, 348, 349, 350};
			death_sound := sounds[RandomInt(sounds.Size())+1];
		elseif ( object.graphic == 403 || object.corpsetype == 401 )
			 // Female ghost / corpse
			var sounds := array{337, 338, 339, 340};
			death_sound := sounds[RandomInt(sounds.Size())+1];
		endif
	endif

	if ( death_sound )
		PlaySoundEffect(object, death_sound);
	endif
	
	return death_sound;
endfunction
