////////////////////////////////////////////////////////////////////////////////
//													//
// 	This Include contains the following Functions.					//
//													//
// 	IsDoor(door) 		IsHair(hair) 		IsFacialHair(beard)	//
// 	IsJewelry(jewelry) 	IsWeapon(weapon)	 	IsWoodenWeapon(weapon)	//
// 	IsMetalWeapon(weapon) 	IsBow(weapon) 		IsArmor(armor)		//
//	IsLeatherArmor(armor) 	IsMetalArmor(armor)	IsShield(shield)		//
//	IsMetalShield(shield) 	IsWoodenShield(Shield)	IsClothing(clothes)	//
//													//
//													//
//													//
////////////////////////////////////////////////////////////////////////////////

include "include/objtype";
///////////////////
//  leather armor
///////////////////

Const UOBJ_LEATHER_GORGET	:= 0x13C7;
Const UBOJ_LEATHER_SLEEVES	:= 0x13C5;
Const UOBJ_LEATHER_SLEEVES2	:= 0x13CD;
Const UOBJ_LEATHER_GLOVES	:= 0x13C6;
Const UOBJ_LEATHER_CAP		:= 0x1DB9;
Const UOBJ_LEATHER_LEGGINGS	:= 0x13CB;
Const UOBJ_LEATHER_TUNIC		:= 0x13CC;
Const UOBJ_LEATHER_TUNIC2	:= 0x13D3;
Const UOBJ_LEATHER_FEMALE	:= 0x1C06;
Const UOBJ_LEATHER_SKIRT		:= 0x1C08;
Const UOBJ_LEATHER_BUSTIER	:= 0x1C0A;
Const UOBJ_LEATHER_SHORTS	:= 0x1C00;




///////////////////
//  studded armor
///////////////////

Const UOBJ_STUDDED_CAP		:= 0x708b;
Const UOBJ_STUDDED_GORGET	:= 0x13D6;
Const UOBJ_STUDDED_GLOVES	:= 0x13D5;
Const UOBJ_STUDDED_SLEEVES	:= 0x13D4;
Const UOBJ_STUDDED_SLEEVES2	:= 0x13DC;
Const UOBJ_STUDDED_LEGGINGS	:= 0x13DA;
Const UOBJ_STUDDED_TUNIC		:= 0x13DB;
Const UOBJ_STUDDED_FEMALE	:= 0x1C02;
Const UOBJ_STUDDED_BUSTIER	:= 0x1C0C;




///////////////////
//  ringmail armor
///////////////////

Const UOBJ_RINGMAIL_COIF		:= 0x708c;
Const UOBJ_RINGMAIL_TUNIC	:= 0x13ec;
Const UOBJ_RINGMAIL_TUNIC2	:= 0x13ed;
Const UOBJ_RINGMAIL_SLEEVES	:= 0x13ee;
Const UOBJ_RINGMAIL_SLEEVES2	:= 0x13ef;
Const UOBJ_RINGMAIL_LEGGINGS	:= 0x13f0;
Const UOBJ_RINGMAIL_LEGGINGS2	:= 0x13f1;
Const UOBJ_RINGMAIL_GLOVES	:= 0x13f2;
Const UOBJ_RINGMAIL_GLOVES2	:= 0x13eb;



///////////////////
//  chainmail armor
///////////////////

Const UOBJ_CHAINMAIL_COIF	:= 0x13bb;
Const UOBJ_CHAINMAIL_COIF2	:= 0x13c0;
Const UOBJ_CHAINMAIL_LEGGINGS	:= 0x13be;
Const UOBJ_CHAINMAIL_LEGGINGS2:= 0x13c3;
Const UOBJ_CHAINMAIL_TUNIC	:= 0x13bf;
Const UOBJ_CHAINMAIL_TUNIC2	:= 0x13c4;
Const UOBJ_CHAINMAIL_GLOVES	:= 0x7066;




///////////////////
//  Platemail armor
///////////////////

Const UOBJ_PLATE_FEMALE		:= 0x1c04;
Const UOBJ_PLATE_FEMALE2		:= 0x1c05;
Const UOBJ_PLATE_BREASTPLATE	:= 0x1415;
Const UOBJ_PLATE_BREASTPLATE2	:= 0x1416;
Const UOBJ_PLATE_ARMS		:= 0x1410;
Const UOBJ_PLATE_ARMS2		:= 0x1417;
Const UOBJ_PLATE_LEGS		:= 0x1411;
Const UOBJ_PLATE_LEGS2		:= 0x141a;
Const UOBJ_PLATE_GLOVES		:= 0x1414;
Const UOBJ_PLATE_GLOVES2		:= 0x1418;
Const UOBJ_PLATE_GORGET		:= 0x1413;




