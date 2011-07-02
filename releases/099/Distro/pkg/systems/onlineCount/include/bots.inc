/*
        Created by *Edwards

        2010-05-29
*/
use uo;

const EVENT_CONNECT_BOT    := 0xA1;
const EVENT_DISCONNECT_BOT := 0xA2;

function BOT_SendCommand( event_id )

	var event := struct{"type":=event_id};
 
	return BOT_GetProcess().SendEvent( event );
endfunction

function BOT_GetProcess()

	var pid := CInt( GetGlobalProperty( "#OnlineCountPid" ));
 
	return GetProcess( pid );
endfunction

function CheckDisconnectBot( byref bots, settings, force:=0 )

        if( bots.Size() <= 0 )
                return 0;
        endif

        if( RandomInt( 100 ) < CInt( settings.ChanceDisconnect ) || force )
                var bot_info := bots.RandomEntry(),
                    bot_name := bot_info.name,
                    bot_gametime := bot_info.gametime;

                if( bot_gametime > ReadGameClock() && !force )
                        //Do not kill the BOT yet..
                        return 0;
                endif

                Broadcast( "Character ["+bot_name+"] has departed", 3, 40 );

                bots := RemoveFromArray( bots, bot_info );
                return 1;
        endif

        return 0;
endfunction

function CheckConnectBot( byref bots, settings, force:=0 )

        var connect_chances := CInt( settings.ChanceConnect ),
            max_bots := CInt( settings.NumBoth ),
            num_bots := CInt( bots.Size() );

        if( num_bots > max_bots )
                connect_chances -= CInt(( num_bots - max_bots ) * 20 );
                if( connect_chances <= 0 )
                        connect_chances := 0;
                endif
        endif

        if( RandomInt( 100 ) < connect_chances || force )
                var name_list := OC_GetnameList(),
                    bot_name := name_list.RandomEntry();
                
                if( AlreadyInOnlinePlayers( bot_name ))
                        return 0;
                elseif( AlreadyInOnlineBots( bot_name, bots ))
                        return 0;
                endif

                Broadcast( "Character ["+bot_name+"] has entered", 3, 40 );

                var bot_info := struct;
                    bot_info.+name := bot_name;
                    bot_info.+gametime := ReadGameClock() + ( SEC_MINUTE * settings.MinOnlineDelay );

                bots.Append( bot_info );
        endif

        return 1;
endfunction

function AlreadyInOnlinePlayers( bot_name )

        foreach player in EnumerateOnlineCharacters();
                if( Lower( player.name ) == Lower( bot_name ))
                        return 1;
                endif
                SleepMS(5);
        endforeach

        return 0;
endfunction

function AlreadyInOnlineBots( bot_name, bots )

        foreach bot in bots
                if( Lower( bot.name ) == Lower( bot_name ))
                        return 1;
                endif
                SleepMS(5);
        endforeach

        return 0;
endfunction
