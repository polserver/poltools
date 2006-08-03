/* $Id$
 *
 */

include ":gumps:gumps";
include ":gumps:gumps_ex";
include ":email:settings";

// BUTTON IDs
CONST NEW_LETTER_BTN	:= 0xA0000;
CONST DELETE_ALL_BTN	:= 0xA0001;
CONST DELETE_MSG_BTN	:= 0xA0002;
CONST CLOSE_EMAIL_BTN	:= 0xA0003;
CONST WORD_WRAP_BTN	:= 0xA0004;
CONST SEND_EMAIL_BTN	:= 0xA0005;
CONST ADDRESS_BOOK_BTN	:= 0xA0006;
CONST BLOCK_LIST_BTN	:= 0xA0007;
CONST UPDATE_INBOX_BTN	:= 0xA0008;
CONST UPDATE_LIST_BTN	:= 0xA0009;

// THE SEND BUTTONS NEXT TO AN ADDRESS BOOK BUTTON
CONST ADDYBOOK_START	:= 0xB0009;

// TEXT ENTRY IDs
CONST SEND_TO_TXT	:= 0xB01;
CONST SUBJECT_TXT	:= 0xB02;
CONST TXT_START		:= 0xB05;

function BuildInboxGump(byref box_id, byref data_elem)
	var gump := GFCreateGump();
	GFMovable(gump, 0);
	GFResizePic(gump, 0, 0, GFGetCfgConst("Defaults", "BackGround"), 640, 480);

	GFResizePic(gump, 15, 15, GFGetCfgConst("Defaults", "ForeGround"), 480, 450);
	GFTextLine(gump, 20, 20, 2100, "Read");
	GFTextLine(gump, 80, 20, 2100, "Sender");
	GFTextLine(gump, 250, 20, 2100, "Subject");

	var y_pos := 50;
	var counter := 1;

	GFPage(gump, 1);
	foreach mail_number in ( data_elem.PropNames() )
		if ( mail_number != "NextMsgID" )
			var mail_info := data_elem.GetProp(mail_number);
			GFAddButton(gump, 30, y_pos+3, 2117, 2118, GF_CLOSE_BTN, mail_number);
			GFTextLine(gump, 80, y_pos, 2100, mail_info.from_name);
			GFTextCrop(gump, 250, y_pos, 210, 20, 2100, mail_info.subject);
			y_pos += 20;
			counter += 1;

			if ( counter >= 20 )
				GFAddButton(gump, 465, 440, 2648, 2649, GF_PAGE_BTN, gump.cur_page+1);
				GFPage(gump, gump.cur_page+1);
				GFAddButton(gump, 465, 20, 2650, 2651, GF_PAGE_BTN, gump.cur_page-1);
				counter := 1;
				y_pos := 50;
			endif

		endif
	endforeach

	GFPage(gump, 0);
	GFResizePic(gump, 500, 15, GFGetCfgConst("Defaults", "ForeGround"), 125, 50);
	GFTextMid(gump, 500, 20, 125, 2100, "Box Number");
	GFTextMid(gump, 500, 40, 125, 2100, CStr(box_id));

	GFResizePic(gump, 500, 70, GFGetCfgConst("Defaults", "ForeGround"), 125, 395);

	GFAddButton(gump, 505, 83, 2117, 2118, GF_CLOSE_BTN, UPDATE_INBOX_BTN);
	GFTextLine(gump, 525, 80, 2100, "Update Inbox");

	GFAddButton(gump, 505, 108, 2117, 2118, GF_CLOSE_BTN, NEW_LETTER_BTN);
	GFTextLine(gump, 525, 105, 2100, "New Email");

	GFAddButton(gump, 505, 133, 2117, 2118, GF_CLOSE_BTN, DELETE_ALL_BTN);
	GFTextLine(gump, 525, 130, 2100, "Delete All");

	GFAddButton(gump, 505, 158, 2117, 2118, GF_CLOSE_BTN, ADDRESS_BOOK_BTN);
	GFTextLine(gump, 525, 155, 2100, "Address Book");

	GFAddButton(gump, 505, 183, 2117, 2118, GF_CLOSE_BTN, BLOCK_LIST_BTN);
	GFTextLine(gump, 525, 180, 2100, "Block List");

	GFAddButton(gump, 505, 208, 2117, 2118, GF_CLOSE_BTN, CLOSE_EMAIL_BTN);
	GFTextLine(gump, 525, 205, 2100, "Close");

	return gump;
endfunction

