///////////////////////////////
//
// creature_spellcast.inc
//
// creature spellcasting routines
//
// Description: one spell per function. Most functions are functionname(caster, target), w
// here target can be either an object or a location (depends on the spell).
// where applicable, and where I implemented it, spell reflection is used. There are still
// some that don't have reflection implemented.
//
// Note: we still need to set up a script for summoned creatures to use, so they'll fight
// for their masters
//
// TODO: add spell reflect code, add die roll damage, add other spells
//
//////////////////////////////

use uo;
use os;
use npc;
use util;
use basic;
use cfgfile;

include "include/client";
include "include/attributes";
include "include/statMod";
include "include/tempMod";
include "include/res";
include "include/resPenalty";

const REAGENT_START := 0x0f84;
const REAGENT_STOP := 0x0f8d;
const UOBJECT_CORPSE := 0x2006;

const MOVING_EFFECT_FIREBALL  := 0x36d4;
const MOVING_EFFECT_MAGIC_ARROW  := 0x36e4;
const MOVING_EFFECT_HARM  := 0x36e4;
const MOVING_EFFECT_FLAMESTRIKE := 0x3709;
const MOVING_EFFECT_EBOLT       := 0x379f;
const EFFECT_SPARKLE  := 0x373a;
const EFFECT_RED_SPARKLE  := 0x374a;

const SOUND_EFFECT_HEAL := 0x1f3;
const SOUND_EFFECT_AGILITY := 0x1e8;
const SOUND_EFFECT_STRENGTH := 0x1ef;
const SOUND_EFFECT_REFLECT := 0x1ee;
const SOUND_EFFECT_POISON := 0x247;
const SOUND_EFFECT_MINDBLAST := 0x214;
const SOUND_EFFECT_FIREBALL  := 0x160;
const sound_effect_curse := 0x1fc;
const SOUND_EFFECT_MAGIC_ARROW	:= 0x01e6;
const SOUND_EFFECT_LIGHTNING  := 0x2a;
const SOUND_EFFECT_HARM := 0x1f2;
const SOUND_EFFECT_FLAMESTRIKE  := 0x209;
const SOUND_EFFECT_FEEBLEMIND := 0x1e5;
const SOUND_EFFECT_EBOLT        := 0x20b;
const SOUND_EFFECT_DISPEL  := 0x02;
const SOUND_EFFECT_CURE := 0x1e1;
const SOUND_EFFECT_CUNNING := 0x1ec;
const sound_effect_clumsy := 0x1e1;
const sound_effect_bless := 0x1eb;
const SOUND_EFFECT_WEAKEN := 0x1e7;




function wears_metallic(who)
  var cfg := ReadConfigFile(":combat:itemdesc");
  var item;
  foreach item in ListEquippedItems(who)
    if((cfg[item.objtype].blockcircle) && (cfg[item.objtype].blockcircle <= 2) && (item.layer > 2))
      print(who.name + " was wearing metallic armor, and is gonna be fried");
      return 1;
    endif
  endforeach
  return 0;
endfunction


/////////////
// Agility //
/////////////
function cast_AGILITY( caster, cast_on )
  if(!CheckLineOfSight( caster, cast_on ))
    return;
  endif
  if(!ConsumeMana(me, 9))
    //return 0;
  endif
  if(!ConsumeReagents(me, 9))
    //return 0;
  endif
  if(saywords)
    SpeakPowerWords( me, 9);
  endif
  PlaySoundEffect(caster, SOUND_EFFECT_AGILITY);
  PlaySoundEffect(cast_on, SOUND_EFFECT_AGILITY);
  PlayObjectCenteredEffect( cast_on, EFFECT_SPARKLE, 7,7);
  var magery := GetEffectiveSkill(caster, SKILLID_MAGERY);
  var mod_amount := GetModAmount( magery );
  var duration := GetModDuration( magery, mod_amount );
  SetDexterityMod(cast_on, GetDexterityMod(cast_on) + mod_amount);
  do_temp_mod(cast_on, "dex", mod_amount,  duration);
endfunction

