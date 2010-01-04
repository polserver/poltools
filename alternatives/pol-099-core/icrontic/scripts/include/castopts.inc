use uo;

var i, rtn;

function IsNoMana(options)

	rtn := 0;
//	options := splitwords(options);
	Print(options);
	foreach i in options
		if (i = "NOMANA")
			rtn := 1;
		endif
	endforeach

	return rtn;

endfunction

function IsNoRegs(options)

	rtn := 0;
//	options := splitwords(options);
	foreach i in options
		if (i = "NOREGS")
			rtn := 1;
		endif
	endforeach

	return rtn;

endfunction

function IsNoDelay(options)

	rtn := 0;
//	options := splitwords(options);
	foreach i in options
		if (i = "NODELAY")
			rtn := 1;
		endif
	endforeach

	return rtn;

endfunction

function IsNoSkillCheck(options)

	rtn := 0;
//	options := splitwords(options);
	foreach i in options
		if (i = "NOSKILL")
			rtn := 1;
		endif
	endforeach

	return rtn;

endfunction

function IsNoWords(options)

	rtn := 0;
//	options := splitwords(options);
	foreach i in options
		if (i = "NOWORDS")
			rtn := 1;
		endif
	endforeach

	return rtn;

endfunction