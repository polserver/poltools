//////////////////
//
// NPC_Nerves.inc
// 
// Created: 07/09/2001 15:42
// Updated: 7/19/2002 2:33PM
//
// by Austin Heilman and TJ Houston
// http://www.tsse.net
//
/////////////////
//
// This include is used by the brain and by nerves
// to manage the various nerves that interract with
// the brain. This is like the "switch board" of the AI
//
////////////////

use uo;
use os;

var g_nerves := dictionary;

function LoadNerves(mobile)
	var g_nerves := GetObjProperty(mobile, "#NerveInfo");
	
	if ( !g_nerves )
		g_nerves := dictionary;
	endif
	
	return g_nerves;
endfunction

function SaveNerves(mobile:=0)
	if ( !mobile )
		mobile := self();
	endif
	SetObjProperty(mobile, "#NerveInfo", g_nerves);
endfunction

function StartNerve(nerve_name, nerve_script, data:=array)
	var process := GetNerve(nerve_name);
	if (process != error)
		return error;
	endif
	
	if ( lower(TypeOf(data)) != "array")
		data := array(data);
	endif
	
	data.insert(1, nerve_name);
	process := start_script(nerve_script, data);
	if ( process.errortext )
		SysLog("g_nervescript ["+nerve_name+"]["+nerve_script+"] failed to start! ->"+process.errortext);
	else
		AddNerve(nerve_name, process);
	endif
	return process;
endfunction

function GetNerve(nerve_name)
	if ( g_nerves.exists(nerve_name) )
		return GetProcess(g_nerves[nerve_name]);
	else
		var temp := error;
		temp.+errortext := "No nerve exists by the name " + nerve_name;
		return temp;
	endif
endfunction

function AddNerve(nerve_name, byref process)
	// Check if a nerve under that name is already running.
	KillNerve(nerve_name);
	g_nerves[nerve_name] := process.pid;
	
	SaveNerves();
endfunction

function KillNerve(nerve_name)
	// Returns '1' if the nerve was found and stopped.
	if ( g_nerves.exists(nerve_name) )
		GetProcess(g_nerves[nerve_name]).kill();
		g_nerves.erase(nerve_name);
		
		SaveNerves();
		return 1;
	else
		return 0;
	endif
endfunction

function StopNerves()
	foreach key in (g_nerves.keys())
		KillNerve(key);
	endforeach
	
	SaveNerves();
endfunction
