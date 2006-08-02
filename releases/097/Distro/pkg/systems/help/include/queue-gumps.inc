/* $Id$
 *
 */

use uo;
use os;

include ":gumps:gumps";
include ":gumps:gumps_ex";
include ":gumps:gumpprompt";
include ":help:help";

CONST CLOSE_BTN		:= 0xA00;
CONST QUEUE_TYPES_BTN	:= 0xA01;
CONST PAGE_LIST_BTN	:= 0xA02;
CONST SHOW_PAGE_BTN	:= 0xA03;
CONST DELETE_PAGE_BTN	:= 0xA04;
CONST GOTO_BTN		:= 0xA06;
CONST SUMMON_BTN	:= 0xA07;

CONST SHOW_QUEUE_START	:= 0xA000;

function BuildGumpTemplate()
	var gump := LoadHelpInfo("QueueTemplate", "Gump");

	if ( gump )
		return gump;
	endif

	gump := GFCreateGump();
	GFResizePic(gump, 0, 0, GFGetCfgConst("Defaults", "BackGround"), 540, 450);
	GFResizePic(gump, 15, 15, GFGetCfgConst("Defaults", "ForeGround"), 510, 25);
	GFTextMid(gump, 15, 20, 570, 2100, "Help Queue");

	// Left bar
	GFResizePic(gump, 15, 45, GFGetCfgConst("Defaults", "ForeGround"), 125, 325);

	// Content Area
	GFResizePic(gump, 145, 45, GFGetCfgConst("Defaults", "ForeGround"), 380, 325);

	return gump;
endfunction

function ShowPagesInQueue(queue_name)
	var gump := BuildGumpTemplate();
	return gump;
endfunction

function ShowQueueTypes(cmdlevel)
	var gump := BuildGumpTemplate();
	var y_pos := 50;
	foreach level in ( GetQueueNamesAccess(cmdlevel) )
		GFAddButton(gump, 20, y_pos+3, 2117, 2118, GF_CLOSE_BTN, SHOW_QUEUE_START+_level_iter);
		GFTextLine(gump, 40, y_pos, 2100, level);
		y_pos += 20;
		SleepMS(2);
	endforeach

	return gump;
endfunction

function ShowPageInQueue(queue_name)
	var gump := BuildGumpTemplate();
	return gump;
endfunction