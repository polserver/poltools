include "include/NPCBackpacks";

const STARTHAIR := 0x2030;
const ENDHAIR := 0x2060;

function grabloot()
  if(GetObjProperty(me,"noloot") || GetObjProperty(me, "summoned"))
    return;
  endif
  var gantiloot := GetGlobalProperty("gantiloot");
  if(gantiloot == 1)
    return;
  endif
  if((me.script == "animal") || (me.script == "meek") || (me.script == "barker") || (me.script == "wolf"))
    return;
  endif
  var numitems := 0;
  var mypack := 0;
  if(!GetObjProperty(me, "serial"))
    SetObjProperty(me, "serial", me.serial);
  endif
  mypack := FindMyPack(me.serial);
  var near_items := ListItemsNearLocation(me.x, me.y, me.z,6);
  foreach item in near_items
    if(CheckLineOfSight(me, item))
      var sh := GetStandingHeight(item.x, item.y, item.z);
      if(sh.multi || sh.multi.serial)
        break;
      endif
      var info := CInt(GetMapInfo(item.x, item.y).landtile);
      if((info > CInt(0x405)) &&(info < CInt(0x456)))
        return;
      endif
      if((item.objtype == UOBJ_CORPSE) && (!GetObjProperty(item.objtype, "npctemplate")))
        var items := { };
        foreach thingie in EnumerateItemsInContainer(item)
          if((thingie.objtype < STARTHAIR || thingie.objtype > ENDHAIR) &&(!CInt(GetObjProperty(thingie,"#ignoreit"))))
            items[len(items) + 1] := thingie;
          endif
        endforeach
        if(len(items))
          RunToIt(me,item);
          if(Distance(me, item) < 2)
            foreach thingie in items
              if(MoveItemToContainer(thingie, mypack))
                say("You notice " + me.name + " rummage through a corpse and take an item.");
                sleep(2);
                break;
              else
                SetObjProperty(thingie,"#ignoreit",1);
              endif
            endforeach
          endif
        else
          SetObjProperty(item,"#ignoreit",1);
        endif
      endif
      break;
    endif
  endforeach
endfunction

function RunToIt(me,item)

	var numsteps;
	for (numsteps := 0; numsteps < 8; numsteps := numsteps +1)
		if ( distance(me, item) < 2 )
			return;
		else
			runtoward(item);
		endif
	endfor

endfunction
