/*
        Created by *Edwards
*/

function shuffleCards( data_elem )

         data_elem.SetProp( "clubs", {2, 3, 4, 5, 6, 7, 8, 9, 10, "J", "Q", "K", "A"} );
         data_elem.SetProp( "diamonds", {2, 3, 4, 5, 6, 7, 8, 9, 10, "J", "Q", "K", "A"} );
         data_elem.SetProp( "hearts", {2, 3, 4, 5, 6, 7, 8, 9, 10, "J", "Q", "K", "A"} );
         data_elem.SetProp( "spades", {2, 3, 4, 5, 6, 7, 8, 9, 10, "J", "Q", "K", "A"} );

         return 1;
endfunction

function setup_TableCards( npc, data_elem )

         //This check saves a lot of process...
         if( data_elem.GetProp( "TCardReady" ))
               return 0;
         else
               data_elem.SetProp( "TCardReady", 1 );
         endif

         var i;
         for( i:=1; i<=5; i+=1 )
               data_elem.SetProp( "TCard"+i, getCard( npc, data_elem ));
               SleepMS(2);
         endfor

         return 1;
endfunction

function Setup_PlayersCards( npc, data_elem )

         var table_info := data_elem.GetProp( "Seats" );

         var i;
         for( i:=1; i<=10; i+=1 )
                 if( table_info[i].money )
                         data_elem.SetProp( "PCardFirst"+i, getCard( npc, data_elem ));
                         data_elem.SetProp( "PCardSecond"+i, getCard( npc, data_elem ));
                         table_info[i].gotcards := 1;
                 endif
                 SleepMS(2);
         endfor

         data_elem.SetProp( "Seats", table_info );

         return 1;
endfunction

function view_TableCards( byref gump, data_elem )

        var gameStat := data_elem.GetProp( "GameStatut" );
        if( gameStat == "Flop" || gameStat == "Turn" || gameStat == "River" )
                var first_card := data_elem.GetProp( "TCard1" );
                if( first_card )
                        GFResizePic( gump, 80, 145, 2171, 50, 65 );
                        GFTextLine( gump, 85, 145, displayColor( first_card ), first_card[2] );
                        GFTextLine( gump, 115, 190, displayColor( first_card ), first_card[2] );
                        GFTextMid( gump, 80, 170, 55, 2100, first_card[1] );
                endif

                var second_card := data_elem.GetProp( "TCard2" );
                if( second_card )
                        GFResizePic( gump, 140, 145, 2171, 50, 65 );
                        GFTextLine( gump, 145, 145, displayColor( second_card ), second_card[2] );
                        GFTextLine( gump, 175, 190, displayColor( second_card ), second_card[2] );
                        GFTextMid( gump, 140, 170, 55, 2100, second_card[1] );
                endif

                var third_card := data_elem.GetProp( "TCard3" );
                if( third_card )
                        GFResizePic( gump, 200, 145, 2171, 50, 65 );
                        GFTextLine( gump, 205, 145, displayColor( third_card ), third_card[2] );
                        GFTextLine( gump, 235, 190, displayColor( third_card ), third_card[2] );
                        GFTextMid( gump, 200, 170, 55, 2100, third_card[1] );
                endif
        endif

        if( gameStat == "Turn" || gameStat == "River" )
                var fourth_card := data_elem.GetProp( "TCard4" );
                if( fourth_card )
                        GFResizePic( gump, 260, 145, 2171, 50, 65 );
                        GFTextLine( gump, 265, 145, displayColor( fourth_card ), fourth_card[2] );
                        GFTextLine( gump, 295, 190, displayColor( fourth_card ), fourth_card[2] );
                        GFTextMid( gump, 260, 170, 55, 2100, fourth_card[1] );
                endif
        endif

        if( gameStat == "River" )
                var fifth_card := data_elem.GetProp( "TCard5" );
                if( fifth_card )
                        GFResizePic( gump, 320, 145, 2171, 50, 65 );
                        GFTextLine( gump, 325, 145, displayColor( fifth_card ), fifth_card[2] );
                        GFTextLine( gump, 355, 190, displayColor( fifth_card ), fifth_card[2] );
                        GFTextMid( gump, 320, 170, 55, 2100, fifth_card[1] );
                endif
        endif

        return 1;
endfunction

function view_PlayersCards( mobile, byref gump, data_elem )

        var table_info := data_elem.GetProp( "Seats" );

        var i;
        for( i:=1; i<=10; i+=1 )
                 if( table_info[i].serial == mobile.serial && table_info[i].gotcards )
                         GFTextLine( gump, 67, 330, 2100, "My Cards" );

                         var card1 := data_elem.GetProp( CStr( "PCardFirst"+i ));
                         if( card1 )
                                 GFResizePic( gump, 40, 350, 2171, 50, 65 );
                                 GFTextLine( gump, 45, 350, displayColor( card1 ), card1[2] );
                                 GFTextLine( gump, 75, 395, displayColor( card1 ), card1[2] );
                                 GFTextMid( gump, 40, 375, 55, 2100, card1[1] );
                         endif

                         var card2 := data_elem.GetProp( CStr( "PCardSecond"+i ));
                         if( card2 )
                                 GFResizePic( gump, 100, 350, 2171, 50, 65 );
                                 GFTextLine( gump, 105, 350, displayColor( card2 ), card2[2] );
                                 GFTextLine( gump, 135, 395, displayColor( card2 ), card2[2] );
                                 GFTextMid( gump, 100, 375, 55, 2100, card2[1] );
                         endif
                 endif
                 SleepMS(2);
        endfor

        return 1;
endfunction

function getCard( npc, data_elem )

        //Shall never run out of cards.
        var the_card;
        while( !the_card )
                the_card := pickCard( data_elem );
                SleepMS(2);
        endwhile

        var color_name := the_card[1],
            color := the_card[3];

        data_elem.SetProp( color_name, color );

        return the_card;
endfunction

function pickCard( data_elem )

         var clubs := data_elem.GetProp( "clubs" ),
             diamonds := data_elem.GetProp( "diamonds" ),
             hearts := data_elem.GetProp( "hearts" ),
             spades := data_elem.GetProp( "spades" );

         var pick_color := RandomDiceRoll( "1d4" ),
            pick_card := RandomDiceRoll( "1d13" ),
            color,
            color_name,
            card;

         case( pick_color )
              1: color := clubs;
                 color_name := "clubs";
                 break;
              2: color := diamonds;
                 color_name := "diamonds";
                 break;
              3: color := hearts;
                 color_name := "hearts";
                 break;
              4: color := spades;
                 color_name := "spades";
                 break;
         endcase

         card := color[pick_card];

         var result := color.Erase( pick_card );
         if( !result )
                 return 0;
         endif

         return {color_name, card, color};
endfunction

function displayColor( card )

        if( card[1] == "diamonds" || card[1] == "hearts" )
              return 33;
        else
              return 0;
        endif
endfunction
