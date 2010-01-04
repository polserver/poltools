include "include/attributes";

const POISON_SOUND := 0x247;
const DRINK_SOUND := 0x32;
const UOBJ_EMPTY_BOTTLE := 0x0f0e;
const EFFECT_BLUE_SPARKLE  := 0x373a;
const SOUND_EFFECT_CURE	:= 0x1e1;
const EFFECT_SPARKLE  := 0x374a;
const HIT_ACTION := 0x148b;
const MOVING_EFFECT_PURPLE_POTION  := 0x0f0d;
const SOUND_EFFECT_EXPLOSION	:= 0x208;
const SOUND_EFFECT_HEAL	:= 0x1f3;
const EFFECT_EXPLOSION := 0x36b0;

var itemdefs := ReadConfigFile("::itemdesc");

function checkifcandrink(character, type)
  if(type != "#red")
    if(GetObjProperty(character, type))
      if(GetObjProperty(character, type) > ReadGameClock())
        SendSysMessage(character, "You must wait until you can drink another potion!");
        return 0;
      else
        SetObjProperty(character, type, ReadGameClock() + 8);
        return 1;
      endif
    else
      SetObjProperty(character, type, ReadGameClock() + 8);
       return 1;
    endif
  else
    return 1;
  endif
endfunction

function empty_bottle(character, potion)
  PlaySoundEffect(character, DRINK_SOUND);
  CreateItemInBackpack(character, UOBJ_EMPTY_BOTTLE, 1);
  DestroyItem(potion);
endfunction

function get_potion_strength(potion)
  var potion_strength := itemdefs[potion.objtype].strength;
  return potion_strength;
endfunction

function is_poison(potion)
  var poison_level := GetObjProperty(potion, "poison_level");
  return poison_level;
endfunction

function do_poison(character, potion)
  var potion_strength := get_potion_strength(potion);
print(potion_strength);
  PrintTextAbovePrivate(character, "You have been poisoned!", character);
  var poison_level :=CInt(GetObjProperty(character, "poison_level"));
  if(poison_level > potion_strength)
    empty_bottle(character, potion);
    return;
  else
    SetObjProperty(character, "poison_level", potion_strength);
    var passparms := {character, character, "poison potion"};
    Detach();
    SetObjProperty(character,"PoisonStamp", ReadGameClock());
    start_script( ":spells:poisonDamage", passparms);
  endif
  empty_bottle(character, potion);
endfunction
