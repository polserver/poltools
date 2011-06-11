/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        Last Modifications 2009-07-25
             * Revised code for better performances

        Last Modifications 2009-09-30
             * Fixed check to allow normal player to gain skills

        Last Modifications 2009-10-19
             * Removed ecompiler warnings

        Last Modifications 2009-10-26
             * Introduced hunger value in gain system.. now returns only skill advance.

        Last Modifications 2009-11-17
             * Fixed now advise if can't advance because of hunger..

        Last Modifications 2009-11-18
             * Removed advise if can't advance because of hunger..

        Last Modifications 2009-11-25
             * Removed hunger for now.

        Last Modifications 2010-01-24
             * Added hunger again. Will alter difficulty and modify % success if you do not eat.

        Last Modifications 2010-02-26
             * Removed: hunger.
             * Fixed SkillCheck() always returning positive pass_success
             * Added "CanTrain" flag for trainning pet ( eggs )
*/
use uo;
use os;
use util;
use cfgfile;

include ":areas:managment";

const ADV_SKILL		:= 0x1;
const ADV_STATS		:= 0x2;
const ADV_ALL		:= ADV_SKILL+ADV_STATS; // 3
const ADV_DISABLE	:= 0x4; // 4 - because the default is ADV_ALL.

function SkillCheck( mobile, attrib_name, difficulty, award_diff:="", advance_flags:=ADV_ALL )

	if( mobile.dead )
		SendSysMessage( mobile, "You can't use skills while dead." );
		return -100;
	elseif( GetObjProperty( mobile, "NoGains" ))
		SendSysMessage( mobile, "Your skills are disabled." );
		return -100;
	elseif( mobile.IsA( POLCLASS_NPC ))
                if( GetObjProperty( mobile, "CanTrain" ))
			advance_flags := ADV_STATS;
                else
			advance_flags := ADV_DISABLE;
                endif
	elseif( mobile.cmdlevel )
		advance_flags := ADV_DISABLE;
        elseif( A_IsIn( mobile, AREAS_NO_GAINS ))
		advance_flags := ADV_DISABLE;
	endif

	// Difficulty checks
	if( difficulty != -1 )
		difficulty := CDbl( difficulty );
	endif

        var effective_skill := AP_GetSkill( mobile, attrib_name ),
	    pass_chance := CalcSuccessPercent( mobile, attrib_name, difficulty, effective_skill ),
	    check_roll := RandomDiceRoll( "1d100+1" );

        if( !mobile.IsA( POLCLASS_NPC ) && !mobile.cmdlevel )
                var hunger := CInt( AP_GetVital( mobile, HUNGER ));
                if( !hunger )
                        hunger := 1;
                endif

		var hc_settings := AP_GetSettingsCfgElem( "Hunger" ),
                    hunger_setting := CInt( hc_settings.MaxHungerAffect ),
                    hunger_value := CInt( pass_chance * CDbl( hunger_setting ));

                pass_chance -= CInt( hunger_value - CDbl( hunger_value * CDbl( hunger / 10 )));
        endif

	// Ensure skill- and stat-gain if it's a -1 skillcheck
	// Do this AFTER the CalcSuccessPercent() call so '-1' isn't messed up

	if( difficulty == -1 )
		difficulty := effective_skill;
	endif

	if( award_diff == "" )
		award_diff := AP_GetSkillDefaultAward( attrib_name );
	endif

	if( advance_flags & ADV_SKILL )
		CheckSkillAdvance( mobile, attrib_name, difficulty, CInt( award_diff ));
	endif

	if( advance_flags & ADV_STATS )
		CheckStatsForSkill( mobile, attrib_name );
	endif

	return (( pass_chance - check_roll ) + 1 );
endfunction

function CalcSuccessPercent( mobile, attrib_name, difficulty, effective_skill )

	var sc_settings := AP_GetSettingsCfgElem( "Skills" ),
            min_chance := CInt( sc_settings.MinChance ),
            max_chance := CInt( sc_settings.MaxChance ),
	    percent := CInt( 50.0 + (( effective_skill - difficulty ) * 1.5 )); // For every 5 points difference there will be a +-10 for percent

	if( difficulty == -1 )
		var neg1chance := CInt( sc_settings.MaxNeg1Chance ),
                    classe_skill := GetClasseIdForSkillName( attrib_name );
                
                if( GetObjProperty( mobile, classe_skill ))
			neg1chance += CInt( ClasseBonus( mobile, classe_skill ) * neg1chance / 10 );
                endif

		percent := CInt( neg1chance * effective_skill / 150 );
	endif

	if( percent < min_chance )
		percent := min_chance;
	elseif( percent > max_chance )
		percent := max_chance;
	endif

	return percent;
endfunction

function DifficultyTooEasy( effective_skill, difficulty )

	var sc_settings := AP_GetSettingsCfgElem( "Skills" );

        var max_diff := difficulty + CDbl( sc_settings.TooEasy );
        if( max_diff > effective_skill )
                return 0;
        endif

	return 1;
endfunction
