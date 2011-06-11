/*
        Created by *Edwards
*/

function viewSeats( byref gump, data_elem )

        var table_info := data_elem.GetProp( "Seats" ),
            game_started := data_elem.GetProp( "GameStarted" );

        GFTilePic( gump, 50, 80, 0xa2b );
        if( !table_info[1].money )
               GFAddButton( gump, 65, 60, 2117, 2118, GF_CLOSE_BTN, 1 );
        else
               if( table_info[1].mustplay )
                      GFTextMid( gump, 45, 70, 75, 55, Get_TableName( table_info[1].serial ));
               else
                      GFTextMid( gump, 45, 70, 75, 2100, Get_TableName( table_info[1].serial ));
               endif

               GFResizePic( gump, 45, 105, GFGetCfgConst( "Backgrounds", "PAPER" ), 60, 20 );
               GFTextLine( gump, 50, 105, 1720, table_info[1].money );

               if( table_info[1].gotcards && game_started )
                      GFTilePic( gump, 30, 90, 0xe17 );
               endif
        endif

        GFTilePic( gump, 125, 80, 0xa2b );
        if( !table_info[2].money )
               GFAddButton( gump, 140, 60, 2117, 2118, GF_CLOSE_BTN, 2 );
        else
               if( table_info[2].mustplay )
                      GFTextMid( gump, 120, 70, 75, 55, Get_TableName( table_info[2].serial ));
               else
                      GFTextMid( gump, 120, 70, 75, 2100, Get_TableName( table_info[2].serial ));
               endif

               GFResizePic( gump, 120, 105, GFGetCfgConst( "Backgrounds", "PAPER" ), 60, 20 );
               GFTextLine( gump, 125, 105, 1720, table_info[2].money );

               if( table_info[2].gotcards && game_started )
                      GFTilePic( gump, 105, 90, 0xe17 );
               endif
        endif

        GFTilePic( gump, 200, 80, 0xa2b );
        if( !table_info[3].money )
               GFAddButton( gump, 215, 60, 2117, 2118, GF_CLOSE_BTN, 3 );
        else
               if( table_info[3].mustplay )
                      GFTextMid( gump, 195, 70, 75, 55, Get_TableName( table_info[3].serial ));
               else
                      GFTextMid( gump, 195, 70, 75, 2100, Get_TableName( table_info[3].serial ));
               endif

               GFResizePic( gump, 195, 105, GFGetCfgConst( "Backgrounds", "PAPER" ), 60, 20 );
               GFTextLine( gump, 200, 105, 1720, table_info[3].money );

               if( table_info[3].gotcards && game_started )
                      GFTilePic( gump, 180, 90, 0xe17 );
               endif
        endif

        GFTilePic( gump, 275, 80, 0xa2b );
        if( !table_info[4].money )
               GFAddButton( gump, 290, 60, 2117, 2118, GF_CLOSE_BTN, 4 );
        else
               if( table_info[4].mustplay )
                      GFTextMid( gump, 270, 70, 75, 55, Get_TableName( table_info[4].serial ));
               else
                      GFTextMid( gump, 270, 70, 75, 2100, Get_TableName( table_info[4].serial ));
               endif

               GFResizePic( gump, 270, 105, GFGetCfgConst( "Backgrounds", "PAPER" ), 60, 20 );
               GFTextLine( gump, 275, 105, 1720, table_info[4].money );

               if( table_info[4].gotcards && game_started )
                      GFTilePic( gump, 255, 90, 0xe17 );
               endif
        endif

        GFTilePic( gump, 350, 80, 0xa2b );
        if( !table_info[5].money )
               GFAddButton( gump, 365, 60, 2117, 2118, GF_CLOSE_BTN, 5 );
        else
               if( table_info[5].mustplay )
                      GFTextMid( gump, 345, 70, 75, 55, Get_TableName( table_info[5].serial ));
               else
                      GFTextMid( gump, 345, 70, 75, 2100, Get_TableName( table_info[5].serial ));
               endif

               GFResizePic( gump, 345, 105, GFGetCfgConst( "Backgrounds", "PAPER" ), 60, 20 );
               GFTextLine( gump, 350, 105, 1720, table_info[5].money );

               if( table_info[5].gotcards && game_started )
                      GFTilePic( gump, 330, 90, 0xe17 );
               endif
        endif

        GFTilePic( gump, 50, 240, 0xa2b );
        if( !table_info[6].money )
               GFAddButton( gump, 65, 275, 2117, 2118, GF_CLOSE_BTN, 6 );
        else
               if( table_info[6].mustplay )
                      GFTextMid( gump, 45, 230, 75, 55, Get_TableName( table_info[6].serial ));
               else
                      GFTextMid( gump, 45, 230, 75, 2100, Get_TableName( table_info[6].serial ));
               endif

               GFResizePic( gump, 45, 270, GFGetCfgConst( "Backgrounds", "PAPER" ), 60, 20 );
               GFTextLine( gump, 50, 270, 1720, table_info[6].money );

               if( table_info[6].gotcards && game_started )
                      GFTilePic( gump, 30, 250, 0xe17 );
               endif
        endif

        GFTilePic( gump, 125, 240, 0xa2b );
        if( !table_info[7].money )
               GFAddButton( gump, 140, 275, 2117, 2118, GF_CLOSE_BTN, 7 );
        else
               if( table_info[7].mustplay )
                      GFTextMid( gump, 120, 230, 75, 55, Get_TableName( table_info[7].serial ));
               else
                      GFTextMid( gump, 120, 230, 75, 2100, Get_TableName( table_info[7].serial ));
               endif

               GFResizePic( gump, 120, 270, GFGetCfgConst( "Backgrounds", "PAPER" ), 60, 20 );
               GFTextLine( gump, 125, 270, 1720, table_info[7].money );

               if( table_info[7].gotcards && game_started )
                      GFTilePic( gump, 105, 250, 0xe17 );
               endif
        endif

        GFTilePic( gump, 200, 240, 0xa2b );
        if( !table_info[8].money )
               GFAddButton( gump, 215, 275, 2117, 2118, GF_CLOSE_BTN, 8 );
        else
               if( table_info[8].mustplay )
                      GFTextMid( gump, 195, 230, 75, 55, Get_TableName( table_info[8].serial ));
               else
                      GFTextMid( gump, 195, 230, 75, 2100, Get_TableName( table_info[8].serial ));
               endif

               GFResizePic( gump, 195, 270, GFGetCfgConst( "Backgrounds", "PAPER" ), 60, 20 );
               GFTextLine( gump, 200, 270, 1720, table_info[8].money );

               if( table_info[8].gotcards && game_started )
                      GFTilePic( gump, 180, 250, 0xe17 );
               endif
        endif

        GFTilePic( gump, 275, 240, 0xa2b );
        if( !table_info[9].money )
               GFAddButton( gump, 290, 275, 2117, 2118, GF_CLOSE_BTN, 9 );
        else
               if( table_info[9].mustplay )
                      GFTextMid( gump, 270, 230, 75, 55, Get_TableName( table_info[9].serial ));
               else
                      GFTextMid( gump, 270, 230, 75, 2100, Get_TableName( table_info[9].serial ));
               endif

               GFResizePic( gump, 270, 270, GFGetCfgConst( "Backgrounds", "PAPER" ), 60, 20 );
               GFTextLine( gump, 275, 270, 1720, table_info[9].money );

               if( table_info[9].gotcards && game_started )
                      GFTilePic( gump, 255, 250, 0xe17 );
               endif
        endif

        GFTilePic( gump, 350, 240, 0xa2b );
        if( !table_info[10].money )
               GFAddButton( gump, 365, 275, 2117, 2118, GF_CLOSE_BTN, 10 );
        else
               if( table_info[10].mustplay )
                      GFTextMid( gump, 345, 230, 75, 55, Get_TableName( table_info[10].serial ));
               else
                      GFTextMid( gump, 345, 230, 75, 2100, Get_TableName( table_info[10].serial ));
               endif

               GFResizePic( gump, 345, 270, GFGetCfgConst( "Backgrounds", "PAPER" ), 60, 20 );
               GFTextLine( gump, 350, 270, 1720, table_info[10].money );

               if( table_info[10].gotcards && game_started )
                      GFTilePic( gump, 330, 250, 0xe17 );
               endif
        endif

        return gump;
