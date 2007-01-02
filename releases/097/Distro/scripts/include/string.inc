/** $Id$
 *
 * Purpose
 * Provide various functions for string handling.
 *
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
CONST STR_DEL_ANY		:= 0x2;
CONST STR_DEL_LEADING		:= 0x4;
CONST STR_DEL_TRAILING		:= 0x8;

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

/* RemoveFromString(text, remove, flags)
 *
 * Purpose
 * Removes strings from a string.
 *
 * Parameters
 * text:	The text to remove text from.
 * remove:	What to remove from the text.
 * flags:	The flags that set where data can get removed.
 * 		STR_DEL_LEADING  - Removes text only at the begining.
 *		STR_DEL_TRAILING - Removes text only at the end.
 *		STR_DEL_ANY	 - Removes text anywhere it appears.
 *		STR_DEL_ONCE     - Removes text anywhere it appears, but only the first instance.
 *
 * Return Value
 * Returns a string
 *
 *//*
function RemoveFromString(text, remove, flags:=STR_DEL_ONCE)
	text := CStr(text);
	remove := CStr(remove);

	if ( flags & STR_DEL_ONCE )
		text[remove] := "";
	else
		var remove_len := Len(remove);
		if ( flags & STR_DEL_LEADING )
			while ( text[1, remove_len] == remove )
				text[1, remove_len] := "";
				SleepMS(2);
			endwhile
		endif

		if ( flags & STR_DEL_TRAILING )
			while ( text[
*/

/*
 * RemoveSpaces(text, flags)
 *
 * Purpose
 * Removes spaces and null values from a string depending on the flags passed.
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
