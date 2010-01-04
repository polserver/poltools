///////////////////
//  tries to cast the specified druid spell
///////////////////
include "include/attributes";

function CastDruidSpell( caster, staff, spellnum )

	//load the config file
	var spell_cfg_file:= ReadConfigFile("druidspells");
	var current_spell := FindConfigelem(spell_cfg_file,spellnum);

	//make sure the caster has enough mana
	if ( current_spell.mana < caster.mana )
		PrintTextAbovePrivate(caster, "Not enough mana!", caster);
		return 0;
	endif

	//consume the reagents
	//the "#nodruidreagents" prop is set if the caster was a GM and declared that they didn't need reagents
	if(!GetObjProperty(caster, "#nodruidreagents"))
		//go through all the reagents
		var reagents := GetConfigStringArray(current_spell,"reagent");
		var reg, amt, regline;
		foreach reagent in reagents
			regline := splitwords(reagent);
			reg := cint(regline[1]);
			amt := cint(regline[2]);
			if (!ConsumeSubstanceIsBroken(caster, reg, amt))
				PrintTextAbovePrivate(caster, "Not enough reagents", caster);
				return 0;
			endif
		endforeach
	endif

	//save the property "#castingdruidspell".  This is done in order to avoid casting more than
	//one spell at a time
	SetObjProperty (caster, "#castingdruidspell", 1);

	//save the player's position
	var casterx := caster.x;
	var castery := caster.y;

	//play the animations
	var animation := GetConfigStringArray(current_spell, "animation");
	foreach motion in animation
		PerformAction( caster, cint(motion));
		sleepms(1500);
		if (casterx != caster.x or castery != caster.y)
			EraseObjProperty (caster, "#castingdruidspell");
			return 0;
		endif
	endforeach

	//do the skill check.  Return 0 on failure and erase the "#castingdruidspell" prop
	if (!PerformDruidSkillCheck (caster, current_spell.skill_magery, current_spell.skill_animallore) )
		EraseObjProperty (caster, "#castingdruidspell");
		return 0;
	//otherwise, subtract the mana, start the script for that spell, and return 1
	else
		caster.mana := caster.mana - current_spell.mana;
		start_script(current_spell.script, caster);
		return 1;
	endif

endfunction




///////////////////
//  does a skillcheck based on magery and animallore
//////////////////

function PerformDruidSkillCheck (caster, spell_skill_magery, spell_skill_animallore)

	//decrease the player's necromancy skill just for trying
	var skillval := (GetEffectiveSkill(caster, SKILLID_SPIRITSPEAK) + 20);
	if (skillval)
		skillval := skillval - (skillval/100);
		if (skillval < 0)
			skillval := 0;
		endif
		SetAttributeBaseValue(caster, SKILLID_SPIRITSPEAK, RawSkillToBaseSkill(skillval) );
	endif

	//now do the actual skill check
	//if it requires both magery and animallore
	if (spell_skill_magery and spell_skill_animallore)
		if (!Checkskill(caster, SKILLID_MAGERY, spell_skill_magery, 0) and !Checkskill(caster, SKILLID_ANIMALLORE, spell_skill_animallore, 0))
			return 0;
		else
			return 1;
		endif
	//if it requires magery but no animallore
	elseif (spell_skill_magery and !spell_skill_animallore)
		if (!Checkskill(caster, SKILLID_MAGERY, spell_skill_magery, 0))
			return 0;
		else
			return 1;
		endif
	//if it requires animallore but no magery
	elseif (!spell_skill_magery and spell_skill_animallore)
		if (!Checkskill(caster, SKILLID_ANIMALLORE, spell_skill_animallore, 0))
			return 0;
		else
			return 1;
		endif
	//otherwise just return 1
	else
		return 1;
	endif
endfunction




///////////////////
//  temporary function until Consume Substance is fixed
///////////////////

function ConsumeSubstanceIsBroken(caster, what, amt)

	var mystuff := HasReagents(caster, what, amt);
	if (!mystuff)
		return 0;
	else
		SubtractAmount(mystuff,amt);
		return 1;
	endif

endfunction

function HasReagents(caster, what, amt)

	foreach item in EnumerateItemsInContainer(caster.backpack)
		if ( (item.objtype = what)  and (item.amount >= amt) )
			return item;
		endif
	endforeach

	return 0;

