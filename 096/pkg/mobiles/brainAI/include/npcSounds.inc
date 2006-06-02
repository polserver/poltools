/*===============================================================
* Current Version
* NPCSOUNDS.INC - v1.0
* Updated 11/01/2005 2:26AM
*
* -- Revision v1.0 --
* MuadDib:
*  Created include file
===============================================================*/
use uo;
use os;
use util;
use cfgfile;

include ":brainAI:npcUtil";
include ":brainAI:npcError";


/////////////////////////////////////////////////////////////////
// * Function List *
//
// NPC_GetIdleSound(mobile)
// NPC_GetDamagedSound(mobile)
// NPC_GetDeathSound(mobile)
// NPC_GetMissSound(mobile)
// NPC_GetHitSound(mobile)
// NPC_GetBarkSound(mobile)
//
/////////////////////////////////////////////////////////////////
// * Internal/Private Function List *
//
/////////////////////////////////////////////////////////////////


/*
 * NPC_GetIdleSound(mobile)
 *
 * Purpose
 * Gets the packed array form of IdleSounds Prop from NPC Template.
 *
 * Parameters
 * mobile:	Mobile to find the idle sound for.
 *
 * Return value
 * Returns an integer that is the sound ID to be played.
 *
 */
function NPC_GetIdleSound(mobile)
	var mobile_template := NPC_GetNPCConfig(mobile);
	var idle_sound_list := GetConfigStringArray(mobile, "IdleSound");

	if( TypeOf(idle_sound_list) != "Array" )
		//NPC_ErrorSysLog(mobile, "Invalid Idle Sound Setting");
		return 0;
	endif

	return RandomInt(idle_sound_list.Size())+1;

endfunction


/*
 * NPC_GetDamagedSound(mobile)
 *
 * Purpose
 * Gets the packed array form of DamagedSounds Prop from NPC Template.
 *
 * Parameters
 * mobile:	Mobile to find the damaged sound for.
 *
 * Return value
 * Returns an integer that is the sound ID to be played.
 *
 */
function NPC_GetDamagedSound(mobile)
	var mobile_template := NPC_GetNPCConfig(mobile);
	var damaged_sound_list := GetConfigStringArray(mobile, "DamagedSound");

	if( TypeOf(damaged_sound_list) != "Array" )
		//NPC_ErrorSysLog(mobile, "Invalid Damaged Sound Setting");
		return 0;
	endif

	return RandomInt(damaged_sound_list.Size())+1;

endfunction


/*
 * NPC_GetDeathSound(mobile)
 *
 * Purpose
 * Gets the packed array form of DeathSounds Prop from NPC Template.
 *
 * Parameters
 * mobile:	Mobile to find the death sound for.
 *
 * Return value
 * Returns an integer that is the sound ID to be played.
 *
 */
function NPC_GetDeathSound(mobile)
	var mobile_template := NPC_GetNPCConfig(mobile);
	var death_sound_list := GetConfigStringArray(mobile, "DeathSound");

	if( TypeOf(death_sound_list) != "Array" )
		//NPC_ErrorSysLog(mobile, "Invalid Death Sound Setting");
		return 0;
	endif

	return RandomInt(death_sound_list.Size())+1;

endfunction


/*
 * NPC_GetMissSound(mobile)
 *
 * Purpose
 * Gets the packed array form of AttackMissSounds Prop from NPC Template.
 *
 * Parameters
 * mobile:	Mobile to find the attack and miss sound for.
 *
 * Return value
 * Returns an integer that is the sound ID to be played.
 *
 */
function NPC_GetMissSound(mobile)
	var mobile_template := NPC_GetNPCConfig(mobile);
	var miss_sound_list := GetConfigStringArray(mobile, "AttackMissSound");

	if( TypeOf(miss_sound_list) != "Array" )
		//NPC_ErrorSysLog(mobile, "Invalid Miss Sound Setting");
		return 0;
	endif

	return RandomInt(miss_sound_list.Size())+1;

endfunction

/*
 * NPC_GetHitSound(mobile)
 *
 * Purpose
 * Gets the packed array form of AttackHitSounds Prop from NPC Template.
 *
 * Parameters
 * mobile:	Mobile to find the attack and hit sound for.
 *
 * Return value
 * Returns an integer that is the sound ID to be played.
 *
 */
function NPC_GetHitSound(mobile)
	var mobile_template := NPC_GetNPCConfig(mobile);
	var hit_sound_list := GetConfigStringArray(mobile, "AttackHitSound");

	if( TypeOf(hit_sound_list) != "Array" )
		NPC_ErrorSysLog(mobile, "Invalid Hit Sound Setting");
		return 0;
	endif

	return RandomInt(hit_sound_list.Size())+1;

endfunction


/*
 * NPC_GetBarkSound(mobile)
 *
 * Purpose
 * Gets the packed array form of BarkSounds Prop from NPC Template.
 *
 * Parameters
 * mobile:	Mobile to find the barking sound for.
 *
 * Return value
 * Returns an integer that is the sound ID to be played.
 *
 */
function NPC_GetBarkSound(mobile)
	var mobile_template := NPC_GetNPCConfig(mobile);
	var bark_sound_list := GetConfigStringArray(mobile, "BarkSound");

	if( TypeOf(bark_sound_list) != "Array" )
		//NPC_ErrorSysLog(mobile, "Invalid Bark Sound Setting");
		return 0;
	endif

	return RandomInt(bark_sound_list.Size())+1;

endfunction