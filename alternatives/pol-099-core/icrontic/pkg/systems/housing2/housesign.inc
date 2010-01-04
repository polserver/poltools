var layout := array(

"page 0",

"resizepic 0 45 2600 412 300",
"gumppic 140 0 100",
"text 190 30 48 0",
"text 190 50 48 31",
"resizepic 15 100 5100 382 25",
"text 25 102 0 1",
"button 78 104 5209 5003 0 1 0",
"text 145 102 0 2",
"button 230 104 5209 5003 0 2 0",
"text 285 102 0 3",
"button 360 104 5209 5003 0 3 0",
"button 50 308 249 248 1 0 0",

"page 1",
"text 120 140 2302 4",
"text 200 140 0 5",
"text 35 180 2302 6",
"text 250 180 0 7",
"text 35 200 2302 8",
"text 250 200 0 9",
"text 35 240 2302 10",
"text 35 260 2302 11",
"text 270 260 0 12",
"text 35 280 2302 13",
"text 150 308 2302 14",
"button 330 308 2714 2715 1 0 1",

"page 2",
"text 55 130 2302 15",
"button 25 130 2714 2715 1 0 2",
"text 55 152 2302 16",
"button 25 152 2714 2715 1 0 3",
"text 55 174 2302 17",
"button 25 174 2714 2715 1 0 4",
"text 250 130 2302 18",
"button 220 130 2714 2715 1 0 5",
"text 250 152 2302 19",
"button 220 152 2714 2715 1 0 6",
"text 250 174 2302 20",
"button 220 174 2714 2715 1 0 7",
"text 250 196 2302 21",
"button 220 196 2714 2715 1 0 8",
"text 145 218 2302 22",
"button 115 218 2714 2715 1 0 9",
"text 145 240 2302 23",
"button 115 240 2714 2715 1 0 10",
"text 145 262 2302 24",
"button 115 262 2714 2715 1 0 11",
"text 145 284 2302 25",
"button 115 284 2714 2715 1 0 12",

"page 3",
"text 90 130 2302 26",
"button 60 130 2714 2715 1 0 13",
"text 90 160 2302 27",
"button 60 160 2714 2715 1 0 14",
"text 90 190 2302 28",
"button 60 190 2714 2715 1 0 15",
"text 90 220 2302 29",
"button 60 220 2714 2715 1 0 16",
"text 90 250 2302 30",
"button 60 250 2714 2715 1 0 17",
);

var data := array(
	"",
	"INFO",
	"FRIENDS",
	"OPTION",
	"Owned by",
	"<NAME>",
	"Number of locked down items:",
	"",
	"Number of secure containers:",
	"",
	"Number of visits this building has had: N/A",
	"",
	"",
	"",
	"Change this house's name!",
	"List of Co-owners",
	"Add a Co-owner",
	"Remove a Co-owner",
	"List of Friends",
	"Add a Friend",
	"Remove a Friend",
	"Clear Friends list",
	"Ban someone from the house",
	"Eject someone from the house",
	"View list of banned people",
	"Lift a ban",
	"Transfer ownership of the house",
	"Demolish the house & get a deed back",
	"Change the sign type",
	"Change the master key",
	"Building type( N/A )",
	" ",
);
var signG;

function RefreshHouseItems( house )

    var addonitem;
    SetObjProperty(house,"decay",(ReadGameClock()+ 2592000));
    foreach item in (house.components)
	if (!item.corpsetype)
            item.decayat := ReadGameClock() + 2592000;  //30 RL days in the future
	endif
    endforeach
   var totaladdons := CInt(GetObjProperty( house, "totaladdons"));

   addonitem := cint(GetObjProperty( house, "component1" ));
   if (addonitem)
     addonitem.decayat := ReadGameClock() + 2592000;
   endif
   addonitem := cint(GetObjProperty( house, "component2" ));
   if (addonitem)
     addonitem.decayat := ReadGameClock() + 2592000;
   endif
   addonitem := cint(GetObjProperty( house, "component3" ));
   if (addonitem)
     addonitem.decayat := ReadGameClock() + 2592000;
   endif

endfunction

function DemolishHouse(who, sign, house)
// Stop redeed if vendors in the house!
foreach npc in (house.mobiles)
  if(npc.npctemplate = "playervendor")
    SendSysMessage(who, "You cannot redeed a house with vendors in it.");
    return;
  endif
