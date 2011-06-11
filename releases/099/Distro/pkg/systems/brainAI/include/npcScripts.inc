/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-10
        
        Last Modificatoins 2009-11-08
             * Separated settings and scripts in different .inc
*/
use uo;
use os;
use cfgfile;

include ":brainAI:npcUtil";

function AI_GetScriptsCfgFile()

	var cfg := ReadConfigFile( ":brainAI:AIScripts" );
	if( cfg.errortext )
		SysLog( "Error::AI_GetScriptsCfgFile() - Unable to open [:brainAI:AIScripts] ->"+cfg.errortext );
	endif

	return cfg;
endfunction

function AI_GetScriptsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := AI_GetScriptsCfgFile();
	endif

	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::AI_GetScriptsCfgElem() - Unable to find elem ["+elem_name+"] -->"+elem.errortext );
	endif

	return elem;
endfunction

function NPC_LoadScriptSettings( source )

	var npc_elem := NPC_GetNPCConfig( source );
	var script_elem := Lower( GetConfigString( npc_elem, "AIScripts" ));

        var scripts_cfg := AI_GetScriptsCfgElem( script_elem );
        if( scripts_cfg.errortext )
                 SysLog( "Error getting AIScripts for <"+source.npctemplate+"> -->"+scripts_cfg.errortext );
        endif

	var script_list := GetConfigStringDictionary( scripts_cfg, "AIScript" );

	var script_settings := dictionary;
	foreach script_type in ( script_list.Keys() )
		var temp := struct;
		temp.+script := script_list[script_type];

		temp.+flags := 0x0;

		script_settings[script_type] := temp;
		SleepMS(10);
	endforeach

	if( source.IsA( POLCLASS_NPC ))
		var cstm_scripts := GetObjProperty( source, "AIScripts" );
		foreach key in ( cstm_scripts.Keys() )
			script_settings[key] := cstm_scripts[key];
			SleepMS(10);
		endforeach
	endif

	return script_settings;
endfunction