///////////////////
//  Bone armor
///////////////////

Const UOBJ_BONE_ARMS		:= 0x144e;
Const UOBJ_BONE_TUNIC		:= 0x144f;
Const UOBJ_BONE_GLOVES		:= 0x1450;
Const UOBJ_BONE_HELM		:= 0x1451;
Const UOBJ_BONE_LEGS		:= 0x1452;




///////////////////
//  helms
///////////////////

Const UOBJ_PLATE_HELM		:= 0x1412;
Const UOBJ_PLATE_HELM2		:= 0x1419;
Const UOBJ_CLOSEHELM		:= 0x1408;
Const UOBJ_CLOSEHELM2		:= 0x1409;
Const UOBJ_HELMET			:= 0x140a;
Const UOBJ_HELMET2			:= 0x140b;
Const UOBJ_BASCINET			:= 0x140c;
Const UOBJ_BASCINET2		:= 0x140d;
Const UOBJ_NOSEHELM			:= 0x140e;
Const UOBJ_NOSEHELM2		:= 0x140f;




///////////////////
//  shields
///////////////////

Const UOBJ_BUCKLER			:= 0x1b73;
Const UOBJ_BRONZESHIELD		:= 0x1b72;
Const UOBJ_METALSHIELD		:= 0x1b7b;
Const UOBJ_KITESHIELD		:= 0x1b74;
Const UOBJ_KITESHIELD2		:= 0x1b75;
Const UOBJ_HEATER			:= 0x1b76;
Const UOBJ_HEATER2			:= 0x1b77;
Const UOBJ_WOODEN_SHIELD		:= 0x1b7a;
Const UOBJ_WOODEN_KITESHIELD	:= 0x1b78;
Const UOBJ_WOODEN_KITESHIELD2	:= 0x1b79;
Const UOBJ_CHAOS_SHIELD		:= 0x1bc3;
Const UOBJ_ORDER_SHIELD		:= 0x1bc4;
Const UOBJ_ORDER_SHIELD2		:= 0x1bc5;




///////////////////
//  woodeden weapons
///////////////////

Const UOBJ_CLUB			:= 0x13b3;
Const UOBJ_SHEEPHERDS_CROOK	:= 0x0e81;
Const UOBJ_BLACK_STAFF		:= 0x0df0;
Const UOBJ_GNARLED_STAFF		:= 0x13f9;
Const UOBJ_MAGE_STAFF		:= 0x7065;
Const UOBJ_DRUID_STAFF		:= 0x7068;
Const UOBJ_NECRO_STAFF		:= 0x706a;
Const UOBJ_QUARTER_STAFF		:= 0x0e8a;

///////////////////
// Some weapons
///////////////////

Const UOBJ_FIST			:= 0xF020;
Const UOBJ_DAGGER			:= 0x0f51;
Const UOBJ_SKINNING_KNIFE	:= 0x0ec4;



function IsDoor (byref door)

	if (door.objtype >= 0xe8 and door.objtype <= 0xf7)
		return 1;
	elseif (door.objtype >= 0x314 and door.objtype <= 0x365)
		return 1;
	elseif (door.objtype >= 0x675 and door.objtype <= 0x6f4)
		return 1;
	elseif (door.objtype >= 0x824 and door.objtype <= 0x833)
		return 1;
	elseif (door.objtype >= 0x839 and door.objtype <= 0x848)
		return 1;
	elseif (door.objtype >= 0x84c and door.objtype <= 0x85b)
		return 1;
	elseif (door.objtype >= 0x866 and door.objtype <= 0x875)
		return 1;
	elseif (door.objtype >= 0x1fed and door.objtype <= 0x1ffc)
		return 1;
	endif

	return 0;

endfunction

function IsHair (byref hair)

	case (hair.objtype)
		0x203b:	//Short Hair
		0x203c:	//Long Hair 
		0x203d:	//Pony Tail
		0x2044:	//MoHawk
		0x2045:	//Page Boy
		0x2046:	//Buns
		0x2047:	//Afro
		0x2048:	//Receding
		0x2049:	//Pigtails
		0x204a:	//Krisna
		return 1;
		default: return 0;
	endcase

endfunction

