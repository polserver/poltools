Cliloc.inc v3.0

Call the function with:

Cliloc(sendto, subject, color, messageID, system, "name")

1. sendto is the reciever of the packet, meaning the person you want to see the message

2. subject is what usually the target, can be same as sendto

3. color is the text color. 

--New in 3.0 : No longer needs to be in hex, uses the same color #'s as other functions such as Sendsysmessage

4. messageID is the cliloc message number. (Explained below)

5. system tells if it is a system message (appears in lower corner) or if 0 means that message appears over the target. Default is 1

6. name is the name that will appear in the journal. If you use anatomy on a cow, it will say "a cow: this target looks.. " etc.

Default is subject.name. 

------

to get the proper cliloc messageID find it in the Localization.txt

To use a cliloc string from a normal cliloc file, use the offset 1000000, plus the file number multiplied by 10,000. 

Example: To display a message from cliloc10, use 1010000 as the offset. 

Then, add the ID number of the message, which can be found in the Localization.txt file that you can download from the link above. 

Example: 1010312 will display message #312 from cliloc10.enu (or .fra, .jpn, etc. depending on the client's language). 

Cliloc messages from the special cliloc-1 file have the offset of 500000. 

500118 displays message #118 from cliloc-1.enu.

that number is your messageID


**this function was jacked based on the Epsilon emulator function...

-bg & kiwi
OPEN SHARD (http://openshard.cjb.net)