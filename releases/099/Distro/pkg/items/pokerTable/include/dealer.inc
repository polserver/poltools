/*
        Created by *Edwards
        For PolServer.com
        
        Mr_Lacerte@hotmail.com
*/

function viewDealer( byref gump, data_elem )

        var dealer_pos := CInt( data_elem.GetProp( "Dealer" ));
        case( dealer_pos )
              1: GFTextLine( gump, 55, 50, 0, "Dealer" );
                 break;
              2: GFTextLine( gump, 130, 50, 0, "Dealer" );
                 break;
              3: GFTextLine( gump, 205, 50, 0, "Dealer" );
                 break;
              4: GFTextLine( gump, 280, 50, 0, "Dealer" );
                 break;
              5: GFTextLine( gump, 355, 50, 0, "Dealer" );
                 break;
              6: GFTextLine( gump, 55, 290, 0, "Dealer" );
                 break;
              7: GFTextLine( gump, 130, 290, 0, "Dealer" );
                 break;
              8: GFTextLine( gump, 205, 290, 0, "Dealer" );
                 break;
              9: GFTextLine( gump, 280, 290, 0, "Dealer" );
                 break;
              10: GFTextLine( gump, 355, 290, 0, "Dealer" );
                  break;
        endcase

        return 1;
endfunction

function Setup_Dealer( npc, data_elem )

         var dealer := CInt( data_elem.GetProp( "Dealer" )),
             seats := data_elem.GetProp( "Seats" ),
             next_dealer := data_elem.GetProp( "NextDealer" );

         if( !dealer )
                 dealer := RandomDiceRoll( "1d10" );
         endif

         if( next_dealer )
                 dealer += 1;
                 if( dealer > 10 )
                        dealer := 1;
                 endif

                 data_elem.EraseProp( "NextDealer" );
         endif

         var new_dealer;
         while( !new_dealer )
                 if( seats[dealer].gotcards )
                        new_dealer := dealer;
                        break;
                 else
                        dealer += 1;
                        if( dealer > 10 )
                               dealer := 1;
                        endif
                 endif

                 SleepMS(2);
         endwhile
         
         data_elem.SetProp( "Dealer", new_dealer );

         return 1;
endfunction

function Setup_WhosTurn( npc, data_elem )

         var game_started := data_elem.GetProp( "GameStarted" );
         if( !game_started )
                  //Do not mind about options at this state.
                  return 0;
         endif

         var whos_turn := CInt( data_elem.GetProp( "WhosTurn" )),
             seats := data_elem.GetProp( "Seats" ),
             next_turn := data_elem.GetProp( "NextTurn" );

         if( !whos_turn )
                 whos_turn += 3;
         endif
         
         if( next_turn )
                 whos_turn += 1;
                 if( whos_turn > 10 )
                        whos_turn := 1;
                 endif

                 data_elem.EraseProp( "NextTurn" );
         endif

         var loop := 1;
         while( loop < 10 )
                 if( seats[whos_turn].gotcards )
                        seats[whos_turn].mustplay := 1;
                        break;
                 else
                        seats[whos_turn].mustplay := 0;

                        whos_turn += 1;
                        if( whos_turn > 10 )
                               whos_turn := 1;
                        endif
                 endif
                 
                 loop += 1;

                 SleepMS(2);
         endwhile
         
         data_elem.SetProp( "WhosTurn", whos_turn );
         data_elem.SetProp( "Seats", seats );

         return 1;
endfunction
