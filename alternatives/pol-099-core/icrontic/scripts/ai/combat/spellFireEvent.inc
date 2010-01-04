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
if(opponent.serial == me.serial)
  var kname, kserial;
  var lastHit := array;
  lastHit := GetObjProperty(me, "LastHit");
  if(!lastHit)
      opponent := 0;
      return;
  endif
  kname := lastHit[1];
  kserial := lastHit[2];
  if(kname)
    opponent := SystemFindObjectBySerial(kserial);
  endif
endif
    if(RandomInt(4) == 1)
      PerformAction(me,0x0c);
      PlaySoundEffect(me, 0x16b);
      sleep(2);
      var base := Cint(((FLAME_STRENGTH * GetHp(me)) / GetMaxHp(me)) / 2);
      var dmg :=  RandomInt(base) + base;
      PlayMovingEffect(me, opponent, 0x36d4, 10, 1, 1);
      ApplyRawDamage(opponent, dmg);
    else
	  cast_offensive_spell(me,opponent);
    endif
    LAST_BREATH := Cint(ReadGameClock() + (RandomInt(4) + 4));
  endif
endfunction

function in_combat_event_loop(opponent, loops)
  spellattack(opponent);
  if ( loops > 50 )
	flee(opponent);
	return;
  endif
endfunction

function prepare_for_fight(opponent)
  if (!CInt(GetObjProperty(me, "mr")))
	CastSpell(me,me,"reflect");
  endif
  DisableMainEvents();
  EnableCombatEvents();
endfunction

function post_combat()
  if((GetHp(me) < (GetMaxHP(me) / 2)) && (RandomInt(4) >= 2))
    CastSpell(me, me, "gheal");
  endif
  DisableCombatEvents();
  EnableMainEvents();
  SetWarMode( 0 );
  SetOpponent( 0 );
  summons := 1;
  sleep(1);
  look_around();
endfunction
