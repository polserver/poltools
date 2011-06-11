/*
        Maintened by *Edwards

        2009-10-10
        
        Last Modifications 2009-12-21
             * Added NPCEVENT_ANCHOR event info.

        Last Modifications 2009-12-26
             * Fixed code to receive npc.master order with better performances.
*/

function ProcessEvent( byref event )

	var old_priority := Set_Priority(25);
 
	case( event.type )

		NPCEVENT_ANCHOR:
			SetAnchor( event.x, event.y, event.distance, event.percent );
			break;
   
		NPCEVENT_MOVE:
                        MovementXY( event );
			break;

		NPCEVENT_WANDER:
			Wander();
			break;

		// Triggered when a mobile enter's the npcs 'area size'.
		SYSEVENT_ENTEREDAREA:
                        if( event.source.IsA( POLCLASS_NPC ) && !event.source.master )
                        	break;
                        endif
                        
			if( g_scripts.Exists( "EnterArea" ))
				var process := GetNerveProcess( npc, "EnterArea" );
				if( !process )
					StartNerve( npc, "EnterArea", g_scripts["EnterArea"].script, array{event, g_settings, g_scripts}, g_scripts["EnterArea"].flags);
				else
					process.SendEvent( event );
				endif
			endif
			break;

		// Triggered when a mobile leaves the npc's 'area size'.
		SYSEVENT_LEFTAREA:
                        if( event.source.IsA( POLCLASS_NPC ) && !event.source.master )
                        	break;
                        endif
                        
			if( g_scripts.Exists( "LeftArea" ))
				var process := GetNerveProcess( npc, "LeftArea" );
				if( !process )
					StartNerve( npc, "LeftArea", g_scripts["LeftArea"].script, array{event, g_settings, g_scripts}, g_scripts["LeftArea"].flags );
				else
					process.SendEvent( event );
				endif
			endif
			break;

		// Someone has changed from blue to gray/red and was already within the ENTERED_AREA range
		SYSEVENT_GONE_CRIMINAL:
			if( g_scripts.Exists( "GoneCriminal" ))
				var process := GetNerveProcess( npc, "GoneCriminal" );
				if( !process )
					StartNerve( npc, "GoneCriminal", g_scripts["GoneCriminal"].script, array{event, g_settings, g_scripts}, g_scripts["GoneCriminal"].flags  );
				else
					process.SendEvent( event );
				endif
			endif
			break;

		// When something attacking it goes out of warmode, this is run.
		SYSEVENT_DISENGAGED:
			if( g_scripts.Exists( "Disengaged" ))
				var process := GetNerveProcess( npc, "Disengaged" );
				if( !process )
					StartNerve( npc, "Disengaged", g_scripts["Disengaged"].script, array{event, g_settings, g_scripts}, g_scripts["Disengaged"].flags );
				else
					process.SendEvent( event );
				endif
			endif
			break;

		// Sent when something targets the npc to begin combat or
		// when something has damaged the npc.
		SYSEVENT_ENGAGED:
		SYSEVENT_DAMAGED:
			if( g_scripts.Exists( "Combat" ))
				var process := GetNerveProcess( npc, "Combat" );
				if( !process )
					StartNerve( npc, "Combat", g_scripts["Combat"].script, array{event, g_settings, g_scripts}, g_scripts["Combat"].flags );
				else
					process.SendEvent( event );
				endif
			endif
			break;

		// When speech is made by a mobile, this nerve is used.
		SYSEVENT_SPEECH:
		SYSEVENT_GHOST_SPEECH:
                        if( master )
				var process := GetNerveProcess( npc, "Listen" );
				if( !process )
					StartNerve( npc, "Listen", ":brainAI:scripts/listen/tamed", array{event, g_settings, g_scripts} );
				else
					process.SendEvent( event );
				endif
			elseif( g_scripts.Exists( "Listen" ))
				var process := GetNerveProcess( npc, "Listen" );
				if( !process )
					StartNerve( npc, "Listen", g_scripts["Listen"].script, array{event, g_settings, g_scripts}, g_scripts["Listen"].flags );
				else
					process.SendEvent( event );
				endif
			endif
			break;

		// Run when the npc is double clicked by a player.
		SYSEVENT_DOUBLECLICKED:
                        if( master )
				if( Distance( event.source, npc ) <= 1 )
					var process := GetNerveProcess( npc, "DblClick" );
					if( !process )
						StartNerve( npc, "DblClick", ":brainAI:scripts/doubleClick/tamed", array{event, g_settings, g_scripts} );
					else
						process.SendEvent( event );
					endif
				endif
                        elseif( g_scripts.Exists( "DblClick" ))
				if( Distance( event.source, npc ) <= g_settings["DblClickRange"] )
					var process := GetNerveProcess( npc, "DblClick" );
					if( !process )
						StartNerve( npc, "DblClick", g_scripts["DblClick"].script, array{event, g_settings, g_scripts}, g_scripts["DblClick"].flags );
					else
						process.SendEvent( event );
					endif
				endif
			endif
			break;

		SYSEVENT_MERCHANT_SOLD:
		SYSEVENT_MERCHANT_BOUGHT:
			if( g_scripts.Exists( "Merchant" ))
				var process := GetNerveProcess( npc, "Merchant" );
				if( !process )
					StartNerve( npc, "Merchant", g_scripts["Merchant"].script, array{event, g_settings, g_scripts}, g_scripts["Merchant"].flags );
				else
					process.SendEvent( event );
				endif
			endif
			break;

		// Someone has dragged an item onto the npc.
		SYSEVENT_ITEM_GIVEN:
			if( g_scripts.Exists( "ItemGiven" ))
				var process := GetNerveProcess( npc, "ItemGiven" );
				if( !process )
					StartNerve( npc, "ItemGiven", g_scripts["ItemGiven"].script, array{event, g_settings, g_scripts}, g_scripts["ItemGiven"].flags );
				else
					process.SendEvent( event );
				endif
			endif
			break;

		//A nerve has instructed the brain to kill another nerve or its self.
		NPCEVENT_ENDNERVE:
			StopNerve( npc, event.name );
			break;

		//A nerve has instructed the brain to start another nerve.
		NPCEVENT_STARTNERVE:
			StartNerve( npc, event.name, event.script, event.data, event.flags );
			break;

		// Say something
		NPCEVENT_SPEAK:
			Say( event.text, event.texttype, event.doevent );
			break;

		// Turn toward something
		NPCEVENT_TURN:
			case( event.direction )
				NETURN_TOWARD:
					TurnToward( event.source );
				NETURN_AWAY:
					TurnAwayFrom( event.source );
			endcase
			break;

		// Face towards or away from XY coordinates.
		NPCEVENT_TURN_XY:
			case( event.direction )
				NETURN_TOWARD:
					TurnTowardLocation( event.x, event.y );
				NETURN_AWAY:
					TurnAwayFromLocation( event.x, event.y );
			endcase
			break;

		// Set a new opponent!
		NPCEVENT_OPPONENT:
			SetOpponent( event.source );
			break;

		// Set WarMode state
		NPCEVENT_WARMODE:
			SetWarmode( event.value );
			break;

		// Force the npc to go into its sleep mode.
		NPCEVENT_SLEEP:
			EnterSleepMode();
			break;

		// Force the npc out of sleep mode.
		NPCEVENT_WAKEUP:
			LeaveSleepMode();
			break;

		// Forward an event from one nerve to another.
		NPCEVENT_FWDNERVE:
			var process := GetNerveProcess( npc, event.NerveName );
			if( process )
				process.SendEvent( event );
			endif
			break;

		// If the event .type is unknown, send it to the
		// npc's "virtual" nerve if it has one.
		default:
			if( g_scripts.Exists( "Virtual" ))
				StartNerve( npc, "Virtual", g_scripts["Virtual"].script, array{event, g_settings, g_scripts}, g_scripts["Virtual"].flags );
			endif
			break;
	endcase

	// If the event has a .process member, reply to it by sending an event with
	// .type being the NPCEVENT_DONE constant and telling it what event it sent
	// that has finished being processed.

	if( event.process )
		var nerve_name := GetNerveNameByPid( npc, event.process );
	 	if( nerve_name )
	 		var flags := GetNerveFlags( npc, nerve_name );
	 		if( flags & NF_INFORM_ON_COMPLETION )
	 			var process := GetProcess( CInt( event.process ));
	 			var response := struct{"type":=NPCEVENT_DONE, "finished":=event.type};
				process.SendEvent( response );
			endif
	 	endif
	endif

	Set_Priority( old_priority );

	return 1;
endfunction
