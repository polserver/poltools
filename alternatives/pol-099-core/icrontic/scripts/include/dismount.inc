//include "include/myutil";

//function returns 0 if the animal couldn't be created and/or
//if the mount wasn't destroyed

function dismounta( me, mount )

	if (!mount)
		if (!me)
			return 0;
		else
			if (!me.corpsetype)
				mount := GetEquipmentByLayer( me, 25 );
			else
				foreach item in ListRootItemsInContainerOfObjtype(me, 0xF021)
					mount := item;
				endforeach
			endif
		endif
	endif

	if (!mount)	return 0; endif

	var critter := "";
	case (mount.graphic)
	0x3ea2: critter := "horse2";
	0x3e9f: critter := "horse";
	0x3ea0: critter := "horse3";
	0x3ea1: critter := "horse4";
	0x3ea6:	critter := "llama";
	0x3ea3:	critter := "desertostard";
	0x3ea4:	critter := "frenziedostard";
	0x3ea5:	critter := "forestostard";
	0x3ea7: critter := "vortex1";
	endcase

	if ((mount.color = 1109) and (critter="")) critter := "nightmare"; endif

	set_critical(1);
	var animal := CreateNPCFromTemplate (critter, me.x, me.y, me.z);
	if (!animal) return 0; endif

	animal.color := mount.color;
	SetObjProperty(animal,"color",mount.color);

	if (GetObjProperty (mount, "petname"))
		animal.name := GetObjProperty (mount, "petname");
	endif
	if (Getobjproperty(mount,"pethp"))
		animal.hp := Getobjproperty(mount,"pethp");
	endif
	if (Getobjproperty(mount,"petmana"))
		animal.mana := Getobjproperty(mount,"petmana");
	endif

	if (me)
		if (!me.corpsetype)
			SetObjProperty (animal, "master", me.serial );
			animal.script := "tamed";
			animal.facing := me.facing;
			RestartScript (animal);
		elseif (me.npctemplate)
			SetObjProperty (animal, "master", GetObjProperty(me,"master"));
			animal.script := "tamed";
			animal.facing := me.facing;
			RestartScript (animal);
		endif
	else
		SetObjProperty(animal,"killme",1);
	endif

	if (DestroyItem( mount )) return 1; else return 0; endif

endfunction
