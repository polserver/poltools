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
      SetObjProperty(tool,"swings", (swings - 1));
      if(tool.isa(POLCLASS_WEAPON))
      	tool.hp := tool.hp - 1;
      endif	
    endif
  endif

endfunction  