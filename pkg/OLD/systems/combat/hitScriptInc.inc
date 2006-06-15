use uo;
use util;
use math;
include ":attributes:attributes";

function DealDamage(defender, attacker, rawdamage)
  rawdamage := rawdamage + RandomInt( (AP_GetSkill(attacker, ANATOMY) ) / 20);
  ApplyRawDamage(defender, rawdamage);
endfunction