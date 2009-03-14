/* $Id$
 *
 */

include "include/arrays";

var chestconfig := ReadConfigFile(":containers:townchest");
var chestlist := GetConfigStringKeys(chestconfig);

function TC_Build_Gump()

// Created with Gump Studio.
// Exported with POLGumpExporter ver 1.1 for gump pkg

	var gump := GFCreateGump();

	GFPage(gump, 0);
	GFResizePic(gump, 0, 0, 3600, 450, 450);
	GFResizePic(gump, 15, 15, 5054, 420, 420);
	GFResizePic(gump, 30, 30, 9270, 390, 40);
	GFTextLine(gump, 299, 404, 1152, "Respawn");
	GFAddButton(gump, 23, 404, 247, 248, GF_CLOSE_BTN, 100);
	GFGumpPic(gump, 361, 403, 2443, 0);
	GFTextEntry(gump, 371, 404, 34, 20, 1152, "30", 20);
	GFTextLine(gump, 27, 377, 1152, "Configuration Page:");
	GFTextLine(gump, 263, 378, 1152, "Chest Type Page:");
	GFAddButton(gump, 157, 381, 1209, 1210, GF_PAGE_BTN, 2);
	GFAddButton(gump, 374, 381, 1209, 1210, GF_PAGE_BTN, 3);
	GFAddButton(gump, 91, 404, 241, 242, GF_CLOSE_BTN, 0);
	GFAddButton(gump, 159, 404, 2031, 2032, GF_PAGE_BTN, 1);

	// Now we populate the HTML Text Area from the townChest.hlp file!
	var text_lines := ReadFile(":containers:townChest/townchest.txt");
	var text := ImplodeArray("\n", text_lines);

	if ( !text )
		text := "Someone has not written the Town Chest Spawn Command Help File!";
	endif

	GFPage(gump, 1);
	GFResizePic(gump, 30, 30, 9270, 390, 40);
	GFTextLine(gump, 122, 39, 1700, "Town Chest Spawn Configuration");
	GFResizePic(gump, 30, 74, 9270, 390, 108);
	GFTextLine(gump, 183, 82, 1700, "Select Chest");
	GFSetRadioGroup(gump, 1);
	GFRadioButton(gump, 49, 146, 210, 211, 1, 56559);
	GFRadioButton(gump, 82, 146, 210, 211, 0, 56558);
	GFRadioButton(gump, 119, 146, 210, 211, 0, 56556);
	GFRadioButton(gump, 156, 146, 210, 211, 0, 56555);
	GFRadioButton(gump, 193, 146, 210, 211, 0, 56560);
	GFRadioButton(gump, 233, 146, 210, 211, 0, 56554);
	GFRadioButton(gump, 284, 146, 210, 211, 0, 56553);
	GFRadioButton(gump, 377, 146, 210, 211, 0, 56552);
	GFRadioButton(gump, 322, 145, 210, 211, 0, 56551);
	GFTilePic(gump, 30, 101, 2473, 0);
	GFTilePic(gump, 63, 101, 3647, 0);
	GFTilePic(gump, 101, 101, 3646, 0);
	GFTilePic(gump, 140, 99, 3644, 0);
	GFTilePic(gump, 179, 99, 3645, 0);
	GFTilePic(gump, 219, 99, 3650, 0);
	GFTilePic(gump, 271, 99, 3651, 0);
	GFTilePic(gump, 364, 99, 3649, 0);
	GFTilePic(gump, 308, 99, 3648, 0);
	GFResizePic(gump, 129, 187, 9270, 193, 184);
	GFTextLine(gump, 185, 196, 1700, "Select Level");
	GFSetRadioGroup(gump, 2);
	GFRadioButton(gump, 146, 230, 210, 211, 1, 101);
	GFRadioButton(gump, 146, 255, 210, 211, 0, 102);
	GFRadioButton(gump, 146, 278, 210, 211, 0, 103);
	GFRadioButton(gump, 146, 303, 210, 211, 0, 104);
	GFRadioButton(gump, 146, 327, 210, 211, 0, 105);
	GFTextLine(gump, 171, 230, 1152, "Level One");
	GFTextLine(gump, 171, 255, 1152, "Level Two");
	GFTextLine(gump, 172, 279, 1152, "Level Three");
	GFTextLine(gump, 171, 303, 1152, "Level Four");
	GFTextLine(gump, 171, 327, 1152, "Level Five");
	GFTextLine(gump, 299, 404, 1152, "Respawn");
	GFGumpPic(gump, 361, 403, 2443, 0);
	GFTextEntry(gump, 371, 404, 34, 20, 1152, "30", 20);

	// Page 3 is where it gets interesting. This will all be
	// generated from the townchest.cfg file to list all the
	// types of chest groups we can display. Max is 27, because
	// I didn't feel like there being more pages than this. :P
	GFPage(gump, 2);
	GFTextLine(gump, 149, 39, 1700, "Town Chest Spawn Types");
	GFResizePic(gump, 30, 75, 9270, 390, 293);
	GFSetRadioGroup(gump, 3);

	var chestx:=48, chesty:=93, textx:=73, texty:=93, chestid:=201, checked:=1;

	foreach chesttype in (chestlist)
		GFRadioButton(gump, chestx, chesty, 210, 211, checked, chestid);
		GFTextLine(gump, textx, texty, 1152, chestconfig[chesttype].name);
		if ( checked == 1 )
			checked := 0;
		endif
		if ( chestx == 286 && texty == 333 )
			break;
		endif
		if ( chestx != 286 )
			chestx += 119;
			textx += 119;
			chestid += 1;
		elseif ( chestx == 286 )
			chestx := 48;
			chesty += 30;
			textx := 73;
			texty += 30;
			chestid += 1;
		endif
	endforeach

	GFPage(gump, 3);
	GFTextLine(gump, 137, 39, 1700, "Town Chest Spawn Command");
	GFHTMLArea(gump, 30, 75, 391, 293, text, 1, 1);

	return gump;

endfunction

function TC_RetrieveGump(elem_name, prop_name)

	var elem := DFFindElement(tc_data_gumps, elem_name);
	return (elem.GetProp(prop_name));

endfunction

function TC_SaveGump(elem_name, gump, prop_name)

	var elem := DFFindElement(tc_data_gumps, elem_name, DF_CREATE);
	return (elem.SetProp(prop_name, gump));

endfunction

function TC_RefreshGump(elem_name, prop_name)

	var elem := DFFindElement(tc_data_gumps, elem_name);
	return (elem.EraseProp(prop_name));

endfunction

function TCExtractType(key)

	key := key-200;

	return chestlist[key];

endfunction
