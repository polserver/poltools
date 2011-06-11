/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2010-01-17
        
        Last Modifications 2010-02-05
             * Added: now support option auto to get reel value for season
*/
use uo;
use os;
use polsys;
use util;

include "include/packets";
include ":datafile:datafile";
include ":season:settings";

var g_datafile := DFOpenDataFile( ":season:season", DF_CREATE );

function SetDataShardSeason()
        
        var season := GetCfgShardSeason();
        
        var data_elem := DFFindElement( g_datafile, SHARD_NAME, DF_CREATE );
        data_elem.SetProp( "ShardSeason", season );
        
        return season;
endfunction

function SetNewShardSeason( season )

        var data_elem := DFFindElement( g_datafile, SHARD_NAME );
        data_elem.SetProp( "ShardSeason", season );

        return 1;
endfunction

function GetCfgShardSeason()

        var season_settings := Season_GetSettingsCfgElem( "Settings" ),
            shard_season := season_settings.ShardSeason;
            
        if( shard_season == "auto" )
                return FindGameTimeSeason();
        endif

        var season;
        case( shard_season )
              0:
                     season := SEASON_SPRING;
                     break;
              1:
                     season := SEASON_SUMMER;
                     break;
              2:
                     season := SEASON_FALL;
                     break;
              3:
                     season := SEASON_WINTER;
                     break;
              4:
                     season := SEASON_DESOLATION;
                     break;
        endcase

	return season;
endfunction

function GetDataShardSeason()

        var data_elem := DFFindElement( g_datafile, SHARD_NAME );
        var season := data_elem.GetProp( "ShardSeason" );
        if( season.errortext )
                SysLog( "Error reading datafile <ShardSeason> -->"+season.errortext );
                return 0;
        endif

	return season;
endfunction

function FindGameTimeSeason()

        var time := SplitWords( StrFormatTime( "%m %d", CInt( POLCore().systime )));
        
        var month := CInt( time[1] ),
            day := CInt( time[2] ),
            season;
            
        if(( month == 12 && day >= 21 ) || month < 3 || ( month == 3 && day <= 20 ))
                //winter december 21 to march 20
                season := SEASON_FALL;
        elseif(( month == 3 && day >= 21 ) || ( month > 3 && month < 6 ) || ( month == 6 && day <= 20 ))
                //spring march 21 to juin 20
                season := SEASON_SUMMER;
        elseif(( month == 6 && day >= 21 ) || ( month > 6 && month < 9 ) || ( month == 9 && day <= 20 ))
                //summer juin 21 to september 20
                season := SEASON_SPRING;
        elseif(( month == 9 && day >= 21 ) || ( month > 9 && month < 12 ) || ( month == 12 && day <= 20 ))
                //fall september 21 to december 20
                season := SEASON_FALL;
        endif

	return season;
endfunction
