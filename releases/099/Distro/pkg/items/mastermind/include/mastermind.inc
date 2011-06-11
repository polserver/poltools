/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2010-07-17
*/

include ":gumps:gumpprompt";

var stones := array{ 6249, 6250, 6251, 6252, 6253, 6254, 6255, 6256 };
var stands := array{"", "", "", "", ""};
var last := array{};
var enigma := array{};

function Mastermind( who, chest )

        enigma := GetObjProperty( chest, "Enigma" );

        while( chest && who.connected )
                if( !SendMasterMind( who, chest ))
                        break;
                endif
        endwhile
 
        return 1;
endfunction

function SendMasterMind( who, chest )

	var gump := GFCreateGump( 50, 50, 500, 400 );

	GFResizePic( gump, 0, 0, GFGetCfgConst( "Defaults", "BackGround" ), 500, 400 );
	GFResizePic( gump, 15, 15, GFGetCfgConst( "Defaults", "ForeGround" ), 470, 370 );

	GFGumpPic( gump, 30, 30, 103 );
	GFTextLine( gump, 100, 50, 53, "A" );
	GFTextLine( gump, 85, 70, 53, "Puzzle" );
	GFTextLine( gump, 90, 90, 53, "Lock" );
 
	var for_help := "Correctly choose the sequence of cylinders need to open the latch. Each
                        cylinder may be used only once.";
                        
	GFDrawWrappedText( gump, 240, 40, GFWordWrap( for_help, 250 ), 1720 );

	GFTextLine( gump, 205, 330, 37, "Attempt left: "+GetObjProperty( chest, "NumTries" ));

	GFAddButton( gump, 225, 350, 2130, 2129, GF_CLOSE_BTN, 999 );	//OK

	//---------------------------------------------------------------------

	GFResizePic( gump, 30, 150, 5100, 30, 30 );
	GFAddButton( gump, 70, 160, 5032, 5032, GF_CLOSE_BTN, 11 );	//button left
	GFTilePic( gump, 25, 160, 6249 );		//black

	GFResizePic( gump, 30, 200, 5100, 30, 30 );
	GFAddButton( gump, 70, 210, 5032, 5032, GF_CLOSE_BTN, 12 );	//button left
	GFTilePic( gump, 25, 210, 6250 );		//blue

	GFResizePic( gump, 30, 250, 5100, 30, 30 );
	GFAddButton( gump, 70, 260, 5032, 5032, GF_CLOSE_BTN, 13 );	//button left
	GFTilePic( gump, 25, 260, 6251 );		//green

	GFResizePic( gump, 30, 300, 5100, 30, 30 );
	GFAddButton( gump, 70, 310, 5032, 5032, GF_CLOSE_BTN, 14 );	//button left
	GFTilePic( gump, 25, 310, 6252 );		//orange

	//--------------------------------------------------------------------

	GFResizePic( gump, 440, 150, 5100, 30, 30 );
	GFAddButton( gump, 420, 160, 5032, 5032, GF_CLOSE_BTN, 15 );	//button right
	GFTilePic( gump, 435, 160, 6253 );		//purple

	GFResizePic( gump, 440, 200, 5100, 30, 30 );
	GFAddButton( gump, 420, 210, 5032, 5032, GF_CLOSE_BTN, 16 );	//button right
	GFTilePic( gump, 435, 210, 6254 );		//red

	GFResizePic( gump, 440, 250, 5100, 30, 30 );
	GFAddButton( gump, 420, 260, 5032, 5032, GF_CLOSE_BTN, 17 );	//button right
	GFTilePic( gump, 435, 260, 6255 );		//soft blue

	GFResizePic( gump, 440, 300, 5100, 30, 30 );
	GFAddButton( gump, 420, 310, 5032, 5032, GF_CLOSE_BTN, 18 );	//button right
	GFTilePic( gump, 435, 310, 6256 );		//yellow

	//---------------------------------------------- previus quess ------
	
	GFTextLine( gump, 120, 165, 188, "Thy previus quess: " );
	GFResizePic( gump, 260, 160, 5100, 120, 30 );

	if( last[1] )
		GFTilePic( gump, 260, 170, last[1] );
	endif
	if( last[2] )
		GFTilePic( gump, 280, 170, last[2] );
	endif
	if( last[3] )
		GFTilePic( gump, 300, 170, last[3] );
	endif
	if( last[4] )
		GFTilePic( gump, 320, 170, last[4] );
	endif
	if( last[5] )
		GFTilePic( gump, 340, 170, last[5] );
	endif

	//----------------------------------------- stands -----------

	GFTilePic( gump, 107, 230, 2818 );
	GFTilePic( gump, 109, 229, 2818 );
	GFTilePic( gump, 113, 229, 2816 );
	GFTilePic( gump, 110, 232, 2816 );
	GFTilePic( gump, 110, 230, 1310 );

	GFTilePic( gump, 167, 230, 2818 );
	GFTilePic( gump, 169, 229, 2818 );
	GFTilePic( gump, 173, 229, 2816 );
	GFTilePic( gump, 170, 232, 2816 );
	GFTilePic( gump, 170, 230, 1310 );

	GFTilePic( gump, 227, 230, 2818 );
	GFTilePic( gump, 229, 229, 2818 );
	GFTilePic( gump, 233, 229, 2816 );
	GFTilePic( gump, 230, 232, 2816 );
	GFTilePic( gump, 230, 230, 1310 );

	GFTilePic( gump, 287, 230, 2818 );
	GFTilePic( gump, 289, 229, 2818 );
	GFTilePic( gump, 293, 229, 2816 );
	GFTilePic( gump, 290, 232, 2816 );
	GFTilePic( gump, 290, 230, 1310 );

	GFTilePic( gump, 347, 230, 2818 );
	GFTilePic( gump, 349, 229, 2818 );
	GFTilePic( gump, 353, 229, 2816 );
	GFTilePic( gump, 350, 232, 2816 );
	GFTilePic( gump, 350, 230, 1310 );

	//------------------------------------------------- stones on stands
 
	if( stands[1] )
		GFTilePic( gump, 110, 245, stands[1] );
	endif
	if( stands[2] )
		GFTilePic( gump, 170, 245, stands[2] );
	endif
	if( stands[3] )
		GFTilePic( gump, 230, 245, stands[3] );
	endif
	if( stands[4] )
		GFTilePic( gump, 290, 245, stands[4] );
	endif
	if( stands[5] )
		GFTilePic( gump, 350, 245, stands[5] );
	endif

	// ---------------------------------------------------- radio under stands
 
	GFSetRadioGroup( gump, 1 );
	GFRadioButton( gump, 118, 295, 2151, 2154, 0, 101 );
	GFRadioButton( gump, 178, 295, 2151, 2154, 0, 102 );
	GFRadioButton( gump, 238, 295, 2151, 2154, 0, 103 );
	GFRadioButton( gump, 298, 295, 2151, 2154, 0, 104 );
	GFRadioButton( gump, 358, 295, 2151, 2154, 0, 105 );

	var Res := GFSendGump( who, gump );
	if( Res[0] < 1 )
		SendSysMessage( who, "Cancelled." );
		return 0;
	endif

	if( Res[0] > 10 && Res[0] < 20 )
		foreach key in ( Res.keys )
			if( key >= 101 && key <= 105 )
				stands[key-100] := stones[res[0]-10];
			endif
                        SleepMS(5);
		endforeach
	endif

	if( res[0] == 999 )
		if( stands[1] && stands[2] && stands[3] && stands[4] && stands[5] )
			if( CheckAnswer( who ))
                                chest.locked := 0;
                                PrintTextAbove( chest, "*Unlocked*" );
				return 0;
			endif

                        var chances := GetObjProperty( chest, "NumTries" );
                        chances -= 1;

                        SetObjProperty( chest, "NumTries", chances );
                        if( !chances )
                        	DestroyItem( chest );
                        endif
                        
			last := stands;
			stands := array{"", "", "", "", ""};
		endif
	endif
 
        return 1;
