Stun Punch & Disarm v1.1
By BlahGoD
Email blahgod@thehellclan.com
Core version 094


There might be alot of extra code laying around in this but its my 35425th revision/attempt at this.
Not To mention this is my first script i've wrote for POL.
If you find any mistakes or bugs feel free to update it and let me know so i can update too :-)

Installation:

1)extract stunpunch.src/disarm.src and .ecl and .dep to your scripts\textcmd\player directory and recompile .src just to be safe.

2)then add the following to mainHitScript.src and recompile it:

  var stunpunch := GetObjProperty(attacker,"StunMode");
  var disarmpunch := GetObjProperty(attacker,"DisarmMode"); 
  if(stunpunch)
    SetObjProperty(attacker,"WrestleTimer",ReadGameClock() + 10);
    SendSysMessage(attacker,"You stop your attempt at stunning your opponent.");
    SetObjProperty(attacker,"StunMode",0);
  elseif(disarmpunch)
    SetObjProperty(attacker,"WrestleTimer",ReadGameClock() + 10);
    SendSysMessage(attacker,"You stop your attempt at disarming your opponent.");
    SetObjProperty(attacker,"DisarmMode",0);
  endif

This makes sure no weapon is used in combonation with the stun effect and turns off wrestling mode.

3)then add the following to wrestlingHitScript.src and recompile it:

  include "include/statMod";  //put this in your include section

** add the following into the program body... i put mine right after program WrestlingHitScript(...)**

  var stunpunch := GetObjProperty(attacker,"StunMode");
  var disarmpunch := GetObjProperty(attacker,"DisarmMode");
  var chance := RandomDiceRoll("1d60+60");  //i just made this up temporarily to check for a successfull move, change as needed
  var anatomyskill := CInt(GetEffectiveSkill(attacker, SKILLID_ANATOMY));
  var armsloreskill := CInt(GetEffectiveSkill(attacker, SKILLID_ARMSLORE));
  var wrestlingskill := CInt(GetEffectiveSkill(attacker, SKILLID_WRESTLING));
  var hand1 := GetEquipmentByLayer(defender, 1);
  var hand2 := GetEquipmentByLayer(defender, 2);
  var bpack := GetEquipmentByLayer(defender, 21);
  if(stunpunch)
    if((chance < anatomyskill) and (chance < wrestlingskill))
	SendSysMessage(attacker, "You stun your opponent!");
	PlaySoundEffect(defender, SFX_SPELL_PARALYZE );
	SendSysMessage(defender, "You are stunned and cannot move!");
	DoTempMod(defender, "p" , 1, 5);
	ApplyStaminaDamage(attacker, 15);
	SetObjProperty(attacker,"StunMode",0);
	SetObjProperty(attacker,"WrestleTimer",ReadGameClock() + 10);
    else
	SendSysMessage(attacker, "You fail to stun your opponent.");
	ApplyStaminaDamage(attacker, 15);
    endif
    return;
  endif
  if(disarmpunch)
   if(hand1)
    if((chance < armsloreskill) and (chance < wrestlingskill))
	SendSysMessage(attacker, "You disarm your opponent!");
	PlaySoundEffect(defender, SFX_50);
	MoveItemToContainer( hand1, bpack, x := -1, y := -1 );
	ApplyStaminaDamage(attacker, 15);
	SetObjProperty(attacker,"DisarmMode",0);
	SetObjProperty(attacker,"WrestleTimer",ReadGameClock() + 10);
    else
	SendSysMessage(attacker, "You fail to disarm your opponent.");
	ApplyStaminaDamage(attacker, 15);
    endif
   elseif(hand2)
      if((chance < armsloreskill) and (chance < wrestlingskill))
	SendSysMessage(attacker, "You disarm your opponent!");
	PlaySoundEffect(defender, SFX_50);
	MoveItemToContainer( hand2, bpack, x := -1, y := -1 );
	ApplyStaminaDamage(attacker, 15);
	SetObjProperty(attacker,"DisarmMode",0);
	SetObjProperty(attacker,"WrestleTimer",ReadGameClock() + 10);
    else
	SendSysMessage(attacker, "You fail to disarm your opponent.");
	ApplyStaminaDamage(attacker, 15);
    endif
   else
     SendSysMessage(attacker, "Your opponent is not armed!");
     ApplyStaminaDamage(attacker, 15);
     SetObjProperty(attacker,"DisarmMode",0);
     SetObjProperty(attacker,"WrestleTimer",ReadGameClock() + 10);
   endif
   return;
  endif

4)Restart POL

Usage:  the command is .stunpunch or .disarm to turn on/off wrestling mode. if you want to know wtf this does go to uo.stratics.com

Things to do:

Make it check if item in hand2 is a weapon or shield (as of now it will disarm shields)
Make a better chance system
Clean up code
Alot

Thats it, enjoy!

-[BlahGoD]