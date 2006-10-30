/* $Id$
 *	  Author: MontuZ
 *	  Created: Wednesday, May 24 2006
 *	  Version: 0.02
 */

use uo;

include	":guilds:settings";

function GetGuildRank(who)

	Return CInt(GetObjProperty(who, "Guild_Rank"));

endfunction

function SetGuildRank(who, rank)

	rank := CInt( rank );
	if ( !rank || rank.errortext )
		return rank;
	endif

	var max_ranks := CInt(Guild_GetSettingsCfgElem("Ranks").MaxRanks);
	if ( !max_ranks || max_ranks.errortext )
		return max_ranks;
	endif

	var min_ranks := CInt(Guild_GetSettingsCfgElem("Ranks").MinRanks);
	if ( !min_ranks || min_ranks.errortext )
		return min_ranks;
	endif

	if ( rank > max_ranks || rank < min_ranks )
		rank := error{"errortext" := "Rank is out of range."};
		return rank;
	endif

	if( SetObjProperty(who, "Guild_Rank", rank ) )
		return 1;
	else
		return 0;
	endif

endfunction

function DemoteMember(who)

	var rank := GetGuildRank(who);
	if ( !rank || rank.errortext )
		return rank;
	endif

	var max_ranks := CInt(Guild_GetSettingsCfgElem("Ranks").MaxRanks);
	if ( !max_ranks || max_ranks.errortext )
		return max_ranks;
	endif

	var min_ranks := CInt(Guild_GetSettingsCfgElem("Ranks").MinRanks);
	if ( !min_ranks || min_ranks.errortext )
		return min_ranks;
	endif

	if ( rank <= min_ranks )
		rank := error{"errortext" := "User cannot be demoted anymore."};
		return rank;
	endif

	rank := rank - 1;

	if( SetObjProperty(who, "Guild_Rank", rank ) )
		return 1;
	else
		return 0;
	endif

endfunction

function PromoteMember(who)

	var rank := GetGuildRank(who);
	if ( !rank || rank.errortext )
		return rank;
	endif

	var max_ranks := CInt(Guild_GetSettingsCfgElem("Ranks").MaxRanks);
	if ( !max_ranks || max_ranks.errortext )
		return max_ranks;
	endif

	var min_ranks := CInt(Guild_GetSettingsCfgElem("Ranks").MinRanks);
	if ( !min_ranks || min_ranks.errortext )
		return min_ranks;
	endif

	if ( rank >= max_ranks )
		rank := error{"errortext" := "User cannot be promoted anymore."};
		return rank;
	endif

	rank := rank + 1;

	if( SetObjProperty(who, "Guild_Rank", rank ) )
		return 1;
	else
		return 0;
	endif

endfunction

function GetRankNameByID(rank)

	var rank_name;

	case ( rank )
	1:
		rank_name := Guild_GetSettingsCfgElem("Ranks").Rank1;
		return rank_name;
	2:
		rank_name := Guild_GetSettingsCfgElem("Ranks").Rank2;
		return rank_name;
	3:
		rank_name := Guild_GetSettingsCfgElem("Ranks").Rank3;
		return rank_name;
	4:
		rank_name := Guild_GetSettingsCfgElem("Ranks").Rank4;
		return rank_name;
	5:
		rank_name := Guild_GetSettingsCfgElem("Ranks").Rank5;
		return rank_name;
	default:
		rank_name := "";
		return rank_name;
	endcase

endfunction