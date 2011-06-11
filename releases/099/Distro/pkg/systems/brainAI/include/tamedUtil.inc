/*
        Maintened by *Edwards

        2009-10-10
*/
use uo;

include ":brainAI:npcCommands";
include ":brainAI:npcInit";
include ":brainAI:npcNerves";
include ":brainAI:npcUtil";
include ":damage:damage";
include ":mounts:mounts";

function NPC_SetupTamed()

        var master_serial := GetObjProperty( npc, "Master" );
        if( !master_serial )
                return 0;
        endif
        
        var owner := SystemFindObjectBySerial( master_serial, SYSFIND_SEARCH_OFFLINE_MOBILES );
        npc.SetMaster( owner );
        
        SendStatus( owner );

	return owner;
endfunction

function ReleaseTamed( npc, master, system:=0 )

        if( GetObjProperty( npc, "SummonedBy" ))
                SetObjProperty( npc, "GuardKill", 1 );
                npc.Kill();
                return 0;
        endif
        
        var npc_template := NPC_GetNPCConfig( npc.npctemplate );
        SetName( npc, npc_template.Name );
        NPC_SetupName( npc );

	//MP_Unmount( npc.master );
 
        SleepMS(250);
        //AI_Speak( npc, "*"+npc.name+" can roam free again.*", SPEAK_TEXTTYPE_DEFAULT, SPEAK_DOEVENT_DISABLE );

        Release( npc, master );

        return 1;
endfunction

function Release( npc, master )

        AI_EndNerve( npc, "Combat" );
	EraseObjProperty( npc, "#Follow" );
        AI_WarMode( npc, 0 );

        SendStatus( master );

        npc.SetMaster(0);
        EraseObjProperty( npc, "Master" );

        return 1;
endfunction

function StopActivities( npc )

        EraseObjProperty( npc, "Stay" );
        EraseObjProperty( npc, "#Follow" );
        EraseObjProperty( npc, "#Guard" );
        
        AI_EndNerve( npc, "Combat" );
        AI_WarMode( npc, 0 );

        return 1;
endfunction
