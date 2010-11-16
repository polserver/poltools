// $Id: functions.inc 1048 2007-04-23 00:56:15Z tekproxy $

CONST MEDITATION_SFX := 0xFA;

function GetManaLoss(mobile)
  var item_cfg := ReadConfigFile(":*:itemdesc");
  var loss := 0;
  var total := 0;

  foreach item in ListEquippedItems(mobile)
    loss := item_cfg[item.objtype]."MedLoss";
    
    if ( Lower(loss) == "nomed" )
      return "nomed";
    else
      total += CInt(loss);
    endif
  endforeach

  return total;
endfunction

function StartMeditating(mobile)
	SendSysMessage(mobile, "You enter a meditative trance.");
	PlaySoundEffect(mobile, MEDITATION_SFX);
	return SetObjProperty(mobile, "#IsMeditating", 1);
endfunction

function StopMeditating(mobile, noMessage:=0)
	if ( !noMessage )
		SendSysMessage(mobile, "You stop meditating");
	endif
	return EraseObjProperty(mobile, "#IsMeditating");
endfunction

function CheckIfMeditating(mobile)
	return GetObjProperty(mobile, "#IsMeditating");
endfunction