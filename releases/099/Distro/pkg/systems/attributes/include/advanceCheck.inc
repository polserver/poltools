/*
        Created by *Edwards

        2010-03-29
*/
use uo;
use os;
use util;
use math;

include ":attributes:attributes_ex";
include ":security:attributesReport";
include ":classes:classes";
include ":skilllocks:common";
include ":powerscrolls:mycaps";

function CheckStatsForSkill( mobile, skill_name )

	 foreach stat_entry in ( AP_GetSkillStats( skill_name ))
  
		var stat_name := CStr( _stat_entry_iter ),
		    check_chance := CInt( stat_entry[1] ),
                    stat_mult := RandomDiceRoll( CStr( stat_entry[2] ));
      
                var roll := RandomFloat( 100.0 );
		if( roll <= check_chance )
	 	 	 CheckStatAdvancement( mobile, stat_name, stat_mult );
                endif

		SleepMS(10);
	endforeach

	return 1;
endfunction

function CheckStatAdvancement( mobile, stat_name, stat_mult )

	var csa_settings := AP_GetSettingsCfgElem( "Stats" ),
	    stat_cap := AP_GetStatCap( stat_name ),
	    stat_value := AP_GetTrueStat( mobile, stat_name ),
	    stat_left := CDbl( stat_cap - stat_value ); // Points until it is capped

	stat_mult *= CDbl( csa_settings.GainSpeed );

        var level_diff := GetDifficultyStat( stat_value ),
	    chance := CDbl(( stat_left * stat_mult ) / 100 ),
            random := RandomFloat( level_diff );
        
	if( random <= chance )
 
		var advance := CDbl( 0.1 );
		if( advance >= stat_left )
			advance := stat_left;
		elseif( advance < 0.1 )
			advance := 0.1;
		endif

                var temp := CDbl( stat_value + advance );

                AP_SetTrueStat( mobile, stat_name, temp );
		GAIN_ReportText( mobile, "Advance "+stat_name+": "+temp, GAIN_REPORT_SYSLOG );
	endif

	return 1;
endfunction

function CheckSkillAdvance( mobile, attrib_name, difficulty, award_diff )

        var true_skill := AP_GetTrueSkill( mobile, attrib_name ),
            skill_value := AP_GetTrueSkill( mobile, attrib_name ),
            skill_id := AP_AttributeNameToSkillId( attrib_name ),
	    skill_lock_state := SLPH_GetLockState( mobile, skill_id );

	if( skill_lock_state == 2 )
		//Skill lock state is set to lock. Aborting.
		return 0;
	endif

        var csa_settings := AP_GetSettingsCfgElem( "Skills" ),
            skill_cap := AP_GetSkillCap( mobile, attrib_name );
     
        if( !DifficultyTooEasy( skill_value, difficulty ))
                award_diff *= CDbl( csa_settings.GainSpeed );
        else
                award_diff *= CDbl( 0.5 );
        endif
        
        var level_diff := GetDifficultySkill( true_skill ),
            limit_left := CDbl( skill_cap - true_skill ),
            skills_left := CDbl( 150.0 - true_skill ),
            random := CDbl( RandomFloat( level_diff )),
	    chance := CDbl(( skills_left * award_diff ) / 100 );
     
        if( chance < 0.5 )
                chance := 0.5;
        endif
        
        if( true_skill < CDbl( csa_settings.FreeGainUntil ))
		//Passed advance check.
	elseif( random > chance )
		//Failed advancement check. No advance.
		return 0;
	endif

	var advance := CDbl( 0.1 );
	if( true_skill < 35 )
		advance := CDbl((( 30 - true_skill ) / 2 ) * 0.1 );
	endif

	if( advance > limit_left )
		advance := limit_left;
	elseif( advance < 0.1 )
		advance := 0.1;
	endif
 
        if( skill_lock_state == 1 ) //Skill lock state is set to raise.
                var temp := CDbl( true_skill - advance );

                AP_SetTrueSkill( mobile, attrib_name, temp );
                GAIN_ReportText( mobile, "Decrease "+attrib_name+": "+temp, GAIN_REPORT_SYSLOG );
        else
                var temp := CDbl( true_skill + advance );

                AP_SetTrueSkill( mobile, attrib_name, temp );
                GAIN_ReportText( mobile, "Increase "+attrib_name+": "+temp, GAIN_REPORT_SYSLOG );
        endif
        
        return 1;
endfunction

function GetDifficultySkill( true_skill )

        var level_diff;
        
        if( true_skill < 70 )
                level_diff := 20.0;
        elseif( true_skill < 90 )
                level_diff := 40.0;
        elseif( true_skill < 110 )
                level_diff := 100.0;
        elseif( true_skill < 115 )
                level_diff := 300.0;
        elseif( true_skill < 120 )
                level_diff := 800.0;
        elseif( true_skill < 130 )
                level_diff := 1500.0;
        elseif( true_skill < 140 )
                level_diff := 3000.0;
        elseif( true_skill < 145 )
                level_diff := 6000.0;
        else
                level_diff := 20000.0;
        endif
        
        return CDbl( level_diff );
endfunction

function GetDifficultyStat( true_stat )

        var level_diff;

        if( true_stat < 100 )
                level_diff := 100.0;
        elseif( true_stat < 110 )
                level_diff := 300.0;
        elseif( true_stat < 120 )
                level_diff := 600.0;
        elseif( true_stat < 125 )
                level_diff := 1500.0;
        else
                level_diff := 5000.0;
        endif

        return CDbl( level_diff );
endfunction