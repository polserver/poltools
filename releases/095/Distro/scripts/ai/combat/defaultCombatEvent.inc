include "include/attributes";
function process_combat_event(opponent)
  if((GetHp(me) * 100/GetMaxHp(me)) < flee_point)
	EraseObjProperty(me,"#flees");
    flee(opponent);
  endif
endfunction

function process_flee_event(opponent)
  return 0;
endfunction

function in_combat_event_loop(opponent, loops)
  if(loops > 50)
    if((dist(me, opponent) > 12) && (!CheckLineOfSight(me, opponent)))
	  RestartScript(me);
	  return;
    endif
  endif
endfunction

function post_combat()
  DisableCombatEvents();
  EnableMainEvents();
  SetWarMode(0);
  SetOpponent(0);
  sleep(1);
  look_around();
endfunction

function prepare_for_fight(opponent)
  DisableMainEvents();
  EnableCombatEvents();
endfunction