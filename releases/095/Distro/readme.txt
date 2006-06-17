Penultima Online (POL)
Copyright (C) 1993-2003 Eric N. Swanson
Do not distribute this package

THIS PACKAGE IS PROVIDED "AS IS" AND WITHOUT ANY EXPRESS OR 
IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED 
WARRANTIES OF MERCHANTIBILITY AND FITNESS FOR A PARTICULAR PURPOSE.

NOTE:
=====
Make SURE the utility you are using to unzip POL supports long filenames (like WINZIP)!
If any of the filenames are truncated the server will now start.

QUICK START:
============
 SERVER
 ------
  In POL.CFG, Edit the UoDatafileRoot= line to match your UO directory.

  in pkg\foundations\hooks\uoclient.cfg, Edit the MaxSkillID to 51 IF all of your
   users will be running the AoS client, or you are using a custom skill gump. Having
   a MaxSkillID of 51 will crash clients that expect there to be only 49 skills!
   (If you aren't sure, leave it be. You can always change it later.)

  in CONFIG\SERVERS.CFG, Edit the first entry's IP address to match your own.
  (Most don't need to do this, as POL detects your IP automatically)

  You may want to edit DATA\ACCOUNTS.TXT to create accounts

  To compile, modify scripts\ecompile.cfg to point to the correct paths.

  From this directory, run POL.EXE

 CLIENT
 ------    
  in your UO\LOGIN.CFG, Change all LoginServer entries to match your 
    IP address, Port 5003.

  Run CLIENT.EXE    

Please report any bugs you find to pol-distro@yahoogroups.com.

If you are running Linux kernal version 2.4.*, you will need to turn
multithreading off in pol.cfg.

IF YOU ARE RUNNING THE 094 DISTRO:

An Admin account exists by default. Username/Password: admin/admin

To figure out why your new merchants aren't responding to your words, or want
to learn how to set up new merchants or guards, look at docs/nodes.txt.

If your users can't create characters, look in the client directory and read
the readme.txt you find there.