endforeach
// Find owner account and remove the house from his list
var addonitem;
var hserial := GetObjProperty( sign , "house_serial" );
var account := FindAccount (who.acctname);
if(account)
  var totalhouse := account.GetProp("HOUSE");
  account.SetProp("HOUSE", totalhouse - 1);
endif
// Give them back the deed
var deedtype := Getobjproperty(House, "DeedType");
CreateItemInBackpack( who, Deedtype, 1 );
// Find and remove all the components such as ladders
addonitem := cint(GetObjProperty( house, "component1" ));
if (addonitem)
  destroyitem(addonitem);
endif
addonitem := cint(GetObjProperty( house, "component2" ));
if (addonitem)
  destroyitem(addonitem);
endif
addonitem := cint(GetObjProperty( house, "component3" ));
if (addonitem)
  destroyitem(addonitem);
endif
if (house.objtype==24788)
  foreach item in ListItemsNearLocation( sign.x, sign.y, 0, 8 )
    if((item.objtype==0x177d) or (item.objtype==0xb41) or (item.objtype=0xb42))
      destroyitem(item);
    endif
  endforeach
endif

// Find and destroy the ban tiles
var tilelist := GetObjProperty(house, "bantiles");
foreach thing in tilelist
  DestroyItem(SystemFindObjectBySerial(thing));
endforeach

//Destroy the house
DestroyMulti( house );
//Destroy keys in thier pack
foreach item in enumerateitemsincontainer(who.backpack)
  sleepms(2);
  if((item.objtype == cint(UOBJ_GOLD_KEY)) and (cint(getobjproperty(item, "builtserial") == hserial)))
    destroyitem(item);
  endif
endforeach

// Check bank for the shit and remove it
var bankbox := Findbankbox(who);
Foreach item in enumerateitemsincontainer(bankbox)
  sleepms(2);
  if (((item.objtype == cint(UOBJ_GOLD_KEY)) and (cint(getobjproperty(item, "builtaserial")) == hserial))
      or( (item.objtype == cint(UOBJ_HOUSE_DEED1)) and (cint(getobjproperty(item, "builtserial")) == hserial)))
    moveitemtocontainer(item,who.backpack);
    destroyitem(item);
  endif
endforeach

endfunction

function ClearFriend( house, who )

    var i;
    for(i := 1; i <= 20; i := i + 1)
    EraseObjProperty(house , "friend" + i);
    endfor

endfunction

function AssignDefaultContainers( house )

    case (house.objtype)
       0x60AB: setobjproperty(house,"numlockdowns",25);//smallstoneandplasterhouse
               setobjproperty(house,"numsecure",1);
       0x60AD: setobjproperty(house,"numlockdowns",25);
               setobjproperty(house,"numsecure",1);
       0x60AF: setobjproperty(house,"numlockdowns",25);
               setobjproperty(house,"numsecure",1);
       0x60B1: setobjproperty(house,"numlockdowns",25);
               setobjproperty(house,"numsecure",1);
       0x60B3: setobjproperty(house,"numlockdowns",25);
               setobjproperty(house,"numsecure",1);
       0x60B5: setobjproperty(house,"numlockdowns",25);
               setobjproperty(house,"numsecure",1);
       0x60BB: setobjproperty(house,"numlockdowns",86);
               setobjproperty(house,"numsecure",3);
       0x60BD: setobjproperty(house,"numlockdowns",108);
               setobjproperty(house,"numsecure",5);
       0x60BF: setobjproperty(house,"numlockdowns",108);
               setobjproperty(house,"numsecure",5);
       0x60C1: setobjproperty(house,"numlockdowns",244);
               setobjproperty(house,"numsecure",12);
       0x60c3: setobjproperty(house,"numlockdowns",375);
               setobjproperty(house,"numsecure",19);
       0x60C5: setobjproperty(house,"numlockdowns",577);
               setobjproperty(house,"numsecure",29);
       0x60D8: setobjproperty(house,"numlockdowns",86);
               setobjproperty(house,"numsecure",4);
       0x60D4: setobjproperty(house,"numlockdowns",86);
               setobjproperty(house,"numsecure",4);
       0x60E4: setobjproperty(house,"numlockdowns",42);
               setobjproperty(house,"numsecure",3);
       0x60E5: setobjproperty(house,"numlockdowns",42);
               setobjproperty(house,"numsecure",3);
       0x60D6: setobjproperty(house,"numlockdowns",58);
               setobjproperty(house,"numsecure",4);
       0x60DA: setobjproperty(house,"numlockdowns",85);
               setobjproperty(house,"numsecure",6);
       0x60D7: setobjproperty(house,"numlockdowns",110);
               setobjproperty(house,"numsecure",8);
       0x60DB: setobjproperty(house,"numlockdowns",110);
               setobjproperty(house,"numsecure",8);
       0x60D5: setobjproperty(house,"numlockdowns",137);
               setobjproperty(house,"numsecure",10);
       0x60DC: setobjproperty(house,"numlockdowns",15);
               setobjproperty(house,"numsecure",1);
    endcase
