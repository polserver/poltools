/* $Id$
 *
 * Utility include for sending packets to clients.
 *
 */

use uo;

/*
 * PlayerSpeedBoost(player, toggle)
 *
 * Purpose
 * Enables or disables the speed boost (walk/run speed) for players.
 * Thanks to CWO for packet information.
 *
 * Arguments
 * player: Player's client to send the command to
 * toggle: 1 - turns on  0 - turns off
 *
 * Return
 * Returns an error struct if there was an error.
 * Returns the value from SendPacket()
 *
 */

function PlayerSpeedBoos(player, toggle:=0)
	if ( toggle )
		return SendPacket(player, "BF0006002601");
	else
		return SendPacket(player, "BF0006002600");
	endif
endfunction