function IsFacialHair (byref beard)

	case (beard.objtype)
		0x203e:	//Long Beard
		0x203f:	//Short Beard
		0x2040:	//Goatee
		0x2041:	//Mustache
		0x204b:	//Short beard w/mustache
		0x204c:	//Long beard w/mustache
		0x204d:	//Vandyke
		return 1;
	endcase
		return 0;
endfunction

function IsJewelry (byref jewelry)

	if (jewelry.objtype >= 0x1085 and jewelry.objtype <= 0x108A)
		return 1;
	elseif (jewelry.objtype >= 0x1F05 and jewelry.objtype <= 0x1F0A)
		return 1;
	endif
		return 0;
endfunction

function IsWeapon (byref weapon)

	if (weapon.objtype >= 0x0DF0 and weapon.objtype <= 0x0DF5)
		return 1;
	elseif (weapon.objtype >= 0x0E81 and weapon.objtype <= 0x0E8A and weapon.objtype != 0x0E83)
		return 1;
	elseif (weapon.objtype >= 0x0EC2 and weapon.objtype <= 0x0EC5)
		return 1;
	elseif (weapon.objtype >= 0x0F43 and weapon.objtype <= 0x0F52)
		return 1;
	elseif (weapon.objtype >= 0x0F5C and weapon.objtype <= 0x0F63)
		return 1;
	elseif (weapon.objtype >= 0x0FB4 and weapon.objtype <= 0x0FB5)
		return 1;
	elseif (weapon.objtype >= 0x13AF and weapon.objtype <= 0x13BA)
		return 1;
	elseif (weapon.objtype >= 0x13E3 and weapon.objtype <= 0x13E4)
		return 1;
	elseif (weapon.objtype >= 0x13F4 and weapon.objtype <= 0x1407)
		return 1;
	elseif (weapon.objtype >= 0x1438 and weapon.objtype <= 0x1443)
		return 1;
	endif
		return 0;
endfunction

function IsWoodenWeapon (byref weapon)

	if (weapon.objtype >= 0x0DF0 and weapon.objtype <= 0x0DF1)
		return 1;
	elseif (weapon.objtype >= 0x0E81 and weapon.objtype <= 0x0E82)
		return 1;
	elseif (weapon.objtype >= 0x0E89 and weapon.objtype <= 0x0E8A)
		return 1;
	elseif (weapon.objtype >= 0x0F4F and weapon.objtype <= 0x0F50)
		return 1;
	elseif (weapon.objtype >= 0x13B1 and weapon.objtype <= 0x13B4)
		return 1;
	elseif (weapon.objtype >= 0x13F4 and weapon.objtype <= 0x13F5)
		return 1;
	elseif (weapon.objtype >= 0x13F8 and weapon.objtype <= 0x13F9)
		return 1;
	elseif (weapon.objtype >= 0x13FC and weapon.objtype <= 0x13FD)
		return 1;
	elseif (weapon.objtype = 0x7068 or weapon.objtype = 0x706a)
		return 1;
	elseif (weapon.objtype = 0x7065)
		return 1;
	endif
		return 0;
endfunction

function IsMetalWeapon (byref weapon)

	if (weapon.objtype >= 0x0DF2 and weapon.objtype <= 0x0DF5)
		return 1;
	elseif (weapon.objtype >= 0x0E85 and weapon.objtype <= 0x0E88)
		return 1;
	elseif (weapon.objtype >= 0x0F43 and weapon.objtype <= 0x0F4E)
		return 1;
	elseif (weapon.objtype >= 0x0F51 and weapon.objtype <= 0x0F52)
		return 1;
	elseif (weapon.objtype >= 0x0F5C and weapon.objtype <= 0x0F63)
		return 1;
	elseif (weapon.objtype >= 0x0FB4 and weapon.objtype <= 0x0FB5)
		return 1;
	elseif (weapon.objtype >= 0x13AF and weapon.objtype <= 0x13B0)
		return 1;
	elseif (weapon.objtype >= 0x13B5 and weapon.objtype <= 0x13BA)
		return 1;
	elseif (weapon.objtype >= 0x13E3 and weapon.objtype <= 0x13E4)
		return 1;
	elseif (weapon.objtype >= 0x13F6 and weapon.objtype <= 0x13F7)
		return 1;
	elseif (weapon.objtype >= 0x13FA and weapon.objtype <= 0x13FB)
		return 1;
	elseif (weapon.objtype >= 0x13FE and weapon.objtype <= 0x1407)
		return 1;
	elseif (weapon.objtype >= 0x1438 and weapon.objtype <= 0x1443)
		return 1;
	endif
		return 0;
