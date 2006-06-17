use uo;

const MAX_YESNO_LINES := 7;

function YesNo(me, prompt)
  if (len(prompt) <= 10)
    return SmallYesNo(me, prompt);
  else
    return BigYesNo(me, prompt);
  endif
endfunction

function SmallYesNo(me, prompt)

  var layoutyn := array(
                      "page 0",
                      "nodispose",
                      "gumppic 0 0 2070",
                      "button 30 75 2130 2129 1 0 1",
                      "button 90 75 2119 2120 1 0 0",
                       "text 40 30 502 0"
	             );

  var datayn := array(  prompt  );
  var res := SendDialogGump( me, layoutyn, datayn );

  if (res)
    return 1;
  else
    return 0;
  endif

endfunction

function BigYesNo(me, prompt)

  var layoutyn := array(
                        "page 0",
			"nodispose",
                        "gumppic 0 0 1140",
                        "button 115 200 2130 2129 1 0 1",
                        "button 205 200 2119 2120 1 0 0",
                        "text 55 45 502 0",
                        "text 55 65 502 1",
                        "text 55 85 502 2",
                        "text 55 105 502 3",
                        "text 55 125 502 4",
                        "text 55 145 502 5",
                        "text 55 165 502 6"
	               );

  var datayn := packPromptToArray(prompt);
  var res := SendDialogGump( me, layoutyn, datayn );

  if (res)
    return 1;
  else
    return 0;
  endif

endfunction

function packPromptToArray(prompt)
  var theArray := array;
  var theString := prompt;
  var i;

  for (i:=1; i<=MAX_YESNO_LINES; i:=i+1)
    theArray[i] := " ";
  endfor
  
  i := 0;

  while ((len(theString) > 0) && (i <= MAX_YESNO_LINES))
    i := i + 1;
    theArray[i] := StripSubstring(theString, 40);
  endwhile

  return theArray;
endfunction

function StripSubstring(byref theString, theSize)
  var subString;
  var newString;
  var pos := theSize;

  if (len(theString) <= theSize)
    subString := theString;
    theString := "";
    return subString;
  endif
  
  while ((theString[pos] != " ") && (pos > 1))
    pos := pos - 1;
  endwhile
  
  if (pos == 1)
    pos := theSize;
  endif
  
  subString := theString[pos+1, len(theString)];
  newString := theString[1,pos];
  
  theString := subString;
  
  return newString; 

endfunction