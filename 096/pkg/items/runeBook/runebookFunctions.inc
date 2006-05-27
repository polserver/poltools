function CreateRunebookGump(book)
	var rune_list := GetRuneList(book);
	if( TypeOf(rune_list) != "Array" )
		rune_list := array{};
		SetObjProperty(book, "%RuneList%", rune_list);
	endif

	var charge_prop := GetObjProperty(book, "%Charges%");
	var charges := charge_prop["charges"];
	if( !charges )
		charges := 0;
	endif
	var max_charges := charge_prop["max_charges"];
	if( !max_charges )
		max_charges := 5;
	endif

	var book_gump := GFCreateGump();
// Begin Page 0 Setup
	GFGumpPic(book_gump, 100, 10, 2200);
	GFGumpPic(book_gump, 125, 50, 57);
	GFGumpPic(book_gump, 145, 50, 58);
	GFGumpPic(book_gump, 160, 50, 58);
	GFGumpPic(book_gump, 175, 50, 58);
	GFGumpPic(book_gump, 190, 50, 58);
	GFGumpPic(book_gump, 205, 50, 58);
	GFGumpPic(book_gump, 220, 50, 58);
	GFGumpPic(book_gump, 230, 50, 59);
	GFGumpPic(book_gump, 290, 50, 57);
	GFGumpPic(book_gump, 310, 50, 58);
	GFGumpPic(book_gump, 325, 50, 58);
	GFGumpPic(book_gump, 340, 50, 58);
	GFGumpPic(book_gump, 355, 50, 58);
	GFGumpPic(book_gump, 370, 50, 58);
	GFGumpPic(book_gump, 385, 50, 58);
	GFGumpPic(book_gump, 395, 50, 59);

	GFAddButton(book_gump, 130, 187, 2225, 2225, GF_PAGE_BTN, 2);
	GFAddButton(book_gump, 165, 187, 2226, 2226, GF_PAGE_BTN, 3);
	GFAddButton(book_gump, 200, 187, 2227, 2227, GF_PAGE_BTN, 4);
	GFAddButton(book_gump, 235, 187, 2228, 2228, GF_PAGE_BTN, 5);
	GFAddButton(book_gump, 300, 187, 2229, 2229, GF_PAGE_BTN, 6);
	GFAddButton(book_gump, 335, 187, 2230, 2230, GF_PAGE_BTN, 7);
	GFAddButton(book_gump, 370, 187, 2231, 2231, GF_PAGE_BTN, 8);
	GFAddButton(book_gump, 405, 187, 2232, 2232, GF_PAGE_BTN, 9);
	GFHTMLArea(book_gump,  140, 40, 80, 18, "Charges:");
	GFHTMLArea(book_gump,  300, 40, 100, 18, "Max Charges:");
	GFHTMLArea(book_gump,  220, 40, 80, 18, charges);
	GFHTMLArea(book_gump,  400, 40, 100, 18, max_charges);
// End Page 0 Setup

// Begin Page 1 Setup
	GFPage(book_gump, 1);
	GFAddButton(book_gump, 125, 15, 2472, 2473, GF_CLOSE_BTN, 50);
	GFHTMLArea(book_gump, 158, 22, 100, 18, "Rename Book");
	GFAddButton(book_gump, 393, 14, 2206, 2206, GF_PAGE_BTN, 2);

	GFAddButton(book_gump, 130, 65,  2103, 2104, GF_CLOSE_BTN, 101);
	GFAddButton(book_gump, 130, 80,  2103, 2104, GF_CLOSE_BTN, 102);
	GFAddButton(book_gump, 130, 95,  2103, 2104, GF_CLOSE_BTN, 103);
	GFAddButton(book_gump, 130, 110, 2103, 2104, GF_CLOSE_BTN, 104);
	GFAddButton(book_gump, 130, 125, 2103, 2104, GF_CLOSE_BTN, 105);
	GFAddButton(book_gump, 130, 140, 2103, 2104, GF_CLOSE_BTN, 106);
	GFAddButton(book_gump, 130, 155, 2103, 2104, GF_CLOSE_BTN, 107);
	GFAddButton(book_gump, 130, 170, 2103, 2104, GF_CLOSE_BTN, 108);

	GFAddButton(book_gump, 290, 65,  2103, 2104, GF_CLOSE_BTN, 119);
	GFAddButton(book_gump, 290, 80,  2103, 2104, GF_CLOSE_BTN, 120);
	GFAddButton(book_gump, 290, 95,  2103, 2104, GF_CLOSE_BTN, 121);
	GFAddButton(book_gump, 290, 110, 2103, 2104, GF_CLOSE_BTN, 122);
	GFAddButton(book_gump, 290, 125, 2103, 2104, GF_CLOSE_BTN, 123);
	GFAddButton(book_gump, 290, 140, 2103, 2104, GF_CLOSE_BTN, 124);
	GFAddButton(book_gump, 290, 155, 2103, 2104, GF_CLOSE_BTN, 125);
	GFAddButton(book_gump, 290, 170, 2103, 2104, GF_CLOSE_BTN, 126);
