//
// Sends gump for player to report players for the murder
// and adjusts their murder counts
//
// TODO: ping pong counter
//

use uo;

const LONG_COUNT_DECAY_TIME := 72000; //20 hours
const SHORT_COUNT_DECAY_TIME := 14400; //4 hours

var layout := {

"page 0",
"nodispose",
"noclose",
"nomove",
"gumppic 0 0 206",
"gumppic 43 0 201",
"gumppic 460 0 207",

"gumppic 0 43 202",
"gumppic 43 43 200",
"gumppic 170 43 200",
"gumppic 295 43 200",
"gumppic 335 43 200",
"gumppic 43 170 200",
"gumppic 170 170 200",
"gumppic 295 170 200",
"gumppic 335 170 200",
"gumppic 43 220 200",
"gumppic 170 220 200",
"gumppic 295 220 200",
"gumppic 335 220 200",
"gumppic 460 43 203",

"gumppic 0 340 204",
"gumppic 43 340 233",
"gumppic 460 340 205",

"button 380 345 249 248 1 0 99",

"text 50 20 40 0",
"text 50 50 40 1",
"text 50 70 40 2",

//"checkbox 100 100 210 211 0 10",
//"checkbox 100 200 210 211 1 11",
};

var data := {
"Now is the time for retribution!",
"Check zero or more players to report for your murder",
"and press OKAY.",
};

var reportables;

function SendReportGump(who)
  if(Checkplace(who))
    SendSysMessage(who, "You are in a no-report area");
    return;
  endif
  reportables := who.reportables;
  FillInArrays();
  var reportdata := {};
  foreach reportable in(who.reportables)
	reportdata.append(reportable.killer.serial);
  endforeach
  SetObjProperty(who, "ReportMenu", reportdata);
  var ret := SendDialogGump(who,layout,data);
  if(ret == 0)
    return;
  endif
  EraseObjProperty(who, "ReportMenu");
  EraseObjProperty(who, "LoginReport");
  var mob;
  foreach id in (ret.keys)
	if(id >= 10)
	  id := id-9;
	  mob := who.reportables[id];
	  if(!(mob.killer in EnumerateOnlineCharacters()))
	    SetObjProperty(mob.killer, "LoginNotify", who.serial);
	  else
	    AdjustNoto(mob.killer, who);
	  endif
	  AdjustMurderCounts(who, mob.killer);
	endif
  endforeach
  foreach reportable in(who.reportables)
	who.removeReportable(reportable.serial, reportable.gameclock);
  endforeach
endfunction

function FillInArrays()
  var prev_y := 80;
  var prev_x := 40;
  var buttonret := 10;
  layout.append("page 1");
  foreach reportable in reportables
    if(!reportable.killer.cmdlevel)
	  if(buttonret==21)
	    prev_x := 300;
	    prev_y := 80;
	  endif
	  layout.append("checkbox " + CStr(prev_x) + " " + CStr(prev_y+20) + " 210 211 0 " + buttonret);
	  data.append(reportable.killer.name);
	  layout.append("text " + CStr(prev_x+20) + " " + CStr(prev_y+20) + " 40 " + CStr(len(data)-1));
	  prev_y := prev_y+20;
	  buttonret := buttonret+1;
    endif
  endforeach
endfunction

function AdjustMurderCounts(who, killer)
  var onlinetime := GetObjProperty(killer,"onlinetimer");
  var longcount := GetObjProperty(killer,"longmurders");
  if(!longcount)
	longcount := 0;
  endif
  longcount := longcount + 1;
  SetObjProperty(killer,"longmurders",longcount);
  if(longcount>4)
	killer.setMurderer(1);
	SendSysMessage(killer,"You are now known as a murderer!");
  endif
  SetObjProperty(killer,"decaylongcountat",onlinetime+LONG_COUNT_DECAY_TIME);
  var shortcount := GetObjProperty(killer,"shortmurders");
  if(!shortcount)
	shortcount := 0;
  endif
  shortcount := shortcount + 1;
  SetObjProperty(killer,"shortmurders",shortcount);
  SetObjProperty(killer,"decayshortcountat",onlinetime+SHORT_COUNT_DECAY_TIME);
endfunction
/*
Function Checkplace(me)
 
 If ( me.x>=5488 and me.y>=1541 and me.x<=5664 and me.y<=1793)
  Return "Region Tourny Complex";  
 Elseif ( me.x>=5488 and me.y>=1541 and me.x<=5528 and me.y<=1641)
  Return "Region 1v1";  
 Elseif ( me.x>=5539 and me.y>=1581 and me.x<=5562 and me.y<=1592)
  Return "Region Monster Bash";      
 Elseif ( me.x>=5700 and me.y>=1653 and me.x<=5737 and me.y<=1691)
  Return "Region 10v10";
 Else
  Return 0;
 Endif 
 
Endfunction
*/
