use uo;
use os;
use util;

include ":datafile:datafile";

/*
 * SS_SelectCoordsInArea(x1, y1, x2, y2)
 *
 * Purpose
 * Takes in four numbers (two coordinates) and
 * picks a random coordinate within the area they create.
 *
 * Parameters
 * x1:	The top-left spot of the X axis.
 * y1:	The top-left spot of the Y axis.
 * x2:	The bottom-right spot of the X axis.
 * y2:	The bottom-right spot of the Y axis.
 *
 * Return value
 * A structure containing an X and Y property.
 *
 */
function SS_SelectCoordsInArea(x1, y1, x2, y2)
	var coord := struct;
	coord.+x := RandomInt(x2-x1)+x1;
	coord.+y := RandomInt(y2-y1)+y1;

	return coord;
endfunction

/*
 * SS_
 */