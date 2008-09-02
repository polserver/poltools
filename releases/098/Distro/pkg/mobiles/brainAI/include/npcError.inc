/*
 * $Id: npcError.inc 866 2006-10-03 22:18:33Z austinheilman $
 *
 *---------------------------------
 * Current Version
 * NPCERRORS.INC - v1.0
 * Updated 8/31/2005 5:08AM
 *
 * -- Revision v1.0 --
 * Austin:
 *  Created include file
 *---------------------------------
 */

use uo;
use os;


/*
 * NPC_ErrorSysLog(errmsg)
 *
 * Purpose
 * Logs an error message.
 *
 * Parameters
 * errmsg:	Error message
 *
 * Return value
 * Always returns 1
 *
 */
function NPC_ErrorSysLog(mobile, errmsg)
	Set_Critical(1);
	SysLog("NPC Template: "+mobile.npctemplate);
	SysLog("NPC Name    : "+mobile.name);
	SysLog(errmsg);
	SysLog("------------");
	Set_Critical(0);

	return 1;
endfunction
