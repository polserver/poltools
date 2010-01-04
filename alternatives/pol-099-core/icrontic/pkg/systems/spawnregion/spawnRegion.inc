use os;
use uo;
use cfgfile;
use basic;
use util;
use datafile;

include "include/createNPC";
include "include/attributes";
include "include/clock";

const MAX_SPAWN_TRIES       := 20;  // Max number of attempts to spawn an npc.
const DEFAULT_GRP_FREQUENCY := 5;   // In minutes.

var npccfg      := ReadConfigFile(":*:npcdesc");
var regcfg      := dictionary;  // Regions config.
var grpcfg      := dictionary;  // Groups config.
var regidx      := 0;           // Region index for SpawnAllRegions.
var regionscfg, groupscfg;

UnloadConfigFile(":spawnregion:spawnregions");
UnloadConfigFile(":spawnregion:spawngroups");

function ReadRegions()
  foreach regname in (regionscfg.Keys())
    var regelem := regionscfg.FindElement(regname);
    if(!regelem)
      continue;
    endif
    regcfg[regname] := regelem;
  endforeach
endfunction

function ReadGroups()
  foreach grpname in (groupscfg.Keys())
    var grpelem := groupscfg.FindElement(grpname);
    if(!grpelem)
      continue;
    endif
    grpcfg[grpname] := grpelem;
  endforeach
endfunction

function SpawnRegion(region)
  if(GetGlobalProperty("SpawnRegionOff"))
    return 0;
  endif
  var propnames := regcfg[region].propnames();
  var reggrps := {};
  foreach thing in propnames
    if(thing != "rect")
      reggrps.append(regcfg[region].GetProp(thing));
    endif
  endforeach
  var grpdata;
  foreach grp in reggrps
    grpdata := SplitWords(grp);
    SpawnGroup(region, grpdata[1], CInt(grpdata[2]), CInt(grpdata[3]), CInt(grpdata[4]), CInt(grpdata[5]));
    sleepms(1);
  endforeach
endfunction

function SpawnGroupRM(region, group, max, freq, hrstart, hrend)
  var nextspawn := GetGroupFreq(region, group);
  if (nextspawn == -1)
      return 0;
  endif
  var spawns := GetGroupSpawns(region, group);
  if (spawns.size() >= CInt(max))
      return 0;
  endif
  if(!freq)
      freq := DEFAULT_GRP_FREQUENCY;
  endif
  var clock := ReadGameClock();
  if (nextspawn > clock)
      return 0;
  endif
  nextspawn := clock + (freq * 60);
  SetGroupFreq(region, group, nextspawn);
  if (spawns.size() >= max)
      return 0;
  endif
  var npcs := grpcfg[group].GetProp("npc");
  if (!npcs)
      syslog("Unable to find SpawnGroup: " + group);
      return 0;
  endif
  var tospawn := max - spawns.size();
  var sp;
  var template;
  var npc;
  for (sp := 1; sp <= tospawn; sp := sp + 1)
    template := PickRandomNpc(npcs);
    npc := SpawnNpc(region, group, template);
    if (!npc)
      continue;
    endif
    spawns.append(npc.serial);
    sleepms(100); // For good measure.
  endfor
  SetGroupSpawns(region, group, spawns);
endfunction

function SpawnGroup(region, group, max, freq, hrstart, hrend)
  var nextspawn := GetGroupFreq(region, group);
  if (nextspawn == -1)
      return 0;
  endif
  var spawns := GetGroupSpawns(region, group);
  if (spawns.size() >= CInt(max))
      return 0;
  endif
  if(!freq)
      freq := DEFAULT_GRP_FREQUENCY;
  endif
  var clock := ReadGameClock();
  if(nextspawn > clock)
      return 0;
  endif
  nextspawn := clock + (freq * 60);
  SetGroupFreq(region, group, nextspawn);
  if (spawns.size() >= max)
      return 0;
  endif
  var npcs := grpcfg[group].GetProp("npc");
  if (!npcs)
      syslog("Unable to find SpawnGroup: " + group);
      return 0;
  endif
  var tospawn := max - spawns.size();
  var sp;
  var template;
  var npc;
  for (sp := 1; sp <= tospawn; sp := sp + 1)
    template := PickRandomNpc(npcs);
    npc := SpawnNpc(region, group, template);
    if (!npc)
      continue;
    endif
    spawns.append(npc.serial);
    sleepms(100); // For good measure.
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
      if ((info < 0x406) || (info > 0x455))
        npc := CreateNpc(template, x, y, z);
        if (npc)
          prop := region + " " + group;
          SetObjProperty(npc, "SpawnRegion", prop);
          return npc;
        endif
      endif
    endif
    sleepms(20);
  until(tries > MAX_SPAWN_TRIES);
  if (tries > MAX_SPAWN_TRIES)
    syslog("Exceeded max tries. Region = " + region + ", npc = " + template);
  endif
  return 0;
endfunction

function SpawnRegionDeath(byref corpse)
  var regdata := SplitWords(GetObjProperty(corpse, "SpawnRegion"));
  RemoveSerialFromGroup(regdata[1], regdata[2], CInt(GetObjProperty(corpse, "AiSerial")));
endfunction

