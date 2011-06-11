/*
        Maintened by *Edwards
        
        2009-12-12
*/
use uo;
use os;
use math;

function TurnObjectToward( object, x_pos, y_pos )

	var orig := object.facing;
	object.facing := GetFacing( object.x, object.y, x_pos, y_pos );

	return orig;
endfunction

function IsFacing( mobile, x, y )

	return( mobile.facing == GetFacing( mobile.x, mobile.y, x, y ));
endfunction

function IsBehind( tgt_mob, locx, locy )

	var frot := Abs((( tgt_mob.facing + 4 ) % 8 ) - GetFacing( tgt_mob.x, tgt_mob.y, locx, locy ));
	if( frot > 4 )
		frot := 8 - frot;
	endif

	return( frot < 2 );
endfunction

function GetPointsByFacing( x_pos, y_pos, facing, dist )

	var coord := struct{"x", "y"};
	var coord_list := array{};

	var xmod := 0, ymod := 0;
	case( facing ) // Should find a more sleek mathish way to do this!
		0: xmod := 0;  ymod := -1; break;
		1: xmod := +1; ymod := -1; break;
		2: xmod := +1; ymod := 0;  break;
		3: xmod := +1; ymod := +1; break;
		4: xmod := 0;  ymod := +1; break;
		5: xmod := -1; ymod := +1; break;
		6: xmod := -1; ymod := 0;  break;
		7: xmod := -1; ymod := -1; break;
		default:
			return error{"errortext":="Invalid facing."};
			break;
	endcase

	var i;
	for( i:=1; i<=dist; i+=1 )
		coord.x := x_pos+( i * xmod );
		coord.y := y_pos+( i * ymod );
		coord_list.Append( coord );
		SleepMS(2);
	endfor

	return coord_list;
endfunction
