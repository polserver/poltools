include ":brainAI:npcUtil";
include ":brainAI:npcError";

/*
 * NPC_SetupNameOldAI(mobile, name_elem)
 *
 * Purpose
 * Replaces every instance of <random> in an npc's name with a valid name using ::names.cfg
 *
 * Parameters
 * mobile:	The mobile to do a name check on.
 * name_elem:	Name elem in names.cfg to use
 *		If no name is set, it will use the 'NameList' line in the npc's template.
 *
 * Return value
 * Returns 1 or error
 *
 */
function NPC_SetupNameOldAI(mobile, name_elem:=0)
	if ( !mobile.name["<random>"] )
		return 1;
	elseif ( !name_elem )
		var npc_elem := NPC_GetNPCConfig(mobile);
		name_elem := npc_elem.NameTemplate;
		case(mobile.graphic)
			0x190: name_elem := name_elem + "_Male";
			0x191: name_elem := name_elem + "_Female";
			default: // Does nothing since is not male/female
		endcase
		if ( name_elem.errortext )
			return name_elem;
		endif
	endif

	var npc_names := ReadConfigFile("::names");
	if ( !npc_names[name_elem] )
		var temp := error;
		temp.+errortext := "There is no elem ["+name_elem+"] in NAMES.CFG";

		NPC_ErrorSysLog(mobile, "Error::NPC_SetupName() - "+temp.errortext);
		PrintTextAbove(mobile, temp.errortext);
		return temp;
	endif

	var name_list := GetConfigStringArray(npc_names[name_elem], "Name");
	if ( name_list.Size() < 1 )
		var temp := error{"errortext":="No names were found in the ["+name_elem+"] elem for names.cfg"};

		SysLog("Error::NPC_SetupName() - "+temp.errortext);
		PrintTextAbove(mobile, temp.errortext);

		return temp;
	endif

	var my_name := mobile.name;
	while ( my_name["<random>"] )
		var replacement := name_list[RandomInt(name_list.Size())+1];
		my_name["<random>"] := replacement;
		sleepms(2);
	endwhile

	SetName(mobile, my_name);

	return 1;
endfunction
