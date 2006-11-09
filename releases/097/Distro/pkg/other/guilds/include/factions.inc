/* $Id: ranks.inc 898 2006-10-31 02:50:49Z MontuZ $
 *	  Author: MontuZ
 *	  Created: Wednesday, May 24 2006
 *	  Version: 0.02
 */

use uo;

include	":guilds:settings";

function IsOpposingFaction(chr_guild, plyr_guild)

	var chr_faction := chr_guild.GetProp("Faction");
	var plyr_faction := plyr_guild.GetProp("Faction");

	if ( chr_faction == "Order" )
		if ( plyr_faction == "Chaos" )
			return 1;
		else
			return 0;
		endif
	elseif( chr_faction == "Chaos" )
		if ( plyr_faction == "Order" )
			return 1;
		else
			return 0;
		endif
	elseif( chr_faction == "Neutral" )
		if ( plyr_faction == "Neutral" )
			return 0;
		else
			return 1;
		endif
	endif

	return 1;

endfunction