/** $Id$
 *
 * Purpose
 * Provide various functions for array handling.
 *
 */
use basic;
use math;
use os;
use uo;

/*
 * GetArrayRange(the_array, start, stop)
 *
 * Purpose
 * Retrieves a range from the array.
 *
 * Parameters
 * the_array:	The array to extract data from.
 * start:	Start index to get data from.
 * stop:	End index to get data from.
 *		If stop is 0, will go until the end of the array is reached.
 *
 * Return value
 * Returns an array on success.
 * Returns an error on failure.
 *
 */
function GetArrayRange(byref the_array, start, stop:=0)
	if ( Lower(TypeOf(the_array)) != "array" )
		return error{"errortext":="Object passed was not an array."};
	endif

	start := CInt(start);
	stop := CInt(stop);

	if ( start < 1 )
		start := 1;
	endif
	if ( !stop )
		stop := the_array.Size();
	elseif ( stop > the_array.Size() )
		stop := the_array.Size();
	endif

	var new_array := array{};
	for ( start; start<=stop; start:=start+1 )
		new_array.Append(the_array[start]);
		SleepMS(2);
	endfor

	return new_array;
endfunction

/*
 * BreakArray(the_array, size)
 *
 * Purpose
 * Breaks an array up into smaller arrays.
 *
 * Parameters
 * the_array:	The array that will be broken up.
 * size:	Size of the new arrays.
 *
 * Return value
 * Returns an array of arrays
 *
 */
function BreakArray(byref the_array, size:=1)
	if ( Lower(TypeOf(the_array)) != "array" )
		return error{"errortext":="Object passed was not an array."};
	endif

	if ( the_array.Size() > size )
		var new_arrays := array();
		var arrays_needed := Ceil(CDbl(the_array.Size()) / CDbl(size));

		var i := 1;
		var start := i;
		var stop := size;
		for ( i; i<=arrays_needed; i+=1 )
			new_arrays.Append(GetArrayRange(the_array, start, stop));
			start := stop+1;
			stop += size;
			SleepMS(2);
		endfor

		return new_arrays;
	else
		return array{the_array};
	endif
endfunction

/*
 * FindInArray(byref the_array, find_what)
 *
 * Purpose
 * Finds the first instance of something in an array.
 *
 * Parameters
 * the_array:	Array to search.
 * find_what:	What to look for.
 *
 * Return value
 * Returns > 1 (the index) if it is found.
 * Returns 0 if 'find_what' was not found.
 *
 */
function FindInArray(byref the_array, find_what)
	if ( Lower(TypeOf(the_array)) != "array" )
		return error{"errortext":="Object passed was not an array."};
	endif

	foreach index in ( the_array )
		if ( index == find_what )
			return _index_iter;
		endif
		SleepMS(2);
	endforeach

	return 0;
endfunction
