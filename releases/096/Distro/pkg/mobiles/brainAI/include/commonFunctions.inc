/* $Id: commonFunctions.inc 87 2006-05-23 19:04:43Z sroyalty $
 * 
 * Description
 * Common code found in just about every nerve script.
 * This include file exists to reduce redundancy.
 * 
 */

use uo;

include ":attributes:attributes";

 
//==========
// Non nerve specific functions
//==========



//==========
// Combat Nerve Functions
//==========

/* NPC_GetFleeLevel(npc, flee_percent)
 * 
 * Purpose
 * Calculates the hit point level when an npc should run away.
 *
 * Parameters
 * npc:			NPC that the nerve belongs to. 
 * flee_percent:	Percentage of hit points that the NPC will flee at.
 *
 * Return Value
 * Returns an integer.
 *
 */
function NPC_GetFleeLevel(npc, flee_percent)
	return CInt(CDbl(AP_GetVitalMaximumValue(npc, "Hits")) * (CDbl(flee_percent)/100.0));
endfunction

/* NPC_SetupRanges(npc, min_range, max_range)
 *
 * Purpose
 * Sets up the minimum and maximum distances the npc will allow its self to be
 * in relation to its opponent. These are based on its weapon settings.
 *
 * Parameters
 * NPC:		NPC the nerve belongs to.
 * min_range:	Minimum range variable to set.
 * max_range:	Maximum range variable to set.
 *
 * Return value
 * No return value.
 *
 */
function NPC_WeaponRanges(npc, byref min_range, byref max_range)
	// To support having distance for archery weapons or being close up for a melee weapon.
	min_range := CInt(CS_GetWeaponInfo(npc.weapon, "MinRange"));
	max_range := CInt(CS_GetWeaponInfo(npc.weapon, "MaxRange"));
	if ( !max_range )
		max_range := 1;
	endif
	
	return 1;
endfunction

