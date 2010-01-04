use uo;
use os;
use util;

include "include/client";
include "util/bank";

var cfg := ReadConfigFile("::npcdesc");
var movecfg := ReadConfigFile(":*:itemdesc");

function GuardKilled(me, corpse)
  var killer := GetObjProperty(me, "LastHit");
  var guard := cfg[killer.npctemplate].script;
  if(GetObjProperty(me, "guardkilled"))
    var victim := me;
/// Moved to up here to perform only once, otherwise in the "foreach" section it performs it for
/// EVERY item, which just uses up sysload when not needed
    var bank := FindBankBox(victim);
    if (!bank)
      print("Can't find bankbox of the victim" + victim.name);
      return;
    endif
    foreach item in EnumerateItemsInContainer(corpse)
      var elm          := FindConfigElem(movecfg, item.objtype);
      var insertscript := elm.OnInsertScript;
      if(insertscript == "::onInsert")
        MoveItemToContainer(item, bank);
      endif
      MoveItemToContainer(item, bank);
    endforeach
    foreach item2 in EnumerateItemsInContainer(victim.backpack)
      MoveItemToContainer(item2, bank);
    endforeach
    var loc:=RandomInt(11);
    case(loc)
	0:  MoveCharacterToLocation(victim, 286, 775, 20, MOVECHAR_FORCELOCATION);
	1:  MoveCharacterToLocation(victim, 280, 775, 20, MOVECHAR_FORCELOCATION);
	2:  MoveCharacterToLocation(victim, 274, 775, 20, MOVECHAR_FORCELOCATION);
	3:  MoveCharacterToLocation(victim, 268, 775, 20, MOVECHAR_FORCELOCATION);
	4:  MoveCharacterToLocation(victim, 262, 775, 20, MOVECHAR_FORCELOCATION);
	5:  MoveCharacterToLocation(victim, 261, 775, 20, MOVECHAR_FORCELOCATION);
	6:  MoveCharacterToLocation(victim, 267, 776, 20, MOVECHAR_FORCELOCATION);
	7:  MoveCharacterToLocation(victim, 273, 776, 20, MOVECHAR_FORCELOCATION);
	8:  MoveCharacterToLocation(victim, 279, 776, 20, MOVECHAR_FORCELOCATION);
	9:  MoveCharacterToLocation(victim, 285, 776, 20, MOVECHAR_FORCELOCATION);
	10: MoveCharacterToLocation(victim, 279, 776, 20, MOVECHAR_FORCELOCATION);
 
    endcase
    SendSysMessage(victim, "You have gone unconcious, you awaken in a stranger place.", 3, 40);
    return;
  endif
  return;
endfunction
