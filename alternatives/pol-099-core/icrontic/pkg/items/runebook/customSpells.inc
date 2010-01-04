use uo;
use os;

include "include/client";
include "include/attributes";
include "include/objtype";
include "include/spellRestrictionsRunebooks";

function custom_recall(caster, parms, book, scroll)
  if(caster.dead == 1)
  PrintTextAbovePrivate(caster, "I am dead and cannot do that", caster);
  return;
  endif
        var oldx := caster.x;
	var oldy := caster.y;
	var oldz := caster.z;
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
  var circle := 2;
  var tox := parms[2];
  var toy := parms[3];
  var toz := parms[4];
// Island in bux
        if((tox = 2699) && (toy = 2079))
                destroyitem(book);
                syslog("Runebook Destroyed, Bugged Run to Bux: " + caster.name);
        endif
  SpeakPowerWords( caster, 32 );
  var delivery := "notarget";
  var noto := "neutral";
  var result;
  if(scroll == 1)
    var params := {};
    params[1] := "#scroll";
    params[2] := caster;
    result := maincast(params, delivery, circle, noto, 32);
  else
    result := maincast(caster, delivery, circle, noto, 32);
  endif
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
  if(!can_cast(caster, 4))
    failure := 1;
  endif
  if (failure == 0);
    If(Checkplace(caster))
      SendSysMessage(caster, "You have tried to recall from an illegal area, staff notified!");
      return;
    endif
    MoveCharacterToLocation(caster, tox, toy, toz);
    if(Checkplace(caster))
      SendSysMessage(caster, "You have tried to recall into an illegal area, staff notified!");
      MoveCharacterToLocation( caster, 5307, 1181, 0 );
      SetObjProperty(caster, "#illegalrecallrb", tox + " " + toy + " " + tox);
      return;
    endif
  else
    SendSysMessage(caster, "Something is interfering with this spell.");
    return 0;
  endif
  PlaySoundEffect(caster, 0x1fd);
  SendSysMessage(caster, "And away we go!");
  return 1;
endfunction

function custom_gate(caster, parms, book)
  if(caster.dead == 1)
  PrintTextAbovePrivate(caster, "I am dead and cannot do that", caster);
  return;
  endif
  var wx := caster.x;
  var wy := caster.y;
  var wz := caster.z;
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
// Island in bux
        if((tox = 2699) && (toy = 2079))
                destroyitem(book);
                syslog("Runebook Destroyed, Bugged Run to Bux: " + caster.name);
        endif
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
  if(!can_cast(caster, 6))
    return;
  endif
  set_critical( 1 );
  var gate1 := CreateItemAtLocation(caster.x, caster.y, caster.z, UOBJ_BLUE_MOONGATE, 1);
  gate1.movable := 0;
  var gate2 := CreateItemAtLocation(tox, toy, toz, UOBJ_BLUE_MOONGATE, 1);
  gate2.movable := 0;
    If(Checkplace(caster))
      SendSysMessage(caster, "You have tried to gate from an illegal area, staff notified!");
      return;
    endif
  if(Checkplace(gate1))
         DestroyItem(gate1);
         DestroyItem(gate2);
         MoveCharacterToLocation( caster, wx, wy, wz );
         SendSysMessage(caster, "You cannot use gates here...Staff Notified!");
         SetObjProperty(caster, "#illegalgaterb", tox + " " + toy + " " + tox);
         return;
  endif
  if(Checkplace(gate2))
         DestroyItem(gate1);
         DestroyItem(gate2);
         MoveCharacterToLocation( caster, wx, wy, wz );
         return;
  endif
  if (!gate1 || !gate2)
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
  sleep(30);
  set_critical(1);
  DestroyItem( gate1 );
  DestroyItem( gate2 );
  set_critical(0);
  return(1);
endfunction

Function Checkplace(me)

 If ( me.x>=5376 and me.y>=1176 and me.x<=5414 and me.y<=1224)
  Return "Region Maze Tourney Entrance";
 Elseif ( me.x>=5720 and me.y>=858 and me.x<=5770 and me.y<=900)
  Return "Region kingofthering";
 Elseif ( me.x>=5379 and me.y>=1279 and me.x<=5629 and me.y<=1510)
  Return "Region Maze Tourny Complex";
 Elseif ( me.x>=5377 and me.y>=1081 and me.x<=5406 and me.y<=1101)
  Return "Region Tourny Complex";
 Elseif ( me.x>=5271 and me.y>=1159 and me.x<=5312 and me.y<=1192)
  Return "Region Jail";
 Else
  Return 0;
 Endif

Endfunction