function RemoveSerialFromGroup(region, group, serial)
  set_critical(1);
  var spawns := GetGroupSpawns(region, group);
  var idx;
  for (idx := 1; idx <= spawns.size(); idx := idx + 1)
    if (spawns[idx] == serial)
      spawns.erase(idx);
    endif
  endfor
  SetGroupSpawns(region, group, spawns);
  set_critical(0);
endfunction

function DespawnRegion(region)
  var spawnregions := GetGlobalProperty("SpawnRegions");
  if (!spawnregions)
    spawnregions := dictionary;
  endif
  var rgroups := spawnregions[region];
  if (!rgroups)
    return 0;
  endif
  foreach grp in (rgroups.keys())
    DespawnGroup(region, grp);
  endforeach
endfunction

function DespawnGroup(region, group)
  var spawns := GetGroupSpawns(region, group);
  foreach spawn in spawns
    Despawn(spawn);
  endforeach
endfunction

function Despawn(serial)
  var npc := SystemFindObjectBySerial(serial);
  if (!npc)
    return 0;
  endif
  SetObjProperty(npc, "guardkill", 1);
  SetObjProperty(npc, "NoSound", 1);
  ApplyRawDamage(npc, GetMaxHp(npc) * 2);
  sleepms(100);
endfunction

function GetGroupSpawns(region, group)
  var spawnregions := GetGlobalProperty("SpawnRegions");
  if (!spawnregions)
    spawnregions := dictionary;
  endif
  if(!spawnregions.exists(region))
    spawnregions[region] := dictionary;
  endif
  var spawns := spawnregions[region][group];
  if (!spawns)
    return array; // Return empty array.
  endif
  return spawns;
endfunction

function SetGroupSpawns(region, group, byref spawns)
  set_critical(1);
  var spawnregions := GetGlobalProperty("SpawnRegions");
  if (!spawnregions)
    spawnregions := dictionary;
  endif
  if (!spawnregions.exists(region))
    spawnregions[region] := dictionary;
  endif
  spawnregions[region][group] := spawns;
  SetGlobalProperty("SpawnRegions", spawnregions);
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
  var spawnregfreq := GetGlobalProperty("SpawnRegionsFreq");
  if (!spawnregfreq)
    spawnregfreq := dictionary;
  endif
  foreach grp in (spawnregfreq[region].keys())
    EnableGroup(region, grp);
  endforeach
endfunction

function DisableRegion(region)
  var spawnregfreq := GetGlobalProperty("SpawnRegionsFreq");
  if (!spawnregfreq)
    spawnregfreq := dictionary;
  endif
  foreach grp in (spawnregfreq[region].keys())
    DisableGroup(region, grp);
  endforeach
endfunction

function EnableGroup(region, group)
  var freq := GetGroupFreq(region, group);
  if (freq != -1)
    return 0;
  endif
  SetGroupFreq(region, group, 1);
endfunction

function DisableGroup(region, group)
    SetGroupFreq(region, group, -1);
endfunction

function GetGroupFreq(region, group)
  var spawnregions := GetGlobalProperty("SpawnRegionsFreq");
  if (!spawnregions)
    spawnregions := dictionary;
  endif
  if (!spawnregions.exists(region))
    spawnregions[region] := dictionary;
  endif
  var freq := spawnregions[region][group];
  return freq;
endfunction

function SetGroupFreq(region, group, freq)
  set_critical(1);
  var spawnregions := GetGlobalProperty("SpawnRegionsFreq");
  if (!spawnregions)
    spawnregions := dictionary;
  endif
  if (!spawnregions.exists(region))
    spawnregions[region] := dictionary;
  endif
  spawnregions[region][group] := freq;
  SetGlobalProperty("SpawnRegionsFreq", spawnregions);
  set_critical(0);
endfunction

function ValidateSpawnRegions()
  var spawnregions := GetGlobalProperty("SpawnRegions");
  if(!spawnregions)
    spawnregions := dictionary;
  endif
  var spawnfreq := GetGlobalProperty("SpawnRegionsFreq");
  if (!spawnfreq)
    spawnfreq := dictionary;
  endif
  var cfggroups, ingrpcfg, spawns, obj, num, holder;
  foreach regname in (spawnregions.keys())
    if(!regcfg.exists(regname))
      DespawnRegion(regname);
      spawnregions.erase(regname);
      spawnfreq.erase(regname);
      continue;
    endif
    foreach grpname in (spawnregions[regname].keys())
      ingrpcfg := 0;
      var holder := {};
      var elm := regcfg.GetProp(regname);
      foreach cgrp in (elm.PropNames())
        if(cgrp != "rect")
          foreach thing in cgrp
            cgrp := SplitWords(thing);
            if(cgrp[1] == grpname)
              ingrpcfg := 1;
              break;
            endif
          endforeach
        endif
      endforeach
      if (!ingrpcfg)
        DespawnGroup(regname, grpname);
        (spawnregions[regname]).erase(grpname);
        (spawnfreq[regname]).erase(grpname);
        continue;
      endif
      var serial;
      for (num := 1; num <= (spawnregions[regname][grpname]).size(); num := num + 1)
        serial := spawnregions[regname][grpname][num];
        obj := SystemFindObjectBySerial(serial);
        if (!obj)
          (spawnregions[regname][grpname]).erase(num);
        endif
      endfor
      sleepms(1);
    endforeach
    sleepms(1);
  endforeach
  SetGlobalProperty("SpawnRegions", spawnregions);
  SetGlobalProperty("SpawnRegionsFreq", spawnfreq);
endfunction