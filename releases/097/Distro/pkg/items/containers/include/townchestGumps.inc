/* $Id$
 *
 */

// Gump backgrounds - Used in various functions - easier to use globally.
var BKGND_SILVERTRIM := GFGetCfgConst("Defaults", "BackGround");
var BKGND_DARK_STONE := GFGetCfgConst("Defaults", "ForeGround");


function TC_Build_Gump()

// Created 3/5/2009 12:38:22 AM, with Gump Studio.
// Exported with POLGumpExporter ver 1.1 for gump pkg

	var gump := GFCreateGump();

	GFPage(gump, 0);
	GFResizePic(gump, 0, 0, 3600, 450, 450);
	GFResizePic(gump, 15, 15, 5054, 420, 420);
	GFResizePic(gump, 30, 30, 9270, 390, 40);
	GFTextLine(gump, 122, 39, 1700, "Town Chest Spawn Configuration");
	GFResizePic(gump, 30, 74, 9270, 390, 108);
	GFTextLine(gump, 183, 82, 1700, "Select Chest");
	GFTilePic(gump, 30, 101, 2473, 0);
	GFTilePic(gump, 63, 101, 3647, 0);
	GFSetRadioGroup(gump, 1);
	GFRadioButton(gump, 49, 146, 210, 211, 1, 56559);
	GFRadioButton(gump, 82, 146, 210, 211, 0, 56558);
	GFTilePic(gump, 101, 101, 3646, 0);
	GFRadioButton(gump, 119, 146, 210, 211, 0, 56556);
	GFTilePic(gump, 140, 99, 3644, 0);
	GFRadioButton(gump, 156, 146, 210, 211, 0, 56555);
	GFTilePic(gump, 179, 99, 3645, 0);
	GFRadioButton(gump, 193, 146, 210, 211, 0, 56560);
	GFTilePic(gump, 219, 99, 3650, 0);
	GFRadioButton(gump, 233, 146, 210, 211, 0, 56554);
	GFTilePic(gump, 271, 99, 3651, 0);
	GFRadioButton(gump, 284, 146, 210, 211, 0, 56553);
	GFTilePic(gump, 364, 99, 3649, 0);
	GFRadioButton(gump, 377, 146, 210, 211, 0, 56552);
	GFTilePic(gump, 308, 99, 3648, 0);
	GFRadioButton(gump, 322, 145, 210, 211, 0, 56551);
	GFResizePic(gump, 30, 187, 9270, 193, 184);
	GFResizePic(gump, 227, 187, 9270, 193, 232);
	GFTextLine(gump, 86, 196, 1700, "Select Level");
	GFSetRadioGroup(gump, 2);
	GFRadioButton(gump, 47, 230, 210, 211, 1, 101);
	GFRadioButton(gump, 47, 255, 210, 211, 0, 102);
	GFTextLine(gump, 72, 230, 1152, "Level One");
	GFTextLine(gump, 72, 255, 1152, "Level Two");
	GFRadioButton(gump, 47, 278, 210, 211, 0, 103);
	GFRadioButton(gump, 47, 303, 210, 211, 0, 104);
	GFRadioButton(gump, 47, 327, 210, 211, 0, 105);
	GFTextLine(gump, 73, 279, 1152, "Level Three");
	GFTextLine(gump, 72, 303, 1152, "Level Four");
	GFTextLine(gump, 72, 327, 1152, "Level Five");
	GFTextLine(gump, 289, 196, 1700, "Select Type");
	GFSetRadioGroup(gump, 3);
	GFRadioButton(gump, 244, 230, 210, 211, 1, 201);
	GFRadioButton(gump, 244, 255, 210, 211, 0, 202);
	GFRadioButton(gump, 244, 279, 210, 211, 0, 203);
	GFRadioButton(gump, 244, 303, 210, 211, 0, 204);
	GFRadioButton(gump, 244, 327, 210, 211, 0, 205);
	GFRadioButton(gump, 244, 351, 210, 211, 0, 206);
	GFTextLine(gump, 269, 230, 1152, "Armory Items");
	GFTextLine(gump, 269, 255, 1152, "Food Items");
	GFTextLine(gump, 269, 279, 1152, "Special Items");
	GFTextLine(gump, 269, 303, 1152, "Mining Items");
	GFTextLine(gump, 269, 327, 1152, "Tinker Items");
	GFTextLine(gump, 269, 351, 1152, "Carpentry Items");
	GFRadioButton(gump, 244, 375, 210, 211, 0, 207);
	GFTextLine(gump, 269, 375, 1152, "Alchemy Items");
	GFTextEntry(gump, 179, 388, 34, 20, 1152, "30", 20);
	GFTextLine(gump, 110, 387, 1152, "Respawn");
	GFAddButton(gump, 22, 404, 247, 248, GF_CLOSE_BTN, 100);

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
	// We only get the keys we want that are for type
	case(key)
		201: return "Town"; break;
		202: return "Food"; break;
		203: return "Special"; break;
		204: return "Mining"; break;
		205: return "Tinker"; break;
		206: return "Carpentry"; break;
		207: return "Alchemy"; break;
		default: return "Town"; break;
	endcase
	return "Town";
endfunction