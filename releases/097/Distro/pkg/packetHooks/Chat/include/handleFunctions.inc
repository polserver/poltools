include ":chat:chatIO";
include ":chat:packets";
include ":chat:functions";
include ":chat:settings";

function hChangePassword(character, byref packet)
	if ( CH_CheckDebug() )
		Print("handleChangePassword: "+character.name+" "+packet);
	endif
	
	// Change conf password
	
	return 1;
endfunction

function hClose(character, byref packet)
	if ( CH_CheckDebug() )
		Print("handleClose: "+character.name+" "+packet);
	endif
	
	// Leave conference ?
	
	var new_packet := CreatePacket(0xB2, 13);
	new_packet.SetInt16(1, 13); // packet size
	new_packet.SetInt16(3, 0x03EC); // close chat
	new_packet.SendPacket(character);
	
	var chat_name := GetObjProperty(character, "ChatName");
	new_packet := CreatePacket(0xB2, 9 + (Len(chat_name) * 2));
	new_packet.SetInt16(1, 9 + (Len(chat_name) * 2)); // packet size
	new_packet.SetInt16(3, 0x03EF); // remove player
	
	CH_RemovePlayer(character.serial);
	
	// Maybe only send to others in conference
	SendChatPacket(new_packet);
	
	return 1;
endfunction

function hMessage(character, byref packet)
	if ( CH_CheckDebug() )
		Print("handleMessage: "+character.name+" "+packet);
	endif
	
	// if player not in conference give an error
	// if player is in conference
	//   find all valid targets
	//   consider everyone's ignore list
	//   consider speaking privs
	//   send text to other valid targets

	return 1;
endfunction

function hJoinConference(character, byref packet)
	if ( CH_CheckDebug() )
		Print("handleJoinConference: "+character.name+" "+packet);
	endif

	// Sent like this: "ConferenceName" password, split them up
	var conference_name := CChrZ(packet.GetUnicodeString(11, (packet.GetSize() / 2) - 5));
	conference_name := SplitWords(conference_name, CChr(0x22));

	var conference_pass;
	if ( conference_name[2] )
		conference_pass := conference_name[2];
		conference_pass := conference_pass[2, Len(conference_pass) - 2];
	endif

	conference_name := conference_name[1];
	
	// AddPlayerToConference() checks password
	AddPlayerToConference(character, conference_name, conference_pass);	
	
	return 1;
endfunction

function hCreateNewConference(character, byref packet)
	if ( CH_CheckDebug() )
		Print("handleCreateNewConference: "+character.name+" "+packet);
	endif
	
	var conference_name := CChrZ(packet.GetUnicodeString(9, (packet.GetSize() / 2) - 5));
	var conference_pass := "";
	
	if ( Find(conference_name, "{", 1) )
		conference_pass := conference_name[Find(conference_name, "{", 1) + 1, Find(conference_name, "}", 1) - 2];
		conference_name := conference_name[1, Find(conference_name, "{", 1) - 1];	
	endif
	
	Print("Recieved conference: "+conference_name+" "+conference_pass);

	// Create conference and automaticly join player since confs are destroyed when all leave.
	CreateNewConference(conference_name, conference_pass, character);
	AddPlayerToConference(character, conference_name, conference_pass);
	
	return 1;
endfunction

function hRenameConference(character, byref packet)
	if ( CH_CheckDebug() )
		Print("handleREnameConference: "+character.name+" "+packet);
	endif

	return 1;
endfunction

function hSendPrivateMessage(character, byref packet)
	// check ignore
	// display message
	if ( CH_CheckDebug() )
		Print("handleSendPrivateMessage: "+character.name+" "+packet);
	endif

	return 1;
endfunction

function hIgnore(character, byref packet)
	// add player to ignore list
	if ( CH_CheckDebug() )
		Print("handleIgnore: "+character.name+" "+packet);
	endif

	return 1;
endfunction

function hStopIgnoring(character, byref packet)
	// remove player from ignore list
	if ( CH_CheckDebug() )
		Print("handleStopIgnoring: "+character.name+" "+packet);
	endif

	return 1;
endfunction

function hToggleIgnore(character, byref packet)
	if ( CH_CheckDebug() )
		Print("handleToggleIgnore: "+character.name+" "+packet);
	endif

	return 1;
endfunction

function hGrantSpeakingPrivs(character, byref packet)
	if ( CH_CheckDebug() )
		Print("handleGrantSpeakingPrivs: "+character.name+" "+packet);
	endif

	return 1;
endfunction

function hRemoveSpeakingPrivs(character, byref packet)
	if ( CH_CheckDebug() )
		Print("handleRemoveSpeakingPrivs: "+character.name+" "+packet);
	endif

	return 1;
endfunction