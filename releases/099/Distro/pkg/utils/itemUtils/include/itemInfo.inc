/* $Id: itemInfo.inc 755 2006-08-25 09:00:55Z austinheilman $
 *
 */
use uo;
use os;
use cfgfile;
use polsys;


/*
 * IU_GetTileInfo(graphic)
 *
 * Purpose
 * Retrieves information on a tile from ::tiles.cfg
 *
 * Parameters
 * graphic:	Graphic number of the tile to retrieve information for.
 *
 * Return value
 * Returns a config elem on success.
 * Returns an error on failure.
 *
 */
function IU_GetTileInfo(graphic)
	var config := ReadConfigFile("::tiles");
	if ( !config )
		return config; // Contains error
	endif

	return config[Hex(CInt(graphic))];
endfunction

