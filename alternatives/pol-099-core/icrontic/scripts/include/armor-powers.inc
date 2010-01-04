Const ArmorNeeded:= 5;

function powertype (me,item)

	var power:= GetObjProperty (item, "power");
	case (power)
                LICHE:  	//Liche Armor
                COPPER:  	//blacksmith armor
                OBSIDEAN: 	//Allow casting up to lvl 6
                SILVER: 	//Reflective Armor to undead
                DAEMON:   	//immune to poison
                ACID:   	//reflective armor
		TITAN:		//full damage - drains mana, stamina
        endcase
endfunction           

function p_silver (me)
	
	var powername := "SILVER";
	var SILVERTotal:= ArmourCount(me, powername);

	if (SILVERTotal == 5)
	  SetObjProperty(me , "a_equipped", powername);
          SendSysMessage(me, "Fear the undead no more!");
		return 2;
	else
	  EraseObjProperty(me , "a_equipped");
		return 0;
	endif

endfunction   

function p_silverunequip (me)

	  EraseObjProperty(me , "a_equipped");
		return 0;
endfunction

function p_liche (me)

	var powername := "LICHE";
	var LICHETotal:= ArmourCount(me, powername);

	if (LICHETotal == 5)
	  SetObjProperty(me , "a_equipped", powername);
          SendSysMessage(me, "Liche Armor activated!");
		return 2;
	else
	  EraseObjProperty(me , "a_equipped");
		return 0;
	endif

endfunction

function p_licheunequip (me)

	  EraseObjProperty(me , "a_equipped");
		return 0;
endfunction

function p_acid (me)

	var powername := "ACID";
	var ACIDTotal:= ArmourCount(me, powername);

	if (ACIDTotal == ArmorNeeded)
	  SetObjProperty(me , "a_equipped", powername);
          SendSysMessage(me, "Do unto others, as they do to you.");
		return 2;
	else
	  EraseObjProperty(me , "a_equipped");
		return 0;
	endif

endfunction   

function p_acidunequip (me)

	  EraseObjProperty(me , "a_equipped");
		return 0;

endfunction

function p_daemon (me)

	var powername := "DAEMON";
	var DAEMONTotal := ArmourCount(me, powername);

	if (DAEMONTotal == 5)
	  SetObjProperty(me , "a_equipped", powername);
          SendSysMessage(me, "Fear not the dangerous poisons of this realm!");
		return 2;
	else
	  EraseObjProperty(me , "a_equipped");
		return 0;
	endif

endfunction

function p_daemonunequip (me)

	  EraseObjProperty(me , "a_equipped");
		return 0;

endfunction

function p_copper (me)
	
	var powername := "COPPER";
	var COPPERTotal:= ArmourCount(me, powername);                          

	if (COPPERTotal == ArmorNeeded)
	  SetObjProperty(me , "a_equipped", powername);
          SendSysMessage(me, "You feel confident in your smithing arts!");
		return 2;
        else
	  EraseObjProperty(me , "a_equipped");
		return 0;
	endif
	
endfunction

function p_copperunequip (me)

	  EraseObjProperty(me , "a_equipped");
		return 0;

endfunction

function p_obsidean (me)

	var powername := "OBSIDEAN";
	var OBSIDEANTotal := ArmourCount(me, powername);

	if (OBSIDEANTotal == ArmorNeeded)
	  SetObjProperty(me , "a_equipped", powername);
          SendSysMessage(me, "You appear able to be able to conentrate on more spells.");
		return 2;
	else
	  EraseObjProperty(me , "a_equipped");
		return 0;
	endif

endfunction  

function p_obsideanunequip (me)

	  EraseObjProperty(me , "a_equipped");
		return 0;

endfunction

function p_titan (me)

	var powername := "TITAN";
	var TITANTotal:= ArmourCount(me, powername);

	if (TITANTotal == ArmorNeeded)
	  SetObjProperty(me , "a_equipped", powername);
          SendSysMessage(me, "Your intelligence and stamina will give you more life!");
		return 2;
	else
	  EraseObjProperty(me , "a_equipped");
		return 0;
	endif

endfunction 

function p_titanunequip (me)

	  EraseObjProperty(me , "a_equipped");
		return 0;

endfunction

Function ArmourCount(me, powername)

var myarmor := ListEquippedItems( me);
Var Counter := 0;
Var piece;

foreach item in myarmor

if (GetObjProperty(item, "power") = powername);
piece := item.objtype;

CASE (piece)
    0x13EC:     Counter := Counter + 1;
                break; 
    0X13F0:	Counter := Counter + 1;
                break;  
    0X13EE:	Counter := Counter + 1;
                break;
    0X13F2:	Counter := Counter + 1;
                break;
    0X13BF:	Counter := Counter + 1;
                break;
    0X13C3:	Counter := Counter + 1;
                break;
    0X13BB:	Counter := Counter + 1;
                break;
    0X140E:	Counter := Counter + 1;
                break;   
    0X1412:	Counter := Counter + 1;
                break; 
    0X140C:	Counter := Counter + 1;
                break; 
    0X140A:	Counter := Counter + 1;
                break;    
    0X1408:	Counter := Counter + 1;
                break;  
    0x1415:	Counter := Counter + 1;
                break;
    0x1411:	Counter := Counter + 1;
                break;  
    0x1413:	Counter := Counter + 1;
                break; 
    0x1410:	Counter := Counter + 1;
                break;
    0x1414:	Counter := Counter + 1;
                break;
    default:	Counter := Counter;
ENDCASE

endif
endforeach 

//SendSysMessage(me, "Counter: " + Counter + ".");

Return Counter;

Endfunction
