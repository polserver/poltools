/*
 * $Id: npcSettings.inc 867 2006-10-03 22:22:04Z austinheilman $
 *
 *---------------------------------
 * Current Version
 * NPCSETTINGS.INC - v1.0
 * Updated 9/8/2005 5:21AM
 *
 * -- Revision v1.0 --
 * Austin:
 *  Created include file
 *---------------------------------
 */

use uo;
use os;
use cfgfile;

include ":brainAI:npcUtil";

/*
 * NPC_DefaultSettings(settings:=0)
 *
 * Purpose
 * Checks the settings dictionary to make sure required values are set.
 * If a dictionary is passed, it will check it for required parameters.
 * If they are not present, they will be set.
 *
 * Parameters
 * settings:	Optional parameter
 *		Dictionary read in from NPC_GetDefaultSettings()
 *
 * Return value
 * Returns a dictionary
 *
 */
function NPC_GetDefaultSettings(settings:=0)
	if ( Lower(TypeOf(settings)) != "dictionary" )
		settings := dictionary;
	endif

	if ( CInt(settings.Exists("CycleWait")) <= 0 )
		settings["CycleWait"] := 10;
	endif

	if ( !settings.Exists("NoSleep") )
		if ( CInt(settings.Exists("SleepWait")) <= 0 )
			settings["SleepWait"] := 1024;
		endif
		if ( CInt(settings.Exists("IdleTicks")) <= 0 )
			settings["IdleTicks"] := 5;
		endif
	endif

	return settings;
endfunction

/*
 * NPC_LoadTemplateSettings(source)
 *
 * Purpose
 * Loads script settings for a template from the appropriate npcdesc.cfg.
 * Uses AISetting lines.
 *
 * Parameters
 * source:	Name of the npc template to load settings for.
 *		If an NPC reference is used it will also load custom settings set on it.
 *
 * Return value
 * Returns a dictionary
 *
 */
function NPC_LoadTemplateSettings(source)
	//
	// * Load config defaults first
	//
	var npc_elem := NPC_GetNPCConfig(source);

	var setting_list := GetConfigStringDictionary(npc_elem, "AISetting");

	var settings := dictionary;
	foreach setting_name in (setting_list.Keys())
		settings[setting_name] := UnPack(setting_list[setting_name]);

		SleepMS(2);
	endforeach

	//
	// * If the source is an npc, check it for custom settings.
	//
	if ( source.IsA(POLCLASS_NPC) )
		var cstm_settings := GetObjProperty(source, "AISettings");
		foreach key in (cstm_settings.Keys())
			if ( cstm_settings[key] == error )
				settings.Erase(key);
			else
				settings[key] := cstm_settings[key];
			endif

			SleepMS(2);
		endforeach
	endif

	return settings;
endfunction

/*
 * NPC_LoadScriptSettings(source)
 *
 * Purpose
 * Loads brain AI script settings for an NPC. Gives .script and .flag values for them.
 * Uses AIScript and AIScriptFlag entries.
 *
 * Parameters
 * source:	Name of the npc template to load settings for.
 *		Can also be an npc reference - if custom settings should be loaded off it.
 *
 * Return value
 * Returns a dictionary
 *
 */
function NPC_LoadScriptSettings(source)
	//
	// * Load config defaults first
	//
	var npc_elem := NPC_GetNPCConfig(source);

	var script_list := GetConfigStringDictionary(npc_elem, "AIScript");
	var flag_list := GetConfigStringDictionary(npc_elem, "AIScriptFlag");

	var script_settings := dictionary;
	foreach script_type in (script_list.Keys())
		var temp := struct;
		temp.+script := script_list[script_type];

		temp.+flags := 0x0;
		if ( flag_list[script_type] )
			temp.flags := UnPack(flag_list[script_type]);
		endif

		script_settings[script_type] := temp;
		SleepMS(2);
	endforeach

	//
	// * If the source is an npc, check it for custom script settings.
	//   Assumes each key in the dictionary property has a .script and .flags pair already.
	//
	if ( source.IsA(POLCLASS_NPC) )
		var cstm_scripts := GetObjProperty(source, "AIScripts");
		foreach key in (cstm_scripts.Keys())
			script_settings[key].script := cstm_scripts[key];
			SleepMS(2);
		endforeach
	endif

	return script_settings;
endfunction

