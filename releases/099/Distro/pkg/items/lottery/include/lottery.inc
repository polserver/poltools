/*
        Created by *Edwards

        2009-10-26
        
        Last Modifications 2010-01-17
             * If prize not claimed it will be added to the next lottery.
*/
use uo;
use os;
use polsys;

include ":datafile:datafile";

var g_datafile := DFOpenDataFile( ":lottery:lottery", DF_CREATE );
var main_elem := DFFindElement( g_datafile, "MainSettings", DF_CREATE );

function Loto_GetLottery()

        var cur_lottery := main_elem.GetProp( "Lottery" );
        if( !cur_lottery )
                cur_lottery := 1;
        endif

        return CInt( cur_lottery );
endfunction

function Loto_SetNewLottery()

        var cur_lottery := Loto_GetLottery();
        
        return Loto_SetLottery( CInt( cur_lottery + 1 ));
endfunction

function Loto_SetLottery( lottery )

        return main_elem.SetProp( "Lottery", lottery );
endfunction

function Loto_EraseLottery( lottery )

        return g_datafile.DeleteElement( "Lottery"+lottery );
endfunction

function Loto_CleanOldLottery()

        var cur_lottery := Loto_GetLottery();
        if( cur_lottery < 2 )
                //At this point we do not want to erase lottery that we are
                //still waiting for the tickets...
                return 0;
        endif

        //I remove 2 from cur_lottery to avoid deleting last lottery so
        //the ticket given to the lotteryman is not expired
        cur_lottery -= 2;
        
        var i;
        for( i:=1; i<=cur_lottery; i+=1 )
                g_datafile.DeleteElement( "Lottery"+i );
                SleepMS(5);
        endfor

        return 1;
endfunction

function Loto_GetTickets( lottery )

        var data_elem := DFFindElement( g_datafile, "Lottery"+lottery );
        var tickets := data_elem.GetProp( "Tickets" );
        if( !tickets )
                tickets := array{};
        endif

        return tickets;
endfunction

function Loto_NewTicket()

        var i;

        var new_number := array{};
        for( i:=1; i<=7; i+=1 )
                new_number.Append( CInt( RandomInt(89)+10 ));
                SleepMS(5);
        endfor

        var the_numb := "";
        for( i:=1; i<=7; i+=1 )
                the_numb += new_number[i]+" ";
                SleepMS(5);
        endfor

        return CStr( the_numb );
endfunction

function Loto_SetTickets( lottery, tickets )

        var data_elem := DFFindElement( g_datafile, "Lottery"+lottery, DF_CREATE );
        return data_elem.SetProp( "Tickets", tickets );
endfunction

function Loto_GetLotteryWinner( lottery )

        var data_elem := DFFindElement( g_datafile, "Lottery"+lottery );
        var the_winner := data_elem.GetProp( "WinTicket" );
        if( !the_winner )
                return "Prize already claimed!";
        endif

        return CStr( the_winner );
endfunction

function Loto_SetLotteryWinner( lottery )

        var data_elem := DFFindElement( g_datafile, "Lottery"+lottery, DF_CREATE );
        var tickets := Loto_GetTickets( lottery );
        var the_ticket := tickets.RandomEntry();

        return data_elem.SetProp( "WinTicket", the_ticket );
endfunction

function Loto_GetLotteryPrize( lottery )

        var data_elem := DFFindElement( g_datafile, "Lottery"+lottery );
        var the_prize := data_elem.GetProp( "Prize" );
        if( !the_prize )
                the_prize := 0;
        endif

        return CInt( the_prize );
endfunction

function Loto_AddLotteryPrize( lottery, amount )

        var data_elem := DFFindElement( g_datafile, "Lottery"+lottery, DF_CREATE );
        var the_prize := Loto_GetLotteryPrize( lottery );

        return data_elem.SetProp( "Prize", CInt( the_prize + amount ));
endfunction

function Loto_GetLotteryWinners()

        var the_winners := main_elem.GetProp( "WinnerLoto" );
        if( !the_winners )
                the_winners := array{};
        endif

        return the_winners;
endfunction

function Loto_SaveLotteryWinner( lottery, mobile )

        var the_winners := Loto_GetLotteryWinners();
        
        var the_infos := struct;
        the_infos["Name"] := mobile.name;
        the_infos["Prize"] := Loto_GetLotteryPrize( lottery );
        
        the_winners.Append( the_infos );

        return main_elem.SetProp( "WinnerLoto", the_winners );
endfunction

function Loto_MainNewLottery()

        var cur_lottery := Loto_GetLottery(),
            previous_prize := Loto_GetLotteryPrize( cur_lottery - 1 );

        Loto_SetLotteryWinner( cur_lottery );
        Loto_SetNewLottery();
        Loto_CleanOldLottery();
        
        Loto_AddLotteryPrize( Loto_GetLottery(), previous_prize );
        
        Broadcast( "A new loto has started!", 3, 65 );
        Sleep(2);
        Broadcast( "See details with .lotofantasia", 3, 65 );

endfunction
