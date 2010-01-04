// Cliloc function v3.0
// By BlahGoD and Kiwi
// Not for the faint of heart :)

use uo;
use basic;

function Cliloc(sendto, subject, color, messageID, system := 1, name := 1)
if(name = 1)
  name := subject.name;
endif
if(color = "03b2")
  color := 996;
endif
var length := 18;
var namehex := "";
var tester;
var message := fixpacketlength(hex(messageID),3);
if(system)
tester := "ffffffffffff07";
else
tester := fixpacketlength(hex(subject.serial),4) + fixpacketlength(hex(subject.graphic),2) + "07";
endif
var count := len(name);
length := length + count;
length := fixpacketlength(hex(length), 2);
var loopcount := 1;
name := CAscZ(name);
while(loopcount < count + 1)
	namehex := namehex + cstr(fixpacketlength(hex(name[loopcount]), 1));
	loopcount := loopcount + 1;
endwhile
var msgCol := fixpacketlength(Hex(color),2);
var packet := "c1" + length + tester + msgCol + "000200" + message + namehex;
SendPacket(sendto, packet);
endfunction

// Thanks to Max for this function :=)

function fixPacketLength( packetString, byteLength )
    if( !packetString || packetString == error )
        packetString := 0;
    endif
    packetString := cStr(packetString);
    
    if( packetString[1,2] == "0x" )
        packetString := cStr(packetString[3,len(packetString)]);
    endif
    
    if( len(packetString) > byteLength * 2 )
        var extraBytes := (byteLength*2) - len(packetString);
        return cStr(packetString[extraBytes,len(packetString)]);
    endif
    
    while( len(packetString) < byteLength * 2 )
        packetString := "0" + packetString;
    endwhile
    
    return packetString;
endfunction