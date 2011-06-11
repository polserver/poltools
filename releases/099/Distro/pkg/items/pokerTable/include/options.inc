/*
        Created by *Edwards
        For PolServer.com

        Mr_Lacerte@hotmail.com
*/

function viewOptions( mobile, byref gump, data_elem )

        var table_info := data_elem.GetProp( "Seats" ),
            seat_ref;

        var i;
        for( i:=1; i<=10; i+=1 )
                 if( table_info[i].serial == mobile.serial )
                        seat_ref := table_info[i];
                        break;
                 endif
                 SleepMS(2);
        endfor

        if( seat_ref.gotcards && seat_ref.mustplay )
                 GFTextLine( gump, 220, 370, 2100, "Fold" );
                 GFAddButton( gump, 200, 374, 0x3, 0x4, GF_CLOSE_BTN, 0xBA2 );

                 var to_call := CInt( data_elem.GetProp( "ToCall" ));
                 if( to_call )
                         GFTextLine( gump, 300, 340, 2100, "Call [ "+to_call+" ]" );
                         GFAddButton( gump, 280, 344, 0x3, 0x4, GF_CLOSE_BTN, 0xBA3 );
                          
                         GFTextLine( gump, 300, 370, 2100, "Raise" );
                         GFAddButton( gump, 280, 374, 0x3, 0x4, GF_CLOSE_BTN, 0xBA4 );
                 else
                         GFTextLine( gump, 220, 340, 2100, "Check" );
                         GFAddButton( gump, 200, 344, 0x3, 0x4, GF_CLOSE_BTN, 0xBA1 );
                 
                         GFTextLine( gump, 300, 370, 2100, "Bet" );
                         GFAddButton( gump, 280, 374, 0x3, 0x4, GF_CLOSE_BTN, 0xBA5 );
                 endif
        elseif( seat_ref.gotcards )
                 GFTextLine( gump, 240, 370, 2100, "[Wait for your turn]" );
        endif

        return gump;
endfunction

function CheckMobile( mobile, npc, data_elem )

        PrintTextAbove( npc, mobile.name+" checked.", 3, 55 );
        data_elem.SetProp( "NextTurn", 1 );

        endTurn( mobile, npc, data_elem );

        return 1;
endfunction

function FoldMobile( mobile, npc, data_elem )

        PrintTextAbove( npc, mobile.name+" folded.", 3, 55 );
        data_elem.SetProp( "NextTurn", 1 );

        endTurn( mobile, npc, data_elem, 1 );

        return 1;
endfunction

function CallMobile( mobile, npc, data_elem )

        var total_pot := CInt( data_elem.GetProp( "TablePot" )),
            player_money := GetPlayerMoney( mobile, data_elem ),
            to_call := CInt( data_elem.GetProp( "ToCall" )),
            new_total;
            
        if( player_money < to_call )
                SetPlayerMoney( mobile, data_elem, 0 );
                new_total := total_pot + player_money;
                
                PrintTextAbove( npc, mobile.name+" is All-In!", 3, 55 );
        else
                SetPlayerMoney( mobile, data_elem, player_money - to_call );
                new_total := total_pot + to_call;
                
                PrintTextAbove( npc, mobile.name+" called "+to_call+".", 3, 55 );
        endif
        
        data_elem.SetProp( "TablePot", new_total );
        data_elem.SetProp( "NextTurn", 1 );

        endTurn( mobile, npc, data_elem );

        return 1;
endfunction

function BetMobile( mobile, npc, data_elem )

        var total_pot := CInt( data_elem.GetProp( "TablePot" )),
            player_money := GetPlayerMoney( mobile, data_elem ),
            small_blind := CInt( GetObjProperty( npc, "small_blind" )),
            bet := CInt( SendTextEntryGump( mobile, "Place your bet!", TE_CANCEL_ENABLE, TE_STYLE_NUMERICAL, player_money )),
            new_total;
            
        if( bet < small_blind )
                if( player_money <= small_blind )
                        SetPlayerMoney( mobile, data_elem, 0 );
                        new_total := player_money;
                        
                        PrintTextAbove( npc, mobile.name+" is All-In!", 3, 55 );
                else
                        SendSysMessage( mobile, "Minimum bet must be "+small_blind+"!" );
                        return 0;
                endif
        else
                SetPlayerMoney( mobile, data_elem, player_money - bet );
                new_total := player_money + bet;
                
                PrintTextAbove( npc, mobile.name+" bets "+bet+".", 3, 55 );
        endif
        
        data_elem.SetProp( "TablePot", new_total );
        data_elem.SetProp( "NextTurn", 1 );

        endTurn( mobile, npc, data_elem );

        return 1;
endfunction

function RaiseMobile( mobile, npc, data_elem )

        var total_pot := CInt( data_elem.GetProp( "TablePot" )),
            player_money := GetPlayerMoney( mobile, data_elem ),
            min_raise := CInt( data_elem.GetProp( "ToCall" ) * 2 ),
            raise := CInt( SendTextEntryGump( mobile, "Place your raise!", TE_CANCEL_ENABLE, TE_STYLE_NUMERICAL, player_money )),
            new_total;

        if( raise < min_raise )
                if( player_money <= min_raise )
                        SetPlayerMoney( mobile, data_elem, 0 );
                        new_total := total_pot + player_money;

                        PrintTextAbove( npc, mobile.name+" is All-In!", 3, 55 );
                else
                        SendSysMessage( mobile, "Minimum raise must be "+min_raise+"!" );
                        return 0;
                endif
        else
                if( raise >= player_money ) //Should never be greater...
                        SetPlayerMoney( mobile, data_elem, 0 );
                        new_total := total_pot + player_money;

                        PrintTextAbove( npc, mobile.name+" is All-In!", 3, 55 );
                else
                        SetPlayerMoney( mobile, data_elem, player_money - raise );
                        new_total := total_pot + raise;

                        PrintTextAbove( npc, mobile.name+" raised to "+raise+".", 3, 55 );
                endif
        endif

        data_elem.SetProp( "TablePot", new_total );
        data_elem.SetProp( "NextTurn", 1 );

        endTurn( mobile, npc, data_elem );

        return 1;
endfunction

function endTurn( mobile, npc, data_elem, fold := 0 )

        var table_info := data_elem.GetProp( "Seats" ),
            played := CInt( data_elem.GetProp( "Played" )),
            num_players := 0;

        var i;
        for( i:=1; i<=10; i+=1 )
                 if( table_info[i].serial == mobile.serial )
                        table_info[i].mustplay := 0;
                        if( fold )
                               table_info[i].gotcards := 0;
                               table_info[i].besthand := 0;
                               table_info[i].handvalue := array{};
                        endif

                        table_info[i].played := 1;
                        played += 1;
                 endif

                 if( table_info[i].gotcards )
                        num_players += 1;
                 endif
                 SleepMS(2);
        endfor

        data_elem.SetProp( "Seats", table_info );
        data_elem.SetProp( "Played", played );
        data_elem.SetProp( "NumPlayers", num_players );

        if( num_players <= 1 )
                 CloseTable( data_elem, npc );
        elseif( played >= num_players )
                 data_elem.SetProp( "NextGameStatut", 1 );
                 data_elem.EraseProp( "Played" );
        endif

        return 1;
endfunction
