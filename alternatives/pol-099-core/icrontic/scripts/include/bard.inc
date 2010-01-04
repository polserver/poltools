use uo;

include "include/client";
include "include/attributes";

function play(character, theskill, item, skil)
  var success, fail;
  case(item.graphic)
    0x0eb1:  success := 0x44;  fail := 0x45;
    0x0eb2:  success := 0x46;  fail := 0x47;
    0x0eb3:  success := 0x4d;  fail := 0x4e;
    0x0eb4:  success := 0x4d;  fail := 0x4e;
    0x0e9c:  success := 0x39;  fail := 0x3a;
    0x0e9d:  success := 0x53;  fail := 0x54;
    0x0e9e:  success := 0x53;  fail := 0x54;
  endcase
  var thepoints := GetEffectiveSkill(character, SKILLID_MUSICIANSHIP);
  if(CheckSkill(character, SKILLID_MUSICIANSHIP, theskill, thepoints))
    if(CheckSkill(character, skil, theskill, thepoints * 2))
      PlaySoundEffect( character, success);
      return 1;
    else
      PlaySoundEffect( character, fail);
      return 0;
    endif
  else
    PlaySoundEffect( character, fail);
    return 0;
  endif
endfunction

function findinstrument(character)
  var instruments := {0x0eb1, 0x0eb2, 0x0eb3, 0x0eb4, 0x0e9c, 0x0e9d, 0x0e9e};
  foreach item in EnumerateItemsInContainer(character.backpack)
    if(item.graphic in instruments)
      return item;
    endif
  endforeach
endfunction

///////////// Added By MuadDib 

function can_sing( me, circle)

var cfg := readconfigfile(":combat:itemdesc");
var item;

foreach item in ListEquippedItems(me)
      if ((cfg[item.objtype].blockcircle) && (cfg[item.objtype].blockcircle <= circle)) 
                SendSysmessage(me, "Your equipment prevents you from using this instrument!");
                return 0;
      endif
endforeach

return 1;

endfunction

////////////////////// Added by MuadDib

function music(character, instrument, diff)

   var inst_id := instrument.graphic;
   var ret := 0;
 
   if (inst_id = UOBJ_HARP)
      ret := play(character,SFX_HARP_SUCC,SFX_HARP_FAIL, diff);
   elseif (inst_id = UOBJ_LUTE)
      ret := play(character,SFX_LUTE_SUCC,SFX_LUTE_FAIL, diff);
   elseif (inst_id = UOBJ_DRUM)
      ret := play(character,SFX_DRUM_SUCC,SFX_DRUM_FAIL, diff);
   elseif (inst_id = UOBJ_TAMB)
      ret := play(character,SFX_TAMB_SUCC,SFX_TAMB_FAIL, diff);
   else
      PrintTextAbovePrivate( character, "That's not something you can play", character );
   endif

   return ret;
endfunction
