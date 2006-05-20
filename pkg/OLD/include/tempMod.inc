use os;

function do_temp_mod(who, mod, amt, duration)
  var parms := {};
  parms[2] := who;
  parms[1] := mod;
  parms[3] := amt;
  parms[4] := duration;
  start_script("::/misc/demod", parms);
endfunction

function can_stat_mod(who, stat)
  var mod;
  case(stat)
    "str": mod := "#ss";
    "dex" : mod := "#dd";
    "int" : mod := "#ii";
    "ar"  : mod := "#aa";
  endcase
  if(GetObjProperty(who, mod))
    return 0;
  else
    SetObjProperty(who, mod, 1);
    return 1;
  endif
endfunction

function allow_stat_mod(who, stat, duration)
  Detach();
  var mod;
  case(stat)
    "str": mod := "#ss";
    "dex" : mod := "#dd";
    "int" : mod := "#ii";
    "ar"  : mod := "#aa";
  endcase
  while(1)
    sleep(duration);
    if(who)
      EraseObjProperty(who, mod);
      return;
    endif
  endwhile
endfunction