//////////////
// Strength //
//////////////
function cast_strength( caster, cast_on )
  if(!CheckLineOfSight( caster, cast_on ))
    return;
  endif
  if(!ConsumeMana(me, 16))
    //return 0;
  endif
  if(!ConsumeReagents(me, 16))
    //return 0;
  endif
  if(saywords)
    SpeakPowerWords( me, 16);
  endif
  PlaySoundEffect(caster, SOUND_EFFECT_STRENGTH);
  PlaySoundEffect(cast_on, SOUND_EFFECT_STRENGTH);
  PlayObjectCenteredEffect( cast_on, EFFECT_SPARKLE, 7,7);
  var magery := GetEffectiveSkill(caster, SKILLID_MAGERY);
  var mod_amount := GetModAmount( magery );
  var duration := GetModDuration( magery, mod_amount );
  Detach();
  SetStrengthMod(cast_on, GetStrengthMod(cast_on) + mod_amount);
  sleep(duration);
  SetStrengthMod(cast_on, GetStrengthMod(cast_on) - mod_amount);
endfunction

/////////////
// Cunning //
/////////////
function cast_CUNNING( caster, cast_on )
  if(!CheckLineOfSight( caster, cast_on ))
     return;
  endif
  if(!ConsumeMana(me, 10))
    //return 0;
  endif
  if(!ConsumeReagents(me, 10))
    //return 0;
  endif
  if(saywords)
    SpeakPowerWords(me, 10);
  endif
  PlaySoundEffect(caster, SOUND_EFFECT_CUNNING);
  PlaySoundEffect(cast_on, SOUND_EFFECT_CUNNING);
  PlayObjectCenteredEffect( cast_on, EFFECT_SPARKLE, 7,7);
  var magery := GetEffectiveSkill(caster, SKILLID_MAGERY);
  var mod_amount := GetModAmount( magery );
  var duration :=   GetModDuration( magery, mod_amount );
  Detach();
  SetIntelligenceMod(cast_on, GetIntelligenceMod(cast_on) + mod_amount);
  sleep(duration);
  SetIntelligenceMod(cast_on, GetIntelligenceMod(cast_on) - mod_amount);
endfunction

////////////
// Clumsy //
////////////
function cast_clumsy( caster, cast_on )
  if(!CheckLineOfSight(caster, cast_on))
    return;
  endif
  if(!ConsumeMana(me, 1))
    //return 0;
  endif
  if(!ConsumeReagents(me, 1))
    //return 0;
  endif
  if(saywords)
    SpeakPowerWords( me, 1);
  endif
  PlaySoundEffect(caster, sound_effect_clumsy);
  PlaySoundEffect(cast_on, sound_effect_clumsy);
  PlayObjectCenteredEffect( cast_on, EFFECT_RED_SPARKLE, 7,7);
  var magery := GetEffectiveSkill(caster, SKILLID_MAGERY);
  var mod_amount := GetModAmount( magery );
  var duration := GetModDuration( magery, mod_amount );
  Detach();
  SetDexterityMod(cast_on, GetDexterityMod(cast_on) - mod_amount);
  do_temp_mod(cast_on, "dex", 0-mod_amount, duration);
endfunction

////////////
// Weaken //
////////////
function cast_WEAKEN( caster, cast_on)
  if(!CheckLineOfSight(caster, cast_on))
    return;
  endif
  if(!ConsumeMana(me, 8))
     //return 0;
  endif
  if(!ConsumeReagents(me, 8))
    //return 0;
  endif
  if(saywords)
    SpeakPowerWords( me, 8);
  endif
  PlaySoundEffect(cast_on, SOUND_EFFECT_WEAKEN);
  PlayObjectCenteredEffect( cast_on, EFFECT_RED_SPARKLE, 7,7);
  var magery := GetEffectiveSkill(caster, SKILLID_MAGERY);
  var mod_amount := GetModAmount( magery );
  var duration := GetModDuration( magery, mod_amount );
  SetStrengthMod(cast_on, GetStrengthMod(cast_on) - mod_amount);
  do_temp_mod(cast_on, "str", 0-mod_amount, duration);
endfunction

////////////////
// Feeblemind //
////////////////

function cast_FEEBLEMIND( caster, cast_on )
    if (!CheckLineOfSight( caster, cast_on ))
        return;
    endif
    if (! ConsumeMana( me, 3 ))
        //return 0;
    endif
    if (! ConsumeReagents(me, 3))
        //return 0;
    endif
    if (saywords)
    SpeakPowerWords( me, 3);
    endif
    PlaySoundEffect(cast_on, SOUND_EFFECT_FEEBLEMIND);
    PlayObjectCenteredEffect( cast_on, EFFECT_RED_SPARKLE, 7, 7 );
    var magery := GetEffectiveSkill(caster, SKILLID_MAGERY);
    var mod_amount := GetModAmount( magery );
    var duration := GetModDuration( magery, mod_amount );
    SetIntelligenceMod(cast_on, GetIntelligenceMod(cast_on) - mod_amount);
    do_temp_mod(cast_on, "int", 0-mod_amount, duration);
