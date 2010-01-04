											/*
								      	     guild.inc	//
								        Package guilds	//
								       var commands	//
											//
//////////////////////////////////////////////////////////////////////////////////////////

                      ////    ////// //   //  //  //////  //  //  //////
                     //  //  //      //  //  //  //  //  //  //  //
                    //  //  ////     // //  //  //  //  //  //  //////
                   //  //  //        ////  //  //  //  //  //      //
                  /////   //////     ///  //  //////  //////  //////

//////////////////////////////////////////////////////////////////////////////////////////
//
//  This file was created by Developer Devious of Neverlands Shard
//  http://www.neverlands.org/
//  devtempo@telusplanet.net
//  Released under extraordinary circumstances, never trust Ego-Crusher, Icrontic, Kain,
//    "Shai`tain", ever, for he was the reason these files were released, beyond my will.
//  I hope everybody enjoys these scripts, and give credit where it is due, unlike the
//    backstabbing bastard above who claimed my scripts and nearly released them before
//    I did.
*/




function GuildResign(who,stone:= 0)

  var guild_id:= GetObjProperty(who,"guild_id");
  var guild:= FindGuild(guild_id);
  var gm:= FindGM(guild);

  if(who.serial = gm[2])
    if(stone)
      Start_Script("guildDestroy",stone);
    else
      guild.setprop("gm",0);
    endif
  endif
  
  guild.removemember(who);
  EraseObjProperty(who,"guild_title");
  EraseObjProperty(who,"guild_id");
  EraseObjProperty(who,"guild_fealty");
  EraseObjProperty(who,"guild_abv");
  who.title_guild:= "";

  var all:= guild.members;
  var i;

  if(all.size() = 0)
    all:= guild.allyguilds;
    if(all.size() != 0)
      for(i:= 1; i <= all.size(); i:= i + 1)
        guild.removeallyguild(all[i]);
      endfor
    endif
    all:= guild.enemyguilds;
    if(all.size() != 0)
      for(i:= 1; i <= all.size(); i:= i + 1)
        guild.removeenemyguild(all[i]);
      endfor
    endif
    DestroyGuild(guild);
    if(stone)
      DestroyItem(stone);
      CreateItemInBackpack(who,0xa391,1);
    endif
  endif

  var clothing:= GetObjProperty(who,"guild_clothing");
  clothing:= SystemFindObjectBySerial(clothing);
  if(!DestroyItem(clothing))
    Start_Script("destroyitem",clothing);
  endif
  EraseObjProperty(who,"guild_clothing");

endfunction


function FindGM(guild)

  var returninfo:= {

	"is Currently in Vote",
	0,

  };
  
 
  var i;
  var all:= guild.members;
  var gm:= guild.getprop("gm");

  for(i:= 1; i <= all.size(); i:= i + 1)
    if(all[i].serial = gm)
      returninfo[1]:= all[i].name;
      returninfo[2]:= all[i].serial;
      returninfo[3]:= all[i];
    endif
  endfor

  return returninfo;

endfunction
