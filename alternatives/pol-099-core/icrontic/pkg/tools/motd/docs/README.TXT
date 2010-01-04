==============
= README.TXT =
===================================================
Package: motd
Version: 2.0
CoreReq: 088
Creator: TJ Houston (Myrathi)
Contact: tjh@Myrathi.co.uk
Created: 24-May-2000
Updated: 23-Jan-2002

Updated: 12-Jan-2003 by Jouni Airaksinen (Mintus)
===================================================
-
-
===================
= SECTION CONTENT =
===================================================
+ File Overview
+ General Information
+ I'm stuck! How do I install it?
+ I've Done What You Said But It Doesn't Work!
+ What Does It Do?
+ HTML Content
+ The Text In The Tip Window Won't Align Properly!
+ I Want To Disable The Login/Reconnect Tip Windows
+ Contact Information
===================================================
-
-
=================
= File Overview =
=================

Files included in this archive (excluding .ECL's):

* /motd/docs/README.TXT          -- <this file>
-
* /motd/textcmd/player/motd.src  -- Text-command to show the tip-window
* /motd/textcmd/player/motd.txt  -- Text-file containing the tip message
-
* /motd/ctrlReg.src             -- Registration script for the ICP package
* /motd/logon.src                -- Script called at player login
* /motd/pkg.cfg                  -- Package Configuration File
* /motd/reconnect.src            -- Script called at player reconnection

=======================
= General Information =
=======================

I don't pass shard-virii scripts but if you're in the least bit dubious about
the nature of any .ECL files within this package, feel free to recompile them!
You should really do that with all supplied .SRC files, anyway! :-)

Please note that this package is shard-friendly and is initially disabled in
'pkg.cfg'. To enable it, simply alter the entry in 'pkg.cfg' from "Enabled 0"
to "Enabled 1" and (re)start your server. :-)

- The ICP package is supplied seperately.
- (check the Yahoogroup under "OptionalPackages/").

===================================
= I'm stuck! How do I install it? =
===================================

1) Install the package to your /pkg/opt directory (or whichever you choose)
2) Ensure the package is enabled (see previous paragraph)
3) Start (or restart) your server so that it's recognized.

Et voilà! It should work! :)

================================================
= I've Done What You Said But It Doesn't Work! =
================================================

The only things that should stop this from working are:

1) Not having an up-to-date core (anything 088 or higher should work)
2) The .ECL files are from a different compiler-version

In those cases:

1) Update your version of the core! At the time of writing, the latest full
   core release is 090!
2) Re-compile all the .SRC files in this archive.

If neither of these work, then I'm not sure what is causing it! Take a note
of any/all error messages that are being shown and email me! :)

====================
= What Does It Do? =
====================

Three things are now in place:

1) When a player initially logs into your shard, the logon.ecl will be called
   and the MOTD.ECL script will be run, showing the tip window.
2) When a player reconnects after a link-dead, the reconnect.ecl will be
   called and the MOTD.ECL script will be run, showing the tip window.
3) When your players type ".motd" the text-command script will be run and the
   tip window will appear.

The "MOTD text" that comes up in the tip window is held in the following file:

   /motd/textcmd/player/motd.txt

================
= HTML Content =
================

You can use simple html tags to format the message of the day. Here are the
tags:

<b>BOLD</b>

<i>ITALIC</i>

<i>UNDERLINED</i>

<center>CENTERED</center>

This has<br>a linebreak


Note that you can nest the tags, so:

<center><b><i>BOLD, ITALIC AND CENTERED</i></b></center>


Other nice ways are to use eg:

<b>o</b> This has kind of list item circle


====================================================
= The Text In The Tip Window Won't Align Properly! =
====================================================

New MESSAGE OF THE DAY works with standard DOS (and mostlikely unix)
end-of-line characters.


DEPRICATED:

What you're probably experiencing is the stupid way the UO client outputs the
Tip Window text! What's so stupid about it? Well, it only expects single
"carriage returns" as end-of-line characters.

Why's that a problem? Well, if you're using Windows, then "end-of-line
characters" are actually CR/LF pairs (ASCII 13/10) and not just single CR
characters (ASCII 13), which causes the Tip Window text to double-linespace!

How do you fix it? You have 2 options...
1) The easiest: Save or convert the file to UNIX format!
2) Use a text editor or other program to search/replace CR/LF pairs with CR
   characters (ugh!)

Trust me when I say that #1 is easier! ;-) UEdit32, for example, will
happily save in UNIX format as will a number of other nifty editors.

=====================================================
= I Want To Disable The Login/Reconnect Tip Windows =
=====================================================

To disable the auto-MOTD options, simply delete the corresponding .ECL file!
If the core (server) can't find the .ECL files, then it can't run them :)

=======================
= Contact Information =
=======================

I hope you find this package useful! :-)

Any comments, ideas, flames or general "stuff" should go, by email, to:

   tjh@myrathi.co.uk
   
Happy scripting!

- Myrathi

=============================================================================
All files within this archive are (OS) TJ Houston (Myrathi), 24-May-2000/1/2
and are, as such, freely editable. Please give due credit, though... 'k? :-)