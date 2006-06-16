/* $Id: spells_ex.inc 85 2006-05-23 18:41:38Z sroyalty $
 *
 */

use uo;
use os;
use cfgfile;

include ":magery:spells";

/*
 * MS_SpellNameToId(spell_name)
 *
 * Purpose
 * Converts the name of a spell to its ID number.
 *
 * Parameters
 * spell_name:	Name of the spell to convert
 *
 * Return value
 * Returns an integer if a spell Id was found.
 * Returns 0 if the spell name is invalid.
 *
 */
  
function MS_SpellNameToId(spell_name)
	var spellex_cfg := ReadConfigFile(":magery:config/spells_ex");
	var cfg_elem := spellex_cfg["NameToId"];
	
	return GetConfigInt(cfg_elem, spell_name);
endfunction
