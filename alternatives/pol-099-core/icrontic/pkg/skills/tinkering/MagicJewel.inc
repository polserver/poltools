var asType := Array(
"alchemy",
"anatomy",
"animallore",
"animaltaming",
"archery",
"armslore",
"begging",
"blacksmithy",
"bowcraft",
"camping",
"carpentry",
"cartography",
"cooking",
"detectinghidden",
"enticement",
"evaluatingintelligence",
"fencing",
"fishing",
"forensicevaluation",
"healing",
"herding",
"hiding",
"inscription",
"itemid",
"lockpicking",
"lumberjacking",
"macefighting",
"magery",
"magicresistance",
"meditation",
"mining",
"musicianship",
"parry",
"peacemaking",
"poisoning",
"provocation",
"removetrap",
"snooping",
"spiritspeak",
"stealing",
"stealth",
"swordsmanship",
"tactics",
"tailoring",
"tasteidentification",
"tinkering",
"tracking",
"veterinary",
"wrestling",
"dexterity",
"intelligence",
"strength");
//"Life",
//"Mana",
//"Stamina");

function makeMagicJewel(item, who) //MuadDib Changed
	var numb := RandomInt(100)+1;
	var skillid := RandomInt(Len(asType))+1;
	var currpref;
	var pref;
	var pricemod := 0;
	var suffix;
	var sSkillId;
	case( skillid )
		1:  suffix:= "of Alchemy";
		    break;
		2:  suffix:= "of Anatomy";
		    break;
		3:  suffix:= "of Animallore";
		    break;
		4:  suffix:= "of Animal Taming";
		    break;
		5:  suffix:= "of Archery";
		    break;
		6:  suffix:= "of Armslore";
		    break;
		7:  suffix:= "of Begging";
		    break;
		8:  suffix:= "of Blacksmithy";
		    break;
		9:  suffix:= "of Bowcraft";
		    break;
		10: suffix:= "of Camping";
		    break;
		11: suffix:= "of Carpentry";
		    break;
		12: suffix:= "of Cartography";
		    break;
		13: suffix:= "of Cooking";
		    break;
		14: suffix:= "of Detecting Hidden";
		    break;
		15: suffix:= "of Enticement";
		    break;
		16: suffix:= "of Evaluating Intelligence";
		    break;
		17: suffix:= "of Fencing";
		    break;
		18: suffix:= "of Fishing";
		    break;
		19: suffix:= "of Forensic Evaluation"; // MuadDib Changed
		    break;
		20: suffix:= "of Healing";
		    break;
		21: suffix:= "of Herding";
		    break;
		22: suffix:= "of Hiding";
		    break;
		23: suffix:= "of Inscription";
		    break;
		24: suffix:= "of Itemid";
		    break;
		25: suffix:= "of Lockpicking";
		    break;
		26: suffix:= "of Lumberjacking";
		    break;
		27: suffix:= "of Macefighting";
		    break;
		28: suffix:= "of Magery";
		    break;
		29: suffix:= "of Magic Resistance";
		    break;
		30: suffix:= "of Meditation";
		    break;
		31: suffix:= "of Mining";
		    break;
		32: suffix:= "of Musicianship";
		    break;
		33: suffix:= "of Parry";
		    break;
		34: suffix:= "of Peacemaking";
		    break;
		35: suffix:= "of Poisoning";
		    break;
		36: suffix:= "of Provocation";
		    break;
		37: suffix:= "of Removetrap";
		    break;
		38: suffix:= "of Snooping";
		    break;
		39: suffix:= "of SpiritSpeak";
		    break;
		40: suffix:= "of Stealing";
		    break;
		41: suffix:= "of Stealth";
		    break;
		42: suffix:= "of Swordsmanship";
		    break;
		43: suffix:= "of Tactics";
		    break;
		44: suffix:= "of Tailoring";
		    break;
		45: suffix:= "of Taste Identification";
		    break;
		46: suffix:= "of Tinkering";  // MuadDib Changed
		    break;
		47: suffix:= "of Tracking";
		    break;
		48: suffix:= "of Veterinary";
		    break;
		49: suffix:= "of Wrestling";
		    break;
		//"dexterity";
		//"intelligence";
		//"strength";
		50: suffix:= "of Dexterity";
		    break;
		51: suffix:= "of Intelligence";
		    break;
		52: suffix:= "of Strength";
			break;
	endcase
	if(( numb >= 50 ) && GetEffectiveSkill(who, SKILLID_TINKERING) >= 100)
		if (skillid > 49)
			SetObjProperty( item , "skill_adj" , 5 );
		else
			SetObjProperty( item , "skill_adj" , 5 );
		endif
		pref := "Lesser";
		pricemod := 300;
		item.color:= 0x8040;
	elseif(( numb >= 25 ) && GetEffectiveSkill(who, SKILLID_TINKERING) >= 105)
		if (skillid > 49)
			SetObjProperty( item , "skill_adj" , 10 );
		else
			SetObjProperty( item , "skill_adj" , 10 );
		endif
		pref := "Good";
		pricemod := 500;
		item.color:= 0x8070;
	elseif(( numb >= 15 ) && GetEffectiveSkill(who, SKILLID_TINKERING) >= 110)
		if (skillid > 49)
			SetObjProperty( item , "skill_adj" , 15 );
		else
			SetObjProperty( item , "skill_adj" , 15 );
		endif
		pref := "Greater";
		pricemod := 900;
		item.color:= 0x8003;
	elseif(( numb >= 5 ) && GetEffectiveSkill(who, SKILLID_TINKERING) >= 115)
		if (skillid > 49)
			SetObjProperty( item , "skill_adj" , 20 );
		else
			SetObjProperty( item , "skill_adj" , 20 );
		endif
		pref := "Glorious";
		pricemod := 1300;
		item.color:= 0x8021;
	elseif(( numb >= 1 ) && GetEffectiveSkill(who, SKILLID_TINKERING) >= 120)
		if (skillid > 49)
			SetObjProperty( item , "skill_adj" , 25 );
		else
			SetObjProperty( item , "skill_adj" , 25 );
		endif
		pref := "Godly";
		pricemod := 2000;
		item.color:= 1965;
	endif
        if(pref != error)
	  SetObjProperty( item , "suffix", suffix );
	  if (skillid >= 49)
		SetObjProperty( item , "skill" , asType[skillid]);
          elseif(skillid == 50)
                SetObjProperty( item , "skill" , "dexterity");
          elseif(skillid == 51)
                SetObjProperty( item , "skill" , "intelligence");
          elseif(skillid == 52)
                SetObjProperty( item , "skill" , "strength");
          else
                SetObjProperty( item , "skill" , GetSkillIdByAttributeId(asType[skillid]));
	  endif
          if(pref == "Lesser")
            SetObjProperty(who, "nextmagic", Readgameclock() + 1800);
            SendSysMessage(who, "It will be 30 minutes before you can make any more magical jewelry", 3, 33);
          elseif(pref == "Good")
            SetObjProperty(who, "nextmagic", Readgameclock() + 3600);
            SendSysMessage(who, "It will be 1 hour before you can make any more magical jewelry", 3, 33);
          elseif(pref == "Greater")
            SetObjProperty(who, "nextmagic", Readgameclock() + 5400);
            SendSysMessage(who, "It will be 1.5 hours before you can make any more magical jewelry", 3, 33);
          elseif(pref == "Glorious")
            SetObjProperty(who, "nextmagic", Readgameclock() + 7200);
            SendSysMessage(who, "It will be 2 hours before you can make any more magical jewelry", 3, 33);
          elseif(pref == "Godly")
            SetObjProperty(who, "nextmagic", Readgameclock() + 10800);
            SendSysMessage(who, "It will be 3 hour before you can make any more magical jewelry", 3, 33);
          endif
        endif
	currpref := GetObjProperty(item,"prefix");
	if(currpref != error)
		currpref := currpref + ", " + pref;
		SetObjProperty(item,"prefix",currpref);
	else
		SetObjProperty(item,"prefix",pref);
	endif
	item.buyprice := item.buyprice + pricemod;
endfunction

function AddName( item )

   var currdesc := item.desc;
   if( currdesc[1] = "a" )
      var space := find( item.desc, " ", 1 );
      var newname := currdesc[space+1,len(item.desc)];
      SetName( item , "a magic " + newname );
   else
      SetName( item , "magic " + currdesc );
   endif
endfunction
