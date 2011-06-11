/*
        Maintened by *Edwards
        
        2008-12-16
        
        Last Modifications 2008-12-17
             * GFCraftInfoPage() can display colored elem
                    Note: if oldclient -> image won't display
                    Has been removed for above reason

        Last Modifications 2008-12-19
             * Modified CProp "Craft" for "#Craft"

        Last Modifications 2008-12-27
             * Removed ''Apply Color'' option
             * GFCraftInfoPage() can display colored elem again!
             * Modified ExceptionalCraft() parms to fit with our "ZuluHotel" system
             * Every kind of bonus will check for craft_skill classe of character

        Last Modifications 2009-08-15
             * Fixed bug with pages. Items listed where not in the good menu or at the good position.

        Last Modifications 2009-11-03
             * Cleaned all codes for better performances..
             * Rebuilt all gump support

        Last Modifications 2009-11-16
             * Fixed size of HTML area that were too large outside the gump itself

        Last Modifications 2009-12-08
             * Added tooltip from skill.cfg for custom informations.
             * Added autoloop functions
*/
use uo;
use os;

include ":autoloop:autoloop";
include ":gumps:gumps";
include ":gumps:gumps_ex";
include ":itemutils:itemdesc";

var menu_config := ReadConfigFile( ":crafting:craftConfig" );

var arinfo := array{},
    arcnt := 61500,
    craft_settings,
    atc := 1,
    PageNum := 1;

function sendCraftGump( who, skill_name )

        if( GetProcess( CInt( GetObjProperty( who, "#CraftingPid" ))))
                //Already crafting.
		return 0;
	else
		SetObjProperty( who, "#CraftingPid", GetPid() );
	endif

	var menu_file  := menu_config[skill_name].MenuFile,
	    skill_config := ReadConfigFile( menu_file );
     
	craft_skill  := menu_config[skill_name].Attribute;
	display_skill := menu_config[skill_name].Name;
 
	// Craft Cprop is a dictionary storing the information for each crafting skill
	craft_settings := GetObjProperty( who, "#Craft" );
	if( TypeOf( craft_settings ) != "Dictionary" )
		craft_settings := cBaseCraftDict();
		SetObjProperty( who, "#Craft", craft_settings );
	endif

        while( who.connected )
		craftL := craft_settings[skill_name].last;
		rcolor := craft_settings[skill_name].retain;
		makermark := craft_settings[skill_name].mark;

		var craft_gump := Craft_IndexGump( who, skill_name );
		if( !craft_gump )
			return 0;
		endif
  
		// Exit the menu, return 0 so crafting script knows it was canceled
	        var res := GFSendGump( who, craft_gump );
		if( res[0] == 1 )
			return 0;
		endif

		var the_objtype := res[0];

		// Handle setting up of Objtype if they pressed "Last Item". Returns with
		// an error if you haven't crafted anything with the skill yet.
		if( res[61450] )
			the_objtype := craftL;
			if( !the_objtype || ( TypeOf( the_objtype ) == "Struct" ))
				SendSysMessage( who, "You haven't made anything yet." );
				return 0;
			endif
		endif

		// See if the Maker's Mark button was pressed.
		if( res[61453] )
			case( makermark )
			       1: craft_settings[skill_name].mark := 0;
			               SetObjProperty( who, "#Craft", craft_settings );
			       default: craft_settings[skill_name].mark := 1;
			               SetObjProperty( who, "#Craft", craft_settings );
			endcase
			continue;
		endif

		// This decides if you pressed the Info button, if so, sends info gump
		if(( res[0] >= 61500 ) && ( res[0] <= 62000 ))
			the_objtype := arinfo[( res[0] - 61499 )];
			var infores := GFCraftInfoPage( who, the_objtype, skill_config );
			case( infores )
			        0:
			               continue;
			        default:
			               the_objtype := infores;
			endcase
		endif

		// Sets the objtype in the Last Item cprop. Redundant to ensure it sets.
		// If a button return (the_objtype) is between 0xF000 and 0xFFFF it is NOT
		// a craftable objtype anyway.
		craft_settings[skill_name].last := the_objtype;
		SetObjProperty( who, "#Craft", craft_settings );

		// Sets the amount you had entered into an array to return to crafting
                var max_amount := CInt( menu_config["Setup"].MaxAmount );
                
		atc := CInt( GFExtractData( res, atc ));
                if( atc == -1 )
			atc := -1;
		elseif( !atc || atc < 1 )
			atc := 1;
		elseif( atc > max_amount)
                        atc := max_amount;
		endif

                SetAutoLoop( who, atc );
		SetObjProperty( who, "#Craft", craft_settings );

		// Build the return array and give it to crafting.
		var minfo := array;
		minfo.Append( the_objtype );
		minfo.Append( atc );
		minfo.Append(( res[61451] )); //Repair
		minfo.Append(( res[61454] )); //Resmelt
		minfo.Append(( res[61452] )); //Fortify
  
		return minfo;
	endwhile

        return 1;
