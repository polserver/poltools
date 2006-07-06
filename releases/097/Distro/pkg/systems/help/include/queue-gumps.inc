/* $Id$
 *
 */

use uo;
use os;

include ":gumps:gumps";
include ":gumps:gumps_ex";
include ":gumps:gumpprompt";
include ":help:help";

CONST CLOSE_BTN		:= 0xA000;
CONST QUEUE_TYPES_BTN	:= 0xA001;
CONST PAGE_LIST_BTN	:= 0xA002;
CONST SHOW_PAGE_BTN	:= 0xA003;
CONST DELETE_PAGE_BTN	:= 0xA004;
CONST GOTO_BTN		:= 0xA006;
CONST SUMMON_BTN	:= 0xA007;

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