endfunction

function IsBow (byref weapon)

	if (weapon.objtype >= 0x0F4F and weapon.objtype  <= 0x0F50)
		return 1;
	elseif (weapon.objtype >= 0x13B1 and weapon.objtype <= 0x13B2)
		return 1;
	elseif (weapon.objtype >= 0x13FC and weapon.objtype <= 0x13FD)
		return 1;
	elseif (weapon.objtype = 0x6050)
		return 1;
	endif
		return 0;
endfunction

function IsArmor (byref armor)

	if (armor.objtype = 0x13BB)
		return 1;
	elseif (armor.objtype >= 0x13BE and armor.objtype <= 0x13C0)
		return 1;
	elseif (armor.objtype >= 0x13C3 and armor.objtype <= 0x13C7)
		return 1;
	elseif (armor.objtype >= 0x13CB and armor.objtype <= 0x13CE)
		return 1;
	elseif (armor.objtype >= 0x13D2 and armor.objtype <= 0x13D6)
		return 1;
	elseif (armor.objtype >= 0x13DA and armor.objtype <= 0x13DD)
		return 1;
	elseif (armor.objtype >= 0x13E1 and armor.objtype <= 0x13E2)
		return 1;
	elseif (armor.objtype >= 0x13EB and armor.objtype <= 0x13F2)
		return 1;
	elseif (armor.objtype >= 0x1408 and armor.objtype <= 0x141A)
		return 1;
	elseif (armor.objtype >= 0x144E and armor.objtype <= 0x1457)
		return 1;
	elseif (armor.objtype >= 0x1C00 and armor.objtype <= 0x1C0D)
		return 1;
	elseif (armor.objtype >= 0x1F0B and armor.objtype <= 0x1F0C)
		return 1;
	endif
		return 0;
endfunction

function IsLeatherArmor (byref armor)

	if (armor.objtype >= 0x13C5 and armor.objtype <= 0x13C7)
		return 1;
	elseif (armor.objtype >= 0x13CB and armor.objtype <= 0x13CE)
		return 1;
	elseif (armor.objtype >= 0x13D2 and armor.objtype <= 0x13D6)
		return 1;
	elseif (armor.objtype >= 0x13DA and armor.objtype <= 0x13DD)
		return 1;
	elseif (armor.objtype >= 0x13E1 and armor.objtype <= 0x13E2)
		return 1;
	elseif (armor.objtype >= 0x144E and armor.objtype <= 0x1457)
		return 1;
	elseif (armor.objtype >= 0x1C00 and armor.objtype <= 0x1C03)
		return 1;
	elseif (armor.objtype >= 0x1C06 and armor.objtype <= 0x1C0D)
		return 1;
	elseif (armor.objtype = 0x1DB9 or armor.objtype = 0x1DBA)
		return 1;
	endif
		return 0;
endfunction

function IsMetalArmor (byref armor)

	if (armor.objtype = 0x13BB)
		return 1;
	elseif (armor.objtype >= 0x13BE and armor.objtype <= 0x13C0)
		return 1;
	elseif (armor.objtype >= 0x13C3 and armor.objtype <= 0x13C4)
		return 1;
	elseif (armor.objtype >= 0x13EB and armor.objtype <= 0x13F2)
		return 1;
	elseif (armor.objtype >= 0x1408 and armor.objtype <= 0x141A)
		return 1;
	elseif (armor.objtype >= 0x1c04 and armor.objtype <= 0x1c05)
		return 1;
	elseif (armor.objtype = UOBJ_CHAINMAIL_GLOVES)
		return 1;
	elseif (armor.objtype = UOBJ_RINGMAIL_COIF)
		return 1;
	endif
	return 0;
endfunction

function IsShield (byref shield)

	if (shield.objtype >= 0x1B72 and shield.objtype <= 0x1B7B)
		return 1;
	elseif (shield.objtype >= 0x1BC3 and shield.objtype <= 0x1BC5)
		return 1;
	endif
		return 0;
endfunction

function IsMetalShield (byref shield)

	if (shield.objtype >= 0x1B72 and shield.objtype <= 0x1B77)
		return 1;
	elseif (shield.objtype = 0x1B7B)
		return 1;
	elseif (shield.objtype >= 0x1BC3 and shield.objtype <= 0x1BC5)
		return 1;
	endif
		return 0;
endfunction