endfunction

function Craft_IndexGump( who, skill_name  )

	var menu_file    := menu_config[skill_name].MenuFile;
	var skill_config := ReadConfigFile( menu_file );

	// Now let's get the amount, based on the menu file from the Cprop.
	atc := GetAutoLoop( who );
 
	// If it == nothing, or is an error, set it to 1. Error should never occur, but, just in case.
	if( !atc || TypeOf( atc ) != "Integer" )
		atc := 1;
	endif
 
	// Done here, it resets the last item crafted when using the menu.
	rcolor := craft_settings[skill_name].retain;
	makermark := craft_settings[skill_name].mark;

	// Resets the array for objtypes within the menu. Used with
	// The info page and it's buttons.
	arinfo  := array;
 
	// Button counter for the Info buttons for items. This way, we can seperate craft now buttons
	// from the info buttons. Just remember to make sure any item's button # should NOT be within
	// the 61500 to 62000 range. If you have 50 items, button range will be 61500 to 61550. So please
	// take care with your items.
	arcnt := 61500;

	var gump := GFCreateGump( 50, 25 );
 
        GFClosable( gump, 0 );

        //Main Background
	GFResizePic( gump, 0, 0, GFGetCfgConst( "Defaults", "BackGround" ), 525, 440 );

        //Title Background
	GFResizePic( gump, 15, 15, GFGetCfgConst( "Defaults", "ForeGround" ), 497, 25 );
	GFHTMLArea( gump, 10, 17, 510, 20, "<basefont color=#E59700>"+CStr( GetConfigString( skill_config["Index"], "Title" )));

        //SubTitles Background
 
        //Categories Background
	GFResizePic( gump, 15, 45, GFGetCfgConst( "Defaults", "ForeGround" ), 197, 280 );
	GFHTMLArea( gump, 10, 45, 200, 20, "<basefont color=#E59700><center>Categories</center>" );

        //Selections Background
	GFResizePic( gump, 215, 45, GFGetCfgConst( "Defaults", "ForeGround" ), 295, 280 );
	GFHTMLArea( gump, 215, 45, 305, 20, "<basefont color=#E59700><center>Selections</center>" );
 
        //Bottom Background
	GFResizePic( gump, 15, 300, GFGetCfgConst( "Defaults", "ForeGround" ), 497, 125 );
 
        //Notice Section
	GFResizePic( gump, 18, 288, GFGetCfgConst( "BackGrounds", "GOLDTRIM" ), 490, 50 );
 
	GFHTMLArea( gump, 65, 395, 50, 18, "<basefont color=#E59700>Exit" );
	GFAddButton( gump, 30, 392, 4017, 4019, GF_CLOSE_BTN, 1 );

	if( menu_config["Setup"].MakeLast == 1 )
		GFHTMLArea( gump, 430, 350, 70, 18, "<basefont color=#E59700>Make Last" );
		GFAddButton( gump, 395, 347, 4005, 4007, GF_CLOSE_BTN, 61450 );
	endif

	GFHTMLArea( gump, 395, 395, 50, 20, "<basefont color=#E59700>Amount" );
	GFGumpPic( gump, 440, 392, 2443 );
	atc := GFTextEntry( gump, 450, 395, 100, 30, 1152, atc );

	if( menu_config[skill_name].MakersMark == 1 )
		if( makermark == 1 )
			GFHTMLArea( gump, 250, 350, 100, 18, "<basefont color=#E59700>Mark Item" );
		else
			GFHTMLArea( gump, 250, 350, 100, 18, "<basefont color=#E59700>*Do Not Mark" );
		endif
		GFAddButton( gump, 215, 347, 4005, 4007, GF_CLOSE_BTN, 61453 );
	endif
 
	if( menu_config[skill_name].Repair == 1 )
		GFHTMLArea( gump, 250, 370, 100, 18, "<basefont color=#E59700>Repair Item" );
		GFAddButton( gump, 215, 367, 4005, 4007, GF_CLOSE_BTN, 61451 );
	endif
 
	if( menu_config["Setup"].Resmelt == 1 )
		if( menu_config[skill_name].Smelt == 1 )
			GFHTMLArea( gump, 250, 390, 100, 18, "<basefont color=#E59700>Resmelt Item" );
			GFAddButton( gump, 215, 387, 4005, 4007, GF_CLOSE_BTN, 61454 );
		endif
	endif

        //The next check assume no smelt option returns true
	if( menu_config[skill_name].Fortify == 1 )
		GFHTMLArea( gump, 250, 390, 100, 18, "<basefont color=#E59700>Fortify Item" );
		GFAddButton( gump, 215, 387, 4005, 4007, GF_CLOSE_BTN, 61452 );
	endif

	// This foreach builds all the pages for Categories and Selections
	// IndexY = the Y position for lines in the Categories.
	var IndexY := 80;
 
	// Page Number counter to keep track of pages while building the gump
	PageNum := 1;
 
	foreach MenuName in GetConfigStringArray( skill_config["Index"], "Group" )
 
		// Add the Category to page 0 for background
		GFPage( gump, "0" );
		GFAddButton( gump, 30, ( IndexY-3 ), 4005, 4007, GF_PAGE_BTN, PageNum );
  
		while( MenuName["_"] )
			MenuName["_"] := " ";
			SleepMS(1);
		endwhile
  
		GFHTMLArea( gump, 65, IndexY, 140, 18, ( "<basefont color=#ffffff>"+MenuName ));
  
		// Now let's build the page for this Category
		GFPage( gump, PageNum );
		GFCraftNewPage( who, gump, skill_config, MenuName );
  
		// Cycle all the variables up accordingly
		if( PageNum > 150 )
			SendSysMessage( who, "Error! Max Amount of Menus Exceeded. Please report to shard staff." );
			return 0;
		endif
  
		IndexY += 20;
		PageNum += 5;
                SleepMS(5);
	endforeach

	return gump;
