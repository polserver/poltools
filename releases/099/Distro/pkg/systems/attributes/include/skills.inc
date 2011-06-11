/*
        Maintened by *Edwards

        2009-02-09
*/
use uo;

function AP_GetSkill( who, skill_name )

	return AP_GetTrueSkill( who, skill_name )+AP_GetSkillMod( who, skill_name );
endfunction

function AP_GetTrueSkill( who, skill_name )

	return CDbl( GetAttributeBaseValue( who, skill_name )) / 10.0;
endfunction

function AP_GetSkillMod( who, skill_name )

	return CDbl( GetAttributeTemporaryMod( who, skill_name )) / 10.0;
endfunction

function AP_SetTrueSkill( mobile, skill_name, points )

        var result;
	if( points >= 0 )
		result := SetAttributeBaseValue( mobile, skill_name, CInt( CDbl( points ) * 10.0 + 0.1 ));
	else
		result := SetAttributeBaseValue( mobile, skill_name, CInt( CDbl( points ) * 10.0 - 0.1 ));
	endif
 
	RecalcVitals( mobile );
	return result;
endfunction

function AP_SetSkillMod( mobile, skill_name, points )

	var result := SetAttributeTemporaryMod( mobile, skill_name, CInt( CDbl( points ) * 10 ));
	RecalcVitals( mobile );
	return result;
endfunction

function AP_ModifySkillMod( mobile, stat_name, points )

	var orig_mod := AP_GetStatMod( mobile, stat_name );
	var new_mod := CDbl( points ) + CDbl( orig_mod );

	AP_SetSkillMod( mobile, stat_name, new_mod );

	return orig_mod;
endfunction

function AP_GetSkillScript( attribute_name )

	var cfg := AP_GetAttributeCfgElem( attribute_name );
	return cfg.Script;
endfunction

function AP_GetSkillStats( attribute_name )

	var cfg_elem := AP_GetAttributeCfgElem( attribute_name );
	var stat_entries := GetConfigStringDictionary( cfg_elem, "StatCheck" );
	foreach textline in ( stat_entries )
		textline := SplitWords( textline );
		SleepMS(2);
	endforeach

	return stat_entries;
endfunction

function AP_GetSkillDefaultAward( skill_name )

	var elem_info := AP_GetAttributeCfgElem( skill_name );
	if( !elem_info )
		return elem_info;
	endif

	if( elem_info.Title )
		return CInt( elem_info.AwardDefault / 5 );
	else
		return error{"errortext":="Attribute does not have a 'AwardDefault' entry."};
	endif
endfunction
