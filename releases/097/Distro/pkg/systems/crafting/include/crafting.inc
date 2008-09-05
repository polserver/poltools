/* $Id$ */

use uo;
use os;
use cfgfile;

include ":containers:containers";

/* GetToolOnMaterialUses(tool, material)
 *
 * Retrieves an config elem of what can be crafted with a combination of a tool on a material.
 *
 * Parameters
 * tool: 	Item that the crafting use script is bound to.
 * material:	Item that the user selected to craft with.
 *
 * Return Value:
 * Returns a config elem on success.
 * Returns 0 if nothing can be crafted with the combination used.
 *
 */
function GetToolOnMaterialUses(tool, material)
	// Tool=0x1234&Material=0x5678
	// {
	//	SubMenu ... (another menu of stuff to make)
	//	Entry	objtype	desc
	// }
	var tool_on_material_objtype := CStr("Tool=" + Hex(tool.objtype) + "&Material=" + Hex(material.objtype));
	var tool_on_material_graphic := CStr("Tool=" + Hex(tool.objtype) + "&Material=" + Hex(material.graphic));
	var cfg := ReadConfigFile(":*:toolOnMaterial");
	var elem := cfg[tool_on_material_objtype];
	if ( !elem )
		elem := cfg[tool_on_material_graphic];
		if ( !elem )
			return 0;
		endif
	endif

	return elem;
endfunction


