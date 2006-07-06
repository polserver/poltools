/* $Id$
 *
 */

use uo;
use os;

include ":gumps:gumps";
include ":gumps:gumps_ex";
include ":gumps:gumpprompt";
include ":help:help";


function BuildGumpTemplate()
	var gump := LoadHelpInfo("QueueTemplate", "Gump");

	if ( gump )
		return gump;
	endif

	gump := GFCreateGump();

	return gump;
endfunction

function ShowPagesInQueue(queue_name)
	return 1;
endfunction

function ShowQueueTypes(cmdlevel)
	return 1;
endfunction

function ShowPageInQueue(queue_name)
	return 1;
endfunction