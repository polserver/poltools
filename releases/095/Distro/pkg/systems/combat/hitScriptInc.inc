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
  fame := fame + CInt( (vfame / 10) / ( CInt(Log10(fame)) + 1 ) );
  if !(defender.npctemplate)
	vfame := CInt(vfame * 0.9);
  endif
  if((karma < -15) && (vkarma < -15))
	return;
  endif
  if(vkarma > 15)
	kmod := -1;
	karma := Abs(karma);
  else
	vkarma := Abs(vkarma);
  endif
  kmod := kmod * CInt( (vkarma / 10) / ( CInt(Log10(karma)) + 1 ) );
  karma := karma + kmod;
endfunction