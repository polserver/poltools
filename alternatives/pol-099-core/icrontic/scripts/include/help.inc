var layout2:= {

	"nodispose",

	"resizepic 100 50 2620 600 500",
	"resizepic 100 50 5054 600 500",

	"gumppic 180 250 510", // Book

	"text 225 165 1100 0",
	"text 225 190 1100 1",

	"button 190 162 4023 4024 1 0 1",
	"button 190 187 4023 4024 1 0 2",

	"text 303 227 1173 2",

	"text 208 253 46 3",
	"text 208 271 46 4",
	"text 208 289 46 5",
	"text 208 307 46 6",
	"text 208 325 46 7",
        "text 208 343 46 8",
	"text 208 361 46 9",
	"text 208 379 46 10",
	"text 208 397 46 11",
	"text 208 415 46 12",
	"text 208 433 46 13",
	"text 208 451 46 14",
	"text 400 253 46 15",
	"text 400 271 46 16",
	"text 400 289 46 17",
	"text 400 307 46 18",
	"text 400 325 46 19",
	"text 400 343 46 20",
	"text 400 361 46 21",

	"text 240 510 1173 22",

	"text 295 70 1100 23",
	"text 235 95 1100 24",
};

var data2:= {

	"Request Counselour Help",
	"Request GameMaster Help",

	"Page System Guildlines",
	"1: No replacing items.",
	"2: Please do not interupt",
        "   staff answering other",
        "   pages. Page to speak",
	"   with them.",
	"3: If your question is",
	"   about Gameplay or",
        "   skills, read our FAQ.",
	"4: Be sure to read our",
	"   FAQ and Information",
	"   pages on the website.",
	"5: We do not help place",
	"   houses.",
	"6: When paging, state",
	"   your problem. Staff",
        "   will not answer pages",
        "   like 'Please help'.",
        "7: Please use Forums for",
        "   Role Play questions.",
        
	"",

	"Icrontic Staff Paging Menu",
	"Be sure to read the guidelines before paging.",
};

function pagesystem(who2)
  var who := who2[1];
  var box := SendDialogGump( who, layout2, data2 );
  case (box[0])

	1: mainpagecoun(who);
	2: mainpagegm(who);
        3: OpenWebSite(who, "http://www.icrontic.net");

  endcase
endfunction

function mainpagecoun(who)
  staffcheck();
  if(!onlinecouncy("councy"))
    SendSysMessage(who,"Sorry, there are no counselors online now, try again later.", 3, 33);
    return;
  endif
  var thepage:= CStr(RequestInput(who,who.backpack,"Write down your question:" ));
  if(thepage = "0")
        SendSysMessage(who, "Cancelled");
        return;
  endif
  counpage(who, thepage);
  EraseObjProperty( who, "canpage" );
  SendSysMessage( who, "The counselors have received your message and will soon reply.", 3, 73);
  SetObjProperty( who, "HELP", thepage);
  SetObjProperty (who, "HELPTIMER", ReadGameClock());
  pagewarning(2, thepage, who);
endfunction

function mainpagegm(who)
  staffcheck();
  if(!onlinecouncy("gm"))
        SendSysMessage( who, "Sorry, there are no GM's online now, try again later.", 3, 33);
        return;
  endif
  var thepage := CStr(RequestInput( who, who.backpack, "Write down your problem:" ));
  if(thepage=="0")
        SendSysMessage(who, "Cancelled");
        return;
  endif
  gmpage(who, thepage);
  EraseObjProperty( who, "canpage" );
  SendSysMessage( who, "GM's have received your message and will soon reply.", 3, 73);
  SetObjProperty( who, "HELP", thepage);
  SetObjProperty (who, "HELPTIMER", ReadGameClock());
  pagewarning(1, thepage, who);
endfunction

