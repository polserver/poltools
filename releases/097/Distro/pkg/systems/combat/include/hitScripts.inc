/* $Id$
 *
 */

use uo;
use os;

include ":brainai:npcUtil";

/* GetWeaponHitScripts(object)
 *
 * Retrieves an array of all hit scripts that the object has when it hits.
 *
 * Parameters
 * object: 	Object to get the weapon hit scripts of.
 * 		Will read from a cprop array called "HitScripts"
 *		If the object is an NPC it will also get any WeaponHitScript lines from its config elem.
 *
 * Return Value:
 * Returns an array.
 *
 */
function GetWeaponHitScripts(object)
	var hit_scripts := GetObjProperty(object, "HitScripts");
	if ( TypeOf(hit_scripts) != "array" )
		hit_scripts := array;
	endif

	if ( object.IsA(POLCLASS_NPC) )
		var cfg_elem := NPC_GetNPCConfig(object);
		var temp := GetConfigStringArray(cfg_elem, "WeaponHitScript");
		if ( temp.Size() > 0 )
			hit_scripts += temp;
		endif
	endif

	return hit_scripts;
endfunction

/* GetArmorHitScripts(object)
 *
 * Retrieves an array of all hit scripts that the object has when it hits.
 *
 * Parameters
 * object: 	Object to get the armor hit scripts of.
 * 		Will read from a cprop array called "ArmorHitScripts"
 *		If the object is an NPC it will also get any ArmorHitScript lines from its config elem.
 *
 * Return Value:
 * Returns an array.
 *
 */
function GetArmorHitScripts(object)
	var hit_scripts := GetObjProperty(object, "ArmorHitScripts");
	if ( TypeOf(hit_scripts) != "array" )
		hit_scripts := array;
	endif

	if ( object.IsA(POLCLASS_NPC) )
		var cfg_elem := NPC_GetNPCConfig(object);
		var temp := GetConfigStringArray(cfg_elem, "ArmorHitScript");
		if ( temp.Size() > 0 )
			hit_scripts += temp;
		endif
	endif

	return hit_scripts;
endfunction