endfunction

///////////
// Bless //
///////////
function cast_bless( caster, cast_on )
    if (!CheckLineOfSight( caster, cast_on ))
        return;
    endif
    if (! ConsumeMana( me, 17 ))
        //return 0;
    endif
    if (! ConsumeReagents(me, 17))
        //return 0;
    endif
    if (saywords)
    SpeakPowerWords( me, 17);
    endif
    PlaySoundEffect(caster, sound_effect_bless);
    PlaySoundEffect(cast_on, sound_effect_bless);
    PlayObjectCenteredEffect( caster, EFFECT_SPARKLE, 10,10);
    var magery := GetEffectiveSkill(caster, SKILLID_MAGERY);
    var die_string := CStr( magery/15 ) + "d3+2";
    var str_mod := RandomDiceRoll( die_string );
    var dex_mod := RandomDiceRoll( die_string );
    var int_mod := RandomDiceRoll( die_string );
    var duration := 20 + magery / 2;
    Detach();
    SetStrengthMod(cast_on, GetStrengthMod(cast_on) + str_mod);
    SetDexterityMod(cast_on, GetDexterityMod(cast_on) + dex_mod);
    SetIntelligenceMod(cast_on, GetIntelligenceMod(cast_on) + int_mod);
    sleep( duration );
    SetStrengthMod(cast_on, GetStrengthMod(cast_on) - str_mod);
    SetDexterityMod(cast_on, GetDexterityMod(cast_on) - dex_mod);
    SetIntelligenceMod(cast_on, GetIntelligenceMod(cast_on) - int_mod);
endfunction

///////////
// Curse //
///////////
function cast_curse( caster, cast_on )
    if (!CheckLineOfSight( caster, cast_on ))
        return;
    endif
    if (! ConsumeMana( me, 27 ))
        //return 0;
    endif
    if (! ConsumeReagents(me, 27))
        //return 0;
    endif
    if (saywords)
        SpeakPowerWords( me, 27);
    endif
    PlaySoundEffect(cast_on, sound_effect_curse);
    PlayObjectCenteredEffect( caster, EFFECT_RED_SPARKLE, 10,10);
    var magery := GetEffectiveSkill(caster, SKILLID_MAGERY);
    var die_string := CStr( magery /15 ) + "d3+2";
    var str_mod := RandomDiceRoll( die_string );
    var dex_mod := RandomDiceRoll( die_string );
    var int_mod := RandomDiceRoll( die_string );
    var duration := 20 + magery / 2;
    SetStrengthMod(cast_on, GetStrengthMod(cast_on) - str_mod);
    do_temp_mod(cast_on, "str", 0-mod_amount, duration);
    SetDexterityMod(cast_on, GetDexterityMod(cast_on) - dex_mod);
    do_temp_mod(cast_on, "dex", 0-mod_amount, duration);
    SetIntelligenceMod(cast_on, GetIntelligenceMod(cast_on) - int_mod);
    do_temp_mod(cast_on, "int", 0-mod_amount, duration);
endfunction

//////////////
// Teleport //
//////////////
function cast_teleport( caster, where )
  if(CheckLosAt( caster, where.x, where.y, where.z ))
    MoveCharacterToLocation( caster, where.x, where.y, where.z );
  endif
endfunction

///////////////
// Resurrect //
///////////////
function cast_resurrect(caster, cast_on)
  if(!CheckLineOfSight(caster, cast_on))
    return;
  endif
  if(ResNow(cast_on)==1)
    PlaySoundEffect(caster, SOUND_EFFECT_CURE);
    PlaySoundEffect(cast_on, SOUND_EFFECT_CURE);
    Resurrect(cast_on);
    ResPenalties(cast_on);
  endif
endfunction


/////////////
// Reflect //
/////////////
function cast_reflect( caster )
    if (! ConsumeMana( me, 36))
        //return 0;
    endif
    if (! ConsumeReagents(me, 36))
        //return 0;
    endif
    if (saywords)
    SpeakPowerWords( me, 36);
    endif
        PlayObjectCenteredEffect( caster, EFFECT_SPARKLE, 10,10);
        SetObjProperty(caster, "mr","1");
endfunction


////////////
// Poison //
////////////
function poison(creature)
  if(creature.dead)
    return;
  endif
  var poison_level;
  poison_level := CInt(GetObjProperty(creature, "poison_level"));
  if(poison_level == 0)
    start_script(":spells:poisonDamage", creature);
  endif
  poison_level := poison_level + 2;
  print( "setting poison level to " + poison_level );
  SetObjProperty(creature,"poison_level", CStr(poison_level));
  PlayObjectCenteredEffect( creature, EFFECT_SPARKLE, 10,10);
  PlaySoundEffect(creature, SOUND_EFFECT_POISON);
