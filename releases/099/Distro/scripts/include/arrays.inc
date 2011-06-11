/*
        Maintened by *Edwards
        
        2009-12-20
*/
use basic;
use math;
use os;
use uo;

function ImplodeArray( glue, byref the_array )

	if( Lower( TypeOf( the_array )) != "array" )
		return error{"errortext":="Object passed was not an array."};
	endif

	var str_array := "";
	foreach element in ( the_array )
		str_array += ( CStr( element ) + glue );
		SleepMS(2);
	endforeach

	return str_array;
endfunction

function GetArrayRange( byref the_array, start, stop:=0 )

	if( Lower( TypeOf( the_array )) != "array" )
		return error{"errortext":="Object passed was not an array."};
	endif

	start := CInt( start );
	stop := CInt( stop );

	if( start < 1 )
		start := 1;
	endif
 
	if( !stop )
		stop := the_array.Size();
	elseif( stop > the_array.Size() )
		stop := the_array.Size();
	endif

	var new_array := array{};
	for( start; start<=stop; start+=1 )
		new_array.Append( the_array[start] );
		SleepMS(2);
	endfor

	return new_array;
endfunction

function BreakArray( byref the_array, size:=1 )

	if( Lower( TypeOf( the_array )) != "array" )
		return error{"errortext":="Object passed was not an array."};
	endif

	var new_arrays := array{};
	if( the_array.Size() > size )
		var arrays_needed := Ceil( CDbl( the_array.Size() ) / CDbl( size ));

		var i := 1;
		var start := i;
		var stop := size;
  
		for( i; i<=arrays_needed; i+=1 )
			new_arrays.Append( GetArrayRange( the_array, start, stop ));
			start := stop+1;
			stop += size;
			SleepMS(2);
		endfor
	else
		new_arrays.append( the_array );
	endif

	return new_arrays;
endfunction

function FindInArray( byref the_array, find_what )

	if( Lower( TypeOf( the_array )) != "array" )
		return error{"errortext":="Object passed was not an array."};
	endif

	foreach index in ( the_array )
		if( index == find_what )
			return _index_iter;
		endif
		SleepMS(2);
	endforeach

	return 0;
endfunction

function RemoveFromArray( byref x_array, match )

	foreach index in ( x_array )
		if( index == match )
			x_array.Erase( _index_iter );
			break;
    		endif
      
    		SleepMS(2);
    	endforeach
     
    	return array;
endfunction
