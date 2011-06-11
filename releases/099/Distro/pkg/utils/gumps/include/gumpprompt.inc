/*
        Maintened by *Edwards
        
        Last Modifications 2009-10-28
             * Cleaned codes for better performances
             * Introduced the resolution option to get screen in the middle
*/
use uo;
use os;

include ":gumps:gumps";
include ":gumps:gumps_ex";

function GumpPrompt( mobile, text, movable:=0 )

	var gump_prompt := GFCreateGump();
 
	GFClosable( gump_prompt, 0 );
	if( !movable )
		GFMovable( gump_prompt, 0 );
	endif
        
	GFResizePic( gump_prompt, 180, 10, GFGetCfgConst( "Defaults", "BackGround" ), 300, 160 );
	GFResizePic( gump_prompt, 195, 25, GFGetCfgConst( "Defaults", "ForeGround" ), 270, 130 );
 
        GFAddAlphaRegion( gump_prompt, 180, 10, 300, 160 );
		
	var y_pos := 35;
	foreach line in ( GFWordWrap( text, 260 ))
		GFTextLine( gump_prompt, 220, y_pos, 1153, line );
		y_pos += 20;
		SleepMS(2);
	endforeach
	
	GFAddButton( gump_prompt, 300, 120, 2128, 2129, GF_CLOSE_BTN, 1 );
	
	var prompt_input := GFSendGump( mobile, gump_prompt );
 
	return prompt_input[0];
endfunction
