use uo;
use cfgfile;

function isInInn( who )

	var inn_config := ReadConfigFile( ":areas:inns-"+who.realm );
	var inn_locations := GetConfigStringKeys( inn_config );
 
	foreach inn in inn_locations
		var element :=  inn_config[inn];
		var rect := element.rect;
		rect := SplitWords( rect );
		if(( who.x >= CInt( rect[1] )) && ( who.x <= CInt( rect[3] )) && ( who.y >= CInt( rect[2] )) && ( who.y <= CInt( rect[4] )))
			return 1;
			break;
		endif
                SleepMS(5);
	endforeach

	return 0;
endfunction
