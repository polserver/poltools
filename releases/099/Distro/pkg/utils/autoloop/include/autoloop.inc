/*
        Maintened by *Edwards
        
        2009-08-15
*/
use uo;

include ":autoloop:settings";

var mobile_x;
var mobile_y;
var loop_left;

function SendAutoloop( mobile, loop := 0 )

        if( loop )
                if( !SetAutoLoop( mobile,loop ))
                        return 0;
                endif
        elseif( !GetAutoLoop( mobile ))
                loop := EnterAutoLoop( mobile );
                if( !loop )
                        return 0;
                elseif( !SetAutoLoop( mobile, loop ))
                        return 0;
                endif
        endif
        
        mobile_x := mobile.x;
        mobile_y := mobile.y;
        loop_left := GetAutoLoop( mobile );
        
        return 1;
endfunction

function GetAutoLoop( mobile )

	var loop := GetObjProperty( mobile, "#Autoloop" );
        if( !loop )
        	return -1;
        endif
        
        return loop;
endfunction

function SetAutoLoop( mobile, loop )

        var loop_settings := ALoop_GetSettingsCfgElem( "Settings" ),
            max_loop := CInt( loop_settings.MaxAutoLoop );
        
        if( loop > max_loop )
                loop := max_loop;
        elseif( CInt( loop ) < 1 && loop != -1 )
                SendSysMessage( mobile, "Loop amount is out of range." );
                return 0;
        endif
        
        SetObjProperty( mobile, "#Autoloop", loop );
        
        return 1;
endfunction

function AutoLoop( mobile, loop_delay := 3 )

	if(( mobile_x == mobile.x ) && ( mobile_y == mobile.y ))
		if( CInt( loop_left ) > 0 )
                        loop_left := CInt( loop_left );
			SendSysMessage( mobile, "Looping ["+loop_left+" more to go].", 3, 89 );
                        if( loop_left != GetAutoLoop( mobile ))
				Sleep( loop_delay );
                        endif
			loop_left -= 1;
                        return 1;
                elseif( loop_left == -1 )
			SendSysMessage( mobile, "Looping.", 3, 89 );
			Sleep( loop_delay );
			return 1;
		endif
	else
		SendSysMessage( mobile, "You moved.", 3, 89 );
		return 0;;
	endif

	return 0;
endfunction

function EndAutoLoop( mobile )

	if( loop_left != -1 && loop_left < 1 )
		SendSysMessage( mobile, "You finished looping.", 3, 89 );
	else
		SendSysMessage( mobile, "Looping aborted.", 3, 89 );
	endif

	return 1;
endfunction

function EnterAutoLoop( mobile )

        var loop_settings := ALoop_GetSettingsCfgElem( "Settings" ),
            max_loop := CInt( loop_settings.MaxAutoLoop );
            
	var loop := CInt( SendTextEntryGump( mobile, "How many loops?", TE_CANCEL_ENABLE, TE_STYLE_NORMAL, max_loop, "[0-"+max_loop+"] [-1 means infinite]" ));
	if( !loop )
		return 0;
        endif
        
        return loop;
endfunction