function BuildBlockListGump(data_elem, byref entry_list)
	var gump := GFCreateGump();
	GFMovable(gump, 0);

	GFResizePic(gump, 0, 0, GFGetCfgConst("Defaults", "BackGround"), 640, 480);
	GFResizePic(gump, 15, 15, GFGetCfgConst("Defaults", "ForeGround"), 610, 450);
	GFTextMid(gump, 15, 25, 620, 2100, "Block List");

	GFAddButton(gump, 25, 428, 2117, 2118, GF_CLOSE_BTN, UPDATE_LIST_BTN);
	GFTextLine(gump, 45, 425, 2100, "Update List");

	GFAddButton(gump, 225, 428, 2117, 2118, GF_CLOSE_BTN, DELETE_ALL_BTN);
	GFTextLine(gump, 245, 425, 2100, "Erase List");

	var y_pos := 65;
	var entry_num := 1;
	var counter := 1;
	var entry_info;

	GFPage(gump, 1);

	for ( entry_num; entry_num<=settings.BlockListEntries; entry_num+=1 )
		entry_info := data_elem.GetProp(Hex(entry_num));
		if ( !entry_info )
			entry_info := struct{"boxid":="", "name":=""};
		endif

		if ( entry_num % 2 == 0 )
			//
			// Right boxes in the line
			//
			GFTextLine(gump, 345, y_pos, 2100, "Number");
			GFTextLine(gump, 445, y_pos, 2100, "Name");
			GFResizePic(gump, 345, y_pos+20, GFGetCfgConst("Backgrounds", "GOLDTRIM"), 100, 30);
			entry_info.boxid := GFTextEntry(gump, 355, y_pos+25, 80, 20, 2100, entry_info.boxid);
			GFResizePic(gump, 440, y_pos+20, GFGetCfgConst("Backgrounds", "GOLDTRIM"), 180, 30);
			entry_info.name := GFTextEntry(gump, 450, y_pos+25, 180, 20, 2100, entry_info.name);

			y_pos += 50;
			counter += 1;
		else
			//
			// Left boxes on the line
			//
			GFTextLine(gump, 20, y_pos, 2100, "Number");
			GFTextLine(gump, 120, y_pos, 2100, "Name");
			GFResizePic(gump, 20, y_pos+20, GFGetCfgConst("Backgrounds", "GOLDTRIM"), 100, 30);
			entry_info.boxid := GFTextEntry(gump, 30, y_pos+25, 80, 20, 2100, entry_info.boxid);
			GFResizePic(gump, 115, y_pos+20, GFGetCfgConst("Backgrounds", "GOLDTRIM"), 180, 30);
			entry_info.name := GFTextEntry(gump, 125, y_pos+25, 180, 20, 2100, entry_info.name);
		endif
		entry_list[entry_num] := entry_info;

		if ( ( counter > 7 ) && ( entry_num < settings.BlockListEntries) )
			GFAddButton(gump, 605, 420, 2648, 2649, GF_PAGE_BTN, gump.cur_page+1);
			GFPage(gump, gump.cur_page+1);
			GFAddButton(gump, 605, 40, 2650, 2651, GF_PAGE_BTN, gump.cur_page-1);

			counter := 1;
			y_pos := 65;
		endif

		SleepMS(2);
	endfor

	return gump;
endfunction

