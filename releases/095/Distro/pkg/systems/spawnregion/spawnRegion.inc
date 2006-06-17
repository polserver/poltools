use os;
use uo;
use cfgfile;
use basic;
use util;
use datafile;

include "include/createNPC";
include "include/datafile";
include "include/attributes";
include "include/clock";

const MAX_SPAWN_TRIES       := 20;  // Max number of attempts to spawn an npc.
const DEFAULT_GRP_FREQUENCY := 5;   // In minutes.

var npccfg          := ReadConfigFile(":*:npcdesc");
var regcfg          := dictionary;  // Regions config.
var grpcfg          := dictionary;  // Groups config.
var regidx          := 0;           // Region index for SpawnAllRegions.

var regionscfg, groupscfg, counter;

UnloadConfigFile(":spawnregion:spawnregions");
UnloadConfigFile(":spawnregion:spawngroups");

function ReadRegions()
  foreach regname in(regionscfg.Keys())
    var regelem := regionscfg.FindElement(regname);
    if(!regelem)
      continue;
    endif
    regcfg[regname] := regelem;
  endforeach
endfunction

function ReadGroups()
  foreach grpname in(groupscfg.Keys())
    var grpelem := groupscfg.FindElement(grpname);
    if(!grpelem)
      continue;
    endif
    grpcfg[grpname] := grpelem;
  endforeach
endfunction

function SpawnRegion(region)
  var ct1 := counter;
  if(GetGlobalProperty("SpawnRegionOff"))
    return 0;
  endif
  foreach thing in(regcfg[region].propnames())
    if(thing != "rect")
      var grpdata := SplitWords(regcfg[region].GetProp(thing));
      SpawnGroup(region, grpdata[1], CInt(grpdata[2]), CInt(grpdata[3]), CInt(grpdata[4]), CInt(grpdata[5]));
      sleepms(1);
    endif
  endforeach
  sleepms(1);
endfunction

function SpawnRegionRM(region)
  var ct1 := counter;
  if(GetGlobalProperty("SpawnRegionOff"))
    return 0;
  endif
  foreach thing in(regcfg[region].propnames())
    if(thing != "rect")
      var grpdata := SplitWords(regcfg[region].GetProp(thing));
      SpawnGroupRM(region, grpdata[1], CInt(grpdata[2]), CInt(grpdata[3]), CInt(grpdata[4]), CInt(grpdata[5]));
    endif
  endforeach
endfunction

function SpawnGroupRM(region, group, max, freq, hrstart, hrend)
  var nextspawn := GetGroupFreq(region, group);
  if(nextspawn == -1)
    return 0;
  endif
  var spawns := GetGroupSpawns(region, group);
  if(spawns.size() >= CInt(max))
    return 0;
  endif
  if(!freq)
    freq := DEFAULT_GRP_FREQUENCY;
  endif
  var clock := ReadGameClock();
  nextspawn := clock +(freq * 60);
  SetGroupFreq(region, group, nextspawn);
  var npcs := grpcfg[group].GetProp("npc");
  if(!npcs)
    syslog("Unable to find SpawnGroup: " + group);
    return 0;
  endif
  var tospawn := max - spawns.size();
  var sp, template, npc;
  for(sp := 1; sp <= tospawn; sp := sp + 1)
    template := PickRandomNpc(npcs);
    npc := SpawnNpc(region, group, template);
    if(!npc)
      continue;
    endif
    spawns.append(npc.serial);
  endfor
  SetGroupSpawns(region, group, spawns);
endfunction

