/* $Id: util_extend.inc 1110 2008-01-07 04:13:52Z AustinHeilman $
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
 * min_val:	The minimum number that can be generated.
 * max_val: The maximum number that can be generated.
 *
 * Return Value:
 * Returns an integer on success.
 *
 */
function RandomIntRange(min_val:=0, max_val:=1)
	if ( min_val < 0 || max_val < 0 )
		return error{"errortext":="Values must be >= 0"};
	elseif ( max_val <= min_val )
		return error{"errortext":="Min is not < max."};
	endif

	var difference := CInt(max_val) - CInt(min_val);
	var random := RandomInt(difference);

	return ( random + min_val );
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
function RandomFloatRange(min_val:=0.0, max_val:=1.0)
	if ( min_val < 0 || max_val < 0 )
		return error{"errortext":="Values must be >= 0"};
	elseif ( max_val <= min_val )
		return error{"errortext":="Min is not < max."};
	endif

	var difference := CDbl(max_val) - CDbl(min_val);
	var random := RandomFloat(difference);

	return ( random + min_val );
endfunction
