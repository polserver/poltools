/*
        Maintened by *Edwards
        
        2009-10-21
*/
use os;
use util;
use cfgfile;

function RandomFoodObjType( category:=0 )

	var cfg_file := ReadConfigFile( ":fooddrink:food" );
	if( cfg_file.errortext )
		return error{"errortext":="Unable to read food.cfg ->"+cfg_file.errortext};
	endif
	
	var food_list := array{};
	if( category )
		var cfg_elem := cfg_file[category];
		food_list := GetConfigStringArray( cfg_elem, "Food" );
	else
		foreach elem_name in ( GetConfigStringKeys( cfg_file ))
			var cfg_elem := cfg_file[elem_name];
			var temp_list := GetConfigStringArray( cfg_elem, "Food" );
			if( temp_list.Size() > 0 )
				food_list += temp_list;
			endif
			SleepMS(2);
		endforeach
	endif
	
	if( food_list.Size() > 0 )
		return CInt( food_list.RandomEntry() );
	else
		return error{"errortext":="No food items found in food.cfg"};
	endif
endfunction
