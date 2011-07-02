/*
        Created by *Edwards

        2011-04-13
*/
use uo;
use os;

include ":classes:classes";
include ":datafile:datafile";

var g_datafile := DFOpenDataFile( ":classes:rank", DF_CREATE );

function SetClasseRank()
 
        var classes := struct{};

        foreach account_name in ListAccounts()
                var account := FindAccount( account_name );

                var i;
                for( i:=1; i<=5; i+=1 )
                        var character := account.GetCharacter(i);
                        if( character )
                        
                                //Erase current rank.
                                var data_elem := DFFindElement( g_datafile, character.serial );
                                data_elem.EraseProp( "Rank" );
                                
                                var classe := Get_Mobile_Classe_Name( character );
                                if( !classe )
                                        classe := PP;
                                endif

                                var total_skills := GetTotalClasseSkills( character, classe );
                                if( total_skills < 0 )
                                        //Classe system is level based.
                                        //75 average * 8 classe skills = 600 total classe skills
                                        //+120 total classe skills is required every additional lvl.
                                        //So since it's easy to get lvl 4 ( 960 total skills ),
                                        //avoid weak players that aren't in the course by sending a
                                        //"no rank yet" message later.
                                        continue;
                                endif

                                if( !classes.Exists( classe ))
		                        classes[classe] := dictionary;
	                        endif
                                classes[classe].Insert( total_skills, character.serial );
                        endif
                        SleepMS(5);
                endfor
                SleepMS(5);
        endforeach

        foreach classe in ( classes.Keys() )

                var count_to := Len( classes[classe].Keys() );
	        for rank:=1 to count_to
		        var current_highest := 0;
		        var current_serial := "";

		        foreach key in ( classes[classe].Keys() )
			        if( CInt( key ) > current_highest )
				        current_serial := classes[classe][key];
				        current_highest := key;
			        endif
                                SleepMS(10);
		        endforeach

                        var character := SystemFindObjectBySerial( current_serial, SYSFIND_SEARCH_OFFLINE_MOBILES );
                        if( character )
                                var data_elem := DFFindElement( g_datafile, current_serial, DF_CREATE );
                                data_elem.SetProp( "Rank", rank );
                        endif

		        classes[classe].Erase( current_highest );

                        SleepMS(5);
	        endfor
                SleepMS(5);
        endforeach

        return 1;
endfunction
