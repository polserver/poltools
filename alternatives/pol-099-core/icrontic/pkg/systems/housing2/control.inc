Function ClearEventQueue()
  var ev;
  repeat
  until (! (ev := os::wait_for_event(1)) );
EndFunction

function decayhouse(house, sign)
  var addonitem;
  var hserial := GetObjProperty( sign , "house_serial" );
  // Find owner account and remove the house from his list
  var oacct := GetObjProperty( house , "owneracct" );
  var account := FindAccount (oacct);
  if(account)
    var totalhouse := account.GetProp("HOUSE");
    account.SetProp("HOUSE", totalhouse - 1);
  endif

  // Destroy the 3 components added during build of house
  // Remove addons and Ban Tiles
  RemoveAddons(house, sign);
  var tilelist := GetObjProperty(house, "bantiles");
  foreach thing in tilelist
    DestroyItem(SystemFindObjectBySerial(thing));
  endforeach

  // Setup for the box array of house
  var lists := GetObjProperty(house, "footage");
  var itemlist := {};
  var holder, chk := 0;
  foreach thing in lists
    holder := ListObjectsInBox(thing[1], thing[2], thing[3], thing[4], thing[5],thing[6]);
    foreach thing in holder
      itemlist.append(thing);
    endforeach
  endforeach

  // Check itemlist for secures and locked down items to get rid of them
  foreach thing in itemlist
    if((GetObjProperty(thing,"secure") == 1) && (thing.movable == 0))
      DestroyItem(thing);
    elseif ((GetObjProperty(thing, "lockeddown" ) == 1) && (thing.movable == 0))
      thing.movable := 1;
      EraseObjProperty(thing, "houseserial" );
      EraseObjProperty(thing, "lockeddown");
      thing.decayat := ReadGameClock() + 10;
    elseif (thing.movable == 0)
      DestroyItem(thing);
    endif
  endforeach

  // Destroy the house
  DestroyMulti( house );
  
endfunction

// Find and remove all the components such as ladders
function RemoveAddons(house, sign)
var addonitem;
addonitem := cint(GetObjProperty( house, "component1" ));
if (addonitem)
  destroyitem(addonitem);
endif
addonitem := cint(GetObjProperty( house, "component2" ));
if (addonitem)
  destroyitem(addonitem);
endif
addonitem := cint(GetObjProperty( house, "component3" ));
if (addonitem)
  destroyitem(addonitem);
endif
if (house.objtype==24788)
  foreach item in ListItemsNearLocation( sign.x, sign.y, 0, 8 )
    if((item.objtype==0x177d) or (item.objtype==0xb41) or (item.objtype=0xb42))
      destroyitem(item);
    endif
  endforeach
endif

return;

endfunction

