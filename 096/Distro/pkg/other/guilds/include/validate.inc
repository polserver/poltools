/* $Id: validate.inc 104 2006-05-25 22:14:15Z montuz $
 *    Author: Unreal
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
        if ( Lower(guilds.GetProp("GuildAbbr")) == Lower(thestring) )
            flag := 0;
            break;
        endif
    endforeach

    return flag;

endfunction

function ValidName(thestring)

    var flag := 1;

    foreach guilds in ListGuilds()
        if ( Lower(guilds.GetProp("GuildName")) == Lower(thestring) )
            flag := 0;
            break;
        endif
    endforeach

    return flag;

endfunction