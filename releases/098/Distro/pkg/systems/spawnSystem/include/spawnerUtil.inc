/* $Id$
 *
 */
use uo;
use os;
use util;

include ":datafile:datafile";

/*
 * SS_SelectCoordsInArea(x1, y1, x2, y2)
 *
 * Purpose
 * Takes in four numbers (two coordinates) and
 * picks a random coordinate within the area they create.
 *
 * Parameters
 * x1:	The top-left spot of the X axis.
 * y1:	The top-left spot of the Y axis.
 * x2:	The bottom-right spot of the X axis.
 * y2:	The bottom-right spot of the Y axis.
 *
 * Return value
 * A structure containing an X and Y property.
 *
 */
function SS_SelectCoordsInArea(x1, y1, x2, y2)
	var coord := struct;
	coord.+x := RandomInt(x2-x1)+x1;
	coord.+y := RandomInt(y2-y1)+y1;

	return coord;
endfunction

/*
 * SS_IsValidCreationTile(x, y, z, realm)
 *
 * Determines if items and npcs can exist on an X Y Z tile.
 *
 * Parameters
 * x:		X coordinate
 * y:		Y coordinate
 * z:		Z coordinate
 * realm:	Realm name
 *
 * Return Value
 * Returns 1 if it is valid.
 * Returns error if it is not.
 *
 */
function SS_IsValidCreationTile(x, y, z, realm)
	var landtiles_cfg := ReadConfigFile("::landtiles");

	var map_info := GetMapInfo(x, y, realm);
	var tile_elem := landtiles_cfg[map_info.land_tile];
	if ( !tile_elem.MoveLand )
		// Okay the landscape tile isnt valid to put something on...
		// Check if there is a valid game tile at the Z position.
		var tiles_cfg := ReadConfigFile("::tiles");
		// First check statics (most likely)
		foreach static in ( ListStaticsAtLocation(x, y, z, realm) )
			if ( static.z == z )
				if ( tiles_cfg[static.objtype].MoveLand )
					return 1;
				endif
			endif
			SleepMS(2);
		endforeach
		// Check for normal items
		foreach item in ( ListItemsAtLocation(x, y, z, realm) )
			if ( item.z == z )
				if ( tiles_cfg[item.graphic].MoveLand )
					return 1;
				endif
			endif
			SleepMS(2);
		endforeach

		// If it got here, nothing valid was found.
		return 0;
	endif

	return 1;
endfunction
