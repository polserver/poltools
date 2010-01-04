use uo;
use util;

include "include/dismount";
include "include/client";

function CanMod(who, Vitals, persistedprop := 0 )

	var modarray := "#mods";
	if ( persistedprop )
		modarray := "mods";
	endif

	var currentmods := GetObjProperty(who,modarray);
	if (!currentmods) return 1; endif
	foreach mmod in currentmods
		if ( mmod[1] = Vitals ) return 0; endif
	endforeach
		
	return 1;

endfunction

//for nonpersistedmods
function DoTempMod(who, Vitals, amt, duration)

	var allmods := GetObjProperty(who,"#mods");
	if (!allmods) allmods := {}; endif
	var mmod := {};
	mmod[1] := Vitals;
	mmod[2] := amt;
	mmod[3] := ReadGameClock()+duration;

	AddToVitalsMods(allmods, mmod);

	case (Vitals)
		"cstr":
        	"str" : who.strength_mod := who.strength_mod + mmod[2];
		"cdex":
        	"dex" : who.dexterity_mod := who.dexterity_mod + mmod[2];
		"cint":
        	"int" : who.intelligence_mod := who.intelligence_mod + mmod[2];
		"car":
        	"ar"  : who.ar_mod := who.ar_mod + mmod[2];
		"p"   :	who.SetParalyzed(1);
		"call":
        	"all" : who.strength_mod := who.strength_mod + mmod[2];
			who.dexterity_mod := who.dexterity_mod + mmod[2];
			who.intelligence_mod := who.intelligence_mod + mmod[2];
		"cpoly":
		"poly":
			dismount(who, 0);
			who.strength_mod := who.strength_mod + mmod[2];
			who.dexterity_mod := who.dexterity_mod + mmod[2];
			who.intelligence_mod := who.intelligence_mod + mmod[2];
			who.ar_mod := who.ar_mod + cint(mmod[2]/3);
	endcase

	SetObjProperty(who,"#mods",allmods);

	if (who.npctemplate)
		var parms := {};
		parms[1] := who;
		parms[2] := mmod[1];
		parms[3] := mmod[2];
		parms[4] := duration;
		parms[5] := 0;//nonpersisted mods
		start_script("::/misc/unmodnpc", parms);
	endif

	set_critical(0);

endfunction

//for name, graphic and color changes!
function DoPersistedMod(who, prop, newvalue, oldvalue, duration)

	set_critical(1);

	var allmods := GetObjProperty(who,"mods");
	if (!allmods) allmods := {}; endif
	var mmod := {};
	mmod[1] := prop;
	mmod[2] := oldvalue;
	mmod[3] := ReadGameClock()+duration;

	AddToPersistedMods(allmods, mmod);

	case (prop)
        	"title_prefix": 
			who.title_prefix := newvalue;
        	"title_suffix": 
			who.title_suffix := newvalue;
        	"name": 
			who.name := newvalue;
		"graphic":
			dismount(who, 0);
			who.graphic := newvalue;
		"color":
			who.color := newvalue;
		"gender":
			who.gender := newvalue;
		"spiritspeak":
			GrantPrivilege( who, "hearghosts" );
			who.enable( "hearghosts" );
		"hair":
			DestroyItem(GetEquipmentByLayer(who,LAYER_HAIR));
			if (newvalue)
				var newhair := CreateItemAtLocation(5288 , 1176 , 0, newvalue, 1);
				newhair.color := RandomHairColor();
				EquipItem(who,newhair);
			endif
		"beard":
			DestroyItem(GetEquipmentByLayer(who,LAYER_BEARD));
			if (newvalue)
				var newhair := CreateItemAtLocation(5288 , 1176 , 0, newvalue, 1);
				newhair.color := GetObjProperty(who,"beardcolor");
				EquipItem(who,newhair);
			endif
	endcase

	SetObjProperty(who,"mods",allmods);

	if (who.npctemplate)
		var parms := {};
		parms[1] := who;
		parms[2] := mmod[1];
		parms[3] := mmod[2];
		parms[4] := duration;
		parms[5] := 1;//persisted mod
		start_script("::/misc/unmodnpc", parms);
	endif

	set_critical(0);

endfunction

function RemoveNPCMod(parms)

	var who := parms[1];
	var prop := parms[2];
	var amount := parms[3];	//for persisted mods, this is the oldvalue
	var duration := parms[4];
	var persistedmod := parms[5];

	sleep( duration + 1);

	set_critical(1);
	if (who)
		CheckMods(who, persistedmod);
	else
		set_critical(0);
		return;
	endif

endfunction

function CheckMods(who, persisted)

