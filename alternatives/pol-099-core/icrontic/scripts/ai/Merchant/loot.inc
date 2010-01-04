include "npcbackpacks";

Const STARTHAIR := 0x2030;
Const ENDHAIR := 0x2060;

function grabloot()

	if ( GetObjProperty(me,"noloot") )
		return;
	endif

	var numitems := 0;
	var mypack := 0;
	if (!getobjproperty(me,"serial"))
		setobjproperty(me, "serial", me.serial);
	endif
	mypack := FindMyPack(me.serial);

	var Near_Items := ListItemsNearLocation(me.x, me.y, me.z,6);

     foreach item in Near_Items
	if( CheckLineOfSight( me, item ) )

	var sh := GetStandingHeight( item.x, item.y, item.z );
	if (sh.multi or sh.multi.serial)
		break;
	endif
	var info := cint(GetMapInfo( item.x , item.y ).landtile);
	if ((info > cint(0x405)) and (info < cint(0x456)))
		break;
	endif
	
	if ( (item.movable) and ((item.objtype < STARTHAIR or item.objtype > ENDHAIR))  )
		RunToIt(me,item);
		if ( (distance(me, item) < 2) and (moveitemtocontainer(item, mypack)) )
				say( "*yoink*" );
				sleep(2);
				return;
		else
			return;
		endif
	elseif (item.objtype = UOBJ_CORPSE)
	    if (!cint(GetObjProperty(item,"#ignoreit")) )
		var Items := { };

		foreach thingie in EnumerateItemsIncontainer(Item)
		if (    (thingie.objtype < STARTHAIR or thingie.objtype > ENDHAIR) 
			and (!cint(GetObjProperty(thingie,"#ignoreit"))) )
			Items[ len(Items)+1 ] := thingie;
		endif
		endforeach

		if ( len(Items) )
			RunToIt(me,item);
			if (distance(me, item) < 2)
				foreach thingie in Items
				    if ( moveitemtocontainer(thingie, mypack) )
					say( "*yoink*" );
					sleep(2);
					return;
				    else
					SetObjProperty(thingie,"#ignoreit",1);
				    endif
				endforeach
			endif 
		else
			SetObjProperty(item,"#ignoreit",1);
		endif

	    endif
	endif

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
