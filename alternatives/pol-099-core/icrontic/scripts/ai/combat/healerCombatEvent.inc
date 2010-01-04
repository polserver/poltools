include "include/attributes";

function process_combat_event(opponent)
  spellattack(opponent);
  if ( GetHp(me) < (GetStrength(me)/2))
    CastSpell(me,me,"gheal");
    if(CInt(GetObjProperty(me, "poison_level")) >1 )
      CastSpell(me,me,"cure");
    endif
  endif
  if ((GetHp(me) * 100/GetMaxHp(me)) < flee_point)
    EraseObjProperty(me,"#flees");
    flee(opponent);
  endif
endfunction

function spellattack(opponent)
  if (!GetObjProperty(me,"#lastbreath"))
    SetObjProperty(me,"#lastbreath",ReadGameClock() + 5);
  endif
  if (GetObjProperty(me,"#lastbreath") > ReadGameClock() )
    return;
  endif
  if (RandomInt(2)==1)
    sleep(1);
    cast_offensive_spell(me,opponent);
  endif
  SetObjProperty(me,"#lastbreath",ReadGameClock() + 5);
  foreach myfriend in ListMobilesNearLocation(me.x, me.y, me.z, HALT_THRESHOLD)
    if (myfriend.npctemplate && myfriend.script != "tamed" )
      healerstuff(myfriend);
    endif
  endforeach
endfunction

function in_combat_event_loop(opponent, loops)
  spellattack(opponent);
  if ( loops > 50 )
    flee(opponent);
    return;
  endif
endfunction

function prepare_for_fight(opponent)
  if (GetObjProperty(me, "mr") != "1")
    CastSpell(me,me,"reflect");
  endif
  DisableMainEvents();
  EnableCombatEvents();
endfunction

function post_combat()
  DisableCombatEvents();
  EnableMainEvents();
  SetWarMode( 0 );
  SetOpponent( 0 );
  num_casts := 2;
  sleep(1);
  look_around();
endfunction

function healerstuff(mobile)
  if (!mobile.npctemplate || mobile.npctemplate["guard"] || mobile.script == "tamed")
    return;
  endif
  var poison_level := CInt(GetObjProperty(mobile, "poison_level"));
  if( poison_level > 0 && CheckLineOfSight(me,mobile) )
    TurnToward(mobile);
    CastSpell(me,mobile,"cure");
  elseif( (GetHp(mobile) < (GetMaxHp(mobile)/2)) && (CheckLineOfSight(me,mobile))  )
    TurnToward(mobile);
    CastSpell(me,mobile,"gheal");
  endif
endfunction