function SpawnGroup(region, group, max, freq, hrstart, hrend)
  var nextspawn := GetGroupFreq(region, group);
  if(nextspawn == -1)
    return 0;
  elseif(hrstart and hrend)
	var hrnow := GetHourOfDay();
    if(!HourWithinRange(hrnow, hrstart, hrend))
      DespawnGroup(region, group);
      return 0;
    endif
  endif
  var spawns := GetGroupSpawns(region, group);
  if(spawns.size() >= CInt(max))
    return 0;
  endif
  if(!freq)
    freq := DEFAULT_GRP_FREQUENCY;
  endif
  var clock := ReadGameClock();
  if(nextspawn > clock)
      return 0;
  endif
  nextspawn := clock +(freq * 60);
  SetGroupFreq(region, group, nextspawn);
  if(spawns.size() >= max)
      return 0;
  endif
  var npcs := grpcfg[group].GetProp("npc");
  if(!npcs)
    syslog("Unable to find SpawnGroup: " + group);
    return 0;
  endif
  var tospawn := max - spawns.size();
  var sp, template, npc;
  for(sp := 1; sp <= tospawn; sp := sp + 1)
    template := PickRandomNpc(npcs);
    npc := SpawnNpc(region, group, template);
    if(!npc)
      continue;
    endif
    spawns.append(npc.serial);
    sleepms(1);
  endfor
  SetGroupSpawns(region, group, spawns);
endfunction

function PickRandomNpc(byref npcs)
  var idx := RandomInt(npcs.size()) + 1;
  return npcs[idx];
endfunction

function SpawnNpc(region, group, template)
  var coords := SplitWords(regcfg[region].GetProp("rect"));
  var x1 := CInt(coords[1]);
  var y1 := CInt(coords[2]);
  var x2 := CInt(coords[3]);
  var y2 := CInt(coords[4]);
  var x, y, z;
  var tries := 0;
  var sh, info, npc, prop;
  repeat
    x := RandomInt(x2-x1)+x1;
    y := RandomInt(y2-y1)+y1;
    z := GetMapInfo(x, y).z;
    tries := tries + 1;
    sh := GetStandingHeight(x, y, z);
    if(sh.multi || sh.multi.serial)
      npc := 0;
    else
      info := Hex(GetMapInfo(x, y).landtile);
      if((info < 0x406) ||(info > 0x455))
        npc := CreateNpc(template, x, y, z);
        if(npc)
          counter := counter + 1;
          prop := array;
          prop.append(region);
          prop.append(group);
          SetObjProperty(npc, "SpawnRegion", prop);
          return npc;
        endif
      endif
    endif
    sleepms(1);
  until(tries > MAX_SPAWN_TRIES);
  if(tries > MAX_SPAWN_TRIES)
    syslog("Exceeded max tries. Region = " + region + ", npc = " + template);
  endif
  return 0;
endfunction

function SpawnRegionDeath(byref corpse)
  var regdata := GetObjProperty(corpse, "SpawnRegion");
  if(!regdata)
    return;
  endif
  RemoveSerialFromGroup(regdata[1], regdata[2], CInt(GetObjProperty(corpse, "AiSerial")));
endfunction

function RemoveSerialFromGroup(region, group, serial)
  set_critical(1);
  var spawns := GetGroupSpawns(region, group);
  var holder := array;
  foreach thing in spawns
    if(thing != serial)
      holder.append(thing);
    endif
  endforeach
  SetGroupSpawns(region, group, holder);
  set_critical(0);
endfunction

function DespawnRegion(region)
  var rgroups := DFFindElement(SpawnRegionData, region, 1);
  if(Len(rgroups))
    foreach grp in(rgroups.PropNames())
      DespawnGroup(region, grp);
    endforeach
  endif
endfunction

function DespawnRegionRM(region)
  var rgroups := DFFindElement(SpawnRegionData, region, 1);
  var pnames := rgroups.PropNames();
  foreach grp in pnames
    DespawnGroupRM(region, grp);
  endforeach
endfunction

function DespawnGroup(region, group)
  var spawns := GetGroupSpawns(region, group);
  foreach spawn in spawns
    Despawn(spawn);
  endforeach
endfunction

function DespawnGroupRM(region, group)
  var spawns := GetGroupSpawns(region, group);
  foreach spawn in spawns
    Despawn(spawn);
  endforeach
  SetGroupFreq(region, group, 0);
endfunction

function Despawn(serial)
  var npc := SystemFindObjectBySerial(serial);
  if(!npc)
    return 0;
  else
    SetObjProperty(npc, "guardkill", 1);
    SetObjProperty(npc, "NoSound", 1);
    ApplyRawDamage(npc, GetMaxHp(npc) * 2);
    sleepms(1);
    counter := counter +1;
  endif
endfunction