var layout3:= {

	"nodispose",

	"resizepic 100 50 2620 600 500",
	"resizepic 100 50 5054 600 500",

	"",

	"text 180 165 1100 0",
	"text 180 190 1100 1",

	"button 150 162 4023 4024 1 0 1",
	"button 150 187 4023 4024 1 0 2",

	"text 285 227 1100 2",

	"text 141 253 1173 3",
	"text 141 271 1173 4",
	"text 141 289 1173 5",
	"text 141 307 1173 6",
	"text 141 325 1173 7",
	"text 141 343 1173 8",
	"text 141 361 1173 9",
	"text 141 379 1173 10",
	"text 141 397 1173 11",
	"text 141 415 1173 12",
	"text 141 433 1173 13",
	"text 141 451 1173 14",
	"text 141 469 1173 15",
	"text 141 487 1173 16",

	"text 260 510 1100 17",

};

var data3:= {

	"General FAQ Webpage (Extensive command list)",
	"General Information Webpage",

	"List of common ingame .commands",
	".statlock - Used for locking/lowering your stats",
	".skill -  Used for locking/lowering your skills",
	".online - Displays total online",
	".vote - Menu with links to vote & downloads",
	".quip - Used to equip your quip list",
	".unquip - Used to unequip to set quip list",
	".arm - Same as quip, except only for weapons",
	".disarm - Same as unquip cept for weapons",
	".age - Tells your current online age",
	".auc - Auction System",
	".barter - Auction system chat",
	".guard - Used to call town guards when needed",
	".msg - Online msging system",
	".reply - Reply to the last person who messaged you",

	"Our website is Http://www.icrontic.net",

};

function dotcommands(who)

  var box := SendDialogGump( who, layout3, data3 );
  case (box[0])

	1: OpenWebSite(who, "http://www.icrontic.net/modules.php?name==FAQ");
	2: OpenWebSite(who, "http://www.icrontic.net/modules.php?name==Content");

  endcase
endfunction

function Icanpage(thechar)
  foreach person in EnumerateOnlineCharacters()
    if ((GetObjProperty(person,"canpage")) && (person.serial != thechar.serial))
      return 0;
    endif
  endforeach
  SetObjProperty(thechar, "canpage", 1);
  return 1;
endfunction

function staffcheck()
  var staff := 0;
  var counselor := 0;
  foreach char in EnumerateOnlineCharacters()
    if (char.cmdlevel >= 3)
      staff := staff + 1;
    elseif (char.cmdlevel = 2)
      counselor := counselor + 1;
    elseif (char.cmdlevel = 1)
      counselor := counselor + 1;
    endif
  endforeach
  counselor := CInt(counselor);
  staff := CInt(staff);
  SetGlobalProperty( "OnlineCounselors", CInt(counselor) );
  SetGlobalProperty( "OnlineStaff", CInt(staff) );
endfunction

function pagecheck(serial, type)
  var cserial := CInt(serial);
  var retval := 0;
  var i;
  case(CInt(type))
    1: var maxi := gmpages.size();
	   for(i := 0; i < maxi; i := i + 1)
		 if (cserial == CInt(gmpages[i]))
		   retval := i;
		   break;
		 endif
	   endfor
    2: var maxi := counpages.size();
	   for(i := 0; i < maxi; i := i + 1)
		 if (cserial == CInt(counpages[i]))
		   retval := i;
		   break;
		 endif
	   endfor
  endcase
  return retval;
endfunction

function pagewarning(type, thepage, who)
  case(type)
    1: foreach char in EnumerateOnlineCharacters()
		 if (char.cmdlevel >= 3)
		   SendSysMessage( char, "Staff Page from " + who.name + ": " + thepage, 3, 73);
		 endif
	   endforeach
    2: foreach char in EnumerateOnlineCharacters()
		 if (char.cmdlevel >= 1)
		   SendSysMessage( char, "Councy Page from " + who.name +  ": " + thepage, 3, 73);
		 endif
	   endforeach
    3: foreach char in EnumerateOnlineCharacters()
		 if (char.cmdlevel >= 1)
		   SendSysMessage( char, "Bug Report from " + who.name +  ": " + thepage, 3, 73);
		 endif
	   endforeach
  endcase