endfunction


// Finds the owner of the house and adjusts thier house limit amount
function FindOwner(who, sign)

  var hserial := GetObjProperty( sign , "house_serial" );
  var account := FindAccount (who.acctname);

  if(account)
    var totalhouse := account.GetProp("HOUSE");
    account.SetProp("HOUSE", totalhouse - 1);
  endif

  return;
  
endfunction

function TestDemolishforNewArray(house, sign, who)
  // Stop redeed if vendors in the house!
  foreach npc in (house.mobiles)
    if(npc.npctemplate = "playervendor")
      SendSysMessage(who, "You cannot redeed a house with vendors in it.");
      return;
    endif
  endforeach

  // Find owner account and remove the house from his list
  var addonitem;
  FindOwner(who, sign);

  // Give them back the deed
  var deedtype := Getobjproperty(House, "DeedType");
  CreateItemInBackpack( who, Deedtype, 1 );

  // Remove addons and Ban Tiles
  RemoveAddons(house, sign);
  var tilelist := GetObjProperty(house, "bantiles");
  foreach thing in tilelist
    DestroyItem(SystemFindObjectBySerial(thing));
  endforeach

  // Setup for the box array of house
  var lists := GetObjProperty(house, "footage");
  var itemlist := {};
  var holder, chk := 0;
  foreach thing in lists
    holder := ListObjectsInBox(thing[1], thing[2], thing[3], thing[4], thing[5],thing[6]);
    foreach thing in holder
      itemlist.append(thing);
    endforeach
  endforeach

  // Check for a guildstone
  foreach thing in itemlist
    if(thing.objtype == 0xa392)
      SendSysMessage(who, "You cannot redeed a house while it has a guildstone.");
      return;
    endif
  endforeach
  
  // Check itemlist for secures and locked down items to get rid of them
  foreach thing in itemlist
    if((GetObjProperty(thing,"secure") == 1) && (thing.movable == 0))
      DestroyItem(thing);
    elseif ((GetObjProperty(thing, "lockeddown" ) == 1) && (thing.movable == 0))
      thing.movable := 1;
      EraseObjProperty(thing, "houseserial" );
      EraseObjProperty(thing, "lockeddown");
      thing.decayat := ReadGameClock() + 10;
    elseif (thing.movable == 0)
      DestroyItem(thing);
    endif
  endforeach

  // Destroy House
  DestroyMulti(house);

  //Destroy keys in thier pack
  foreach item in enumerateitemsincontainer(who.backpack)
    sleepms(2);
    if((item.objtype == cint(UOBJ_GOLD_KEY)) and (cint(getobjproperty(item, "builtserial") == hserial)))
      destroyitem(item);
    endif
  endforeach

  // Check bank for the shit and remove it
  var bankbox := Findbankbox(who);
  Foreach item in enumerateitemsincontainer(bankbox)
    sleepms(2);
    if (((item.objtype == cint(UOBJ_GOLD_KEY)) and (cint(getobjproperty(item, "builtaserial")) == hserial))
        or( (item.objtype == cint(UOBJ_HOUSE_DEED1)) and (cint(getobjproperty(item, "builtserial")) == hserial)))
      moveitemtocontainer(item,who.backpack);
      destroyitem(item);
    endif
  endforeach

endfunction

// Find and remove all the components such as ladders
function RemoveAddons(house, sign)
var addonitem;
addonitem := cint(GetObjProperty( house, "component1" ));
if (addonitem)
  destroyitem(addonitem);
endif
addonitem := cint(GetObjProperty( house, "component2" ));
if (addonitem)
  destroyitem(addonitem);
endif
addonitem := cint(GetObjProperty( house, "component3" ));
if (addonitem)
  destroyitem(addonitem);
endif
if (house.objtype==24788)
  foreach item in ListItemsNearLocation( sign.x, sign.y, 0, 8 )
    if((item.objtype==0x177d) or (item.objtype==0xb41) or (item.objtype=0xb42))
      destroyitem(item);
    endif
  endforeach
endif

return;

endfunction

