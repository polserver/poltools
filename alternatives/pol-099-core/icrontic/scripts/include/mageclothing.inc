//include "passivemana";

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

function protectionclothing (cast_on, mod_amount, duration)
	
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
			   	duration:= duration *1;
			   	sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   	break;
				5: mod_amount:= mod_amount *3;
				duration:= duration *2;
				sendsysmessage (cast_on, "Your clothing glows!",3,53);
				break;
			endcase
		endif
	endforeach  
	Return (mod_amount); 

endfunction

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
		if (resist = 1)
		case (resist)
			1: halfamt:= cint(halfamt/6*5);
			   sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   break;
			2: halfamt:= cint(halfamt/6*4);
			   sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   break;
			3: halfamt:= cint(halfamt/6*3);
			   sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   break;
			4: halfamt:= cint(halfamt/6*2);
			   sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   break;
			5: halfamt:= cint(halfamt/6*1);
			   sendsysmessage (cast_on, "Your clothing glows!",3,53);
			   break;
		endcase
		endif
          
	endforeach  
	Return (halfamt);  

endfunction

function lightningclothing (caster, dmg)

	if (checkforarmor (caster) = 1)
		return (dmg);
	endif
	
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
			1: dmg:=cint(dmg*1.5);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			2: dmg:=cint(dmg*2);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			3: dmg:=cint(dmg*2.5);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			4: dmg:=cint(dmg*3);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			5: dmg:=cint(dmg*3.5);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
		endcase
		endif
          
	endforeach  
	Return (dmg);  	
endfunction

function fireclothing (caster, dmg)

	if (checkforarmor (caster) = 1)
		return (dmg);
	endif
	
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
			1: dmg:=cint(dmg*1.5);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			2: dmg:=cint(dmg*2);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			3: dmg:=cint(dmg*2.5);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			4: dmg:=cint(dmg*3);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			5: dmg:=cint(dmg*3.5);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
		endcase
		endif
        
	endforeach  
	Return (dmg);    	
endfunction

/*

function manaclothing (caster, circle)
	
	if (checkforarmor (caster) = 1)
		return;
	endif
	
	var item;
	var manaclothes:= 0;
	var originalmana:= 0;
		
		case (circle)
			1: originalmana:= 5;
			break;
			2: originalmana:= 6;
			break;
			3: originalmana:= 9;
			break;
			4: originalmana:= 11;
			break;
			5: originalmana:= 14;
			break;
			6: originalmana:= 20;
			break;
			7: originalmana:= 40;
			break;
			8: originalmana:= 50;
			break;
		endcase
	
	print ("originalmana = " + originalmana);
		
	foreach item in ListEquippedItems(caster)
		manaclothes:= getobjproperty (item, "manaclothing");
		if (manaclothes !=ERROR)
			count:= count +1;
		endif		
		if (count = 1)
		case (manaclothes)
			1: originalmana:= cint(originalmana /6);				
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			2: originalmana:= cint(originalmana /5);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			3: originalmana:= cint(originalmana /4);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			4: originalmana:= cint(originalmana /3);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			5: originalmana:= cint(originalmana /2);
			   sendsysmessage (caster, "Your clothing glows!",3,53);
			   break;
			default: originalmana := 0;
		endcase
		endif
	endforeach  
	caster.mana:= caster.mana + originalmana;
	print ("mana = " + caster.mana);

endfunction
*/

function manaregeneration (chr, item)


var manaregen2;
var count:=0;


	setmanaregenrate( chr );
	
	if (checkforarmor (chr) = 1)
		return;
	endif
	
	var manaregen:= 0;
	manaregen:= getobjproperty (item, "manaregen");
	
		case (manaregen)
			1: chr.mana_regen_rate:= cint((chr.mana_regen_rate/10)*12);	
		SendSysmessage(chr, "Your regenerate mana more easily.", 3 , 48);		
			   break;
			2: chr.mana_regen_rate:=  cint((chr.mana_regen_rate/10)*14);	
			SendSysmessage(chr, "Your regenerate mana more easily.", 3 , 48);
			   break;
			3: chr.mana_regen_rate:=  cint((chr.mana_regen_rate/10)*16);
			SendSysmessage(chr, "Your regenerate mana more easily.", 3 , 48);	
			   break;
			4: chr.mana_regen_rate:=  cint((chr.mana_regen_rate/10)*18);
			SendSysmessage(chr, "Your regenerate mana more easily.", 3 , 48);	
			   break;
			5: chr.mana_regen_rate:=  cint((chr.mana_regen_rate/10)*20);	
			SendSysmessage(chr, "Your regenerate mana more easily.", 3 , 48);
			   break;
		endcase
		


endfunction

function unmanaregeneration (chr, item)

	setmanaregenrate( chr );

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
			1: mana:= mana*1.5;
			   break;
			2: mana:= mana*2;
			   break;
			3: mana:= mana*2.5;
			   break;
			4: mana:= mana*3;
			   break;
			5: mana:= mana*3.5;
			   break;
		endcase
		endif
          
	endforeach  
	Return (mana);  	
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
			1: mana:= mana*1.5;
			   break;
			2: mana:= mana*2;
			   break;
			3: mana:= mana*2.5;
			   break;
			4: mana:= mana*3;
			   break;
			5: mana:= mana*3.5;
			   break;
		endcase
		endif
          
	endforeach  
	Return (mana);  	
endfunction


function makemageclothing( item )

   	var rndnum:= randomint(5)+1;
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

