include "vetement";

var speech;
var ammotype;
var ammoamount;
var theammo;

var master;
var spells := {};
var me := self();

var cast_pct;
var num_casts;
var count_casts ;
var saywords := 1;
var summons := 3;

var graphics;
var num_changes;
var will_cast;
var will_breathe;
var flee_point;

	drop_anchor();
var equiptm := GetObjProperty( me, "Equipt");
var npccfg := readconfigfile("npcdesc");
var speechelem := npccfg[me.npctemplate];
speech := getconfigstring(speechelem, "speech") ;
spells := GetConfigStringArray( speechelem, "spell" );
cast_pct := speechelem.cast_pct;
num_casts  := speechelem.num_casts;
count_casts  := speechelem.count_casts;
var equipname := speechelem.equip;
saywords := speechelem.saywords;
if (!cast_pct)
        cast_pct := 10;
endif
flee_point := speechelem.flee_point;
if (!flee_point)
        flee_point := 10;
endif
var mybow := npccfg[me.npctemplate].missileweapon;
if (!me.backpack)
	var newbackpack := CreateItemAtLocation(me.x, me.y, me.z, 0xe75,1);
	EquipItem(me, newbackpack);
	if (npccfg[me.npctemplate].colorrange)
		var parms := splitwords(npccfg[me.npctemplate].colorrange);
		me.color := randomint(cint(parms[2]) - cint(parms[1]))+cint(parms[1]);
	endif
	if (npccfg[me.npctemplate].dress)
		var parms := {};
		parms[1]:= me;
		parms[2]:= npccfg[me.npctemplate].dress;
		start_script("::/misc/dressme", parms);
	elseif (npccfg[me.npctemplate].equip)
    		EquipFromTemplate(me, npccfg[me.npctemplate].equip);
	elseif (equipname)
        	equipfromtemplate(me, equipname);
	elseif (npccfg[me.npctemplate].ammotype)
		ammotype := npccfg[me.npctemplate].ammotype;
		if (ammotype)
        		if (me.backpack)
                		destroyitem(me.backpack);
        		endif
        		equipfromtemplate(me, mybow);
        		ammoamount := npccfg[me.npctemplate].ammoamount;
			if (!ammoamount)
				ammoamount := 30;
			endif
			theammo := createiteminbackpack(me, ammotype, ammoamount);
		endif
	endif
endif
if( me.name["<random>"] )
	if ((me.graphic = 400) or (me.graphic = 401))
		me.graphic := 400 + RandomInt(2);
	endif
	if (npccfg[me.npctemplate].title)
		var title := randomint(20);
		var ttitle := "";

		case (title) 
          	0: ttitle := "";
          	1: ttitle := " the townsperson";
          	2: ttitle := " the farmer";
          	3: ttitle := " the sailor";
          	4: ttitle := " the drunkard";
          	5: ttitle := " the wanderer";
          	6: ttitle := " the villager";
          	7: ttitle := " the gypsie";
          	8: ttitle := " the vagabond";
          	9: ttitle := " the laborer";
        	  default: ttitle := "";
		endcase

		var tname := randomname(me);
		setname(me, tname + ttitle);
	else
		SetName ( me , RandomName( me ) );
	endif
	if( equiptm)
	   set_priority(50);
       Habille( me , equiptm );
       set_priority(1);
    endif
endif