function GetGroupSpawns(region, group)
  var rgroups := DFFindElement(SpawnRegionData, region, 1);
  var sp := rgroups.GetProp(group);
  if (sp)
    return sp;
  else
    rgroups.SetProp(group, array);
    return array;
  endif
endfunction

function GetGroupSpawnsRM(region, group)
  var rgroups := DFFindElement(SpawnRegionData, region, 1);
  var spawns := rgroups.GetProp(group);
  if (spawns)
    return spawns;
  else
    var freq := GetGroupFreq(region, group);
    rgroups.SetProp(group, array);
    if(freq == -1)
      return -1;
    else
      return array;
    endif
  endif
endfunction


function SetGroupSpawns(region, group, byref spawns)
  set_critical(1);
  var rgroups := DFFindElement(SpawnRegionData, region, 1);
  rgroups.SetProp(group, spawns);
  set_critical(0);
endfunction

function SpawnAllRegions()
  if(GetGlobalProperty("SpawnRegionOff"))
    return 0;
  endif
  var regions := regionscfg.Keys();
  foreach reg in regions
    SetGlobalProperty("#" + reg, 1);
    SpawnRegion(reg);
    EraseGlobalProperty("#" + reg);
    sleepms(1);
  endforeach
endfunction

function EnableRegion(region)
  var srfreq := DFFindElement(SpawnRegionsFreq, region, 1);
  foreach grp in (srfreq.PropNames())
    EnableGroup(region, grp);
  endforeach
endfunction

function DisableRegion(region)
  var srfreq := DFFindElement(SpawnRegionsFreq, region, 1);
  foreach grp in (srfreq.PropNames())
    DisableGroup(region, grp);
  endforeach
endfunction

function EnableGroup(region, group)
  var freq := GetGroupFreq(region, group);
  if(freq != -1)
    return 0;
  endif
  SetGroupFreq(region, group, "R");
endfunction

function DisableGroup(region, group)
  SetGroupFreq(region, group, -1);
endfunction

function GetGroupFreq(region, group)
  var srfreq := DFFindElement(SpawnRegionFreq, region, 1);
  var freq := DFGetProp(srfreq, group, 1, 0);
  return freq;
endfunction

function SetGroupFreq(region, group, freq)
  set_critical(1);
  var srfreq := DFFindElement(SpawnRegionFreq, region, 1);
  srfreq.SetProp(group, freq);
  set_critical(0);
endfunction

function ValidateSpawnRegions()
  print("////////////////////////////////////////");
  print("// Validating Spawn Regions");
  print("////////////////////////////////////////");
  var count := 0;
  var cfggroups, ingrpcfg, spawns, obj, num;
  var keys := SpawnRegionData.keys();
  foreach regname in keys
    if(!regcfg.exists(regname))
      DespawnRegion(regname);
      SpawnRegionData.DeleteElement(regname);
      SpawnRegionFreq.DeleteElement(regname);
      continue;
    endif
    var delem := DFFindElement(SpawnRegionData, regname);
    var felem := DFFindElement(SpawnRegionFreq, regname);
    foreach grpname in (delem.PropNames())
      ingrpcfg := 0;
      var elm := regcfg[regname];
      foreach cgrp in(elm.PropNames())
        cgrp := elm.GetProp(cgrp);
        cgrp := SplitWords(cgrp);
        if(cgrp[1] == grpname)
          ingrpcfg := 1;
          break;
        endif
      endforeach
      if(!ingrpcfg)
        DespawnGroup(regname, grpname);
        delem.EraseProp(grpname);
        felem.EraseProp(grpname);
        continue;
      endif
      var holder := array;
      foreach thing in DFGetPRop(delem, grpname)
        obj := SystemFindObjectBySerial(thing);
        if(obj)
          holder.append(thing);
        else
          count := count + 1;
        endif
      endforeach
      delem.SetProp(grpname, holder);
      holder := {};
      sleepms(1);
    endforeach
    sleepms(1);
  endforeach
  EraseGlobalProperty("SpawnRegions");
  EraseGlobalProperty("SpawnRegionsFreq");
  print("////////////////////////////////////////");
  print("//Done validating Spawn Region");
  print("//");
  print("//removed " + count + " invalid entries");
  print("////////////////////////////////////////");
endfunction