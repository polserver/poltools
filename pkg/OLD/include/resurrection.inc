/*
 * $Id: resurrection.inc 898 2005-11-04 07:39:41Z muaddiblsd $
 *
 * Uhh what the fuck is this anyway?
 *
 */

use uo;
use os;

include ":attributes:attributes";
include ":gumps:gumps";

const MAXIMUM_LOSS := 0.20; //20% loss for 20 shortterm murders or more

function ResNow(mobile)

	var gump := GFCreateGump();
	GFClosable(gump, 0);
	GFGumpPic(gump, 300, 230, 2070);
	GFAddButton(gump, 330, 305, 2073, 2072, GF_CLOSE_BTN, 10);
	GFAddButton(gump, 400, 305, 2076, 2075, GF_CLOSE_BTN, 11);
	GFTextLine(gump, 332, 264, 40, "Resurrect Now?");

	var input := GFSendGump(mobile, gump);
	input := input[0];
	input := input-10;

	if ( !input )
		return 0;
	else
		return 1;
	endif

endfunction

function MurdererStatLoss(who)

	var shortc := GetObjProperty(who,"shortmurders");
	if( !shortc )
		return 0;
	endif

	if( shortc > 4 )
		var multiplier := CInt(shortc)/100.0;
		if( multiplier > MAXIMUM_LOSS )
			multiplier := MAXIMUM_LOSS;
		endif

		multiplier := 1.0 - multiplier;

		var str := AP_GetTrueStat(who, "strength");
		var dex := AP_GetTrueStat(who, "dexterity");
		var inte := AP_GetTrueStat(who, "intelligence");

		str := CInt(str * multiplier);
		dex := CInt(dex * multiplier);
		inte := CInt(inte * multiplier);

		AP_SetTrueStat(who, "strength", str);
		AP_SetTrueStat(who, "dexterity", dex);
		AP_SetTrueStat(who, "intelligence", inte);

		AP_SetVital(who, "hits", AP_GetVitalMaximumValue(who, "hits"));
		AP_SetVital(who, "stamina", AP_GetVitalMaximumValue(who, "stamina"));
		AP_SetVital(who, "mana", AP_GetVitalMaximumValue(who, "mana"));

		var newval;

		foreach entry in (AP_ListAttributesByType("skill"))
			var skill_level := AP_GetTrueSkill(who, entry);
			if( skill_level > 0.0 )
				newval := skill_level * multiplier;
				AP_SetTrueSkill(who, entry, newval);
			endif
		endforeach
		SendSysMessage(who,"Thy body pays the price for thy sins.");
	endif

endfunction