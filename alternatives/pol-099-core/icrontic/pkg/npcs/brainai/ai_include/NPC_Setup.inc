// 1.3
//
// NPC_Setup.inc
//
// Functions for NPC creation (naming, equipping, etc)
//
// Created: 07/09/2001 15:42
// Updated: 7/19/2002 2:34PM
//
// By Austin Heilman and TJ Houstin
// http://www.tsse.net
//
/////////////////////////////////////////////
//
// Functions you can use in an init_script or
// that in some instances the brain will call.
//
// These are functions that you run when an npc
// is created to set special things up on it.
//
/////////////////////////////////////////////

use uo;
use os;
use util;
use cfgfile;

include "include/vetement";
include "include/startEqp";

//  SetNPCName()
//  Pass a mobile, and an optional name_elem. If no name_elem
//  is defined, it will use the NameList setting for that npc in the NPCDESC.CFG
//  Any instances of <random> in the mobiles name will be replaced with something
//  else based on what part of NAMES.CFG it reads from.
//  Returns error (with errortext) on failure, 1 on success.
function SetNPCName(mobile, name_elem := 0)
	
	if ( !mobile.name["<random>"] )
		return 1;
	elseif ( !name_elem )
		var npc_config := ReadConfigFile("::npcdesc");
		name_elem := npc_config[mobile.npctemplate].NamesList;
		
	endif
		
	var npc_names := ReadConfigFile("::names2");
	if ( !npc_names[name_elem] )
		var temp := error;
		temp.+errortext := "There is no elem ["+name_elem+"] in NAMES.CFG";
		
		PrintTextAbove(mobile, temp.errortext);
		return temp;
	endif

	var name_list := GetConfigStringArray(npc_names[name_elem], "Name");
	if ( name_list.size() < 1 )
		var temp := error;
		temp.+errortext := "No names were found in the ["+name_elem+"] elem for NAMES2.CFG";
		
		PrintTextAbove(mobile, temp.errortext);
		return temp;
	endif
	
	var my_name := mobile.name;
	while ( my_name["<random>"] )
		var replacement := name_list[RandomInt(name_list.size())+1];
		my_name["<random>"] := replacement;
	endwhile
	
	SetName(mobile, my_name);
	
	return 1;
endfunction

//  SetNPCEquipment()
//  Pass a mobile, and an optional equip_elem. If no equip_elem
//  is defined, it will use the Equip setting for that npc in the NPCDESC.CFG
//  it will equip the npc based on the equipment template that equip_elem is set to
//  Returns error (with errortext) on failure, 1 on success.
function SetNPCEquipment(mobile, equip_elem := 0)
var pncfg;
var equipt;
var who := mobile;
    pncfg := ReadConfigFile("::npcdesc");
    equipt := GetObjProperty(mobile, "Equipt");

    if(equipt)

      if(who.graphic == 0x190)
        case(randomint(2))
          0: who.graphic := 0x190;
             who.gender := 0;
             equipt := equipt + "M";
          1: who.graphic := 0x191;
             who.gender := 1;
             equipt := equipt + "F";
        endcase
      elseif(who.graphic == 0x191)
        case(randomint(2))
          0: who.graphic := 0x190;
             who.gender := 0;
             equipt := equipt + "M";
          1: who.graphic := 0x191;
             who.gender := 1;
             equipt := equipt + "F";
        endcase
      endif
      SetObjProperty(mobile, "Graphic", cint(mobile.graphic));

      Habille(mobile, equipt);
      EraseObjProperty(mobile, "Equipt");
    else
	var equip_cfg := ReadConfigFile("::equip2");

	if ( !equip_elem )
		var npc_cfg := ReadConfigFile("::npcdesc");
		var npc_info := npc_cfg[mobile.npctemplate];
		equip_elem := npc_info.Equipt;
	endif
	
	if ( !equip_elem )
		var backpack := CreateItemAtLocation(mobile.x, mobile.y, mobile.z, "Backpack", 1);
		if ( !EquipItem(mobile, backpack) )
			DestroyItem(backpack);
		endif
		return 1;
	elseif ( !equip_cfg )
		var temp := error;
		temp.+errortext := "Could not open EQUIP2.CFG";
		
		PrintTextAbove(mobile, temp.errortext);
		return temp;
	elseif ( !equip_cfg[equip_elem] )
		var temp := error;
		temp.+errortext := "There is no elem ["+equip_elem+"] in EQUIP2.CFG";
		
		PrintTextAbove(mobile, temp.errortext);
		return temp;
	else
		EquipFromTemplate(mobile, equip_elem);
	endif
    endif

    var lewtgroup := pncfg[mobile.npctemplate].lootgroup;
    if(lewtgroup)
      MakeLoot(mobile, cint(lewtgroup));
    endif
	
    if ( !mobile.backpack )
	EquipItem(mobile, CreateItemAtLocation(mobile.x, mobile.y, mobile.z, "backpack", 1));
    endif
	
    return 1;
