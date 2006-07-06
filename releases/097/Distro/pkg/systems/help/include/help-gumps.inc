/* $Id$
 *
 */

include ":datafile:datafile";
include ":gumps:gumps";
include ":gumps:gumps_ex";
include ":help:help";
include ":help:settings";
include "include/arrays";

CONST CLOSE_BTN			:= 0xA00;
CONST MAIN_PAGE_BTN		:= 0xA01;
CONST COUNSELOR_PAGE_BTN	:= 0xA02;
CONST STAFF_PAGE_BTN		:= 0xA03;
CONST SEND_PAGE_BTN		:= 0xA04;
CONST HELP_TOPICS_BTN		:= 0xA05;
CONST WORD_WRAP_BTN		:= 0xA06;

CONST HELP_TOPIC_START		:= 0xA001;
CONST PAGE_TXT_START		:= 0xB001;
CONST PAGE_TXT_END		:= 0xB00D;

function BuildGumpTemplate(byref settings)
	var gump := LoadHelpInfo("Template", "Gump");

	if ( gump )
		return gump;
	endif

	var text;
	gump := GFCreateGump();
	GFResizePic(gump, 0, 0, GFGetCfgConst("Defaults", "BackGround"), 600, 450);
	GFResizePic(gump, 15, 15, GFGetCfgConst("Defaults", "ForeGround"), 570, 25);
	GFTextMid(gump, 15, 20, 570, 2100, "Help Menu");

	// Main stone area where content goes
	GFResizePic(gump, 205, 45, GFGetCfgConst("Defaults", "ForeGround"), 380, 325);

	// Box with the GM in it
	GFResizePic(gump, 15, 45, GFGetCfgConst("Defaults", "ForeGround"), 185, 325);
	GFResizePic(gump, 20, 50, GFGetCfgConst("BackGrounds", "GoldTrim"), 175, 315);
	GFGumpPic(gump, 20, 125, 0xC, 1023);
	GFGumpPic(gump, 20, 125, 0xC72B, 2101);
	GFGumpPic(gump, 20, 125, 0xC5B9);
	GFGumpPic(gump, 20, 100, 0xC541, 1720); // Halo ;p
	text := "This menu will help guide you to the best place for answers about
		"+settings.ShardName+". Please choose from the options on the right.";
	DrawWrappedText(gump, 25, 55, GFWordWrap(text, 175));

	// Bar along the bottom with URL and cancel button.
	GFResizePic(gump, 15, 375, GFGetCfgConst("Defaults", "ForeGround"), 570, 60);
	text := "If you cannot find the help you require on this list, please visit our
		webpage at "+settings.website+" for more information.";
	DrawWrappedText(gump, 20, 380, GFWordWrap(text, 570));
	GFAddButton(gump, 500, 410, 2119, 2120, GF_CLOSE_BTN, CLOSE_BTN);

	SaveHelpInfo("Template", "Gump", gump);

	return gump;
endfunction

function BuildIntroGump(byref settings)
	var gump := BuildGumpTemplate(settings);
	var text, y_pos := 50;

	GFAddButton(gump, 210, y_pos, 4005, 4006, GF_CLOSE_BTN, HELP_TOPICS_BTN);
	GFTextLine(gump, 245, y_pos, 1720, "General questions about "+settings.ShardName+".");
	text := "Select this option if you are having difficulties learning to use a skill,
		if you are lost or if you have general gameplay questions.";
	DrawWrappedText(gump, 250, (y_pos+=20), GFWordWrap(text, 380));

	GFAddButton(gump, 210, y_pos, 4011, 4030, GF_CLOSE_BTN, COUNSELOR_PAGE_BTN);
	GFTextLine(gump, 245, y_pos, 1720, "Contact a counselor.");
	text := "A counselor is an experienced player on the shard who has volunteered time
		to help answer questions. Selecting this option will let you send a message to a
		counselor. Please remember they are volunteers and may not be availale at all times.";
	DrawWrappedText(gump, 250, (y_pos+=20), GFWordWrap(text, 380));

	GFAddButton(gump, 210, y_pos, 4011, 4030, GF_CLOSE_BTN, STAFF_PAGE_BTN);
	GFTextLine(gump, 245, y_pos, 1720, "Contact a staff member.");
	text := "If you are experiencing a problem in the game such as being stuck, harased by
		another player or other game stopping issue, you may contact a game master or other
		staff member here.";
	DrawWrappedText(gump, 250, (y_pos+=20), GFWordWrap(text, 380));

	return gump;
