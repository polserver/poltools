// //$Id: craftingSkill.inc 128 2006-05-27 17:36:42Z austinheilman $

// This is a modified version of the ShilCheckSkill export. The purpose
// of this function is to mimic the OSI % chance system crafting uses.
function CraftCheckSkill(who, craft_skill, difficulty, points, report:=0)

  difficulty := CDbl(difficulty);
  points := CDbl(points);
  if(difficulty == error)
    return 0;
  elseif(difficulty == 0.0)
    return 1;
  endif
  var chance;
  var GainCap := 1200;
  // I'm Lazy. Change this later.
  var effective := CDbl(AP_GetSkill(who, craft_skill));
  var hunger  := CInt(GetObjProperty(who, "hunger"));
  if(!hunger || hunger.errortext)
    hunger := 2;
  endif
  case(hunger)
     0: effective := (effective - 10.1); points := (points * 0.95);
     1: effective := (effective + 20.1); points := (points * 1.05);
     5:
     6: effective := (effective - 15.1); points := (points * 0.95);
     7:
     8: effective := (effective - 20.1); points := (points * 0.97);
     9:
    10: effective := (effective - 40.1); points := (points * 0.95);
  endcase
  if(effective < 0)
    effective := 0.0;
  endif
  chance := (effective - difficulty) * 2.0 * 10.0;
  var GainCheck := chance;
  if(chance < 0)
    chance := 0;
  elseif(chance > 1000)
    chance := 1000;
  endif
  if(report)
    return CDbl(chance/10.0);
  endif
  var temp_chance;
  temp_chance := (effective - difficulty) * 2.5 + 500;
  if(temp_chance < 0)
    temp_chance := 0;
  elseif(temp_chance > 1000)
    temp_chance := 1000;
  endif
  if(points != 0)
    points  := points * ((1000-temp_chance) / 1000);
  endif
  if(GainCheck >= GainCap)
    points := 0;
  endif
  if(RandomInt(1000) < chance)
    if(points > 0)
//      AwardPoints(who, craft_skill, points);
    endif
    return 1;
  else
    if(effective < 10)
      if(points > 0)
//        AwardPoints(who, craft_skill, points);
      endif
    endif
    return 0;
  endif
endfunction

// This is a modified version of the CraftCheckSkill. The purpose
// of this function is to mimic the OSI % chance system exceptional.
// This system is, .2% chance per .1 skill above the required skill (+30)
// to craft the item as exceptional. Aidskill has been added to help
// add a custom method for POL on this. Aidskills would be either
// arms lore (as in with blacksmithy), or otherwise it is the
// Secondary skill required to craft the item. See the config files
// In the crafting pkgs for their layouts and the defining of them.
function ExceptionalCraft(skill, reqd, aidskill:=0, report:=0);

  if(!aidskill or aidskill.errortext)
    aidskill := skill;
  endif
  skill := CDbl(skill);
  reqd := CDbl(reqd) + 30.0;
  var excmod := skill + (aidskill/3.3333333333333333333333333333333);
  var chance := (excmod - reqd) * 2.0 * 10.0;
  if(chance < 0)
    chance := 0;
  elseif(chance > 1000)
    chance := 1000;
  endif
  if(report)
    return CDbl(chance/10.0);
  endif
  if(RandomInt(1000) < chance)
    return 1;
  else
    return 0;
  endif

endfunction