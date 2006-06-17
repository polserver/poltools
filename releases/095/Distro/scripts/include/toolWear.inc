function CheckToolWear (who, tool, skillid)
  var swings := GetObjProperty(tool,"swings");
  if(!swings)
    var mods := GetObjProperty(tool, "durability");
    var bonus := 0;
    case(mods)
      "durable":        bonus := 10;
      "substantial":    bonus := 20;
      "massive":        bonus := 30;
      "fortified":      bonus := 40;
      "indestructable": bonus := 50;
    endcase
    swings := Cint(50 * tool.quality) + bonus;
    SetObjProperty(tool,"swings", swings);
  endif
  if(swings <= 0)
    DestroyItem(tool);
    SendSysMessage(who,"your tool broke.");
  else
    var skill := GetEffectiveSkill(who, skillid);
    if(skill > 80)
      skill := 80;
    elseif(skill < 20)
      skill := 20;
    endif
    if((RandomInt(100)+1) > skill)
      if(tool.isa(POLCLASS_WEAPON))
        if(tool.hp > 1)
          SetObjProperty(tool,"swings", (swings - 1));
          tool.hp := tool.hp - 1;
        elseif((tool.hp > tool.maxhp) or (tool.hp == 1))
          SendSysMessage(who,"your tool broke.");
          DestroyItem(tool);
        endif
      else
        SetObjProperty(tool,"swings", (swings - 1));
      endif	
    endif
  endif
endfunction  