/*
        Maintened by *Edwards
         
        2010-03-10
*/

use uo;
use os;

// Constants for the RemoveSpaces() function.
CONST CLR_LEADING_SPACES 	:= 0x1;
CONST CLR_TRAILING_SPACES 	:= 0x2;
CONST CLR_DOUBLE_SPACES		:= 0x4;
CONST CLR_ALL_SPACES		:= 0x8;

// Constants for the RemoveFromString() function.
CONST STR_DEL_ONCE		:= 0x1;
CONST STR_DEL_ALL		:= 0x2;
CONST STR_DEL_LEADING		:= 0x4;
CONST STR_DEL_TRAILING		:= 0x8;

function TruncateArticle( text )
	text := CStr(text);
	var i := Len(text);

	if ( Lower(text[1, 2]) == "a " )
		text := text[3, i];
	elseif ( Lower(text[1, 3]) == "an " )
		text := text[4, i];
	endif

	return text;
endfunction

function AddArticle( text )
	var vowels := array{"a", "e", "i", "o", "u"};
	if ( Lower(text[1]) in vowels )
		text := "an "+text;
	else
		text := "a "+text;
	endif

	return text;
endfunction

function UCFirst( text )
	text := CStr(text);

	if ( Len(text) > 1 )
		text := Lower(text);
		text[1] := Upper(text[1]);
	endif

	return text;
endfunction

function LCFirst( text )
	text := CStr(text);

	if ( Len(text) > 1 )
		text := Lower(text);
		text[1] := Lower(text[1]);
	endif

	return text;
endfunction

function RemoveFromString( text, remove, flags:=STR_DEL_ALL )
	text := CStr(text);
	remove := CStr(remove);

	if ( flags == STR_DEL_ALL )
		// Remove all instances of 'remove'
		while ( text[remove] )
			text[remove] := "";
			SleepMs(2);
		endwhile
	elseif ( flags == STR_DEL_ONCE )
		// Only remove first instance of 'remove'
		// Really its just better to do text[replace] := "" in a script
		// but this is here since it is used for leading/trailing removes.
		text[remove] := "";
	else
		var remove_len := Len(remove);
		if ( flags & STR_DEL_LEADING )
			// Remove from the begining of 'text' only.
			while ( text[1, remove_len] == remove )
				text[1, remove_len] := "";
				SleepMs(2);
				if ( flags & STR_DEL_ONCE )
					// Remove only first instance of 'remove' from the begining.
					break;
				endif
			endwhile
		endif

		if ( flags & STR_DEL_TRAILING )
			// Remove from the end of 'text' only.
			var end_start := (Len(text) - remove_len)+1;
			while ( text[end_start, Len(text)] == remove )
				text[end_start, Len(text)] := "";
				SleepMs(2);
				if ( flags & STR_DEL_ONCE )
					// Remove only first instance of 'remove' from the ending.
					break;
				endif
				end_start := (Len(text) - remove_len)+1;
			endwhile
		endif
	endif

	return text;
endfunction

function RemoveSpaces( text, flags:=CLR_LEADING_SPACES )
	if ( flags & CLR_ALL_SPACES )
		while ( text[" "] )
			text[" "] := "";
			SleepMS(2);
		endwhile
	else
		if ( flags & CLR_LEADING_SPACES )
			while ( text[1] == " " || CAsc(text[1]) == 0 )
				text[1] := "";
				SleepMS(2);
			endwhile
		endif

		if ( flags & CLR_TRAILING_SPACES )
			while ( text[Len(text)] == " " || CAsc(text[Len(text)]) == 0 )
				text[Len(text)] := "";
				SleepMS(2);
			endwhile
		endif

		if ( flags & CLR_DOUBLE_SPACES )
			while ( text["  "] )
				text["  "] := " ";
				SleepMS(2);
			endwhile
		endif
	endif

	return text;
endfunction

function Check_ValidChar( text, allow_space:=0 )

         //Make sure you check a text without spaces or it will fail.
         var valid_char := array{ "a", "b", "c", "d", "e", "f", "g",
                                  "h", "i", "j", "k", "l", "m", "n",
                                  "o", "p", "q", "r", "s", "t", "u",
                                  "v", "w", "x", "y", "z", "A", "B",
                                  "C", "D", "E", "F", "G", "H", "I",
                                  "P", "O", "N", "M", "L", "K", "J",
                                  "Q", "R", "S", "T", "U", "V", "W",
                                  "X", "Y", "Z", "1", "2", "3", "4",
                                  "5", "6", "7", "8", "9", "0" };

         var len_text := Len( text );

         var i;
         for( i:=1; i<=len_text; i+=1 )
                  SleepMS(5);
                  var letter := CStr( text[i] );
                  if( letter in valid_char )
                           continue;
                  elseif( allow_space && ( text[i] == " " || CAsc( text[i] ) == 0 ))
                           continue;
                  endif
                  
                  return 0;
         endfor
         
         return 1;
endfunction