endfunction

function Get_TableName( serial )

        var who := SystemFindObjectBySerial( serial ),
            name;

        if( len( who.name ) > 7 )
               name := who.name[1, 7];
        else
               name := who.name;
        endif

        return name;
endfunction

function Set_PlayerSeat( mobile, npc, data_elem, value )

        var table_info := data_elem.GetProp( "Seats" ),
            num_players := CInt( data_elem.GetProp( "NumPlayers" ));

        if( table_info[value].money )
               SendSysMessage( mobile, "This seat is already reserved." );
               return 0;
        endif

        var i;
        for( i:=1; i<=10; i+=1 )
                 if( table_info[i].serial == mobile.serial )
                        SendSysMessage( mobile, "You already have a reserved seat on this table." );
                        return 0;
                 endif
                 SleepMS(2);
        endfor
        
        var max_buyIn := CInt( GetObjProperty( npc, "max_buyIn" )),
            min_buyIn := CInt( GetObjProperty( npc, "min_buyIn" ));
        
        var money := CInt( SendTextEntryGump( mobile, "Enter the buy-in amount.", TE_CANCEL_ENABLE, TE_STYLE_NUMERICAL, max_buyIn ));
        if( !money )
               SendSysMessage( mobile, "Cancelled." );
               return 0;
        elseif( money < min_buyIn )
               SendSysMessage( mobile, "You did not enter a good buy-in amount of golds." );
               return 0;
        elseif( !ConsumeSubstance( mobile.backpack, 0xeed, money ))
               SendSysMessage( mobile, "You do not have enough golds." );
               return 0;
        endif
        
        table_info[value].serial := mobile.serial;
        table_info[value].money := money;

        num_players += 1;

        data_elem.SetProp( "Seats", table_info );
        data_elem.SetProp( "NumPlayers", num_players );
        
        PrintTextAbove( npc, mobile.name+" joins the table.", 3, 55 );

        return 1;
endfunction

function Remove_PlayerSeat( mobile, npc, data_elem )

         SendSysMessage( mobile, "Disconnecting from the table..." );

         var table_info := data_elem.GetProp( "Seats" ),
             num_players := CInt( data_elem.GetProp( "NumPlayers" ));
         
         var i;
         for( i:=1; i<=10; i+=1 )
                 if( table_info[i].serial == mobile.serial )
                         ReturnMoney( mobile, table_info[i] );
                         
                         table_info[i].money := 0;
                         table_info[i].serial := 0;
                         table_info[i].gotcards := 0;
                         table_info[i].played := 0;
                         table_info[i].besthand := 0;
                         table_info[i].handvalue := array{};
                 elseif( table_info[i].money )
                         num_players += 1;
                 endif
                 SleepMS(2);
         endfor

         if( num_players < 2 )
                 CloseTable( data_elem, npc );
         endif

         data_elem.SetProp( "Seats", table_info );
         data_elem.SetProp( "NumPlayers", num_players );

         EraseObjProperty( mobile, "#Poker" );
         
         PrintTextAbove( npc, mobile.name+" leaves the table.", 3, 55 );

         return 1;
endfunction
