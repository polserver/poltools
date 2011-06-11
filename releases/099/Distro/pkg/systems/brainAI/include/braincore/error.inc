/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-10
*/

function FatalError( errmsg )

	PrintTextAbove( npc, errmsg, 3, 33 );
	NPC_ErrorSysLog( npc, errmsg );
	
	npc.Disable( "invul" );
	SetObjProperty( npc, "GuardKill", 1 );
	npc.Kill();
	
	return 1;
endfunction
