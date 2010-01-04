use uo;
use util;
use math;
include "include/attributes";

function DealDamage(defender, attacker, rawdamage)
  rawdamage := rawdamage + RandomInt( (GetEffectiveSkill(attacker, 1) ) / 20);
  ApplyRawDamage(defender, rawdamage);
endfunction

function AdjustNoto(attacker, defender)
  if!(defender.dead)
	return 0;
  endif
  var karma := CInt(GetObjProperty(attacker, "Karma") );
  var fame := CInt(GetObjProperty(attacker, "Fame") );
  var vkarma := CInt(GetObjProperty(defender, "Karma") );
  var vfame := CInt(GetObjProperty(defender, "Fame") );
  var kmod := 1;
  Fame := Fame + CInt( (vFame / 10) / ( CInt(Log10(Fame)) + 1 ) );
  if !(defender.npctemplate)
	vFame := CInt(vFame * 0.9);
  endif
  if((Karma < -15) && (vKarma < -15))
	return;
  endif
  if(vKarma > 15)
	kmod := -1;
	Karma := Abs(Karma);
  else
	vKarma := Abs(vKarma);
  endif
  kmod := kmod * CInt( (vkarma / 10) / ( CInt(Log10(Karma)) + 1 ) );
  Karma := Karma + kmod;
endfunction
