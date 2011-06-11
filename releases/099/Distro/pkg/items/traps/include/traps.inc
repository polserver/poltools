/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-22
*/
use uo;
use cfgfile;
use util;
use os;

function GetTrapInfo( trap_name )

	var trap_cfg := ReadConfigFile( ":traps:traps" );
	if( trap_cfg.errortext )
		var temp := error{"errortxt":="GetTrapInfo()::Could not open :traps:traps.cfg ->"+trap_cfg.errortext};
		SysLog( temp.errortext );
		return temp;
	endif
	
	var trap_elem := trap_cfg[trap_name];
	if( trap_elem.errortext )
		var temp := error{"errortext":="GetTrapInfo()::Could not find elem ["+trap_name+"] -> "+trap_elem.errortext};
		SysLog( temp.errortext );
		return temp;
	endif
	
	return trap_elem;
endfunction

function AddTrap( object, trap_name, strength, count:=0 )

	var trap_list := GetObjProperty( object, "TrapList" );
	
	if( Lower( TypeOf( trap_list )) != "array" )
		trap_list := array{};
	endif
	
	var trap_elem := GetTrapInfo( trap_name );
	if( trap_elem.errortext )
		return trap_elem; // Contains an error
	endif
		
	if( !count )
		count := trap_elem.Count;
		if( !count )
			count := -1;
		//elseif ( count["d"] ) // this kept reporting false positives ??
		elseif( find( count, "d", 1 ))
			//print("There is a d in: " + count);
			count := RandomDiceRoll( trap_elem.count );
		else
			count := CInt( trap_elem.Count );
		endif
	endif
	
	var trap := struct;
	trap.+Name := trap_name;
	trap.+Strength := strength;
	trap.+Count := count;
	
	trap_list.Append( trap );
	
	SetObjProperty( object, "TrapList", trap_list );
	
	return 1;
endfunction

function RemoveTrap( object, index:="all" )

        if( index == "all" )
		EraseObjProperty( object, "TrapList" );
		EraseObjProperty( object, "TrapsRevealed" );
                return 0;
        endif
        
	var trap_list := GetObjProperty( object, "TrapList" );
	if( trap_list == error )
		return error{"errortext":="Object has no traps."};
	endif
	
	trap_list.Erase( index );
	
	if( trap_list.Size() > 0 )
		SetObjProperty( object, "TrapList", trap_list );
	else
		EraseObjProperty( object, "TrapList" );
		EraseObjProperty( object, "TrapsRevealed" );
	endif
	
	return 1;
endfunction

function RunTraps( object, mobile )

	var trap_list := GetObjProperty( object, "TrapList" );
	if( trap_list == error )
		return 0;
	endif
	
	foreach trap_info in ( trap_list )
		var cfg_info := GetTrapInfo( trap_info.name );
		
		var result := Start_Script( cfg_info.Script, {mobile, object, trap_info, _trap_info_iter} );
		if( result.errortext )
			PrintTextAbove( object, result.errortext );
			continue;
		endif
		
		if( trap_info.Count > 0 ) // -1 = permanent trap
			trap_info.Count := trap_info.Count - 1;
			
			if( trap_info.count == 0 )
				RemoveTrap( object, _trap_info_iter );
			endif
		endif
		SleepMS(2);
	endforeach
	
	return 1;
endfunction

function GetTrapList( object )

	var trap_list := GetObjProperty( object, "TrapList" );
	if( Lower( TypeOf( trap_list )) != "array" )
		trap_list := array{};
	endif
	
	return trap_list;
endfunction

function RevealTraps( object )

	if( !GetObjProperty( object, "TrapsRevealed" ))
		var trapCount := Len( GetTrapList( object ));
		if( trapCount )
			SetObjProperty( object, "TrapsRevealed", 1 );
			return trapCount;
		endif
	endif

	return 0;
endfunction

function IsTrapRevealed( object )

	return GetObjProperty( object, "TrapsRevealed" );
endfunction
