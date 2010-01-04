use os;
use cfgfile;
use uo;

const EVENT_ADD_POISON              := 1;
const EVENT_CURE_POISON             := 2;
const EVENT_CURE_SPECIFIC_POISON    := 3;
const EVENT_DEAD                    := 4;
const EVENT_TERMINATE_SPECIFIC      := 5;
const EVENT_RESET_DURATION          := 6;

var cfgfile := ReadConfigFile(":poisonwatcher:poisons");

function ListPoisonsByType(mobile, the_type)
  var plist := array;
  var templist := GetObjProperty(mobile, "Poisons");
  foreach poisonname in (templist.keys())
    if (lower(cfginfo(poisonname).Type) == lower(the_type))
      plist.append(poisonname);
    endif
  endforeach
  return plist;
endfunction

function GetAllPoisons(mobile)
  var plist := array;
  var templist := GetObjProperty(mobile, "poisons");
  var temp := templist.keys();
  foreach poisonname in (templist.keys())
    plist.append(poisonname);
  endforeach
  return plist;
endfunction

function AddPoison(mobile, poisonname, amount, attacker)
  var event := struct;
  event.+type       := EVENT_ADD_POISON;
  event.+name       := poisonname;
  event.+amount     := amount;
  event.+attacker   := attacker;
  event.+pid        := GetPid();
  while(1)
    GetScript(mobile).SendEvent(event);
    var ev := wait_for_event(5);
    if(ev.type = "OK")
      break;
    endif
  endwhile
endfunction

function TerminateSpecific(mobile, poisonname)
  var event := struct;
  event.+ type;
  event.+ name;
  event.+ amount;
  event.type := EVENT_TERMINATE_SPECIFIC;
  event.name := poisonname;
  GetScript(mobile).SendEvent(event);
endfunction

function CureSpecific(mobile, poisonname, amount)
  var event := struct;
  event.+ type;
  event.+ name;
  event.+ amount;
  event.type := EVENT_CURE_SPECIFIC_POISON;
  event.name := poisonname;
  event.amount := amount;
  GetScript(mobile).SendEvent(event);
endfunction

function Cure_OSI_Poison(mobile)
  var poison := Find_OSI_Poisons(mobile);
  CureSpecific(mobile, poison, -1);
endfunction

function Find_OSI_Poisons(mobile)
  var plist := 0;
  var templist := GetObjProperty(mobile, "Poisons");
  foreach poisonname in (templist.keys())
    if(cfginfo(poisonname).Type == "O")
      plist := poisonname;
      break;
    endif
  endforeach
  return plist;
endfunction

function GetScript(mobile)
  var PoisonPid := cint(GetObjProperty(mobile, "#PoisonPID"));
  var PoisonWatcher := GetProcess(PoisonPid);
  if(!PoisonWatcher)
    PoisonWatcher := start_script(":poisonwatcher:poisonwatcher", {mobile});
    if (PoisonWatcher.errortext)
      PrintTextAbove(mobile, "Poison Watcher GetScript() " + PoisonWatcher.errortext);
    endif
  endif
  return PoisonWatcher;
endfunction

function ResetDuration(mobile, poisonname, amount)
  var event := struct;
  event.+ type;
  event.+ name;
  event.+ amount;
  event.type := EVENT_RESET_DURATION;
  event.name := poisonname;
  event.amount := amount;
  GetScript(mobile).SendEvent(event);
endfunction

function cfginfo(poisonname)
  return cfgfile[poisonname];
endfunction

///////////////////
//  if we're currently poisoned, figures out how poisoned.
///////////////////
function GetPoisonLevel (byref who)
	var poisoned := GetObjProperty (who, "poison_level");
	if (!poisoned)
		return 0;
	endif

	return (poisoned [1]);
endfunction

///////////////////
//  Cures the poison
///////////////////

function CurePoison (who, sound := 0, msg := "")
	EraseObjProperty (who, "poison_level");
	who.poisoned := 0;
	if (sound)
		PlaySoundEffect (who, sound);
	endif
	if (msg)
		PrintTextAbovePrivate (who, msg, who);
	endif
endfunction
