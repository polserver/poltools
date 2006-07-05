/* $Id$
 *
 */

include ":gumps:gumps";
include ":gumps:gumps_ex";
include ":help:settings";

CONST CLOSE_BTN			:= 0xA001;
CONST COUNSELOR_PAGE_BTN	:= 0xA002;
CONST STAFF_PAGE_BTN		:= 0xA003;

CONST COUN_TXT_START		:= 0xA0001;
CONST STAFF_TXT_START		:= 0xB0002;

function BuildGumpTemplate()
	var settings := HS_GetSettingsCfgElem("Settings");

	var gump := GFCreateGump();
	GFResizePic(gump, 0, 0, GFGetCfgConst("Defaults", "BackGround"), 600, 450);
	GFResizePic(gump, 15, 15, GFGetCfgConst("Defaults", "ForeGround"), 570, 25);
	GFTextMid(gump, 15, 20, 570, 2100, "Help Menu");

	GFResizePic(gump, 205, 45, GFGetCfgConst("Defaults", "ForeGround"), 380, 325);

	BuildGMBox(gump, settings);
	BuildBottomBar(gump, settings);

	// The "template" ends here, and the stuff im jsut working on begins below..

	BuildPage1Gump(gump, settings);

	BuildHelpTopicsIndexGump(gump);

	BuildTextEntry(gump, "Counselor Page", COUNSELOR_PAGE_BTN, COUN_TXT_START);
	BuildTextEntry(gump, "Staff Page", STAFF_PAGE_BTN, STAFF_TXT_START);

	// End non template stuff

	return gump;
endfunction

function BuildGMBox(byref gump, byref settings)
	GFResizePic(gump, 15, 45, GFGetCfgConst("Defaults", "ForeGround"), 185, 325);
	GFResizePic(gump, 20, 50, GFGetCfgConst("BackGrounds", "GoldTrim"), 175, 315);
	GFGumpPic(gump, 20, 125, 0xC, 1023);
	GFGumpPic(gump, 20, 125, 0xC72B, 2101);
	GFGumpPic(gump, 20, 125, 0xC5B9);
	GFGumpPic(gump, 20, 100, 0xC541, 1720); // Halo ;p

	var text := "This menu will help guide you to the best place for answers about
		"+settings.ShardName+". Please choose from the options on the right.";

	DrawWrappedText(gump, 25, 55, GFWordWrap(text, 175));

	return 1;
endfunction

function BuildBottomBar(byref gump, byref settings)
	GFResizePic(gump, 15, 375, GFGetCfgConst("Defaults", "ForeGround"), 570, 60);

	var text := "If you cannot find the help you require on this list, please visit our
		webpage at "+settings.website+" for more information.";

	DrawWrappedText(gump, 20, 380, GFWordWrap(text, 570));

	GFAddButton(gump, 500, 410, 2119, 2120, GF_CLOSE_BTN, CLOSE_BTN);

	return 1;
endfunction

function BuildPage1Gump(byref gump, byref settings)
	var text, y_pos := 50;

	GFPage(gump, 1);
	GFAddButton(gump, 210, y_pos, 4005, 4006, GF_PAGE_BTN, 2);
	GFTextLine(gump, 245, y_pos, 1720, "General questions about "+settings.ShardName);
	text := "Select this option if you are having difficulties learning to use a skill,
		if you are lost or if you have general gameplay questions.";
	DrawWrappedText(gump, 250, (y_pos+=20), GFWordWrap(text, 380));

	GFAddButton(gump, 210, y_pos, 4011, 4030, GF_PAGE_BTN, COUNSELOR_PAGE_BTN);
	GFTextLine(gump, 245, y_pos, 1720, "Contact a counselor");
	text := "A counselor is an experienced player on the shard who has volunteered time
		to help answer questions. Selecting this option will let you send a message to a
		counselor. Please remember they are volunteers and may not be availale at all times.";
	DrawWrappedText(gump, 250, (y_pos+=20), GFWordWrap(text, 380));

	GFAddButton(gump, 210, y_pos, 4011, 4030, GF_PAGE_BTN, STAFF_PAGE_BTN);
	GFTextLine(gump, 245, y_pos, 1720, "Contact a staff member.");
	text := "If you are experiencing a problem in the game such as being stuck, harased by
		another player or other game stopping issue, you may contact a game master or other
		staff member here.";
	DrawWrappedText(gump, 250, (y_pos+=20), GFWordWrap(text, 380));

	return 1;
endfunction

function BuildTextEntry(byref gump, title, page_num, txt_start)
	GFPage(gump, page_num);
	GFTextMid(gump, 210, 50, 380, 2100, title);

	GFResizePic(gump, 210, 70, GFGetCfgConst("BackGrounds", "GoldTrim"), 370, 270);
	var i;
	var y_pos := 75;
	for ( i:=0; i<=12; i+=1 )
		GFTextEntry(gump, 215, y_pos, 360, 20, 2100, "", txt_start+i);
		y_pos += 20;
		SleepMS(2);
	endfor


	GFAddButton(gump, 210, 345, 4014, 4015, GF_PAGE_BTN, 1);
	GFTextLine(gump, 245, 345, 2100, "Back");

	GFAddButton(gump, 550, 345, 4029, 4030, GF_CLOSE_BTN, STAFF_PAGE_BTN);
	GFTextLine(gump, 485, 345, 2100, "Send Page");

	return 1;
endfunction

function BuildHelpTopicsIndexGump(byref gump)
	GFPage(gump, 2);

	GFAddButton(gump, 210, 345, 4014, 4015, GF_PAGE_BTN, 1);
	GFTextLine(gump, 245, 345, 2100, "Back");

	return 1;
endfunction

function DrawWrappedText(byref gump, byref x_pos, byref y_pos, byref text_lines)
	foreach line in ( text_lines )
		GFTextLine(gump, x_pos, y_pos, 2100, line);
		y_pos += 20;
		SleepMS(2);
	endforeach

	return 1;
endfunction