endfunction

function GFCraftNewPage( who, byref gump, skill_config, MenuName )

	var ItemX    := 270;
	var ButtonX  := 235;
	var ItemY    := 80;
	var ButtonX2 := 470;
 
	foreach item in GetConfigStringArray( skill_config[MenuName], "Item" )

		var objtype := GetObjTypeByName( item );
		if( TypeOf( objtype ) == "Struct" )
			continue;
		endif

		var itemdesc := GetObjTypeDesc( objtype );
		if( TypeOf( itemdesc ) == "Struct" )
			continue;
		endif
  
		if( ItemY >= 243 )
			ItemY := 80;
			GFAddButton( gump, 370, 260, 4005, 4007, GF_PAGE_BTN, ( PageNum+1 ));
			GFHTMLArea( gump, 405, 263, 100, 18, "<basefont color=#E59700>Next Page" );
			PageNum += 1;
			GFPage( gump, PageNum );
			GFAddButton( gump, 330, 260, 4014, 4015, GF_PAGE_BTN, ( PageNum-1 ));
			GFHTMLArea( gump, 240, 263, 100, 20, "<basefont color=#E59700>Previous Page" );
		endif
  
		GFHTMLArea( gump, ItemX, ItemY, 200, 18, ( "<basefont color=#ffffff>"+ Lower( itemdesc )));
		GFAddButton( gump, ButtonX, ( ItemY-3 ), 4005, 4007, GF_CLOSE_BTN, objtype );
  
		if( menu_config["Setup"].Info == 1 )
			GFAddButton( gump, ButtonX2, ( ItemY-3 ), 4011, 4012, GF_CLOSE_BTN, arcnt );
			arinfo.Append( objtype );
			arcnt += 1;
		endif

		// Cycle all the variables up accordingly
		if( _item_iter > 90 )
			SendSysMessage( who, "Error! Max Amount of items exceeded! Please report to shard staff." );
			return 0;
		endif
  
		ItemY += 20;
		SleepMS(5);
	endforeach

	return 1;
