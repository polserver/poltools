/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2008-04-25
        
        Last Modifications 2009-02-14
             * UCFirst with var elemental
             * Switched for level 2 protection instead of 3

        Last Modifications 2009-10-21
             * Changed error message now displayed to the mobile

        Last Modifications 2009-12-14
             * Added settings color to tile pics

        Thanks to ZuluHotel for this original idea.
*/
use uo;
use os;

include ":elementals:settings";
include ":gumps:gumps";
include ":gumps:gumps_ex";
include ":security:report";
include "include/string";

function Send_ElementalPrize( mobile, elemental )

        elemental := UCFirst( elemental );

        var settings := ELE_GetSettingsCfgElem( elemental );
        if( settings.errortext )
		SendSysMessage( mobile, "Error:: Unable to find element [:elementals:settings.cfg] ->"+settings.errortext );
                return 0;
	endif
 
        var text := "";

        //Initialize the gump
        var gump := GFCreateGump();

        GFClosable( gump, 0 );

        //Load Main Background
        GFResizePic( gump, 0, 0, GFGetCfgConst( "Defaults", "BackGround" ), 500, 390 );
	GFResizePic( gump, 15, 45, GFGetCfgConst( "Defaults", "ForeGround" ), 470, 335 );
        GFResizePic( gump, 15, 15, GFGetCfgConst( "Defaults", "ForeGround" ), 470, 25 );

        GFTextMid( gump, 15, 17, 470, 1720, "The "+elemental+" Prize Screen" );

        GFTilePic( gump, 70, 80, settings.Weapon, settings.Color );
        GFTilePic( gump, 70, 180, 0x13d3, settings.Color );
        GFTilePic( gump, 70, 280, 0x1416, settings.Color );

        text := settings.WeaponDesc;
	GFDrawWrappedText( gump, 160, 80, GFWordWrap( text, 300 ) );

        text := settings.LeatherDesc;
	GFDrawWrappedText( gump, 160, 180, GFWordWrap( text, 300 ) );

        text := settings.PlatemailDesc;
	GFDrawWrappedText( gump, 160, 280, GFWordWrap( text, 300 ) );

        GFAddButton( gump, 40, 105, 2117, 2118, GF_CLOSE_BTN, 1 );
        GFAddButton( gump, 40, 205, 2117, 2118, GF_CLOSE_BTN, 2 );
        GFAddButton( gump, 40, 305, 2117, 2118, GF_CLOSE_BTN, 3 );

        //And now we need to know the answer...
        var input := GFSendGump( mobile, gump );
        if( input[1] )
                 Create_ElementalWeapon( mobile, elemental );
                 ReportText( mobile.serial+" created Elemental Weapon: "+elemental, REPORT_SYSLOG );
        elseif( input[2] )
                 Create_ElementalLeather( mobile, elemental );
                 ReportText( mobile.serial+" created Elemental LeatherArmor: "+elemental, REPORT_SYSLOG );
        elseif( input[3] )
                 Create_ElementalArmor( mobile, elemental );
                 ReportText( mobile.serial+" created Elemental PlateArmor: "+elemental, REPORT_SYSLOG );
        else
                 return 0;
        endif

        return 1;
endfunction

function Create_ElementalWeapon( mobile, elemental )

        var settings := ELE_GetSettingsCfgElem( elemental );
        if( settings.errortext )
		SendSysMessage( mobile, "Error:: Unable to find element [:elementals:settings.cfg] ->"+settings.errortext );
                return 0;
	endif
 
        var Weapon_Name := settings.WeaponName;

        var create_Eweapon := CreateItemInBackpack( mobile, Weapon_Name, 1 );
        if( create_Eweapon.errortext )
		SendSysMessage( mobile, "Error:: Unable to create element [:elementals:settings.cfg] ->"+create_Eweapon.errortext );
                return 0;
	endif
        
        return 1;
endfunction

