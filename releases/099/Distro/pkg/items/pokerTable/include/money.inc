/*
        Created by *Edwards
*/

include "include/security";

function GetPlayerMoney( mobile, data_elem )

         var table_info := data_elem.GetProp( "Seats" ),
             player_money := 0;

         var i;
         for( i:=1; i<=10; i+=1 )
                 if( table_info[i].serial == mobile.serial )
                        player_money := table_info[i].money;
                        break;
                 endif
                 SleepMS(2);
         endfor
         
         return player_money;
endfunction

function SetPlayerMoney( mobile, data_elem, amount )

         var table_info := data_elem.GetProp( "Seats" );

         var i;
         for( i:=1; i<=10; i+=1 )
                 if( table_info[i].serial == mobile.serial )
                        table_info[i].money := amount;
                        break;
                 endif
                 SleepMS(2);
         endfor
         
         data_elem.SetProp( "Seats", table_info );

         return 1;
endfunction

function ReturnMoney( mobile, table_position )
         
         var current_money := CInt( table_position.money );
         if( current_money )
                  var created := CreateItemInBackpack( mobile, 0xeed, current_money );
                  if( created.errortext )
                           NotifyStaffOnline( mobile, "PokerTable couldn't return "+current_money+" gold coins in the backpack." );
                           return 0;
                  endif
         endif

         return 1;
endfunction