endfunction

function GFCraftInfoPage( who, objtype, skill_config )

	var i_config := ReadConfigFile( ":*:itemdesc" ),
	    thisitem := i_config[objtype],
            elem := craft_config[objtype];
     
	if( TypeOf( thisitem ) != "ConfigElemRef" )
		thisitem := i_config[( "0x0"+( CStr( objtype ) - "0x" ))];
	endif

	var gump := GFCreateGump( 50, 25 );

        GFClosable( gump, 0 );

        //Main Background
	GFResizePic( gump, 0, 0, GFGetCfgConst( "Defaults", "BackGround" ), 525, 440 );

        //Title Background
	GFResizePic( gump, 15, 15, GFGetCfgConst( "Defaults", "ForeGround" ), 497, 25 );
	GFHTMLArea( gump, 10, 17, 510, 20, "<basefont color=#E59700>"+CStr( GetConfigString( skill_config["Index"], "Title" )));

        //SubTitles Background

        //ITEM Background
	GFResizePic( gump, 15, 45, GFGetCfgConst( "Defaults", "ForeGround" ), 197, 280 );
	GFHTMLArea( gump, 10, 45, 200, 20, "<basefont color=#E59700><center>ITEM</center>" );
 
        var graphic := CInt( thisitem.graphic );
	if( !graphic )
		graphic := CInt( objtype );
	endif
 
	GFTilePic( gump, 90, 100, CInt( graphic ), thisitem.color );

        //SKILLS Background
	GFResizePic( gump, 215, 45, GFGetCfgConst( "Defaults", "ForeGround" ), 295, 150 );
	GFHTMLArea( gump, 215, 45, 305, 20, "<basefont color=#E59700><center>SKILLS</center>" );
 
 	var skill_req := CDbl( elem.Difficulty ),
            skill2 := elem.Skill2,
            skill2_req := elem.Difficulty2;
 
	GFHTMLArea( gump, 250, 130, 150, 20, ( "<basefont color=#ffffff>"+display_skill+" required" ));
	GFHTMLArea( gump, 460, 130, 30, 18, ( "<basefont color=#ffffff>"+skill_req ));
 
	if( skill2 )
		GFHTMLArea( gump, 250, 150, 150, 20, ( "<basefont color=#ffffff>"+skill2+" required" ));
		GFHTMLArea( gump, 460, 150, 30, 18, ( "<basefont color=#ffffff>"+skill2_req ));
	endif
 
        var schance := CStr( Craft_SkillCheck( who, elem ))+"%";
	GFTextLine( gump, 250, 70, 1153, "Success Chance:" );
	GFTextLine( gump, 460, 70, 1153, schance );

	var echance := 0;
	if( elem.Exceptional == 1 )
		echance := CStr( Craft_ExpCheck( who, elem ))+"%";
	endif

	if( echance )
		GFTextLine( gump, 250, 90, 1153, "Exceptional Chance:" );
		GFTextLine( gump, 460, 90, 1153, echance );
	endif

        //MATERIALS Background
	GFResizePic( gump, 215, 198, GFGetCfgConst( "Defaults", "ForeGround" ), 295, 150 );
	GFHTMLArea( gump, 215, 198, 305, 20, "<basefont color=#E59700><center>MATERIALS</center>" );
 
        var mat_parms := GetMaterials( elem ),
            mat_name := mat_parms[1],
            material := mat_parms[2];
            
	GFHTMLArea( gump, 250, 225, 270, 80, ( "<basefont color=#ffffff>"+mat_name ) );
	if( material )
		GFHTMLArea( gump, 460, 225, 30, 80, ( "<basefont color=#ffffff>"+material ));
	endif

        //Bottom Background
	GFResizePic( gump, 15, 300, GFGetCfgConst( "Defaults", "ForeGround" ), 497, 125 );

        //Notice Section
	GFResizePic( gump, 18, 288, GFGetCfgConst( "BackGrounds", "GOLDTRIM" ), 490, 50 );
	GFHTMLArea( gump, 10, 302, 130, 20, "<basefont color=#E59700><center>Notices:</center>" );
 
        var tooltip := thisitem.tooltip;
	if( tooltip )
		GFHTMLArea( gump, 170, 302, 300, 40, ( "<basefont color=#ffffff>"+tooltip ));
        else
                var tooltip := craft_config[objtype].tooltip;
        	if( craft_config[objtype].tooltip )
			GFTextLine( gump, 170, 292, 1153, "- "+tooltip );
		endif
  
		if( craft_config[objtype].retain == 1 )
			GFTextLine( gump, 170, 292, 1153, "- The item retains the color of the material." );
		endif

        	if( craft_config[objtype].mark == 1 )
			GFTextLine( gump, 170, 312, 1153, "- This item may hold its maker's mark." );
		endif
	endif

	GFHTMLArea( gump, 65, 395, 60, 18, "<basefont color=#E59700>Back" );
	GFAddButton( gump, 30, 392, 4014, 4016, GF_CLOSE_BTN, 1 );

	GFHTMLArea( gump, 430, 350, 80, 18, "<basefont color=#E59700>Make Now" );
	GFAddButton( gump, 395, 347, 4005, 4007, GF_CLOSE_BTN, CInt( objtype ));

	var ifres := GFSendGump( who, gump );
	if( ifres[0] == 1 )
		return 0;
	else
		return ifres[0];
	endif
