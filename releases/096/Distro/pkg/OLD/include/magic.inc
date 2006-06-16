const EVID_DISCHARGE := 0x123457;

include ":attributes:attributes";


function Resisted(circle, caster, cast_on, amt)
  // Purpose: to check for resistance to spells cast upon a character
  // called by: most spells
  // returns: amount of damage not resisted?

  var magery := CInt(AP_GetSkill(caster, MAGERY));
  var evalint := CInt(AP_GetSkill(caster, EVALINT));
  var resist := CInt(AP_GetSkill(cast_on, RESISTING_SPELLS));
        
  // chk1 relates to the resist ability.
  var chk1 := (resist / 5);
  // chk2 relates to the difficulty level
  var chk2 := resist - (((magery - 20) / 5) + (circle * 5));
        
  if(chk1 < 1)
    chk1 := 1;
  endif
  if(chk2 < 1)
    chk2 := 1;
  endif
        
  // What an odd construct.  
  // If you're hit with an easy spell then you have a difficulty of your resist
  var diff := 0;
  if(chk1 > chk2)
    diff := chk1;
  else
    diff := chk2;
  endif
        
  if(cast_on.dead)
    return 0;
  endif
  if( SkillCheck(cast_on, RESISTING_SPELLS, diff) > 0 )
    SendSysMessage(cast_on, "You feel yourself resisting magical energy!");
    //PlaySoundEffect(cast_on, SFX_SPELL_WEAKEN);
    //PlayObjectCenteredEffect(cast_on, FX_CURSE_EFFECT, 5,5);
    amt := (amt * 0.5);
    if(amt < 1)
      amt := 1;
    endif
  endif
  var modamt := 1;
  if(resist > evalint)
    modamt := (1 + ((evalint - resist) / 200.0));
  elseif(evalint > resist)
    modamt := (1 + ((evalint - resist) / 500.0));
  endif
  amt := (amt * modamt);
  return CInt(amt);
endfunction

function getresistpoints(c, victim, skillid)
  var points;
  var diff;
  case(c)
    1:   points :=  20; diff := 20;
    2:   points :=  40; diff := 30;
    3:   points :=  80; diff := 40;
    4:   points := 100; diff := 50;
    5:   points := 120; diff := 60;
    6:   points := 140; diff := 70;
    7:   points := 160; diff := 86;
    8:   points := 180; diff := 100;
  endcase
  var skill := AP_GetSkill(victim, skillid);
  if(skill > (diff - 20))
    if((skill - diff) <= 5)
      points := points;
    elseif((skill - diff) <= 10)
      points := (points * 3) / 4;
    elseif((skill - diff) <= 15)
      points := points / 2;
    elseif((skill - diff) <= 15)
      points := points / 4;
    else
      points := 0;
    endif
  endif
  return points;
endfunction

function CalcSpellDamage(circle, caster)
  var die_string := circle+"d8";
  var dmg := RandomDiceRoll(die_string);
  dmg := dmg + CInt(AP_GetSkill(caster, MAGERY)/10);
  if(dmg > (circle * 8))
    dmg := circle * 8;
  endif
  return dmg;
endfunction