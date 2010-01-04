var cfg := ReadConfigFile("alchemy");

include "include/attributes";
function is_reagent(what)
  case(what.objtype)
    0xf7a: return 1;
    0xf7b: return 1;
    0xf84: return 1;
    0xf85: return 1;
    0xf86: return 1;
    0xf88: return 1;
    0xf8c: return 1;
    0xf8d: return 1;
  endcase
  return 0;
endfunction

function PourPotion(user,mortar)
  try_to_make_potion(user, "spacefill", 1, mortar, 0);
endfunction

function make_the_potion(user, mortar, use_on)
  if(!can_access(user, use_on))
    return;
  endif
  if(!ReserveItem(use_on))
    return;
  endif
  if(is_reagent(use_on))
    var cfg := ReadConfigFile("alchemy");
    if(!cfg)
      return;
    endif
    var elem := cfg[use_on.objtype];
    if(!cfg)
      return;
    endif
    var possible := array;
    var tmenu := CreateMenu("Select a potion to make");
    var num_possibilities := 0;
    var selections := array;
    possible := GetConfigStringArray( elem, "potion");
    foreach thingie in possible
      if(can_make(user, thingie, use_on))
        var titem := cfg[thingie].objtype;
        if(titem)
          thingie["_"] := " ";
          AddMenuItem( tmenu, titem, thingie );
          num_possibilities := num_possibilities + 1;
        endif
      endif
    endforeach
    if(num_possibilities == 0)
      SendSysMessage(user, "You can't make any potions!");
      return;
    endif
    var citem := SelectMenuItem2(user, tmenu);
    if(!citem.objtype)
      SendSysMessage(user, "Targetting canceled");
      return -1;
    endif
    if (GetObjProperty(user,"autoAlchemy"))
     var px := user.x, py := user.y;
     while ((px = user.x) && (py = user.y))
	    	if (!try_to_make_potion(user, possible[citem.index], 0, mortar, use_on))
         return;
      endif
     if(!GetObjProperty(user, "#Mixing"))
       return;
     endif
   Sleep(1);
  endwhile
 else
		try_to_make_potion(user, possible[citem.index], 0, mortar, use_on);
 endif
  elseif(use_on == mortar)
    var last := GetObjProperty(mortar, "LastMade");
    if(last)
  	  try_to_make_last(user, last, mortar);
  	else
  	  SendSysMessage(user, "You cannot grind that.");
  	  return;
  	endif
  else
    SendSysMessage(user, "That's not a reagent!");
  endif
endfunction

function can_make(user, potion, reg)
  var skillneeded:= CInt(cfg[potion].skill);
  if(skillneeded > 20)
    skillneeded := skillneeded - 20;
  else
    skillneeded := 0;
  endif
  if(GetEffectiveSkill(user, SKILLID_ALCHEMY) >= skillneeded)
    return 1;
  else
    return 0;
  endif
endfunction

function take_regs(user, potion, reg)
  var amount_needed:= cfg[potion].reagent_amount;
  set_critical(1);
  if(CInt(amount_needed) <= reg.amount)
    SubtractAmount(reg, amount_needed);
    set_critical(0);
    return 1;
  else
    SendSysMessage(user, "You ran out of reagents!");
    set_critical(0);
    return 0;
  endif
endfunction

function try_to_make_potion(user, potion, in_mortar, mortar, reg)
  if(!in_mortar)
    potion[" "] := "_";
    var difficulty := cfg[potion].skill;
    var points := cfg[potion].points;
    var product := cfg[potion].objtype;
    var cntr;
    SetObjProperty(mortar, "LastMade", potion);
    var loop := 0;
    while(loop < 3)
      PlaySoundEffect(user, SOUND_EFFECT_GRIND);
      PrintTextAbovePrivate(user, "You grind the reagent",user);
      sleep(1);
      loop := loop + 1;
    endwhile
    if(!take_regs(user, potion, reg))
      return;
    endif
    if(CheckSkill(user, SKILLID_ALCHEMY, difficulty, points))
      var hasbottle:= 0;
      set_critical(1);
      foreach item in EnumerateItemsInContainer(user.backpack)
        if((item.objtype == UOBJECT_EMPTY_BOTTLE) && (item.container.objtype != 0x1940))
          if(SubtractAmount(item, 1))
            hasbottle := 1;
            break;
          endif
        endif
      endforeach
      set_critical(0);
      if(hasbottle)
        var thing := CreateItemInBackpack(user, product, 1);
        PlaySoundEffect(user, SOUND_EFFECT_POUR);
        PrintTextAbovePrivate(user, "You create the potion and put it in your backpack", user);
        return 1;
      else
        PrintTextAbovePrivate(user, "You dont have any bottles.", user);
        SetObjProperty(mortar, "contents", product);
        return 0;
      endif
    else
      PlaySoundEffect(user, SOUND_EFFECT_POUR);
      PrintTextAbovePrivate(user, "You pour out the useless mixture", user);
      return 1;
    endif
  else
    var hasbottle:= 0;
    set_critical(1);
    foreach item in EnumerateItemsInContainer(user.backpack)
      if(item.objtype == UOBJECT_EMPTY_BOTTLE)
        SubtractAmount(item, 1);
        hasbottle := 1;
        break;
      endif
    endforeach
    set_critical(0);
    if(hasbottle)
      var product := GetObjProperty(mortar, "contents");
      var thing := CreateItemInBackpack(user, product, 1);
      PlaySoundEffect(user, SOUND_EFFECT_POUR);
      PrintTextAbovePrivate(user, "You create the item and put it in your backpack", user);
      EraseObjProperty( mortar, "contents" );
      return 1;
    else
      PrintTextAbovePrivate(user, "You dont have any bottles.", user);
      return 0;
    endif
  endif
