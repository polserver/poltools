/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-09-06
*/
use uo;
use os;

include ":gumps:gumps";
include ":gumps:gumps_ex";
include "include/security";

function doomWarning( mobile )

        var gump := GFCreateGump();

        GFClosable( gump, 1 );
        GFMovable( gump, 0 );

        //Load Main Background
	GFResizePic( gump, 0, 0, GFGetCfgConst( "Defaults", "BackGround" ), 350, 325 );

        GFResizePic( gump, 15, 15, GFGetCfgConst( "Defaults", "ForeGround" ), 320, 25 );
	GFTextMid( gump, 15, 17, 320, 1720, "Doom Dungeon" );

        //BackgroundAltar
	GFResizePic( gump, 15, 45, GFGetCfgConst( "Defaults", "ForeGround" ), 320, 270 );
        GFTextMid( gump, 15, 60, 320, 1720, "Main Behavior" );

        //Okay Button
        GFAddButton( gump, 150, 282, 2128, 2129, GF_CLOSE_BTN, 0xA00 );

        //Initialize the text...
        var text := "You are enterring Doom Dungeon. If you are unfamiliar with fighting the most powerfull
                    monsters of Fantasia Shard nor are uneasy about losing your stuff, then you are advised
                    against proceeding. Gate & Recall are magically removed while inside Doom Dungeon.";

	GFDrawWrappedText( gump, 60, 85, GFWordWrap( text, 250 ));

        //And now we need to know the answer...
        var input := GFSendGump( mobile, gump );
        if( input[0xA00] )
                 SendSysMessage( mobile, "You accepted to enter Doom Dungeon. Goodluck brave traveler!", 3, 33 );
                 return 1;
        endif

	return 0;
endfunction
