/* $Id$ */

use uo;
use os;
use cfgfile;

include ":containers:containers";
include ":itemUtils:itemdesc";

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

/* GetCraftablesByTool(tool)
 *
 * Returns.. something, haven't decided yet!
 *
 * Parameters
 * tool: 	Item reference or object type of the tool being used.
 *
 * Return Value:
 * Possibly an array of config elem references?
 *
 */
function GetToolUses(tool)
	var search;
	
	if ( tool.IsA(POLCLASS_ITEM) )
		search := Lower("Tool=" + Hex(tool.objtype));
	else
		search := Lower("Tool=" + Hex(CInt(tool)));
	endif
	
	var size := Len(search);
	
	var cfg := ReadConfigFile(":*:toolOnMaterial");
	
	var elem_list := array{};
	foreach elem_name in ( GetConfigStringKeys(cfg) )
		if ( Lower(elem_name) == search )
			elem_list.Append(cfg[elem_name]);
		endif
		SleepMS(2);
	endforeach

	return elem_list;
endfunction

/* ShowCraftablesMenu(mobile, cur_menu_elem, level, material.color);
 *
 * Receives an elem containing submenu properties and craft item properties.
 * Displays them using the left-right slider menu in the UO client.
 *
 * Parameters
 * mobile: 	Player that will be interacting with the menu.
 * menu_elem:	Config elem that will control the options in the menu.
 * depth:	>1 will have a back button.
 * color:	Optional argument to make the items (not submenus) colored.
 *
 * Return Value:
 * Returns 0 if the menu was not displayed or was cancelled.
 * Returns a struct containing either a SubMenu property or an objecttype to craft.
 *
 */
function ShowCraftablesMenu(mobile, menu_elem, depth, color:=0)
	var menu_selection := struct;
	menu_selection.+objtype;
	menu_selection.+SubMenu;
	
	var menu_options := array;
	
	var menu_obj := CreateMenu("What would you like to make?");
	
	if ( depth > 1 )
		menu_selection.objtype := 0;
		menu_selection.SubMenu := -1;
		menu_options.Append(menu_selection);
		
		AddMenuItem(menu_obj, 8304, "^- Back");
		menu_selection.objtype := 0;
		menu_selection.SubMenu := 0;
	endif
	
	var menu_list := GetConfigStringArray(menu_elem, "SubMenu");
	foreach submenu in ( menu_list )
		var line := SplitWords(submenu);
		var graphic := GetObjTypeGraphic(CInt(line[2]));

		var item_name := line[3];
		while( item_name["_"] )
			item_name["_"] := " ";
		endwhile

		menu_selection.SubMenu := line[1];
		menu_options.Append(menu_selection);
		menu_selection.objtype := 0;
		menu_selection.SubMenu := 0;

		AddMenuItem(menu_obj, graphic, item_name+" (submenu)");
		sleepms(2);
	endforeach
	
	var item_list := GetConfigStringArray(menu_elem, "Entry");
	foreach entry in ( item_list )
		var line := SplitWords(entry);
		
		var graphic := GetObjTypeGraphic(CInt(line[1]));
		
		var item_name := line[2];
		while( item_name["_"] )
			item_name["_"] := " ";
		endwhile
		
		menu_selection.SubMenu := line[1];
		menu_options.Append(menu_selection);
		menu_selection.objtype := 0;
		menu_selection.SubMenu := 0;

		AddMenuItem(menu_obj, graphic, item_name, color);
		sleepms(2);
		
		sleepms(2);
	endforeach
	
	if ( (menu_list.size() < 1) && (item_list.size() < 1) )
		SendSysMessage(mobile, "There are no submenus or items listed here.");
		return 0;
	endif
	
	var input := SelectMenuItem2(mobile, menu_obj);
	return menu_options[input.index];
endfunction
