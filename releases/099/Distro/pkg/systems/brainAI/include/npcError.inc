/*
        Maintened by *Edwards

        2009-10-10
*/
use uo;
use os;

function NPC_ErrorSysLog( npc, errmsg )

	PrintTextAbove( npc, errmsg, 3, 33 );
 
	SysLog( "------------" );
	SysLog( "NPC Template: "+npc.npctemplate );
	SysLog( "NPC Name    : "+npc.name );
	SysLog( errmsg );
	SysLog( "------------" );

	npc.Disable( "invul" );
	SetObjProperty( npc, "GuardKill", 1 );
	npc.Kill();

	return 1;
endfunction
