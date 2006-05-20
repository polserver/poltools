/* $Id: fishingFunctions.inc 917 2005-11-05 23:51:11Z austin $
 *
 */

function is_fish(theobj)

	if((theobj >= UOBJ_FISH_START) && (theobj <= UOBJ_FISH_END) )
		return 1;
	elseif ((theobj >= UOBJ_SFISH_START) && (theobj <= UOBJ_SFISH_END))
		return 1;
	else
		return 0;
	endif

	return 0;

endfunction


function CarveFish(who, blade, use_on)

	if( (!Accessible(who, use_on)) or (!CheckLineOfSight(who, use_on)) or (distance(who, use_on) > 2) )
		SendSysMessage(who, "You cannot use that.");
		return 0;
	endif
	if( (!ReserveItem(use_on)) || (use_on.movable == 0) )
		SendSysMessage(who, "You cannot use that.");
		return 0;
	endif

	var num_steaks := (4 * use_on.amount);
	PlaySoundEffect(who, SFX_57);
	if( DestroyItem(use_on) )
		CreateItemInBackpack(who, UOBJ_FISHSTEAK, num_steaks);
		SendSysMessage(who, "You carve the fish steaks and put them in your backpack.");
		return 1;
	endif
	sleep(1);

	return 0;

endfunction

function IsInRange(who, x, y, z)

	z := z;
	var xd, yd;
	var retval := 1;
	xd := Abs(who.x - x);
	yd := Abs(who.y - y);
	if((xd > 6)||(yd > 6))
		retval := 0;
	endif

	return retval;

endfunction


function IsWater(thistile, locinfo)

	if(thistile.objtype)
		if((thistile.objtype >= 0x1797)&&(thistile.objtype <= 0x179c))
			return 1;
		elseif((thistile.objtype >= 0x346e)&&(thistile.objtype <= 0x3485))
			return 1;
		elseif((thistile.objtype >= 0x3490)&&(thistile.objtype <= 0x34ab))
			return 1;
		elseif((thistile.objtype >= 0x34b5)&&(thistile.objtype <= 0x34d5))
			return 1;
		endif
	else
		if((locinfo.landtile >= 0x00a8)&&(locinfo.landtile <= 0x00ab))
			return 1;
		elseif((locinfo.landtile >= 0x0136)&&(locinfo.landtile <= 0x0137))
			return 1;
		elseif((locinfo.landtile >= 0x3ff0)&&(locinfo.landtile <= 0x3ff3))
			return 1;
		endif
	endif

	return 0;

endfunction


function IsValidFishingSpot(who, targetsquare, locinfo)

	var retval := 1;
	if(!IsWater(targetsquare, locinfo))
		SendSysMessage(who, "You can't fish there!");
		retval := 0;
	elseif(!IsInRange(who, targetsquare.x, targetsquare.y, targetsquare.z))
		SendSysMessage(who, "That is too far away");
		retval := 0;
	elseif(!CheckLosAt(who, targetsquare.x, targetsquare.y, targetsquare.z))
		SendSysMessage(who, "That location is not in your line of sight!");
		retval := 0;
	endif

	return retval;

endfunction


