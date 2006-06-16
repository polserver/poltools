/////////////////////////////////////////////////////////////////////////////
//
// selchar.inc: 
//
// SelectOnlineCharacter( selector, charname, action )
//
// if charname is blank, prompts selector with a dialog box
// 'action' is the text displayed above the column of buttons
//
/////////////////////////////////////////////////////////////////////////////

use uo;


function SelectOnlineCharacter( selector, charname, action )

  if(!charname)

    return SelectCharacterFromDialog( selector, "Goto" );
  else
    //print (who.name + " wants to visit " + whom);

    charname := upper(charname);

    foreach onlinechr in EnumerateOnlineCharacters()
        if (upper(onlinechr.name) == charname)
            return onlinechr;
        endif
    endforeach

    return 0;

  endif

endfunction


var playercount := 0;
var pagecount := 1;

var layout := array(

"page 0",
"nodispose",
"gumppic 0 50 2620",
"gumppic 20 50 2621",
"gumppic 290 50 2622",
"gumppic 0 68 2623",
"gumppic 22 68 2624",
"gumppic 288 68 2625",
"gumppic 0 272 2623",
"gumppic 22 272 2624",
"gumppic 288 272 2625",
"gumppic 0 476 2626",
"gumppic 20 476 2627",
"gumppic 290 476 2628",
"gumppic 20 80 2621",   

"text 20 59 40 0",
"text 260 59 40 1"
);

var data := array(
       "Player Name",         //0
       "[action]"
);


function SelectCharacterFromDialog( selector, action )

    var players := EnumerateOnlineCharacters();
    var datalen;
    var layoutlen;
    var prev_y := CInt(80);
    var datindx;
    var buttonret := 1;
   

    layout[GetLayoutLength()+1] := "page " + pagecount;
    if( len(players) > 19)
        layout[GetLayoutLength()+1] := "button 285 465 2648 2647 0 " + (pagecount + 1);
    endif

    data[2] := action;

    foreach player in players
        playercount := playercount + 1;

        datalen := GetDataLength();
        layoutlen := GetLayoutLength();

        datindx := datalen+1;
        data[datindx] := player.name;
        if( playercount == 20 || playercount == 39 || playercount == 58 || playercount == 77 || playercount == 96)

            pagecount := pagecount + 1;
            layout[GetLayoutLength()+1] := "page " + pagecount;
            prev_y := 80;
            //add page turners

            layout[GetLayoutLength()+1] := "button 285 85 2650 2651 0 " + (pagecount - 1);
            if( len(players) > playercount + 19)
                layout[GetLayoutLength()+1] := "button 285 465 2648 2647 0 " + (pagecount + 1);
            endif
        endif

        layout[GetLayoutLength()+1] := ("text 20 " + CStr(prev_y + 20) + " 40 " + datalen);
        layout[GetLayoutLength()+1] := ("button 260 " + CStr(prev_y + 20) + " 2640 2641 1 0 " + buttonret);

        prev_y := prev_y + 20;
        buttonret := buttonret + 1;
    endforeach

    var result := SendDialogGump( selector, layout, data );

    if (result)
        return players[ result[0] ];
    else
        return 0;
    endif
   

endfunction


function GetLayoutLength();
    return (len(layout));
endfunction

function GetDataLength();
    return (len(data));
endfunction