function IsWoodenShield (byref shield)

	if (shield.objtype  >= 0x1B78 and shield.objtype <= 0x1B7A)
		return 1;
	endif
		return 0;
endfunction

function IsClothing (byref clothes)

	if (clothes.objtype >= 0x1515 and clothes.objtype <= 0x1518)
 		return 1;
	elseif (clothes.objtype >= 0x152E and clothes.objtype <= 0x1531)
 		return 1;
	elseif (clothes.objtype >= 0x1537 and clothes.objtype <= 0x1544)
		return 1;
	elseif (clothes.objtype >= 0x1708 and clothes.objtype <= 0x171C)
		return 1;
	elseif (clothes.objtype >= 0x1EFD and clothes.objtype <= 0x1F04)
		return 1;
	elseif (clothes.objtype >= 0x1F7B and clothes.objtype <= 0x1F7C)
		return 1;
	elseif (clothes.objtype >= 0x1F9F and clothes.objtype <= 0x1FA2)
		return 1;
	elseif (clothes.objtype >= 0x1FFD and clothes.objtype <= 0x1FFE)
		return 1;
	elseif (clothes.objtype >= 0x204E and clothes.objtype <= 0x204F)
		return 1;
	endif
		return 0;
endfunction

function IsMusicalInstrument (byref item)

	case(item.objtype)
		0x7067:    //FLUTE	
		0x0e9d:    //TAMBORINE
		0x0e9e:    //TAMBORINE2
		0x0eb3:    //LUTE
		0x0eb4:    //LUTE2	
		0x0e9c:    //DRUM
		0x0eb2:    //HARP	
		0x0eb1:    //STANDING HARP
		return 1;
		default: return 0;
	endcase
endfunction

///////////////////////////Needs work////////////////////////
/////////////////////////////////////////////////////////////
//////////////////////Finish me NOW!/////////////////////////
/////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////
function IsReagent (byref item)

	case(item.objtype)
////////////////// 8 Original Reagents ///////////////
		0x0F7A:	//BLACK PEARL
		0x0F7B:	//BLOOD MOSS
		0x0F84:	//GARLIC
		0x0F85:	//GINSENG
		0x0F86:	//MANDRAKE ROOT
		0x0F88:	//NIGHTSHADE
		0x0F8C:	//SULFUROUS ASH
		0x0F8D:	//SPIDER SILK

//////////////////// Newer Reagents //////////////////
		0x0F78:	//BATWING
		0x0f79:	//BLACKMOOR
		0x0F7C:	//BLOODSPAWN
		0x0F7E:	//BONE
		0x0F7F:	//BRIMSTONE
		0x0F80:	//DAEMON BONE
		0x0F90:	//DEAD WOOD
		0x0F82:	//DRAGONS BLOOD
		0x0F83:	//EXECUTIONERS CAP
		0x0F81:	//FERTILE DIRT
		0x0F8E:	//SERPENT SCALE
		0x0F7D:	//VIAL OF BLOOD
		0x0F8F:	//VOLCANIC ASH
		0x0F91:	//WYRMS HEART
		return 1;
		default: return 0;
	endcase
endfunction




///////////////////
//  Determines if the item passed is a type of metal ingot
///////////////////

function IsIngot (byref use_on)
	case (use_on)
		UOBJ_IRON_INGOT:		return 1;
		UOBJ_BRONZE_INGOT:		return 1;
		UOBJ_DULLCOPPER_INGOT:	return 1;
		UOBJ_COPPER_INGOT:		return 1;
		UOBJ_ONYX_INGOT:		return 1;
		UOBJ_PYRITE_INGOT:		return 1;
		UOBJ_MALACHITE_INGOT:	return 1;
		UOBJ_AZURITE_INGOT:		return 1;
		UOBJ_BLUESTEEL_INGOT:	return 1;
		UOBJ_RUBYSTEEL_INGOT:	return 1;
		default:				return 0;
	endcase;
	return 0;
endfunction




///////////////////
//  determines if the item can stack with other items that the character already has
///////////////////

