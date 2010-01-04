function process_combat_event(ev)

        if (ev.type = EVID_DAMAGED)
                        if ((me.hp * 100/me.maxhp) < flee_point)
                                flee(ev.source);
                        endif
        endif

endfunction

function in_combat_event_loop(opponent, loops)

	if ( loops > 100 )
		flee(opponent);
		return;
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