function Create_ElementalLeather( mobile, elemental )

        var settings := ELE_GetSettingsCfgElem( elemental );
        if( settings.errortext )
		SendSysMessage( mobile, "Error:: Unable to find element [:elementals:settings.cfg] ->"+settings.errortext );
                return 0;
	endif

        var create_ELarmor;

        //Leather Gorget
        create_ELarmor := CreateItemInBackpack( mobile, 0x7596, 1 );
                   create_ELarmor.color := settings.Color;
                   create_ELarmor.name := "Leather Gorget of the "+elemental+" Element";
                   SetObjProperty( create_ELarmor, elemental+"Protection", 2 );

        //Leather Sleeves
        create_ELarmor := CreateItemInBackpack( mobile, 0x7597, 1 );
                   create_ELarmor.color := settings.Color;
                   create_ELarmor.name := "Leather Sleeves of the "+elemental+" Element";
                   SetObjProperty( create_ELarmor, elemental+"Protection", 2 );

        //Leather Gloves
        create_ELarmor := CreateItemInBackpack( mobile, 0x7598, 1 );
                   create_ELarmor.color := settings.Color;
                   create_ELarmor.name := "Leather Gloves of the "+elemental+" Element";
                   SetObjProperty( create_ELarmor, elemental+"Protection", 2 );

        //Leather Cap
        create_ELarmor := CreateItemInBackpack( mobile, 0x7599, 1 );
                   create_ELarmor.color := settings.Color;
                   create_ELarmor.name := "Leather Cap of the "+elemental+" Element";
                   SetObjProperty( create_ELarmor, elemental+"Protection", 2 );

        //Leather Leggings
        create_ELarmor := CreateItemInBackpack( mobile, 0x759a, 1 );
                   create_ELarmor.color := settings.Color;
                   create_ELarmor.name := "Leather Leggings of the "+elemental+" Element";
                   SetObjProperty( create_ELarmor, elemental+"Protection", 2 );

        //Leather Tunic
        create_ELarmor := CreateItemInBackpack( mobile, 0x759b, 1 );
                   create_ELarmor.color := settings.Color;
                   create_ELarmor.name := "Leather Tunic of the "+elemental+" Element";
                   SetObjProperty( create_ELarmor, elemental+"Protection", 2 );

        return 1;
endfunction

function Create_ElementalArmor( mobile, elemental )

        var settings := ELE_GetSettingsCfgElem( elemental );
        if( settings.errortext )
		SendSysMessage( mobile, "Error:: Unable to find element [:elementals:settings.cfg] ->"+settings.errortext );
                return 0;
	endif

        var create_EParmor;

        //Platemail Breastplate
        create_EParmor := CreateItemInBackpack( mobile, 0x759c, 1 );
                   create_EParmor.color := settings.Color;
                   create_EParmor.name := "Platemail Breastplate of the "+elemental+" Element";
                   SetObjProperty( create_EParmor, Elemental+"Protection", 3 );

        //Platemail Arms
        create_EParmor := CreateItemInBackpack( mobile, 0x1f0c, 1 );
                   create_EParmor.color := settings.Color;
                   create_EParmor.name := "Platemail Arms of the "+elemental+" Element";
                   SetObjProperty( create_EParmor, Elemental+"Protection", 2 );

        //Platemail Leggings
        create_EParmor := CreateItemInBackpack( mobile, 0x1f0d, 1 );
                   create_EParmor.color := settings.Color;
                   create_EParmor.name := "Platemail Leggings of the "+elemental+" Element";
                   SetObjProperty( create_EParmor, Elemental+"Protection", 2 );

        //Platemail Gloves
        create_EParmor := CreateItemInBackpack( mobile, 0x1f0e, 1 );
                   create_EParmor.color := settings.Color;
                   create_EParmor.name := "Platemail Gloves of the "+elemental+" Element";
                   SetObjProperty( create_EParmor, Elemental+"Protection", 2 );

        //Platemail Gorget
        create_EParmor := CreateItemInBackpack( mobile, 0x1f0f, 1 );
                   create_EParmor.color := settings.Color;
                   create_EParmor.name := "Platemail Gorget of the "+elemental+" Element";
                   SetObjProperty( create_EParmor, Elemental+"Protection", 2 );

        //Closed Helm
        create_EParmor := CreateItemInBackpack( mobile, 0x1f10, 1 );
                   create_EParmor.color := settings.Color;
                   create_EParmor.name := "Closed Helm of the "+elemental+" Element";
                   SetObjProperty( create_EParmor, Elemental+"Protection", 2 );

        return 1;
endfunction