//For Reference:
//	mmod[1] := Vitals;
//	mmod[2] := amt;
//	mmod[3] := ReadGameClock()+duration;

	set_critical(1);
	var now := ReadGameClock();
	var modarray := "#mods";
	if (persisted)
		modarray := "mods";
		if ( CanMod(who,"poison",1) or (!who.poisoned) )
			// this person should not be poisoned
			if ( who.hp_regen_rate != BaseRegenRate(who) )
				who.hp_regen_rate := BaseRegenRate(who);
				who.SetPoisoned(0);
			endif
		endif
	endif

	var newmods := {};
	var currentmods := GetObjProperty(who,modarray);
	if (!currentmods) set_critical(0); return; endif
	foreach mmod in currentmods
		if ( now > mmod[3] )
			UndoTheMod( who, mmod[1], mmod[2] );
		else
			newmods.append(mmod);
		endif
	endforeach

	if (len(newmods))
		SetObjProperty(who,modarray,newmods);
	else
		EraseObjProperty(who,modarray);
	endif

	set_critical(0);

endfunction

function UndoTheMod( who, prop, amt )

	var msg, sound;
	var oldvalue := amt;
	case (prop)
		"cstr":
		"str" :	who.strength_mod := who.strength_mod - amt; 
		"cdex":
		"dex" :	who.dexterity_mod := who.dexterity_mod - amt;
		"cint":
		"int" :	who.intelligence_mod := who.intelligence_mod - amt;
		"car":
		"ar"  :	who.ar_mod := who.ar_mod - amt;
				msg := "Armor is now " + who.ar;
				sound := SFX_1E3;
		"p"   :	who.SetParalyzed(0);
		"call":
		"all" :
				who.strength_mod := who.strength_mod - amt; 
				who.dexterity_mod := who.dexterity_mod - amt;
				who.intelligence_mod := who.intelligence_mod - amt;
		"cpoly":
		"poly":	
				who.ar_mod := who.ar_mod - cint(amt/3);
				msg := "Armor is now " + who.ar;
				sound := SFX_1E3;
				who.strength_mod := who.strength_mod - amt; 
				who.dexterity_mod := who.dexterity_mod - amt;
				who.intelligence_mod := who.intelligence_mod - amt;
        	"name"    : who.name := oldvalue;
				msg := "Your name is now " + who.name;
				sound := SFX_1E3;
		"graphic" : who.graphic := oldvalue;
		"color"   : who.color := oldvalue;
		"spiritspeak":
			if (!amt) RevokePrivilege( who, "hearghosts" ); endif
        	"title_prefix": 
			who.title_prefix := oldvalue;
        	"title_suffix": 
			who.title_suffix := oldvalue;
		"gender":
			who.gender := oldvalue;
		"hair":
			DestroyItem(GetEquipmentByLayer(who,LAYER_HAIR));
			if (oldvalue)
				var newhair := CreateItemAtLocation(5288 , 1176 , 0, oldvalue, 1);
				newhair.color := GetObjProperty(who,"haircolor");
				EquipItem(who,newhair);
			endif
		"beard":
			DestroyItem(GetEquipmentByLayer(who,LAYER_BEARD));
			if (oldvalue)
				var newhair := CreateItemAtLocation(5288 , 1176 , 0, oldvalue, 1);
				newhair.color := GetObjProperty(who,"haircolor");
				EquipItem(who,newhair);
			endif
		"poison":
			who.hp_regen_rate := BaseRegenRate(who);
			who.SetPoisoned(0);
	endcase

	if (sound) PlaySoundEffectPrivate(who, sound, who); endif
	if (msg) PrintTextAbovePrivate(who,msg,who); endif

endfunction

function GetModAmount( magery ) 

    var mod_amount := randomint(10) + (magery/6);
    return mod_amount;

endfunction

function GetModDuration( magery )

    var duration := 60 + ((magery / 15) * 60);
    return (duration);

endfunction

function AddToVitalsMods(byref allmods, byref mmod)

//For Reference:
//	mmod[1] := Vitals;
//	mmod[2] := amt;
//	mmod[3] := ReadGameClock()+duration;

	foreach oldmod in allmods

		if ( oldmod[1]=mmod[1] )
			if ( (mmod[2] > 0 and oldmod[2] > 0) or (mmod[2] < 0 and oldmod[2] < 0) )
				mmod[2] := 0;
			else
				oldmod[2] := oldmod[2] + mmod[2];
			endif
			oldmod[3] := mmod[3];
			return 0;
		endif

	endforeach

	allmods.append(mmod);
	return 1;

endfunction

function AddToPersistedMods(byref allmods, byref mmod)

//For Reference:
//	mmod[1] := prop;
//	mmod[2] := oldvalue;
//	mmod[3] := ReadGameClock()+duration;

//note:  If the player already has this mod, we don't want to 
//change the oldvalue - we just want to extend the duration.
//and do the mod:

	foreach oldmod in allmods
		if ( oldmod[1]=mmod[1] )
			mmod[2] := oldmod[2];//keep the original oldvalue
			oldmod[3] := mmod[3];//reset the duration
			return 0;
		endif
	endforeach

	allmods.append(mmod);
	return 1;

endfunction

