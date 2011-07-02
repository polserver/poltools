/*
        Created by *Edwards

        2011-04-13
*/
use uo;
use os;

const BARD := "IsBard";
const CRAFTER := "IsCrafter";
const MAGE := "IsMage";
const RANGER := "IsRanger";
const THIEF := "IsThief";
const WARRIOR := "IsWarrior";
const PP := "IsPowerplayer";

function GetClasseName( classe_id )

        case( classe_id )
                BARD:		return( "bard" );
                CRAFTER:	return( "crafter" );
                MAGE:		return( "mage" );
                RANGER:	        return( "ranger" );
                THIEF:		return( "thief" );
                WARRIOR:	return( "warrior" );
		PP:		return( "powerplayer" );
        endcase
endfunction

function Get_Mobile_Classe_Name( mobile )

        if( GetObjProperty( mobile, BARD ));
                return BARD;
        elseif( GetObjProperty( mobile, CRAFTER ));
                return CRAFTER;
        elseif( GetObjProperty( mobile, MAGE ));
                return MAGE;
        elseif( GetObjProperty( mobile, RANGER ));
                return RANGER;
        elseif( GetObjProperty( mobile, THIEF ));
                return THIEF;
        elseif( GetObjProperty( mobile, WARRIOR ));
                return WARRIOR;
        elseif( GetObjProperty( mobile, PP ));
                return PP;
        endif

        return 0;
endfunction

function GetName( classe_id )

        case( lower( classe_id ))
                "bard":	        return BARD;
                                break;

                "crafter":	return CRAFTER;
                                break;

                "mage":		return MAGE;
                                break;

                "ranger":	return RANGER;
                                break;

                "thief":	return THIEF;
                                break;

                "warrior":	return WARRIOR;
                                break;

                "powerplayer":	return PP;
                                break;

                default:	return 0;
        endcase

        return 1;
endfunction

function GetClasseIdForSkillName( skill_name )

	foreach classe in GetClasseIds()
		if( skill_name in GetClasseSkills( classe ))
			return classe;
		endif

                SleepMS(5);
	endforeach

        return 0;
endfunction

function GetClasseIds()

	return { BARD,
		 CRAFTER,
		 MAGE,
		 RANGER,
		 THIEF,
		 WARRIOR,
		 PP	  };
endfunction

function GetClasseSkills( classe_id )

	case( classe_id )
		BARD:		return { BEGGING,
					 CARTOGRAPHY,
					 DISCORDANCE,
                                         HERDING,
					 MUSICIANSHIP,
					 PEACEMAKING,
					 PROVOCATION,
					 TASTE_IDENTIFICATION	};

		CRAFTER:	return { ARMS_LORE,
					 BLACKSMITHY,
					 BOWCRAFT_FLETCHING,
					 CARPENTRY,
					 LUMBERJACKING,
					 MINING,
                                         TAILORING,
					 TINKERING	};

		MAGE:		return { ALCHEMY,
					 EVALUATING_INTELLIGENCE,
					 INSCRIPTION,
					 ITEM_IDENTIFICATION,
					 MAGERY,
                                         MEDITATION,
					 RESISTING_SPELLS,
					 SPIRIT_SPEAK	};

		RANGER:	        return { ANIMAL_LORE,
					 ANIMAL_TAMING,
					 ARCHERY,
					 CAMPING,
					 COOKING,
					 FISHING,
					 TRACKING,
					 VETERINARY	};

		THIEF:		return { DETECTING_HIDDEN,
					 HIDING,
					 LOCKPICKING,
					 POISONING,
					 REMOVE_TRAP,
					 SNOOPING,
					 STEALING,
					 STEALTH	};

		WARRIOR:	return { ANATOMY,
					 FENCING,
					 HEALING,
					 MACE_FIGHTING,
                                         PARRY,
					 SWORDSMANSHIP,
					 TACTICS,
					 WRESTLING          };

                 PP:	        return { BEGGING,
					 CARTOGRAPHY,
					 DISCORDANCE,
					 HERDING,
					 MUSICIANSHIP,
					 PEACEMAKING,
					 PROVOCATION,
					 TASTE_IDENTIFICATION,
                                         ARMS_LORE,
					 BLACKSMITHY,
					 BOWCRAFT_FLETCHING,
					 CARPENTRY,
					 LUMBERJACKING,
					 MINING,
                                         TAILORING,
					 TINKERING,
                                         ALCHEMY,
					 EVALUATING_INTELLIGENCE,
					 INSCRIPTION,
					 ITEM_IDENTIFICATION,
					 MAGERY,
                                         MEDITATION,
					 RESISTING_SPELLS,
					 SPIRIT_SPEAK,
                                         ANIMAL_LORE,
					 ANIMAL_TAMING,
					 ARCHERY,
					 CAMPING,
					 COOKING,
					 FISHING,
					 TRACKING,
					 VETERINARY,
                                         DETECTING_HIDDEN,
					 HIDING,
					 LOCKPICKING,
					 POISONING,
					 REMOVE_TRAP,
					 SNOOPING,
					 STEALING,
					 STEALTH,
                                         ANATOMY,
					 FENCING,
					 HEALING,
					 MACE_FIGHTING,
                                         PARRY,
					 SWORDSMANSHIP,
					 TACTICS,
					 WRESTLING,
                                         FORENSIC_EVALUATION };
	endcase

        return 1;
endfunction
