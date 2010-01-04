//
// Gump Utils
//
// Copyright 2003 Jouni Airaksinen, mintus@bluecave.net
//

use uo;

function SendStringAsHtmlWindow(who, text, title := "Message Of The Day")
  var data := array(
  "page 0",    
//  "nomove",  
  "nodispose", 
//  "noclose", 
  "resizepic 40 40 5054 300 400",
  "tilepic 56 52 5357",
  "tilepic 65 49 4032",
  "text 109 50 62 0",
  "htmlgump 52 79 276 348 1 1 1",
  );
  var lines := array(
  title,
  text
  );
  
  return SendDialogGump( who, data, lines );
endfunction