endfunction

function RandomSet( chest )

        var set := array{};

        var i;
        for( i:=1; i<=5; i+=1 )
                var entry := stones.RandomEntry();
                set[i] := entry;

                stones.Erase( entry );
                SleepMS(5);
        endfor

        SetObjProperty( chest, "Enigma", set );

	return 1;
endfunction

function CheckAnswer( who )

	var x;
	var hits := 0;
	var colors := 0;
	var desc;
	var desc2;
	var EnigmaTemp := enigma;
	var StandsTemp := stands;

	for( x:=1; x<=5; x+=1 )
		if( StandsTemp[x] )
			if( StandsTemp[x] == EnigmaTemp[x] )
				hits += 1;
				EnigmaTemp[x] := -10;
				StandsTemp[x] := -20;
			endif
		endif
                SleepMS(5);
	endfor

	for( x:=1; x<=5; x+=1 )
		if( StandsTemp[x] in EnigmaTemp )
			colors += 1;
			EnigmaTemp[FindInArray( EnigmaTemp, StandsTemp[x] )] := -10;
			StandsTemp[x] := -20;
		endif
                SleepMS(5);
	endfor

	case( hits )
		0: 		desc := "0 element on correct position.";
		1: 		desc := "1 element is on correct position.";
		default:	desc := CInt( hits )+" elements are on correct position.";
	endcase

	case( colors )
		0: 		desc2 := "0 element in correct color.";
		1: 		desc2 := "1 element is correct color.";
		default:	desc2 := CInt( colors )+" elements are in correct color.";
	endcase
	
	if( hits == 5 )
		return 1;
        endif
        
        GumpPrompt( who, desc+" "+desc2 );
	
	return 0;
endfunction
