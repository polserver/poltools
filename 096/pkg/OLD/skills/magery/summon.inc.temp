use uo;
use os;

include "include/client";
include "include/sound";
include "include/tileEffects";
include ":attributes:attributes";


function DoSummon( caster, circle, npctemplate, cast_loc )
  var thecritter;
  var magery := AP_GetSkill(caster, MAGERY);
  var duration := 20 + (magery * 2);
  var parms := {};
  parms .+ script := "tamed";
  Detach();
  sleepms(100);
  thecritter := CreateNpcFromTemplate(npctemplate, cast_loc.x, cast_loc.y,cast_loc.z, parms, caster.realm);
  if(npctemplate == "summoneddaemon")
    PlaySoundEffect(caster, 535);
  else
    if(circle == 8)
      PlaySoundEffect(caster, 536);
      PerformAction(thecritter, 12);
    else
      PlaySoundEffect(caster, 534);
    endif
  endif
  SetObjProperty(thecritter,"master", caster.serial);
  SetScriptController( thecritter);
  SetObjProperty(thecritter, "happiness",100);
  SetObjProperty(thecritter, "summoned",1);
  RestartScript(thecritter);
  //SetBaseSkillBaseValue( thecritter, SKILLID_MAGICRESISTANCE,  magery * 10  );
  AP_SetTrueSkill(thecritter, RESISTING_SPELLS, magery*10);
  sleep(duration);
  PlaySoundEffect( thecritter, SFX_SPELL_MANA_VAMPIRE );
  PlayStationaryEffect( thecritter.x, thecritter.y, thecritter.z, FX_SMOKE, 0xa, 0xa, 0, caster.realm );
  MoveCharacterToLocation(thecritter,0,0,0,MOVECHAR_FORCELOCATION);
  ApplyRawDamage(thecritter, AP_GetVital(thecritter, HITS) + 3);
endfunction
