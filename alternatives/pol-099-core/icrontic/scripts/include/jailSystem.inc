var acctnotedata:= {

	"Account Notes:",
	"Save Notes:",
	"",
	"",
	"",
	"",
	"",
	"",
	"",
	"",
	"",
	"",
	"",
	"",

};

var banlayout := array(
    "gumppic  20  60 2624            ",      //account note box
    "gumppic 285  60 2624            ",      //account note box
    "gumppic  20   5 2624            ",      //account note box
    "gumppic 285   5 2624            ",      //account note box
    "gumppic  20   0 2621            ",      //account note box
    "gumppic 285   0 2621            ",      //account note box
    "gumppic  20   0 2620            ",      //account note box
    "gumppic 535   0 2622            ",      //account note box
    "gumppic  20   5 2623            ",      //account note box
    "gumppic 535   5 2625            ",      //account note box
    "gumppic  20  45 2623            ",      //account note box
    "gumppic 535  45 2625            ",      //account note box
    "gumppic  20 250 2627            ",      //account note box
    "gumppic  20 250 2626            ",      //account note box
    "gumppic 285 250 2627            ",      //account note box
    "gumppic 537 250 2628            ",      //account note box
    "text     90  60   37      0     ",      //warning text          (red)
    "text     90  75   37      1     ",      //warning text          (red)
    "text     90  90   37      2     ",      //warning text          (red)
    "text    150 105   37      3     ",      //warning text          (red)
    "button  105 190  247  249 1 0  1",      //cancel button
    "button  405 190  242  240 1 0  2",      //confirm button
);

var bandata := array(
    "WARNING! Banning an account through this interface will",        //0
    "delete all account characters as well as destroy their bank",    //1
    "boxes! This is not reversable, use this feature with care!",     //2
    "do you still wish to ban this account?",                         //3
);

function jail(who, tojail, person);
  var val := CInt(SendTextEntryGump(who, "How many days will this account remain jailed?(Max 14)", 40));
  var time;
  case(val)
     1: time := 86400;
     2: time := 172800;
     3: time := 259200;
     4: time := 345600;
     5: time := 432000;
     6: time := 518400;
     7: time := 604800;
     8: time := 691200;
     9: time := 777600;
    10: time := 864000;
    11: time := 950400;
    12: time := 1036800;
    13: time := 1123200;
    14: time := 1209600;
  endcase
  var act := FindAccount(person.acctname);
  act.setprop("releaseday",(ReadGameClock() + time));
  var characters := EnumerateOnlineCharacters();
  act.disable();
  var acctcfg := ReadConfigFile(":gmtools:accts");
  var elemjail := { };
  elemjail.append({"releaseday", (ReadGameClock() + time)});
  elemjail.append({"reason", notes(who, person)});
  var jailed := AppendConfigFileElem(":gmtools:accts", "Account", person.acctname, elemjail);
  UnloadConfigFile(":gmtools:accts");
  foreach char in characters
    if(char.acctname == person.acctname)
      SendSysMessage(char, "Your account has beed jailed by " + who.name + ".");
      DisconnectClient(char);
      break;
    endif
  endforeach
endfunction

function editjail(who, tojail, person);
  var val := CInt(SendTextEntryGump(who, "enter new total?(Max 14)", 40));
  var time;
  case(val)
     1: time := 86400;
     2: time := 172800;
     3: time := 259200;
     4: time := 345600;
     5: time := 432000;
     6: time := 518400;
     7: time := 604800;
     8: time := 691200;
     9: time := 777600;
    10: time := 864000;
    11: time := 950400;
    12: time := 1036800;
    13: time := 1123200;
    14: time := 1209600;
  endcase
  var act := FindAccount(person.acctname);
  act.setprop("releaseday",(ReadGameClock() + time));
endfunction

