use uo;
use os;

include "include/client";
include "include/attributes";

function summon_creature( caster, circle, npctemplate, cast_loc )
  var thecreature;
  var thecritter;
  var magery := GetEffectiveSkill(caster, SKILLID_MAGERY);
  var duration := 20 + (magery * 2);
  var parms := {};
  parms .+ script := "tamed";
  Detach();
  sleepms(100);
  thecritter := CreateNpcFromTemplate(npctemplate, cast_loc.x, cast_loc.y,cast_loc.z, parms);
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
  SetBaseSkillBaseValue( thecritter, SKILLID_MAGICRESISTANCE,  magery * 10  );
  sleep(duration);
  PlaySoundEffect( thecritter, SFX_SPELL_MANA_VAMPIRE );
  PlayStationaryEffect( thecritter.x, thecritter.y, thecritter.z, FX_SMOKE, 0xa, 0xa );
  MoveCharacterToLocation(thecritter,0,0,0,MOVECHAR_FORCELOCATION);
  Damage(thecritter, GetHp(thecritter) + 3);
endfunction