endfunction

function onlinecouncy(type)
var onlinecounselors;
var onlinestaff;
if(type == "councy")
  onlinecounselors := GetGlobalProperty("OnlineCounselors");
  return onlinecounselors;
elseif(type == "gm")
  onlinestaff := GetGlobalProperty("OnlineStaff");
  return onlinestaff;
endif
endfunction

var layout4:= {

	"nodispose",

	"resizepic 100 50 2620 600 500",
	"resizepic 100 50 5054 600 500",

	"gumppic 440 290 104",
	"gumppic 475 50 50990",

	"text 182 142 1100 0",
	"text 182 167 1100 1",

	"button 150 142 4023 4024 1 0 1",
	"button 150 167 4023 4024 1 0 2",

	"text 131 235 1173 2",
	"text 131 253 1173 3",
	"text 131 271 1173 4",
	"text 131 289 1173 5",
	"text 131 307 1173 6",
	"text 131 325 1173 7",
	"text 131 343 1173 8",
	"text 131 361 1173 9",
	"text 131 379 1173 10",
	"text 131 397 1173 11",
	"text 131 415 1173 12",
	"text 131 433 1173 13",
	"text 131 451 1173 14",

	"text 182 469 1100 15",
	"button 150 469 4023 4024 1 0 3",

	"text 257 70 1173 16",

	"text 140 510 1100 17",

};

var data4:= {

	"Become a Volunteer",
	"Request Volunteer Help",

	"Page System Guildlines",
	"",
	"1. These pages are answered by volunteers who",
	"    are players just like you, and cannot perform",
	"    any abilities you cannot.",
	"2. Volunteers give thier time to help others, please",
	"    do not harass them or abuse thier offer of help",
	"3. Volunteers are here for general skill and game",
	"    questions. They do not take bug reports.",
	"4. Volunteers cannot handle ingame reports of a rule",
	"    being violated. Page a staff for reporting.",
	"",
	"",
	"Resign from Volunteer status",
	"Icrontic: The Volunteer Help System",

	"Beware friend, for thou dost live in dangerous times",

};

function volsystem(who2)
  var who := who2[1];
  var box := SendDialogGump( who, layout4, data4 );
  case (box[0])

	1: becomeone(who);
	2: pagevol(who);
        3: stopvol(who);

  endcase
endfunction

function becomeone(who)
  SetObjProperty(who, "#volunteer", 1);
  SendSysMessage(who, "Welcome to our volunteer system! use .volpage to view pages.");
endfunction

function stopvol(who)
  EraseObjProperty(who, "#volunteer");
  SendSysMessage(who, "Your help will be missed. We hope you return to aid more!");
endfunction

function pagevol(who)
  var thepage:= CStr(RequestInput(who,who.backpack,"Write down your question:" ));
  if(thepage = "0")
        SendSysMessage(who, "Cancelled");
        return;
  endif
  volpage(who, thepage);
  EraseObjProperty( who, "canpage" );
  SendSysMessage( who, "Volunteers have recieved your page. Please be patient", 3, 73);
  foreach char in EnumerateOnlineCharacters()
          if(GetObjProperty(char, "#volunteer") = 1)
             SendSysMessage( char, "Page from " + who.name +  ": " + thepage, 3, 73);
          endif
  endforeach
endfunction

function volpage(who, thepage)
  if(!volpages)
    volpages := {};
  endif
  while(1)
    if (!Icanpage(who))
      sleep(1);
    else
      break;
    endif
  endwhile
  var cserial := CInt(who.serial);
  var newarry := {};
  newarry.append(who.name);
  newarry.append(thepage);
  newarry.append(who.serial);
  volpages.append(newarry);
  SetGlobalProperty("volpages", volpages);
endfunction
