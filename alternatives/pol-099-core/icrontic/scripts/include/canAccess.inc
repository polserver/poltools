
function can_access(who, item)
  if(GetObjProperty(item, "Owner"))
    if(GetObjProperty(item, "Owner") != who.serial)
      SendSysMessage(who, "You cannot use that.");
      return 0;
    endif
  endif
  if(GetObjProperty(item, "Vendored"))
    SendSysMessage(who, "You cannot use that.");
    return 0;
  endif
  return 1;
endfunction