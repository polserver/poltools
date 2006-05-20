/** $Id: string.inc 1433 2006-05-18 21:48:04Z austin $
 *
 * Purpose
 * Provide various functions for string handling.
 *
 */

use uo;
use os;

// Constants for the RemoveSpaces() function.
const CLR_LEADING_SPACES 	:= 0x1;
const CLR_TRAILING_SPACES 	:= 0x2;
const CLR_DOUBLE_SPACES		:= 0x4;
const CLR_ALL_SPACES		:= 0x8;

/*
 * TruncateArticle(text)
 *
 * Purpose
 * Removes "a " or "an " from the begining of a string.
 *
 * Parameters
 * text:	Text to remove the article from.
 *
 * Return value:
 * Returns a string
 *
 */
function TruncateArticle(text)
	text := CStr(text);
	var i := Len(text);

	if ( Lower(text[1, 2]) == "a " )
		text := text[3, i];
	elseif ( Lower(text[1, 3]) == "an " )
		text := text[4, i];
	endif

	return text;
endfunction

/*
 * AddArticle(text)
 *
 * Purpose
 * Appends "a " or "an " to the begining of a string.
 * If a string begins with a consonant, it uses "a", and "an" for vowels.
 *
 * Parameters
 * text:	Text to remove the article from.
 *
 * Return value:
 * Returns a string
 *
 */
function AddArticle(text)
	var vowels := array{"a", "e", "i", "o", "u"};
	if ( Lower(text[1]) in vowels )
		text := "an "+text;
	else
		text := "a "+text;
	endif

	return text;
endfunction

/*
 * UCFirst(text)
 *
 * Purpose
 * Capitalizes the first letter in a string and sets the rest to lowercase.
 *
 * Parameters
 * text:	The text to change.
 *
 * Return value
 * Returns a string
 *
 */
function UCFirst(text)
	text := CStr(text);

	if ( Len(text) > 1 )
		text := Lower(text);
		text[1] := Upper(text[1]);
	endif

	return text;
endfunction

/*
 * RemoveSpaces(text, flags)
 *
 * Purpose
 * Removes spaces from a string depending on the flags passed.
 *
 * Parameters
 * text:	The text to remove spaces from.
 * flags:	The flags to set which spaces get removed.
 *		CLR_LEADING_SPACES - Removes spaces at the begining of the string.
 *		CLR_TRAILING_SPACES - Removes spaces at the end of the string.
 *		CLR_DOUBLE_SPACES - Removes all double spaces.
 *		CLR_ALL_SPACES - Removes every space character from the string.
 *
 * Return value
 * Returns a string
 *
 */
function RemoveSpaces(text, flags:=CLR_LEADING_SPACES)
	if ( flags & CLR_ALL_SPACES )
		while ( text[" "] )
			text[" "] := "";
			sleepms(2);
		endwhile
	else
		if ( flags & CLR_LEADING_SPACES )
			while ( text[1] == " " )
				text[1] := "";
				sleepms(2);
			endwhile
		endif
		
		if ( flags & CLR_TRAILING_SPACES )
			while ( text[len(text)] == " " )
				text[len(text)] := "";
				sleepms(2);
			endwhile
		endif

		if ( flags & CLR_DOUBLE_SPACES )
			while ( text["  "] )
				text["  "] := " ";
				sleepms(2);
			endwhile
		endif
	endif

	return text;
endfunction
