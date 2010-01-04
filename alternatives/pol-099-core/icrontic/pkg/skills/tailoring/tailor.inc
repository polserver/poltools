///////////////////
//  fortify a hat with a helm
///////////////////

function FortifyHat (character, hat)
 var fortified := GetObjProperty(hat, "fortified");
 If((GetEffectiveSkill(character, SKILLID_TAILORING) >= 105) && (GetEffectiveSkill(character, SKILLID_ARMSLORE) >= 80))
    if(fortified == 1)
	SendSysMessage (character, "That item is already fortified!");
        return 0;
    endif
	SendSysMessage (character, "Fortify this hat with what?");
	var helm := Target (character);
	if (!helm)
		SendSysMessage (character, "Canceled.");
		return;
	endif

	if (!IsAHelm (helm))
		SendSysMessage (character, "That's not a helmet.");
		return;
	endif

	if ((!ReserveItem (helm)) or  ( !InInventory (character.backpack, hat) or !InInventory (character.backpack, helm) ))
		SendSysMessage (character, "The helm must be in your backpack and not in use.");
		return;
	endif

	if (!CheckSkill (character, SKILLID_TAILORING, -1, 0))
		SendSysMessage (character, "You fail and ruin the hat.");
		DestroyItem (hat);
		return;
	endif

	PlaySoundEffect(character, SCISSORS_SOUND);
	sleep (2);
	PlaySoundEffect(character, SCISSORS_SOUND);
	sleep (2);

	var tempstring := TruncateArticle (helm.desc);
	hat.name := "a fortified " + TruncateArticle (hat.desc);
        hat.maxhp_mod := helm.hp;
        hat.ar_mod := helm.ar;
	DestroyItem (helm);
        SetObjProperty(hat, "fortified", 1);
        hat.newbie:=0;
        hat.buyprice := CInt(hat.buyprice * 2);
  else
    SendSysMessage (character, "You are not skilled enough to fortify items.");
    return;
 endif
endfunction

function FortifyDoublet (character, doublet)
 var fortified := GetObjProperty(doublet, "fortified");
 If((GetEffectiveSkill(character, SKILLID_TAILORING) >= 105) && (GetEffectiveSkill(character, SKILLID_ARMSLORE) >= 80))
    if(fortified == 1)
	SendSysMessage (character, "That item is already fortified!");
        return 0;
    endif
	SendSysMessage (character, "Fortify this Doublet with what?");
	var breastplate := Target (character);
	if (!breastplate)
		SendSysMessage (character, "Canceled.");
		return;
	endif

	if (!IsABreastplate (breastplate))
		SendSysMessage (character, "That's not useable for this.");
		return;
	endif

	if ((!ReserveItem (breastplate)) or  ( !InInventory (character.backpack, doublet) or !InInventory (character.backpack, breastplate) ))
		SendSysMessage (character, "The item must be in your backpack and not in use.");
		return;
	endif

	if (!CheckSkill (character, SKILLID_TAILORING, -1, 0))
		SendSysMessage (character, "You fail and ruin the doublet.");
		DestroyItem (doublet);
		return;
	endif

	PlaySoundEffect(character, SCISSORS_SOUND);
	sleep (2);
	PlaySoundEffect(character, SCISSORS_SOUND);
	sleep (2);

	var tempstring := TruncateArticle (doublet.desc);
	doublet.name := "a fortified " + TruncateArticle (doublet.desc);
        doublet.maxhp_mod := breastplate.hp;
        doublet.ar_mod := breastplate.ar;
	DestroyItem (breastplate);
        doublet.buyprice:= CInt(doublet.buyprice * 2);
        SetObjProperty(doublet, "fortified", 1);
  else
    SendSysMessage (character, "You are not skilled enough to fortify items.");
    return;
 endif
endfunction

function CreateRanger (character, studded)
 var fortified := GetObjProperty(studded, "ranger");
 If((GetEffectiveSkill(character, SKILLID_TAILORING) >= 110) && (GetEffectiveSkill(character, SKILLID_ARMSLORE) >= 105))
    if(fortified == 1)
	SendSysMessage (character, "That item is already modified!");
        return 0;
    endif
	SendSysMessage (character, "Select the iron to use.");
	var ironing := Target (character);
	if (!ironing)
		SendSysMessage (character, "Canceled.");
		return;
	endif

	if (ironing.objtype != 0x6309)
		SendSysMessage (character, "That's not iron!");
		return;
	endif

	if (ironing.amount <= 5)
		SendSysMessage (character, "Not enough iron.");
		return;
	endif

	if ((!ReserveItem (ironing)) or  ( !InInventory (character.backpack, studded) or !InInventory (character.backpack, ironing) ))
		SendSysMessage (character, "The iron must be in your backpack and not in use.");
		return;
	endif
 
	if (!CheckSkill (character, SKILLID_TAILORING, -1, 0))
		SendSysMessage (character, "You fail and ruin the armor.");
		DestroyItem (studded);
		return;
	endif

	PlaySoundEffect(character, SCISSORS_SOUND);
	sleep (2);
	PlaySoundEffect(character, SCISSORS_SOUND);
	sleep (2);

	var tempstring := TruncateArticle (studded.desc);
        var smod := StuddedType(studded);
	studded.name := "a Ranger's " + TruncateArticle (studded.desc);
        studded.ar_mod := smod + (studded.ar / 3);
        SubtractAmount(ironing, 5);
        studded.color := 34202;
        SetObjProperty(studded, "fortified", 1);
        SetObjProperty(studded, "ranger", 1);
        studded.buyprice := CInt(studded.buyprice * 2);
  else
    SendSysMessage (character, "You are not skilled enough to create ranger armor.");
    return;
 endif
