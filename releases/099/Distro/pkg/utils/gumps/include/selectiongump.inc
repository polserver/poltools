/*
        Maintened by *Edwards
        
        2009-12-12
*/
use uo;
use os;

include ":gumps:gumps";
include ":gumps:gumps_ex";

CONST BTN_TYPE_RADIO	:= 0x1;
CONST BTN_TYPE_CHECKBOX	:= 0x2;
CONST BTN_TYPE_NORMAL	:= 0x4;
CONST BTN_NOTYPE	:= 0x8;

CONST OKAY_BTN		:= 0xA00;

function SelectionGump( mobile, title, options, btn_type:=BTN_TYPE_NORMAL )

	if( Lower( TypeOf( options )) != "array" )
		return error{"errortext":="Options list must be an array."};
	endif

	var s_gump := GFCreateGump();

	GFResizePic( s_gump, 0, 0, GFGetCfgConst( "Defaults", "BackGround" ), 310, 480 );
	GFResizePic( s_gump, 15, 15, GFGetCfgConst( "Defaults", "ForeGround" ), 280, 450 );

        GFAddAlphaRegion( s_gump, 0, 0, 310, 480 );
        
	GFTextMid( s_gump, 15, 20, 280, 1720, title );
	GFAddButton( s_gump, 135, 440, 2128, 2129, GF_CLOSE_BTN, OKAY_BTN );

	GFPage( s_gump, 1 );
	var num_options := options.Size();
	var y_pos := 60;
	foreach entry in ( options )
		case( btn_type )
			BTN_TYPE_NORMAL:
				GFAddButton( s_gump, 30, y_pos+3, 2117, 2118, GF_CLOSE_BTN, _entry_iter );
				break;
			BTN_TYPE_CHECKBOX:
				GFCheckBox( s_gump, 30, y_pos, 210, 211, 0, _entry_iter );
				break;
			BTN_TYPE_RADIO:
				GFRadioButton( s_gump, 30, y_pos, 210, 211, 0, _entry_iter );
				break;
		endcase
  
		GFTextLine( s_gump, 60, y_pos, 2100, entry );
		y_pos := y_pos+20;

		if( _entry_iter % 19 == 0 && _entry_iter < num_options )
			GFAddButton( s_gump, 283, 405, 0xA92, 0xA93, GF_PAGE_BTN, s_gump.cur_page+1 );
			GFPage( s_gump, s_gump.cur_page+1 );
			GFAddButton( s_gump, 283, 45, 0xA90, 0xA91, GF_PAGE_BTN, s_gump.cur_page-1 );
			y_pos := 60;
		endif


		SleepMS(2);
	endforeach

	var input := GFSendGump( mobile, s_gump );
	if( !input[0] )
		return error{"errortext":="No option was selected."};
	endif

	case( btn_type )
		BTN_TYPE_NORMAL:
			var val := input[0];
			return options[val];
		BTN_TYPE_CHECKBOX:
			var values := array;
			foreach key in ( input.keys )
				if( key >= 1 && key <= num_options )
					values.Append( options[key] );
				endif
				SleepMS(2);
			endforeach
			return values;
		BTN_TYPE_RADIO:
			foreach key in ( input.keys )
				if ( key >= 1 && key <= num_options )
					return options[key];
				endif
				SleepMS(2);
			endforeach
	endcase
endfunction