endfunction

function cBaseCraftDict()

	var baseDict  := dictionary;
	foreach skillentry in GetConfigStringArray( menu_config["Skills"], "Skill" )
		var temp := dictionary;
		temp.+skill  := menu_config[skillentry].Attribute;
		temp.+amount := CInt( menu_config["Setup"].DefaultAmount );
		temp.+mark   := CInt( menu_config[skillentry].MakersMark );
		temp.+retain := CInt( menu_config[skillentry].RetainColor );
		temp.+last   := 0;
		baseDict[temp.skill] := temp;
	endforeach

	return baseDict;
endfunction

function GetMaterials( elem )

	// Get the material type required to craft the item.
	var matname := Lower( elem.type );
	if( TypeOf( elem.mtype ) == "String" )
		matname := elem.mtype;
	endif

	// Get the materials amount required to craft the item
	var material  := elem.material;
	if(( elem.material2 > 0 ) && ( elem.material2 != error ))
		// Material 2 name
		matname := matname+"\n"+elem.type2;
		material := material+"\n"+elem.material2;
	endif

	// Below added for checking for parts. If parts are present, then create the part list
	// instead.
	var tmatname  := 0;
	var tmpmat    := "";

	if( TypeOf( elem.parts ) == "String" )
		tmpmat := SplitWords( elem.parts );
		matname  := "";
		material := "";

		if(( Len( tmpmat ) / 2 ) < 5 )
			for j := 1 to ( Len( tmpmat ) / 2 )
				tmatname := GetObjTypeDesc( CInt( tmpmat[j] ));
				case(j)
					1:
					2:
					3: matname += tmatname+"\n";
					4: matname += tmatname;
					default: matname += tmatname+"\n";
				endcase
				material += ( tmpmat[j + ( Len( tmpmat ) / 2 )] )+"\n";
                                SleepMS(5);
			endfor
		else
			material := 0;
			for j := 1 to ( Len( tmpmat ) / 2 )
				tmatname := GetObjTypeDesc( CInt( tmpmat[j] ));
				if( j == ( Len( tmpmat ) / 2 ))
					matname += "& "+(tmpmat[j+(Len(tmpmat)/2)])+" "+tmatname+" required";
				else
					matname += ( tmpmat[j+( Len( tmpmat ) / 2 )] )+" "+tmatname+", ";
				endif
                                SleepMS(5);
			endfor
		endif
	endif

	return array{matname, material};
endfunction