endfunction

///////////////////
//  Determines if the item objtype passed is a hat
///////////////////

function IsAHat (item)

	case (item.objtype)
		0x153F: // Bandana
		0x1543: // Skullcap
		0x1713: // Floppy Hat
		0x1714: // Wide Brim Hat
		0x1715: // Cap
		0x1716: // Straw Hat
		0x1719: // Bonnet
		0x171A: // Feathered Hat
		0x171B: // Tricorn Hat
		0x171C: // Jester Hat
		0x141c:	// orc mask
                0x1718: // Wizards Hat
			return 1;
	endcase

	return 0;

endfunction

function IsABreastPlate (item)

	case (item.objtype)
		0x1415: // PlatemailBreastplate
		0x1416: // PlatemailBreastplate2
		0x13bf: // ChainmailTunic
		0x13c4: // ChainmailTunic2
		0x13ec: // RingmailTunic
		0x13ed: // RingmailTunic2
                0x90ab: // Balron Tunic
                0x95af: // Daemon Tunic
                0x144f: // Bone Tunic
                
			return 1;
	endcase

	return 0;

endfunction

function IsADoublet (item)

	case (item.objtype)
		0x1f7b: // Doublet
		        return 1;
	endcase

	return 0;

endfunction


///////////////////
//  Determines if the item objtype passed is a helm
///////////////////

function IsAHelm (item)

	case (item.objtype)
		0x1408:
		0x1409:
		0x140A:
		0x140B:
		0x140C:
		0x140D:
		0x140E:
		0x140F:
		0x1412:
		0x1419:
		0x1DB9:
		0x13BB:
		0x13C0:
		0x1451: // Bone Helm
                0x89ac: // Daemon Helm
                0x88ac: // Balron Helm
			return 1;
	endcase

	return 0;

endfunction

function IsCloth (item)

	case (item.objtype)
		0x0f95:
		0x0f96:
		0x0f97:
		0x0f98:
		0x0f99:
		0x0f9a:
		0x0f9b:
		0x0f9c:
                0x175d:
		0x175e:
		0x175f:
		0x1760:
		0x1761:
		0x1762:
		0x1763:
		0x1764:
		0x1765:
		0x1766:
		0x1767:
		0x1768:
			return 1;
	endcase

	return 0;

endfunction

function IsLeather (item)
  case (item.objtype)
    0x1078:
    0x1079:
    0x7020:
    0x7021:
    0x7022:
    0x7023:
    0x7024:
    0x7025:
    0x7026:
    0x7027:
    0x7028:
    0x7029:
    0x702a:
    0x702b:
    0x702c:
    0x702d:
    return 1;
  endcase
return 0;
endfunction

function InInventory (byref inventory, byref it)
  foreach item in EnumerateItemsInContainer(inventory)
    if (item = it)
      return 1;
    endif
  endforeach
  return 0;
endfunction

function TruncateArticle(x)

	x := CStr(x);
	var i := len(x);

	var an_articlelist := { "an ", "An ", "AN ", "aN " };
	var a_articlelist := { "a ", "A " };

	if ( (x[1,3]) in an_articlelist )
		x := (x[4,i-3]);
	elseif ( (x[1,2]) in a_articlelist )
		x := (x[3,i-2]);
	endif

	return x;

endfunction

function IsStudded (item)

	case (item.objtype)
		0x13d4: // Studded Sleeves
		0x13d5: // Studded Gloves
		0x13d6: // Studded Gorget
		0x13da: // Studded Leggings
		0x1db9: // Leather Cap
		0x13db: // Studded Tunic
		0x170b: // Leather Boots
		0x1711: // Thigh Boots
			return 1;
	endcase

	return 0;

endfunction

function StuddedType(item)

  var studdmod := (item.ar / 3);
  if(studdmod < 3)
    studdmod := 2;
  endif
  return studdmod;

endfunction

