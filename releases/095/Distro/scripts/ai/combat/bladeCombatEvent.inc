function in_combat_event_loop(opponent, loops)
  if(loops > 50)
	RestartScript(me);
  endif
endfunction

function post_combat()
  DisableCombatEvents();
  EnableMainEvents();
  SetWarMode( 0 );
  SetOpponent( 0 );
  sleep(1);
  look_around();
endfunction

function prepare_for_fight(opponent)
  DisableMainEvents();
  EnableCombatEvents();
endfunction