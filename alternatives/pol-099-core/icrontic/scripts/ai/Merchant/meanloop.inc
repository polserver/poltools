function main_AI_loop(next_wander := 0)

    var ev;
    var looter := GetObjProperty( me, "looter" );
    var wanders := 60;
    var next_loot := readgameclock() + 20;

    look_around();

    EnableMainEvents();

    while (1)

	if (readgameclock() > next_wander)
        	wander();
		if ( (looter) and (ReadGameClock() > next_loot) )
			grabloot();
			next_loot := ReadGameClock() + 10;
		endif
		next_wander := readgameclock()+1;
		wanders := wanders +1;
		if (wanders > 60)
			wanders :=0;
			ev := sleepmode();
	    	endif
	else
		ev := os::wait_for_event( 1 );
	endif

        repeat
            case (ev.type)
                EVID_ENGAGED:
                EVID_DAMAGED:
		    fight(ev.source);
                EVID_ENTEREDAREA:
                    if (!ev.source.npctemplate)
			Fight( ev.source );
                    endif
                default:
		    process_noncombat_event(ev);
            endcase

        until (! (ev := os::wait_for_event(1)) );

   endwhile      

endfunction       

function look_around()

	foreach critter in ListMobilesInLineOfSight( me, HALT_THRESHOLD )
          if ((!critter.npctemplate) or (critter.script = "tamed") or (critter.script = "meek") )
                Fight( critter );
		return;
          endif
	endforeach

endfunction

function EnableMainEvents()

    cleareventque();
    DisableEvents( EVID_SPEECH + EVID_LEFTAREA + EVID_DISENGAGED + EVID_OPPONENT_MOVED );
    EnableEvents( EVID_ENGAGED + EVID_DAMAGED + EVID_ENTEREDAREA, HALT_THRESHOLD );

endfunction

function DisableMainEvents()

    cleareventque();
    DisableEvents( EVID_ENGAGED + EVID_DAMAGED + EVID_ENTEREDAREA);
    cleareventque();
    
endfunction

function cleareventque()

	var ev;
	repeat
	until (! (ev := os::wait_for_event(0)) );

endfunction