// End Page 1 Setup

// Below you will notice in the loop, there are entries put BACK into page
// 1. Do not worry, we run an Optimize function on the gump to put all
// of those BACK into the initial Page 1 section we just made to keep our
// gump's arrays looking nice and clean.

	var pOX := 145;
	var pOY := 60;
	var charge_button    := 101;
	var recall_button    := 201;
	var gate_button      := 301;
	var drop_rune_button := 401;
	var set_default_rune := 501;
	var rune_pos         := 1;

	for i := 2 to 9
		GFPage(book_gump, i);
		GFAddButton(book_gump, 125, 14, 2205, 2205, GF_PAGE_BTN, (i-1));
		if(i != 9)
			GFAddButton(book_gump, 393, 14, 2206, 2206, GF_PAGE_BTN, (i+1));
		endif

		var rune_entry := rune_list[rune_pos];
		if( TypeOf(rune_entry) == "Struct" )
			if( rune_entry.name )
				// Fixes any error with the text color.
				if( TypeOf(rune_entry.Color) != "Integer" || rune_entry.color < 0 )
					rune_entry.color := 24;
				endif
				// This puts the rune in page 1's list.
				GFPage(book_gump, 1);
				GFTextCrop(book_gump, pOX,  pOY, 115, 17, rune_entry.color, rune_entry.name);
				GFPage(book_gump, i);

				GFAddButton(book_gump, 130, 65, 2103, 2104, GF_CLOSE_BTN, charge_button);
				GFTextCrop(book_gump, 145, 60, 115, 17, rune_entry.color, rune_entry.name);
				GFTextLine(book_gump, 135, 80, 0, ConvertLatLong(rune_entry.x, rune_entry.y, 1));
				GFTextLine(book_gump, 135, 95, 0, ConvertLatLong(rune_entry.x, rune_entry.y, 2));

				GFAddButton(book_gump, 135, 115, 2437, 2438, GF_CLOSE_BTN, drop_rune_button);
				GFHTMLArea(book_gump, 150, 115, 100, 18, "Drop Rune");
				GFAddButton(book_gump, 160, 20, 2361, 2361, GF_CLOSE_BTN, set_default_rune);
				GFHTMLArea(book_gump, 175, 15, 100, 18, "Set Default");

				GFAddButton(book_gump, 135, 140, 2271, 2271, GF_CLOSE_BTN, recall_button);
				GFAddButton(book_gump, 205, 140, 2291, 2291, GF_CLOSE_BTN, gate_button);

				charge_button := charge_button + 1;
				drop_rune_button := drop_rune_button + 1;
				set_default_rune := set_default_rune + 1;
				recall_button := recall_button + 1;
				gate_button := gate_button + 1;
			endif
		else
			GenerateEmptySide(book_gump, 1, pOX, pOY, i);
		endif
                rune_pos := rune_pos + 1;
		pOY := pOY + 15;
		if( pOY > 165 )
			pOY := 60;
			pOX := 305;
		endif

		rune_entry := rune_list[rune_pos];
		if( TypeOf(rune_entry) == "Struct" )
			if( rune_entry.name )
				// Fixes any error with the text color.
				if( TypeOf(rune_entry.color) != "Integer" || rune_entry.color < 0 )
					rune_entry.color := 10;
				endif
				// This puts the rune in page 1's list.
				GFPage(book_gump, 1);
				GFTextCrop(book_gump, pOX,  pOY, 115, 17, rune_entry.color, rune_entry.name);
				GFPage(book_gump, i);

				GFAddButton(book_gump, 290, 65, 2103, 2104, GF_CLOSE_BTN, charge_button);
				GFTextCrop(book_gump, 305, 60, 115, 17, rune_entry.color, rune_entry.name);
				GFTextLine(book_gump, 295, 80, 0, ConvertLatLong(rune_entry.x, rune_entry.y, 1));
				GFTextLine(book_gump, 295, 95, 0, ConvertLatLong(rune_entry.x, rune_entry.y, 2));

				GFAddButton(book_gump, 295, 115, 2437, 2438, GF_CLOSE_BTN, drop_rune_button);
				GFHTMLArea(book_gump, 310, 115, 100, 18, "Drop Rune");
				GFAddButton(book_gump, 300, 20, 2361, 2361, GF_CLOSE_BTN, set_default_rune);
				GFHTMLArea(book_gump, 315, 15, 100, 18, "Set Default");

				GFAddButton(book_gump, 295, 140, 2271, 2271, GF_CLOSE_BTN, recall_button);
				GFAddButton(book_gump, 365, 140, 2291, 2291, GF_CLOSE_BTN, gate_button);

				charge_button := charge_button + 1;
				drop_rune_button := drop_rune_button + 1;
				set_default_rune := set_default_rune + 1;
				recall_button := recall_button + 1;
				gate_button := gate_button + 1;
			endif
		else
			GenerateEmptySide(book_gump, 2, pOX, pOY, i);
		endif

                rune_pos := rune_pos + 1;
		pOY := pOY + 15;
		if( pOY > 165 )
			pOY := 60;
			pOX := 305;
		endif

		SleepMS(2);
	endfor