function ban(who, acctname);
  var banverify := SendDialogGump(who, banlayout, bandata);
  var act := FindAccount(acctname);
  case(banverify[0])
    1: var characters := EnumerateOnlineCharacters();
       foreach char in characters
         if(char.acctname == acctname)
           SendSysMessage(char, "You were kicked by " + who.name + ".");
           SetObjProperty(char,"justbanned", 1);
           DisconnectClient(char);
           break;
         endif
       endforeach
       foreach chr in characters
         if(chr.cmdlevel > 1)
           SendSysMessage(chr, "account " + acctname + " has been banned.");
         endif
       endforeach
       Detach();
       sleep(10);
       act.ban();
	   var x, i;
       WipeBankBox(act.GetCharacter(1));
	   for(i := 1; i <= 25; i := i + 1)
	     x := GetEquipmentByLayer(act.GetCharacter(1), i);
	     MoveItemToContainer(x, act.GetCharacter(1).backpack);
	   endfor
       DestroyItem(act.GetCharacter(1).backpack);
       act.DeleteCharacter(1);
       i := 0;
       WipeBankBox(act.GetCharacter(2));
	   for(i := 1; i <= 25; i := i + 1)
	     x := GetEquipmentByLayer(act.GetCharacter(2), i);
	     MoveItemToContainer(x, act.GetCharacter(2).backpack);
	   endfor
       DestroyItem(act.GetCharacter(2).backpack);
       act.DeleteCharacter(2);
       i := 0;
       WipeBankBox(act.GetCharacter(3));
	   for(i := 1; i <= 25; i := i + 1)
	     x := GetEquipmentByLayer(act.GetCharacter(3), i);
	     MoveItemToContainer(x, act.GetCharacter(3).backpack);
	   endfor
       DestroyItem(act.GetCharacter(3).backpack);
       act.DeleteCharacter(3);
       i := 0;
       WipeBankBox(act.GetCharacter(4));
	   for(i := 1; i <= 25; i := i + 1)
	     x := GetEquipmentByLayer(act.GetCharacter(4), i);
	     MoveItemToContainer(x, act.GetCharacter(4).backpack);
	   endfor
       DestroyItem(act.GetCharacter(4).backpack);
       act.DeleteCharacter(4);
       i := 0;
       WipeBankBox(act.GetCharacter(5));
	   for(i := 1; i <= 24; i := i + 1)
	     x := GetEquipmentByLayer(act.GetCharacter(5), i);
	     MoveItemToContainer(x, act.GetCharacter(5).backpack);
	   endfor
       DestroyItem(act.GetCharacter(5).backpack);
       act.DeleteCharacter(5);
    2: SendSysMessage(who,"cancelled");
       return;
  endcase
endfunction


function notes(who, targ)

  SendSysMessage(who,"Note why this account has been jailed and your name.");

  var acc:= targ.acct;
  var note:= acc.GetProp("Notes");

  var layout:= {

	"nodispose",

	"resizepic  20 20 2620 517 260",

	"text 30 0 910 0 ",
	"text 410 0 910 1 ",

	"button 508 0 4011 4013 1 0 1",

	"textentry 30 30 500 20 910 2 2",
	"textentry 30 50 500 20 910 3 3",
	"textentry 30 70 500 20 910 4 4",
	"textentry 30 90 500 20 910 5 5",
	"textentry 30 110 500 20 910 6 6",
	"textentry 30 130 500 20 910 7 7",
	"textentry 30 150 500 20 910 8 8",
	"textentry 30 170 500 20 910 9 9",
	"textentry 30 190 500 20 910 10 10",
	"textentry 30 210 500 20 910 11 11",
	"textentry 30 230 500 20 910 12 12",
	"textentry 30 250 500 20 910 13 13",

  };

  SetNotes(note);

  var res:= SendDialogGump(who,layout,acctnotedata);

  if(!res[0])
    return;
  endif

  var subnote, i;

  note:= "";
  for(i:= 2; i <= 12; i:= i + 1)
    subnote:= res[i];
    subnote[1,find(subnote,": ",1) + 1]:= "";
    if(subnote != "")
      note:= note +" "+ subnote;
    endif
  endfor

  acc.setprop("Notes",note);
  return note;
endfunction

function SetNotes(note)
  var split:=SplitWords(note);
  var width:=0;
  var lines:=0;
  var index:=1;
  while ((index<=len(split)) && (lines<=12))
    if (width==0)
      var testword:=split[index];
      lines:=lines+1;
      acctnotedata[2+lines]:=testword;
      width:=GumpTextWidthFast(testword);
      index:=index+1;
    else
      var testword:=" "+split[index];
      var testwidth:=GumpTextWidthFast(testword);
      if (width+testwidth<CInt(500/1.5))
        acctnotedata[2+lines]:= acctnotedata[2+lines]+testword;
        width:=width+testwidth;
        index:=index+1;
      else
        width:=0;
      endif
    endif
  endwhile
  if (lines>12)
    acctnotedata[1+lines]:= acctnotedata[1+lines]+"...";
  else
    while(lines<12)
      lines:=lines+1;
      acctnotedata[2+lines]:="";
    endwhile
  endif
endfunction