endfunction

//  SetupAmmo()
//  Pass a mobile. It will create the 'AmmoType' specified in its
//  NPCDESC.CFG entry and will make 'AmmoAmount' amount of it.
//  Ammo created is newbified.
//  Returns error on failure, 1 on success
function SetupAmmo(mobile)
	var npc_cfg := ReadConfigFile("::npcdesc");
	if ( !npc_cfg )
		var temp := error;
		temp.+errortext := "Could not open npcdesc.cfg";
		return temp;
	endif
	var npc_info := npc_cfg[mobile.npctemplate];
	if ( !npc_info )
		var temp := error;
		temp.+errortext := "Could not open npcdesc.cfg["+mobile.npctemplate+"]";
		return temp;
	endif
	
	if ( npc_info.MissileWeapon )
		var weapon := CreateItemAtLocation(mobile.x, mobile.y, mobile.z, npc_info.MissileWeapon, 1);
//		weapon.newbie := 1;
		if ( !EquipItem(mobile, weapon) )
			DestroyItem(weapon);
		endif
	endif

        var TypeOfAmmo := npc_info.AmmoType;

        case (mobile.weapon.objtype)
          0x13b2 :
          0x9889 : TypeOfAmmo := "arrow";
          0xf4f  :
          0x13fd : TypeOfAmmo := "bolt";
        endcase

	if ( TypeOfAmmo )
		if ( npc_info.AmmoAmount )
			var ammo := CreateItemInBackpack(mobile, TypeOfAmmo, npc_info.AmmoAmount);
			if ( !ammo )
				var temp := error;
				temp.+errortext := "The ammo was not created.";
				return temp;
			endif
			
//			ammo.newbie := 1;
			return 1;
		else
			var temp := error;
			temp.+errortext := "I have been told to make AmmoType ["+TypeOfAmmo+"] but was not given AmmoAmount.";
		
			PrintTextAbove(mobile, temp.errortext);
			return temp;
		endif
	else
		var temp := error;
		temp.+errortext := "No AmmoType was specified.";
		
		PrintTextAbove(mobile, temp.errortext);
		return temp;
	endif
endfunction

//  RandomEquip()
//  Pass a mobile, and an optional equip_elem. If no equip_elem
//  is defined, it will use the Equip setting for that npc in the NPCDESC.CFG
//  when finished, it will equip an npc with random clothing that actually looks good!
//  Returns error (with errortext) on failure, 1 on success.
function RandomEquip(mobile, equip_elem := 0)
	// To be completed some time this century.
endfunction

//  SetNPCProps(mobile)
//  This is run by the brain when an npc is created.
//  It stores the template, serial and AI pid
//  Returns a struct 'temp' 
function SetNPCProps(mobile)
	var temp		:= struct;
	temp.+npctemplate	:= mobile.npctemplate;
	temp.+npcserial		:= mobile.serial;
	temp.+AI_PID		:= GetPid();
	SetObjProperty(mobile, "npc_info", temp);
	
	//This is for compatability with bishop's current spawner.
	SetObjProperty(mobile, "AiSerial", CInt(mobile.serial));
	
	return temp;
endfunction
