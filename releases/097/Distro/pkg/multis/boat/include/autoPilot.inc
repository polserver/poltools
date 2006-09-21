/* $Id$
 *
 */

// Bresenham's line drawing algorithm. Determines a line between two waypoints.
// Used here to get the next coordinate to move.
function BresenhamLine(x0, y0, x1, y1)
	var dy := y1 - y0;
	var dx := x1 - x0;
	var stepx, stepy;

	if ( dy < 0 )
	  	dy := -dy;
	   	stepy := -1;
	else
	  	stepy := 1;
	endif

	if ( dx < 0 )
	   	dx := -dx;
	   	stepx := -1;
	else
	  	stepx := 1;
	endif

	dy *= 2;
	dx *= 2;

	if ( dx > dy )
		var fraction := 2 * dy - dx;
		while ( x0 != x1 )
			if ( fraction >= 0 )
				y0 += stepy;
				fraction -= dx;
		   	endif

			x0 += stepx;
			fraction += dy;

			return struct{"x":=x0, "y":=y0};
		endwhile
	else
		var fraction := 2 * dx - dy;
		while ( y0 != y1 )
			if ( fraction >= 0 )
				x0 += stepx;
				fraction -= dy;
			endif

			y0 += stepy;
			fraction += dx;

			return struct{"x":=x0, "y":=y0};
		endwhile
	endif

	return error;
endfunction

// convert facing 0-7 to turn code 0 - 3
function DetermineTurnCode(boat, desired)
	var current := CInt(boat.facing / 2);
	desired := CInt(desired / 2);

	if ( desired < current )
		desired := desired + 4;
	endif

	return (desired - current);
endfunction
