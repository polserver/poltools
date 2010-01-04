Matrix Shard (www.matrixshard.com)
Artisan Skill - version 1.0
Creator - Arcadia (death@kissofdeath.co.uk)
Created - 07/07/01


Instructions
=============
1. unzip these files to pkg/opt/painting

1. add this code to your pol/config/menu.cfg file

ItemMenu
{
	Name	art
	Title	Which painting?
	Entry   0xea8 Small Painting
 	Entry	0xea7 Small Painting
	Entry 	0xea4 Medium Painting
	Entry 	0xea3 Medium Painting
	Entry 	0xec9 Portrait
	Entry 	0xe9f Portrait
	Entry	0xea0 Large Painting
}

2. Check that my item numbers in my itemdesc.cfg dont clash with any of yours. 
If they do you will have to edit mine

3. run and enjoy


What does it do?
================

by double clicking a palette and targeting a blank canvas while standing next to an easel 
players can paint pictures. If they have high enough skill they will paint a masterpiece.
I wrote this code especially for the artisan skill on matrix shard but since most shards
wont have this skill ive edited it slightly to use tailoring skill so you can use it too,
but you can change it to which ever skill you like. 
The amount of skill needed to make each painting can be edited in art.cfg

Bugs
====
As far as I know there are no bugs in my code, in the unlikely event that you discover a
bug please email me (death@kissofdeath.co.uk) and tell me about it

Stuff still to do be done
==========================

make the portrait paintings require another target allowing the artist to select a 
subject and add the subjects name to the painting.
(a portrait of "subjectname")

make masterpiece portrait paintings renamed to 
(a masterpiece portrait of "subjectname" created by "artistsname"

HAPPY PAINTING!! - Arcadia