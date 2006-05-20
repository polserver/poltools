// $Id: bard.inc 834 2005-11-02 14:09:50Z austin $

include "bardSkill";
include "bardInstrument";
include ":brainAI:npcUtil";

/*
 * figureBardDifficulty(character, npc, instrument, attribute)
 *
 * Purpose
 * Retrieves information about an NPC to figure the barding
 * difficulty level of said NPC.
 *
 * Parameters
 * character:	Mobile reference of player using bard skill.
 * npc:		NPC to check for difficulty points.
 * instrument:  Instrument character is using for skills.
 * attribute:	Attribute of the skill that is being used.
 *
 * Return value
 * Returns an integer.
 *
 */
function figureBardDifficulty(who, peacee, instrument, attribute)

	var pHP := CDbl(AP_GetVital(peacee, "Hits") * 1.6);
	var pSt := CDbl(AP_GetVital(peacee, "Stamina"));
	var pMa := CDbl(AP_GetVital(peacee, "Mana"));
	var pSkills := AP_CheckSkillsTotal(peacee);
	var statSkillTotal := CInt(pHP+pSt+pMa+pSkills);

	var abilityTotal := figureAbilityTotal(peacee);

	var pPoison := GetObjProperty(peacee, "poison_strength");
	if( TypeOf(pPoison) != "Integer" )
		pPoison := 0;
	endif
	pPoison := pPoison * 20;

	var totalAdjustment := statSkillTotal+abilityTotal+pPoison;
	var barding_difficulty;
	if( statSkillTotal > 700 )
		totalAdjustment := totalAdjustment - 700;
		totalAdjustment := CDbl(totalAdjustment) * 0.275;
		totalAdjustment := totalAdjustment + 700;
		barding_difficulty := CInt(totalAdjustment / 10);
	else
		barding_difficulty := CInt(totalAdjustment / 10);
	endif

	if( barding_difficulty > 160 )
		barding_difficulty := 160;
	endif

	if( CInt(GetObjProperty(instrument, "Exceptional")) )
		print( "Debug Bard: Exceptional Check Passed");
		barding_difficulty := barding_difficulty - 5;
	endif
	var musicianship := CInt(AP_GetSkill(who, MUSICIANSHIP));
	if( musicianship > 100 )
		barding_difficulty := barding_difficulty - CInt((musicianship - 100) / 2);
	endif
	if( attribute == PROVOCATION )
		barding_difficulty := barding_difficulty - 5;
	elseif( attribute == PEACEMAKING || attribute == DISCORDANCE )
		barding_difficulty := barding_difficulty - 10;
	endif

	return barding_difficulty;

endfunction


/*
 * figureAbilityTotal(mobile)
 *
 * Purpose
 * Retrieves the base value of the abilities * 100.
 *
 * Parameters
 * mobile:	Mobile reference to retrieve the points from.
 *
 * Return value
 * Returns an integer.
 *
 */
function figureAbilityTotal(peacee)

	var ability_total := 0;
	var pTemplate := NPC_GetNPCConfig(peacee.npctemplate);
	foreach ability in (Splitwords(pTemplate.Abilities))
		// var = var used to stop unused var report in compile.
		ability := ability;
		ability_total := CInt(ability_total) + 100;
	endforeach

	return ability_total;

endfunction


function isBardable(character, thetarget, bard_skill, bard_difficulty)

	if( CalcSuccessPercent(character, bard_skill, bard_difficulty, AP_GetSkill( character, bard_skill)) < 50 )
		return 0;
	endif

	var pTemplate := NPC_GetNPCConfig(thetarget.npctemplate);
	var ifCanBard := 1;

	case(bard_skill)
		DISCORDANCE: ifCanBard := CInt(pTemplate.NoDiscord);
		PEACEMAKING: ifCanBard := CInt(pTemplate.NoPeacmake);
		PROVOCATION: ifCanBard := CInt(pTemplate.NoProvoke);
	endcase

	return ifCanBard;

endfunction
