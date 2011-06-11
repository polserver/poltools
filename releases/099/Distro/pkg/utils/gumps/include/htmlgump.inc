/*
        Maintened by *Edwards
        
        2009-10-30
*/
use uo;
use os;

include ":gumps:gumps";
include ":gumps:gumps_ex";

function SendHTMLGump( mobile, title, text )

	var gump := GFCreateGump( 220, 20 );
	GFResizePic( gump, 0, 0, GFGetCfgConst( "Defaults", "BackGround" ), 360, 480 );
	GFResizePic( gump, 15, 12, GFGetCfgConst( "Defaults", "ForeGround" ), 330, 30 );
	GFResizePic( gump, 15, 438, GFGetCfgConst("Defaults", "ForeGround" ), 330, 30 );
	GFResizePic( gump, 15, 45, GFGetCfgConst("Backgrounds", "GOLDTRIM" ), 330, 390 );
 
        GFAddAlphaRegion( gump, 0, 0, 360, 480 );
        
	GFTextMid( gump, 15, 17, 370, 1720, title );

	GFResizePic( gump, 15, 45, GFGetCfgConst("Backgrounds", "GOLDTRIM" ), 330, 390 );
	GFHTMLArea( gump, 22, 53, 322, 375, text, 0, 1 );
	
	GFAddButton( gump, 150, 443, 2128, 2129, GF_CLOSE_BTN, 0XA1 );
	
	return GFSendGump( mobile, gump );
endfunction
