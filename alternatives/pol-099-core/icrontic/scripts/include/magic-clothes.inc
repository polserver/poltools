///////////////////////////////////////////////////////
//     
//
//        INDEX
//     - Check for armor functions
//     - Protection clothes
//     - Resist clothes
//     - Lightining clothes and mana
//     - Fire clothes and mana
//     - Mana clothes and reset function
//     - HP clothes and reset function	
//     - Main mageclothing creation function
//     - Utility functions
//
///////////////////////////////////////////////////////


////********CHECK FOR ARMOR (NORMAL OR LEATHER)********
function checkforarmor(cast_on)
	
	var cfg := readconfigfile(":combat:itemdesc");
	var item;

	foreach item in ListEquippedItems(cast_on)
      		if ((cfg[item.objtype].blockcircle) && 
		(cfg[item.objtype].blockcircle <= 5) 
		&& (!cfg[item.objtype].errortext) or (cfg[item.objtype].hitsound))
                	return 1;
		endif
	endforeach
	
endfunction

function checkforleather(cast_on)
	
	var cfg := readconfigfile(":combat:itemdesc");
	var item;

	foreach item in ListEquippedItems(cast_on)
      		if ((cfg[item.objtype].blockcircle) && 
		(cfg[item.objtype].blockcircle <= 8) 
		&& (!cfg[item.objtype].errortext) or (cfg[item.objtype].hitsound))
                	return 1;
                 endif
	endforeach
	
endfunction


////********PROTECTION CLOTHES********
function protectionclothing (cast_on, mod_amount)
	
	if (checkforleather (cast_on) = 1)
		sendsysmessage (cast_on, "Your protection clothes are inhibited by your equipment!",3,53);
		return (mod_amount);
	endif
	
	var item;
	var protection:= 0;
	var count:=0;
	
	foreach item in ListEquippedItems(cast_on)
		protection:= getobjproperty (item, "protection");
		if (protection !=ERROR)
			count:= count +1;
		endif
		if (count = 1)
			case (protection)
				1: mod_amount:= mod_amount *1.5;
			   	sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   	break;
				2: mod_amount:= mod_amount *2;
			   	sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   	break;
				3: mod_amount:= mod_amount *2.5;
			   	sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   	break;
				4: mod_amount:= mod_amount *3;
			   	sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   	break;
				5: mod_amount:= mod_amount *3;
				sendsysmessage (cast_on, "Your clothing glows!",3,53);
				break;
			endcase
		endif
	endforeach  
	Return (mod_amount); 

endfunction

function protectionclothingduration (cast_on, duration)
	
	if (checkforleather (cast_on) = 1)
		sendsysmessage (cast_on, "Your protection clothes are inhibited by your equipment!",3,53);
		return (duration);
	endif
	
	var item;
	var protection:= 0;
	var count:=0;
	
	foreach item in ListEquippedItems(cast_on)
		protection:= getobjproperty (item, "protection");
		if (protection !=ERROR)
			count:= count +1;
		endif
		if (count = 1)
			case (protection)
				1: duration:= duration *1;
			   	break;
				2: duration:= duration *1;
			   	break;
				3: duration:= duration *1;
			   	break;
				4: duration:= duration *1.5;
			   	break;
				5: duration:= duration *2;
				break;
			endcase
		endif
	endforeach  
	Return (duration); 

endfunction

////********RESIST CLOTHES********
function resistclothing (cast_on,halfamt)

	if (checkforarmor (cast_on) = 1)
		return (halfamt);
	endif
	var item;
	var resist:= 0;
	var count:=0;
	
	foreach item in ListEquippedItems(cast_on)
		resist:= getobjproperty (item, "resist");
		if (resist !=ERROR)
			count:= count +1;
		endif
		if (count = 1)
		case (resist)
			1: halfamt:= cint(halfamt/6*5);
			   sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   break;
			2: halfamt:= cint(halfamt/6*4.5);
			   sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   break;
			3: halfamt:= cint(halfamt/6*4);
			   sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   break;
			4: halfamt:= cint(halfamt/6*3.5);
			   sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   break;
			5: halfamt:= cint(halfamt/6*3);
			   sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   break;
		endcase
		endif
          
	endforeach  
	Return (halfamt);  

endfunction


