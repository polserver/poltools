// $Id: bardInstrument.inc 834 2005-11-02 14:09:50Z austin $

/*
 * findInstrument(character)
 *
 * Purpose
 * Gets ItemRef via CProp for instrument character is
 * using for barding. If none can be gotten, uses a
 * target function to try to get one.
 * instrument to use in Barding.
 *
 * Parameters
 * character:	Mobile reference of player.
 *
 * Return value
 * Returns a 0 or ItemRef on success.
 *
 */
function findInstrument(character)

	var myInstrument := GetObjProperty(character, "#Instrument");
	if( TypeOf(myInstrument) != "Integer")
		myInstrument := 0;
	endif
	if( myInstrument )
		myInstrument := SystemFindObjectBySerial(myInstrument);
	else
		myInstrument := targetNewInstrument(character);
	endif
	if( !myInstrument.isA(POLCLASS_ITEM) )
		SendSysMessage(character, "Could not find your instrument!");
		return 0;
	elseif( !(myInstrument in EnumerateItemsInContainer(character.backpack)) )
		SendSysMessage(character, "Could not find your instrument!");
		return 0;
	endif

	SetObjProperty(character, "#Instrument", CInt(myInstrument.serial));

	return myInstrument;


endfunction


/*
 * targetNewInstrument(character)
 *
 * Purpose
 * Gets new ItemRef via targetting by player for an
 * instrument to use in Barding.
 *
 * Parameters
 * character:	Mobile reference of player.
 *
 * Return value
 * Returns a 0 or ItemRef on success.
 *
 */
function targetNewInstrument(character)

	var instrumentList := {0xeb1, 0xeb2, 0xeb3, 0xeb4, 0xe9c, 0xe9d, 0xe9e};

	SendSysMessage(character, "Target the instrument you wish to use.");

	var instrument := Target(character);
	if( !instrument.isA(POLCLASS_ITEM) )
		SendSysMessage(character, "Targetting Cancelled.");
		return 0;
	endif

	if( instrument.graphic in instrumentList )
		return instrument;
	else
		return 0;
	endif

endfunction


/*
 * bardSoundCheck(character, instrument, attribute)
 *
 * Purpose
 * Gets the sound number to play based on
 * instrument, skill type, and client version.
 *
 * Parameters
 * character:	Mobile reference of player.
 * instrument:	Instrument Item reference.
 * attribute:	Attribute of skill being used.
 *
 * Return value
 * Returns dictionary.
 *
 */
function bardSoundCheck(character, instrument, bard_skill)

	var retvalue := dictionary { "success", "fail" };;

	if( CInt(character.clientversion[1]) > 2 )
		case(instrument.graphic)
			0xeb1:  retvalue["success"] := playHarp(bard_skill);  retvalue["fail"] := 0x45;
			0xeb2:  retvalue["success"] := playHarp(bard_skill, 2);  retvalue["fail"] := 0x47;
			0xeb3:  retvalue["success"] := playLute(bard_skill);  retvalue["fail"] := 0x4E;
			0xeb4:  retvalue["success"] := playLute(bard_skill);  retvalue["fail"] := 0x4E;
			0xe9c:  retvalue["success"] := playDrum(bard_skill);  retvalue["fail"] := 0x3A;
			0xe9d:  retvalue["success"] := playTamb(bard_skill);  retvalue["fail"] := 0x54;
			0xe9e:  retvalue["success"] := playTamb(bard_skill);  retvalue["fail"] := 0x54;
		endcase
	else
		case(instrument.graphic)
			0xeb1:  retvalue["success"] := 0x44;  retvalue["fail"] := 0x45;
			0xeb2:  retvalue["success"] := 0x46;  retvalue["fail"] := 0x47;
			0xeb3:  retvalue["success"] := 0x4d;  retvalue["fail"] := 0x4e;
			0xeb4:  retvalue["success"] := 0x4d;  retvalue["fail"] := 0x4e;
			0xe9c:  retvalue["success"] := 0x39;  retvalue["fail"] := 0x3a;
			0xe9d:  retvalue["success"] := 0x53;  retvalue["fail"] := 0x54;
			0xe9e:  retvalue["success"] := 0x53;  retvalue["fail"] := 0x54;
		endcase
	endif

	return retvalue;

endfunction


/*
 * playHarp(attribute, instrumentType)
 *
 * Purpose
 * Returns sound for successful music playing
 * based on skill attempting the music, for the
 * Harp instrument.
 *
 * Parameters
 * attribute:	Attribute attempting the music.
 * iTtype:	lap hard 1, or standing harp 2.
 * Return value
 * Returns a 0 or +Integer on success.
 *
 */
function playHarp(theskill, iType:=1)

	case(theskill)
		DISCORDANCE:  return 0x393;
		PEACEMAKING: return 0x394;
		PROVOCATION: return 0x392;
	endcase

	// If skill wasn't one of the three above,
	// then play the standard music.
	case(iType)
		1: return 0x44;
		2: return 0x46;
	endcase

endfunction


/*
 * playLute(attribute)
 *
 * Purpose
 * Returns sound for successful music playing
 * based on skill attempting the music, for the
 * Lute instrument.
 *
 * Parameters
 * attribute:	Attribute attempting the music.
 *
 * Return value
 * Returns a 0 or +Integer on success.
 *
 */
function playLute(theskill)

	case(theskill)
		DISCORDANCE:  return 0x40C;
		PEACEMAKING: return 0x419;
		PROVOCATION: return 0x404;
	endcase

	// If skill wasn't one of the three above,
	// then play the standard music.
	return 0x4d;

endfunction


/*
 * playDrum(attribute)
 *
 * Purpose
 * Returns sound for successful music playing
 * based on skill attempting the music, for the
 * Drum instrument.
 *
 * Parameters
 * attribute:	Attribute attempting the music.
 *
 * Return value
 * Returns a 0 or +Integer on success.
 *
 */
function playDrum(theskill)

	case(theskill)
		DISCORDANCE:  return 0x2E9;
		PEACEMAKING: return 0x2EA;
		PROVOCATION: return 0x2E8;
	endcase

	// If skill wasn't one of the three above,
	// then play the standard music.
	return 0x39;

endfunction


/*
 * playTamb(attribute)
 *
 * Purpose
 * Returns sound for successful music playing
 * based on skill attempting the music, for the
 * Tambourine instrument.
 *
 * Parameters
 * attribute:	Attribute attempting the music.
 *
 * Return value
 * Returns a 0 or +Integer on success.
 *
 */
function playTamb(theskill)

	case(theskill)
		DISCORDANCE:  return 0x4B7;
		PEACEMAKING: return 0x4B8;
		PROVOCATION: return 0x4B6;
	endcase

	// If skill wasn't one of the three above,
	// then play the standard music.
	return 0x53;

endfunction
