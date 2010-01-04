// String is passed as an array
// Max 3 lines per statement (not sure if this applies to items also, if not then
// the error checking delimiter needs to be adjusted)

use npc;
use uo;
use os;

function centerarray(phrase)

	// splits params into an array (max 3 lines per call)
	var words := SplitWords( phrase );

	// initialize the return array
	var point := {};

	// Error checking, if array is longer then 3 elements, return error
	if ( words.size() > 3)
		point.append( "Error");
		return point;
	endif

	// width of a single line
	var linelength := 17;

	//set space per word in the array
	foreach word in words

		var ctrwrd;
		var spaces := "";
		var lng := len(word);
		lng := lng - (lng /4);
		var blankspaces := ((linelength / 2) - lng);

		for a := 1 to blankspaces
			spaces := spaces + " ";
		endfor

		ctrwrd := spaces + word;

		point.append( ctrwrd );
		
	endforeach

	//returns an array
	return point;

endfunction