////********LIGHTINING CLOTHES AND MANA********
function lightningclothing (caster, dmg)

	if (checkforarmor (caster) = 1)
		return (dmg);
	endif
	
	var lightning:= 0;
	var item;
	var count:=0;
	var old_dmg := dmg;
	var multiplier;
	
	foreach item in ListEquippedItems(caster)
		lightning:= getobjproperty (item, "lightning_clothing");
		if (lightning !=ERROR)
			count:= count +1;
		endif
		if (count = 1)
		case (lightning)
			1: multiplier := 1.3 * skill_adjust_cloth(lightning, SKILLID_EVALINT, caster);
			   dmg:=cint(dmg*multiplier.dmg);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			2: multiplier := 1.5 * skill_adjust_cloth(lightning, SKILLID_EVALINT, caster);
			   dmg:=cint(dmg*multiplier.dmg);
                           sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			3: multiplier := 1.8 * skill_adjust_cloth(lightning, SKILLID_EVALINT, caster);
			   dmg:=cint(dmg*multiplier.dmg);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			4: multiplier := 2.1 * skill_adjust_cloth(lightning, SKILLID_EVALINT, caster);
			   dmg:=cint(dmg*multiplier.dmg);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			5: multiplier := 2.4 * skill_adjust_cloth(lightning, SKILLID_EVALINT, caster);
			   dmg:=cint(dmg*multiplier.dmg);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
		endcase
		endif
          
	endforeach  
	Return (dmg);  	
endfunction

function lightningmana (caster, mana)

	//if (checkforarmor (caster) = 1)
	//	return (dmg);
	//endif
	
	var lightning:= 0;
	var item;
	var count:=0;

	foreach item in ListEquippedItems(caster)
		lightning:= getobjproperty (item, "lightning_clothing");
		if (lightning !=ERROR)
			count:= count +1;
		endif
		if (count = 1)
		case (lightning)
			1: mana:= mana*1.2;
			   break;
			2: mana:= mana*1.4;
			   break;
			3: mana:= mana*1.6;
			   break;
			4: mana:= mana*1.8;
			   break;
			5: mana:= mana*2;
			   break;
		endcase
		endif
          
	endforeach  
	Return (mana);  	
endfunction


////********FIRE CLOTHES AND MANA********
function fireclothing (caster, dmg)

	if (checkforarmor (caster) = 1)
		return (dmg);
	endif
	
	var fire:= 0;
	var item;
	var count:=0;
	var old_dmg := dmg;
	var multiplier;
	
	foreach item in ListEquippedItems(caster)
		fire:= getobjproperty (item, "fire_clothing");
		if (fire !=ERROR)
			count:= count +1;
		endif		
		if (count = 1)
		case (fire)
			1: multiplier := 1.3 * skill_adjust_cloth(fire, SKILLID_EVALINT, caster);
			   dmg:=cint(dmg*multiplier.dmg);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			2: multiplier := 1.5 * skill_adjust_cloth(fire, SKILLID_EVALINT, caster);
			   dmg:=cint(dmg*multiplier.dmg);
                           sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			3: multiplier := 1.8 * skill_adjust_cloth(fire, SKILLID_EVALINT, caster);
			   dmg:=cint(dmg*multiplier.dmg);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			4: multiplier := 2.1 * skill_adjust_cloth(fire, SKILLID_EVALINT, caster);
			   dmg:=cint(dmg*multiplier.dmg);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			5: multiplier := 2.4 * skill_adjust_cloth(fire, SKILLID_EVALINT, caster);
			   dmg:=cint(dmg*multiplier.dmg);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
		endcase
		endif
        
	endforeach  
	Return (dmg);    	
endfunction

function firemana (caster, mana)

	//if (checkforarmor (caster) = 1)
	//	return (dmg);
	//endif
	
	var fire:= 0;
	var item;
	var count:=0;
	
	foreach item in ListEquippedItems(caster)
		fire:= getobjproperty (item, "fire_clothing");
		if (fire !=ERROR)
			count:= count +1;
		endif
		if (count = 1)
		case (fire)
			1: mana:= mana*1.2;
			   break;
			2: mana:= mana*1.4;
			   break;
			3: mana:= mana*1.6;
			   break;
			4: mana:= mana*1.8;
			   break;
			5: mana:= mana*2;
			   break;
		endcase
		endif
          
	endforeach  
	Return (mana);  	
endfunction


////********MANA CLOTHES AND RESET MANA FUNCTION********
function manaregeneration (caster, item)

