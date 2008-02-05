// $Id$

CONST FIELD_SHAPE_VERTICAL := 1;
CONST FIELD_SHAPE_HORIZONTAL := 2;

use util;

include "include/shapes";
include ":timedscripts:timedScripts";

function GetDirectionByFacing(mobile)
	case ( mobile.facing )
		0: return FIELD_SHAPE_HORIZONTAL; break; //N
		2: return FIELD_SHAPE_VERTICAL; break;   //E
		4: return FIELD_SHAPE_HORIZONTAL; break; //S
		6: return FIELD_SHAPE_VERTICAL; break;   //W
	endcase

	return (RandomInt(2)+1); // Diagonal - pick a random direction
endfunction

function GetCoordinatesForField(x, y, direction, size)
	var x2 := x, y2 := y;

	if ( direction == FIELD_SHAPE_VERTICAL )
		y -= size;
		y2 += size;
	elseif ( direction == FIELD_SHAPE_HORIZONTAL )
		x -= size;
		x2 += size;
	else
		x -= size;
		y -= size;
		x2 += size;
		y2 += size;
	endif

	return GetCoordsInRect(x, y, x2, y2);
endfunction

function CreateField(targ, shape, objtype, size, duration, cprops:="", caster:="", check_los:=1, delay:=2, skip_mobiles:=0)
	var start_cord, stop_cord, object;
	var field := array{};
	var coords := array{};

	case ( shape )
		FIELD_SHAPE_VERTICAL:
			if ( size % 2 )
				start_cord := targ.x - ((size - 1) / 2);
			else
				start_cord := targ.x - (size / 2);
			endif

			for i := start_cord to (start_cord + size - 1)
				coords.Append(struct{"x" := i, "y" := targ.y});
			endfor
			break;
		FIELD_SHAPE_HORIZONTAL:
			if ( size % 2 )
				start_cord := targ.y - ((size - 1) / 2);
			else
				start_cord := targ.y - (size / 2);
			endif

			for i := start_cord to (start_cord + size - 1)
				coords.Append(struct{"x" := targ.x, "y" := i});
			endfor
			break;
	endcase


	foreach coord in coords
		// Some fields might not be created directly on the target (wall of stone, energy field)
		if ( skip_mobiles )
			if ( (ListMobilesNearLocationEx(coord.x, coord.y, targ.z, 0, LISTEX_FLAG_NORMAL+LISTEX_FLAG_HIDDEN, targ.realm)).Size() )
				continue;
			endif
		endif

		object := CreateItemAtLocation(coord.x, coord.y, targ.z, objtype, 1, targ.realm);
		if ( object )
			// If there is no SetDuration method, set it manually
			if ( object.SetDuration(duration) == error && caster )
				TS_StartTimer(object, "destroyObject", duration, 0, caster);
			endif

			// If there is no SetOwner method, set it manually
			if ( object.SetOwner(caster.serial) == error && caster )
				SetObjProperty(field, "Owner", caster.serial);
			endif
		endif

		SleepMS(delay);
	endforeach


	// Destroy field items that caster doesn't have LOS to
	if ( field )
		if ( check_los )
			// object type doesn't matter, it'll be deleted soon
			check_los := CreateItemAtLocation(targ.x, targ.y, targ.z, 0x002, 1, targ.realm);
			check_los.invisible := 1;

			foreach object in field
				if ( !CheckLineOfSight(check_los, object) )
					DestroyItem(object);
					continue;
				endif
			endforeach

			DestroyItem(check_los);
		endif

		// Set cprops, can be array of structs or just a struct
		foreach object in field
			if ( cprops )
				if ( Lower(TypeOf(cprops)) == "array" )
					foreach prop_struct in cprops
						foreach prop_name in ( cprops.Keys() )
							SetObjProperty(object, prop_name, cprops[prop_name]);
						endforeach
					endforeach
				elseif ( Lower(TypeOf(cprops)) == "struct" )
					foreach prop_name in ( cprops.Keys() )
						SetObjProperty(object, prop_name, cprops[prop_name]);
					endforeach
				endif
			endif
		endforeach
	endif

	return 1;
endfunction