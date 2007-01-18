use cfgfile;

include ":attributes:attributes";

const POISON_SOUND := 0x247;
const DRINK_SOUND := 0x32;
const UOBJ_EMPTY_BOTTLE := 0x0f0e;
const EFFECT_BLUE_SPARKLE  := 0x373a;
const SOUND_EFFECT_CURE	:= 0x1e1;
const EFFECT_SPARKLE  := 0x374a;
const HIT_ACTION := 0x148b;
const MOVING_EFFECT_PURPLE_POTION  := 0x0f0d;
const SOUND_EFFECT_EXPLOSION	:= 0x208;
const SOUND_EFFECT_HEAL	:= 0x1f3;
const EFFECT_EXPLOSION := 0x36b0;

function CheckToDrink(character, type)
	// Check to make sure they have at least one hand free to drink the potion
	var equippedright := GetEquipmentByLayer(character, 0x01);
	var equippedleft := GetEquipmentByLayer(character, 0x02);
	var cfg := ReadConfigFile(":*:itemdesc");
	var twohanded := Cint(cfg[equippedleft.objtype].TwoHanded);
	if( equippedright && equippedleft || twohanded )
		SendSysMessage(character, "You must have at least one hand free to drink a potion.");
		return 0;
	endif
	if( (type != "#red") && (type != "#green_potion") )
		if( GetObjProperty(character, type) )
			if( CInt(GetObjProperty(character, type)) > ReadGameClock() )
				SendSysMessage(character, "You must wait until you can drink another potion.");
				return 0;
			else
				SetObjProperty(character, type, ReadGameClock() + 10);
				return 1;
			endif
		else
			SetObjProperty(character, type, ReadGameClock() + 10);
			return 1;
		endif
	else
		return 1;
	endif
endfunction

function empty_bottle(character, potion)
	PlaySoundEffect(character, DRINK_SOUND);
	CreateItemInBackpack(character, UOBJ_EMPTY_BOTTLE, 1);
	DestroyItem(potion);
endfunction

function is_poison(potion)
	var poison_level := GetObjProperty(potion, "poison_level");
	return poison_level;
endfunction
