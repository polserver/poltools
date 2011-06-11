/* 
 * Purpose
 * TODO
 *
 * Function List
 * GFCloseGump()
 * GFCloseGumpTimed()
 * GFExtractData()
 * GFOptimizeGump()
 * GFTextCoords()
 * GFTextMid()
 * GFTextRight()
 * GFTextWidth()
 * GFWordWrap()
 *
 */
use uo;
use os;
use polsys;
use cfgfile;

include ":gumps:gumps";

// CONSTants for GFTextCoords()
CONST ALIGN_CENTER	:= 0x1;
CONST ALIGN_RIGHT	:= 0x2;

function GFCloseGump(mobile, pid, ret_val:=0)
 
        return CloseGump( mobile, pid, ret_val );
endfunction

function GFCloseGumpTimed(mobile, pid, timer, ret_val:=0)
	return Start_Script(":gumps:autoClose", array{mobile, pid, timer, ret_val});
endfunction

function GFGetCfgConst(elem_name, prop_name)
	var cfg := ReadConfigFile(":gumps:GumpInfo");
	if ( cfg.errortext )
		return XGFError("GFGetCfgConst():: Unable to open GumpInfo.cfg -> "+cfg.errortext);
	endif

	var elem := cfg[elem_name];
	if ( elem.errortext )
		return XGFError("GFGetCfgConst():: Unable to find GumpInfo.cfg["+elem_name+"] ->"+elem.errortext);
	endif

	var value := GetConfigInt(elem, prop_name);
	if ( !value )
		return XGFError("GFGetCfgConst():: Unable to find property ["+prop_name+"] in elem ["+elem_name+"]");
	endif

	return value;
endfunction

function GFExtractData(input, data_id)
	var key_string := CStr(data_id)+": ";
	var data_string := input[data_id];
	var ret_val := data_string[Len(key_string)+1,Len(data_string)];

	return ret_val;
endfunction

function GFOptimizeGump(byref gump)
	//Arrays cant do [0] for page 0
	var cur_page := 0;
	var gump_pages := dictionary;
	gump_pages[0] := array;

	//Break the pages apart.
	foreach entry in (gump.layout)
		var l_entry := Lower(entry);
		if ( l_entry[1, 5] == "page " )
			//  12345   6
			// "Page " "#"
			var page_num := CInt(l_entry[6, Len(l_entry)]);
			if ( !gump_pages.Exists(page_num) )
				gump_pages[page_num] := array;
				cur_page := page_num;
			else
				cur_page := page_num;
				continue;
			endif
		endif
		gump_pages[cur_page].Append(entry);
		SleepMS(2);
	endforeach

	//Put the gump back together again.
	gump.layout := array{}; // Wipe it out
	foreach page_array in (gump_pages)
		 // Goes through the dictionary which contains each page's array.
		foreach entry in (page_array)
			// Goes through the array containing layout data for that page.
			(gump.layout).Append(entry);
			SleepMS(2);
		endforeach
		SleepMS(2);
	endforeach

	return 1;
endfunction

function GFTextCoords(type:=ALIGN_CENTER, gump_text, x_pos, width:=0)
	x_pos := CInt(x_pos);
	width := CInt(width);
	if ( type == ALIGN_CENTER )
		return CInt(x_pos+CInt((width-GFTextWidth(gump_text))/2));
	elseif ( type == ALIGN_RIGHT )
		return CInt(x_pos-GFTextWidth(gump_text));
	endif
endfunction

function GFTextMid(byref gump, x, y, width, color, text)
	var x_pos := GFTextCoords(ALIGN_CENTER, text, x, CStr(width));
	return GFTextLine(gump, x_pos, y, color, CStr(text));
endfunction

function GFTextRight(byref gump, x, y, color, text)
	var x_pos := GFTextCoords(ALIGN_RIGHT, CStr(text), x);
	return GFTextLine(gump, x_pos, y, color, CStr(text));
endfunction

function GFTextWidth(gump_text)
	var gft_length := struct;
	gft_length.+charsf :=
	"||||||||||||||"+	//  2
	"`',.;:::::::::"+	//  3
	"!!!!!!!!!!!!!!"+	//  4
	"il1[]]]]]]]]]\""+	//  5
	"bcejotî(){}==="+	//  6
	"adfpqrszIÑ~~~~"+	//  7
	"ghnuvyÅ<>-----"+	//  8
	"xJ578$?++++++ "+	//  9
	"kCE2369^/*****"+	// 10
	"mwABDFOPéô40%%"+	// 11
	"GLNQSZ@#######"+	// 12
	"HRT&__________"+	// 13
	"KöUYYYYYYYYYYY"+	// 14
	"XXXXXXXXXXXXXX"+	// 15
	"VVVVVVVVVVVVVV"+	// 16
	"MMMMMMMMMMMMMM"+	// 18
	"WWWWWWWWWWWWWW";	// 21
	gft_length.+width := array
	{
		2, 3, 4, 5, 6, 7, 8, 9, 10, 11,
		12, 13, 14, 15, 16, 18, 21
	};

	var points:=0;
	var j := Len(gump_text);

	for ( j; j>0; j-=1 )
		var i := Find(gft_length.charsf, gump_text[j], 1) - 1;
		if ( i>=0 )
			points += gft_length.width[CInt(i/14)+1];
		else
			print("XGFTextWidth: Unknown Character = '"+gump_text[j]+"'");
		endif
	endfor

	return (points);
endfunction

function GFWordWrap(text, pixels)
	var wrap_lines := array{};
	var word_list := SplitWords(text);
	//var num_words := word_list.Size();

	var space_size := GFTextWidth(" ");

	var cur_line := "";
	var cur_length := 0;
	var start_new := 0;

	foreach word in ( word_list )
		var word_width := GFTextWidth(word);

		if ( word == "[P]" )
			// [P] will force a line break
			word := "";
			word_width := 0;
			start_new := 1;
		elseif ( (word_width+cur_length) > pixels )
			start_new := 1;
			if ( cur_line[Len(cur_line)] == " " )
				cur_line[Len(cur_line)] := "";
			endif
		endif

		if ( start_new )
			start_new := 0;
			wrap_lines.Append(cur_line);
			cur_line := "";
			cur_length := 0;
		endif
		if ( word )
			cur_line += word+" ";
			cur_length += word_width+space_size;
		endif

		SleepMS(2);
	endforeach

	//Make sure nothing got left behind.
	if ( wrap_lines[wrap_lines.Size()] != cur_line )
		if ( cur_line[Len(cur_line)] == " " )
			cur_line[Len(cur_line)] := "";
		endif
		wrap_lines.Append(cur_line);
	endif

	return wrap_lines;
endfunction