endfunction


/////////////
// Poison2 //
/////////////
function cast_poison( caster, cast_on )
  var parms := array;
  var parms2 := array;
  if(!CheckLineOfSight(caster, cast_on))
    //return 0;
  endif
  if(!ConsumeMana(me, 20))
    //return 0;
  endif
  if(! ConsumeReagents(me, 20))
    //return 0;
  endif
  if(saywords)
    SpeakPowerWords(me, 20);
  endif
  parms[1]  := caster;
  parms[2]  := cast_on;
  parms[3]  := 0;
  parms[4]  := EFFECT_RED_SPARKLE;
  parms[5]  := 10;
  parms[6]  := 10;
  parms[7]  := 0;
  parms[8]  := 0;
  parms[9]  := 0;
  parms[10] := ":spells:effect_poisoning";
  parms2[1] := cast_on;
  parms2[2] := caster;
  parms2[3] := CInt(GetEffectiveSkill( caster, SKILLID_MAGERY ) /15);
  parms[11] := parms2;
  start_script( ":spells:reflect_spell",parms);
  print("starting reflect_spell!");
endfunction

///////////////
// MindBlast //
///////////////
function cast_mind_blast(caster, victim)
  var parms := array;
  if(!CheckLineOfSight(caster, victim))
    //return 0;
  endif
  if(!ConsumeMana(me, 37))
     //return 0;
  endif
  if(!ConsumeReagents(me, 37))
     //return 0;
  endif
  if(saywords)
    SpeakPowerWords(me, 37);
  endif
  var my_int := CInt(GetIntelligence(caster));
  var his_int := CInt(GetIntelligence(victim));
  if(my_int > his_int)
	var diff := (my_int - his_int);
	parms[1] := caster;
    parms[2] := victim;
    parms[3] := "";
    parms[4] := EFFECT_RED_SPARKLE;
    parms[5] := 10;
    parms[6] := 1;
    parms[7] := 0;
    parms[8] := diff;
    parms[9] := 0;
    parms[10] := "";
    parms[11] := "";
    start_script( ":spells:reflect_spell",parms);
	PlaySoundEffect(victim, SOUND_EFFECT_MINDBLAST);
    elseif(his_int > my_int)
	var diff := (his_int - my_int);
    parms[1] := victim;
    parms[2] := caster;
    parms[3] := "";
    parms[4] := EFFECT_RED_SPARKLE;
    parms[5] := 10;
    parms[6] := 1;
    parms[7] := 0;
    parms[8] := diff;
    parms[9] := 0;
    parms[10] := "";
    parms[11] := "";
    start_script(":spells:reflect_spell",parms);
	PlaySoundEffect(caster, SOUND_EFFECT_MINDBLAST);
  endif
endfunction

/////////////
// Meteors //
/////////////
function cast_meteor( caster, cast_loc )
  var parms := array;
  var targets := ListMobilesNearLocation(cast_loc.x,cast_loc.y,cast_loc.z,4);
  if(saywords)
    SpeakPowerWords( me, 55);
  endif
  foreach mobile in targets
	if((mobile.serial != caster.serial)  && (CheckLineOfSight(caster, mobile)))
	  var dmg := GetEffectiveSkill( caster, SKILLID_MAGERY ) / (RandomInt(3) + 5);
	  dmg := dmg /2;
	  parms[1] := caster;
      parms[2] := mobile;
      parms[3] := MOVING_EFFECT_FIREBALL;
      parms[4] := "";
      parms[5] := 10;  //speed
      parms[6] := 1; // loop
      parms[7] := 1; // explode
      parms[8] := dmg;
      parms[9] := 0;
      parms[10] := "";
      parms[11] := "";
	  start_script( ":spells:reflect_spell",parms);
	  PlaySoundEffect( caster, SOUND_EFFECT_FIREBALL );
	endif
  endforeach
endfunction

