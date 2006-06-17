/* $Id$
 *
 * Purpose
 * Provide functions related to world saving.
 *
 */
use uo;
use os;
use polsys;

include "include/time";
include "include/messages";
include "include/localization";


/*
 * SaveWorld()
 *
 * Author: Folko
 *
 * Purpose
 * Announces save, saves world, broadcasts time needed.
 *
 * Parameters
 * No Parameters
 *
 * Return value
 * The time the save took, in seconds
 * 
 */
function SaveWorld()
	var before, time;

	SendAll(MSG_SAVING_NOW, {}, MSG_IMPORTANT);
	before := ReadMillisecondClock();
	SaveWorldState();
	time := (ReadMillisecondClock() - before) / MIL_SECOND;
	SendAll(MSG_WORLD_SAVED, {time}, MSG_IMPORTANT);

	return(time);
endfunction
