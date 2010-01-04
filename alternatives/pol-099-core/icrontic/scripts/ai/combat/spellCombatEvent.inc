include "include/attributes";

function process_combat_event(opponent)
  spellattack(opponent);
  var plevel := Cint(GetObjProperty(me, "poison_level"));
  var rint := RandomInt(8);
  if((plevel >= 1) && (rint >= 5))
    CastSpell(me, me, "cure");
  endif
  if(((GetHp(me) * 100) / GetMaxHp(me)) < flee_point)
    EraseObjProperty(me,"#flees");
    flee(opponent);
  endif
endfunction

function process_flee_event(opponent)
  spellattack(opponent);
  if((GetObjProperty(me, "poison_level") >= 1) && (RandomInt(8) >= 3))
    CastSpell(me, me, "cure");
    return 0;
  endif
  if(((GetHp(me) * 100) / GetMaxHp(me)) >= flee_point)
    return 1;
  endif
endfunction

function spellattack(opponent)
  if(LAST_BREATH > ReadGameClock())
    return;
  endif
  if((CheckLineOfSight(me, opponent)) and (dist(me, opponent) <= 15))
    cast_offensive_spell(me, opponent);
    LAST_BREATH := Cint(ReadGameClock() + (RandomInt(4) + 4));
  endif
endfunction

function prepare_for_fight(opponent)
  if((!CInt(GetObjProperty(me, "mr"))) and (RandomInt(3) = 2))
    CastSpell(me, me, "reflect");
  endif
  DisableMainEvents();
  EnableCombatEvents();
endfunction

function post_combat()
  if((GetHp(me) < (GetMaxHP(me) / 2)) && (RandomInt(4) >= 2))
    CastSpell(me, me, "gheal");
  endif
  if(CInt(GetObjProperty(me, "poison_level")) >= 1)
    CastSpell(me, me, "cure");
  endif
  DisableCombatEvents();
  EnableMainEvents();
  SetWarMode(0);
  SetOpponent(0);
  sleep(1);
  look_around();
endfunction
