use cfgfile;
use util;
use basic;
use os;
use uo;

/*
include "include/convert";
getSkill(who, SKILLID_WHATEVER): newGetSkill(who, SKILLID_WHATEVER)

who.strength : getStr(who) setStr(who, amount)
who.intelligence: getInt(who) strInt(who, amount)
who.dexterity: getDex(who) setDex(who, amount)

who.int_mod: getIntMod(who) setIntMod(who, amount)
who.str_mod: getStrMod(who) setStrMod(who, amount)
who.dex_mod: getDexMod(who) setDexMod(who, amount)

who.hp: getHP(who) setHp(who, amount)
who.maxhp: getMaxHp(who) 

who.stamina: getStam(who) setStam(who, amount)
who.maxstamina: getMaxStam(who)

who.mana: getMana(who) setMana(who, amount)
who.maxmana: getMaxMana(who)

who.hp_regen_rate : getOldHpRegenRate(who)           setOldHpRegenRate(who, amount)
who.stamina_regen_rate : getOldStaminaRegenRate(who) setOldStaminaRegenRate(who, amount)
who.mana_regen_rate : getOldManaRegenRate(who)       setOldManaRegenRate(who, amount)

helper functions: 
	getAttributeForSkillId: returns the attribute name (ie "Magery") for a skill ID

*/

use uo;

function getnewSkillIdForAttribute(attribute)
var cf := readconfigfile(":*:attributes");
return cf[attribute].skillid;
endfunction

function incstrmod(who, amt)
	var now := getstrmod(who);
	//syslog(who.name + "'s str mod is now " + now);
	now := now + (amt   );
	setstrmod(who,  now );
	 now := getstrmod(who);
	//syslog("After change " + who.name + "'s str mod is now " + now);
endfunction

function incattrmod(who, attr, amt)
	var now := getattributetemporarymod(who, attr);
	now := now + amt;
	setattributetemporarymod(who, attr, now);
	recalcvitals(who);
endfunction

function incdexmod(who, amt)
	var now := getdexmod(who);
	//syslog("Current dexmod for " + who.name + " is " + now);
	now := now + amt;
	
	setdexmod(who,  now );
	now := getdexmod(who);
	 //syslog("after change mod  is " + now);
endfunction

function incintmod(who, amt)
	var now := getintmod(who);
	 
	now := now + amt;
	setintmod(who,  now );
	now := getintmod(who);
 
	 
endfunction
 



function newSetRawSkill(who, skillid, value)
	var at := GetAttributeForSkillIdd(skillid);
	var v := rawskilltobaseskill(value);
	setattributebasevalue(who, at, v);
	recalcvitals(who);
endfunction



function LowerStam(who, amount := 1)
 
	var hisstam := getstam(who);
	hisstam := hisstam - amount;
	if (hisstam < 0)
		setstam(who, 0);
	else
		
		setstam(who, hisstam);
	endif
	recalcvitals(who);
	return getstam(who);
endfunction

function Lowermana(who, amount := 1)
	if ((getmana(who) - amount) < 0)
		setmana(who, 0);
	else
		setmana(who, getmana(who) - amount);
	endif
	recalcvitals(who);
	return getmana(who);
endfunction



function Lowerhp(who, amount := 1)
	if ((gethp(who) - amount) < 0)
		sethp(who, 0);
	else
		sethp(who, gethp(who) - amount);
	endif
	recalcvitals(who);
	return gethp(who);
endfunction

function incmana(who, amt)
	var m := getmana(who);
	var newm := m + amt;
	if (newm < 0)
		setmana(who, 0);
		return;
	endif
	if (newm > getmaxmana(who))
		setmana(who, getmaxmana(who));
		return;
	endif
	recalcvitals(who);
	setmana(who, newm);
endfunction

function incstam(who, amt)
	var s := getstam(who);
	var news := s + amt;
	if (news < 0)
		setstam(who, 0);
		return;
	endif
	if (news > getmaxstam(who))
		setmana(who, getmaxstam(who));
		return;
	endif
	recalcvitals(who);
	setstam(who, news);
endfunction




function NewGetSkill(who, skillid)
	
	var att := getAttributeForSkillIdd(skillid);
	
	return getAttributebasevalue(who, att)/10.0;
endfunction
 
function setOldHpRegenRate(who, amt)
	amt := cint(amt);
	//return setVitalRegenRate(who, "Life", amt * 12);
	setobjproperty(who, "#hpregen", (amt * 12));
	recalcvitals(who);
endfunction

function setOldStaminaRegenRate(who, amt)
	amt := cint(amt);
	setobjproperty(who, "#stamregen", (amt * 12));
	recalcvitals(who);
	//return setVitalRegenRate(who, "Stamina", amt * 12);