/////////////////
// Magic Arrow //
/////////////////
function cast_magicarrow(caster, cast_on)
  var parms := array;
  if(!CheckLineOfSight(caster, cast_on))
    return;
  endif
  if(!ConsumeMana(me, 5))
    //return 0;
  endif
  if(!ConsumeReagents(me, 5))
    //return 0;
  endif
  if(saywords)
    SpeakPowerWords( me, 5);
  endif
  PlaySoundEffect( caster, SOUND_EFFECT_MAGIC_ARROW );
  var die_string := "d3+2";
  var dmg := RandomDiceRoll( die_string );
  dmg := dmg + CInt( GetEffectiveSkill(caster, SKILLID_MAGERY)/20);
  print( "damage will be: " + dmg );
  parms[1] := caster;
  parms[2] := cast_on;
  parms[3] := MOVING_EFFECT_MAGIC_ARROW;
  parms[4] := "";
  parms[5] := 5;  //speed
  parms[6] := 1; // loop
  parms[7] := 1; // explode
  parms[8] := dmg;
  parms[9] := 0;
  parms[10] := "";
  parms[11] := "";
  start_script( ":spells:reflect_spell",parms);
endfunction

///////////////
// Lightning //
///////////////
function cast_lightning( caster, cast_on)
    if (!CheckLineOfSight( caster, cast_on ))
       	return;
    endif
        if (! ConsumeMana( me, 30 ))
    //    return 0;
    endif
    if (! ConsumeReagents(me, 30))
    //    return 0;
    endif
    if (saywords)
    SpeakPowerWords( me, 30);
    endif
    PlayLightningBoltEffect( cast_on );
    PlaySoundEffect( caster, SOUND_EFFECT_LIGHTNING );

    var die_string := "3d6";
    var dmg := RandomDiceRoll( die_string );
    dmg := dmg + (GetEffectiveSkill( caster, SKILLID_MAGERY ) / 15);


    	var parms := array;
    	parms[1] := cast_on;
    	parms[2] := caster;
    	parms[3] := dmg;
    	start_script(":spells:lightningdamage", parms);

endfunction

//////////
// Harm //
//////////
function cast_harm( caster, cast_on)

	var parms := array;

	if (!CheckLineOfSight( caster, cast_on ))

        	return;
    	endif
    if (! ConsumeMana( me, 12 ))
        //return 0;
    endif
    if (! ConsumeReagents(me, 12))
        //return 0;
    endif
    if (saywords)
    SpeakPowerWords( me, 12);
    endif
	PlaySoundEffect( caster, SOUND_EFFECT_HARM );

        var die_string := "d5+2";
	var dmg := RandomDiceRoll( die_string );
    	dmg := dmg + (GetEffectiveSkill( caster, SKILLID_MAGERY ) / 15);

	parms[1] := caster;
    	parms[2] := cast_on;
    	parms[3] := "";
    	parms[4] := MOVING_EFFECT_HARM;
    	parms[5] := 0x0a;  //speed
    	parms[6] := 0x1e; // loop
    	parms[7] := 0; // explode
    	parms[8] := dmg;
    	parms[9] := 0;
    	parms[10] := "";
    	parms[11] := "";

	start_script( ":spells:reflect_spell",parms);
endfunction

/////////////////
// Flamestrike //
/////////////////
function cast_flamestrike( caster, cast_on )

    var parms := array;

    PlaySoundEffect( cast_on, SOUND_EFFECT_FLAMESTRIKE );
    if (! ConsumeMana( me, 51 ))
        //return 0;
    endif
    if (! ConsumeReagents(me, 51))
        //return 0;
    endif
    if (saywords)
    SpeakPowerWords( me, 51);
    endif
    var die_string := "3d6+2";
    var dmg := RandomDiceRoll( die_string );
    dmg := dmg + (GetEffectiveSkill( caster, SKILLID_MAGERY ) / 10);
        dmg := dmg /2;
    parms[1] := caster;
    parms[2] := cast_on;
    parms[3] := "";
    parms[4] := MOVING_EFFECT_FLAMESTRIKE;
    parms[5] := 0x0a;  //speed
    parms[6] := 0x1e; // loop
    parms[7] := 1; // explode
    parms[8] := dmg;
    parms[9] := 0;
    parms[10] := "";
    parms[11] := "";

    start_script( ":spells:reflect_spell",parms);

endfunction

///////////
// Ebolt //
///////////
function cast_ebolt( caster, cast_on)

    var parms := array;

    if (!CheckLineOfSight( caster, cast_on ))
        //return 0;
    endif
    if (! ConsumeMana( me, 42 ))
        //return 0;
    endif
    if (! ConsumeReagents(me, 42))
        //return 0;
    endif
    if (saywords)
    SpeakPowerWords( me, 42);
    endif
    var die_string := "3d5+1";
    var dmg := RandomDiceRoll( die_string );
    dmg := dmg + (GetEffectiveSkill( caster, SKILLID_MAGERY ) / 10);
    dmg :=  dmg   /3;
    parms[1] := caster;
    parms[2] := cast_on;
    parms[3] :=  MOVING_EFFECT_EBOLT;
    parms[4] :="";
    parms[5] := 0x0a;  //speed
    parms[6] := 0x1e; // loop
    parms[7] := 1; // explode
    parms[8] := dmg;
    parms[9] := 0;
    parms[10] := "";
    parms[11] := "";

    start_script( ":spells:reflect_spell",parms);

    PlaySoundEffect( cast_on, SOUND_EFFECT_EBOLT );

