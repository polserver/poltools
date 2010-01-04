function get_direction(who)
  var ret;
  if(who.facing in array(1, 3, 5, 7))
    case (RandomInt(2))
      0: ret := "NS";
      1: ret := "EW";
	endcase
  else
    case(who.facing)
      2: ret := "EW";
      4: ret := "NS";
      6: ret := "EW";
      0: ret := "NS";
    endcase
  endif
  return ret;
endfunction

function CreateNorthSouthField(caster, fieldtype,x,y,z, length := 5)
  var counter := 0;
  var i, lst;
  var fires := { };
  var tester := CreateItemAtLocation(x,y,z, 0xeed, 1);
  for ( i := x - 2; i <= x - 2 + length; i := i + 1)
    counter := counter  + 1;
    if(fieldtype == 0x80)
      if((CheckLosAt(tester, i, y, z )) and (len(ListMobilesNearLocation(i, y, z, 0)) = 0))
        fires[counter] := CreateItemAtLocation( i, y, z, fieldtype, 1);
        SetObjProperty(fires[counter], "f","1");
        SetObjProperty(fires[counter], "creater", caster.serial);
      endif
    else
      if(CheckLosAt(tester, i, y, z ))
        fires[counter] := CreateItemAtLocation( i,y,z,fieldtype, 1 );
        SetObjProperty(fires[counter], "creater", caster.serial);
        SetObjProperty(fires[counter], "f","1");
      endif
    endif
  endfor
  DestroyItem(tester);
  return fires;
endfunction

function CreateEastWestField(caster, fieldtype,x,y,z, length)
  var counter := 0;
  var i, lst;
  var fires := { };
  var tester := CreateItemAtLocation(x,y,z, 0xeed, 1);
  for ( i := y - 2; i <= y - 2 + length; i := i + 1)
    counter := counter  + 1;
    if(fieldtype == 0x80)
      if((CheckLosAt(tester, x, i, z )) and (len(ListMobilesNearLocation(x, i, z, 0)) = 0))
        fires[counter] := CreateItemAtLocation( x,i,z,fieldtype, 1 );
        SetObjProperty(fires[counter], "f","1");
        SetObjProperty(fires[counter], "creater", caster.serial);
      endif
    else
      if(CheckLosAt(tester, x, i, z ))
        fires[counter] := CreateItemAtLocation( x,i,z,fieldtype, 1 );
        SetObjProperty(fires[counter], "f","1");
        SetObjProperty(fires[counter], "creater", caster.serial);
      endif
    endif
  endfor
  DestroyItem(tester);
  return fires;
endfunction
