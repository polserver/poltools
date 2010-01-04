function Credit(who,credits)
  var creds:= CInt(GetObjProperty(who,"credits"));
  creds:= creds + credits;
  SetObjProperty(who,"credits",creds);
  SendSysMessage(who,"You have recieved "+ credits +" Quest Points. You now have "+ creds +" Points.");
endfunction
