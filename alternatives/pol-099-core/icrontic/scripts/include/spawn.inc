use os;
use uo;
use util;
use cfgfile;

include "npcinfo";


///////////////////
//  tries to create an NPC of the given template at the given location
///////////////////

function SpawnNPCAtLocation (npctemplate, x, y, z := "guess", specialparms := {})
	var realtemplate := FindPackage (npctemplate);
	if (!realtemplate)
		Syslog ("Error finding NPC template - " + npctemplate);
		return "error";
	endif

	if (z = "guess")
		z := GetMapInfo (x,y).z;
	endif

	var npc := CreateNPCFromTemplate (realtemplate, x, y, z, specialparms);
	return npc;
endfunction




///////////////////
//  figures out what package the NPC is in
///////////////////

function FindPackage (template)

	template := TrimTemplate (template);
	foreach pkgname in (polcore().packages)
		var cfg := ReadConfigFile (":" + pkgname + ":npcdesc");
		if (cfg)
			if (FindConfigElem (cfg, template))
				return CSTR (":" + pkgname + ":" + template);
			endif
		endif
	endforeach

	var cfg := ReadConfigFile ("::npcdesc");
	if (FindConfigElem (cfg, template))
		return template;
	endif

	return 0;

endfunction




///////////////////
//  tries to create an NPC in the area given
///////////////////

function SpawnNPCInArea (npctemplate, given_x, given_y, range := 4, maxtries := 20)

	var tries := 0;
	var npc := 0;
	var x, y;
	while (!npc and tries < maxtries)
		x := given_x + RandomInt (2*range+1) - range;
		y := given_y + RandomInt (2*range+1) - range;
		npc := SpawnNPCAtLocation (npctemplate, x, y);
		if (npc = "error")
			return 0;
		endif
		if (npc)
			return npc;
		endif
		tries := tries + 1;
	endwhile

	return 0;

endfunction
