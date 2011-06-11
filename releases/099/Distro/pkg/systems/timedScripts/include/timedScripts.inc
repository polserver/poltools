/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-02-11
*/
use uo;
use cfgfile;
use os;

include ":security:report";

// Event flags
CONST EVENT_ADD_TIMER			:= 0x1;
CONST EVENT_LOWER_TIMER			:= 0x2;
CONST EVENT_PLAYER_DIED			:= 0x4;
CONST EVENT_NPC_DIED			:= 0x8;
CONST EVENT_LOGOFF			:= 0x16;

// Constants
CONST NOSTART_CONTROLLER		:= 0x0;
CONST START_CONTROLLER			:= 0x1;

function TS_GetCfgFile()

	var cfg := ReadConfigFile( ":timedscripts:timedScripts" );

	if( cfg.errortext )
		ReportText( "Error::TS_GetSettingsCfgFile() - Unable to open [:timedscripts:timedScripts] ->"+cfg.errortext, REPORT_SYSLOG );
	endif

	return cfg;
endfunction

function TS_GetCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := TS_GetCfgFile();
	endif

	var elem := cfg_file[elem_name];

	if( elem.errortext )
		ReportText( "Error::TS_GetCfgElem - Unable to find elem ["+elem_name+"] ->"+elem.errortext, REPORT_SYSLOG );
	endif

	return elem;
endfunction

function TS_GetControllerProcess( mobile, start:=NOSTART_CONTROLLER )

	var pid := CInt(GetObjProperty( mobile, "#TimerControlPID" ));
	var process := GetProcess( pid );
	if( !process )
		if( start )
			process := TS_StartController( mobile );
		endif
	endif

	return process;
endfunction

function TS_StartController( mobile )

	if( TS_GetControllerProcess( mobile, NOSTART_CONTROLLER ))
		return error{"errortext":="Error::TS_StartController() - Controller is already running."};
	endif

	var script := Start_Script( ":timedscripts:timerControl", mobile );
	if( script.errortext )
		ReportText( "Error::TS_StartController() ->"+script.errortext, REPORT_SYSLOG );
	endif

	return script;
endfunction

function TS_GetTimerList( mobile )

	var timer_list := GetObjProperty( mobile, "%TimerList%" );
	if( Lower( TypeOf( timer_list )) != "dictionary" )
		timer_list := dictionary;
	endif

	return timer_list;
endfunction

function TS_SaveTimerList( mobile, byref timer_list )

	return SetObjProperty( mobile, "%TimerList%", timer_list );
endfunction

function TS_GetTimerNames( mobile, type:="" )

	type := Lower( type );
	var timer_list := TS_GetTimerList( mobile );
	var name_list := array{};

	foreach timer_name in ( timer_list.Keys() )
		if( type )
			var elem_info := TS_GetCfgElem( timer_name );
			if( elem_info.errortext )
				ReportText( "Error::TS_GetTimerNames() - Unable to find config elem ["+timer_name+"] - "+elem_info.errortext, REPORT_SYSLOG );
				return 0;
			elseif( Lower( elem_info.Type ) == type )
				name_list.Append( timer_name );
			endif
		else
			name_list.Append( timer_name );
		endif
		SleepMS(2);
	endforeach

	return name_list;
endfunction

function TS_GetTimerInfo( mobile, timer_name )

	var timer_list := TS_GetTimerList( mobile );
	if( timer_list.Exists( timer_name ))
		return timer_list[timer_name];
	else
		return error{"errortext":="Error::TS_GetTimerInfo() - No timer exists by that name."};
	endif
endfunction

function TS_GetTimerDuration( mobile, timer_name )

	var timer := TS_GetTimerInfo( mobile, timer_name );
	if( !timer )
		return timer; // Returns the error
	endif

	return( timer.endtime - ReadGameClock() );
endfunction

function TS_StartTimer( mobile, timer_name, duration, level:=0, attacker:=0 )

	duration := CInt( duration );
	if( duration < 1 )
		return error{"errortext":="Error::TS_StartTimer() - Duration must be greater than 0."};
	endif

	var event := struct;
	event.+type := EVENT_ADD_TIMER;
	event.+name := CStr(timer_name);
	event.+amount := duration;
	event.+level := CInt(level);
	event.+attacker := attacker;

	var process := TS_GetControllerProcess( mobile, START_CONTROLLER );
	return process.SendEvent( event );
endfunction

function TS_LowerDuration( mobile, timer_name, amount )

	var event := struct;
	event.+type := EVENT_LOWER_TIMER;
	event.+name := CStr( timer_name );
	event.+amount := CInt( amount );

	var process := TS_GetControllerProcess( mobile );
	return process.SendEvent( event );
endfunction

function TS_LogOff( mobile )

	if( mobile.IsA( POLCLASS_NPC ))
		return error{"errortext":="NPCs can't log off."};
	endif

	var event := struct;
	event.+type := EVENT_LOGOFF;

	var process := TS_GetControllerProcess( mobile );
	return process.SendEvent( event );
endfunction

function TS_Death( mobile )

	var event := struct;
	if( mobile.IsA( POLCLASS_MOBILE ))
		event.+type := EVENT_PLAYER_DIED;
	elseif( mobile.IsA( POLCLASS_CORPSE ))
		event.+type := EVENT_NPC_DIED;
	endif

	var process := TS_GetControllerProcess( mobile );
	return process.SendEvent( event );
endfunction

function TS_WipeMods( mobile )

        var timers := { "Clumsy", "FeebleMind", "Weaken", "Agility", "Cunning", "Protection",
                        "Strength", "Bless", "Curse", "Polymorph", "EarthBless", "Liche", "Wraith",
                        "spiritspeak", "ReactiveArmor", "SpeedBoost" };
            
        foreach timer in timers
                TS_LowerDuration( mobile, timer, -1 );
                SleepMS(5);
        endforeach
            
        return 1;
endfunction