endfunction

function BuildTextEntryGump(title,  byref settings, byref cur_txt)
	var gump := BuildGumpTemplate(settings);

	GFTextMid(gump, 210, 50, 380, 2100, title);

	GFResizePic(gump, 210, 70, GFGetCfgConst("BackGrounds", "GoldTrim"), 370, 270);
	var i;
	var y_pos := 75;
	for ( i:=PAGE_TXT_START; i<=PAGE_TXT_END; i+=1 )
		var cur_line := cur_txt[i-(PAGE_TXT_START-1)];
		if ( !cur_line )
			cur_line := "";
		endif

		GFTextEntry(gump, 215, y_pos, 360, 20, 2100, cur_line, i);

		y_pos += 20;
		SleepMS(2);
	endfor

	GFAddButton(gump, 210, 345, 4014, 4015, GF_CLOSE_BTN, MAIN_PAGE_BTN);
	GFTextLine(gump, 245, 345, 2100, "Back");

	GFAddButton(gump, 330, 348, 2117, 2118, GF_CLOSE_BTN, WORD_WRAP_BTN);
	GFTextLine(gump, 350, 345, 2100, "Word Wrap");

	GFAddButton(gump, 550, 345, 4029, 4030, GF_CLOSE_BTN, SEND_PAGE_BTN);
	GFTextLine(gump, 485, 345, 2100, "Send Page");

	return gump;
endfunction

function BuildHelpTopicsIndexGump(byref settings)
	var gump := BuildGumpTemplate(settings);

	GFAddButton(gump, 210, 345, 4014, 4015, GF_CLOSE_BTN, MAIN_PAGE_BTN);
	GFTextLine(gump, 245, 345, 2100, "Back");

	GFPage(gump, 1);
	var y_pos := 50;
	var help_cfg := ReadConfigFile(":help:helptopics");
	var elem_names := GetConfigStringKeys(help_cfg);

	SaveHelpInfo("TopicNames", "List", elem_names);

	foreach elem_name in ( elem_names )
		GFAddButton(gump, 210, (y_pos+3), 2117, 2118, GF_CLOSE_BTN, HELP_TOPIC_START+_elem_name_iter);
		GFTextLine(gump, 230, y_pos, 2100, elem_name);
		y_pos += 20;

		if ( _elem_name_iter % 14 == 0 )
			GFAddButton(gump, 565, 350, 0xA58, 0xA59, GF_PAGE_BTN, gump.cur_page+1);
			GFPage(gump, gump.cur_page+1);
			GFAddButton(gump, 565, 46, 0xA5A, 0xA5B, GF_PAGE_BTN, gump.cur_page-1);
			y_pos := 50;
		endif

		SleepMS(2);
	endforeach

	return gump;
endfunction

function BuildHelpTopicGump(topic_name, byref settings)
	var gump := BuildGumpTemplate(settings);

	GFAddButton(gump, 210, 345, 4014, 4015, GF_CLOSE_BTN, HELP_TOPICS_BTN);
	GFTextLine(gump, 245, 345, 2100, "Back");

	GFPage(gump, 1);
	var help_cfg := ReadConfigFile(":help:helptopics");
	var help_elem := help_cfg[topic_name];

	var text := "<CENTER><U>"+topic_name+"</U></CENTER>";
	text += ImplodeArray(" ", GetConfigStringArray(help_elem, "Text"));

	GFHTMLArea(gump, 210, 50, 370, 270, text, 1, 1);

	return gump;
endfunction

function DrawWrappedText(byref gump, byref x_pos, byref y_pos, byref text_lines)
	foreach line in ( text_lines )
		GFTextLine(gump, x_pos, y_pos, 2100, line);
		y_pos += 20;
		SleepMS(2);
	endforeach

	return 1;
endfunction