function IsStackableItem (byref item)
	if (item.amount > 1)
		return 1;
	endif

	case (item.objtype)
		UOBJ_GOLD_COIN:
		UOBJ_BATWING:
		UOBJ_BLACKMOOR:
		UOBJ_BLACK_PEARL:
		UOBJ_BLOOD_MOSS:
		UOBJ_BLOODSPAWN:
		UOBJ_VIAL_OF_BLOOD:
		UOBJ_BONE:
		UOBJ_BRIMSTONE:
		UOBJ_DAEMON_BONE:
		UOBJ_FERTILE_DIRT:
		UOBJ_DRAGONS_BLOOD:
		UOBJ_EXECUTIONERS_CAP:
		UOBJ_GARLIC:
		UOBJ_GINSENG:
		UOBJ_MANDRAKE_ROOT:
		UOBJ_EYE_OF_NEWT:
		UOBJ_NIGHTSHADE:
		UOBJ_OBSIDIAN:
		UOBJ_PIG_IRON:
		UOBJ_PUMICE:
		UOBJ_SULFUROUS_ASH:
		UOBJ_SPIDER_SILK:
		UOBJ_SERPENT_SCALE:
		UOBJ_VOLCANIC_ASH:
		UOBJ_DEAD_WOOD:
		UOBJ_WYRMS_HEART:
		UOBJ_GEM_AMBER:
		UOBJ_GEM_STAR_SAPPHIRE:
		UOBJ_GEM_EMERALD:
		UOBJ_GEM_SAPPHIRE:
		UOBJ_GEM_RUBY:
		UOBJ_GEM_CITRINE:
		UOBJ_GEM_AMETHYST:
		UOBJ_GEM_TOURMALINE:
		UOBJ_GEM_DIAMOND:
		UOBJ_FEATHER:
		UOBJ_LIGHT_HEAL_POTION:
		UOBJ_HEAL_POTION:
		UOBJ_GREATER_HEAL_POTION:
		UOBJ_LESSER_POISON_POTION:
		UOBJ_POISON_POTION:
		UOBJ_GREATER_POISON_POTION:
		UOBJ_DEADLY_POISON_POTION:
		UOBJ_LESSER_EXPLOSION_POTION:
		UOBJ_EXPLOSION_POTION:
		UOBJ_GREATER_EXPLOSION_POTION:
		UOBJ_LESSER_AGILITY_POTION:
		UOBJ_AGILITY_POTION:
		UOBJ_GREATER_AGILITY_POTION:
		UOBJ_REFRESH_POTION:
		UOBJ_FULL_REFRESH_POTION:
		UOBJ_LESSER_STRENGTH_POTION:
		UOBJ_STRENGTH_POTION:
		UOBJ_GREATER_STRENGTH_POTION:
		UOBJ_LESSER_CURE_POTION:
		UOBJ_CURE_POTION:
		UOBJ_GREATER_CURE_POTION:
		UOBJ_NIGHTSIGHT_POTION:
		UOBJ_EGG:
          UOBJ_HIDE:
          UOBJ_BROWN_HIDE:
          UOBJ_RED_HIDE:
          UOBJ_BLUE_HIDE:
          UOBJ_GREEN_HIDE:
		UOBJ_SILVER_HIDE:
		UOBJ_BANDAGE:
		UOBJ_ARROW:
		UOBJ_BOLT:
		UOBJ_REAPER_HEARTWOOD:
		UOBJ_ENT_HEARTWOOD:
		UOBJ_ENCHANTED_BONE:
          UOBJ_RAW_RIB:
          UOBJ_RAW_FISHSTEAK:
          UOBJ_RAW_BIRD:
          UOBJ_RAW_HAM:
          UOBJ_BACON_SLAB:
          UOBJ_RAW_SAUSAGE:
          UOBJ_HONEY:
          UOBJ_CARROT:
          UOBJ_APPLE:
          UOBJ_PEACH:
          UOBJ_LEMON:
          UOBJ_LIME:
          UOBJ_BANANA:
          UOBJ_PEAR:
          UOBJ_GRAPES:
          UOBJ_DATES:
          UOBJ_PUMPKIN:
          UOBJ_LETTUCE:
          UOBJ_CABBAGE:
          UOBJ_ONION:
          UOBJ_TURNIP:
          UOBJ_COCONUT:
          UOBJ_RAW_CHICKEN_LEG:
          UOBJ_RAW_LEG_OF_LAMB:
			return 1;
	endcase

	//Already went through the common stackable items and still haven't been able to figure out if its stackable or not, so
	//we'll see if we can create 2 of the item as a last resort.
	set_critical(1);
	var newitem := CreateItemAtLocation (0, 0, 0, item.objtype, 2);
	if (newitem)
		DestroyItem (newitem);
		set_critical(0);
		return 1;
	endif
	set_critical(0);
	return 0;
endfunction

function IsLeatherBoots (byref item)
	if (item.objtype = 0x170b or item.objtype = 0x1711)
		return 1;
	endif
	return 0;
endfunction

