/*
        Created by *Edwards
*/

function mainTable( mobile, npc, data_elem )

        var old_priority := Set_Priority(25);
        
        var game_started;
        while( Primary_Check( mobile, npc ))
        
              var gump := GFCreateGump();
              
              Cycle_SeatsData( data_elem );
              Cycle_Statut( npc, data_elem );

              /* --------------------------------------------- */

              game_started := data_elem.GetProp( "GameStarted" );
              if( !game_started )
                       
                       var shuffle := data_elem.GetProp( "Shuffle" );
                       if( !shuffle )
                              shuffleCards( data_elem );
                              data_elem.SetProp( "Shuffle", 1 );
                       endif

                       Setup_TableCards( npc, data_elem );
                       Setup_PlayersCards( npc, data_elem );

                       var can_start := data_elem.GetProp( "CanStart" );
                       if( can_start )
                                data_elem.EraseProp( "CanStart" );
                                data_elem.SetProp( "GameStarted", 1 );
                                Setup_WhosTurn( npc, data_elem );
                       endif
              endif
              
              /* --------------------------------------------- */
              
              viewTable( gump, data_elem );

              /* --------------------------------------------- */

              viewSeats( gump, data_elem );

              /* --------------------------------------------- */

              game_started := data_elem.GetProp( "GameStarted" );
              if( game_started )
                       Setup_Dealer( npc, data_elem );

                       viewDealer( gump, data_elem );

                       view_TableCards( gump, data_elem );
                       view_PlayersCards( mobile, gump, data_elem );

                       viewOptions( mobile, gump, data_elem );
              endif

              /* --------------------------------------------- */
              
              var input := GFSendGump( mobile, gump );
              if( !input || input[0xA11] )
                       return 1;
              elseif( input[0xBA1] ) //Check
                       CheckMobile( mobile, npc, data_elem );
                       Setup_WhosTurn( npc, data_elem );
              elseif( input[0xBA2] ) //Fold
                       FoldMobile( mobile, npc, data_elem );
                       Setup_WhosTurn( npc, data_elem );
              elseif( input[0xBA3] ) //Call
                       CallMobile( mobile, npc, data_elem );
                       Setup_WhosTurn( npc, data_elem );
              elseif( input[0xBA4] ) //Raise
                       RaiseMobile( mobile, npc, data_elem );
                       Setup_WhosTurn( npc, data_elem );
              elseif( input[0xBA5] ) //Bet
                       BetMobile( mobile, npc, data_elem );
                       Setup_WhosTurn( npc, data_elem );
              elseif( !input[0xA10] )
                       var table_info := data_elem.GetProp( "Seats" );
                       foreach key in ( input.keys )
                                var value := input[key];
                                if( table_info[value] )
                                        Set_PlayerSeat( mobile, npc, data_elem, value );
                                        break;
                                endif
                                SleepMS(2);
                       endforeach
              endif
              
              Set_Priority( old_priority );
              
              SleepMS(2);
        endwhile

        return 1;
endfunction

function Primary_Check( mobile, npc )

        if( !GetObjProperty( mobile, "#Poker" ))
              return 0;
        elseif( Distance( mobile, npc ) > 5 )
              return 0;
        elseif( !CheckLineOfSight( mobile, npc ))
              return 0;
        endif

        return 1;
endfunction

function viewTable( byref gump, data_elem )

        //Player Seats Background
        GFResizePic( gump, 15, 40, GFGetCfgConst( "BackGrounds", "STONESLAB" ), 420, 275 );
        GFAddAlphaRegion( gump, 15, 40, 430, 275 ); //Transparency

        GFResizePic( gump, 15, 315, GFGetCfgConst( "BackGrounds", "STONESLAB" ), 420, 155 );
        GFAddAlphaRegion( gump, 15, 315, 430, 155 );//Transparency

        GFTextLine( gump, 307, 441, 1720, "Refresh Table" );
        GFAddButton( gump, 400, 440, 0xfa8, 0xfa9, GF_CLOSE_BTN, 0xA10 );

        GFTextLine( gump, 60, 441, 1720, "Quit Table" );
        GFAddButton( gump, 25, 440, 0xfb4, 0xfb5, GF_CLOSE_BTN, 0xA11 );

        var table_info := data_elem.GetProp( "TablePot" );
        if( table_info )
               GFTextMid( gump, 15, 220, 435, 2100, "Pot: "+table_info );
        endif

        return gump;
endfunction

function CloseTable( data_elem, npc )

         //Delete TableCards
         data_elem.EraseProp( "TCardReady" );
         
         var i;
         for( i:=1; i<=5; i+=1 )
                 data_elem.EraseProp( "TCard"+i );
                 SleepMS(2);
         endfor

         //Delete PlayersCards
         for( i:=1; i<=10; i+=1 )
                 data_elem.EraseProp( "PCardFirst"+i );
                 data_elem.EraseProp( "PCardSecond"+i );
                 SleepMS(2);
         endfor

         data_elem.EraseProp( "Shuffle" );
         data_elem.EraseProp( "Dealer" );
         data_elem.EraseProp( "WhosTurn" );
         data_elem.EraseProp( "NextGameStatut" );
         data_elem.EraseProp( "GameStatut" );
         data_elem.EraseProp( "GameStarted" );
         data_elem.EraseProp( "ToCall" );
         data_elem.EraseProp( "TablePot" );

         return 1;
endfunction
