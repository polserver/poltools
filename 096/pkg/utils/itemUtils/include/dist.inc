/* $Id: dist.inc 903 2005-11-05 08:10:21Z muaddiblsd $
 *
 */
/*
 *  dist( obj1, obj2 )
 *      - returns the largest of the x-distance and y-distance
 *        between two objects.
 */

use uo;

function coordist( x1, y1, x2, y2 )
	var xd := x1 - x2;
	var yd := y1 - y2;
	if (xd < 0)
		xd := -xd;
	endif
	if (yd < 0)
		yd := -yd;
	endif
	if (xd > yd)
		return xd;
	else
		return yd;
	endif
endfunction