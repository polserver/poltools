///////////////////
//  Renames the keg passed to it, dependant upon the potion type, how much is in the keg, and
//  if the keg is .own'ed
//////////////////

function RenameKeg (byref keg)
	local keg_potion_type := GetObjProperty (keg, "type");
	local keg_potion_subtype := GetObjProperty (keg, "subtype");
	local keg_potion_amount := GetObjProperty (keg, "amount");
	local keg_owner_name := GetObjProperty (keg, "ownername");

	if (!keg_potion_type or !keg_potion_amount)
		if (keg_owner_name)
			keg.name := CStr(keg_owner_name + "'s keg");
		else
			keg.name := "a keg";
		endif
		return;
	endif

	local temp_string := "";
	case (keg_potion_type)
		0xdc01:		temp_string := "keg of light healing potions";
		0xdc02:		temp_string := "keg of healing potions";
		0xdc03:		temp_string := "keg of greater healing potions";
		0xdc04:		temp_string := "keg of lesser poison potions";
		0xdc05:		temp_string := "keg of poison potions";
		0xdc06:		temp_string := "keg of greater poison potions";
		0xdc07:		temp_string := "keg of greater explosion potions";
		0xdc08:		temp_string := "keg of lesser explosion potions";
		0xdc09:		temp_string := "keg of explosion potions";
		0xdc0b:		temp_string := "keg of lesser agility potions";
		0xdc0c:		temp_string := "keg of agility potions";
		0xdc0d:		temp_string := "keg of greater agility potions";
		0xdc0e:		temp_string := "keg of refresh potions";
		0xdc0f:		temp_string := "keg of greater refresh potions";
		0xdc10:		temp_string := "keg of lesser strength potions";
		0xdc11:		temp_string := "keg of strength potions";
		0xdc12:		temp_string := "keg of greater strength potions";
		0xdc14:		temp_string := "keg of lesser cure potions";
		0xdc15:		temp_string := "keg of cure potions";
		0xdc16:		temp_string := "keg of greater cure potions";
		0xdc17:		temp_string := "keg of deadly poison potions";
		0xdce6:		temp_string := "keg of nightsight potions";
		0x7059:
			case (keg_potion_subtype)
				23:	temp_string := "keg of invisibility potions";
				24:	temp_string := "keg of Phandel's Fine Intellect potions";
				25:	temp_string := "keg of Phandel's Fabulous Intellect potions";
				26:	temp_string := "keg of Phandel's Fantastic Intellect potions";
				27:	temp_string := "keg of lesser Mego Invulnerability potions";
				28:	temp_string := "keg of Mego Invulnerability potions";
				29:	temp_string := "keg of greater Mego Invulnerability potions";
				31:	temp_string := "keg of Homeric Might potions";
				32:	temp_string := "keg of Greater Homeric Might potions";
				33:	temp_string := "keg of Tamla Heal potions";
				34:	temp_string := "keg of Taint's Minor Transmutation potions";
				35:	temp_string := "keg of Taint's Major Transmutation potions";
			endcase;
	endcase;

	if (!temp_string)
		keg.name := "an unknown keg";
		return;
	endif

	if (keg_owner_name)
		keg.name := CStr (keg_owner_name + "'s " + temp_string + " [" + keg_potion_amount + "]");
		SetObjProperty (keg, "oldname", CStr("a " + temp_string + " [" + keg_potion_amount + "]"));
	else
		keg.name := CStr ("a " + temp_string + " [" + keg_potion_amount + "]");
	endif
endfunction




///////////////////
//  Renames the potion passed to it based on it's type
//////////////////

function RenamePotion (byref potion)
	local potion_type := potion.objtype;
	local potion_subtype := GetObjProperty (potion, "itemtype");

	local temp_string := "";
	case (potion_type)
		0xdc01:		temp_string := "a light healing potion";
		0xdc02:		temp_string := "a healing potion";
		0xdc03:		temp_string := "a greater healing potion";
		0xdc04:		temp_string := "a lesser poison potion";
		0xdc05:		temp_string := "a poison potion";
		0xdc06:		temp_string := "a greater poison potion";
		0xdc07:		temp_string := "a greater explosion potion";
		0xdc08:		temp_string := "a lesser explosion potion";
		0xdc09:		temp_string := "an explosion potion";
		0xdc0b:		temp_string := "a lesser agility potion";
		0xdc0c:		temp_string := "an agility potion";
		0xdc0d:		temp_string := "a greater agility potion";
		0xdc0e:		temp_string := "a refresh potion";
		0xdc0f:		temp_string := "a greater refresh potion";
		0xdc10:		temp_string := "a lesser strength potion";
		0xdc11:		temp_string := "a strength potion";
		0xdc12:		temp_string := "a greater strength potion";
		0xdc14:		temp_string := "a lesser cure potion";
		0xdc15:		temp_string := "a cure potion";
		0xdc16:		temp_string := "a greater cure potion";
		0xdc17:		temp_string := "a deadly poison potion";
		0xdce6:		temp_string := "a nightsight potion";
		0x7059:
			case (potion_subtype)
				23:	temp_string := "an invisibility potion";
				24:	temp_string := "Phandel's Fine Intellect";
				25:	temp_string := "Phandel's Fabulous Intellect";
				26:	temp_string := "Phandel's Fantastic Intellect";
				27:	temp_string := "a lesser Mego Invulnerability potion";
				28:	temp_string := "a Mego Invulnerability potion";
				29:	temp_string := "a greater Mego Invulnerability potion";
				31:	temp_string := "a Homeric Might potion";
				32:	temp_string := "a Greater Homeric Might potion";
				33:	temp_string := "a Tamla Heal potion";
				34:	temp_string := "Taint's Minor Transmutation";
				35:	temp_string := "Taint's Major Transmutation";
			endcase;
	endcase;

	if (temp_string)
		potion.name := temp_string;
	else
		potion.name := "and unknown potion";
	endif

endfunction





