use uo;
use os;

include "include/client";
include "include/attributes";

function summon_creature( caster, circle := 8, npctemplate, cast_loc, gate)
  var thecreature;
  var thecritter;
  var magery := GetEffectiveSkill(caster, SKILLID_MAGERY);
  var duration := 20 + (magery * 2);
  var parms := {};
  parms .+ script := "tamed";
  Detach();
  sleep(2);
  thecritter := CreateNpcFromTemplate(npctemplate, 5849, 1207, 0, parms);
  if(npctemplate == "daemon")
    thecritter.setCriminal(1);
    caster.setCriminal(1);
  endif
  MoveCharacterToLocation(thecritter, cast_loc.x, cast_loc.y, cast_loc.z, MOVECHAR_FORCELOCATION);
  sleepms(500);
  DestroyItem(gate);
  SetObjProperty(thecritter,"master", caster.serial);
  SetScriptController( thecritter);
  SetObjProperty(thecritter, "happiness",100);
  SetObjProperty(thecritter, "summoned",1);
  RestartScript(thecritter);
  SetBaseSkillBaseValue( thecritter, SKILLID_MAGICRESISTANCE,  magery*10  );
  PlaySoundEffect(caster, SFX_SPELL_SUMMON_ELEMENTAL);
  sleep(duration);
  PlaySoundEffect( thecritter, SFX_SPELL_MANA_VAMPIRE );
  PlayStationaryEffect( thecritter.x, thecritter.y, thecritter.z, FX_SMOKE, 0xa, 0xa );
  MoveCharacterToLocation(thecritter,0,0,0,MOVECHAR_FORCELOCATION);
  Damage(thecritter, GetHp(thecritter) + 3);
endfunction