var count:=0;


	nlsetmanaregenrate( caster );
	
	if (checkforarmor (caster) = 1)
		return;
	endif
	
	var manaregen:= 0;
	manaregen:= getobjproperty (item, "manaregen");
	
var oldregenrate := GetObjProperty( caster, "mana_regen_rate" );
               case (manaregen)
			1: SetObjProperty( caster, "mana_regen_rate", 1800 * skill_adjust_cloth(manaregen, SKILLID_MEDITATION, caster) );
			RecalcVitals( caster );
		        SendSysmessage(caster, "Your regenerate mana more easily.", 3 , 48);
		        var oldregenrate2 := GetObjProperty( caster, "mana_regen_rate" );
			   break;
			2: SetObjProperty( caster, "mana_regen_rate", 2400 * skill_adjust_cloth(manaregen, SKILLID_MEDITATION, caster) );
			RecalcVitals( caster );	
			SendSysmessage(caster, "Your regenerate mana more easily.", 3 , 48);
			var oldregenrate2 := GetObjProperty( caster, "mana_regen_rate" );
			   break;
			3: SetObjProperty( caster, "mana_regen_rate", 3600 * skill_adjust_cloth(manaregen, SKILLID_MEDITATION, caster) );
			RecalcVitals( caster );
			SendSysmessage(caster, "Your regenerate mana more easily.", 3 , 48);
			var oldregenrate2 := GetObjProperty( caster, "mana_regen_rate" );
			   break;
			4: SetObjProperty( caster, "mana_regen_rate", 4800 * skill_adjust_cloth(manaregen, SKILLID_MEDITATION, caster) );
			RecalcVitals( caster );
			SendSysmessage(caster, "Your regenerate mana more easily.", 3 , 48);
			var oldregenrate2 := GetObjProperty( caster, "mana_regen_rate" );
			   break;
			5: SetObjProperty( caster, "mana_regen_rate", 6000 * skill_adjust_cloth(manaregen, SKILLID_MEDITATION, caster) );
			RecalcVitals( caster );	
			SendSysmessage(caster, "Your regenerate mana more easily.", 3 , 48);
			var oldregenrate2 := GetObjProperty( caster, "mana_regen_rate" );
			   break;
		endcase

endfunction

function unmanaregeneration (caster, item)

	nlsetmanaregenrate( caster );

endfunction

function nlsetmanaregenrate( character )

        const lowestmedirate := 1200;
	var circle:=1;
	var cfg := readconfigfile(":combat:itemdesc");
	var medi := GetEffectiveSkill(character, SKILLID_MEDITATION) * 6;
	var oldregenrate := cint(GetObjProperty( character, "mana_regen_rate" ));
	
	if (medi < lowestmedirate)
	    medi := lowestmedirate;
	endif
        
        SetObjProperty( character, "mana_regen_rate", CInt( medi ) );

	foreach item in ListEquippedItems(character)
      	    if ((cfg[item.objtype].blockcircle) && (cfg[item.objtype].blockcircle <= circle) && !cfg[item.objtype].errortext)
		SetObjProperty( character, "mana_regen_rate", CInt( lowestmedirate ) );
	    endif
	endforeach	
	
	RecalcVitals( character );
	
// Display a message to the character
	if (oldregenrate != GetObjProperty( character, "mana_regen_rate" ))
	    if (oldregenrate > GetObjProperty( character, "mana_regen_rate" ))
		SendSysmessage(character, "Your mana regeneration becomes inhibited", 3 , 48);
	      else
		SendSysmessage(character, "Your regenerate mana more easily.", 3 , 48);
	    endif
	endif

endfunction

////********HP CLOTHES AND RESET HP FUNCTION********
function hpregeneration (caster, item)

var count:=0;


	nlsethpregenrate( caster );
	
	if (checkforarmor (caster) = 1)
		return;
	endif
	
	var hpregen:= 0;
	hpregen:= getobjproperty (item, "hpregen");
	
