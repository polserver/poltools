use uo;
use os;

function NPC_GetMaster(npc)
	return GetObjProperty(npc, "Master");
endfunction

function NPC_SetMaster(npc, master)
	return SetObjProperty(npc, "Master", master);
endfunction