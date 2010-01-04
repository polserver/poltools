function HandleMods(who, item)

  set_critical(1);

  var itemdesc := ReadConfigFile("itemdesc");
  var penalty  := CInt(itemdesc[item.objtype].DexPenalty);
  var restrict := Cint(itemdesc[item.objtype].GenderSpecific);
  var rangera  := GetObjProperty(item, "ranger");
  var archerskill := GetEffectiveSkill(who, SKILLID_ARCHERY);
  if((rangera == 1) && (archerskill < 100))
    SendSysMessage(who, "Only GrandMaster Archers understand this complex piece of equipment.");
    return 0;
  endif
  if((restrict == 1) and (who.graphic == 400))
    SendSysMessage(who, "Only females may wear that.");
    return 0;
  endif
  if(penalty)
    SetObjProperty(item, "#equipped", who.serial);
    SetDexterityMod(who, GetDexterityMod(who) - penalty);
    if(GetObjProperty(who, "ReactiveArmor"))
      EraseObjProperty(who, "ReactiveArmor");
      EraseObjProperty(who, "ar");
      SendSysMessage(who, "Your armor breaks the ability to reflect damage");
    endif
  endif
  figureManaLoss(who, item);
  start_script(":meditation:medCheck", who);
  var type := GetObjProperty(item, "type");
  if(!type)
    return 1;
  endif
  var val := 0;
  if(!who.ip)
    return 1;
  endif
  case(type)
    "accuracy":     val := accuracy(who,item);
    "cursed":       val := cursed(who,item);
    "sight":        val := sight(who,item);
    "reflect":      val := reflect(who,item);
    "protection":   val := protection(who,item);
    "wizzohat":     val := wizzohat(who,item);
    "strength":     val := strength(who,item);
    "invisibility": val := invisibility(who,item);
    default:        val := 1;
  endcase
  return val;
  set_critical(0);
endfunction

function strength(who,item)
  var holder := {};
  var gain := GetObjProperty(item, "gain");
  if(!gain)
    gain := 5;
    SetObjProperty(item, "gain", gain);
  endif
  SetObjProperty(item, "#equipped", who.serial);
  var parms := {who, item};
  start_script("chargeUpkeep", parms);
  return 1;
endfunction

function wizzohat(who,item)
  SetIntelligenceMod(who, GetIntelligenceMod(who) + 10);
  SetStrengthMod(who, GetStrengthMod(who) - 5);
  SetDexterityMod(who, GetDexterityMod(who) - 5);
  SetObjProperty(item, "#equipped", who.serial);
  return 1;
endfunction

function accuracy(who,item)
  var amt := CInt(GetObjProperty(item,"gain"));
  if((!amt) || (amt == 0))
    amt := 5;
  endif
  var cfg := ReadConfigFile(":combat:itemdesc");
  var attrib := cfg[item.objtype].Attribute;
  case(attrib)
    "Wrestling": ModifyAttributeTemporaryMod(who, ATTRIBUTEID_WRESTLING,     amt);
    "Fencing":   ModifyAttributeTemporaryMod(who, ATTRIBUTEID_FENCING,       amt);
    "Archery":   ModifyAttributeTemporaryMod(who, ATTRIBUTEID_ARCHERY,       amt);
    "Swords":    ModifyAttributeTemporaryMod(who, ATTRIBUTEID_SWORDSMANSHIP, amt);
    "Mace":      ModifyAttributeTemporaryMod(who, ATTRIBUTEID_MACEFIGHTING,  amt);
  endcase
  SetObjProperty(item, "#equipped", who.serial);
  return 1;
endfunction

function cursed(who,item)
  PlaySoundEffect(who, 0x246);
  if(!GetObjProperty(item,"named"))
    item.name := item.name + " [Cursed]";
    SetObjProperty(item,"named", 1);
  endif
  PrintTextAbovePrivate(who,"you have been cursed!", who);
  SetObjProperty(item, "#equipped", who.serial);
  return 1;
endfunction

function invisibility(who,item)
  var gain := GetObjProperty(item, "gain");
  if(!gain)
    gain := 5;
  endif
  SetObjProperty(item, "#equipped", who.serial);
  var parms := {who, item};
  start_script("chargeUpkeep", parms);
  return 1;
endfunction

function sight(who,item)
  var gain := GetObjProperty(item, "gain");
  if(!gain)
    gain := 5;
  endif
  SetObjProperty(item, "#equipped", who.serial);
  var parms := {who, item};
  start_script("chargeUpkeep", parms);
  return 1;
endfunction

function protection(who,item)
  var gain := GetObjProperty(item, "gain");
  if(!gain)
    gain := 5;
  endif
  SetObjProperty(item, "#equipped", who.serial);
  var parms := {who, item};
  start_script("chargeUpkeep", parms);
  return 1;
endfunction

function reflect(who,item)
  var gain := GetObjProperty(item, "gain");
  if(!gain)
    gain := 5;
  endif
  SetObjProperty(item, "#equipped", who.serial);
  var parms := {who, item};
  start_script("chargeUpkeep", parms);
  return 1;
endfunction

function figureManaLoss(who, item)
  var combatcfg := ReadConfigFile(":combat:itemdesc");
  var loss := 0;
  var newloss := 0;
  foreach thing in ListEquippedItems(who)
    newloss := combatcfg[thing.objtype].medloss;
    if(!newloss or newloss.errortext)
      newloss := 0;
    endif
    if(newloss == "nomed")
      SetObjProperty(who, "#ManaLoss", "nomed");
      loss := "nomed";
      break;
    else
      loss := loss + Cint(newloss);
    endif
  endforeach
  if(loss != "nomed")
    loss := loss + combatcfg[item.objtype].medloss;
    SetObjProperty(who, "#ManaLoss", CInt(loss));
  endif
  return;
endfunction
