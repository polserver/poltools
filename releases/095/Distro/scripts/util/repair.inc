

include "include/attributes";
function repair_item(character, what, skillid )
  var maxhp;
  maxhp := what.maxhp;
  maxhp := cint(maxhp * what.quality);
  var thehp := what.hp;
  var damaged := maxhp - thehp;
  var hisskill := GetEffectiveSkill(character, skillid);
  var damageamount := 0.0;
  damageamount := (125 -  hisskill)/3000.0;
  if(what.hp == what.maxhp)
    SendSysMessage(character, "That item is not damaged.");
    return; 
  endif
  if((what.hp <= 0) || (what.quality <= 0.1) || (!what.quality))
    SendSysMessage(character, "The brittle material breaks when you handle it");
    DestroyItem(what);
    return;
  endif
  if(what.quality <= damageamount)
    SendSysMessage(character, "The brittle material breaks when you handle it");
    DestroyItem(what);
    return;
  endif
  what.quality := what.quality - damageamount;
  if((what.hp <= 0) || (what.quality <= 0.1) || (!what.quality))
    SendSysMessage(character, "The brittle material breaks when you handle it");
    DestroyItem(what);
    return;
  endif
  if (CheckSkill(character, skillid, cint(damaged / 3), 0))
    var repaired_amount := cint( ((randomint(hisskill) + 1))/2);
    if (damaged == 1)
      repaired_amount := 1;
    endif
    if (thehp + repaired_amount > what.maxhp)
      what.hp := what.maxhp;
      SendSysMessage(character, "You  repair the item completely");       
    else
      what.hp := thehp + repaired_amount;
      SendSysMessage(character, "You successfully repair some of the damage");
    endif
    if ((what.hp <= 0) || (what.quality <= 0.1) || (!what.quality))
      SendSysMessage(character, "The brittle material breaks when you handle it");
      DestroyItem(what);
      print("Destroyed item when " + character.name + " repaired it");
      return;
    endif     
  else
    var damage_amount := randomint(10);
    if (thehp <= damage_amount)
      DestroyItem(what);
      SendSysMessage(character, "You fail and destroy the item");
    endif
    thehp := thehp - damage_amount;
    what.hp := thehp;
    if(randomint(2) == 1)
      what.quality := what.quality - (damaged/100);
    endif
    if(thehp <= 0)
      DestroyItem(what);
      SendSysMessage(character, "You fail and destroy the item");
    else
      SendSysMessage(character, "You fail and damage the item");
    endif
    if ((what.hp <= 0) || (what.quality <= 0.1) || (!what.quality))
      SendSysMessage(character, "The brittle material breaks when you handle it");
      DestroyItem(what);
      return;
    endif     
  endif
endfunction