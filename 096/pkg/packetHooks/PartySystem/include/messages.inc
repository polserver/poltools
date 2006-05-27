use uo;
use os;
use polsys;
use unicode;

include ":partySystem:config";
include ":partySystem:packetInfo";

const OFFSET_PARTY_SOURCE_PLAYERID	:= 6;
const OFFSET_PARTY_MESSAGE		:= 10;

const USE_PACKETED_MESSAGES		:= 1;

function SendPartymessage(targ, source, message, self_send := 0)
	if ( TypeOf(message) != "Array" )
		message := CAscZ(message);
	endif
	
	if ( USE_PACKETED_MESSAGES )
		// Create party message packet
		var packet := CreatePacket(MSGTYPE_PARTY, MSGLEN_VARIABLE);
		packet.SetInt16(OFFSET_PARTY_SUBCMD, SUBCMD_PARTY); // Set subcmd to Party
		packet.SetInt32(OFFSET_PARTY_SOURCE_PLAYERID, source.serial); // Set source serial
		packet.SetUnicodeString(OFFSET_PARTY_MESSAGE, message, 1); // Set unicode message
		packet.SetInt16(OFFSET_PARTY_MSGLEN, packet.GetLength()); // Set packet length
		
		// Send packet to party members if targ is an array, else send to targ
		if ( TypeOf(targ) == "Array" )
			packet.SetInt8(OFFSET_PARTY_SUBSUBCMD, 4); // Set subsubcmd to party message
			
			var member;
			foreach memberid in (targ)
				if ( self_send || source.serial != memberid )
					member := SystemFindObjectBySerial(memberid);
					if ( member )
						packet.SendPacket(member);
					endif
				endif
				SleepMS(2);
			endforeach
		else
			packet.SetInt8(OFFSET_PARTY_SUBSUBCMD, 3); // Set subsubcmd to private
			packet.SendPacket(targ);
		endif
	else
		// Send packet to party members if targ is an array, else send to targ
		if ( TypeOf(targ) == "Array")
			var member;
			foreach memberid in (targ)
				if ( self_send || source.serial != memberid )
					member := SystemFindObjectBySerial(memberid);
					if ( member )
						SendSysMessageUC(member, CAscZ("["+source.name+"]: ")+message, member.uclang, 3, 368);
					endif
				endif
				SleepMS(2);
			endforeach
		else
			SendSysMessageUC(targ, CAscZ("["+source.name+"]: ")+message, targ.uclang, 3, 368);
		endif
	endif
	
	return 1;
endfunction