/* $Id$
 *
 */

/*===============================================================
* Current Version
* SELECTIONGUMP.INC - v1.0
* Updated 7/4/2006 1:10AM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

use uo;
use os;

include ":gumps:gumps";
include ":gumps:gumps_ex";

CONST BTN_TYPE_RADIO	:= 0x1;
CONST BTN_TYPE_CHECKBOX	:= 0x2;
CONST BTN_TYPE_NORMAL	:= 0x4;

/*
 * SelectionGump(mobile, options, btn_type)
 *
 * Purpose
 * Creates a rectangle shaped gump with buttons and text.
 *
 * Parameters
 * mobile:	Mobile to send the gump to.
 * options:	Array containing strings to display next to the buttons.
 * btn_type:	Type of button to display next to the text.
 *
 * Return Values
 * Returns an a single value from the options array if RADIO or NORMAL are used.
 * Returns an array of option values if CHECKBOX is used.
 * Returns an error on failure.
 *
 */
function SelectionGump(mobile, title, options, btn_type:=BTN_TYPE_NORMAL)
	if ( Lower(TypeOf(options)) != "array" )
		return error{"errortext":="Options list must be an array."};
	endif

	var s_gump := GFCreateGump();

	GFResizePic(s_gump, 0, 0, GFGetCfgConst("Defaults", "BackGround"), 310, 420);
	GFResizePic(s_gump, 15, 15, GFGetCfgConst("Defaults", "ForeGround"), 280, 390);
	GFTextMid(s_gump, 15, 20, 280, 1153, title);

	GFPage(s_gump, 1);
	var y_pos := 40;
	foreach entry in ( options )
		GFTextLine(s_gump, 20, y_pos, 1153, entry);
		y_pos := y_pos+20;
		SleepMS(2);
	endforeach

	var input := GFSendGump(mobile, s_gump);

endfunction