function BuildAddressBookGump(data_elem, byref entry_list)
	var gump := GFCreateGump();
	GFMovable(gump, 0);

	GFResizePic(gump, 0, 0, GFGetCfgConst("Defaults", "BackGround"), 640, 480);
	GFResizePic(gump, 15, 15, GFGetCfgConst("Defaults", "ForeGround"), 610, 450);
	GFTextMid(gump, 15, 25, 620, 2100, "Block List");

	GFAddButton(gump, 25, 428, 2117, 2118, GF_CLOSE_BTN, UPDATE_LIST_BTN);
	GFTextLine(gump, 45, 425, 2100, "Update List");

	GFAddButton(gump, 225, 428, 2117, 2118, GF_CLOSE_BTN, DELETE_ALL_BTN);
	GFTextLine(gump, 245, 425, 2100, "Erase List");

	var y_pos := 65;
	var entry_num := 1;
	var counter := 1;
	var entry_info;

	GFPage(gump, 1);

	for ( entry_num; entry_num<=settings.AddressBookEntries; entry_num+=1 )
		entry_info := data_elem.GetProp(Hex(entry_num));
		if ( !entry_info )
			entry_info := struct{"boxid":="", "name":=""};
		endif

		if ( entry_num % 2 == 0 )
			//
			// Right boxes in the line
			//
			GFTextLine(gump, 345, y_pos, 2100, "Number");
			GFTextLine(gump, 445, y_pos, 2100, "Name");
			GFResizePic(gump, 345, y_pos+20, GFGetCfgConst("Backgrounds", "GOLDTRIM"), 100, 30);
			entry_info.boxid := GFTextEntry(gump, 355, y_pos+25, 80, 20, 2100, entry_info.boxid);
			GFResizePic(gump, 440, y_pos+20, GFGetCfgConst("Backgrounds", "GOLDTRIM"), 180, 30);
			entry_info.name := GFTextEntry(gump, 450, y_pos+25, 140, 20, 2100, entry_info.name);
			GFAddButton(gump, 590, y_pos+25, 4029, 4030, GF_CLOSE_BTN, ADDYBOOK_START+entry_num);

			y_pos += 50;
			counter += 1;
		else
			//
			// Left boxes on the line
			//
			GFTextLine(gump, 20, y_pos, 2100, "Number");
			GFTextLine(gump, 120, y_pos, 2100, "Name");
			GFResizePic(gump, 20, y_pos+20, GFGetCfgConst("Backgrounds", "GOLDTRIM"), 100, 30);
			entry_info.boxid := GFTextEntry(gump, 30, y_pos+25, 80, 20, 2100, entry_info.boxid);
			GFResizePic(gump, 115, y_pos+20, GFGetCfgConst("Backgrounds", "GOLDTRIM"), 180, 30);
			entry_info.name := GFTextEntry(gump, 125, y_pos+25, 140, 20, 2100, entry_info.name);
			GFAddButton(gump, 265, y_pos+23, 4029, 4030, GF_CLOSE_BTN, ADDYBOOK_START+entry_num);
		endif
		entry_list[entry_num] := entry_info;

		if ( ( counter > 7 ) && ( entry_num < settings.AddressBookEntries) )
			GFAddButton(gump, 605, 420, 2648, 2649, GF_PAGE_BTN, gump.cur_page+1);
			GFPage(gump, gump.cur_page+1);
			GFAddButton(gump, 605, 40, 2650, 2651, GF_PAGE_BTN, gump.cur_page-1);

			counter := 1;
			y_pos := 65;
		endif

		SleepMS(2);
	endfor

	return gump;
endfunction

function BuildComposeGump(send_to:="", subject:="", byref msg_array:=array)
	var gump := GFCreateGump();

	GFMovable(gump, 0);
	GFResizePic(gump, 0, 0, GFGetCfgConst("Defaults", "BackGround"), 640, 480);

	GFResizePic(gump, 15, 15, GFGetCfgConst("Defaults", "ForeGround"), 480, 80);
	GFTextLine(gump, 20, 23, 2100, "Send To :");
	GFGumpPic(gump, 100, 20, 1141);
	GFTextEntry(gump, 110, 22, 250, 20, 2100, send_to, SEND_TO_TXT);
	GFAddButton(gump, 380, 23, 4008, 4009, GF_CLOSE_BTN, ADDRESS_BOOK_BTN);
	GFTextLine(gump, 415, 23, 2100, "Addresses");

	GFTextLine(gump, 20, 48, 2100, "Subject :");
	GFGumpPic(gump, 100, 45, 1141);
	GFTextEntry(gump, 110, 47, 250, 20, 2100, subject, SUBJECT_TXT);
	GFTextLine(gump, 20, 70, 2100, "To send to multiple recipients, separate box numbers with a space.");

	GFResizePic(gump, 500, 15, GFGetCfgConst("Defaults", "ForeGround"), 125, 80);
	GFAddButton(gump, 505, 23, 2117, 2118, GF_CLOSE_BTN, SEND_EMAIL_BTN);
	GFTextLine(gump, 525, 20, 2100, "Send");
	GFAddButton(gump, 505, 43, 2117, 2118, GF_CLOSE_BTN, WORD_WRAP_BTN);
	GFTextLine(gump, 525, 40, 2100, "Word Wrap");
	GFAddButton(gump, 505, 63, 2117, 2118, GF_CLOSE_BTN, CLOSE_EMAIL_BTN);
	GFTextLine(gump, 525, 60, 2100, "Cancel");

	var y_pos := 100;
	var i;
	GFResizePic(gump, 15, 95, GFGetCfgConst("Backgrounds", "GOLDTRIM"), 610, 370);
	for ( i:=1; i<=18; i+=1 )
		var text := msg_array[i];
		if ( !text )
			text := "";
		endif
		msg_array[i] := GFTextEntry(gump, 20, y_pos, 595, 20, 2100, text, TXT_START+i);
		y_pos += 20;
		SleepMS(2);
	endfor

	return gump;
endfunction