endfunction

//////////////////
// Dispel Field //
//////////////////
function cast_dispel_field( caster, cast_area )

var items;
var therange;

therange := CInt(GetEffectiveSkill( caster, SKILLID_MAGERY )/10 );

    items := ListItemsNearLocation(cast_area.x, cast_area.y, cast_area.z, therange);
    PlaySoundEffect( caster, SOUND_EFFECT_DISPEL );

    foreach item in items
        if (GetObjProperty(item, "f") == "1")
                PlayObjectCenteredEffect( item, EFFECT_SPARKLE, 10,10);
                DestroyItem(item);
        endif
   endforeach
endfunction

//////////
// Cure //
//////////
function cast_cure( caster, cast_on)

	if (!CheckLineOfSight( caster, cast_on ))

        	return;
    	endif

        PlaySoundEffect(caster, SOUND_EFFECT_CURE);
        PlayObjectCenteredEffect( cast_on, EFFECT_SPARKLE, 10,10);

	var poison_descr := GetObjProperty( cast_on,"poison_level" );
        var poison_level := CInt(poison_descr);
        if (poison_level >= 1)
                poison_level := poison_level - 1;
                if (poison_level >= 1)

                else
                 SendSysMessage(caster, "Target cured.");
                endif
                SetObjProperty(cast_on, "poison_level", CStr(poison_level));
        endif

endfunction

/////////////////////
// Chain Lightning //
/////////////////////
function cast_chainlightning( caster, cast_loc)
    if (saywords)
        SpeakPowerWords( me, 49);
    endif
    var targets := ListMobilesNearLocation(cast_loc.x,cast_loc.y,cast_loc.z,4);

    foreach mobile in targets
	    if( (mobile.serial != caster.serial) && (mobile.dead == 0) && (CheckLineOfSight( caster, mobile )) )

		    PlayLightningBoltEffect( mobile );

		    PlaySoundEffect( caster, SOUND_EFFECT_LIGHTNING );

		    var damagestr := CStr(GetEffectiveSkill( caster, SKILLID_MAGERY ) / 9) + "d4";
		    var dmg := RandomDiceRoll(damagestr);
		    ApplyRawDamage( mobile, dmg );
	    endif
    endforeach
endfunction

//////////////
// ArchCure //
//////////////
function cast_archcure( caster, cast_on )

	if (!CheckLineOfSight( caster, cast_on ))
        	return  0;
    	endif

        PlaySoundEffect(caster, SOUND_EFFECT_CURE);
        PlayObjectCenteredEffect( cast_on, EFFECT_SPARKLE, 10,10);

	var poison_descr := GetObjProperty( cast_on,"poison_level" );
	if (poison_descr == "")
	   poison_descr := "0";
	endif
        var poison_level := CInt(poison_descr);

        if (poison_level >= 1)
                poison_level := poison_level - 4;
                if (poison_level > 1)
                    SendSysMessage(cast_on, "Some poison was cured, but not all");
                else
                    poison_level := 0;
                    SendSysMessage(cast_on, "You were cured.");
                endif
                SetObjProperty(cast_on, "poison_level", CStr(poison_level));
        endif

endfunction

//////////////////
// Greater Heal //
//////////////////
function cast_gheal( caster, cast_on )
    sleep(3);
    var die_string := "5d8+6";
    var healed := RandomDiceRoll( die_string );
    if (! ConsumeMana( me, 29 ))
        //return 0;
    endif
    if (! ConsumeReagents(me,29))
        //return 0;
    endif
    if (saywords)
    SpeakPowerWords( me, 29);
    endif
    HealDamage( cast_on, healed/2 );
    PlayObjectCenteredEffect( cast_on,EFFECT_SPARKLE,7, 0x10 );
    PlaySoundEffect( cast_on, SOUND_EFFECT_HEAL );

    //SendSysmessage( caster, "You healed " + healed + " damage." );

endfunction

function give_me_regs(me)

var reagents := {0x0f7a,0x0f7b,0x0f84,0x0f85,0x0f86,0x0f88,0x0f8c,0x0f8d};

