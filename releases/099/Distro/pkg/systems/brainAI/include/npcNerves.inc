/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-10
*/
use uo;
use os;

include ":brainAI:npcError";


/////////////////////////////////////////////////////////////////
// * Function List *
//
// GetNerveFlags(mobile, nerve_name)
// GetNerveProcess(mobile, nerve_name)
// KillNerves(mobile)
// LoadNerves(mobile)
// SaveNerves(mobile, nerve_dict)
// StartNerve(mobile, nerve_name, nerve_script, data:=array, flags:=NF_NO_FLAGS)
// StopNerve(mobile, nerve_name)
//
/////////////////////////////////////////////////////////////////

// - Nerve script has no special settings.
CONST NF_NO_FLAGS		:= 0x0;

// - Nerve script will not be stopped when the npc goes to sleep If an idle script is present
CONST NF_CONTINUE_ON_SLEEP	:= 0x1;

// - Brain will send an NPCEVENT_DONE event when it completes a command sent by a nerve script.
CONST NF_INFORM_ON_COMPLETION	:= 0x2;

function LoadNerves( mobile )

	var nerve_dict := GetObjProperty( mobile, "#NerveInfo" );
	if ( Lower( TypeOf( nerve_dict )) != "dictionary" )
		nerve_dict := dictionary;
	endif

	return nerve_dict;
endfunction

function SaveNerves( mobile, nerve_dict )

	return SetObjProperty( mobile, "#NerveInfo", nerve_dict );
endfunction

function GetNerveProcess( mobile, nerve_name )

	var nerve_dict := LoadNerves( mobile );
	if( nerve_dict.Exists( nerve_name ))
		return GetProcess( CInt( nerve_dict[nerve_name].pid ));
	else
		return error{"errortext" := "Error::GetNerveProcess() - Invalid nerve name  ["+nerve_name+"]"};
	endif
endfunction

function GetNerveNameByPid( mobile, pid )

	foreach nerve in ( LoadNerves( mobile ) )
		if( nerve.pid == pid )
			return _nerve_iter;
		endif
		SleepMS(2);
	endforeach

	return error{"errortext":="Error::GetNerveNameByPid() - No nerve with that PID exists."};
endfunction

function GetNerveFlags( mobile, nerve_name )

	var nerve_dict := LoadNerves( mobile );
	if( nerve_dict.Exists( nerve_name ))
		return nerve_dict[nerve_name].flags;
	else
		return error{"errortext" := "Error::GetNerveFlags() - Invalid nerve name  ["+nerve_name+"]"};
	endif
endfunction

function StartNerve( mobile, nerve_name, nerve_script, data:=array, flags:=NF_NO_FLAGS )

        if( GetNerveProcess( mobile, nerve_name ))
                return 0;
        endif
        
	if( Lower( TypeOf( data )) != "array")
		data := array{data};
	endif
 
	//Mobile is always the begining of the data array.
	data.Insert( 1, mobile );
 
	//Nerve name is always the second part of the data array.
	data.Insert( 2, nerve_name );

	var process := Start_Script( nerve_script, data );
	if( process.errortext )
		var errmsg := "Error::StartNerve() - ["+nerve_name+"] ["+nerve_script+"] failed to start! ->"+process.errortext;
		NPC_ErrorSysLog( mobile, errmsg );
		return error{"errortext":=errmsg};
	else
		var nerve_dict := LoadNerves( mobile );
		if( nerve_dict.Exists( nerve_name ))
			StopNerve( mobile, nerve_name );
		endif

		var nerve_info := struct;
		nerve_info.+pid := process.pid;
		nerve_info.+flags := flags;
		nerve_dict[nerve_name] := nerve_info;

		SaveNerves( mobile, nerve_dict );

		return process;
	endif
endfunction

function StopNerve( mobile, nerve_name )

	var nerve_dict := LoadNerves( mobile );
	if( nerve_dict.Exists( nerve_name ))
		var process := GetNerveProcess( mobile, nerve_name );
                if( process )
                	process.Kill();
                endif
                
		nerve_dict.Erase( nerve_name );
		return SaveNerves( mobile, nerve_dict );
	else
		return error{"errortext" := "Error::StopNerve() - Invalid nerve name  ["+nerve_name+"]"};
	endif
endfunction

function KillNerves( mobile )

	var nerve_dict := LoadNerves( mobile );

	foreach nerve_name in ( nerve_dict.Keys() )
		StopNerve( mobile, nerve_name );
	endforeach

	return 1;
endfunction
