/* $Id: messages.inc 900 2005-11-04 13:48:31Z panosl $
 *
 * Purpose
 * Provide various functions and constants for system messages.
 *
 */
use uo;
use polsys;
use basic;

include ":gumps:gumps";
include ":gumps:gumps_ex";
include "include/localization";
include "include/packets";
include "include/client";
include "include/time";

enum MSG_TYPE
	MSG_IMPORTANT	:= 1,
	MSG_MISC	:= 2,
	MSG_COMMAND	:= 3,
	MSG_SKILL	:= 4
endenum

var BKGND_SILVERTRIM := GFCfgConst("Defaults", "BackGround");
var BKGND_DARK_STONE := GFCfgConst("Defaults", "ForeGround");

/*
 * SendMessage(who, id, type)
 *
 * Purpose
 * Sends a message to a character.
 *
 * Parameters
 * who:     Reference to the character
 * id:      The text ID to send to the player
 * parms:   The text ID parameters
 * type:    The type of text to be send
 *
 * Return value
 * No return value.
 *
 * TODO:
 * Make the hue depend on the message type.
 *
 */
function SendMessage(who, id, parms, type)
	type := 0; // TODO
	if(TypeOf(id) == "Integer")
		SendCliloc(who, id, parms);
	else
		SendSysMessage(who, FormatMessage(id, parms));
	endif

	return;
endfunction


/*
 * SendAll(id, parms, type)
 *
 * Purpose
 * Sends a message to all online characters.
 *
 * Parameters
 * text:    The text ID to send to all players
 * parms:   The text ID parameters
 * type:    The type of text to be send
 *
 * Return value
 * No return value.
 *
 */
function SendAll(text, parms, type)
	foreach player in EnumerateOnlineCharacters()
		SendMessage(player, text, parms, type);
		SleepMs(NO_RUNAWAY);
	endforeach

	return;
endfunction


/*
 * SendHTMLMessage(who, text)
 *
 * Purpose
 * Sends a HTML gump containing the text to the character.
 *
 * Parameters
 * who:     Reference to the character which should receive the gump
 * text:    The text to send, may contain HTML tags
 *
 * Return value
 * The gump's return value
 *
 */
function SendHTMLMessage(who, text)
	var msgGump := GFCreateGump(100, 100, 510, 230);
	GFAddAlphaRegion(msgGump, 0, 0, 510, 230);
	GFPage(msgGump, 0);
	GFResizePic(msgGump, 0, 0, BKGND_SILVERTRIM, 510, 230);
	GFHTMLArea(msgGump, 10, 10, 490, 210, text, 1, 1);
	GFSendGump(who, msgGump);

	return;
endfunction


/*
 * SendCliloc(who, num, params, type, origin, hue, font)
 *
 * Purpose
 * Sends a localized message to the client.
 *
 * Parameters
 * who:     Reference to the character which should receive the message
 * num:     The cliloc index
 * params:  Array of strings to use within the localized message
 * type:    The type of the message
 * origin:  Reference to the origin of the text
 * hue:     The text hue
 * font:    The text font
 *
 * Return value
 * No return value
 *
 */
function SendCliloc(who, num, params, type := SPEECH_REGULAR, origin := 0, hue := _DEFAULT_TEXT_COLOR, font := FONT_NORMAL)
	var packet;

	packet := CreatePacket(PACKET_CLILOC, MSGLEN_VARIABLE);
	if (origin)
		packet.SetInt32(3, origin.serial);
		if (origin.isA(POLCLASS_ITEM))
			packet.SetString(18, origin.desc, 1);
		else
			packet.SetString(18, origin.name, 1);
		endif
	else
		packet.SetInt32(3, 0xFFFFFFFF);
		packet.SetString(18, "SYSTEM", 1);
	endif
	packet.SetInt16(7, who.graphic);
	packet.SetInt8(9, type);
	packet.SetInt16(10, hue);
	packet.SetInt16(12, font);
	packet.SetInt32(14, num);
	if (params == 0)
		packet.SetInt8(47, 0);
	else
		var args := array;
		foreach param in params
			args := args + CAscZ(CStr(param)) + CAscZ("\t");
		endforeach
		packet.SetUnicodeString(47, args, 1);
	endif
	packet.SetInt16(1, packet.GetSize());
	packet.SendPacket(who);

	return;
endfunction


/*
 * FormatMessage(message, parms)
 *
 * Purpose
 * Formats a message by inserting the parameters.
 *
 * Parameters
 * message: The message to be formatted, delimited with ~
 * parms:   The parameters to be inserted, as array
 *
 * Return value
 * The formatted message
 *
 */
function FormatMessage(message, parms)
    var i, j;
    var num := 1;

    for(i := 1; i <= len(message); i := i + 1)
        if(message[i] == "~")
            var keyword := "";
            for(j := i; j < len(message); j := j + 1)
                keyword := keyword + message[j];
                if(message[j + 1] == "~")
                    keyword := keyword + message[j + 1];
                    break;
                endif
            endfor
            message[keyword] := CStr(parms[num]);
            num := num + 1;
        endif
    endfor

    return message;
endfunction
