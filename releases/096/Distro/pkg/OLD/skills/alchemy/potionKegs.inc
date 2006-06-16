
function rename_keg(keg, potion)
  var itemdesc := ReadConfigFile(":alchemy:itemdesc");
  var obj := FindConfigElem(itemdesc, potion);
  var amt  := 0;
  foreach thing in EnumerateItemsInContainer(keg)
    if(thing.objtype == 0xf0e)
      amt := amt + thing.amount;
    endif
  endforeach
  if(amt > 0)
    var str := "a keg of " + obj.desc + "s. " + GetFullness(keg);
    SetObjProperty(keg, "PotionName", obj.desc);
    SetName(keg, str);
  else
    var str := "specially lined keg for holding potions. " + GetFullness(keg);
    EraseObjProperty(keg, "PotionName");
    EraseObjProperty(keg, "PotionType");
    EraseObjProperty(keg, "OwnerSerial");
    EraseObjProperty(keg, "houseserial");
    SetName(keg, str);
  endif
  return;
endfunction


function GetFullness( pkeg )
  var amt := 0;
  foreach thing in EnumerateItemsInContainer(pkeg)
    if(thing.objtype == 0xf0e)
      amt := amt + thing.amount;
    endif
  endforeach
  var str;
  if (amt == 0)
    str := "The keg is empty.";
  elseif ((amt >= 1) && (amt <= 4))
    str := "The keg is nearly empty.";
  elseif ((amt >= 5) && (amt <= 19))
    str := "The keg is not very full.";
  elseif ((amt >= 20) && (amt <= 29))
    str := "The keg is about one quarter full.";
  elseif ((amt >= 30) && (amt <= 39))
    str := "The keg is about one third full.";
  elseif ((amt >= 40) && (amt <= 46))
    str := "The keg is almost half full.";
  elseif ((amt >= 47) && (amt <= 53))
    str := "The keg is approximately half full.";
  elseif ((amt >= 54) && (amt <= 69))
    str := "The keg is more than half full.";
  elseif ((amt >= 70) && (amt <= 79))
    str := "The keg is about three quarters full.";
  elseif ((amt >= 80) && (amt <= 95))
    str := "The keg is very full.";
  elseif ((amt >= 96) && (amt <= 99))
    str := "The liquid is almost to the top of the keg.";
  else
    str := "The keg is completely full.";
  endif
  return str;
endfunction

function is_potion(obj)
  if((obj.objtype >= 0xdc01) && (obj.objtype <= 0xdce6))
    return 1;
  endif
  return 0;
endfunction

