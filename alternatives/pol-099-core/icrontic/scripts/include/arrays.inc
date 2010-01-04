//////////////////////////////////////////////////////////////////////////
//
// Arrays.inc
//
// Contains some useful functions for arrays.
//
// Originally uploaded by Shilohen.
// One function added by Madman Across the Water.
//
//////////////////////////////////////////////////////////////////////////

use basic;
use os;
use uo;


////////////////////////////////////////////////////////////////
// Function	GetIndex(byref the_array, byref to_find)
// Parameters	the_array containing the element
//		what to find
// Return	the element's index if found, error otherwise.
// Errortext	<Element not found>
////////////////////////////////////////////////////////////////
function GetIndex( byref the_array, byref to_find )

	var max := the_array.Size();
	if( !max )
		var ret := error;
		ret.+errortext := "Element not found";
		return ret;
	endif
	
	var ctr;
	var min := 1;
	if( the_array[1].IsA(POLCLASS_UOBJECT) )
		while( min <= max )
			ctr := Cint((min+max)/2);
			if( the_array[ctr].serial = to_find.serial )
				return ctr;
			elseif( the_array[ctr].serial < to_find.serial )
				min := ctr + 1;
			else
				max := ctr - 1;
			endif
		endwhile
		
		var ret := error;
		ret.+errortext := "Element not found";
		return ret;
	else
		while( min <= max )
			ctr := Cint((min+max)/2);
			if( the_array[ctr] = to_find )
				return ctr;
			elseif( the_array[ctr] < to_find )
				min := ctr + 1;
			else
				max := ctr - 1;
			endif
		endwhile
		
		var ret := error;
		ret.+errortext := "Element not found";
		return ret;	
	endif
	
endfunction


////////////////////////////////////////////////////////////////
// Function	InsertElementInSortedArray(byref the_array, byref to_find)
// Parameters	the_array containing the element
//		what to insert
// Return	non-zero if the element could be insert, error otherwise.
// Errortext	<Element <element> already exist in the array>
//		<Couldn't insert element <element>>
// Goal		Insert an element at the good index, must be the only one
//		instance of that element in the array, no dupe allowed.
////////////////////////////////////////////////////////////////
function InsertInSortedArray( byref the_array, byref element )

	var max := the_array.Size();
	if( !max )
		return the_array.Append(element);
	endif
	
	var ctr;
	var min := 1;
	if( the_array[1].IsA(POLCLASS_UOBJECT) )
		while( min <= max )
			ctr := Cint((min+max)/2);
			if( the_array[ctr].serial = element.serial )
				var ret := error;
				ret.+errortext := "Element " + element + " already exist in array";
				return ret;
			elseif( the_array[ctr].serial < element.serial )
				min := ctr + 1;
			else
				max := ctr - 1;
			endif
		endwhile
	else
		while( min <= max )
			ctr := Cint((min+max)/2);
			if( the_array[ctr] = element )
				var ret := error;
				ret.+errortext := "Element " + element + " already exist in array";
				return ret;
			elseif( the_array[ctr] < element )
				min := ctr + 1;
			else
				max := ctr - 1;
			endif
		endwhile
	endif

	return the_array.Insert( min, element );
	
endfunction


////////////////////////////////////////////////////////////////
// Function	RemoveElement(byref the_array, byref element)
// Parameters	the_array containing the element
//		the element to remove
// Return	1 is the elment was successfully removed
//		An error otherwise
////////////////////////////////////////////////////////////////
function RemoveElement( byref the_array, byref element )

	var index := GetIndex( the_array, element );
	if( index )
		return the_array.Erase(index);
	else
		return index;
	endif

endfunction


////////////////////////////////////////////////////////////////
// Function	SortArray(byref the_array)
// Parameters	the_array to sort
// Return	1 if the sorting was success, 0 otherwise.
// Goal		Sort the array, even array of uobject.
////////////////////////////////////////////////////////////////
function SortArray( byref the_array )

	var length := the_array.Size();
	if( length )
		if( the_array[1].IsA(POLCLASS_UOBJECT) )
			var i;
			var j;
			var temp;
			var old_prio := set_priority(50);
			for( i := 1; i < length; i := i + 1 )
				for( j := 2; j <= (length-i+1); j := j + 1 )
					if( the_array[j].serial < the_array[j-1].serial )
						temp := the_array[j];
						the_array[j]	:= the_array[j-1];
						the_array[j-1]	:= temp;
					endif
				endfor
			endfor
			set_priority(old_prio);
		else
			return the_array.Sort();
		endif
	endif
	
	return 1;
	
endfunction

//Fill_array
// you don't want to send this non-integers. Error checking isn't at the moment.
// This function is handed start and end numbers, and increments. If you send it
// (1,5,1), for instance, it will return an array {1,2,3,4,5}. (5,1,-2) will return
// {5,3,1}. (1,5,3) will return {1,4}.
// Added by Madman Across the Water

function fill_array(starts, ends, incr := 1)

	var newarray := array;
	if (incr = 0)
	     return starts;
	elseif (incr > 0)
	     var i:= 1;
	     while (1)
	         newarray[i] := starts;
	         starts := starts + incr;
	         if (starts > ends)
	         	return newarray;
	         endif
	         i := i + 1;
	     endwhile
	else
	     var i := 1;
	     while (1)
	         newarray[i] := starts;
	         starts := starts + incr;
	         if (starts < ends)
	         	return newarray;
	         endif
	         i := i + 1;
	     endwhile
	endif
	
	return 0;
endfunction

