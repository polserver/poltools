/* $Id$
 *
 */

use uo;
use util;

/*
 * RandomIntRange(min, max)
 *
 * Purpose
 * Generates a random number >= min and <= max
 *
 * Parameters
 * min:	The minimum number that can be generated.
 * max: The maximum number that can be generated.
 *
 * Return Value:
 * Returns an integer on success.
 *
 */
function RandomIntRange(min:=0, max:=1)
	if ( min < 0 || max < 0 )
		return error{"errortext":="Values must be >= 0"};
	elseif ( max <= min )
		return error{"errortext":="Min is not < max."};
	endif

	var difference := CInt(max) - CInt(min);
	var random := RandomInt(difference);

	return ( random + min );
endfunction

/*
 * RandomFloatRange(min, max)
 *
 * Purpose
 * Generates a random number >= min and <= max
 *
 * Parameters
 * min:	The minimum number that can be generated.
 * max: The maximum number that can be generated.
 *
 * Return Value:
 * Returns an integer on success.
 *
 */
function RandomFloatRange(min:=0.0, max:=1.0)
	if ( min < 0 || max < 0 )
		return error{"errortext":="Values must be >= 0"};
	elseif ( max <= min )
		return error{"errortext":="Min is not < max."};
	endif

	var difference := CDbl(max) - CDbl(min);
	var random := RandomFloat(difference);

	return ( random + min );
endfunction
