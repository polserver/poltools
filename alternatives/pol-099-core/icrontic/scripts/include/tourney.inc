use uo;
use os;
use util;

function mazedeath(corpse, ghost)
 var maze := GetGlobalProperty("mazetourn");
 var surv := GetGlobalProperty("survtourn");
 if ( ghost.x>=5379 and ghost.y>=1279 and ghost.x<=5629 and ghost.y<=1510)
  dismount(ghost, corpse);
  var corpsenamearray := SplitWords(corpse.name);
  var corpsenamearraylen := len(corpsenamearray);
  var x, corpsename := "";
  for (x := 4; x <= corpsenamearraylen; x := x + 1)
    corpsename := corpsename + " " + corpsenamearray[x];
  endfor
  var team := getobjproperty(ghost, "TEAM");
  	 If (team="RED")
            if(maze)
		MoveCharacterToLocation( ghost, 5518, 1395, 0 );
                SendSysMessage(ghost, "You died in the Maze Complex, sending back to base.");
                DestroyItem(corpse);
                foreach reportable in(ghost.reportables)
                  ghost.removeReportable(reportable.serial,reportable.gameclock);
                  SendSysMessage(ghost, "Murder counts removed from the tournament.", 3, 0x0481);
                endforeach
		return;
            elseif(surv)
		MoveCharacterToLocation( ghost, 6095, 278, 44 );
                SendSysMessage(ghost, "You died in the Survivor Complex, sending back to base.");
                DestroyItem(corpse);
                foreach reportable in(ghost.reportables)
                  ghost.removeReportable(reportable.serial,reportable.gameclock);
                  SendSysMessage(ghost, "Murder counts removed from the tournament.", 3, 0x0481);
                endforeach
		return;
           endif
	endif
         If (team="BLUE")
           if(maze)
		MoveCharacterToLocation( ghost, 5443, 1411, 0 );
                SendSysMessage(ghost, "You died in the Maze Complex, sending back to base.");
                DestroyItem(corpse);
                foreach reportable in(ghost.reportables)
                  ghost.removeReportable(reportable.serial,reportable.gameclock);
                  SendSysMessage(ghost, "Murder counts removed from the tournament.", 3, 0x0481);
                endforeach
		return;
           elseif(surv)
		MoveCharacterToLocation( ghost, 6095, 278, 44 );
                SendSysMessage(ghost, "You died in the Maze Complex, sending back to base.");
                DestroyItem(corpse);
                foreach reportable in(ghost.reportables)
                  ghost.removeReportable(reportable.serial,reportable.gameclock);
                  SendSysMessage(ghost, "Murder counts removed from the tournament.", 3, 0x0481);
                endforeach
		return;
           endif
	endif
  endif
endfunction

function noreportzone ( ghost )

  if ( ghost.x>=5379 and ghost.y>=1279 and ghost.x<=5629 and ghost.y<=1510)  //Maze Tourny Area
    SetObjProperty(ghost, "#noreport", 1);
    SetObjProperty(ghost, "#nfloss", 1);
    return;
  elseif( ghost.x>=5720 and ghost.y>=858 and ghost.x<=5770 and ghost.y<=900) // KingoftheRing
    SetObjProperty(ghost, "#noreport", 1);
    SetObjProperty(ghost, "#nfloss", 1);
    return;
  elseif( ghost.x>=5376 and ghost.y>=1176 and ghost.x<=5414 and ghost.y<=1224) // Maze Entrance
    SetObjProperty(ghost, "#noreport", 1);
    SetObjProperty(ghost, "#nfloss", 1);
    return;
  elseif( ghost.x>=5377 and ghost.y>=1081 and ghost.x<=5406 and ghost.y<=1101) // Tourny Complex
    SetObjProperty(ghost, "#noreport", 1);
    SetObjProperty(ghost, "#nfloss", 1);
    return;
  else
    return;
  endif
endfunction

function noobzone (ghost)
  if( ghost.x>=5377 and ghost.y>=1081 and ghost.x<=5406 and ghost.y<=1101) // Tourny Complex
    SetObjProperty(ghost, "newbie", 1);
    return;
  elseif( ghost.x>=5271 and ghost.y>=1159 and ghost.x<=5312 and ghost.y<=1192) // Jail (for .jailkill
    SetObjProperty(ghost, "newbie", 1);
    return;
  elseif( ghost.x>=5377 and ghost.y>=1081 and ghost.x<=5406 and ghost.y<=1101) // Tourny Complex
    SetObjProperty(ghost, "newbie", 1);
    return;
  else
    return;
  endif
endfunction

function dismountmaze(me,corpse)
  var npccfgfile     := ReadConfigFile("npcdesc");
  var mount := GetEquipmentByLayer( corpse, 25 );
  foreach item in EnumerateItemsInContainer(corpse)
	if (item.objtype == 0x3ea2)
	  mount := item;
	  break;
	endif
  endforeach
  if (!mount)
	return;
  endif
  var animal := SystemFindObjectBySerial(CInt(GetObjProperty(mount,"serial")));
  animal.facing := corpse.facing;
  animal.script := npccfgfile[me.npctemplate].script;
  animal.setmaster(0);
  EraseObjProperty(animal, "mounted");
  EraseObjProperty(animal, "mounted_on");
  EraseObjProperty(animal, "master");
  EraseObjProperty(animal, "serial");
  EraseObjProperty(animal, "script");
  EraseObjProperty(animal, "happiness");
  EraseObjProperty(animal, "mounted");
  EraseObjProperty(animal, "mounted_on");
  MoveCharacterToLocation( animal, corpse.x, corpse.y, corpse.z, MOVECHAR_FORCELOCATION);
  DestroyItem(mount);
  ApplyRawDamage(animal, 200);
endfunction
