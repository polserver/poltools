// $Id: fields.inc 1172 2008-02-05 20:09:05Z AustinHeilman $

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
