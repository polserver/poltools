											/*
								     	    readme.txt  //
								        Package guilds	//
						     		  Newest guild package	//
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





  Hey everybody,

  I'm releasing this package because I was threatened that someone else would release it
under their name. I did not want to release this package that took a month to perfect,
but since it was some of my best work, I only want to have my name on it, not some lamer
who thinks that I'm a bad scripter, yet threatens me with my own packages. I hope everybody
has more decency and respectability than the person who made me do this.

  This package was scripted under POL 094 Core release, with the help previous guild folders
created/modified by Zulu/Bishop Ebonhand, (kudoes to them, it was very useful in making this
package). To cut to the chase, here's a list of what you have to do to use this package.

  The special features to this package includes: different guild types (Newbie, Merchant,
Warrior). Warrior guilds may war any other warrior guilds at any time, while Merchant and
Newbie stay the normal, where both must agree to war, a nice layout, a webpage to display
the guilds as well as a few statistics, guild chat, guild clothing, and many old features
made better.

  - Move this package preferably to pol\pkg\items\guilds, for this is where I worked with
      it, and it is probably best to leave it there.
  - Inside the "www" folder, there is the website script. If you wish to use this, move it
      to pol\scripts\misc\www\. From there, you may access this webpage through
      http://[yourip]:9000/guilds.ecl, if your webserver port is 9000 and if it is turned
      on itself.
  - A few customizable options for are in the following files:
    - guildStone: COLOUR_START and COLOUR_END are currently best for Neverlands. I will ask
        you not to use our hues, and to use your own hues, as it is easy to change the range
        of colours. Thank you.
    - guildDeed: COLOUR_START is required if you are changing the colour ranges, since the
        default guild colour is the very first colour in the range.
    - websiteTimers: DELAY_BETWEEN_REFRESH is the time in seconds between the guild page is
        updated. I recommend between 10 and 30 minutes, as it is a long number of loops on
        the larger shards. However, I do have many pauses, so do not be worried. ON lets you
        easily change between on and off without deleting the file if you don't use the
        webpage.
  - Do not change the package name, unless you are willing to go through the files and
      change each entry.
  - As mentioned above, please do not use our hues file.
  - When everything is set and done, use the eCompiler and compile this entire package, as
      well as the website file if being used.
  - I would recommend looking into your .info command and recompiling it with the guild
      resignation updated, so that it does it correctly, rather than the old method.

  It is recommended that the current guilds (if you have any) be destroyed and resupplied
with guild deeds when changing packages, as the systems require many upgrades. Maybe one
day I will make a converter of the old guilds to the new ones. However, you may try to
keep the old guilds, just tell me what happens, if you do in fact try.

  Also recommended to add the following code to your equip script for all tailored items. It
makes sure that someone from a different guild or anything doesn't wear the clothing. Add
this near the top:


	//Devious) Guildclothes

  var guildabv:= Splitwords(item.name);
  guildabv:= guildabv[len(guildabv)];

  var abv, personserial;

  if((guildabv[1] = "[") and (guildabv[len(guildabv)] = "]") and (len(guildabv) < 7))
    personserial:= GetObjProperty(who,"guild_clothing");
    if(personserial != item.serial)
      SendSysMessage(who,"That is not your Guild clothing!");
      Start_Script(":guildstone:destroyitem",item);
      return;
    else
      SendSysMessage(who,"That is your Guild clothing!");
    endif
  endif


  The only known bug in this is package is concerning recruiting people into the guilds.
sometimes, it will say "This person is in a guild." when that person is not. To fix this,
look through the person trying to be recruited for a "guild_candidate" or "guild_sponser"
property, and erase it. This is a rare bug, but that is how it is fixed.

  I have a few ideas of how to make this package better. Currently there is a "Faction"
part to the guilds, which in fact is only a name, "Chaos", "Order", and "Peace". I wish
to expand upon this idea and make it truely Factioned, in my style, and not in OSI's.

  I took out the vote leader and set fealty, because usually the person who owns the house
where the guildstone is is actually the undisputed owner of the guild. This was quite
useless on our shard, and I think it would be for yours as well.

  Please enjoy this, give credit where it is due (to my shard and myself), and I hope this
package that I am most proud of helps optimize your shard. Email me to share your thoughts,
comments, or bugs.


      -Fin-
