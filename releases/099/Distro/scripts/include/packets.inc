/*
        Maintened by *Edwards
        
        Last Modifications 2009-09-02
             * Added DisplayDamage()
*/
use uo;
use polsys;

enum PACKET_ID
    PACKET_FEATURES     := 0xB9,
    PACKET_CLILOC       := 0xC1
endenum

enum FEATURE_BITS
    FEATURE_CHAT        := 0x0001,
    FEATURE_LBR         := 0x0002,
    FEATURE_AOS         := 0x0004
endenum

enum SEASONS
    SEASON_SPRING := 0,
    SEASON_SUMMER := 1,
    SEASON_FALL := 2,
    SEASON_WINTER := 3,
    SEASON_DESOLATION := 4
endenum

/*
 * SendFeatures(who, features)
 *
 * Purpose
 * Enable OSI client features like the chat button or LBR graphics.
 *
 * Parameters
 * who:         The character on which to enable the features
 * features:    The features to enable, see enumeration above for possible features
 *
 * Return value
 * No return value
 *
 * TODO:
 * Make sure the feature bits are right
 *
 */
function SendFeatures(who, features)
	var packet;

	packet := CreatePacket(PACKET_FEATURES, 0x03);
	packet.SetInt16(1, features);
	packet.SendPacket(who);

	return 1;
endfunction

function PlayMovingEffectXYZHued(source, dest, effect, speed, loop:=0, explode:=0, color:=0, translucent:=0)
	var packet := CreatePacket(0xC0, 36);
	packet.SetInt8(1, 0x00);
	packet.SetInt32(2, source.serial);
	packet.SetInt32(6, dest.serial);
	packet.SetInt16(10, effect);
	packet.SetInt16(12, source.x);
	packet.SetInt16(14, source.y);
	packet.SetInt8(16, source.z);
	packet.SetInt16(17, dest.x);
	packet.SetInt16(19, dest.y);
	packet.SetInt8(21, dest.z);
	packet.SetInt8(22, speed);
	packet.SetInt8(23, loop);
	packet.SetInt16(24, 0);
	packet.SetInt8(26, 0);
	packet.SetInt8(27, explode);
	packet.SetInt32(28, color);
	packet.SetInt32(32, translucent);
	packet.SendAreaPacket(source.x, source.y, 16, source.realm);
endfunction

function OpenBrowser( who, link:="" )

	var packet;

	packet := CreatePacket(0xA5, MSGLEN_VARIABLE);
	packet.SetString(0x03, link, 1);
	packet.SetInt16(0x01, packet.GetSize());
	packet.SendPacket(who);

	return 1;

endfunction

function PlayerSpeedBoost( player, toggle := 0 )

	if( toggle )
		SendPacket( player, "BF0006002601" );
	else
		SendPacket( player, "BF0006002600" );
	endif
 
        return 1;
endfunction

function DisplayDamage( damaged, amount )

        if( damaged.hidden || damaged.concealed )
                return 0;
        endif

	var dmg_packet := CreatePacket( 0x0B, 0x07 );

	if( CInt( amount ) <= 255 )
		dmg_packet.SetInt8( 0x06, CInt( amount ));
	else
		dmg_packet.SetInt16( 0x05, CInt( amount ));
	endif

	dmg_packet.SetInt32( 0x01, damaged.serial );
 
        foreach mobile in
                SleepMS(5);
                if( mobile.IsA( POLCLASS_NPC ))
                        continue;
                elseif( !GetObjProperty( mobile, "NotifyDmg" ))
                        continue;
                endif
                
		dmg_packet.SendPacket( mobile );
        endforeach

	return 1;
endfunction

function SendSeasonForPlayer( who, season )

	var packetString := "BC"+fixPacketLength( season, 1 )+"01";
 
	return SendPacket( who, packetString );
endfunction

function FakeDead( who, wake_up:=1 )

	Detach();

	if( GetProcess( CInt( GetObjProperty( who, "#FakeDeadPid" ))))
		return 0;
	else
		SetObjProperty( who, "#FakeDeadPid", GetPid() );
	endif
 
	if( wake_up )
		PlayCharAnim( who );
	else
        	var x := who.x,
            	    y := who.y;

		while( who.connected )
			PlayCharAnim( who );

                	if( who.x != x || who.y != y )
               			break;
                	endif
                	SleepMS( 100 );
        	endwhile
	endif
        
        return 1;
endfunction

function PlayCharAnim( who )

        var dir := 1;
        var speed := 5;            //0 = fastest, 30 = way too  slow
        var doesrepeat := 0;
        var framecount := 6;      // seems to set the frame it uses?
        var numrepeats := 1;

        var packetString := "6E";
        packetString := packetString + fixPacketLength( hex( who.serial ), 4 );
        packetString := packetString + fixPacketLength( hex( 21 ), 2 );         // animation
        packetString := packetString + fixPacketLength( hex( framecount ), 2 );      // frame count
        packetString := packetString + fixPacketLength( hex( numrepeats ), 2 );      // Repetition times
        packetString := packetString + fixPacketLength( hex( dir ), 1 );         // 00 == fwd, 01 == bkwd
        packetString := packetString + fixPacketLength( hex( doesrepeat ), 1 );      // Does this animation repeat? 01 == yes
        packetString := packetString + fixPacketLength( hex( speed ), 1 );         // frame delay

        foreach chr in ListMobilesNearLocation( who.x, who.y, who.z, 16, who.realm );
               SendPacket( chr, packetString );
               SleepMS(10);
        endforeach

        return 1;
endfunction

function FixPacketLength( packetString, byteLength )

        if( !packetString || packetString == error )
                packetString := 0;
        endif
        
        packetString := CStr( packetString );

        if( packetString[1,2] == "0x" )
                packetString := cStr( packetString[3, len( packetString )] );
        endif

        if( len( packetString ) > byteLength * 2 )
		var extraBytes := len( packetString ) - ( byteLength*2 ) + 1;
		return cStr( packetString[extraBytes, len( packetString )] );
        endif

        while( len( packetString ) < byteLength * 2 )
                packetString := "0"+packetString;
                SleepMS(10);
        endwhile

        return packetString;
endfunction
