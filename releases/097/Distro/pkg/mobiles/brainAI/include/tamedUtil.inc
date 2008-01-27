/*
 * $Id$
 *
 * Description
 * Functions to assist in setting an NPC up for tamed nerves.
 * Functions to asisst with general tamed AI usage.
 *
 */

use uo;

/* NPC_SetupTamed(npc, master)
 *
 * Purpose
 * Sets an NPC's master and nerve overrides.
 *
 * Parameters
 * npc:			NPC that is being set up.
 * master:		Mobile (usually player) that will be the NPC's master.
 *
 * Return Value
 * Returns 1
 *
 */
function NPC_SetupTamed(npc, master)
	var nerves := dictionary;
	// Set all nerves the npc is currently using to empty strings to disable them.
	// Set the listen nerve to the tamed listen nerve.
	// Leave the combat nerve the npc comes with to allow it to melee/spell cast, etc.

	var settings := dictionary;
	settings["ListenRange"] := 12;

	npc.SetMaster(master);

	return 1;
endfunction