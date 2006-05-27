/*===============================================================
* Current Version
* ERROR.INC - v1.0
* Updated 8/31/2005 5:08AM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

/*
 * FatalError(errmsg)
 *
 * Purpose
 * Logs an error message and then kills the npc.
 *
 * Parameters
 * errmsg:	Error message - reason for removing the npc.
 *
 * Return value
 * Always returns 1
 *
 */
function FatalError(errmsg)
	Set_Critical(1);
	
	PrintTextAbove(npc, errmsg, _DEFAULT_TEXT_FONT, 32);
	
	NPC_ErrorSysLog(npc, errmsg);
	
	npc.cmdlevel := 1;
	npc.concealed := 1;
	
	npc.disable("invul");
	SetObjProperty(npc, "GuardKill", 1);
	ApplyRawDamage(npc, GetVitalMaximumValue(npc, "Hits"));
	
	Set_Critical(0);
	
	return 1;
endfunction