function farout(who)

	var x := who.x, y := who.y;
	var chk := 0;

	var theinfo := GetMapInfo( x, y );
	if((theinfo.landtile >= 0x00a8)&&(theinfo.landtile <= 0x00ab))
		chk := 1;
	elseif((theinfo.landtile >= 0x0136)&&(theinfo.landtile <= 0x0137))
		chk := 1;
	elseif((theinfo.landtile >= 0x3ff0)&&(theinfo.landtile <= 0x3ff3))
		chk := 1;
	else
		return 0;
	endif

	var distinfo := GetMapInfo( (x+15), (y+15) );
	if((distinfo.landtile >= 0x00a8)&&(distinfo.landtile <= 0x00ab))
		return 1;
	elseif((distinfo.landtile >= 0x0136)&&(distinfo.landtile <= 0x0137))
		return 1;
	elseif((distinfo.landtile >= 0x3ff0)&&(distinfo.landtile <= 0x3ff3))
		return 1;
	endif

	distinfo := GetMapInfo( (x-15), (y-15) );
	if((distinfo.landtile >= 0x00a8)&&(distinfo.landtile <= 0x00ab))
		return 1;
	elseif((distinfo.landtile >= 0x0136)&&(distinfo.landtile <= 0x0137))
		return 1;
	elseif((distinfo.landtile >= 0x3ff0)&&(distinfo.landtile <= 0x3ff3))
		return 1;
	endif

	distinfo := GetMapInfo( (x+15), (y-15) );
	if((distinfo.landtile >= 0x00a8)&&(distinfo.landtile <= 0x00ab))
		return 1;
	elseif((distinfo.landtile >= 0x0136)&&(distinfo.landtile <= 0x0137))
		return 1;
	elseif((distinfo.landtile >= 0x3ff0)&&(distinfo.landtile <= 0x3ff3))
		return 1;
	endif

	distinfo := GetMapInfo( (x-15), (y+15) );
	if((distinfo.landtile >= 0x00a8)&&(distinfo.landtile <= 0x00ab))
		return 1;
	elseif((distinfo.landtile >= 0x0136)&&(distinfo.landtile <= 0x0137))
		return 1;
	elseif((distinfo.landtile >= 0x3ff0)&&(distinfo.landtile <= 0x3ff3))
		return 1;
	endif

	return 0;

endfunction


function isSOS(who, targetsquare)

	var packitems;
	packitems := EnumerateItemsInContainer(who.backpack);
	var i, sosx, sosy;

	for(i := 1; i <= len(packitems); i := i + 1)
		if((packitems[i].objtype == 0xa363))
			sosx := GetObjProperty(packitems[i], "tileX");
			sosy := GetObjProperty(packitems[i], "tileY");
			var xd, yd;
			xd := Abs(targetsquare.x - sosx);
			yd := Abs(targetsquare.y - sosy);
			if((xd <= 6)&&(yd <= 6))
				GetSOSTreasure(who, targetsquare, packitems[i]);
				return 1;
			endif
		endif
	endfor

	return 0;

endfunction


function GetSOSTreasure(who, targetsquare, sosmsg)

	targetsquare := targetsquare;
	if( SkillCheck(who, FISHING, 80, 200) > 0 )
		var itemcfg := ReadConfigFile(":*:itemdesc");
		var it, oldname;
		if( RandomInt(100) < 75 )
			var newitem;
			var artarray;
			case(RandomInt(3))
				0:	newitem := RandomInt(0x0fcc - 0x0fc4)+ 0x0fc4;
				1:	artarray := array(0xea4, 0xea8, 0xea7, 0xe9f, 0xea0, 0xea3, 0xea6);
					newitem := artarray[RandomInt(7) + 1];
				2:	artarray := array(0x1397, 0x13a4, 0x13a5, 0x13a6, 0x13a7, 0x13a8, 0x13a9, 0x13ab, 0x13ac, 0x13ad, 0x13ae, 0x163a, 0x163b, 0x163c);
					newitem := artarray[RandomInt(14) + 1];
			endcase
			it := CreateItemInBackpack(who, newitem, 1);
			oldname := it.name;
			if( (!oldname) || (oldname == "") )
				oldname := itemcfg[newitem].desc;
			endif
			SetName(it, (oldname + " [Recovered from a shipwreck]"));
			it.movable := 1;
			SendSysMessage(who, "You caught something interesting!");
		else
			SendSysMessage(who, "You catch a sunken treasure!");
			var chest := CreateItemInBackpack(who, 0x9e40, 1);
			case(RandomInt(4) + 1)
				1: chest.graphic := 0xe40;
				2: chest.graphic := 0xe41;
				3: chest.graphic := 0xe7c;
				4: chest.graphic := 0x9ab;
			endcase
			SetName(chest, ("metal chest [Recovered from a shipwreck]"));
			chest.movable := 1;
			MakeLoot(chest, RandomInt(2) + 5);
			DestroyItem(sosmsg);
		endif
	else
		SendSysMessage(who, "Something big just slipped off your hook!");
	endif

	return 0;

endfunction