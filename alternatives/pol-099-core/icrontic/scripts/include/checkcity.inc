use os;
use uo;

Function CheckCity(character, item)
  var who := character;
  var cfgfile := ReadConfigFile("::gzone");
  var entries := GetConfigStringKeys(cfgfile);
  var element;
  var range;
  var guardzone := 0;
  foreach listing in entries
    element :=  cfgfile[listing];
    range := element.range;
    range := SplitWords(range);
//    SendSysMessage(who, range);
    if((who.x >= CInt(range[1])) && (who.x <= CInt(range[3])) && (who.y >= CInt(range[2])) && (who.y <= CInt(range[4])))
//      SendSysMessage(who,"you are in a guard zone.");
      guardzone := 1;
      break;
    endif
  endforeach
  foreach listing in entries
    element :=  cfgfile[listing];
    range := element.range;
    range := SplitWords(range);
    if((item.x >= CInt(range[1])) && (item.x <= CInt(range[3])) && (item.y >= CInt(range[2])) && (item.y <= CInt(range[4])))
//      SendSysMessage(who,"you are in a guard zone.");
      guardzone := 1;
      break;
    endif
  endforeach
  return guardzone;
EndFunction
