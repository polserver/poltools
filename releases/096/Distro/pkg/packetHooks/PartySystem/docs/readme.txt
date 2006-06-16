[ - What is this? -]
An OSI-like party system. This means it uses the client's party manifest gump and can handle all client party commands (/add, /rem, /quit, etc...). It also handles status updates so you see the stamina and mana of other party members.


[ - Changes - ]
1.1:
  Added support for /# private messanging
    # is position in the party of the player you want to message, ex: "/1 hello" will send "hello" to the party leader
  Added support for the /add client command
  Removed SendStatus hook, it is not really necessary, requires the attributes package and conflicts with another package in the 096 distro
  Fixed a recursion bug (the one Pierce mentions)
  Fixed lots of little small bugs
  Removed the dependency on the new attributes package
  Party messages now use specific party packets (instead of SendSysMessageUC())
  Tried to improve some of the comments...
  Removed my buggy and incomplete onCorpseRemove, since the 096 distro will handle this somewhere else

1.0:
  Crappy initial release :)
  

[ - Installation - ]
Copy the files over to your packages directory and recompile.

If you have corpse looting criminality checks then add some code to check if the owner of the corpse has PARTY_LOOT_PROP set to true and the person looting is in the party. For refrence, on OSI, you can loot a corpse if it is:
 - your own corpse
 - your agressor's corpse (someone who attacks you and you kill them)
 - a criminal/murderer's
 - fellow/rival guild member

By default this package uses the party message packets (subsub commands 3 and 4) to send messages to other party members. If they have PartyMessageColor=0 in uo.cfg then they will not be able to read the messages. You can fix this by telling all of your players to set it to PartyMessageColor=368 or you can open config.inc in the include directory and change USE_PACKETED_MESSAGES to 0. This will substitute the packets for SendSysMessageUC(<message>, 3, 368).


[ - Considerations - ]
Thanks to Max Sherr for his initial help getting this moving, to Kinetix who posted some code on Folko's old forums, to Aeros, grak and Pierce who posted on the forums when they found something wrong and a massive thanks to all of the POL devs, especially Austin for answering all of my questions.


[ - TODO - ]
Add support for party chat window (I think this is an AoS+ feature)
Add support for factions (currently factions are not even implemented)
	Here is what OSI says about factions and parties:
	"The party cannot have members from opposing factions."
	"You have been removed from your party due to factional conflict."
	"You cannot have players from opposing factions in the same party!"
I still do not know exactly what messages OSI displays and how they look


[ - Note - ]
While coding this I came accross two undocumented subsub commands for the party system: 5 and 7. From what I can tell, 5 works exactly like 4, which is send party message. Subsub command 7 is an "invite" command, that I might not be using correctly. If anyone could tell me the precise messages OSI sends and when it sends them for the party system, or perhaps a UOLOG (my wildest dream) of a party session, I would be very greatful and could make this package more OSI-like. The below information is my best guess, but it may be wrong.

Command 0xBF, subcommand 6:
	Subsubcommand 5: Tell full party a message? (Variable # of bytes) 
	· BYTE[4] id (of source) 
	· BYTE[n][2] Null terminated Unicode message. 
	Note: Server Message
	
	Subsubcommand 7: Send party invitation (4 bytes)
	· BYTE[4] id (of source) 
	Note: Server Message


[ - Contact - ]
Please send me any comments, questions or BUGS.
E-mail: tekproxy@gmail.com
AIM: tekproxy