var oldregenrate := GetObjProperty( caster, "hp_regen_rate" );
               case (hpregen)
			1: SetObjProperty( caster, "hp_regen_rate", 1800 * skill_adjust_cloth(hpregen, SKILLID_HEALING, caster) );
			RecalcVitals( caster );
		        SendSysmessage(caster, "Your regenerate hp more easily.", 3 , 48);
		        var oldregenrate2 := GetObjProperty( caster, "hp_regen_rate" );
			   break;
			2: SetObjProperty( caster, "hp_regen_rate", 2400 * skill_adjust_cloth(hpregen, SKILLID_HEALING, caster) );
			RecalcVitals( caster );	
			SendSysmessage(caster, "Your regenerate hp more easily.", 3 , 48);
			var oldregenrate2 := GetObjProperty( caster, "hp_regen_rate" );
			   break;
			3: SetObjProperty( caster, "hp_regen_rate", 3600 * skill_adjust_cloth(hpregen, SKILLID_HEALING, caster) );
			RecalcVitals( caster );
			SendSysmessage(caster, "Your regenerate hp more easily.", 3 , 48);
			var oldregenrate2 := GetObjProperty( caster, "hp_regen_rate" );
			   break;
			4: SetObjProperty( caster, "hp_regen_rate", 4800 * skill_adjust_cloth(hpregen, SKILLID_HEALING, caster) );
			RecalcVitals( caster );
			SendSysmessage(caster, "Your regenerate hp more easily.", 3 , 48);
			var oldregenrate2 := GetObjProperty( caster, "hp_regen_rate" );
			   break;
			5: SetObjProperty( caster, "hp_regen_rate", 6000 * skill_adjust_cloth(hpregen, SKILLID_HEALING, caster) );
			RecalcVitals( caster );	
			SendSysmessage(caster, "Your regenerate hp more easily.", 3 , 48);
			var oldregenrate2 := GetObjProperty( caster, "hp_regen_rate" );
			   break;
		endcase

endfunction

function unhpregeneration (caster, item)

	nlsethpregenrate( caster );

endfunction

function nlsethpregenrate( character )

        const lowesthprate := 1200;
	var circle:=1;
	var cfg := readconfigfile(":combat:itemdesc");
	var oldregenrate := cint(GetObjProperty( character, "hp_regen_rate" ));
	        
        SetObjProperty( character, "hp_regen_rate", CInt( lowesthprate ) );

	RecalcVitals( character );
	
// Display a message to the character
	if (oldregenrate != GetObjProperty( character, "hp_regen_rate" ))
	    if (oldregenrate > GetObjProperty( character, "hp_regen_rate" ))
		SendSysmessage(character, "Your hit points regeneration becomes inhibited", 3 , 48);
	      else
		SendSysmessage(character, "Your regenerate hit points more easily.", 3 , 48);
	    endif
	endif

endfunction

////********MAIN MAGECLOTHING CREATION FUNCTION********

function makemageclothing( item )

   	var rndnum:= randomint(6)+1;
   	var numb:= randomint(100);
	var pref;
  	var suffix;
  	var obj;
  	var pricemod := 0;

	case (rndnum)
		1: suffix:= "of Protection";
		obj:= "protection";
		break;
		2: suffix:= "of Mana Regeneration";
		obj:= "manaregen";
		break;	
		3: suffix:= "of Spell Resistance";
		obj:= "resist";
		break;
		4: suffix:= "of Lightning";
		obj:= "lightning_clothing";
		break;
		5: suffix:= "of Fire";
		obj:= "fire_clothing";
		break;
		6: suffix:= "of Health Regeneration";
		obj:= "hpregen";
		break;
	endcase

   if( numb >= 50 )
       SetObjProperty( item , obj , 1 );
       pref := "Schollar";
       pricemod := 2;
   elseif( numb >= 25 )
       SetObjProperty( item , obj , 2 );
       pref := "Learned";
       pricemod := 3;
   elseif( numb >= 15 )
       SetObjProperty( item , obj , 3 );
       pref := "Seer";
       pricemod := 4;
   elseif( numb >= 5 )
       SetObjProperty( item , obj , 4 );
       pref := "Soothsayer";
       pricemod := 5;
   elseif( numb >= 1 )
       SetObjProperty( item , obj , 5 );
       pref := "Oracle";
       pricemod := 6;
   endif
   
   	item.buyprice := item.buyprice * pricemod;

	SetObjProperty(item,"prefix",pref);
	SetObjProperty(item,"suffix",suffix);

endfunction

////********UTILITY FUNCTIONS********

function skill_adjust_cloth (level, skill_id, caster)

case(level)
1: level:= 20.0;
   break;
2: level:= 40.0;
   break;
3: level:= 60.0;
   break;
4: level:= 80.0;
   break;
5: level:= 100.0;
   break;
endcase

var skill := cint(GetEffectiveSkill( caster, skill_id ))/level;
if (skill > 1)
   skill := 1;
endif
return skill;

endfunction