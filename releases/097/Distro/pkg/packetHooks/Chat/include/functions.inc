use os;

include ":chat:settings";

function CheckForValidChars(conference_name)
	// Todo: Move to config, and make note for non-english speakers
	var settings_elem := CH_GetSettingsCfgElem("Settings");
	var valid_chars := GetConfigString(settings_elem, "ValidConfChars");
	
	for i := 1 to Len(conference_name)
		if ( !Find(valid_chars, conference_name[i], 1) )
				return 0;
		endif
	endfor
	
	return 1;
endfunction

function CreateNewConference(conference_name, conference_pass:="", character:="")	
	// I don't know how OSI validates conference names, this is just off of the top of my head
	var settings_elem := CH_GetSettingsCfgElem("Settings");
	if ( len(conference_name) < GetConfigInt(settings_elem, "MinConfNameLength") ||
	     len(conference_name) > GetConfigInt(settings_elem, "MaxConfNameLength") || 
	     CheckForValidChars(conference_name) )
		if ( character )
			SendSysMessage(character, "That is not a valid conference name.");
		else
			Print("CreateNewConference() error: Invalid conference name.");
		endif
		return 0;
	endif
	
	// Check if conference of this name already exists
	if ( CH_GetProp("Conferences", conference_name) != error )
		if ( character )
			SendSysMessage(character, "There is already a conference of that name.");
		else
			Print("CreateNewConference() error: Conference of that name already exists.");
		endif
		return 0;
	endif
	
	var settings := dictionary{"pass" -> conference_pass};
	Print("Calling conf creation: "+CH_SetProp("Conferences", conference_name, settings));
	
	// Send new conference to everyone ("" = everyone because of SendChatPacket())
	SendConferences("", conference_name, conference_pass);
endfunction

function SendConferences(targ:="", conference_name:="", conference_pass:="")
	var conferences := CH_GetDataElem("Conferences");
	var packet;
	
	if ( conference_name )
		if ( CH_CheckDebug() )
			Print("Sending conferences ("+conference_name+") to "+targ);
		endif

		SendChatPacket(CreateNewConferencePacket(conference_name, conference_pass), targ);
	else	
		foreach conference in (conferences.PropNames())
			conferences := CH_GetProp("Conferences", conference);
			conference_pass := conferences.pass;

			if ( CH_CheckDebug() )
				Print("Sending conferences ("+conferences.name+") to "+targ);
			endif

			// Send packet targ, if targ = "" it goes to everyone
			SendChatPacket(CreateNewConferencePacket(conference, conference_pass), targ);
		endforeach
	endif
	
endfunction

function CreateNewConferencePacket(conference_name, conference_pass)
	// Create conference packet and send it to everyone chatting
	var packet := CreatePacket(0xB2, 15 + (Len(conference_name) * 2));
	packet.SetInt16(1, 15 + (Len(conference_name) * 2)); // packet length
	packet.SetInt16(3, 0x03E8); // create conference subcmd
	packet.SetUnicodeString(9, CAscZ(conference_name), 0); // unicode conference name

	if ( conference_pass )
		packet.SetInt16(11 + (Len(conference_name) * 2), 0x31); // password required
	else
		packet.SetInt16(11 + (Len(conference_name) * 2), 0x30); // no password
	endif

	return packet;
endfunction

// Checks password, sends join packet to player and adds them to datafiles.
function AddPlayerToConference(character, conference_name, conference_pass:="")
	Print("Adding player to conference: "+character+" "+conference_name+" "+conference_pass);
	
	if ( conference_pass )
		var conference := CH_GetProp("Conferences", conference_name);
			
		if ( conference.pass != conference_pass )
			SendChatClilocMessage(character, 0x000E); // incorrect password
			return 0;
		endif
	endif
	
	var join_packet := CreatePacket(0xB2, 13 + (Len(conference_name) * 2));
	join_packet.SetInt16(1, 13 + (Len(conference_name) * 2)); // packet size
	join_packet.SetInt16(3, 0x03F1); // you have joined message
	join_packet.SetUnicodeString(9, CAscZ(conference_name), 0);
	join_packet.SendPacket(character);

	CH_AddPlayerToConference(character, conference_name);

	return 1;
endfunction

function SendChatClilocMessage(targ, msgtype, text1:="", text2:="")
	var packet := CreatePacket(0xB2, MSGLEN_VARIABLE);
	packet.SetInt16(3, msgtype); // message type
	packet.SetInt32(5, 0); // unknown
	
	if ( text1 )
		packet.SetUnicodeString(7, CAscZ(text1)); // text for %1
	endif
	
	if ( text2 )
		packet.SetInt16(packet.GetSize(), 0); // null
		packet.SetUnicodeString(packet.GetSize(), CAscZ(text2)); // text for %2
	endif
	
	packet.SetInt16(packet.GetSize(), 0); // null
	packet.SetInt16(1, packet.GetSize()); // packet length
	
	Print("Cliloc is now: "+packet);
	SendChatPacket(packet, targ);
endfunction