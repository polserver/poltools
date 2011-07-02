/*
        Maintened by *Edwards

        Last Modifications 2009-01-18
             * Cleaned the GetClasseSkills()

        Last Modifications 2009-02-14
             * Moved Animal Taming and Animal Lore to Bard
             * Moved Cartography and Herding to Ranger
             * Renamed Bard for Druid

        Last Modifications 2009-06-05
             * Removed the last modifications of 2009-02-14
             * Moved main var to config settings

        Last Modifications 2009-10-19
             * Removed ecompiler warnings
*/
use uo;

include ":attributes:attributes";
include ":attributes:attributes_ex";
include ":classes:settings";
include ":classes:classes_ref";

var settings := Classes_GetSettingsCfgElem( "Settings" );

function ClasseBonus( mobile, classe )

	var level := CInt( GetObjProperty( mobile, classe ));
	if( level )
		return CDbl( settings.BonusLevel * level ) + 1;
	else
		return 1;
	endif
endfunction

function PourcentClasseBonus( mobile, classe )

        var level := CInt( GetObjProperty( mobile, classe ));
	if( level )
		return CDbl( BONUS_PER_LEVEL * level );
	else
                return 1;
        endif
endfunction

function IsFromThatClasse( mobile, classe_skills )

        var classe := 0,
	    total := 0,
	    number := len( classe_skills ),
	    level := 0;

	foreach i in AP_ListAttributesByType( "skill" )

		var amount := AP_GetSkill( mobile, i );
		total += amount;
		if( i in classe_skills )
			classe += amount;
		endif
                SleepMS(5);
	endforeach

	if( classe < settings.AverageSkill * number )
		return 0;
	elseif( classe_skills == GetClasseSkills( PP ) )
		var average := classe / SKILLID__HIGHEST,
		    raise_average := 0;

		while( average >= ( settings.AveragePPSkills + raise_average ))
			level += 1;
			raise_average += settings.AveragePPMod;
                        SleepMS(5);
		endwhile
		return level;
	elseif( classe < CInt( total * number * settings.EachMustRepresent * 0.01 ))
		return 0;
	else
		level := 1;

		var represent := settings.EachMustRepresent + settings.RepresentLevelMod,
		    percent   := CDbl( total * number * represent * 0.01 ),
		    average_t := CDbl(( settings.AverageSkill + settings.AverageLevelMod ) * number );

		while(( classe >= average_t ) && ( classe >= percent ))
			level     += 1;
			represent := CDbl( represent + settings.RepresentLevelMod );
			percent   := CDbl( total * number * represent * 0.01 );
			average_t := CDbl( average_t + settings.AverageLevelMod * number );
                        SleepMS(5);
		endwhile

		return level;
	endif
endFunction

function IsSpecialisedIn( mobile, skill_name )

	return CInt( GetObjProperty( mobile, GetClasseIdForSkillName( skill_name )));
endfunction

function HaveStatAffinity( mobile, stat_name )

	var classe_ids;
	case( stat_name )
		"Strength":	classe_ids := {  CRAFTER,
					         WARRIOR };
                                                 break;

		"Dexterity":	classe_ids := {  RANGER,
					         THIEF   };
                                                 break;

		"Intelligence":	classe_ids := {  BARD,
					         MAGE    };
                                                 break;

		default:	classe_ids := {};
	endcase

	foreach id in classe_ids
		var level := CInt( GetObjProperty( mobile, id ));
		if( level )
			return id;
		endif

                SleepMS(5);
	endforeach

	return 0;
endfunction

function HaveStatDifficulty( mobile, stat_name )

        var classe_ids;
	case( stat_name )
		"Strength":	classe_ids := {  BARD,
					         MAGE };
                                                 break;

		"Intelligence":	classe_ids := {  CRAFTER,
					         WARRIOR    };
                                                 break;

		default:	classe_ids := {};
	endcase

	foreach id in classe_ids
		var level := CInt( GetObjProperty( mobile, id ));
		if( level )
			return id;
		endif

                SleepMS(5);
	endforeach

	return 0;
endfunction

function GetTotalClasseSkills( character, classe )

        var skills := 0;
        foreach skill in GetClasseSkills( classe )
                skills += AP_GetTrueSkill( character, skill );
                SleepMS(5);
        endforeach

	return skills;
endfunction

function GetMaxClasseTitle( classe_name )

        var master_class := classe_name;
        
        case( classe_name )
                BARD:
                        master_class := "Minstrel";
                        break;
                CRAFTER:
                        master_class := "artisan";
                        break;
                MAGE:
                        master_class := "wizard";
                        break;
                RANGER:
                        master_class := "hunter";
                        break;
                THIEF:
                        master_class := "rogue";
                        break;
                WARRIOR:
                        master_class := "knight";
                        break;
        endcase
        
        return master_class;
endfunction
