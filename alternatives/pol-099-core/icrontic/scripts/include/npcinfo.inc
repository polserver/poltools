function IsAnUndeadCreature(npctemplate)
	var NpcElem := GetNpcTemplateElem (npctemplate);
	if (CINT (NpcElem.undead))
		return CINT(NpcElem.undead);
	endif

	return 0;
endfunction

function HasNoCorpse (npctemplate)

	var NpcElem := GetNpcTemplateElem ( npctemplate );
	if (NpcElem.nocorpse)
		return 1;
	endif

	return 0;

endfunction

function IsHostile (byref mob)
	if (!mob.npctemplate)
		return 0; 
	endif
	
	if (mob.script ["tamed"])
		var masterserial := GetObjProperty (mob, "master");
		if (!masterserial)
			return 0;
		endif
		var master := SystemFindObjectBySerial (masterserial);
		if (!master or !master.cmdlevel)
			return 0;
		endif
		
		if (master.murderer)
			return 1;
		endif
		return 1;
	endif

	var NpcElem := GetNpcTemplateElem ( mob.npctemplate );
	if (NpcElem.hostile)
		return 1;
	endif

	return 0;
endfunction

function IsGood (byref mob)
	if (!mob.npctemplate or mob.script["guard"])
		return 1;
	endif
	
	if (mob.script ["tamed"])
		var masterserial := GetObjProperty (mob, "master");
		if (!masterserial)
			return 1;
		endif
		var master := SystemFindObjectBySerial (masterserial);
		if (!master or !master.cmdlevel)
			return 1;
		endif
		
		if (master.murderer)
			return 0;
		endif
		return 1;
	endif

	if ( mob.script = "humuc" or mob.script["playermerchant"] )
		return 0;
	endif

	var NpcElem := GetNpcTemplateElem (mob.npctemplate);
	if (NpcElem.alignment = "good")
		return 1;
	endif

	return 0;
endfunction

function GetNpcTemplateElem (npctemplate)
	if ( npctemplate[":"] )
		npctemplate[":"] := "";
		npctemplate := npctemplate[(find(npctemplate, ":", 1)+1), len(npctemplate)];
	endif
	var cfg := ReadConfigFile (":*:npcdesc");
	return FindConfigElem (cfg, npctemplate);
endfunction

function TrimTemplate(npctemplate)
	if ( npctemplate[":"] )
		npctemplate[":"] := "";
		npctemplate := npctemplate[(find(npctemplate, ":", 1)+1), len(npctemplate)];
	endif
	return npctemplate;
endfunction

function FindNpcDesc (npctemplate)
	if ( npctemplate[":"] )
		var npcdesc := npctemplate[1,find(npctemplate, ":", 2)];
		return npcdesc + "npcdesc";
	else
		return "::npcdesc";
	endif
endfunction

function FindNPCPackage (template)
	template := TrimTemplate (template);
	var cfg := ReadConfigFile ("::npcdesc");
	if (FindConfigElem (cfg, template))
		return "none";
	endif

	foreach pkgname in (polcore().packages)
		cfg := ReadConfigFile (":" + pkgname + ":npcdesc");
		if (cfg)
			if (FindConfigElem (cfg, template))
				return CSTR (pkgname);
			endif
		endif
	endforeach

	return 0;
endfunction