foreach reg in reagents

        CreateItemInBackpack(me, reg, RandomInt(40));

endforeach
endfunction

function cast_summon_skel(me, onwho)

var utterance := RandomInt(4);

case (utterance)
        0: say("Awake, my children! Come forth and take your rightful place!");
        1: say("Come forth my minions!");
        2: say("I call upon the forces of darkness to attend me!");
        3: say("Mortal, you shall soon be my slave!");
endcase

CreateNpcFromTemplate("skeleton", me.x, me.y -2, me.z, 0);
CreateNpcFromTemplate("zombie", onwho.x, onwho.y - 1, onwho.z, 0);
CreateNpcFromTemplate("skeleton", me.x, me.y + 1, me.z, 0);



endfunction

function  cast_summon_fire(me, onwho)


var utterance := RandomInt(4);

case (utterance)
        0: say("Fire! I summon fire, death, and pain! Rise up, O Flame!");
        1: say("Your ashes shall grace our altar, human!");
        2: say("I call uppon the forces of fire!");
        3: say("Burn, human, burn!");
endcase

CreateNpcFromTemplate("fireelemental", me.x, me.y -2, me.z, 0);

endfunction

function  cast_summon_water(me, onwho)


var utterance := RandomInt(4);

case (utterance)
        0: say("I call upon the powers of water and ice!");
        1: say("Wash away my enemies, my pet");
        2: say("Flow and conquer, servants");
        3: say("Care for a drink, hummie?");
endcase

CreateNpcFromTemplate("waterelemental", me.x, me.y -2, me.z, 0);

endfunction

function cast_explosion( caster, cast_on )

    if (saywords)
        SpeakPowerWords( me, 43);
    endif
    var die_string := "4d6";
    var dmg := 0;

    var victims := ListMobilesNearLocationEx( cast_on.x, cast_on.y, cast_on.z, 3, 0x03);

    foreach mobile in victims
	if( (caster.serial != mobile.serial) && (CheckLosAt( mobile, cast_on.x, cast_on.y, cast_on.z ) && (!cast_on.npctemplate)))

	   dmg := RandomDiceRoll( die_string );

           dmg := dmg * (GetEffectiveSkill( caster, SKILLID_MAGERY ) / 50);
           if (CheckSkill(cast_on, SKILLID_MAGICRESISTANCE, -1, 80))
                dmg := dmg /2;
           endif

	   ApplyRawDamage( mobile, dmg );

	   PlaySoundEffect( mobile, SFX_SPELL_EXPLOSION );
	   PlayObjectCenteredEffect( mobile, FX_EXPLODE_3, 7, 0x10 );
        endif
    endforeach
endfunction

function cast_fireball( caster, cast_on )


    var parms := {};

    if (!cast_on)
        return;
    endif


    if (!CheckLineOfSight( caster, cast_on ))

        return;
    endif
    if (saywords)
        SpeakPowerWords( me, 18);
    endif

    PlaySoundEffect( caster, SFX_SPELL_FIREBALL ); // was 15F

    var die_string := CStr( CInt( GetEffectiveSkill(caster, SKILLID_MAGERY)/20) ) + "d3+2";

    var dmg := RandomDiceRoll( die_string );


    parms[1] := caster;
    parms[2] := cast_on;
    parms[3] := FX_FIREBALL;
    parms[4] := "";
    parms[5] := 8;  //speed
    parms[6] := 1; // loop
    parms[7] := 1; // explode
    parms[8] := dmg;
    parms[9] := 0;
    parms[10] := "";
    parms[11] := "";


    start_script( ":spells:reflect_spell",parms);

endfunction

function  cast_paralyze( caster, cast_on)

    if (!CheckLineOfSight( caster, cast_on ))
        return;
    endif
    if (saywords)
        SpeakPowerWords( me, 38);
    endif
    if (!CheckSkill(cast_on, SKILLID_MAGICRESISTANCE, -1, 50))
        PlayStationaryEffect( cast_on.x, cast_on.y, cast_on.z, FX_CURSE_EFFECT, 10, 0xa, 0 );
        PlaySoundEffect( cast_on, SFX_SPELL_PARALYZE );
        cast_on.paralyzed := 1;
        do_temp_mod(cast_on, "p", 1, 15);

    endif
endfunction

