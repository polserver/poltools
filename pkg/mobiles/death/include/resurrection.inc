/*
 * $Id$
 *
 */

use uo;

include ":gumps:gumps";
include "include/client";
include "include/sounds";

function ResurrectionGump(mobile)
	var gump := GFCreateGump();
	
	var input := GFSendGump(mobile, gump);
	return input[0];
endfunction

function Resurrection(mobile)
	if ( !mobile.IsA(POLCLASS_MOBILE) )
		return error{"errortext":="object is not a mobile."};
	elseif ( !mobile.dead )
		return error{"errortext":="mobile is not dead."};
	endif
	
	mobile.graphic := mobile.trueobjtype;
	PlaySoundEffect(mobile, SFX_RESURRECT);
	PlayObjectCenteredEffect(mobile, GFX_BLUE_SPARKLES, 10,10);
	return Resurrect(mobile);
endfunction

	