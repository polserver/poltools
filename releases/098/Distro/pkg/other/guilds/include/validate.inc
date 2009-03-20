/* $Id: validate.inc 900 2006-11-01 17:21:50Z MontuZ $
 *    Author: MontuZ
 *    Created: Sunday, May 21 2006
 *    Version: 0.01
 */

function ValidStr( thestring )

    var i, valid_chars := {
    "0", "1", "2", "3", "4",
    "5", "6", "7", "8", "9",
    "a", "b", "c", "d", "e",
    "f", "g", "h", "i", "j",
    "k", "l", "m", "n", "o",
    "p", "q", "r", "s", "t",
    "u", "v", "w", "x", "y",
    "z", "$", "*", "&", "#",
    "@", "!", "?", "+", "-",
    "^", " "};

    thestring := Lower(thestring);

    var flag := 1;

    for ( i := 1; i <= Len(thestring); i := i + 1 )
        if ( !( thestring[i] in valid_chars ) )
            flag := 0;
        endif
    endfor

    return flag;

endfunction

function ValidAbbr(thestring)

    var flag := 1;

    foreach guilds in ListGuilds()
        if ( Lower(guilds.GetProp("Abbr")) == Lower(thestring) )
            flag := 0;
            break;
        endif
    endforeach

    return flag;

endfunction

function ValidName(thestring)

    var flag := 1;

    foreach guilds in ListGuilds()
        if ( Lower(guilds.GetProp("Name")) == Lower(thestring) )
            flag := 0;
            break;
        endif
    endforeach

    return flag;

endfunction