function cast_masscurse( caster, cast_on )

  if (saywords)
        SpeakPowerWords(me, 46);
  endif

    PlaySoundEffect(caster, SFX_SPELL_MASS_CURSE);

    var targets := ListMobilesNearLocation( cast_on.x, cast_on.y, cast_on.z, 4 );

    var magery := GetEffectiveSkill(caster, SKILLID_MAGERY);
    var die_string := CStr( magery /15 ) + "d3+2";

    var str_mod := RandomDiceRoll( die_string );
    var dex_mod := RandomDiceRoll( die_string );
    var int_mod := RandomDiceRoll( die_string );
    var duration := 20 + magery / 2;



    foreach cast_on in targets

        if ( (cast_on.serial != caster.serial) && (CheckLineOfSight( caster, cast_on )) )
            PlaySoundEffect(cast_on, SFX_SPELL_MASS_CURSE);
            PlayObjectCenteredEffect( cast_on, FX_CURSE_EFFECT, 10,10);

            SetStrengthMod(cast_on, GetStrengthMod(cast_on) - str_mod);
            do_temp_mod(cast_on, "str", 0-str_mod, duration);
            SetDexterityMod(cast_on, GetDexterityMod(cast_on) - dex_mod);
            do_temp_mod(cast_on, "dex", 0-dex_mod, duration);
            SetIntelligenceMod(cast_on, GetIntelligenceMod(cast_on) - int_mod);
            do_temp_mod(cast_on, "int", 0-int_mod, duration);
        else
            cast_on := 0;
        endif
    endforeach

endfunction

function  cast_summon_earth(me, onwho)


var utterance := RandomInt(4);

case (utterance)
        0: say("Awake, Son of Earth!");
        1: say("Mud and stone, arise and walk!");
        2: say("I call upon the stones and earth to drive forth my foes!");
        3: say("Return to earth, human!");
endcase

CreateNpcFromTemplate("earthelemental", me.x, me.y -2, me.z, 0);

endfunction

function  cast_summon_air(me, onwho)


var utterance := RandomInt(4);

case (utterance)
        0: say("Wind blow and lightning flash! I call upon the forces of nature!");
        1: say("Do you like wind, human?");
        2: say("I call uppon the forces of wind!");
        3: say("By the winds of the east and west, I command you to serve me");
endcase

CreateNpcFromTemplate("airelemental", me.x, me.y -2, me.z, 0);

endfunction

function  cast_summon_daemon(me, onwho)


var utterance := RandomInt(4);

case (utterance)
        0: say("Come forth, servant, and destroy these mortal fools!");
        1: say("Servant, I command you: Destroy these mortals");
        2: say("Fire and hail, blood and brimstone, heed my command!");
        3: say("Take these slaves back to your realm, servant of darkness!");
endcase

CreateNpcFromTemplate("daemon", me.x, me.y -2, me.z, 0);

endfunction

function  cast_summon_vortex(me, onwho)


var utterance := RandomInt(4);

case (utterance)
        0: say("Lightning flash and fire burn!");
        1: say("Awake, oh servant of chaos!");
        2: say("Rise up and consume my enemies, slave!");
        3: say("Come forth, Chaos spawn!");
endcase

CreateNpcFromTemplate("vortex", me.x, me.y -2, me.z, 0);

endfunction


function cast_offensive_spell(onwho)


if ( Distance(me, onwho) > 10 ) then
        return;
endif
if (!CheckLosAt(me, onwho.x, onwho.y, onwho.z))
        return;
endif
var thespell := spells[RandomInt( len(spells) +1 )];

//say (" Gonna cast spell " + cstr(thespell) + " on you!");
case (thespell)
        "poison": cast_poison(me, onwho);
        "flamestrike": cast_flamestrike( me, onwho );
        "ebolt": cast_ebolt(me, onwho);
        "lightening": cast_lightning(me, onwho);
        "harm": cast_harm(me, onwho);
        "mindblast": cast_magicarrow(me, onwho);
        "magicarrow": cast_magicarrow(me, onwho);
        "meteorswarm": cast_meteor(me, onwho);
        "summonskel" : cast_summon_skel(me, onwho);
        "summonfire" : cast_summon_fire(me, onwho);
        "summonwater": cast_summon_water(me, onwho);
        "summonair"  : cast_summon_air(me, onwho);
        "summonearth" : cast_summon_earth(me, onwho);
        "summondaemon" : cast_summon_daemon(me,onwho);
        "summonvortex": cast_summon_vortex(me, onwho);
        "paralyze" : cast_paralyze(me, onwho);
        "chainlightning": cast_chainlightning(me, onwho);
        "masscurse" : cast_masscurse(me, onwho);
        "weaken" : cast_WEAKEN(me, onwho);
        "fireball" : cast_fireball(me, onwho);
        "explosion" : cast_explosion(me, onwho);

endcase



endfunction