use os;
use npc;
use util;
use cfgfile;

include "include/client";
include "include/statMod";

function castspell(caster, cast_on, spellname)
  var spelltype := 0;
  var spellid := GetSpellID(spellname);
  if(!spellid)
    return;
  endif
  spelltype := spellT(spellid);
if((cast_on.serial == caster.serial) && spelltype != 1)
  var kname, kserial;
  var lastHit := array;
  lastHit := GetObjProperty(caster, "LastHit");
  if(!lastHit)
      cast_on := 0;
      return;
  endif
  kname := lastHit[1];
  kserial := lastHit[2];
  if(kname)
    cast_on := SystemFindObjectBySerial(kserial);
  endif
endif
  if(!CheckLineOfSight(caster, cast_on))
    return;
  endif
  var parms array;
  parms[1] := "#MOB";
  parms[2] := caster;
  parms[3] := cast_on;
  Detach();
  start_script(":spells:"+spellname, parms);
endfunction

function GetSpellID(spellname)
  var spellid := 0;
  case(spellname)
    "clumsy"        : spellid := 1;
    "feeblemind"    : spellid := 3;
    "heal"          : spellid := 4;
    "magicarrow"    : spellid := 5;
    "nightsight"    : spellid := 6;
    "reactivearmor" : spellid := 7;
    "weaken"        : spellid := 8;
    "agility"       : spellid := 9;
    "cunning"       : spellid := 10;
    "cure"          : spellid := 11;
    "harm"          : spellid := 12;
    "protection"    : spellid := 15;
    "strength"      : spellid := 16;
    "bless"         : spellid := 17;
    "fireball"      : spellid := 18;
    "poison"        : spellid := 20;
    "teleport"      : spellid := 22;
    "wallofstone"   : spellid := 24;
    "archcure"      : spellid := 25;
    "archprot"      : spellid := 26;
    "curse"         : spellid := 27;
    "firefield"     : spellid := 28;
    "gheal"         : spellid := 29;
    "lightning"     : spellid := 30;
    "manadrain"     : spellid := 31;
    "bladespirit"   : spellid := 33;
    "dispelfield"   : spellid := 34;
    "reflect"       : spellid := 36;
    "mindblast"     : spellid := 37;
    "paralyze"      : spellid := 38;
    "poisonfield"   : spellid := 39;
    "summoncreature": spellid := 40;
    "dispel"        : spellid := 41;
    "ebolt"         : spellid := 42;
    "explosion"     : spellid := 43;
    "invisibility"  : spellid := 44;
    "masscurse"     : spellid := 46;
    "parafield"     : spellid := 47;
    "reveal"        : spellid := 48;
    "chainlightning": spellid := 49;
    "energyfield"   : spellid := 50;
    "fstrike"       : spellid := 51;
    "manavamp"      : spellid := 53;
    "massdispel"    : spellid := 54;
    "meteorswarm"   : spellid := 55;
    "polymorph"     : spellid := 56;
    "earthquake"    : spellid := 57;
    "vortex"        : spellid := 58;
    "resurrect"     : spellid := 59;
    "summonair"     : spellid := 60;
    "summondaemon"  : spellid := 61;
    "summonearth"   : spellid := 62;
    "summonfire"    : spellid := 63;
    "summonwater"   : spellid := 64;
  endcase
  return spellid;
endfunction

function cast_offensive_spell(caster, cast_on)
  var thespell;
  var kname, kserial;
  var lastHit := array;
  lastHit := GetObjProperty(me, "LastHit");
  if(!lastHit)
      cast_on := 0;
      return;
  endif
  kname := lastHit[1];
  kserial := lastHit[2];
  if(kname)
    cast_on := SystemFindObjectBySerial(kserial);
  endif
  if((GetObjProperty(cast_on, "summoned")) && (RandomInt(5) >= 2))
    thespell := "dispel";
    var nexttarget := SystemFindObjectBySerial(GetObjProperty(cast_on, "master"));
    castspell(caster, cast_on, thespell);
    if(!cast_on)
      fight(nexttarget);
    endif
  else
    thespell := spells[RandomInt(len(spells) +1)];
    castspell(caster, cast_on, thespell);
  endif
endfunction

function spellT(spellid)
  var spelltype := 0;
  case(spellid)
    1   : spelltype := 0;
    3   : spelltype := 0;
    4   : spelltype := 1;
    5   : spelltype := 0;
    6   : spelltype := 1;
    7   : spelltype := 1;
    8   : spelltype := 0;
    9   : spelltype := 1;
    10  : spelltype := 1;
    11  : spelltype := 1;
    12  : spelltype := 0;
    15  : spelltype := 1;
    16  : spelltype := 1;
    17  : spelltype := 1;
    18  : spelltype := 0;
    20  : spelltype := 0;
    22  : spelltype := 0;
    24  : spelltype := 0;
    25  : spelltype := 1;
    26  : spelltype := 1;
    27  : spelltype := 0;
    28  : spelltype := 0;
    29  : spelltype := 1;
    30  : spelltype := 0;
    31  : spelltype := 0;
    33  : spelltype := 0;
    34  : spelltype := 0;
    36  : spelltype := 1;
    37  : spelltype := 0;
    38  : spelltype := 0;
    39  : spelltype := 0;
    40  : spelltype := 0;
    41  : spelltype := 0;
    42  : spelltype := 0;
    43  : spelltype := 0;
    44  : spelltype := 0;
    46  : spelltype := 0;
    47  : spelltype := 0;
    48  : spelltype := 0;
    49  : spelltype := 0;
    50  : spelltype := 0;
    51  : spelltype := 0;
    53  : spelltype := 0;
    54  : spelltype := 0;
    55  : spelltype := 0;
    56  : spelltype := 0;
    57  : spelltype := 0;
    58  : spelltype := 0;
    59  : spelltype := 0;
    60  : spelltype := 0;
    61  : spelltype := 0;
    62  : spelltype := 0;
    63  : spelltype := 0;
    64  : spelltype := 0;
  endcase
  return spelltype;
endfunction
