/* $Id$
 *
 */

use os;
use cfgfile;

/*
 * ParseCommands(text, last_cmd)
 *
 * Purpose:
 * Takes in spoken text and finds boat commands inside of it.
 * Returns valid commands in the order they were issued.
 *
 * Parameters:
 * text:	Text to parse for commands.
 * cmd_keys:	Array of strings containing commands.
 *
 * Return Value:
 * Returns an array of strings.
 *
 */
function ParseCommands(byref text, byref cmd_keys)
	text := Lower(text);

	var match := 0, pos := 0;
	foreach cmd in ( cmd_keys )
		pos := pos + 1;
		cmd := Lower(cmd);
		while ( text[cmd] )
			text[cmd] := "[["+CStr(pos)+"]] ";
			match := 1;
			SleepMS(2);
		endwhile
		SleepMS(2);
	endforeach
	if ( match == 0 )
		return 0;
	endif

	var cmd_order := array{};
	text := SplitWords(text);
	foreach cmd in ( text )
		if ( cmd["[["] && cmd["]]"] )
			cmd["[["] := "";
			cmd["]]"] := "";
			cmd := cmd_keys[CInt(cmd)];
			cmd_order.Append(cmd);
		endif
		SleepMS(2);
	endforeach
	text := "";

	return cmd_order;
endfunction
