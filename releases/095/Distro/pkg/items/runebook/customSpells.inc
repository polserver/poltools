use uo;
use os;

include "include/client";
include "include/attributes";
include "include/objtype";
include "include/spellRestrictions";

function custom_recall(caster, parms, book)
  book := book; // stops ecompile "not used" warning
  caster.hidden := 0;
  if(GetMana(caster) < 11)
    caster.frozen :=1;
	PerformAction(caster,ANIM_CAST_DIR);
    sleepms(1200);
    caster.frozen :=0;
    SendSysMessage(caster,"you do not have enough mana to cast that");
    PlaySoundEffect(caster,0x005d);
    PlayObjectCenteredEffect(caster, 0x3735,0x0a, 0x1e);
    return 0;
  endif
  var failure := 0;
  var circle := 4;
  var tox := parms[2];
  var toy := parms[3];
  var toz := parms[4];
  SpeakPowerWords( caster, 32 );
  var delivery := "notarget";
  var noto := "neutral";
  var result := maincast(caster, delivery, circle, noto, 32);
  if(result[1] == 0)
    return;
  endif
  var multicheck := CreateItemAtLocation(tox, toy, toz, 0xeed, 1);
  multicheck.hidden := 1;
  if(multicheck.multi.serial)
    SendSysMessage(caster, "Something is interfering with this spell.");
    DestroyItem(multicheck);
    return;
  endif
  DestroyItem(multicheck);
  var signs := ListItemsNearLocationOfType( tox, toy, toz, 30, 0x7060);
  foreach item in signs
      var infoarr := GetObjProperty( item, "homeinfo" );
    if (infoarr)
      if ((tox >= infoarr[2]) && (tox <= infoarr[4]) && (toy >= infoarr[3]) && (toy <= infoarr[5]))
        failure := 1;
      endif
    endif
  endforeach
  if (failure == 0);
    MoveCharacterToLocation(caster, tox, toy, toz);
  else
    SendSysMessage(caster, "Something is interfering with this spell.");
    return 0;
  endif
  PlaySoundEffect(caster, 0x1fd);
  return 1;
endfunction

function custom_gate(caster, parms, book)
  book := book; // stops ecompile "not used" warning
  caster.hidden := 0;
  if(GetMana(caster) < 40)
    caster.frozen :=1;
	PerformAction(caster,ANIM_CAST_DIR);
    sleepms(1200);
    caster.frozen :=0;
    SendSysMessage(caster,"you do not have enough mana to cast that");
    PlaySoundEffect(caster,0x005d);
    PlayObjectCenteredEffect(caster, 0x3735,0x0a, 0x1e);
    return 0;
  endif
  var circle := 7;
  var tox := parms[2];
  var toy := parms[3];
  var toz := parms[4];
  SpeakPowerWords( caster, 52 );
  var delivery := "notarget";
  var noto := "neutral";
  var result := maincast(caster, delivery, circle, noto, 52);
  if(result == 0)
    return;
  endif
  var multicheck := CreateItemAtLocation(tox, toy, toz, 0xeed, 1);
  multicheck.hidden := 1;
  if(multicheck.multi.serial)
    DestroyItem(multicheck);
    return;
  endif
  DestroyItem(multicheck);
  var signs := ListItemsNearLocationOfType( tox, toy, toz, 30, 0x7060);
  var chk := 0;
  foreach item in signs
    var infoarr := GetObjProperty(item, "homeinfo");
    if(infoarr)
      if((tox >= infoarr[2]) && (tox <= infoarr[4]) && (toy >= infoarr[3]) && (toy <= infoarr[5]))
        chk := 1;
        break;
      endif
    endif
  endforeach
  if(chk == 1)
    return;
  endif
  set_critical( 1 );
  var wx := caster.x;
  var wy := caster.y;
  var wz := caster.z;
  PlayStationaryEffect( wx, wy, wz, 6899, 1, 30, explode := 0 );
  PlayStationaryEffect( tox, toy, toz, 6899, 1, 30, explode := 0 );
  sleepms(2800);
  set_critical( 1 );
  var gate1 := CreateItemAtLocation( wx, wy, wz, UOBJ_BLUE_MOONGATE, 1 );
  gate1.movable := 0;
  var gate2 := CreateItemAtLocation( tox, toy, toz, UOBJ_BLUE_MOONGATE, 1 );
  gate2.movable := 0;
  if(!gate1)
    DestroyItem(gate2);
    SendSysMessage(caster, "something is interfering with the spell");
    return;
  endif
  if (!gate2)
    DestroyItem(gate1);
    SendSysMessage(caster, "something is interfering with the spell");
    return;
  endif
  SetObjProperty( gate1, "GateDestX", gate2.x );
  SetObjProperty( gate1, "GateDestY", gate2.y );
  SetObjProperty( gate1, "GateDestZ", gate2.z );
  SetObjProperty( gate2, "GateDestX", gate1.x );
  SetObjProperty( gate2, "GateDestY", gate1.y );
  SetObjProperty( gate2, "GateDestZ", gate1.z );
  Detach();
  set_critical( 0 );
  PlaySoundEffect( caster, SFX_SPELL_GATE_TRAVEL );
  detach();
  sleep(30);
  set_critical(1);
  DestroyItem( gate1 );
  DestroyItem( gate2 );
  set_critical(0);
  return(1);
endfunction