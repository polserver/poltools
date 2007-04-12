include ":chat:chatIO";
include ":chat:settings";

function SendChatPacket(byref packet, targ:="", except:="");
	// Determine targets
	var targs;
	if ( !targ ) // everyone
		targs := CH_GetProp("Settings", "Online");
	elseif ( targ.IsA(POLCLASS_MOBILE) && !targ.IsA(POLCLASS_NPC) ) // character ref
		targs := array{ targ.serial };
	elseif ( Lower(TypeOf(targ)) == "array" ) // array of serials
		targs := targ;
	elseif ( Lower(TypeOf(targ)) == "string" ) // conference
		targs := (CH_GetProp("Conferences", targ)).joined;
	endif
	
	foreach serial in targs
		if ( serial in except )
			continue;
		endif
		
		var character := SystemFindObjectBySerial(serial);
		if ( CH_CheckDebug() )
			Print("SendPacketToTarget(): "+character.name+" packet: "+packet);
		endif
		packet.SendPacket(character);
	endforeach
	
	return targs.Size();
endfunction