endfunction

function try_to_make_last(user, potion, mortar)
  var reg;
  case(potion)
    "Refresh":            reg := FindRegs(user, 0xf7a);
    "Total_Refresh":      reg := FindRegs(user, 0xf7a);
    "Agility":            reg := FindRegs(user, 0xf7b);
    "Greater_Agility":    reg := FindRegs(user, 0xf7b);
    "Ultra_Agility":      reg := FindRegs(user, 0xf7b);
    "Lesser_Cure":        reg := FindRegs(user, 0xf84);
    "Cure":               reg := FindRegs(user, 0xf84);
    "Greater_Cure":       reg := FindRegs(user, 0xf84);
    "Lesser_Heal":        reg := FindRegs(user, 0xf85);
    "Heal":               reg := FindRegs(user, 0xf85);
    "Greater_Heal":       reg := FindRegs(user, 0xf85);
    "Ultra_Heal":         reg := FindRegs(user, 0xf85);
    "Strength":           reg := FindRegs(user, 0xf86);
    "Greater_Strength":   reg := FindRegs(user, 0xf86);
    "Ultra_Strength":     reg := FindRegs(user, 0xf86);
    "Lesser_Poison":      reg := FindRegs(user, 0xf88);
    "Poison":             reg := FindRegs(user, 0xf88);
    "Greater_Poison":     reg := FindRegs(user, 0xf88);
    "Deadly_Poison":      reg := FindRegs(user, 0xf88);
    "Lesser_Explosion":   reg := FindRegs(user, 0xf8c);
    "Explosion":          reg := FindRegs(user, 0xf8c);
    "Greater_Explosion":  reg := FindRegs(user, 0xf8c);
    "Nightsight":         reg := FindRegs(user, 0xf8d);
  endcase
  if(reg == "none")
    SendSysMessage(user, "You do not have enough reagents to make that.");
    return;
  endif
  potion[" "] := "_";
  var difficulty := cfg[potion].skill;
  var points := cfg[potion].points;
  var product := cfg[potion].objtype;
  var cntr;
    if(!take_regs(user, potion, reg))
    SendSysMessage(user, "You do not have enough reagents to make that.");
    return;
  endif
  PlaySoundEffect(user, SOUND_EFFECT_GRIND);
  PrintTextAbovePrivate(user, "You grind the reagent",user);
  sleep(2);
  PlaySoundEffect(user, SOUND_EFFECT_GRIND);
  PrintTextAbovePrivate(user, "You grind the reagent",user);
  sleep(2);
  PlaySoundEffect(user, SOUND_EFFECT_GRIND);
  PrintTextAbovePrivate(user, "You grind the reagent",user);
  sleep(2);
  if(CheckSkill(user, SKILLID_ALCHEMY, difficulty, points))
    var hasbottle:= 0;
    set_critical(1);
    foreach item in EnumerateItemsInContainer(user.backpack)
      if(item.objtype == UOBJECT_EMPTY_BOTTLE)
        if(SubtractAmount(item, 1))
          hasbottle := 1;
          break;
        endif
      endif
    endforeach
    set_critical(0);
    if(hasbottle)
      var thing := CreateItemInBackpack(user, product, 1);
      PlaySoundEffect(user, SOUND_EFFECT_POUR);
      SetObjProperty(mortar, "LastMade", potion);
      PrintTextAbovePrivate(user, "You create the potion and put it in your backpack", user);

      return;
    else
      PrintTextAbovePrivate(user, "You dont have any bottles.", user);
      SetObjProperty(mortar, "contents", product);
      return;
    endif
  else
    PlaySoundEffect(user, SOUND_EFFECT_POUR);
    PrintTextAbovePrivate(user, "You pour out the useless mixture", user);
    return;
  endif
endfunction

function FindRegs(who, type)
  var holder := "none";
  foreach thing in EnumerateItemsInContainer(who.backpack)
    if(thing.objtype == type)
      holder := thing;
      break;
    endif
  endforeach
  return holder;
endfunction
