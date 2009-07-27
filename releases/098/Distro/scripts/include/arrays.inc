/** $Id: arrays.inc 876 2006-10-17 10:13:18Z austinheilman $
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
 * ImplodeArray(glue, array)
 *
 * Purpose
 * Returns a string containing the array elements in the same order with
 * the 'glue' string in between each array element.
 *
 * Parameters
 * glue:	String to place between each array element.
 * array:	Array to turn into a string.
 *
 * Return Value
 * Returns a string on success.
 * Returns an error on failure.
 *
 */
function ImplodeArray(glue, byref the_array)
	if ( Lower(TypeOf(the_array)) != "array" )
		return error{"errortext":="Object passed was not an array."};
	endif

	var str_array := "";
	foreach element in ( the_array )
		str_array += (CStr(element)+glue);
		SleepMS(2);
	endforeach

	return str_array;
endfunction

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

	var new_arrays := array{};
	if ( the_array.Size() > size )
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
	else
		new_arrays.append(the_array);
	endif

	return new_arrays;
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

/*
 * Bubble_Sort_Array(byref data_array, pos)
 *
 * (Anyone feel free to make this description a little better, lol.)
 *
 * Purpose
 * Sorts values in an array.
 *
 * Examples
 * var test_array := Array{Array{value1, value2}};
 * Bubble_Sort_Array(test_array, 2);
 * - Would sort test_array from small to largest number of value2.
 *
 * Parameters
 * the_array:	Array to search.
 * pos:	posistion to sort from.
 *
 * Return value
 * Returns = 1 finished sorting
 *
 */
function Bubble_Sort_Array(byref data_array, pos)

	var i := 1;
	var l := data_array.Size();

	for ( i := 1; i <= l; i := i + 1 )
		var ind := data_array[i];
		var j := i;
		while ( (j > 0) && (data_array[j - 1][pos] > ind[pos]) )
			data_array[j] := data_array[j - 1];
			j := j - 1;
			SleepMS(2);
		endwhile
		if ( (data_array[j] !=  data_array[j - 1]) || (data_array[j] !=  data_array[j + 1]) )
			data_array[j] := ind;
		endif
		SleepMS(2);
	endfor

	return 1;

endfunction
