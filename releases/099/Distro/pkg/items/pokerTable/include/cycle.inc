/*
        Created by *Edwards
*/

function Cycle_SeatsData( data_elem )

         var table_info := data_elem.GetProp( "Seats" ),
             num_players := 0,
             index := struct,
             i;

         for( i:=1; i<=10; i+=1 )
                 if( !table_info[i].money )
                         index.+money := 0;
                         index.+serial := 0;
                         index.+gotcards := 0;
                         index.+mustplay := 0;
                         index.+played := 0;
                         index.+besthand := 0;
                         index.+handvalue := array{};
                         
                         table_info[i] := index;
                 else
                         num_players += 1;
                 endif
                 SleepMS(2);
         endfor
         
         if( num_players >= 2 )
                 data_elem.SetProp( "CanStart", 1 );
         endif
         
         data_elem.SetProp( "NumPlayers", num_players );
         data_elem.SetProp( "Seats", table_info );

         return 1;
endfunction

function Cycle_Statut( npc, data_elem )

         var gameStat := data_elem.GetProp( "GameStatut" ),
             next_GS := data_elem.GetProp( "NextGameStatut" );

         if( next_GS )
               if( !gameStat )
                       data_elem.SetProp( "GameStatut", "Flop" );
                       data_elem.EraseProp( "ToCall" );
               elseif( gameStat == "Flop" )
                       data_elem.SetProp( "GameStatut", "Turn" );
                       data_elem.EraseProp( "ToCall" );
               elseif( gameStat == "Turn" )
                       data_elem.SetProp( "GameStatut", "River" );
                       data_elem.EraseProp( "ToCall" );
               elseif( gameStat == "River" )
                       CheckCards( data_elem );
                       Check_Winner( npc, data_elem );

                       CloseTable( data_elem, npc );
               endif

               data_elem.EraseProp( "NextGameStatut" );
         endif

         return 1;
endfunction
