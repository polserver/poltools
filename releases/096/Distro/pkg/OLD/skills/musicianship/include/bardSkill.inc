// $Id: bardSkill.inc 87 2006-05-23 19:04:43Z sroyalty $

/*
 * bardSkillCheck(character, instrument, attribute, difficulty)
 *
 * Purpose
 * Checks skill success/failure and plays appropriate sounds
 * based on SkillCheck() function. Performs a Client Version
 * check for playing a more broad selection of sounds if the
 * client supports them.
 *
 * Parameters
 * character:	Mobile reference of player using bard skill.
 * instrument:  Instrument character is using for barding.
 * attribute:	Attribute of the skill that is being used.
 * difficulty:	Diffuculty to pass to SkillCheck() function.
 *
 * Return value
 * Returns an integer. 1 on success, 0 on failure.
 *
 */
function bardSkillCheck( character, instrument, bard_skill, bard_difficulty, award_diff:=0, advance_flags:=3)

	// Gets a dictionary for success/fail.
	var soundCheck := bardSoundCheck(character, instrument, bard_skill);

	SkillCheck(character, MUSICIANSHIP, 1);
	if( (AP_GetSkill(character, MUSICIANSHIP) < 10) && (bard_skill != MUSICIANSHIP) )
		SendSysMessage(character, "You are not skilled enough in the musical arts.");
		return 0;
	endif

	if(SkillCheck(character, bard_skill, bard_difficulty, award_diff, advance_flags) > 0 )
		PlaySoundEffect( character, soundCheck.success );
		return 1;
	else
		PlaySoundEffect( character, soundCheck.fail );
		return 0;
	endif

	return 0;

endfunction
