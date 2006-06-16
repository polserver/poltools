use uo;
use os;

//include ":bountyboard:bounty";
include ":gumps:include/gumps";
include ":datafile:datafile";

const LONG_COUNT_DECAY_TIME := 144000; //40 hours
const SHORT_COUNT_DECAY_TIME := 28800; //8 hours

//var reportables;
//var reportdata := array{};

function SendReportGump(who)
	var reportables := who.reportables;
	var report_data := array{};

	var gump := GFCreateGump();
	GFPage(gump, 0);
	GFDisposable(gump, 0);
	GFClosable(gump, 0);
	GFMovable(gump, 0);
	GFResizePic(gump, 0, 0, 2600, 480, 480);
	GFAddButton(gump, 380, 345, 249, 248, 1, 99);
	GFTextLine(gump, 50, 20, 0, "Now is the time for retribution!");
	GFTextLine(gump, 50, 50, 0, "Check zero or more players to report for your murder");
	GFTextLine(gump, 50, 70, 0, "and press OKAY");

	FillInArrays(reportables, report_data, gump);

	SetObjProperty(who, "ReportMenu", report_data);

	var ret := GFSendGump(who, gump);
	if( ret == 0 )
		return;
	endif

	EraseObjProperty(who, "ReportMenu");
	EraseObjProperty(who, "LoginReport");

	var holder := array{};
	foreach id in (ret.keys)
		if( id in report_data )
			var mob := SystemFindObjectBySerial(id, SYSFIND_SEARCH_OFFLINE_MOBILES);
			if( !mob.ip )
				SetObjProperty(mob.killer, "LoginNotify", who.serial);
			else
				AdjustNoto(mob, who);
			endif

			holder.append(id);
			AdjustMurderCounts(mob.killer);
		endif
		sleepms(2);
	endforeach

	foreach reportable in (who.reportables)
		who.removeReportable(reportable.serial, reportable.gameclock);
		sleepms(2);
	endforeach

	var parms := array{who, holder};
	var script := start_script(":bountyboard:bounty", parms);
	if ( script.errortext )
		SysLog("ERROR SendReportGump():: Unable to start :bountyboard:bounty ->"+script.errortext);
		return error;
	endif

	return 1;
endfunction

function FillInArrays(reportables, byref report_data, byref gump)
	var prev_y := 80;
	var buttonret := 10;
	GFPage(gump, 1);
	foreach reportable in reportables
		if( !(reportable.killer).cmdlevel )
			if( buttonret == 21 )
				prev_y := 80;
			endif
			report_data.append((reportable.killer).serial);
			GFCheckBox(gump, 40, prev_y+20, 210, 211, 0, (reportable.killer).serial);
			GFTextLine(gump, 60, prev_y+20, 0, (reportable.killer).name);
			prev_y := prev_y+20;
			buttonret := buttonret+1;
		endif

		sleepms(2);
	endforeach

	return 1;
endfunction

function AdjustMurderCounts(killer)
	var onlinetime := GetObjProperty(killer, "onlinetimer");
	var longcount := GetObjProperty(killer, "longmurders");

	if( !longcount )
		longcount := 0;
	endif

	longcount := longcount + 1;
	SetObjProperty(killer, "longmurders", longcount);

	if( longcount > 4 )
		killer.setMurderer(1);
		SendSysMessage(killer, "You are now known as a murderer!");
	endif

	SetObjProperty(killer, "decaylongcountat", (onlinetime+LONG_COUNT_DECAY_TIME));

	var shortcount := CInt(GetObjProperty(killer, "shortmurders"));
	shortcount := shortcount + 1;
	SetObjProperty(killer, "shortmurders", shortcount);
	SetObjProperty(killer, "decayshortcountat", (onlinetime+SHORT_COUNT_DECAY_TIME));
endfunction