//	GFOptimizeGump(book_gump);

	return book_gump;
endfunction


function GenerateEmptySide(byref book_gump, side, pOX, pOY, page_num)

	if( side == 1)
		// This puts the rune in page 1's list.
		GFPage(book_gump, 1);
		GFTextCrop(book_gump, pOX,  pOY, 115, 17, 10, "Empty");
		GFPage(book_gump, page_num);
		GFTextCrop(book_gump, 145, 60, 115, 17, 10, "Empty");
	else
		// This puts the rune in page 1's list.
		GFPage(book_gump, 1);
		GFTextCrop(book_gump, pOX,  pOY, 115, 17, 10, "Empty");
		GFPage(book_gump, page_num);
		GFTextCrop(book_gump, 305, 60, 115, 17, 10, "Empty");
	endif

endfunction


function SetBookName(book, who, auto:=0)
	var charge_prop := GetObjProperty(book, "%Charges%");
	var charges := charge_prop["charges"];
	if( !charges )
		charges := 0;
	endif

	var name := GetObjProperty(book, "%Name%");
	if( !auto )
		name := SendTextEntryGump( who, "Enter the new name of the runebook. ", TE_CANCEL_ENABLE, TE_STYLE_NORMAL);
		if( (!name) || (name == "") )
			name := GetObjProperty(book, "%Name%");
			if( !name )
				name := "a runebook";
			endif
		endif
	endif

	SetObjProperty(book, "%Name%", name);
	name := (name+" [charges: "+charges+"]");
	SetName(book, name);

	return 1;
endfunction


function SetCharges(book, who, charges)
	var charge_prop := GetObjProperty(book, "%Charges%");
	charge_prop["charges"] := charges;
	SetObjProperty(book, "%Charges%", charge_prop);
	SetBookName(book, who, 1);
	return 1;
endfunction


function GetRuneList(book)
	var rune_list := GetObjProperty(book, "%RuneList%");
	if( TypeOf(rune_list) != "Array" )
		rune_list := array{};
		SetObjProperty(book, "%RuneList%", rune_list);
	endif

	return rune_list;
endfunction


function GetRuneDetails(book, rune_pos)
	var rune_list := GetRuneList(book);
	if( TypeOf(rune_list) != "Array" )
		rune_list := array{};
		SetObjProperty(book, "%RuneList%", rune_list);
	endif

	return rune_list[rune_pos];
endfunction

function ConvertLatLong(x, y, pos)
	var workit := ToSextant(x, y);
	var j, newtemp := "";
	for(j:=1; j<=Len(workit); j:=j+1)
		if(workit[j] == " ")
			workit[j] := "_";
		elseif(workit[j] == ",")
			workit[j] := " ";
			workit[j+1] := "";
		endif
	endfor

	newtemp := SplitWords(workit);
	if(pos == 1)
		newtemp := newtemp[1];
		for(j:=1; j<=Len(newtemp); j:=j+1)
			if(newtemp[j] == "_")
				newtemp[j] := " ";
			endif
		endfor
		return newtemp;
	else
		newtemp := newtemp[2];
		for(j:=1; j<=Len(newtemp); j:=j+1)
			if(newtemp[j] == "_")
				newtemp[j] := " ";
			endif
		endfor
		return newtemp;
	endif
endfunction


function DeleteRune(book, rune_pos)
	var rune_list := GetRuneList(book);
	if( TypeOf(rune_list) != "Array" )
		rune_list := array{};
		SetObjProperty(book, "%RuneList%", rune_list);
	endif
	if( rune_list[rune_pos] )
		rune_list.Erase(rune_pos);
		SetObjProperty(book, "%RuneList%", rune_list);
	endif
	return 1;
endfunction


function SetDefault(book, who, rune_pos)
	var rune_info := GetRuneDetails(book, rune_pos);
	SetObjProperty(book, "Destination", rune_info);
	SendSysMessage(who, "Default rune set to: "+rune_info.name);
	return 1;
endfunction


function ToSextant(x, y)
	var latitude_direction, longitude_direction;
	var latitude_degrees, longitude_degrees;
	var latitude_minutes, longitude_minutes;
	latitude_degrees := (y-1624)*0.087890625;
	longitude_degrees := (x-1323)*0.0703125;
	if(latitude_degrees < 0)
		latitude_direction := "N";
	else
		latitude_direction := "S";
	endif
	if(longitude_degrees < 0)
		longitude_direction := "W";
	else
		longitude_direction := "E";
	endif
	latitude_degrees := Abs(latitude_degrees);
	longitude_degrees := Abs(longitude_degrees);
	latitude_minutes := CInt((CInt((latitude_degrees-Floor(latitude_degrees))*100)*60)/100);
	longitude_minutes := CInt((CInt((longitude_degrees-Floor(longitude_degrees))*100)*60)/100);
	return(Floor(latitude_degrees)+"o "+latitude_minutes+"'"+latitude_direction+", "+Floor(longitude_degrees)+"o "+longitude_minutes+"'"+longitude_direction);
endfunction