
function process_wool(who, blade, sheep)
  if((!CheckLineOfSight(who, sheep)) or (Distance(who, sheep) > 2))
    SendSysMessage(who, "You cannot use that.");
    return;
  endif
  if(sheep.graphic == 207)
    sheep.graphic := 223;
    CreateItemInContainer(who.backpack, 0xf125, 3);
  else
    SendSysMessage(who,"That sheep is not yet ready to be shorn.");
  endif
endfunction