endfunction

function setOldManaRegenRate(who, amt)
	amt := cint(amt);
	setobjproperty(who, "#manaregen", (amt * 12));
	recalcvitals(who);
	//return setVitalRegenRate(who, "Mana", amt * 12);
endfunction
 
function getOldHpRegenRate(who)
	return GetVitalRegenRate( who,"Life" ) / 12;
endfunction

function getOldStaminaRegenRate(who)
	return GetVitalRegenRate( who,"Stamina" ) / 12;
endfunction

function getOldManaRegenRate(who)
	return GetVitalRegenRate( who,"Mana" ) / 12;
endfunction

function getMaxHp(who)
	var max := GetVitalMaximumValue( who, "Life" )  ;
	max := max / 100.0;
	return max;
endfunction



function getMaxStam(who)
	var max := GetVitalMaximumValue( who, "Stamina" )  ;
	max := max / 100.0;
	return max;
endfunction

function getMaxMana(who)
	var max := GetVitalMaximumValue( who, "Mana" )  ;
	max := max / 100.0;
	return max;
endfunction

function setMana(who, amount)
	
	SetVital( who, "Mana", cint(amount * 100.0));
	recalcvitals(who);
endfunction

function setHP(who, amount)
	SetVital( who, "Life", cint(amount * 100.0));
	recalcvitals(who);
endfunction

function gethunger(who)
	var v :=  getvital(who, "Hunger");
	recalcvitals(who);
	return v;
endfunction

function sethunger(who, amt)
	setvital(who, "Hunger", amt);
	recalcvitals(who);
endfunction

function incHunger(who, amt := 1)
	sethunger(who, gethunger(who) + amt);
	recalcvitals(who);
endfunction

function setStam(who, amount )
	var ret := SetVital( who, "Stamina", cint(amount * 100.0));
	recalcvitals(who);
	return ret; 
endfunction

function GetHp(who)
	var h := getVital(who, "Life");
	h := h / 100;
	return h ;
endfunction

function getMana(who)
	var h := getVital(who, "Mana");
	h := h / 100;
	return h ;
endfunction

function getStam(who)
	var h := getVital(who, "Stamina");
	h := h / 100;
	return h ;
endfunction

function GetAttributeForSkillIdd(skilld)
	var s := readconfigfile(":*:skillconv");
	return s[skilld].attribute;
endfunction

function setStrMod(who, amount)
	amount := amount * 10;
	amount := cint(amount);
	setAttributeTemporaryMod(who, "Strength", amount  );
	recalcvitals(who);
	return ;
endfunction

function setIntMod(who, amount)
	amount := amount * 10;
	amount := cint(amount);
	setAttributeTemporaryMod(who, "Intelligence", amount);
	recalcvitals(who);
endfunction

function setDexMod(who, amount)
	amount := amount * 10;
	amount := cint(amount);
	setAttributeTemporaryMod(who, "Dexterity",amount);
	recalcvitals(who);
	return;
endfunction


function getIntMod(who)
	var i := GetAttributeTemporaryMod( who, "Intelligence" );
	
	i := i / 10.0;
	return i ;
endfunction

function getDexMod(who)
	
	var i := GetAttributeTemporaryMod( who, "Dexterity" );
	i := i / 10.0;
	return i ;
	 
endfunction

function getStrMod(who)
	var i := GetAttributeTemporaryMod( who, "Strength" );
	i := i / 10.0;
	return i ;
endfunction


function getStr(who)
	var s := getAttribute(who, "Strength");
	return s;
endfunction

function getInt(who)
	var s := getAttribute(who, "Intelligence");
	return s;
endfunction

function getDex(who)
	var s := getAttribute(who, "Dexterity");
	return s;
endfunction

function setStr(who, amount)
	 setattributebasevalue(who,"Strength",  (amount*10));
	 recalcvitals(who);
endfunction

function setInt(who, amount)
	  setattributebasevalue(who,"Intelligence", (amount*10));
	  recalcvitals(who);
endfunction

function setDex(who, amount)
	 setattributebasevalue(who,"Dexterity",  (amount*10));
	 recalcvitals(who);
endfunction


function tell(who, msg)
	if (who.cmdlevel)
		sendsysmessage(who, msg);
	endif
endfunction

function getRealStr(who)
	var s := getattributebasevalue(who, "Strength");
	s := s / 10.0;
	return s;
endfunction

function getRealInt(who)
	var s := getattributebasevalue(who, "Intelligence");
	s := s / 10.0;
	return s;
endfunction

function getRealDex(who)
	var s := getattributebasevalue(who, "Dexterity");
	s := s / 10.0;
	return s;
endfunction

