/* $Id: packets.inc 984 2005-11-14 09:02:34Z muaddiblsd $
 *
 * Purpose
 * Provide various functions and constants for raw packets.
 *
 */
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