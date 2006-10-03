/*
 * $Id$
 *
 *---------------------------------
 * Current Version
 * EVENTPROCESSOR.INC - v1.0
 * Updated 9/11/2005 2:06AM
 *
 * -- Revision v1.0 --
 * Austin:
 *  Created include file
 *---------------------------------
 */

function ProcessEvent(byref event)
	var old_priority := Set_Priority(25);

	case( event.type )
		//
		// *Check movement first, since its the event that seems to be sent most often.
		//

		// Move NPC
		// .source	: From/To whom/what?
		// .speed	: Walk/Run
		// .direction	: Away/Towards
		// .loop	: Number of times to attempt to loop the movement.
		NPCEVENT_MOVE:
			Movement(event);
			break;

		// Instructs the npc to walk toward an XY coordinate.
		// .x and .y	: From/To  coordinates
		// .speed	: Walk/Run
		// .direction	: Away/Towards
		// .loop	: Number of times to attempt to loop the movement.
		NPCEVENT_MOVE_XY:
			MovementXY(event);
			break;

		// Wander somewhere
		// .loop	: Number of times to attept to wander.
		NPCEVENT_WANDER:
			MovementWander(event);
			break;

		//
		// * Check System Events that the core sends.
		//

		// Triggered when a mobile enter's the npcs 'area size'.
		// .source	: What entered the area
		SYSEVENT_ENTEREDAREA:
			if ( g_scripts.Exists("EnterArea") )
				var process := GetNerveProcess(npc, "EnterArea");
				if ( !process )
					StartNerve(npc, "EnterArea", g_scripts["EnterArea"].script, array{event, g_settings, g_scripts}, g_scripts["EnterArea"].flags);
				else
					process.SendEvent(event);
				endif
			endif
			break;

		// Triggered when a mobile leaves the npc's 'area size'.
		// .source	: What left the area
		SYSEVENT_LEFTAREA:
			if ( g_scripts.Exists("LeftArea") )
				var process := GetNerveProcess(npc, "LeftArea");
				if ( !process )
					StartNerve(npc, "LeftArea", g_scripts["LeftArea"].script, array{event, g_settings, g_scripts}, g_scripts["LeftArea"].flags);
				else
					process.SendEvent(event);
				endif
			endif
			break;

		// Someone has changed from blue to gray/red and was already within the ENTERED_AREA range
		// .source	: What changed criminal status.
		SYSEVENT_GONE_CRIMINAL:
			if ( g_scripts.Exists("GoneCriminal") )
				var process := GetNerveProcess(npc, "GoneCriminal");
				if ( !process )
					StartNerve(npc, "GoneCriminal", g_scripts["GoneCriminal"].script, array{event, g_settings, g_scripts}, g_scripts["GoneCriminal"].flags);
				else
					process.SendEvent(event);
				endif
			endif
			break;

		// Instructs the npc to run its disengage nerve.
		// When something attacking it goes out of warmode, this is run.
		// .source	: What was targetting this npc but has now left combat mode.
		SYSEVENT_DISENGAGED:
			if ( g_scripts.Exists("Disengaged") )
				var process := GetNerveProcess(npc, "Disengaged");
				if ( !process )
					StartNerve(npc, "Disengaged", g_scripts["Disengaged"].script, array{event, g_settings, g_scripts}, g_scripts["Disengaged"].flags);
				else
					process.SendEvent(event);
				endif
			endif
			break;

		// Sent when something targets the npc to begin combat or
		// when something has damaged the npc.
		// .source	: What caused the damage or engaged the npc
		// For SYSEVENT_DAMAGED
		// .damaged	: Integer amount of damage.
		SYSEVENT_ENGAGED:
		SYSEVENT_DAMAGED:
			if ( g_scripts.Exists("Combat") )
				if ( (event.source).ISA(POLCLASS_MOBILE) )
					var process := GetNerveProcess(npc, "Combat");
					if ( !process )
						StartNerve(npc, "Combat", g_scripts["Combat"].script, array{event, g_settings, g_scripts}, g_scripts["Combat"].flags);
					else
						process.SendEvent(event);
					endif
				endif
			endif
			break;

		// When speech is made by a mobile, this nerve is used.
		// .source	: Where the speech originated.
		// .text	: The actual speech
		// .uc_text	: a "Unicode array" of 2-byte "Big Endian" integers.
		// .langcode	: a 3-character, uppercase language code.
		// .texttype	: "yell", "whisper", "emote", "default".
		SYSEVENT_SPEECH:
		SYSEVENT_GHOST_SPEECH:
			if( g_scripts.Exists("Listen") )
				var process := GetNerveProcess(npc, "Listen");
				if ( !process )
					StartNerve(npc, "Listen", g_scripts["Listen"].script, array{event, g_settings, g_scripts}, g_scripts["Listen"].flags);
				else
					process.SendEvent(event);
				endif
			endif
			break;

		// Instructs the npc to run its double click script.
		// Run when the npc is double clicked by a player.
		// .source	: What sent the double click
		SYSEVENT_DOUBLECLICKED:
			if ( g_scripts.Exists("DblClick") )
				if ( Distance(event.source, npc) <= g_settings["DblClickRange"] )
					var process := GetNerveProcess(npc, "DblClick");
					if ( !process )
						StartNerve(npc, "DblClick", g_scripts["DblClick"].script, array{event, g_settings, g_scripts}, g_scripts["DblClick"].flags);
					else
						process.SendEvent(event);
					endif
				endif
			endif
			break;

		// The NPC is a merchant, and has sold or purchased something.
		// Both sold and bought events are sent to this same nerve.
		// The best thing to do is fork it into two functions inside the
		// nerve script to handle each event type.
		// MERCHANT_SOLD means the merchant sold an item to the player.
		// MERCHANT_BOUGHT means the merchatn bought something from the player.
		//
		// .source	: Who the merchant is selling to/buying from.
		// .amount	: Gold amount for the sale/purchase.
		SYSEVENT_MERCHANT_SOLD:
		SYSEVENT_MERCHANT_BOUGHT:
			if ( g_scripts.Exists("Merchant") )
				var process := GetNerveProcess(npc, "Merchant");
				if ( !process )
					StartNerve(npc, "Merchant", g_scripts["Merchant"].script, array{event, g_settings, g_scripts}, g_scripts["Merchant"].flags);
				else
					process.SendEvent(event);
				endif
			endif
			break;

		// Someone has dragged an item onto the npc.
		// .source	: What dragged the item over the npc.
		// .item	: The item being given to the npc.
		SYSEVENT_ITEM_GIVEN:
			if ( g_scripts.Exists("ItemGiven") )
				var process := GetNerveProcess(npc, "ItemGiven");
				if ( !process )
					StartNerve(npc, "ItemGiven", g_scripts["ItemGiven"].script, array{event, g_settings, g_scripts}, g_scripts["ItemGiven"].flags);
				else
					process.SendEvent(event);
				endif
			endif
			break;

		//
		// * Check custom events
		//

		//A nerve has instructed the brain to kill another nerve or its self.
		// .name	: Nerve name
		NPCEVENT_ENDNERVE:
			StopNerve(npc, event.name);
			break;

		//A nerve has instructed the brain to start another nerve.
		// .name	: Nerve name
		// .script	: Nerve script
		// .data	: array of goodies
		NPCEVENT_STARTNERVE:
			StartNerve(npc, event.name, event.script, event.data, event.flags);
			break;

		// Say something
		// .text	: Speech content
		// .texttype	: Speech type ("default", "whisper", yell")
		// .doevent	: Sends SYSEVENT_SPEECH to surrounding npcs
		NPCEVENT_SPEAK:
			Say(event.text, event.texttype, event.doevent);
			break;

		// Turn toward something
		// .source	: Target (anything)
		// .direction	: NETURN_TOWARD or NETURN_AWAY
		NPCEVENT_TURN:
			case (event.direction)
				NETURN_TOWARD:
					TurnToward(event.source);
				NETURN_AWAY:
					TurnAwayFrom(event.source);
			endcase
			break;

		// Face towards or away from XY coordinates.
		// .x and .y replace .source for TURN_XY
		// .direction	: NETURN_TOWARD or NETURN_AWAY
		NPCEVENT_TURN_XY:
			case (event.direction)
				NETURN_TOWARD:
					TurnTowardLocation(event.x, event.y);
				NETURN_AWAY:
					TurnAwayFromLocation(event.x, event.y);
			endcase
			break;

		// Set a new opponent!
		// NOTE: Auto-sets Warmode=1!
		// .source	: Opponent (mobile)
		NPCEVENT_OPPONENT:
			SetOpponent(event.source);
			break;

		// Set WarMode state
		// NOTE: Auto-clears Opponent if turned off!
		// .value : 0=Off. 1=On.
		NPCEVENT_WARMODE:
			SetWarmode(event.value);
			break;

		// Force the npc to go into its sleep mode.
		// If the next event in the queue tells it to wake up,
		// this command will be useless.
		NPCEVENT_SLEEP:
			idle_ticks := g_settings["IdleTicks"]+1;
			EnterSleepMode();
			break;

		// Force the npc out of sleep mode.
		NPCEVENT_WAKEUP:
			LeaveSleepMode();
			break;

		// Forward an event from one nerve to another.
		// .NerveName	: The name of the nerve to forward the event to.
		// .mask	: Type of event for the nerve to receive.
		// .data	: Data to forward to the nerve.
		NPCEVENT_FWDNERVE:
		var process := GetNerveProcess(npc, event.NerveName);
			if ( process )
				process.SendEvent(event);
			endif
			break;

		// If the event .type is unknown, send it to the
		// npc's "virtual" nerve if it has one.
		default:
			if ( g_scripts.Exists("Virtual") )
				var process := GetNerveProcess(npc, "Virtual");
				if ( !process )
					StartNerve(npc, "Virtual", g_scripts["Virtual"].script, array{event, g_settings, g_scripts}, g_scripts["Virtual"].flags);
				else
					process.SendEvent(event);
				endif
			endif
			break;
	endcase

	// If the event has a .process member, reply to it by sending an event with
	// .type being the NPCEVENT_DONE constant and telling it what event it sent
	// that has finished being processed.
	/*
	 * Pseudo code for now..
	if ( flags & NF_INFORM_ON_COMPLETION )
		var response := struct{"type":=NPCEVENT_DONE, "finished":=event.type};
		nerve.SendEvent(response);
	endif
	*/

	Set_Priority(old_priority);
endfunction