endfunction




///////////////////
//  what happens when the spell fizzles
///////////////////

function Fizzle(caster)

	PlaySoundEffect(caster, 0x005d);
	PlayObjectCenteredEffect(caster, 0x3735, 10, 30);
	sleep(4);

endfunction




///////////////////
//  Put the rune ojbtype in the bag specified.  Doesn't check to make sure its a valid rune.
///////////////////

function PutRuneInBag (caster, bag, rune_objtype)

	var rune_string := GetObjProperty (bag, "runes");
	if (!rune_string)
		rune_string := "";
	endif
	var rune_array := splitwords(rune_string);
	var number_of_runes := len (rune_array);
	var i;

	for (i := 0; i <= number_of_runes; i := i + 1)
		if (cint(rune_array[i]) = cint(rune_objtype) )
			return 0;
		endif
	endfor

	rune_string := rune_string + " " + rune_objtype;
	SetObjProperty (bag, "runes", rune_string);

	return 1;

endfunction



///////////////////
//  takes the rune objtype out of the bag.  Doesn't check to make sure its a valid rune
///////////////////

function TakeRuneOutOfBag (caster, bag, rune_objtype)

	var rune_string := GetObjProperty (bag, "runes");
	if (!rune_string)
		rune_string := "";
	endif
	var rune_array := splitwords(rune_string);
	var number_of_runes := len (rune_array);
	var i;
	var rune_location := 0;

	for (i := 0; i <= number_of_runes; i := i + 1)
		if (cint(rune_array[i]) = cint(rune_objtype) )
			rune_location := i;
		endif
	endfor

	if (!rune_location)
		return 0;
	endif

	var second_rune_string := "";

	for (i := 1; i <= number_of_runes; i := i + 1)

		if (i != rune_location)
			second_rune_string := second_rune_string + " " + rune_array[i];
		endif
	endfor

	SetObjProperty (bag, "runes", second_rune_string);

	return 1;

endfunction




///////////////////
//  checks to see if the runeobjtype is in the bag
///////////////////

function CheckForRuneInBag (caster, bag, rune_objtype)

	var rune_string := GetObjProperty (bag, "runes");
	if (!rune_string)
		return 0;
	endif

	var rune_array := splitwords(rune_string);

	foreach rune in rune_array
		if ( cint(rune) = cint (rune_objtype) )
			return 1;
		endif
	endforeach

	return 0;

endfunction




///////////////////
//  check to see if the source of the spell resists
///////////////////

function DruidResist(caster, cast_on, spell_damage)

	var cast_on_resist := GetAttribute(cast_on, SKILLID_MAGICRESISTANCE);
	var caster_animallore := GetAttribute(caster, SKILLID_ANIMALLORE);

	if (!CheckSkill(cast_on, SKILLID_MAGICRESISTANCE, caster_animallore, 0 ))
		return spell_damage;
	else
		SendSysMessage(cast_on, "You feel yourself resisting magical energy!");
		PlaySoundEffect(cast_on, SFX_SPELL_WEAKEN);
		PlayObjectCenteredEffect (cast_on, FX_CURSE_EFFECT, 5, 5);
		spell_damage := cint (spell_damage/2);
	endif

	if (!CheckSkill(caster, SKILLID_ANIMALLORE, -1, 0 ))
		return spell_damage;
	endif

	if ( caster_animallore > cint(cast_on_resist/4) )
		spell_damage := cint(spell_damage * 2.25);
	elseif ( caster_animallore > cint(cast_on_resist/2) )
		spell_damage := cint(spell_damage * 2);
	elseif ( caster_animallore > cast_on_resist )
		spell_damage := cint(spell_damage * 1.5);
	else
		spell_damage := cint(spell_damage * 1.25);
	endif

	return spell_damage;

endfunction

function FindEquippedStaff (caster)
	var weapon_one := GetEquipmentByLayer( caster,  LAYER_HAND1 );
	var weapon_two := GetEquipmentByLayer( caster,  LAYER_HAND2 );

	if ( weapon_one.objtype = 0x7081 )
		return weapon_one;
	elseif ( weapon_two.objtype = 0x7081 )
		return weapon_two;
	else
		return 0;
	endif
endfunction



