/* $Id$
 *
 */

include ":gumps:gumps";
include ":gumps:gumps_ex";

CONST CLOSE_BTN	:= 0xA00;

function BuildGumpTemplate(byref settings)
	var gump := GFCreateGump();
	GFResizePic(gump, 0, 0, GFGetCfgConst("Defaults", "BackGround"), 600, 450);
	GFResizePic(gump, 15, 15, GFGetCfgConst("Defaults", "ForeGround"), 570, 25);
	GFTextMid(gump, 15, 20, 570, 2100, "Help Menu");

	GFResizePic(gump, 205, 45, GFGetCfgConst("Defaults", "ForeGround"), 380, 325);

	BuildGMBox(gump, settings);
	BuildBottomBar(gump, settings);

	BuildPage1Gump(gump, settings);

	return gump;
endfunction

function BuildGMBox(byref gump, byref settings)
	GFResizePic(gump, 15, 45, GFGetCfgConst("Defaults", "ForeGround"), 185, 325);
	GFResizePic(gump, 20, 50, GFGetCfgConst("BackGrounds", "GoldTrim"), 175, 315);
	GFGumpPic(gump, 20, 125, 0xC, 1023);
	GFGumpPic(gump, 20, 125, 0xC72B, 2101);
	GFGumpPic(gump, 20, 125, 0xC5B9);

	var text := "Greetings, traveler! [P]
		This menu will help guide you to the best place for answers about
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
	var text;

	GFPage(gump, 1);
	GFAddButton(gump, 210, 50, 4005, 4006, GF_PAGE_BTN, 2);
	GFTextLine(gump, 250, 50, 1720, "General questions about "+settings.ShardName);
	text := "Select this option if you are having difficulties learning to use a skill,
		if you are lost or if you have general gameplay questions.";
	DrawWrappedText(gump, 250, 70, GFWordWrap(text, 380));


	return 1;
endfunction

function DrawWrappedText(byref gump, x_pos, start_y, byref text_lines)
	foreach line in ( text_lines )
		GFTextLine(gump, x_pos, start_y, 2100, line);
		start_y += 20;
		SleepMS(2);
	endforeach

	return 1;
endfunction