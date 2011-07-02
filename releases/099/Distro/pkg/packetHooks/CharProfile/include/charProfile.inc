/*
        Created by *Edwards

        Last Modifications 2011-03-31
*/
use uo;

include ":classes:classes";
include ":classes:ranks";
include ":statistics:playTime_ranks";
include ":statistics:settings";
include ":timeutils:gameTime";
include ":vote:settings";

function MainBackground( byref gump, chr )

        //Main Background
  	GFGumpPic( gump, 83, 63, 0x820 );
  	GFGumpPic( gump, 100, 100, 0x821 );
     	GFGumpPic( gump, 100, 240, 0x821 );
  	GFGumpPic( gump, 100, 170, 0x822 );

  	GFGumpPic( gump, 100, 280, 0x823 );

        //Top/Bottom Deco
  	GFGumpPic( gump, 120, 100, 0x034 );
  	GFGumpPic( gump, 120, 260, 0x82b );

        GFTextMid( gump, 80, 70, 340, 1720, "Email Box #"+chr.serial ); //Profile Name

        return 1;
endfunction

function BuildPlaytimeInfo( byref gump, chr )

        getReelPlayTime( chr );
        
        var stats_datafile := Statistic_OpenDatafile(),
            data_elem := DFFindElement( stats_datafile, chr.acctname ),
            online_Time := CInt( data_elem.GetProp( "OnlineTime" )),
            month_time := CInt( data_elem.GetProp( TIME_GetCurrentPlaytimeMonth() ));

        GFPage( gump, 1 );
        
        GFAddButton( gump, 340, 70, 0x15e5, 0x15e1, GF_PAGE_BTN, 2 ); //Next

        GFTextLine( gump, 180, 105, 1720, "Playtime" );

        GFTextLine( gump, 110, 170, 1720, "Total:" );
        GFTextMid( gump, 80, 190, 340, 1153, TIME_GetTimeDesc( online_Time ));

        GFTextLine( gump, 110, 220, 1720, TIME_Systime_GetMonth() );
        GFTextMid( gump, 80, 240, 340, 1153, TIME_GetTimeDesc( month_time ));

        return 1;
endfunction

function BuildVoteInfo( byref gump, chr )

        var vote_datafile := Vote_OpenDatafile(),
            data_elem := DFFindElement( vote_datafile, chr.acctname ),
            total_votes := CInt( data_elem.GetProp( "TotalVotes" )),
            last_vote := CStr( data_elem.GetProp( "LastVoteTime" ));

        GFPage( gump, 2 );

        GFAddButton( gump, 120, 70, 0x15e7, 0x15e3, GF_PAGE_BTN, 1 ); //Previous
        GFAddButton( gump, 340, 70, 0x15e5, 0x15e1, GF_PAGE_BTN, 3 ); //Next

        if( !total_votes )
                last_vote := "No votes yet";
        endif

        GFTextLine( gump, 180, 105, 1720, "Vote" );

        GFTextLine( gump, 110, 170, 1720, "Total:" );
        GFTextMid( gump, 80, 190, 340, 1153, total_votes );

        GFTextLine( gump, 110, 220, 1720, "Last:" );
        GFTextMid( gump, 80, 240, 340, 1153, last_vote );

        return 1;
endfunction

function BuildClasseInfo( byref gump, chr )

        GFPage( gump, 3 );

        GFAddButton( gump, 120, 70, 0x15e7, 0x15e3, GF_PAGE_BTN, 2 ); //Previous
        GFAddButton( gump, 340, 70, 0x15e5, 0x15e1, GF_PAGE_BTN, 4 ); //Next

        GFTextLine( gump, 180, 105, 1720, "Classe" );
        if( chr.cmdlevel )
                GFTextMid( gump, 80, 190, 340, 1153, "Qualified level "+chr.cmdlevel+" staff." );
        elseif( Get_Mobile_Classe_Name( chr ))
                var classe := Get_Mobile_Classe_Name( chr ),
                    level := GetObjProperty( chr, classe ),
                    classe_title := GetClasseName( classe ),
                    classe_rank := GetClasseRankString( chr );
                    
                if( level >= 6 )
                        classe_title := GetMaxClasseTitle( classe );
                endif

                GFTextMid( gump, 80, 190, 340, 1153, "Qualified level "+level+" "+lower( classe_title )+"." );
                
                if( classe_rank )
                        GFTextMid( gump, 80, 210, 340, 1153, "Ranked "+classe_rank+" among the "+lower( GetClasseName( classe ))+"s." );
                else
                        GFTextMid( gump, 80, 210, 340, 1153, "You're not strong enough to be ranked." );
                endif
        else
                GFTextMid( gump, 80, 190, 340, 1153, "Isn't in any specific classe." );
        endif

        return 1;
endfunction

function BuildReputationInfo( byref gump, chr )

        chr := chr; //Avoid compiler warning...

        GFPage( gump, 4 );

        GFAddButton( gump, 120, 70, 0x15e7, 0x15e3, GF_PAGE_BTN, 3 ); //Previous

        GFTextLine( gump, 180, 105, 1720, "Reputation" );
        
        GFTextLine( gump, 110, 165, 1720, "Boss Killed:" );
        GFTextLine( gump, 210, 165, 1720, 0 );
        GFTextMid( gump, 80, 185, 340, 1153, "Ranked 0th among the PvMs." );

        GFTextLine( gump, 110, 225, 1720, "Players Killed:" );
        GFTextLine( gump, 210, 225, 1720, 0);
        GFTextMid( gump, 80, 245, 340, 1153, "Ranked 0th among the PvPs." );
        
        return 1;
endfunction
