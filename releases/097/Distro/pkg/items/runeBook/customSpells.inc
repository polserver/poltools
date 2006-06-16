use uo;
use os;

function runebookRecall(who, parms, book, charges, use_charge:=0)
	var runebook_info := array{ book, parms, use_charge };
	var script := Start_Script(":magery:spellStarter", {who, 32, 1, runebook_info});
	if ( script.errortext )
		SendSysMessage(who, "Error - Could not start spell starter ->"+script.errortext);
		return 0;
	endif
	if( use_charge )
		charges := charges - 1;
		SetCharges(book, who, charges);
	endif

	return 1;
endfunction


function runebookGate(who, parms, book)
	var runebook_info := array{ book, parms };
	var script := Start_Script(":magery:spellStarter", {who, 52, 1, runebook_info});
	if ( script.errortext )
		SendSysMessage(who, "Error - Could not start spell starter ->"+script.errortext);
		return 0;
	endif

	return 1;
endfunction