function RandomHairColor()

	return RandomInt(46)+1102;

endfunction

function SetPoison(who, plvl, msg)
	//:poisoning:poisondamage is called, which calls
	//setthepoison() below.  This is so we can tweak poisondamage
	//on-the-fly, and only have to recompile the one script

	var parms := {};
	parms[1] := who;
	parms[2] := plvl;
	parms[3] := msg;

	start_script(":poisoning:poisondamage", parms);

endfunction

function SetThePoison(who, plvl, msg)

	if (ProtectedFromPoison(who)) return; endif

	plvl := cint(plvl);
	if (plvl < 1) return; endif
	if ( plvl > 5 ) plvl := RandomInt(3)+1; endif

	var currentmods := GetObjProperty(who,"mods");
	if (!currentmods) currentmods := { }; endif
	var current := 0;

	if (currentmods)
		current := GetCurrentPoisonLevel(who, currentmods);
	else
		currentmods := {};
	endif

	if (plvl < current) return; endif //they are already more poisoned

	set_critical(1);

	var currentpoisonregenmod := GetPoisonRegenMod(current);

	var newpoisonregenmod := GetPoisonRegenMod(plvl);

	who.hp_regen_rate := BaseRegenRate(who) + (newpoisonregenmod);

	var duration := 30+(20*plvl);

	var mmod := {};
	mmod[1] := "poison";
	mmod[2] := plvl;
	mmod[3] := ReadGameClock()+duration;

	if (!current)
		who.SetPoisoned(1);
		if ( msg ) PrintTextAbovePrivate(who,msg,who); endif
		currentmods.append(mmod);
	else
		AddToPoisonMod(currentmods, mmod);
	endif

	SetObjProperty(who,"mods",currentmods);

	set_critical(0);
	if (who.npctemplate)
		var parms := {};
		parms[1] := who;
		parms[2] := mmod[1];
		parms[3] := mmod[2];
		parms[4] := duration;
		parms[5] := 1;//persisted mods
		start_script("::/misc/unmodnpc", parms);
	endif

endfunction

function GetPoisonRegenMod(plvl)

	case (plvl)
		0: return 0;
		1: return -1000;
		2: return -1200;
		3: return -1500;
		4: return -1800;
		5: return -2500;
	endcase

endfunction

function GetCurrentPoisonLevel(who, byref currentmods)

	if (!len(currentmods)) return 0; endif
	foreach mmod in currentmods
		if ( mmod[1] = "poison" )
			return mmod[2];
		endif
	endforeach
		
	return 0;

endfunction

function AddToPoisonMod(byref currentmods, byref mmod)

	foreach oldmod in currentmods
		if (oldmod[1] = "poison")
			oldmod[2] := mmod[2];
			oldmod[3] := mmod[3];
			return;
		endif
	endforeach

	currentmods.append(mmod);

endfunction

function CurePoison(who, sound := 0, msg := 0)

	set_critical(1);
	var currentmods := GetObjProperty(who,"mods");
	if (!currentmods) set_critical(0); return; endif
	var newmods := {};
	foreach mmod in currentmods
		if ( mmod[1] = "poison" )
			UndoTheMod( who, mmod[1], mmod[2] );
		else
			newmods.append(mmod);
		endif
	endforeach

	if (len(newmods))
		SetObjProperty(who,"mods",newmods);
	else
		EraseObjProperty(who,"mods");
	endif

	set_critical(0);
	if (sound) PlaySoundEffectPrivate(who, sound, who); endif
	if (msg) PrintTextAbovePrivate(who,msg,who); endif

endfunction

function PoisonLevel(who)

	var currentmods := GetObjProperty(who,"mods");
	if (!currentmods)
		return 0;
	else
		return GetCurrentPoisonLevel(who, currentmods);
	endif

endfunction

function ProtectedFromPoison(who)

	var protserial := GetObjProperty(who,"#protpoison");
	if (!protserial)
		return 0;
	endif

	var item;
	foreach thing in ListEquippedItems(who)
		if ( thing.serial = protserial )
			item := thing;
			break;
		endif
	endforeach

	if (!item)
		return 0;
	endif

	var charges := GetObjProperty(item,"protpoison");
	if (!charges)
		eraseobjproperty(who,"#protpoison");
		eraseobjproperty(item,"protpoison");
		PrintTextAbovePrivate(item,item.desc + " is out of charges!",who);
		return 0;
	else
		charges := charges -1;
		setobjproperty(item,"protpoison",charges);
		EraseObjProperty(who, "poison_level");
		PrintTextAbovePrivate(who,item.desc + " protects you from the poison!",who);
		return 1;
	endif

endfunction


function BaseRegenRate(who)

	//have equipped items add/subtract from the character's cprop "baseregen"
	//otherwise it will always be reset to 100:

	var baseregen := GetObjProperty(who,"baseregen");
	if (!baseregen) return 100; endif
	return baseregen;

endfunction
