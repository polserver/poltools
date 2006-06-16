/*
 * $Id: titles.inc 128 2006-05-27 17:36:42Z austinheilman $
 *
 */

use uo;
use os;
use cfgfile;

include ":attributes:attributes";

/*
 * AP_GetLevelTitle(skill_level)
 *
 * Purpose
 * Gets the title for a particlar skill level.
 *
 * Parameters
 * skill_level:	Skill level of a skill. (e.g. 80% = 80)
 *
 * Return value
 * Returns a string on success.
 * Returns an error on failure.
 *
 */
function AP_GetLevelTitle(skill_level)
	skill_level := CDbl(skill_level);
	
	if ( skill_level < 0.0 )
		return "U SUK";
	endif
	
	var title_cfg := ReadConfigFile(":attributes:config/titles.cfg");
	if ( !title_cfg )
		return title_cfg;
	endif
	
	var title_elem := title_cfg["LevelTitles"];
	if ( !title_elem )
		return title_elem;
	endif
	
	var levels_list := GetConfigStringDictionary(title_elem, "Level");
	
	foreach entry in ( levels_list )
		if ( skill_level < CDbl(entry) )
			return _entry_iter;
		endif
		SleepMS(2);
	endforeach
	
	return error{"errortext":="Could not find skill title for that level."};
endfunction

/*
 * AP_GetSkillTitle(skill_name)
 * 
 * Purpose
 * Finds the title name of a skill.
 *
 * Parameters
 * skill_name:	Name of a skill from attributes.cfg
 *
 * Return value
 * Returns a string on success.
 * Returns an error on failure.
 *
 */
function AP_GetSkillTitle(skill_name)
	var elem_info := AP_GetAttributeCfgElem(skill_name);
	if ( !elem_info )
		return elem_info;
	endif
	
	if ( elem_info.Title) 
		return elem_info.Title;
	else
		return error{"errortext":="Attribute does not have a 'Title' entry."};
	endif
endfunction